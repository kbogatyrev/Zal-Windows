#include "StdAfx.h"
#include "Globals.h"
//#include "ExtString.h"
#include "ZalEntryParser.h"

using namespace Hlib;

bool CZalEntryParser::bCheckSharp(CEString& sDescriptor)
{
    if (sDescriptor.uiLength() <= 0)
    {
        return true;
    }
    bool bMatch = sDescriptor.bRegexMatch(L"^([^#]*?),?\\s*\\#\\s*(\\d+)\\s*(.*)$");
    if (!bMatch && m_stEntry.iSection <= 0)
    {
        m_stEntry.iSection = 0;
        return true;
    }

    CEString sNumber = sDescriptor.sGetRegexMatch(1);
    if (sNumber.uiLength() == 0)
    {
        return false;
    }

    m_stEntry.iSection = _wtoi(sNumber);
    CEString sRest = sDescriptor.sGetRegexMatch(2);
    if (sRest.uiLength() > 0)
    {
        sDescriptor = sDescriptor.sGetRegexMatch(0) + L" " + sRest;
    }
    else
    {
        sDescriptor = sDescriptor.sGetRegexMatch(0);
    }
    return true;

}   //  b_CheckSharp(...)

bool CZalEntryParser::bCheckTriangle(CEString& sDescriptor, bool bAlternative)
{
    if (sDescriptor.uiLength() <= 0)
    {
        return true;
    }
    // @ comment can be followed by $, #, [, or %
    bool bMatch = sDescriptor.bRegexMatch 
        (L"^(.*?)\\s*@\\s*([^\\s\\$\\#\\[\\%][^\\$\\#\\[\\%]+[^\\s])\\s*(([\\$\\#\\[\\%].*)?)$");
    if (!bMatch)
    {
        return true;
    }

    CEString sComment = sDescriptor.sGetRegexMatch(1);
    CEString sRest = sDescriptor.sGetRegexMatch(0) + sDescriptor.sGetRegexMatch(2);

    // _сравн._ бойч<ее//б<ойче  --> _сравн._ бойч<ее, _сравн._ б<ойче
    bDetalizeDoubleSlashes(sComment);

    // _мн._ бр<ат/ья, -ьев, -ьям --> _мн._ бр<атья, бр<атьев, бр<атьям
    // OR
    // _буд._ возд<а/м, -шь, -ст, -д<им, -д<ите, -д<ут ->
    // -> _буд._ возд<ам, возд<ашь, возд<аст, воздад<им, воздад<ите, воздад<ут
    bDetalizeSlashes(sComment);
    bDeleteDoubleStress(sComment);

    // _буд. 1 ед._ (_устар._) застон<аю
    bCheckDifficulties(sComment);

    // ALTPAR or POETPAR marks handling
    bMatch = sComment.bRegexMatch(L"^(.*?)[;,]?\\s*(ALTPAR|POETPAR)\\{([^\\}]+)\\}(.*)$");
    if (bMatch)
    {
        CEString sAlt(L"@" + sComment.sGetRegexMatch(2));
        sComment = sComment.sGetRegexMatch(0) + sComment.sGetRegexMatch(3);
        bool bAltProcessed = bCheckTriangle(sAlt, true);
        //ERROR_LOG(L"ALTPAR: " + str_new_descriptor + L";str_comment: " + str_comment);
        if (!bAltProcessed)
        {
            return false;
        }

        if (sComment.bIsEmpty() || sComment.bRegexMatch(L"^\\s*$"))
        {
            if (!bAlternative)
            {
                sDescriptor = sRest;
            }
            return true;
        }
    }

    if (m_mapPos[m_stEntry.sMainSymbol] == POS_NOUN)
    {
        // Replace strings like "_мн._ к<уры, кур, к<урам" with the full paradigm
        CEString sRegex = L"(.*)_мн\\._\\s*(\\(_затрудн._\\))?\\s*([<>\\w\\-]+),\\s*([<>\\w\\-]*),\\s*([<>\\w\\-]*)(.*)";
        bMatch = sComment.bRegexMatch(sRegex);
        while (bMatch)
        {
            CEString sLeft = sComment.sGetRegexMatch(0);
            CEString sNom = sComment.sGetRegexMatch(2);
            CEString sGen = sComment.sGetRegexMatch(3);
            CEString sDat = sComment.sGetRegexMatch(4);
            CEString sRight = sComment.sGetRegexMatch(5);
            CEString sMiddle = L"_И. мн._ " + sNom + 
                               L", _Р. мн._ " + sGen + 
                               L", _Д. мн._ " + sDat;

            // Accusative ending depends on animacy
            if (m_stEntry.sMainSymbol == L"мо" || 
                m_stEntry.sMainSymbol == L"жо" || 
                m_stEntry.sMainSymbol == L"со" ||
                m_stEntry.sMainSymbol == L"мо-жо" ||
                m_stEntry.sMainSymbol == L"мн. одуш.")
            {
                sMiddle += L", _В. мн._ " + sGen +
                              L", _Т. мн._ " + sDat + L"и" +
                              L", _П. мн._ " + sDat.sSubstr (0, sDat.uiLength() - 1) + L"х";
            }
            else
            {
                sMiddle += L", _В. мн._ " + sNom + 
                           L", _Т. мн._ " + sDat + L"и" +
                           L", _П. мн._ " + sDat.sSubstr (0, sDat.uiLength() - 1) + L"х";
            }
            sComment = sLeft + sMiddle + sRight;
            bMatch = sComment.bRegexMatch (sRegex);
        }

        // Replace strings like "_Д., П. ед._ пареми/<и" with the detailed description
        CEString sRegexBase 
            (L"(.*)(И|Р2?|Д|В|Т|П2?)\\.?,\\s*(И|Р2?|Д|В|Т|П2?)\\.?\\s*((ед\\.)|(мн\\.))_\\s*([<>\\-\\w]*)");
        bMatch = sComment.bRegexMatch (sRegexBase + L"(\\/?)([<>\\-\\w]*)(.*)"); 
        while (bMatch)
        {
            CEString sLeft = sComment.sGetRegexMatch(0);
            CEString sCase1 = sComment.sGetRegexMatch(1);
            CEString sCase2 = sComment.sGetRegexMatch(2);
            CEString sNum = sComment.sGetRegexMatch(3);
            CEString sWordForm = sComment.sGetRegexMatch(6) + sComment.sGetRegexMatch(8);
            CEString sWordFormSlash = sComment.sGetRegexMatch(6) + 
                                   sComment.sGetRegexMatch(7) + 
                                   sComment.sGetRegexMatch(8);
            CEString sRight = sComment.sGetRegexMatch(9);
            sComment = sLeft + sCase1 + L". " + sNum + L"_ " +
                           sWordForm + L", _" + sCase2 + L". " +
                           sNum + L"_ " + sWordFormSlash + sRight;

            bMatch = sComment.bRegexMatch (sRegexBase + L"(.*)");
        }   //  while (bMatch)

        // Р. одуш. --> В.
        if (m_stEntry.sMainSymbol == L"мо" ||
            m_stEntry.sMainSymbol == L"жо" ||
            m_stEntry.sMainSymbol == L"со" ||
            m_stEntry.sMainSymbol == L"мо-жо" ||
            m_stEntry.sMainSymbol == L"мн. одуш.")
        {
            if (sComment.bRegexMatch (L"_Р.\\s*ед.") && !sComment.bRegexMatch(L"_В.\\s*ед."))
            {
                if (sComment.bRegexSearch (L"_Р.\\s*ед._\\s*([^\\s]+)"))
                {
                    CEString sGen = sComment.sGetRegexMatch(0);
                    sComment += L", _В. ед._ " + sGen;
                }
            }
            if (sComment.bRegexMatch(L"_Р.\\s*мн.") && !sComment.bRegexMatch(L"_В.\\s*мн."))
            {
                if (sComment.bRegexSearch(L"_Р.\\s*мн._\\s*([^\\s]+)"))
                {
                    CEString sGen = sComment.sGetRegexMatch(0);
                    sComment += L", _В. мн._ " + sGen;
                }
            }
            if (sComment.bRegexMatch(L"_Р._") && !sComment.bRegexMatch(L"_В._"))
            {
                if (sComment.bRegexSearch(L"_Р._\\s*([^\\s]+)"))
                {
                    CEString sGen = sComment.sGetRegexMatch(0);
                    sComment += L", _В._ " + sGen;
                }
            }
        }

        // Finally, if the string is OK, we put it to str_IrregularForms.
        bMatch = sComment.bRegexMatch (L"(_(И|Р2?|Д|В|Т|П2?)\\.?\\s*(ед|мн)\\._\\s*[<>\\w][<>\\-\\w]*([,;]\\s*|$))*");
        if (bMatch)
        {
            if (bAlternative)
            {
                m_stEntry.sAltIrregularForms = sComment;  // slice...
            }
            else
            {
                m_stEntry.sIrregularForms = sComment;
                sDescriptor = sRest;
            }
            //ERROR_LOG (L"--> " + str_comment);
            return true;
        }
    }       //  if (m_mapPos[m_stEntry.sMainSymbol] == POS_NOUN)

    if (m_mapPos[m_stEntry.sMainSymbol] == POS_PRONOUN || 
        m_mapPos[m_stEntry.sMainSymbol] == POS_PRONOUN_ADJ || 
        m_mapPos[m_stEntry.sMainSymbol] == POS_NUM || 
        m_mapPos[m_stEntry.sMainSymbol] == POS_NUM_ADJ)
    {
        // Replace strings like "_Р., Д., П._ двен<адцат/и" with the detailed description
        CEString sMatcher (L"(.*)([^_,]+[^\\s]),\\s*([^_,]+[^\\s])_\\s*([<>\\-\\w]*)(\\/?)([<>\\-\\w]*)(.*)");
        while (sComment.bRegexMatch (sMatcher))
        {
            CEString sLeft = sComment.sGetRegexMatch(0);
            CEString sCase1 = sComment.sGetRegexMatch(1);
            CEString sCase2 = sComment.sGetRegexMatch(2);
            CEString sWordForm = sComment.sGetRegexMatch(3) + sComment.sGetRegexMatch(5);
            CEString sWordFormSlash = sComment.sGetRegexMatch(3) + 
                                      sComment.sGetRegexMatch(4) + 
                                      sComment.sGetRegexMatch(5);
            CEString sRight = sComment.sGetRegexMatch(6);
            sComment = sLeft + sCase1 + L"_ " + sWordForm + 
                       L", _" + sCase2 + L"_ " + sWordFormSlash + sRight;
            bMatch = sComment.bRegexMatch(sMatcher);
        }

        // Replace _В.=И._
        // TODO: Add the stress to the accusative form
        bMatch = sComment.bRegexMatch (L"(.*)_В\\._?\\s*=\\s*_?И\\._(.*)");
        if (bMatch)
        {
            sComment = sComment.sGetRegexMatch(0) + L"_В._ " +
                       m_pstHead->sRawSource + sComment.sGetRegexMatch(1);
            //ERROR_LOG (L"--> " + str_comment);
        }

        // Finally, put the string to str_IrregularForms.
        // (We don't check it because it's easier to check all
        // relevant dictionary entries manually.)
        if (bAlternative)
        {
            m_stEntry.sAltIrregularForms = sComment;     // slice
        }
        else
        {
            m_stEntry.sIrregularForms = sComment;        // slice
            sDescriptor = sRest;
        }

        return true;

    }   //  POS_PRONOUN, POS_PRONOUN_ADJ, POS_NUM, POS_NUM_ADJ

    if (m_mapPos[m_stEntry.sMainSymbol] == POS_ADJ)
    {
        // Replace strings like "_кф_ нес<олон, несолон<а, нес<олоно, 
        // нес<олоны" with the detailed description
        bMatch = sComment.bRegexMatch (L"(.*)_кф_\\s*([<>\\-\\w]+),\\s*([<>\\w]+),\\s*([<>\\w]+),\\s*([<>\\w]+)(.*)");
        if (bMatch)
        {
            CEString sLeft = sComment.sGetRegexMatch (0);
            CEString sM = sComment.sGetRegexMatch (1);
            CEString sF = sComment.sGetRegexMatch (2);
            CEString sN = sComment.sGetRegexMatch (3);
            CEString sPl = sComment.sGetRegexMatch (4);
            CEString sRight = sComment.sGetRegexMatch (5);
            sComment = sLeft + L"_кф м_ " + sM + L", _кф ж_ " + sF 
                + L", _кф с_ " + sN + L", _кф мн._ " + sPl + sRight;
        }

        // Finally, if the string is OK, we put it to str_IrregularForms.
//        if (regex_match(str_comment, (const wregex)L"нет_"))
        if (ecNotFound != sComment.uiFind (L"нет_"))
        {
            bMatch = bCheckDifficulties (sComment);
            if (!bMatch)
            {
                return false;
            }
        }
        bMatch = sComment.bRegexMatch 
            (L"(_(сравн\\.|кф\\s*(мн\\.|м|ж|с))_\\s*[<>\\-\\w]+[;,]\\s*)*(_(сравн\\.|кф\\s*(мн\\.|м|ж|с))_\\s*[<>\\-\\w]+)");
        if (bMatch)
        {
            if (bAlternative)
            {
                m_stEntry.sAltIrregularForms = sComment;  // slice
            }
            else
            {
                m_stEntry.sIrregularForms = sComment;     // slice
                sDescriptor = sRest;
            }
            return true;
        }
    }       //  if (m_mapPos[m_stEntry.sMainSymbol] == POS_ADJ)

    if (m_mapPos[m_stEntry.sMainSymbol] == POS_VERB)
    {
        // _буд._ возд<ам, возд<ашь, возд<аст, воздад<им, воздад<ите, воздад<ут
        bMatch = sComment.bRegexMatch 
            (L"(.*_)(буд\\.|наст\\.)_\\s*([<>\\w]+),\\s*([<>\\w]+),\\s*([<>\\w]+),\\s*([<>\\w]+),\\s*([<>\\w]+),\\s*([<>\\w]+)(.*)");
        if (bMatch)
        {
            CEString sLeft = sComment.sGetRegexMatch (0);
            CEString sTense = sComment.sGetRegexMatch (1);
            CEString s1sg = sComment.sGetRegexMatch (2);
            CEString s2sg = sComment.sGetRegexMatch (3);
            CEString s3sg = sComment.sGetRegexMatch (4);
            CEString s1pl = sComment.sGetRegexMatch (5);
            CEString s2pl = sComment.sGetRegexMatch (6);
            CEString s3pl = sComment.sGetRegexMatch (7);
            CEString sRight = sComment.sGetRegexMatch (8);
            sComment = sLeft + sTense + L" 1 ед._ " + s1sg + L", _" + 
                sTense + L" 2 ед._ " + s2sg + L", _" + 
                sTense + L" 3 ед._ " + s3sg + L", _" + 
                sTense + L" 1 мн._ " + s1pl + L", _" +  
                sTense + L" 2 мн._ " + s2pl + L", _" +
                sTense + L" 3 мн._ " + s3pl + sRight;
        }

        // Expand
        // "_буд._ вгон<ю, вг<онит, вг<онят" OR "_буд._ взрев<у, взрев<ёт"
        bMatch = sComment.bRegexMatch 
            (L"(.*_)(буд\\.|наст\\.)_\\s*([<>\\w]+(у|ю)((сь)?)),\\s*([<>\\w]+)((,\\s*([<>\\w]+))?)(.*)");
        if (bMatch)
        {
            CEString sLeft = sComment.sGetRegexMatch (0);
            CEString sTense = sComment.sGetRegexMatch (1);
            CEString s1sg = sComment.sGetRegexMatch (2);
            CEString s1sgShort = sComment.sGetRegexMatch (3);
            CEString sRefl = sComment.sGetRegexMatch (4);
            CEString s3sg = sComment.sGetRegexMatch (6);
            CEString s3plGroup = sComment.sGetRegexMatch (7);

            CEString s3pl;
            if (s3plGroup.uiLength() > 0)
            {
                s3pl = sComment.sGetRegexMatch (9);
            }

            CEString sRight = sComment.sGetRegexMatch (10);
            if (sRefl.uiLength() > 0)
            {
                sComment = sLeft + sTense + L" 1 ед._ " + s1sg + L", _" + 
                    sTense + L" 2 ед._ " + s3sg.sSubstr (0, s3sg.uiLength() - 3) + L"шься, _" + 
                    sTense + L" 3 ед._ " + s3sg + L", _" + 
                    sTense + L" 1 мн._ " + s3sg.sSubstr (0, s3sg.uiLength() - 3) + L"мся, _" + 
                    sTense + L" 2 мн._ " + s3sg.sSubstr (0, s3sg.uiLength() - 2) + L"есь, _" + 
                    sTense + L" 3 мн._ ";
                if (s3plGroup.uiLength() > 0)
                {
                    sComment += s3pl;
                }
                else
                {
                    sComment += s3sg.sSubstr (0, s3sg.uiLength() - 4) + s1sgShort + L"тся"; 
                                                                // There are no such verbs with а/я in 3pl
                }
            }
            else
            {
                sComment = sLeft + sTense + L" 1 ед._ " + s1sg + L", _" + 
                    sTense + L" 2 ед._ " + s3sg.sSubstr (0, s3sg.uiLength() - 1) + L"шь, _" + 
                    sTense + L" 3 ед._ " + s3sg + L", _" + sTense + L" 1 мн._ " + 
                    s3sg.sSubstr (0, s3sg.uiLength() - 1) + L"м, _" + 
                    sTense + L" 2 мн._ " + s3sg + L"е, _" + sTense + L" 3 мн._ ";
                if (s3plGroup.uiLength() > 0)
                {
                    sComment += s3pl;
                }
                else
                {
                    sComment += s3sg.sSubstr(0, s3sg.uiLength() - 2) + s1sgShort + L"т";
                }
            }
            sComment += sRight;
        
        }   //  if (bMatch)

        // Expand
        // "_повел._ бег<и"
        /*
        CEString sSource (sComment);
        bMatch = sSource.bRegexMatch (L"(.*_)(повел\\.)_\\s*([<>\\w]+?(ь|и)(ся|сь)?)([\\s,;\\}\\]].*)");
        CEString str_mood;
        CEString str_ending;

//        bMatch = regex_match (str_comment, 
//                               result, 
//                               (const wregex)L"(.*_)(повел\\.)_\\s*([<>\\w]+(ь|и)(ся|сь)?))?)(.*)");
        if (bMatch)
        {
            ATLASSERT (sSource.i_GetNumOfRegexMatches() > 0);
            CEString sLeft = sSource.sGetRegexMatch (0);
            CEString str_mood = sSource.sGetRegexMatch (1);
            CEString str_ending = sSource.sGetRegexMatch (2);
            CEString sRefl = sSource.sGetRegexMatch (3);
            CEString sRight = sSource.sGetRegexMatch (4);

//            sLeft = result[1];
//            str_mood = result[2];
//            str_ending = result[3];
//            sRefl = result[4];
//            sRight = result[5];
            if (sRefl.uiLength() > 0)
            {
int gogo = -1;
            }
            else
            {
int gogo = -2;
            }
            sComment += sRight;
        }
        */

        // Expand "_прош._ возд<ал, воздал<а, возд<ало, возд<али"
        CEString sRegex = L"(.*_)прош\\._\\s*([<>\\w]+),\\s*([<>\\w]+),\\s*([<>\\w]+),\\s*([<>\\w]+)(.*)";
        bMatch = sComment.bRegexMatch(sRegex);
        while (bMatch)
        {
            CEString sLeft = sComment.sGetRegexMatch(0);
            CEString sM = sComment.sGetRegexMatch(1);
            CEString sF = sComment.sGetRegexMatch(2);
            CEString sN = sComment.sGetRegexMatch(3);
            CEString sPl = sComment.sGetRegexMatch(4);
            CEString sRight = sComment.sGetRegexMatch(5);
            sComment = sLeft + L"прош. м_ " + sM + L", _прош. ж_ " +
                sF + L", _прош. с_ " + sN + L", _прош. мн._ " + sPl +
                sRight;
            bMatch = sComment.bRegexMatch(sRegex);
        }

        // Expand "_прош._ в<ырос, в<ыросла, в<ыросший"
        // OR
        // "_прош._ взрев<ел, взрев<ела"
        bMatch = sComment.bRegexMatch 
            (L"(.*_)прош\\._\\s*([<>\\w]+),\\s*([<>\\w]+)((,\\s*([<>\\w]+ий(ся)?))?)(.*)");
        if (bMatch)
        {
            CEString sLeft = sComment.sGetRegexMatch (0);
            CEString sM = sComment.sGetRegexMatch (1);
            CEString sF = sComment.sGetRegexMatch (2);
            CEString sParticipleGroup = sComment.sGetRegexMatch (3);
            CEString sParticiple = sComment.sGetRegexMatch (5);
            CEString sRight = sComment.sGetRegexMatch (7);

            CEString sN;
            CEString sPl;
            if (sM.sSubstr (sM.uiLength() - 2, 2) == L"ся")
            {
                sN = sF.sSubstr (0, sF.uiLength() - 3) + L"ось";
                sPl = sF.sSubstr (0, sF.uiLength() - 3) + L"ись";
            }
            else
            {
                sN = sF.sSubstr (0, sF.uiLength() - 1) + L"о";
                sPl = sF.sSubstr (0, sF.uiLength() - 1) + L"и";
            }
            sComment = sLeft + L"прош. м_ " + sM + L", _прош. ж_ " +
                sF + L", _прош. с_ " + sN + L", _прош. мн._ " + sPl;
            if (sParticipleGroup.uiLength() > 0)
            {
                sComment += L", _прич. прош._ " + sParticiple + sRight;
            }
        }

        //     _наст. 2 ед._ вопи<ешь, _3 ед._ -<ет --> 
        // --> _наст. 2 ед._ вопи<ешь, _3 ед._ вопи<ет
        bMatch = sComment.bRegexSearch (L"\\w\\/[<>\\w].*\\s\\-[<>\\w]");
        if (bMatch)
        {
            bDetalizeSlashes (sComment);
            bDeleteDoubleStress (sComment);
        }
        //     _наст. 2 ед._ вопи<ешь, _3 ед._ вопи<ет --> 
        // --> _наст. 2 ед._ вопи<ешь, _наст. 3 ед._ вопи<ет
        bDetalizeCategories (sComment);

        //     _прич. страд._ сд<анный (_кф_ сдан, сдан<а, сдан<о, сдан<ы) --> 
        // --> _прич. страд._ сд<анный, _прич. страд. кф м_ сдан, ...
        bMatch = sComment.bRegexMatch 
            (L"(.*?_(прич\\.\\s*(прош|страд|наст)\\.)_\\s*[<>\\w]+)[,;]?\\s*\\(_кф_\\s*([<>\\-\\w]+),\\s*([<>\\w]+),\\s*([<>\\w]+),\\s*([<>\\w]+)\\)(.*)");
        if (bMatch)
        {
            CEString sLeft = sComment.sGetRegexMatch (0);
            CEString sParticiple = sComment.sGetRegexMatch (1);
            CEString sM = sComment.sGetRegexMatch (3);
            CEString sF = sComment.sGetRegexMatch (4);
            CEString sN = sComment.sGetRegexMatch (5);
            CEString sPl = sComment.sGetRegexMatch (6);
            CEString sRight = sComment.sGetRegexMatch (7);
            sComment = sLeft + L", _" + sParticiple + L" кф м_ " + sM +
                L", _" + sParticiple + L" кф ж_ " + sF + L", _" +
                sParticiple + L" кф с_ " + sN + L", _" +
                sParticiple + L" кф мн._ " + sPl + sRight;
        }

        // Finally, if the string is OK, we put it to str_IrregularForms.
//        if (regex_match(str_comment, (const wregex)L"нет_"))
        if (ecNotFound != sComment.uiFind (L"нет_"))
        {
            bMatch = bCheckDifficulties (sComment);
            if (!bMatch)
            {
                return false;
            }
        }

        //bMatch = sComment.bRegexMatch 
        //    (L"(_(прич\\.\\s*(страд\\.(\\s*(прош|наст)\\.)?|прош\\.(\\s*(мн\\.|м|ж|с))?|наст\\.)(\\s*кф\\s*(мн\\.|ж|м|с))?(\\s*ед\\.)?|деепр\\.(\\s*(прош|наст)\\.)|повел\\.|прош\\.\\s*(мн\\.|м|ж|с)(\\s*ед\\.)?|(наст|буд)\\.\\s*[123]\\s*(ед|мн)\\.|инф\\.)_\\s*[<>\\w][<>\\-\\w]*([,;]\\s*|$))*");
        //if (bMatch)
        //{
            if (bAlternative)
            {
                m_stEntry.sAltIrregularForms = sComment;  // slice
            }
            else
            {
                m_stEntry.sIrregularForms = sComment;
                sDescriptor = sRest;
            }

            return true;
        //}
    }       //  if (m_mapPos[m_stEntry.sMainSymbol] == POS_VERB)

    ERROR_LOG (L"CheckTriangle couldn't recognize the text: " + sComment);
    return false;

}   //  b_CheckTriangle (...)

