#include "StdAfx.h"
#include "EndingsParser.h"

void CEndingsParser::Init()
{
    m_eClass = ENDING_CLASS_UNDEFINED;
    m_eGender = GENDER_UNDEFINED;
    m_iInflectionType = -1;
    m_eCase = CASE_UNDEFINED;
    m_eNumber = NUM_UNDEFINED;
//    i_Person = -1;
    m_ePerson = PERSON_UNDEFINED;
    m_eAnimacy = ANIM_UNDEFINED;
    m_eStress = STRESS_LOCATION_UNDEFINED;
    m_iConjugation = -1;
    m_bStemAugment = false;
    m_iStemAugmentType = -1;
    m_iCommonDeviation = 0;
    m_eStemAuslaut = STEM_AUSLAUT_UNDEFINED;

}   //  v_Init()

bool CEndingsParser::bParse()
{
    Init();

    wchar_t chrClass = m_sSource[0];
    switch (chrClass)
    {
        case L'N':
        {
            m_eClass = ENDING_CLASS_NOUN;
            break;
        }
        case L'A':
        {
            m_eClass = ENDING_CLASS_ADJECTIVE;
            break;
        }
        case L'P':
        {
            m_eClass = ENDING_CLASS_PRONOUN;
            break;
        }
        case L'H':
        {
            m_eClass = ENDING_CLASS_SHORT_ADJECTIVE;
            break;
        }
        case L'R':
        {
            m_eClass = ENDING_CLASS_PRESENT_TENSE;
            break;
        }
        case L'S':
        {
            m_eClass = ENDING_CLASS_PAST_TENSE;
            break;
        }
		case L'C':
        {
			m_eClass = ENDING_CLASS_COMPARATIVE;
            break;
        }
        default:
        {
            ATLASSERT(0);
            CEString sMsg (L"Unexpected character in class field: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }

    }   // switch ...

    m_sEnding = m_sSource.sSubstr (1, 7);
    m_sEnding.Trim();

    wchar_t chr_gender = m_sSource[8];
    switch (chr_gender)
    {
        case L'm':
        {
            m_eGender = GENDER_M;
            break;
        }
        case L'n':
        {
            m_eGender = GENDER_N;
            break;
        }
        case L'f':
        {
            m_eGender = GENDER_F;
            break;
        }
        default:
        {
            m_eGender = GENDER_UNDEFINED;
            if (L' ' != chr_gender)
            {
                ATLASSERT(0);
                CEString sMsg (L"Unexpected character in gender field: ");
                sMsg += m_sSource;
                ERROR_LOG (sMsg);
                return false;
            }
        }

    }   // switch ...

    if ((ENDING_CLASS_PRESENT_TENSE != m_eClass) && (ENDING_CLASS_PAST_TENSE !=  m_eClass))
    {
        if (L' ' == m_sSource[9])
        {
            ATLASSERT(0);
            CEString sMsg (L"Missing inflection number: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
        m_iInflectionType = _wtoi (&m_sSource[9]);
        if (L'A' == m_sSource[10])
        {
            m_eAnimacy = ANIM_YES;
        }
        else
        {
//            m_iInflectionType = _wtoi (&m_sSource[9]);
            // TODO: check value

            if (L'U' == m_sSource[10])
            {
                m_eAnimacy = ANIM_NO;
            }
            else if (L' ' != m_sSource[10])
            {
                ATLASSERT(0);
                CEString sMsg (L"Unexpected character in animacy field: ");
                sMsg += m_sSource;
                ERROR_LOG (sMsg);
                return false;
            }
        }
    }

    if (ENDING_CLASS_PRESENT_TENSE == m_eClass)
    {
        if (L'е' == m_sSource[10])
        {
            m_iInflectionType = 1;
        }
        else if (L'и' == m_sSource[10])
        {
            m_iInflectionType = 2;
        }
        else
        {
            ATLASSERT(0);
            CEString sMsg (L"Unexpected character in conjugation field: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
    }

    if ((L'S' == m_sSource[11]) || (L'A' == m_sSource[11]))
    {
        m_eStress = STRESS_LOCATION_ENDING;
    }
    else
    {
        if (L'U' == m_sSource[11])
        {
            m_eStress = STRESS_LOCATION_STEM;
        }
        else if (L' ' != m_sSource[11])
        {
            ATLASSERT(0);
            CEString sMsg (L"Unexpected character in stress field: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
    }

    if (L'S' == m_sSource[12])
    {
//        b_Plural = false;
        m_eNumber = NUM_SG;
    }
    else
    {
        if (L'P' == m_sSource[12])
        {
//            b_Plural = true;
            m_eNumber = NUM_PL;
        }
        else
        {
            ATLASSERT(0);
            CEString sMsg (L"Unexpected character in number field: ");
            sMsg += m_sSource;
            ERROR_LOG (sMsg);
            return false;
        }
    }

    if (m_sSource.uiLength() >= 14)
    {
        if (ENDING_CLASS_PRESENT_TENSE == m_eClass)
        {
            wchar_t arrchr_person[2];
            arrchr_person[0] = m_sSource[13];
            arrchr_person[1] = 0;
            int i_person = _wtoi (arrchr_person);
            if ((1 > i_person) && (3 < i_person))
            {
                ATLASSERT(0);
                CEString sMsg (L"Unexpected character in person field: ");
                sMsg += m_sSource;
                ERROR_LOG (sMsg);
                return false;
            }
            
            switch (i_person)
            {
                case 1:
                {
                    m_ePerson = PERSON_1;
                    break;
                }
                case 2:
                {
                    m_ePerson = PERSON_2;
                    break;
                }
                case 3:
                {
                    m_ePerson = PERSON_3;
                    break;
                }
                default:
                {
                    ATLASSERT(0);
                    CEString sMsg (L"Unexpected digit in person field: ");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                }
            }
        }
        else
        {
            wchar_t cCase = m_sSource[13];
            switch (cCase)
            {
                case L'N':
                {
                    m_eCase = CASE_NOM;
                    break;
                }
                case L'G':
                {
                    m_eCase = CASE_GEN;
                    break;
                }
                case L'D':
                {
                    m_eCase = CASE_DAT;
                    break;
                }
                case L'A':
                {
                    m_eCase = CASE_ACC;
                    break;
                }
                case L'I':
                {
                    m_eCase = CASE_INST;
                    break;
                }
                case L'L':
                {
                    m_eCase = CASE_PREP;
                    break;
                }
                default:
                {
                    m_eCase = CASE_UNDEFINED;
                    if (L' ' != cCase)
                    {
                        ATLASSERT(0);
                        CEString sMsg (L"Unexpected character in case field: ");
                        sMsg += m_sSource;
                        ERROR_LOG (sMsg);
                        return false;
                    }
                }
            }   // switch ...
        }
    }   // if (m_sSource.length() >= 14)

    if (m_sSource.uiLength() >= 15)
    {
        if (ENDING_CLASS_PRESENT_TENSE == m_eClass)
        {
            m_iConjugation = _wtoi (&m_sSource[14]);
        }
        else
        {
            wchar_t c_ = m_sSource[14];
            switch (c_)
            {
                case L'°':
                {
                    m_bStemAugment = true;
                    m_iStemAugmentType = 1;
                    break;
                }
                case L'1':
                {
                    m_iCommonDeviation = 1;
                    break;
                }
                case L'2':
                {
                    m_iCommonDeviation = 2;
                    break;
                }
                default:
                {
                    if (L' ' != c_)
                    {
                        ATLASSERT(0);
                        CEString sMsg (L"Unexpected character in circle field: ");
                        sMsg += m_sSource;
                        ERROR_LOG (sMsg);
                        return false;
                    }
                }
            }   // switch ...
        }
    }

    if (m_sSource.uiLength() >= 16)
    {
        if (ENDING_CLASS_NOUN == m_eClass)
        {
            if (L'ч' == m_sSource[15])
            {
                if (!m_bStemAugment)
                {
                    ATLASSERT(0);
                    CEString sMsg (L"Stem augment unexpected: ");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                }
                m_iStemAugmentType = 2; 
            }
            if (L'я' == m_sSource[15])
            {
                if (!m_bStemAugment)
                {
                    ATLASSERT(0);
                    CEString sMsg (L"Stem augment unexpected: ");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                }
                m_iStemAugmentType = 3; 
            }
            if (L'1' == m_sSource[15])
            {
                if (!m_bStemAugment)
                {
                    ATLASSERT(0);
                    CEString sMsg (L"Stem augment unexpected: ");
                    sMsg += m_sSource;
                    ERROR_LOG (sMsg);
                    return false;
                }
                m_iCommonDeviation = 1;  // only господа?
            }
        }
    }

    if (m_sSource.uiLength() >= 17)
    {
        if (ENDING_CLASS_PRESENT_TENSE == m_eClass)
        {
            if (L'ш' == m_sSource[16])
            {
                m_eStemAuslaut = STEM_AUSLAUT_SH;
            }
            else if (L'N' == m_sSource[16])
            {
                m_eStemAuslaut = STEM_AUSLAUT_NOT_SH;
            }
            else if (L' ' != m_sSource[16])
            {
                ATLASSERT(0);
                CEString sMsg (L"Unexpected character in stem auslaut field: ");
                sMsg += m_sSource;
                ERROR_LOG (sMsg);
                return false;
            }
        }
    }

    bool b_ = bSaveToDb();

    return true;

}   // b_Parse()

bool CEndingsParser::bSaveToDb()
{
    try
    {
        m_pDb->PrepareForInsert (L"endings", 13);
        m_pDb->Bind (1, m_sEnding);  // 0-based
        m_pDb->Bind (2, m_eClass);
        m_pDb->Bind (3, m_iInflectionType);
        m_pDb->Bind (4, m_eCase);
        m_pDb->Bind (5, m_ePerson);
        m_pDb->Bind (6, m_eGender);
        m_pDb->Bind (7, m_eAnimacy);
        m_pDb->Bind (8, m_eStress);
        m_pDb->Bind (9, m_eNumber);
        m_pDb->Bind (10, m_iStemAugmentType);
        m_pDb->Bind (11, m_iCommonDeviation);
        m_pDb->Bind (12, m_iConjugation);
        m_pDb->Bind (13, m_eStemAuslaut);

        m_pDb->InsertRow();
        m_pDb->Finalize();        
    }
    catch (...)
    {
        CEString sMsg;
        CEString sError;
        try
        {
            m_pDb->GetLastError (sError);
            sMsg += L", error %d: ";
            sMsg += sError;
        }
        catch (...)
        {
            sMsg = L"Apparent DB error ";
        }
    
        CString cs_msg;
        cs_msg.Format (sMsg, m_pDb->iGetLastError());
        ERROR_LOG ((LPCTSTR)cs_msg);

        return false;
    }

     return true;

}   //  b_SaveToDb()
