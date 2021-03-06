#include "StdAfx.h"
//#include "ExtString.h"
//#include "LexemeHasher.h"
#include "ZalEntryParser.h"

using namespace Hlib;

bool operator== (const StHeadword& lhs, const StHeadword& rhs)
{
    if (lhs.sRawSource == rhs.sRawSource)
    {
        return true;
    }

    return false;
}

bool operator!= (const StHeadword& lhs, const StHeadword& rhs)
{
    return !(lhs == rhs);
}

bool operator < (const StHeadword& lhs, const StHeadword& rhs)
{
    return lhs.sRawSource < rhs.sRawSource;
}


void CZalEntryParser::Init()
{
    CEString arrMainSymbol[] = { L"м", L"мо", L"ж", L"жо", L"с", L"со", L"мо-жо", L"мн.",
     L"мн. неод.", L"мн. одуш.", L"мн. _от_", L"п", L"мс", L"мс-п", L"мс-предик.", L"числ.", L"числ.-п", 
     L"св", L"нсв", L"св-нсв", L"н", L"предл.", L"союз", L"предик.", L"вводн.", L"сравн.", 
     L"част.", L"межд.", L"NULL"  };

    for (ET_MainSymbol eMs = MS_START; eMs < MS_END; ++eMs)
    {
        m_mapMainSymbol[arrMainSymbol[eMs]] = eMs; 
    }

    for (int iMs = 0; iMs < (int)MS_END; ++iMs)
    {
        CEString sMs = arrMainSymbol[iMs];
        ET_MainSymbol eMs = m_mapMainSymbol[sMs];
        switch (eMs)
        {
            case MS_M:
            case MS_MO:
            case MS_ZH:
            case MS_ZHO:
            case MS_S:
            case MS_SO:
            case MS_MO_ZHO:
            case MS_MN:
            case MS_MN_NEOD:
            case MS_MN_ODUSH:
            case MS_MN_OT:
            {
                m_mapPos[sMs] = POS_NOUN;
                break;
            }

            case MS_SV:
            case MS_NSV:
            case MS_SV_NSV:
            {
                m_mapPos[sMs] = POS_VERB;
                break;
            }

            case MS_P:
            {
                m_mapPos[sMs] = POS_ADJ;
                break;
            }

            case MS_MS:
            {
                m_mapPos[sMs] = POS_PRONOUN;
                break;
            }

            case MS_MS_P:
            {
                m_mapPos[sMs] = POS_PRONOUN_ADJ;
                break;
            }

            case MS_MS_PREDIK:
            {
                m_mapPos[sMs] = POS_PRONOUN_PREDIC;
                break;
            }

            case MS_N:
            {
                m_mapPos[sMs] = POS_ADV;
                break;
            }

            case MS_CHISL:
            {
                m_mapPos[sMs] = POS_NUM;
                break;
            }

            case MS_CHISL_P:
            {
                m_mapPos[sMs] = POS_NUM_ADJ;
                break;
            }

            case MS_CHAST:
            {
                m_mapPos[sMs] = POS_PARTICLE;
                break;
            }

            case MS_PREDL:
            {
                m_mapPos[sMs] = POS_PREP;
                break;
            }

            case MS_SOJUZ:
            {
                m_mapPos[sMs] = POS_CONJ;
                break;
            }

            case MS_VVODN:
            {
                m_mapPos[sMs] = POS_PARENTH;
                break;
            }

            case MS_PREDIK:
            {
                m_mapPos[sMs] = POS_PREDIC;
                break;
            }

            case MS_MEZHD:
            {
                m_mapPos[sMs] = POS_INTERJ;
                break;
            }

            case MS_SRAVN:
            {
                m_mapPos[sMs] = POS_COMPAR;
                break;
            }

            case MS_NULL:
            {
                m_mapPos[sMs] = POS_NULL;
                break;
            }

            default:
            {
                ATLASSERT(0);
                ERROR_LOG (L"Unable to initialize POS map.");
                return;
            }

        }    // switch

    }   //  for (int iMs = 0; ... )

    CEString arrAccent[] = { L"а", L"а'", L"в", L"в'", L"с", L"с'", L"с''", L"D", L"D'", L"е", L"F", L"F'", L"F''" };

    for (ET_AccentType eAt = AT_A; eAt < AT_COUNT; ++eAt)
    {
        m_mapAccentType[arrAccent[eAt-1]] = eAt;
    }

    m_bCommaAfterMainSymbol = false;

}   //  Init()

void CZalEntryParser::SetOriginationRawHeadword (const CEString& sSourceHead)
{
    m_pstHead->sOrigination = sSourceHead;
}