bool CZalEntryParser::bDetalizeDoubleSlashes (CEString& sSource)
{
    // _сравн._ (_затрудн._) г<ульче//г<улче
    CEString sStem = L"", sRight = L"", sLeft = L"";
    CEString sMatcher = L"^(.*?)(_[^_]*_(\\s*\\(_затрудн\\.?_\\))?)\\s*([^\\s\\/]*)\\s*\\/\\/\\s*([^\\s\\/_]+.*)$";
    bool bMatch = sSource.bRegexMatch(sMatcher);
    while (bMatch)
    {
        sSource = sSource.sGetRegexMatch(0) +
            sSource.sGetRegexMatch(1) + L" " + sSource.sGetRegexMatch(3) + L", " +
            sSource.sGetRegexMatch(1) + L" " + sSource.sGetRegexMatch(4);
        bMatch = sSource.bRegexMatch(sMatcher);
    }
    // _сравн._ г<ульче//_устар._ г<улче
    sMatcher = L"^(.*?)(_[^_]*_)\\s*([^\\s\\/]*)\\s*\\/\\/\\s*(_устар\\._|_затрудн\\._)\\s*([^\\s\\/]+.*)$";
    bMatch = sSource.bRegexMatch(sMatcher);
    while (bMatch)
    {
        //ERROR_LOG(L"****b_DetalizeDoubleSlashes - begin: " + str_source);
        sSource = sSource.sGetRegexMatch(0) +
            sSource.sGetRegexMatch(1) + L" " + sSource.sGetRegexMatch(2) + L", " +
            sSource.sGetRegexMatch(1) + L" (" + sSource.sGetRegexMatch(3) + L") " +
            sSource.sGetRegexMatch(4);
        bMatch = sSource.bRegexMatch(sMatcher);
    }
    //ERROR_LOG(L"****b_DetalizeDoubleSlashes: " + str_source);
    return true;
}   //  b_DetalizeDoubleSlashes (...)

bool CZalEntryParser::bDetalizeSlashes (CEString& sSource)
{
    // _мн._ бр<ат/ья, -ьев, -ьям --> _мн._ бр<ат--ья, бр<ат--ьев, бр<ат--ьям
    // 1. (наст.|буд.)
    if (sSource.bRegexMatch 
        (L"(.*?(_наст\\._|_буд\\._)\\s*)([\\w<>]+?)(у|ю|усь|юсь),\\s*\\-([\\w<>]+)(,\\s*\\-([\\w<>]+))?(.*)$"))
    {
        CEString sLeft = sSource.sGetRegexMatch (0);
        CEString sRight = sSource.sGetRegexMatch (7);
        CEString sStem = sSource.sGetRegexMatch (2);
        CEString str_end1 = sSource.sGetRegexMatch (3);
        CEString str_end2 = sSource.sGetRegexMatch (4);
        CEString str_end3 = sSource.sGetRegexMatch (6);
        sSource = sLeft + sStem + str_end1 + L", " + sStem + L"--" + str_end2;
        if (str_end3.uiLength() > 0)
        {
            sSource += L", " + sStem + L"--" + str_end3;
        }
        sSource += sRight;

        bDeleteDoubleStress (sSource);
        bDetalizeSlashes (sSource); // in case there are several such substrings
        
        return true;
    }
    // 2. With a slash: "_буд._ взъ<е/мся, -шься"
    else if (sSource.bRegexMatch (L"(.*?)([\\w<>\\-]+)\\/([\\w<>]+)(.*)"))
    {
        CEString sLeft = sSource.sGetRegexMatch(0) + 
                         sSource.sGetRegexMatch(1) + 
                         L"--" + 
                         sSource.sGetRegexMatch(2);
        CEString sRight = sSource.sGetRegexMatch(3);
        CEString sStem = sSource.sGetRegexMatch(1);

        while (sRight.bRegexMatch (L"([^\\-\\/]*)\\-([\\w<>]+)(.*)"))
        {
            sLeft += sRight.sGetRegexMatch(0) + sStem + L"--" + sRight.sGetRegexMatch(1);
            sRight = sRight.sGetRegexMatch(2);
            if (sRight.bRegexSearch(L"^[^\\-]*\\s[<>\\w]+[,;]?\\s"))
            {
                // _буд._ зад<а/м, -шь, <...>; _повел._ зад<ай; _прош._ з<адал, -<а, -о, -и
                break;
            }
        }
        sSource = sLeft + sRight;

        bDeleteDoubleStress (sSource);
        bDetalizeSlashes (sSource); // in case there are several such substrings
        
        return true;
    }
    // 3. Without a slash: "_прош._ был, -<а, -о, -и" OR "кф ..."
    else if (sSource.bRegexMatch (L"(.*?_[^_\\s]*_)\\s*([\\w<>]+),\\s*\\-([\\w<>]+)(.*)"))
    {
        CEString sStem = sSource.sGetRegexMatch(1);
        CEString sLeft = sSource.sGetRegexMatch(0) + L" " + sStem + L", " +
            sStem + L"--" + sSource.sGetRegexMatch(2);
        CEString sRight = sSource.sGetRegexMatch(3);
        while (sRight.bRegexMatch (L",\\s*\\-([\\w<>]+)(.*)"))
        {
            sLeft += L", " + sStem + L"--" + sRight.sGetRegexMatch(0);
            sRight = sRight.sGetRegexMatch(1);
        }
        sSource = sLeft + sRight;

        bDeleteDoubleStress (sSource);
        bDetalizeSlashes (sSource); // in case there are several such substrings
        
        return true;
    }
    
    return false;

}   //  bDetalizeSlashes (...)