bool CZalEntryParser::bParse (const CEString& sHead, const CEString sUsageComment)
{
    CEString arrMainSymbol[] = { L"м", L"мо", L"ж", L"жо", L"с", L"со", L"мо-жо", L"мн.",
     L"мн. неод.", L"мн. одуш.", L"мн. _от_", L"п", L"мс", L"мс-п", L"мс-предик.", L"числ.", L"числ.-п", 
     L"св", L"нсв", L"св-нсв", L"н", L"предл.", L"союз", L"предик.", L"вводн.", L"сравн.", 
     L"част.", L"межд.", L"NULL"  };

    m_pstHead->sUsage = sUsageComment;

    CEString sEntry (m_sSource);

    if (!sHead.bIsEmpty())
    {
        ATLASSERT (m_bSecondPass);
        m_pstHead->sRawSource = sHead;
        bool b_ = bExtractStress (const_cast<CEString&>(sHead));
        if (!b_)
        {
            return false;
        }
    }
    else
    {
        bool b_ = bExtractHead (sEntry);
        if (!b_)
        {
            return false;
        }
    }

    // First, we split the entry into parts divided by semicolons.
    //
    // We should omit semicolons in parentheses or curly brackets.
    // Here, we take into account that parentheses can occur inside
    // curly brackets, whereas the converse doesn't hold.

    vector<CEString> vecStrParts;
    CEString sLeft;
    const CEString sSemicolonMatcher 
        (L"^(([^\\{]*?\\{[^\\}]*?\\})*?([^\\{\\(]*\\([^\\)]*?\\))*?[^\\{\\(;]*);(\\s*.+)");

    bool bSemicolon = sEntry.bRegexMatch (sSemicolonMatcher);
    if (!bSemicolon)
    {
        vecStrParts.push_back (sEntry);
        sEntry.Erase();
    }

    while (bSemicolon)
    {
        sLeft = sEntry.sGetRegexMatch (0);
        sEntry = sEntry.sGetRegexMatch (3);
        bool bGoodSemicolon = false;
        bool bMatch = sEntry.bRegexMatch (L"^\\s*([^\\s;:,\\/]*).*");
        if (!bMatch)
        {
            ATLASSERT(0);
            ERROR_LOG (L"No word after semicolon: " + sEntry);
            return false;
        }

        CEString sFirstWord = sEntry.sGetRegexMatch (0);
        for (int iMs = 0; iMs < (int)MS_END; ++iMs)
        {
            if (sFirstWord == arrMainSymbol[iMs])
            {
                bGoodSemicolon = true;
                break;
            }
        }

        if (sFirstWord.bRegexMatch (L"^\\s*\\d"))
        {
            bGoodSemicolon = true;
        }
        while (!bGoodSemicolon)
        {
            // Then it's a semicolon with some other meaning,
            // so we have to continue our search
            bSemicolon = sEntry.bRegexMatch (sSemicolonMatcher);
            if (!bSemicolon)
            {
                // we reached the end of the string and haven't found a good semicolon
                sLeft += L";" + sEntry;
                sEntry.Erase();
                break;
            }

            CEString sMiddle = sEntry.sGetRegexMatch (0);
            sEntry = sEntry.sGetRegexMatch (3);
            sLeft += L";" + sMiddle;
            bMatch = sEntry.bRegexMatch (L"^\\s*([^\\s;:,\\/]*).*");
            if (!bMatch)
            {
                ERROR_LOG(L"No word after semicolon: " + sEntry);
                return false;
            }

            sFirstWord = sEntry.sGetRegexMatch (0);
            for (int iMs = 0; iMs < (int)MS_END; ++iMs)
            {
                if (arrMainSymbol[iMs] == sFirstWord)
                {
                    bGoodSemicolon = true;
                    break;
                }
            }

            if (sFirstWord.bRegexMatch (L"^\\s*\\d.*"))
            {
                bGoodSemicolon = true;
            }
        }
        // Here, we have yet another portion of the entry (sLeft)
        // and the rest of it (sEntry).
        vecStrParts.push_back (sLeft);
        if (sEntry.uiLength() <= 0)
        {
            break;
        }

        bSemicolon = sEntry.bRegexMatch (sSemicolonMatcher);
    }

    if (sEntry.uiLength() > 0)
    {
        vecStrParts.push_back (sEntry);
    }


    // Now, we can handle each part separately
    for (vector<CEString>::iterator itParts = vecStrParts.begin();
         itParts != vecStrParts.end();
         ++itParts)
    {
        sEntry = *itParts;
        sEntry.SetBreakChars (L" ");
        sEntry.Trim();
        if (L';' == sEntry[0])
        {
            ATLASSERT(0);
            CEString sMsg (L"Unexpected \';\' at entry start: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }

        bool b_ = bCheckMnOt (sEntry);
        if (!b_)
        {
            return false;
        }
        
        b_ = bGetMainSymbol (sEntry, false);
        if (!b_)
        {
            return false;
        }

        b_ = bCheckInflectionType (sEntry);
        if (!b_)
        {
            return false;
        }

        b_ = bParseInflectionGroup (sEntry);
        if (!b_)
        {
            return false;
        }

        if (sEntry.uiFind (L"//") == 0)
        {
            sEntry = sEntry.sSubstr (2);
            b_ = bParseInflectionGroup (sEntry);
            if (!b_)
            {
                return false;
            }
        }
        
        sEntry.ResetSeparators();
        sEntry.SetBreakChars (L", ");
        sEntry.Trim();
        sEntry.ResetSeparators();
        bool bMatch = sEntry.bRegexMatch (L"^(?:\\(\\-([щсдтбгкнми]+)\\-\\))(.*)");
        if (bMatch)
        {
            m_stEntry.sVerbAlternation = sEntry.sGetRegexMatch (0);
            sEntry = sEntry.sGetRegexMatch (1);
        }
        else
        {
            bMatch = 
                sEntry.bRegexMatch (L"^(?:\\(([абвгдеёжзийклмнопрстуфхцчшщъыьэюя]+)\\-\\))(.*)");
            if (bMatch)
            {
                if ((L"св" != m_stEntry.sMainSymbol &&
                     L"нсв" != m_stEntry.sAltMainSymbol &&
                     L"св-нсв" != m_stEntry.sMainSymbol))
                {
                    ATLASSERT(0);
                    CEString sMsg (L"This type of stem alternation is expected for verbs only: ");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                }
                if (14 != m_stEntry.stInflectionGroup.iType)
                {
                    ATLASSERT(0);
                    CEString sMsg (L"This type of verb stem alternation is expected for type 14 only.");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                }
                m_stEntry.sVerbAlternation = sEntry.sGetRegexMatch (0);
                sEntry = sEntry.sGetRegexMatch (1);
            }
            else
            {
                m_stEntry.bPastPartPassZhD = sEntry.bRegexMatch(L"(\\s*[,;]?\\s*_?прич\\. страд\\._?\\s*-жд-)\\s*(.*)");
                if (m_stEntry.bPastPartPassZhD)
                {
                    sEntry = sEntry.sGetRegexMatch(1);
                }
            }
        }

        // Here we have to repeat common deviations check since sometimes
        // a circled digit may appear after stem alternation 
        b_ = bCheckCircledDigits (sEntry, m_stEntry.stInflectionGroup);
        if (!b_)
        {
            return false;
        }

        b_ = bCheckTilde (sEntry);
        if (!b_)
        {
            return false;
        }

        //if (L'-' == sEntry[0])
        //{
        //    m_stEntry.b_AssumedForms = true;
        //    CEString sDescriptor (sEntry.sSubstr (1), L" ");
        //    sDescriptor.Trim();
        //    sEntry = sDescriptor.c_str();
        //}

        if ((sEntry.uiLength() > 0) && (L'-' == sEntry[0]))
        {
            m_stEntry.bAssumedForms = true;
            sEntry.sErase (0, 1);
            sEntry.SetBreakChars (L" ");
            sEntry.Trim();
        }

        b_ = bCheckCognate (sEntry);
        if (!b_)
        {
            return false;
        }

        b_ = bCheckInflectedParts (sEntry);
        if (!b_)
        {
            return false;
        }

        b_ = bCheckVerbType (sEntry);
        if (!b_)
        {
            return false;
        }

        b_ = bCheckSharp (sEntry);
        if (!b_)
        {
            return false;
        }

        b_ = bCheckColon (sEntry);
        if (!b_)
        {
            return false;
        }

        b_ = bCheckPercent (sEntry);
        if (!b_)
        {
            return false;
        }

        b_ = bCheckDollar (sEntry);
        if (!b_)
        {
            return false;
        }

        b_ = bCheckTriangle (sEntry);
        if (!b_)
        {
            return false;
        }
        if (!m_stEntry.sIrregularForms.bIsEmpty() ||
            !m_stEntry.sAltIrregularForms.bIsEmpty())
        {
            m_stEntry.bHasIrregularForms = true;
        }
        
        b_ = bCheckDifficulties (sEntry);           // those outside the triangle group
        if (!b_)
        {
            return false;
        }
        if (!m_stEntry.sDifficulties.bIsEmpty() ||
            !m_stEntry.sObsolete.bIsEmpty() ||
            !m_stEntry.sColloquial.bIsEmpty())
        {
            m_stEntry.bHasDifficulties = true;
        }
        if (!m_stEntry.sDeficient.bIsEmpty())
        {
            m_stEntry.bHasDeficiencies = true;
            m_stEntry.bNoLongForms = false;
            for (int iAt = 0; iAt < (int)m_stEntry.sDeficient.uiNFields(); ++iAt)
            {
                if (L"пф" == m_stEntry.sDeficient.sGetField (iAt))
                {
                    m_stEntry.bNoLongForms = true;
                    break;
                }
            }
        }

        b_ = bCheckYo (sEntry);
        if (!b_)
        {
            return false;
        }

        b_ = bCheckSecondCases (sEntry);
        if (!b_)
        {
            return false;
        }

        /*
        b_ = b_CheckComma (sEntry, m_stEntry);
        if (!b_)
        {
            return false;
        }
        */

        b_ = bCheckSquareBrackets (sEntry);
        if (!b_)
        {
            return false;
        }

/*
        if (!m_stHead.str_SeeAlso.bIsEmpty())
        {
//&&&&
            CEString sMsg (L"WARNING: ignoring ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return true;
        }
*/

        sEntry.Trim();

        b_ = sEntry.bRegexMatch (L"\\(_(.+?)_\\)(.*)");
        if (b_)
        {
            ATLASSERT (sEntry.uiGetNumOfRegexMatches() > 0);
            m_stEntry.sTrailingComment = sEntry.sGetRegexMatch (0);
            if (sEntry.uiGetNumOfRegexMatches() > 1)
            {
                sEntry = sEntry.sGetRegexMatch (1);
            }
            else
            {
                sEntry.Erase();
            }
        }

        b_ = bMakeGraphicStem();
//        m_vecDescriptors.push_back (m_stEntry);
        m_mmapDictionary.insert(pair<StHeadword *, StDescriptor>(m_pstHead, m_stEntry));

        if (sEntry.uiLength() > 0)
        {
            ERROR_LOG(L"WARNING: Something still unparsed in the end: " + sEntry);
//            return false;
        }
    }   //  for (vector<wstring>::iterator iter_parts ...

    return true;

}   //  iParse (...)

bool CZalEntryParser::bExtractHead (CEString& sSource)
{
    sSource.ResetSeparators();
    sSource.SetBreakChars (L" ");
    if (sSource.uiNFields() < 1)
    {
        ATLASSERT(0);
        CEString sMsg (L"Too few fields: ");
        sMsg += m_sSource;
        ERROR_LOG (sMsg);
        return false;
    }

    CEString sHead (sSource.sGetField (0));
    bool bMatch = sHead.bRegexMatch (L"^(?:([1-5])?(?:,([1-5]))*/)?([^ ]+)");

    CEString sDigit;
    for (int i_ = 0; i_ < (int)sHead.uiNRegexMatches() - 1; ++i_)
    {
        sDigit = sHead.sGetRegexMatch (i_);
        if (sDigit.uiLength() != 1)
        {
//            ATLASSERT(0);
//            CEString sMsg (L"Bad homonym number: ");
//            sMsg += m_sSource;
//            ERROR_LOG (sMsg);
//            return false;
            continue;
        }
        if (!iswdigit (sDigit[0]))
        {
            ATLASSERT(0);
            CEString sMsg (L"Homonym number is not numeric: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
        m_pstHead->vecIHomonyms.push_back (_wtoi (&sDigit[0]));
    }

    if (sHead.uiNRegexMatches() < 1)
    {
        ATLASSERT(0);
        CEString sMsg (L"No headword: ");
        sMsg += m_sSource;
        ERROR_LOG (sMsg);
        return false;
    }

    CEString sHeadword (sHead.sGetRegexMatch (sHead.uiNRegexMatches()-1));
    m_pstHead->sRawSource = sHeadword;        // slice

    bool bRet = bExtractStress (sHeadword);

    if (sSource.uiNFields() < 2)
    {
        return true;
    }

    sSource = sSource.sSubstr (sSource.uiGetFieldOffset (1));
    bMatch = sSource.bRegexMatch (L"^(?:\\((.+?)\\)\\s)(.+)");
    if (bMatch)
    {
        if (sSource.uiNRegexMatches() != 2)
        {
            ATLASSERT(0);
            CEString sMsg (L"Error matching parenthetical expression: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
        m_pstHead->sComment = sSource.sGetRegexMatch (0);
        sSource = sSource.sGetRegexMatch (1);
    }

    return true;

}   //  bExtractHead()

bool CZalEntryParser::bExtractStress (CEString& sHeadWord)
{
    sHeadWord.SetBreakChars (L"<>");

    for (int iTextField = 0; iTextField < (int)sHeadWord.uiGetNumOfFields(); ++iTextField)
    {
         m_pstHead->sSource += sHeadWord.sGetField (iTextField);
    }
    CEString sCopy(m_pstHead->sSource);

    int iMarksFound = 0;
    for (int iBreakField = 0; 
         iBreakField < (int)sHeadWord.uiGetNumOfFields (ecTokenBreakChars); 
         ++iBreakField)
    {
        CEString sStressMark = sHeadWord.sGetField (iBreakField, ecTokenBreakChars);
        if (sStressMark.uiLength() != 1)
        {
            ATLASSERT(0);
            CEString sMsg (L"Error processing stress marks: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }

        int iPos = sHeadWord.uiGetFieldOffset (iBreakField, ecTokenBreakChars);
        iPos -= iMarksFound;
        ++iMarksFound;

        if (L"<" == sStressMark)
        {
            m_pstHead->mapStress[iPos] = STRESS_PRIMARY;
        }
        else if (L">" == sStressMark)
        {
            m_pstHead->mapStress[iPos] = STRESS_SECONDARY;
        }
        else
        {
            ATLASSERT(0);
            CEString sMsg (L"Error extracting stress marks: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }

    }   //  for (int iBreakField = 0; ...)

    if (m_pstHead->mapStress.empty())
    {
        sCopy.SetVowels (g_szRusVowels);
        int iYoPos = sCopy.uiFind (L"ё");
        if (ecNotFound != iYoPos)
        {
            m_pstHead->mapStress[iYoPos] = STRESS_PRIMARY;
        }
        else
        {
            if (sCopy.uiGetNumOfSyllables() != 1)
            {
                ATLASSERT(0);
                ERROR_LOG (L"No stress position given for a polysyllabic irregular form.");
                return false;
            }
            int iStressPos = sCopy.uiFindFirstOf (g_szRusVowels);
            if (ecNotFound == iStressPos)
            {
                ATLASSERT(0);
                ERROR_LOG (L"Unable to find vowel.");
                return false;
            }
            m_pstHead->mapStress[iStressPos] = STRESS_PRIMARY;
        }
    }

    return true;

//    if (sSource.uiNFields() < 2)
//    {
//        ATLASSERT(0);
//        CEString sMsg (L"Expected field is not found: ");
//        sMsg += m_sSource;
//        ERROR_LOG (sMsg);
//        return false;
//    }

}   //  bExtractStress (...)

bool CZalEntryParser::bCheckMnOt (CEString& sSource)
{
    m_stEntry.bPluralOf = false;

    sSource.SetBreakChars (L" /,;:");
    if (sSource.bIsEmpty())
    {
        ATLASSERT(0);
        CEString sMsg (L"No main symbol: ");
        sMsg += sSource;
        ERROR_LOG (sMsg);
        return false;
    }

    CEString sMain (sSource.sGetField (0));
    if (m_mapPos.find (sMain) == m_mapPos.end())
    {
        ATLASSERT(0);
        CEString sMsg (L"Unknown main symbol: ");
        sMsg += m_sSource;
        ERROR_LOG (sMsg);
        return false;
    }

    if (L"мн." != sMain)
    {
        return true;
    }

    if (sSource.uiNFields() < 2)
    {
        ATLASSERT(0);
        CEString sMsg (L"Unterminated \"мн.\": ");
        sMsg += m_sSource;
        ERROR_LOG (sMsg);
        return false;
    }

    CEString sNextToken (sSource.sGetField (1));        
    if (sSource.uiFind (L"_от_") != sSource.uiGetFieldOffset (1))
    {
        return true;    // nothing to do
    }

    if (sSource.uiNFields() < 3)
    {
        ATLASSERT(0);
        CEString sMsg (L"Unterminated \"мн. _от_\": ");
        sMsg += m_sSource;
        ERROR_LOG (sMsg);
        return false;
    }

    m_stEntry.bPluralOf = true;

    if (m_mapPos.find (sSource.sGetField(2)) != m_mapPos.end())
    {
        int iOffset = sSource.uiGetFieldOffset(1) + sSource.uiGetFieldLength(1);
        sSource = sSource.sSubstr (iOffset);

        return true;
    }

    m_pstHead->sPluralOf = sSource.sGetField(2);
    if (sSource.uiNFields() < 4)
    {
        ATLASSERT(0);
        CEString sMsg (L"Unterminated \"мн. _от_ <headword>\": ");
        sMsg += m_sSource;
        ERROR_LOG (sMsg);
        return false;
    }

    int iOffset = sSource.uiGetFieldOffset (2) + sSource.uiGetFieldLength (2);
    sSource = sSource.sSubstr (iOffset);
    
    return true;

}   //  b_CheckMnOt (...)

bool CZalEntryParser::bGetMainSymbol (CEString& sDescriptor, bool bInflectionTypeMismatch)
{
    sDescriptor.SetBreakChars (L" ;:");
    
    int iNextField = 1;
    CEString sMainSymb (sDescriptor.sGetField (0));
    sMainSymb.SetBreakChars (L"/,;");
    if (L"мн." == sMainSymb)
    {
        if (sDescriptor.uiNFields() > 1)
        {
            CEString sNext (sDescriptor.sGetField (1));
            sNext.SetBreakChars (L"/,;");
            sNext.Trim();
            if ((L"неод." == sNext) || (L"одуш." == sNext))
            {
                sMainSymb += L" " + sNext;
                ++iNextField;
            }
        }
    }

    if ((L"св" == sMainSymb) || (L"нсв" == sMainSymb) || (L"св-нсв" == sMainSymb))
    {
        if (sDescriptor.uiNFields() > 1)
        {
            CEString sNext (sDescriptor.sGetField (1));
            sNext.SetBreakChars (L"/,;");
            sNext.Trim();
            if (L"нп" == sNext)
            {
                m_stEntry.bIntransitive = true;
                ++iNextField;
            }
        }
    }

    if (sDescriptor.uiNFields() < (unsigned int)iNextField + 1)
    {
        sDescriptor.Erase();
    }
    else
    {
        int iOffset = sDescriptor.uiGetFieldOffset (iNextField - 1) + 
                       sDescriptor.uiGetFieldLength (iNextField - 1);
        sDescriptor = sDescriptor.sSubstr (iOffset);
    }

    CEString& rsInflection = bInflectionTypeMismatch 
        ? m_stEntry.sInflectionSymbol
        : m_stEntry.sMainSymbol;

    rsInflection = sMainSymb.sGetField (0);
    if (m_mapPos.find (rsInflection) == m_mapPos.end())
    {
        ATLASSERT(0);
        CEString sMsg (L"Unidentified main symbol: ");
        sMsg += m_sSource;
        ERROR_LOG (sMsg);
        return false;
    }

    if (sMainSymb.uiNFields() > 1)
    {
        CEString sAlternate (sMainSymb.sGetField (1));
        bool bMatch = sAlternate.bRegexMatch (L"_(.+?)_.*");
        if (bMatch)
        {
            m_stEntry.sAltMainSymbolComment = sAlternate.sGetRegexMatch (0);
            int iOffset = sAlternate.uiGetFieldOffset (0, ecTokenRegexMatch);
            int iLength = sAlternate.uiGetFieldLength (0, ecTokenRegexMatch);
            CEString sLeft = sAlternate.sSubstr (0, iOffset);
            CEString sRight = sAlternate.sSubstr (iOffset + iLength);
            sAlternate = sLeft + sRight;

            sAlternate.SetBreakChars (L" ");
            sAlternate.Trim();
        }

        m_stEntry.sAltMainSymbol = sAlternate;
        if (m_mapPos.find (m_stEntry.sAltMainSymbol) == m_mapPos.end())
        {
            ATLASSERT(0);
            CEString sMsg (L"Unidentified alternate main symbol: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
    }

    m_stEntry.ePartOfSpeech = m_mapPos[rsInflection];    // and alt. main symbol?
    m_stEntry.eInflectionSymbol = m_mapMainSymbol[rsInflection];

    if (!bInflectionTypeMismatch)
    {
        m_stEntry.sInflectionSymbol = m_stEntry.sMainSymbol;
    }

    return true;

}   //  bGetMainSymbol (...)

bool CZalEntryParser::bCheckInflectionType (CEString& sSource)
{
    sSource.SetBreakChars (L" ");
    sSource.Trim();

    // Extract inflection type (if different from main symbol, 
    // e.g., б<абий п <мс 6*а>)
    bool bMatch = sSource.bRegexMatch (L"(<(.+?)>).*");
    if (!bMatch)
    {
        return true;
    }

    CEString sDescriptor (sSource.sGetRegexMatch (1));
    int iRightOffset = sSource.uiGetFieldOffset (0, ecTokenRegexMatch) +
                       sSource.uiGetFieldLength (0, ecTokenRegexMatch);
    CEString sRight = sSource.sSubstr (iRightOffset);
    sSource = sDescriptor + sRight;

    bool bRet = bGetMainSymbol (sSource, true);

    return bRet;

}   //  bCheckInflectionType (...)

bool CZalEntryParser::bParseInflectionGroup (CEString& sDescriptor)
{
    sDescriptor.SetBreakChars (L" ");
    sDescriptor.Trim();

    StInflectionGroup& stInflectionGroup = m_stEntry.stInflectionGroup.bHasData 
        ? m_stEntry.stAltInflectionGroup 
        : m_stEntry.stInflectionGroup;

    bool bMatch = sDescriptor.bRegexMatch (L"^(_.+?_\\s*?)?(\\d{1,2}).*");
    if (bMatch)
    {
        stInflectionGroup.bHasData = true;

        int iLength = (sDescriptor.sGetRegexMatch(0)).uiLength();
        if (iLength > 0)
        {
            stInflectionGroup.sComment = sDescriptor.sGetRegexMatch (0);     // rare
        }

        CEString sType = sDescriptor.sGetRegexMatch (1);
        if (sType.uiLength() < 1)
        {
            ATLASSERT(0);
            CEString sMsg (L"Error retrieving inflection type number: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }

        iLength += sType.uiLength();
        stInflectionGroup.iType = _wtoi (sType);
        sDescriptor = sDescriptor.sSubstr (iLength);
    }
    if (sDescriptor.bIsEmpty())
    {
        return true;
    }

    if (L'°' == sDescriptor[0])
    {
        if (POS_NOUN == m_mapPos[m_stEntry.sMainSymbol])
        {
            sDescriptor = sDescriptor.sSubstr (1);
            CEString sHeadWord (m_pstHead->sSource);
            if (1 == stInflectionGroup.iType)
            {
                stInflectionGroup.iStemAugmentType = 1;
            }
            if (3 == stInflectionGroup.iType)
            {
                if (sHeadWord.bEndsWith (L"онок") || sHeadWord.bEndsWith (L"ёнок"))
                {
                    stInflectionGroup.iStemAugmentType = 1;
                }
                else
                {
                    if (sHeadWord.bEndsWith (L"оночек") || sHeadWord.bEndsWith (L"ёночек"))
                    {
                        stInflectionGroup.iStemAugmentType = 2;
                    }
                    else
                    {
                        ATLASSERT(0);
                        CEString sMsg (L"Unexpected source for a 3° noun: ");
                        sMsg += m_sSource;
                        ERROR_LOG (sMsg);
                        return false;
                    }
                }
            }
            if (8 == stInflectionGroup.iType)
            {
                if (m_pstHead->sSource.bEndsWith (L"мя"))
                {
                    stInflectionGroup.iStemAugmentType = 3;
                }
                else
                {
                    ATLASSERT(0);
                    CEString sMsg (L"Unexpected source for an 8° noun: ");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                }
            }
        }
        else if (POS_VERB == m_mapPos[m_stEntry.sMainSymbol])
        {
            sDescriptor = sDescriptor.sSubstr (1);
            stInflectionGroup.iStemAugmentType = 1;
        }
    }
    else
    {
        stInflectionGroup.iStemAugmentType = -1;
    }

    if (sDescriptor.bIsEmpty())
    {
        return true;
    }

    if (L'*' == sDescriptor[0])
    {
        stInflectionGroup.bFleetingVowel = true;
        sDescriptor = sDescriptor.sSubstr (1);
    }
    if (sDescriptor.bIsEmpty())
    {
        return true;
    }

    sDescriptor.ResetSeparators();
    bMatch = sDescriptor.bRegexMatch (L"^([авсDеF]'{0,2}).*");
    if (bMatch)
    {
        CEString sAt1 = sDescriptor.sGetRegexMatch (0);
        if (sAt1.uiLength() < 1)
        {
            ATLASSERT(0);
            CEString sMsg (L"Error retrieving accent type 1: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
        if (m_mapAccentType.find (sAt1) != m_mapAccentType.end())
        {
            stInflectionGroup.eAccentType1 = m_mapAccentType[sAt1];
        }
        else
        {
            ATLASSERT(0);
            CEString sMsg (L"Unrecognized accent type 1: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
        sDescriptor = sDescriptor.sSubstr (sAt1.uiLength());
    }
    if (sDescriptor.bIsEmpty())
    {
        return true;
    }
    
    sDescriptor.ResetSeparators();
    bMatch = sDescriptor.bRegexMatch (L"^/([авсDеF]'{0,2}).*");
    if (bMatch)
    {
        CEString sAt2 = sDescriptor.sGetRegexMatch (0);
        if (sAt2.uiLength() < 1)
        {
            ATLASSERT(0);
            CEString sMsg (L"Error retrieving accent type 2: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
        if (m_mapAccentType.find (sAt2) != m_mapAccentType.end())
        {
            stInflectionGroup.eAccentType2 = m_mapAccentType[sAt2];
        }
        else
        {
            ATLASSERT(0);
            CEString sMsg (L"Unrecognized accent type 2: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
        int iOffset = sDescriptor.uiGetFieldOffset (0, ecTokenRegexMatch) +
                      sDescriptor.uiGetFieldLength (0, ecTokenRegexMatch);
        sDescriptor = sDescriptor.sSubstr (iOffset);
    }

    if (sDescriptor.bIsEmpty())
    {
        return true;
    }

    sDescriptor.ResetSeparators();
    bMatch = sDescriptor.bRegexMatch (L"(.*?)([!\\?])(.*)");
    if (bMatch)
    {    
//        ATLASSERT (sDescriptor.uiGetFieldLength (0, CEString::ec_TokenRegexMatch) == 0);
        CEString sX = sDescriptor.sGetRegexMatch (1);
        if (sX.uiLength() > 0)
        {
            if (L"!" == sX)
            {
                stInflectionGroup.bX = true;
            }
            if (L"?" == sX)
            {
                stInflectionGroup.bBoxedX = true;
            }
            ATLASSERT (stInflectionGroup.bX || stInflectionGroup.bBoxedX);
        }
        sDescriptor = sDescriptor.sGetRegexMatch (0) + sDescriptor.sGetRegexMatch (2);
    }

    bool b_ = bCheckCircledDigits (sDescriptor, stInflectionGroup);
    if (!b_)
    {
        return false;
    }

    return true;

}   //  bParseInflectionGroup (...)

bool CZalEntryParser::bCheckCircledDigits (CEString& sDescriptor, StInflectionGroup& stEntry)
{
    bool bMatch = true;
    sDescriptor.SetBreakChars (L" ");
    sDescriptor.Trim();
    do
    {
        StCommonDeviation stCd;
        bMatch = sDescriptor.bRegexMatch (L"^(\\[?(?:\"(\\d)\")\\]?).*");
        if (bMatch)
        {
            StToken stToken;
//            bool b_ = sDescriptor.bGetField (0, ec_TokenRegexMatch, stToken);
            try
            {
                stToken = sDescriptor.stGetField (0, ecTokenRegexMatch);
            }
            catch (CException ex)
            {
                ATLASSERT(0);
                return false;
            }

            if (L'[' == sDescriptor[stToken.uiOffset])
            {
                if (L']' != sDescriptor[stToken.uiLength - 1])
                {
                    ATLASSERT(0);
                    CEString sMsg (L"No closing bracket: ");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                }
                stCd.bOptional = true;
            }
            else
            {
                stCd.bOptional = false;
            }

            CEString sDigit = sDescriptor.sGetField (1, ecTokenRegexMatch);
            if (!iswdigit (sDigit[0]))
            {
                ATLASSERT(0);
                CEString sMsg (L"Digit expected: ");
                sMsg += m_sSource;
                ERROR_LOG (sMsg);
                return false;
            }

            stCd.iType = _wtoi (&sDigit[0]);
            if (1 > stCd.iType || 9 < stCd.iType)
            {
                ATLASSERT(0);
                CEString sMsg (L"Illegal circled number: ");
                sMsg += m_sSource;
                ERROR_LOG (sMsg);
                return false;
            }

            stEntry.vecCommonDeviations.push_back (stCd);
            sDescriptor = sDescriptor.sSubstr (stToken.uiLength);
        }

    } while (bMatch);

    sDescriptor.SetBreakChars (L" ");
    sDescriptor.Trim();

    return true;

}   //  bCheckCircledDigit (...)

bool CZalEntryParser::bCheckTilde (CEString& sDescriptor)
{
    bool bMatch = sDescriptor.bRegexMatch (L"(.*?)(~)(.*)");
    if (!bMatch)
    {
        return true;
    }

    ATLASSERT (sDescriptor.uiGetFieldLength (0, ecTokenRegexMatch) == 0);
    CEString sTilde = sDescriptor.sGetRegexMatch (1);
    if (sTilde.uiLength() > 0)
    {
        ATLASSERT (L"~" == sTilde);
        m_stEntry.bNoComparative = true;
    }

    sDescriptor = sDescriptor.sGetRegexMatch (2);

    return true;

}   //  bCheckTilde (...)

bool CZalEntryParser::bCheckSquareBrackets (CEString& sDescriptor)
{
// Using regex may be tricky since we need to handle nested brackets
    sDescriptor.Trim();
    // We don't search for ; or [ inside comments
    bool bMatch = sDescriptor.bRegexMatch (L"[^\\[;\\(]*(\\(_.*?_\\)[^\\[;\\(]*)*([;\\[].*)");
    if (!bMatch)
    {
        return true;
    }
    int iStartOffset = sDescriptor.uiGetRegexOffset(1);

    //
    // Don't go beyond first ";"
    //
    if (L';' == sDescriptor[iStartOffset])
    {
        return true;
    }

    if (iStartOffset > 0)
    {
        ATLASSERT (0);
        CEString sMsg (L"Unidentifed sequence before opening bracket: ");
        sMsg += m_sSource;
        ERROR_LOG (sMsg);
//        return false;
    }

    // We don't search for ] inside comments
    bMatch = sDescriptor.bRegexMatch(L"[^\\]\\(]*(\\(_.*?_\\)[^\\]\\(]*)*(\\].*)");
    if (!bMatch)
    {
        ATLASSERT(0);
        CEString sMsg (L"No closing bracket: ");
        sMsg += m_sSource;
        ERROR_LOG (sMsg);
        return false;
    }
    int iEndOffset = sDescriptor.uiGetRegexOffset(1);
    if (iEndOffset <= iStartOffset)
    {
        ATLASSERT(0);
        CEString sMsg (L"The closing bracket precedes the opening one: ");
        sMsg += m_sSource;
        ERROR_LOG (sMsg);
        return false;
    }

    CEString sSource = sDescriptor.sSubstr (iStartOffset + 1, iEndOffset - iStartOffset - 1);
    sDescriptor = sDescriptor.sSubstr (iEndOffset+1);

    //  аб<ак м 3а [//аб<ака]
    //  ак<ант м 1а [//_устар._ ак<анф]
    bMatch = sSource.bRegexMatch 
        (L"^//(_[абвгдеёжзийклмнопрстуфхцчшщъыьэюя\\s\\.\\-]+?_\\s)?([абвгдеёжзийклмнопрстуфхцчшщъыьэюя<>\\-]+)");
    if (bMatch)
    {
        CEString sCaptureGroup = sSource.sGetRegexMatch (0);
        if (sCaptureGroup.bIsEmpty())
        {
            CEString sComment (sCaptureGroup);
            sComment.SetBreakChars (L"_ ");
            sComment.Trim();
            m_pstHead->sVariantComment = sComment;
        }

        m_pstHead->sVariant = sSource.sGetRegexMatch (1);

        return true;
    }

    //  ак<анф м 1а [//ак<ант (_см._)]
    bMatch = sSource.bRegexMatch (L"^//([абвгдеёжзийклмнопрстуфхцчшщъыьэюя<>]+)(?:\\s)(\\(_см._\\))");
    if (bMatch)
    {
        m_pstHead->sSeeRef = sSource.sGetRegexMatch (0);        // 3
        return true;
    }

    //  к<апсель м 2а [_проф._ м 2с"1"]
    //  б<ондарь мо 2а [//бонд<арь мо 2в] 
    //  врожд<ённый п 1*а"1" [//п 1*а/в"2", ё]
    bMatch = sSource.bRegexMatch 
        (L"^(//)?((?:[1-5])?(?:,[1-5])?(?:/))?(_[абвгдеёжзийклмнопрстуфхцчшщъыьэюя\\s\\.\\-]+?_\\s)?([^\\s]+)(?:\\s)?([^\\s+])?(?:\\s)?(.+)?");
    if (!bMatch)
    {
        return true;
    }

    int iOffset = 0;
    CEString sHead;
    CEString sUsageComment;
    bool bMainSymbolFound = false;
    bool bHeadWordFound = false;
    for (int iAt = 0; 
         !bMainSymbolFound && (iAt < (int)sSource.uiGetNumOfRegexMatches()); 
         ++iAt)
    {
        CEString sCaptureGroup = sSource.sGetRegexMatch (iAt);
        if (sCaptureGroup.bIsEmpty())
        {
            continue;
        }
        if (L"//" == sCaptureGroup)
        {
            iOffset = sSource.uiGetFieldOffset (iAt, ecTokenRegexMatch) + 
                      sSource.uiGetFieldLength (iAt, ecTokenRegexMatch);
            continue;
        }
        if (L'_' == sCaptureGroup[0])
        {
            CEString sComment (sCaptureGroup);
            sComment.SetBreakChars (L"_ ");
            sComment.Trim();
            sUsageComment = sComment;
            iOffset = sSource.uiGetFieldOffset (iAt, ecTokenRegexMatch) + 
                      sSource.uiGetFieldLength (iAt, ecTokenRegexMatch);
            continue;
        }

        if (m_mapPos.find (sCaptureGroup) != m_mapPos.end())
        {
            bMainSymbolFound = true;
        }
        else
        {
            if (!sCaptureGroup.bIsEmpty())
            {
                sHead = sCaptureGroup;
                bHeadWordFound = true; // additional tests here?
            }
        }
    }

    if (bMainSymbolFound)
    {
//&&&& вставить ссылку в исходный вариант
        CEString sEntry (sSource.sSubstr (iOffset));
        bool bSecondPass = true;
//        CZalEntryParser Entry (sEntry, m_pDb, bSecondPass);
        CZalEntryParser Entry (sEntry, m_mmapDictionary, bSecondPass);
        if (!bHeadWordFound)
        {
            sHead = m_pstHead->sRawSource;
        }
        else
        {
            Entry.SetOriginationRawHeadword(m_pstHead->sRawSource);
            sHead.Erase();
        }

        bool bParseResult = Entry.bParse (sHead, sUsageComment);
        if (!bParseResult)
        {
            CEString sMsg (L"*** Parsing error: \n");
            sMsg += sEntry;
            ERROR_LOG (sMsg);
            return false;
        }
    }
    else
    {
        m_pstHead->sVariant = sSource.sGetRegexMatch(0);
        m_pstHead->sVariantComment = sUsageComment;
        ATLASSERT(0);
    }

    return true;

}   //  b_CheckSquareBrackets (...)

bool CZalEntryParser::bCheckCognate (CEString& sDescriptor)
{
    // Handles cases:
    // (_от_ ...)
    // (_женск. к_ ...)
    // (_уменьш. к_ ...)
    // (_увеличит. к_ ...)
    // (_ласкат. к_ ...)
    // (_к_ ...)
    // (_сущ. к_ ...)
    // (_ср._ ...)
    // (_см. также_ ...)
    if (sDescriptor.uiLength() <= 0)
    {
        return true;
    }

    bool bMatch = sDescriptor.bRegexSearch 
        (L"\\(_(к|женск\\.\\s*к|уменьш\\.\\s*к|увеличит\\.\\s*к|ласкат\\.\\s*к|сущ\\.\\s*к|ср\\.|см\\.\\s*также|от)_");
    if (bMatch == false)
    {
        return true;
    }

    bMatch = sDescriptor.bRegexMatch
        (L"(.*?),?\\s*\\(_(к|женск\\.\\s*к|ср\\.|уменьш\\.\\s*к|увеличит\\.\\s*к|ласкат\\.\\s*к|сущ\\.\\s*к|см\\.\\s*также|от)_\\s*([^\\)]*)\\)(.*)");
    if (bMatch == true)
    {
        CEString sLeft = sDescriptor.sGetRegexMatch (0);
        CEString sRight = sDescriptor.sGetRegexMatch (3);
        m_stEntry.sCognate = sDescriptor.sGetRegexMatch (2);
        sDescriptor = sLeft + sRight;
        return true;
    }

    ERROR_LOG(L"CheckCognate was started on " + sDescriptor + L" and couldn't recognize the string.");
    return false;

}   //  bCheckCognate (...)

bool CZalEntryParser::bCheckVerbType (CEString& sDescriptor)
{
    // Handles cases:
    // ..., безл.
    // ..., многокр.
    if (sDescriptor.uiLength() <= 0)
    {
        return true;
    }

    bool bMatch = sDescriptor.bRegexMatch (L"^(.*?),?\\s*безл\\.(.*)");
    if (bMatch)
    {
        sDescriptor = sDescriptor.sGetRegexMatch (0) + sDescriptor.sGetRegexMatch (1);
        m_stEntry.bIsImpersonal = true;
    }
    bMatch = sDescriptor.bRegexMatch (L"^(.*?),?\\s*многокр\\.(.*)");
    if (bMatch)
    {
        sDescriptor = sDescriptor.sGetRegexMatch (0) + sDescriptor.sGetRegexMatch (1);
        m_stEntry.bIsIterative = true;
    }

    return true;

}   //  bCheckVerbType (...)

bool CZalEntryParser::bCheckDollar (CEString& sDescriptor)
{
    m_stEntry.bHasAspectPair = false;

    // The function looks for the aspect pair information.
    // If the number after $ is Roman, write it
    // to i_AspectPairType as negative, otherwise as positive.
    // Push all the numbers found to vec_i_AspectPairType
    // (there can be several, like in $II//I(-<а-)).
    // If there is also a comment in parentheses, push it to vec_str_AspectPairComment.

    if (m_mapPos[m_stEntry.sMainSymbol] != POS_VERB)
    {
        return true;
    }
    m_stEntry.vecAspectPairType.clear();
    m_stEntry.vecAspectPairComment.clear();
    bool bMatch = sDescriptor.bRegexMatch (L"^([^\\$]*)\\$\\s*([^\\[\\]]*)(.*)$");
    if (bMatch == false)
    {
        return true;
    }
  
    CEString sRest = sDescriptor.sGetRegexMatch (1);
    sDescriptor = sDescriptor.sGetRegexMatch (0) + sDescriptor.sGetRegexMatch (2);
    CEString sErrorMsg = L"CheckDollar couldn't recognize the text: " + sRest;
    m_stEntry.bHasAspectPair = true;
    //ERROR_LOG(L"---- CheckDollar - str_comment: " + str_comment + L"\n---- CheckDollar - str_descriptor: " + str_descriptor);
    // Several references can be divided by ; or //
    // (don't look for them inside parentheses)
    CEString sMatcher (L"\\s*([^\\/;\\(]+(\\([^\\)]*?\\)[^\\/;\\(]*)*)(;|\\/\\/)?(.*)?");
    CEString sCurrent;
    CEString sAspectComment;
    while (sRest.bRegexMatch (sMatcher))
    {
        sCurrent = sRest.sGetRegexMatch (0);
        sRest = sRest.sGetRegexMatch (3);
        //ERROR_LOG(L"**** str_current: " + str_current);

        // 1. Roman numerals
        CEString sMatcher1 (L"(_\\s*н?св\\.?\\s*также\\s*_|_простореч\\._|_устар\\._)?");
        CEString sMatcher2 (L"\\s*(I+)\\s*(\\(([\\-<>\\w\\/]*|_[\\w<>\\-\\.\\s\\/,;]*_|");        
        CEString sMatcher3 (L"[\\-<>\\w\\/]*,\\s*_[\\w<>\\-\\.\\s\\/,;]*_)\\))?\\s*");
        if (sCurrent.bRegexMatch (sMatcher1 + sMatcher2 + sMatcher3))
        {
            sAspectComment = sCurrent.sGetRegexMatch (0);
            if (sAspectComment.uiLength() <= 0)
            {
                sAspectComment = sCurrent.sGetRegexMatch (3);
            }
            else
            {
                sAspectComment += L" " + sCurrent.sGetRegexMatch (3);
            }

            m_stEntry.vecAspectPairType.push_back (-1 * ((int)(sCurrent.sGetRegexMatch(1).uiLength())));
            m_stEntry.vecAspectPairComment.push_back (sAspectComment);
            //ERROR_LOG(L"---- CheckDollar: Roman: " + str_ToString<int>(stEntry.vec_i_AspectPairType.at(0)) + L"; " + stEntry.vec_str_AspectPairComment.at(0));
        }        
        // 2. Arabic numerals
//        else if (regex_match(str_current, result,
//            (const wregex)L"(_\\s*н?св\\.?\\s*также\\s*_|_простореч\\._|_устар\\._)?\\s*(\\d+)\\s*(\\(([\\-<>\\w\\/]*|_[\\w<>\\-\\.\\s\\/,;]*_|[\\-<>\\w\\/]*,\\s*_[\\w<>\\-\\.\\s\\/,;]*_)\\))?\\s*"))
        else
        {
            sMatcher1 = L"(_\\s*н?св\\.?\\s*также\\s*_|_простореч\\._|_устар\\._)?";
            sMatcher2 = L"\\s*(\\d+)\\s*(\\(([\\-<>\\w\\/]*|_[\\w<>\\-\\.\\s\\/,;]*_|";
            sMatcher3 = L"[\\-<>\\w\\/]*,\\s*_[\\w<>\\-\\.\\s\\/,;]*_)\\))?\\s*";
            if (sCurrent.bRegexMatch (sMatcher1 + sMatcher2 + sMatcher3))
            {
                sAspectComment = sCurrent.sGetRegexMatch (0);
                if (sAspectComment.uiLength() <= 0)
                {
                    sAspectComment += L" " + sCurrent.sGetRegexMatch (3);
                }
                m_stEntry.vecAspectPairType.push_back (_wtoi (sCurrent.sGetRegexMatch (1)));
                m_stEntry.vecAspectPairComment.push_back (sAspectComment);
            //ERROR_LOG(L"---- CheckDollar: Arabic: " + str_ToString<int>(stEntry.vec_i_AspectPairType.at(0)) + L"; " + stEntry.vec_str_AspectPairComment.at(0));
            }
            else
            {
        // 3. No number at all
                sMatcher = L"((_\\s*н?св\\.?\\s*также\\s*_|_простореч\\._|_устар\\._)?\\s*[\\-<>\\w\\/]+(\\s*\\([^\\)]*\\))?)\\s*";
                if (sCurrent.bRegexMatch (sMatcher))
                {
                    m_stEntry.vecAspectPairType.push_back (0);
                    m_stEntry.vecAspectPairComment.push_back (sCurrent.sGetRegexMatch (0));
                    //ERROR_LOG(L"---- CheckDollar: No number: " + str_ToString<int>(stEntry.vec_i_AspectPairType.at(0)) + L"; " + stEntry.vec_str_AspectPairComment.at(0));
                }
                else
                {
                    ERROR_LOG (sErrorMsg);
                    return false;
                }
            }
        }
    
    }
    
    return true;

}   //  bCheckDollar (...)

bool CZalEntryParser::bCheckYo (CEString& sDescriptor)
{
    bool bMatch = sDescriptor.bRegexMatch (L"^\\s*,?\\s*((\\([^\\)]*\\),?\\s*)*)([оё])([\\s,\\.;\\$%@:\\(].*$|$)");
    if (bMatch == false)
    {
        return true;
    }
    CEString sOYo = sDescriptor.sGetRegexMatch(2);
    sDescriptor = sDescriptor.sGetRegexMatch(0) + sDescriptor.sGetRegexMatch(3);
    if (sOYo == L"о")
    {
        m_stEntry.bO = true;
    }
    else if (sOYo == L"ё")
    {
        m_stEntry.bYo = true;
    }
    return true;
}

bool CZalEntryParser::bCheckSecondCases (CEString& sDescriptor)
{
    bool bMatch = sDescriptor.bRegexMatch(L"^\\s*,?\\s*((\\([^\\)]*\\),?\\s*)*)((\\[?[ПР]2\\s*(\\([^\\)_]*\\))?\\]?,?\\s*)+)([\\s,\\.;\\$%@:\\(].*$|$)");
    if (bMatch == false)
    {
        return true;
    }
    m_stEntry.bLoc2Optional = false;
    m_stEntry.sLoc2Preposition = L"";
    CEString s2nd = sDescriptor.sGetRegexMatch(2);
    sDescriptor = sDescriptor.sGetRegexMatch(0) + sDescriptor.sGetRegexMatch(5);
    while (s2nd.uiLength() > 0)
    {
        bMatch = s2nd.bRegexMatch(L"^\\s*(\\[*)([ПР])2\\s*((\\([^\\)_]*\\))?)\\]?,?\\s*(.*)$");
        if (!bMatch)
        {
            ATLASSERT(0);
            CEString sMsg (L"Error in b_CheckSecondCases: " + s2nd + L"; source: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
        if (s2nd.sGetRegexMatch(1) == L"Р")
        {
            m_stEntry.bGen2 = true;
        }
        else if (s2nd.sGetRegexMatch(1) == L"П")
        {
            m_stEntry.bLoc2 = true;
            if (s2nd.sGetRegexMatch(0).uiLength() > 0)
            {
                m_stEntry.bLoc2Optional = true;
            }
            CEString sPreposition = s2nd.sGetRegexMatch(2);
            if (sPreposition.uiLength() > 0)
            {
                bMatch = sPreposition.bRegexMatch(L"^\\s*\\((.*?)\\)\\]?,?\\s*$");
                if (!bMatch)
                {
                    ATLASSERT(0);
                    CEString sMsg (L"Error in b_CheckSecondCases, xstr_preposition: " + sPreposition + L"; source: ");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                }
                m_stEntry.sLoc2Preposition = sPreposition.sGetRegexMatch(0);
            }
        }
        s2nd = s2nd.sGetRegexMatch(4);
    }
    return true;

}   //  bCheckSecondCases (...)

bool CZalEntryParser::bCheckComma (CEString& sDescriptor)
{
    if (L',' != sDescriptor[0])
    {
        return true;
    }

    sDescriptor.SetBreakChars (L", ");
    if (sDescriptor.uiGetNumOfFields() < 1)
    {
        ATLASSERT(0);
        CEString sMsg (L"Continuation expected after comma: ");
        sMsg += m_sSource;
        ERROR_LOG (sMsg);
        return false;
    }

    sDescriptor = sDescriptor.sSubstr (1); // default value to be returned in 
                                           // case we don't find anything interesting

    CEString sAfterComma = sDescriptor.sGetField (0);
    bool b_ = sAfterComma.bRegexMatch (L"#(\\d{1,2}).*");
    if (b_)
    {
        CEString sSectionNum = sAfterComma.sGetRegexMatch (0);
        m_stEntry.iSection = _wtoi (sSectionNum);
        sDescriptor = sDescriptor.sSubstr (sDescriptor.uiGetFieldOffset (0) + sDescriptor.uiGetFieldLength (0));
    }

    if (L'ё' == sAfterComma[0])
    {
        m_stEntry.bYo = true;
        sDescriptor = sDescriptor.sSubstr (sDescriptor.uiGetFieldOffset (0) + sDescriptor.uiGetFieldLength (0));
    }

    if (L'о' == sAfterComma[0])
    {
        m_stEntry.bO = true;
        sDescriptor = sDescriptor.sSubstr (sDescriptor.uiGetFieldOffset (0) + sDescriptor.uiGetFieldLength (0));
    }

    if (sAfterComma.bStartsWith (L"Р2"))
    {
        m_stEntry.bGen2 = true;
        sDescriptor = sDescriptor.sSubstr (sDescriptor.uiGetFieldOffset (0) + sDescriptor.uiGetFieldLength (0));
    }

    b_ = sAfterComma.bRegexMatch 
            (L".?(П2\\s?(?:\\(([абвгдеёжзийклмнопрстуфхцчшщъыьэюя]{1,2})\\))?).*");
    if (b_)
    {
        if (sAfterComma.uiGetNumOfRegexMatches() > 1)
        {
            CEString sPreposition = sAfterComma.sGetRegexMatch (1);
            if ((L"на" == sPreposition) || (L"в" == sPreposition))
            {
                m_stEntry.sLoc2Preposition = sPreposition;
            }
            int iOffset = sAfterComma.uiGetFieldOffset (0);
            int iLength = sAfterComma.uiGetFieldLength (0);
            if (L'[' == sAfterComma[0])
            {
                int iAt = sAfterComma.uiFind (L"]", iOffset + 1);
                if (wstring::npos == iAt)
                {
                    ATLASSERT(0);
                    CEString sMsg (L"Right bracket missing: ");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                }
                m_stEntry.bLoc2Optional = true;
                iLength = iAt + 1;
            }
            else
            {
                m_stEntry.bLoc2 = true;
            }
            sDescriptor = sDescriptor.sSubstr (sDescriptor.uiGetFieldOffset (0) + iLength);
        }
        else
        {
            ATLASSERT(0);
            CEString sMsg (L"Unexpected preposition after П2: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
    }

    return true;

}   //  b_CheckComma (...)

bool CZalEntryParser::bMakeGraphicStem()
{
    CEString sSource = m_pstHead->sSource;

    switch (m_stEntry.eInflectionSymbol)
    {
        case MS_M:
        case MS_MO:
        case MS_ZH:
        case MS_ZHO:
        case MS_MO_ZHO:
        case MS_S:
        case MS_SO:
        case MS_MS_P:
        {
            if (0 == m_stEntry.stInflectionGroup.iType)
            {
                m_stEntry.sGraphicStem = m_pstHead->sSource;
                break;
            }

            CEString sFinal (g_szRusVowels);
            sFinal += L'й';
            sFinal += L'ь';
            if (sSource.bEndsWithOneOf (sFinal))
            {
                if (sSource.uiLength() < 2)
                {
                    ATLASSERT(0);
                    CEString sMsg (L"Headword too short: ");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                }
                m_stEntry.sGraphicStem = sSource.sSubstr (0, sSource.uiLength()-1);
            }
            else
            {
                m_stEntry.sGraphicStem = m_pstHead->sSource;
            }
            break;
        }
        case MS_MS:
        {
            // In this case, all forms should be considered irregular
            m_stEntry.sGraphicStem = m_pstHead->sSource;
            break;
        }
        case MS_MN:
        case MS_MN_NEOD:
        case MS_MN_ODUSH:
        case MS_MN_OT:
        {
            // This is not mentioned in GS; needs verification.
            if (sSource.uiLength() < 2)
            {
                ATLASSERT(0);
                CEString sMsg (L"Headword too short: ");
                sMsg += sSource;
                ERROR_LOG (sMsg);
                return false;
            }
            m_stEntry.sGraphicStem = sSource.sSubstr (0, sSource.uiLength()-1);
            break;
        }
        case MS_P:
        {
            if (m_stEntry.bNoLongForms)
            {
                m_stEntry.sGraphicStem = sSource;
                break;
            }

            unsigned int uiCharsToRemove = 0;
            if (sSource.bEndsWith (L"ся"))
            {
                uiCharsToRemove = 4;
            }
            else
            {
                uiCharsToRemove = 2;
            }
            if (sSource.uiLength() < uiCharsToRemove)
            {
                ATLASSERT(0);
                CEString sMsg (L"Headword too short: ");
                sMsg += sSource;
                ERROR_LOG (sMsg);
                return false;
            }
            m_stEntry.sGraphicStem = sSource.sSubstr (0, sSource.uiLength() - uiCharsToRemove);
            break;
        }
        case MS_SV:
        case MS_NSV:
        case MS_SV_NSV:
        {
            if (sSource.bEndsWith (L"ти") || 
                sSource.bEndsWith (L"ть") || 
                sSource.bEndsWith (L"чь"))
            {
                m_stEntry.sGraphicStem = sSource.sSubstr (0, sSource.uiLength() - 2);
            }
            else
            {
                if (sSource.bEndsWith (L"тись") || 
                    sSource.bEndsWith (L"ться") || 
                    sSource.bEndsWith (L"чься"))
                {
                    m_stEntry.sGraphicStem = sSource.sSubstr (0, sSource.uiLength() - 4);
                    m_stEntry.bReflexive = true;
                }
                else
                {
                    /*
                    ATLASSERT(0);
                    CEString sMsg (L"Verb not recognized: ");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                    */
                    // For verbal wordforms with no infinitive form, like "поезжай"
                    m_stEntry.sGraphicStem = sSource;
                }
            }
            break;
        }
        default:
        {
            m_stEntry.sGraphicStem = m_pstHead->sSource;
        }
    }

    return true;

}   //  bMakeGraphicStem()

//bool CZalEntryParser::bSaveToDb (StHeadword& stHead, vector<StDescriptor>& vecDescriptors)
//{
//    __int64 llWordId = 0;
//    stHead.bSaveToDb (m_pDb, llWordId);
//    for (unsigned int uiEntry = 0; uiEntry < vecDescriptors.size(); ++uiEntry)
//    {
//        StDescriptor& stEntry = vecDescriptors[uiEntry];
//        __int64 llDescriptorId = -1;
//        bool bRet = stEntry.bSaveToDb (m_pDb, llWordId, llDescriptorId);
//        if (!bRet)
//        {
//            continue;
//        }
//
//        bRet = bSaveLexemeHash (stHead, stEntry, llDescriptorId);
//    }
//
//    return true;
//
//}       //  bSaveToDb (...)