bool CZalEntryParser::bDetalizeCategories (CEString& sSource)
{
    //     _наст. 2 ед._ вопи<ешь, _3 ед._ вопи<ет --> 
    // --> _наст. 2 ед._ вопи<ешь, _наст. 3 ед._ вопи<ет
    bool bMatch = sSource.bRegexMatch 
        (L"(.*_)((наст)|(буд)\\.)(\\s*[123]\\s*((ед)|(мн))\\._[^_]*_)\\s*([123]\\s*((ед)|(мн))\\._)(.*)");
    if (bMatch)
    {
        CEString sCategory = sSource.sGetRegexMatch(1);
        CEString sLeft = sSource.sGetRegexMatch(0) + sCategory + sSource.sGetRegexMatch(4) +
            sCategory + L" " + sSource.sGetRegexMatch(8);
        CEString sRight = sSource.sGetRegexMatch(12);
        while (sRight.bRegexMatch (L"([^_]*_)\\s*([123]\\s*((ед)|(мн))\\._)(.*)"))
        {
            sLeft += sRight.sGetRegexMatch(0) + sCategory + L" " + sRight.sGetRegexMatch(1);
            sRight = sRight.sGetRegexMatch(5);
        }
        sSource = sLeft + sRight;

        return true;
    }
    
    return false;

}   //  b_DetalizeCategories (...)

bool CZalEntryParser::bDeleteDoubleStress (CEString& sSource)
{
    bool bChangesMade = false;

    // 1. невел<ик--<о --> невелик<о
    while (sSource.bRegexMatch (L"(.*)<(\\w*)\\-\\-(\\w*<)(.*)"))
    {
        sSource = sSource.sGetRegexMatch(0) + sSource.sGetRegexMatch(1) + 
            sSource.sGetRegexMatch(2) + sSource.sGetRegexMatch(3);
        bChangesMade = true;
    }

    // 2. был--о --> б<ыло
    while (sSource.bRegexMatch 
        (L"(.*?\\s)([бвгджзйклмнпрстфхцчшщъь]*)([аеёиоуыэюя][^\\-\\s<>]*)\\-\\-([^<]*[,;$].*)"))
    {
        sSource = sSource.sGetRegexMatch(0) + sSource.sGetRegexMatch(1) + L"<" + 
            sSource.sGetRegexMatch (2) + sSource.sGetRegexMatch (3);
        bChangesMade = true;
    }

    // 3. был--<а --> был<а
    while (sSource.bRegexMatch (L"(.*?)\\-\\-(.*)"))
    {
        sSource = sSource.sGetRegexMatch(0) + sSource.sGetRegexMatch(1);
        bChangesMade = true;
    }

    return bChangesMade;

}   //  bDeleteDoubleStress (...)

bool CZalEntryParser::bCheckColon (CEString& sDescriptor)
{
    if (sDescriptor.uiLength() <= 0)
    {
        return true;
    }

    bool bMatch = false;
//    wsmatch ct_result;

    // Looks for: ...:...
    // Except: (...:...)
    bMatch = sDescriptor.bRegexSearch (L"^[^\\(]*(\\([^\\)]*\\)[^\\(]*)*\\:");
    if (!bMatch)
    {
        return true;
    }

    // Handles:
    // ... :{...} ...
    bMatch = sDescriptor.bRegexMatch (L"^(.*?),?\\s*\\:\\s*\\{([^\\}]*)\\}\\s*(.*)$");
    if (bMatch)
    {
        m_stEntry.sContexts = sDescriptor.sGetRegexMatch(1);
        sDescriptor = sDescriptor.sGetRegexMatch(0) + sDescriptor.sGetRegexMatch(2);
        return true;
    }

    // ... : ... [;|%|#|$ ...]
    bMatch = sDescriptor.bRegexMatch 
        (L"^([^\\(]*(\\([^\\)]*\\)[^\\(]*?)*),?\\s*\\:\\s*([^;%#\\$\\{\\}]*)(.*)$");
    if (bMatch)
    {
        m_stEntry.sContexts = sDescriptor.sGetRegexMatch (2);
        sDescriptor = sDescriptor.sGetRegexMatch(0) + sDescriptor.sGetRegexMatch(3);
        return true;
    }

    ERROR_LOG(L"CheckColon was started on " + sDescriptor + L" and couldn't recognize the string.");
    return false;

}   //  b_CheckColon (...)

bool CZalEntryParser::bCheckPercent (CEString& sDescriptor)
{
    if (sDescriptor.uiLength() <= 0)
    {
        return true;
    }
    //ERROR_LOG(L"CheckPercent: " + sDescriptor);
    if (ecNotFound == sDescriptor.uiFind (L"%"))
    {
        return true;
    }
    bool bMatch = sDescriptor.bRegexMatch (L"^([^\\%]*?),?\\s*\\%\\{([^\\}]*)\\}(.*)$");
    if (bMatch)
    {
        //ERROR_LOG(L"CheckPercent, %(): " + str_descriptor);
        m_stEntry.sRestrictedForms = sDescriptor.sGetRegexMatch (1);
        sDescriptor = sDescriptor.sGetRegexMatch (0) + sDescriptor.sGetRegexMatch (2);
        return true;
    }

    bMatch = sDescriptor.bRegexMatch (L"^([^\\%]*?),?\\s*\\%\\s*([^#;\\:\\$]*)(.*)$");
    if (bMatch)
    {
        m_stEntry.sRestrictedForms = sDescriptor.sGetRegexMatch (1);
        //ERROR_LOG(L"CheckPercent was started on " + str_descriptor + L", result is " + m_stEntry.sRestrictedForms);
        sDescriptor = sDescriptor.sGetRegexMatch (0) + sDescriptor.sGetRegexMatch (2);
        return true;
    }

    ERROR_LOG (L"CheckPercent was started on " + sDescriptor + L" and couldn't recognize the string.");
    return false; // If there are parentheses, slashes, or something

}   //  b_CheckPercent (...)

bool CZalEntryParser::bCheckDifficulties (CEString& sDescriptor)
{
    // Handles cases:
    // _Р. мн. затрудн._
    // _пф нет_
    // etc.
    if (sDescriptor.uiLength() <= 0)
    {
        return true;
    }

    bool bMatch = sDescriptor.bRegexSearch (L"[^\\$]*(затрудн\\.|нет|устар\\.|простореч\\.)_");
    if (!bMatch)
    {
        return true;
    }

    // _Р. мн. затрудн._, etc
    CEString sMatch = L"(.*?)[;,]?\\s*_([^_]+?)\\s*затрудн\\._(.*)";
    bMatch = sDescriptor.bRegexMatch (sMatch);
    while (bMatch)
    {
        CEString sLeft = sDescriptor.sGetRegexMatch(0);
        CEString sRight = sDescriptor.sGetRegexMatch(2);
        CEString sTarget (sDescriptor.sGetRegexMatch(1));
        bMatch = sTarget.bRegexMatch (L"(.*)\\sи\\s(.*)");
        if (bMatch)
        {
            CEString sGramm (sTarget.sGetRegexMatch (0));
            bMatch = sGramm.bRegexMatch (L"(.*)\\s(м|ж|с|ед\\.|мн\\.)");
            if (bMatch)
            {
                m_stEntry.sDifficulties += sGramm.sGetRegexMatch(0) + L" " +
                    sGramm.sGetRegexMatch(1) + L", " + 
                    sTarget.sGetRegexMatch(1) + L", ";
                sDescriptor = sLeft + sRight;
            }
            else
            {
                m_stEntry.sDifficulties += sGramm.sGetRegexMatch(0) + L", " + 
                    sGramm.sGetRegexMatch(1) + L",";
                sDescriptor = sLeft + sRight;
            }
        }
        else
        {
            m_stEntry.sDifficulties += sTarget + L",";
            sDescriptor = sLeft + sRight;
        }

        bMatch = sDescriptor.bRegexMatch (sMatch);  
    }   //  while (bMatch)
    
    // _Р. мн. устар._
    sMatch = L"(.*?)[;,]?\\s*_([^_]+?)\\s*устар\\._(.*)";
    bMatch = sDescriptor.bRegexMatch (sMatch);
    while (bMatch)
    {
        CEString sLeft = sDescriptor.sGetRegexMatch(0);
        CEString sRight = sDescriptor.sGetRegexMatch(2);
        m_stEntry.sObsolete += sDescriptor.sGetRegexMatch(1) + L",";
        sDescriptor = sLeft + sRight;
        bMatch = sDescriptor.bRegexMatch (sMatch);  
    }

    // _Р. мн._ (_затрудн._) ...
    // We should also memorize the form itself because there can be several
    // different forms with the same grammatical parameters, only one of which
    // being "difficult"
    sMatch = L"(.*_([^_]*)_\\s*)\\(_затрудн\\._\\)\\s*([\\w<>\\-]+)(.*)";
    bMatch = sDescriptor.bRegexMatch (sMatch);
    while (bMatch)
    {
        m_stEntry.sDifficulties += sDescriptor.sGetRegexMatch (1) + L" (" +
                                     sDescriptor.sGetRegexMatch (2) + L"), ";
        sDescriptor = sDescriptor.sGetRegexMatch (0) +
                          sDescriptor.sGetRegexMatch (2) +
                          sDescriptor.sGetRegexMatch (3);
        bMatch = sDescriptor.bRegexMatch (sMatch);
    }

    // _Р. мн._ (_устар._) ...
    // We should also memorize the form itself because there can be several
    // different forms with the same grammatical parameters, only one of which
    // being obsolete
    sMatch = L"(.*_([^_]*)_\\s*)\\(_устар\\._\\)\\s*([\\w<>\\-]+)(.*)";
    bMatch = sDescriptor.bRegexMatch (sMatch);
    while (bMatch)
    {
        m_stEntry.sObsolete += sDescriptor.sGetRegexMatch (1) + L" (" +
                                  sDescriptor.sGetRegexMatch (2) + L"), ";
        sDescriptor = sDescriptor.sGetRegexMatch (0) +
                          sDescriptor.sGetRegexMatch (2) +
                          sDescriptor.sGetRegexMatch (3);
        bMatch = sDescriptor.bRegexMatch (sMatch);
    }

    // _повел._ (_простореч._) ...
    // We should also memorize the form itself because there can be several
    // different forms with the same grammatical parameters, only one of which
    // is informal
    sMatch = L"([^\\$]*_([^_]*)_\\s*)\\(_простореч\\._\\)\\s*([\\w<>\\-]+)(.*)";
    bMatch = sDescriptor.bRegexMatch (sMatch);
    while (bMatch)
    {
        m_stEntry.sColloquial += sDescriptor.sGetRegexMatch (1) + L" (" +
                                    sDescriptor.sGetRegexMatch (2) + L"), ";
        sDescriptor = sDescriptor.sGetRegexMatch (0) +
                          sDescriptor.sGetRegexMatch (2) +
                          sDescriptor.sGetRegexMatch (3);
        bMatch = sDescriptor.bRegexMatch (sMatch);
    }

    // _пф нет_
    //m_stEntry.str_Deficient = L"";
    sMatch = L"(.*?)[;,]?\\s*_([^_]+?)\\s*нет_(.*)";
    bMatch = sDescriptor.bRegexMatch (sMatch);
    while (bMatch)
    {
        m_stEntry.sDeficient += sDescriptor.sGetRegexMatch (1) + L", ";
        sDescriptor = sDescriptor.sGetRegexMatch (0) + sDescriptor.sGetRegexMatch (2);
        bMatch = sDescriptor.bRegexMatch (sMatch);
    }

    // Finally, check if we've done everything we had to
    return !sDescriptor.bRegexSearch (L"(затрудн\\.|[\\s_]нет|\\(_устар\\.)_|^[^\\$]*_простореч\\._");

}   // b_CheckDifficulties (...)

bool CZalEntryParser::bCheckInflectedParts (CEString& sDescriptor)
{
    // Handles cases:
    // ..., INFL=1
    // ..., INFL=2
    if (sDescriptor.uiLength() <= 0)
    {
        return true;
    }
    
    bool bMatch = sDescriptor.bRegexMatch (L"^(.*?),?\\s*INFL=1(.*)$");
    if (bMatch)
    {
        sDescriptor = sDescriptor.sGetRegexMatch (0) + sDescriptor.sGetRegexMatch (1);
        m_stEntry.stInflectionGroup.iInflectedParts = 1;
    }

    bMatch = sDescriptor.bRegexMatch (L"^(.*?),?\\s*INFL=2(.*)$");
    if (bMatch)
    {
        sDescriptor = sDescriptor.sGetRegexMatch (0) + sDescriptor.sGetRegexMatch (1);
        m_stEntry.stInflectionGroup.iInflectedParts = 2;
    }
    return true;

}   //  b_CheckInflectedParts (...)
