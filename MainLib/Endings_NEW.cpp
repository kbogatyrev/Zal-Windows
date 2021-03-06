#include "StdAfx.h"
#include "Lexeme.h"
#include "SqliteWrapper.h"
#include "Endings.h"

void CEndings::ReportDbError()
{
    if (!m_pLexeme)
    {
        ERROR_LOG (L"No lexeme handle.");
    }

    CSqlite * pDb = NULL;
    CEString sMsg;
    try
    {
        pDb = m_pLexeme->pGetDb();
        CEString sError;
        pDb->GetLastError (sError);
        sMsg = L"DB error: ";
        sMsg += sError;
        sMsg += L"; code = ";
        sMsg += CEString::sToString (pDb->iGetLastError());
    }
    catch (...)
    {
        sMsg = L"Apparent DB error ";
    }

    sMsg += CEString::sToString (pDb->iGetLastError());
    ERROR_LOG (sMsg);

}   //  ReportDbError()

CNounEndings::CNounEndings(CLexeme * pLexeme) : CEndings(pLexeme)
{}

ET_ReturnCode CNounEndings::eLoad()
{
    ET_ReturnCode rc = H_NO_ERROR;

    CEString sSelect (L"SELECT DISTINCT ending, number, case_value, stress FROM endings");
    sSelect += L" WHERE inflection_class = ";
    sSelect += CEString::sToString (ENDING_CLASS_NOUN);
    sSelect += L" AND inflection_type = ";
    sSelect += CEString::sToString (m_pLexeme->iType());
    sSelect += L" AND (gender = ";
    sSelect += CEString::sToString (m_pLexeme->eGender());
    sSelect += L" OR gender = ";
    sSelect += CEString::sToString (GENDER_UNDEFINED) + L")";
    sSelect += L" AND (animacy = ";
    sSelect += CEString::sToString (m_pLexeme->eAnimacy());
    sSelect += L" OR animacy = ";
    sSelect += CEString::sToString (ANIM_UNDEFINED) + L")";
    sSelect += L" AND stem_augment = ";
//    sSelect += pLexeme->bStemAugment ? L"1" : L"0";
    sSelect += CEString::sToString (m_pLexeme->iStemAugment());

    CSqlite * pDb = NULL;
    try
    {
        pDb = m_pLexeme->pGetDb();

        CEString sSelectBase (sSelect);
        sSelectBase += L" AND common_deviation = 0;";

        pDb->PrepareForSelect (sSelectBase);
        while (pDb->bGetRow())
        {
            CEString sEnding;
            StEndingDescriptor stD;
            pDb->GetData (0, sEnding);
            pDb->GetData (1, (int&)stD.eNumber);
            pDb->GetData (2, (int&)stD.eCase);
            pDb->GetData (3, (int&)stD.eStress);

            bool bIsOptional = false;
            if (m_pLexeme->bFindCommonDeviation(1, bIsOptional))
            {
                if (NUM_PL == stD.eNumber && CASE_NOM == stD.eCase && GENDER_F != stD.eGender)
                {
                    if (!bIsOptional)
                    {
                        continue;
                    }
                }
            }

            if (m_pLexeme->bFindCommonDeviation(2, bIsOptional))
            {
                if (NUM_PL == stD.eNumber && CASE_GEN == stD.eCase)
                {
                    if (!bIsOptional)    // optional?
                    {
                        continue;
                    }
                }
            }
            rc = eAddEnding (sEnding, stD);
            if (rc != H_NO_ERROR)
            {
                return rc;
            }

        }   //  while (pDb->bGetRow())

        pDb->Finalize();
    }
    catch (...)
    {
        ReportDbError();
        return H_ERROR_GENERAL;
    }

    for (int iCd = 1; iCd <= 2; ++iCd)
    {
        if (!m_pLexeme->bHasCommonDeviation(iCd))
        {
            continue;
        }

        CEString sSelectCD (sSelect);
        sSelectCD += L" AND common_deviation = ";
        sSelectCD += CEString::sToString (iCd);
        sSelectCD += L";";

        try
        {
            pDb->PrepareForSelect (sSelectCD);
            while (pDb->bGetRow())
            {
                CEString sEnding;
                StEndingDescriptor stD;
                pDb->GetData (0, sEnding);
                pDb->GetData (1, (int&)stD.eNumber);
                pDb->GetData (2, (int&)stD.eCase);
                pDb->GetData (3, (int&)stD.eStress);
                rc = eAddEnding (sEnding, stD);
                if (rc != H_NO_ERROR)
                {
                    return rc;
                }
            }
            pDb->Finalize();
        }
        catch (...)
        {
            ReportDbError();
            return H_ERROR_DB;
        }

    }   // for ...

    return H_NO_ERROR;

}   //  eLoad()

int CNounEndings::iHash (const StEndingDescriptor& stD)
{
    ASSERT (stD.eAnimacy == ANIM_UNDEFINED && stD.eGender == GENDER_UNDEFINED);

    int iKey = stD.eNumber * CASE_COUNT * STRESS_LOCATION_COUNT +
                stD.eCase * STRESS_LOCATION_COUNT +
                stD.eStress;

    return iKey;
}

ET_ReturnCode CNounEndings::eAddEnding (const CEString& sEnding, const StEndingDescriptor& stDescriptor)
{
    std::vector<ET_Number> vecNumber;
    if (NUM_UNDEFINED == stDescriptor.eNumber)
    {
        for (int iNum = (int)NUM_UNDEFINED; iNum < (int)NUM_COUNT; ++iNum)
        {
            vecNumber.push_back ((ET_Number)iNum);
        }
    }
    else
    {
        vecNumber.push_back (stDescriptor.eNumber);
    }

    //
    // Case must be defined for nouns
    //
    if (CASE_UNDEFINED == stDescriptor.eCase)
    {
        ERROR_LOG (L"Undefined case in noun ending.");
        return H_ERROR_INVALID_ARG;
    }

    std::vector<ET_StressLocation> vecStress;
    if (STRESS_LOCATION_UNDEFINED == stDescriptor.eStress)
    {
        for (int iStress = STRESS_LOCATION_UNDEFINED; 
             iStress < (int)STRESS_LOCATION_COUNT; 
             ++iStress)
        {
            vecStress.push_back ((ET_StressLocation)iStress);
        }
    }
    else
    {
        vecStress.push_back (stDescriptor.eStress);
    }

    for (int iN = 0; iN < (int)vecNumber.size(); ++iN)
        for (int iS = 0; iS < (int)vecStress.size(); ++iS)
        {
            StEndingDescriptor stD (vecNumber[iN], stDescriptor.eCase, vecStress[iS]);
            int iKey = iHash (stD);
            m_mmEndings.insert (std::pair<int, CEString> (iKey, sEnding));
        }

    return H_NO_ERROR;

}   // CNounEndings::eAddEnding (...)

////////////////////////////////////////////////////////////////////////////////////

CAdjLongEndings::CAdjLongEndings(CLexeme * pLexeme, ET_Subparadigm eSubparadigm) : CEndings(pLexeme), m_eSubparadigm(eSubparadigm)
{}

ET_ReturnCode CAdjLongEndings::eLoad()
{
    ET_ReturnCode rc = H_NO_ERROR;

    CEString sSelect (L"SELECT DISTINCT ending, gender, number, case_value, animacy, stress FROM endings");
    sSelect += L" WHERE inflection_class = ";

    if (L"мс" == m_pLexeme->sInflectionType())
    {
        sSelect += CEString::sToString (ENDING_CLASS_PRONOUN);
    }
    else
    {
        sSelect += CEString::sToString (ENDING_CLASS_ADJECTIVE);
    }

    sSelect += L" AND inflection_type = ";
    if (SUBPARADIGM_PART_PRES_ACT == m_eSubparadigm || SUBPARADIGM_PART_PAST_ACT == m_eSubparadigm)
    {
        sSelect += L"4";
    }
    else if (SUBPARADIGM_PART_PRES_PASS_LONG == m_eSubparadigm || SUBPARADIGM_PART_PAST_PASS_LONG == m_eSubparadigm)
    {
        sSelect += L"1";
    }
    else
    {
        sSelect += CEString::sToString (m_pLexeme->iType());
    }
    sSelect += L";";

    CSqlite * pDb = NULL;

    try
    {
        pDb = m_pLexeme->pGetDb();
        pDb->PrepareForSelect (sSelect);
        while (pDb->bGetRow())
        {
            CEString sEnding;
            StEndingDescriptor stD;
            pDb->GetData (0, sEnding);
            pDb->GetData (1, (int&)stD.eGender);
            pDb->GetData (2, (int&)stD.eNumber);
            pDb->GetData (3, (int&)stD.eCase);
            pDb->GetData (4, (int&)stD.eAnimacy);
            pDb->GetData (5, (int&)stD.eStress);
            rc = eAddEnding (sEnding, stD);
            if (rc != H_NO_ERROR)
            {
                return rc;
            }
        }
        pDb->Finalize();
    }
    catch (...)
    {
        ReportDbError();    
        return H_ERROR_DB; 
    }

    return rc;
        
}   //  eLoad()

int CAdjLongEndings::iHash (const StEndingDescriptor& stD)
{
    int iKey = stD.eGender * NUM_COUNT * CASE_COUNT * ANIM_COUNT * STRESS_LOCATION_COUNT +
               stD.eNumber * CASE_COUNT * ANIM_COUNT * STRESS_LOCATION_COUNT +
               stD.eCase * ANIM_COUNT * STRESS_LOCATION_COUNT +
               stD.eAnimacy * STRESS_LOCATION_COUNT +
               stD.eStress;

    return iKey;
}

ET_ReturnCode CAdjLongEndings::eAddEnding (const CEString& sEnding, const StEndingDescriptor& stDescriptor)
{
    //
    // Gender
    //
    std::vector<ET_Gender> vecGender;
    if (GENDER_UNDEFINED == stDescriptor.eGender)
    {
        for (int iGen = (int)GENDER_UNDEFINED; iGen < GENDER_COUNT; ++iGen)
        {
            vecGender.push_back ((ET_Gender)iGen);
        }
    }
    else
    {
        vecGender.push_back (stDescriptor.eGender);
    }

    //
    // Number
    //
    std::vector<ET_Number> vecNumber;
    if (NUM_UNDEFINED == stDescriptor.eNumber)
    {
        for (int iNum = (int)NUM_UNDEFINED; iNum < (int)NUM_COUNT; ++iNum)
        {
            vecNumber.push_back ((ET_Number)iNum);
        }
    }
    else
    {
        vecNumber.push_back (stDescriptor.eNumber);
    }

    //
    // Animacy
    //
    std::vector<ET_Animacy> vecAnimacy;
    if (ANIM_UNDEFINED == stDescriptor.eAnimacy)
    {
        for (int iAnim = ANIM_UNDEFINED; iAnim < (int)ANIM_COUNT; ++iAnim)
        {
            vecAnimacy.push_back ((ET_Animacy)iAnim);
        }
    }
    else
    {
        vecAnimacy.push_back (stDescriptor.eAnimacy);
    }

    //
    // Ending stressed/unstressed
    //
    std::vector<ET_StressLocation> vecStress;
    if (STRESS_LOCATION_UNDEFINED == stDescriptor.eStress)
    {
        for (int iStress = STRESS_LOCATION_UNDEFINED; iStress < (int)STRESS_LOCATION_COUNT; ++iStress)
        {
            vecStress.push_back ((ET_StressLocation)iStress);
        }
    }
    else
    {
        vecStress.push_back (stDescriptor.eStress);
    }

    for (int iG = 0; iG < (int)vecGender.size(); ++iG)
        for (int iN = 0; iN < (int)vecNumber.size(); ++iN)
            for (int iA = 0; iA < (int)vecAnimacy.size(); ++iA)
                for (int iS = 0; iS < (int)vecStress.size(); ++iS)
                {
                    StEndingDescriptor stD (vecGender[iG], vecNumber[iN], stDescriptor.eCase, vecAnimacy[iA], \vecStress[iS]);
                    int iKey = iHash (stD);
                    m_mmEndings.insert (std::pair<int, CEString> (iKey, sEnding));
                }

    return H_NO_ERROR;

}   //  CAdjLongEndings::eAddEnding (...)

////////////////////////////////////////////////////////////////////////////////////

CAdjShortEndings::CAdjShortEndings(CLexeme * pLexeme) : CEndings(pLexeme)
{}

ET_ReturnCode CAdjShortEndings::eLoad()
{
    ET_ReturnCode rc = H_NO_ERROR;

    CEString sSelect (L"SELECT DISTINCT ending, gender, number, stress FROM endings");
    sSelect += L" WHERE inflection_class = ";
    sSelect += CEString::sToString (ENDING_CLASS_SHORT_ADJECTIVE);
    sSelect += L" AND inflection_type = ";
    sSelect += CEString::sToString (m_pLexeme->iType());
    sSelect += L";";

    CSqlite * pDb = NULL;

    try
    {
        pDb = m_pLexeme->pGetDb();
        pDb->PrepareForSelect (sSelect);
        while (pDb->bGetRow())
        {
            CEString sEnding;
            StEndingDescriptor stD;
            pDb->GetData (0, sEnding);
            pDb->GetData (1, (int&)stD.eGender);
            pDb->GetData (2, (int&)stD.eNumber);
            pDb->GetData (3, (int&)stD.eStress);
            ET_ReturnCode rc = eAddEnding (sEnding, stD);
        }
        pDb->Finalize();
    }
    catch (...)
    {
        ReportDbError();    
        return H_ERROR_DB;
    }

    return rc;
        
}   //  eLoad()

int CAdjShortEndings::iHash (const StEndingDescriptor& stD)
{
    int iKey = stD.eGender * NUM_COUNT * STRESS_LOCATION_COUNT +
               stD.eNumber * STRESS_LOCATION_COUNT +
               stD.eStress;
    return iKey;
}

ET_ReturnCode CAdjShortEndings::eAddEnding (const CEString& sEnding, const StEndingDescriptor& stDescriptor)
{
    //
    // Gender
    //
    std::vector<ET_Gender> vecGender;
    if (GENDER_UNDEFINED == stDescriptor.eGender)
    {
        for (int iGen = (int)GENDER_UNDEFINED; iGen < GENDER_COUNT; ++iGen)
        {
            vecGender.push_back ((ET_Gender)iGen);
        }
    }
    else
    {
        vecGender.push_back (stDescriptor.eGender);
    }

    //
    // Number
    //
    if (NUM_UNDEFINED == stDescriptor.eNumber)
    {
        ERROR_LOG (L"Undefined number in short form ending.");
        return H_ERROR_INVALID_ARG;
    }

    //
    // Ending stressed/unstressed
    //
    std::vector<ET_StressLocation> vecStress;
    if (STRESS_LOCATION_UNDEFINED == stDescriptor.eStress)
    {
        for (int iStress = STRESS_LOCATION_UNDEFINED; 
             iStress < (int)STRESS_LOCATION_COUNT; 
             ++iStress)
        {
            vecStress.push_back ((ET_StressLocation)iStress);
        }
    }
    else
    {
        vecStress.push_back (stDescriptor.eStress);
    }

    for (int iG = 0; iG < (int)vecGender.size(); ++iG)
        for (int iS = 0; iS < (int)vecStress.size(); ++iS)
        {
            StEndingDescriptor stD (vecGender[iG], stDescriptor.eNumber, vecStress[iS]);
            int iKey = iHash (stD);
            m_mmEndings.insert (std::pair<int, CEString> (iKey, sEnding));
        }

    return H_NO_ERROR;

}   //  CAdjShortEndings::hAddEnding (...)


////////////////////////////////////////////////////////////////////////////////////

CPersonalEndings::CPersonalEndings(CLexeme * pLexeme) : CEndings(pLexeme)
{}

ET_ReturnCode CPersonalEndings::eLoad()
{
    ET_ReturnCode rc = H_NO_ERROR;

    if (POS_VERB != m_pLexeme->ePartOfSpeech())
    {
        ERROR_LOG (L"Bad part of speech value.");
        return H_ERROR_UNEXPECTED;
    }

    CEString sSelect (L"SELECT DISTINCT ending, person, number, stress, stem_auslaut, conjugation FROM endings");
    sSelect += L" WHERE inflection_class = " + CEString::sToString (ENDING_CLASS_PRESENT_TENSE);

    CSqlite * pDb = NULL;

    try
    {
        pDb = m_pLexeme->pGetDb();
        pDb->PrepareForSelect (sSelect);
        while (pDb->bGetRow())
        {
            CEString sEnding;
            StEndingDescriptor stD;
            pDb->GetData (0, sEnding);
            pDb->GetData (1, (int&)stD.ePerson);
            pDb->GetData (2, (int&)stD.eNumber);
            pDb->GetData (3, (int&)stD.eStress);
            pDb->GetData (4, (int&)stD.eStemAuslaut);
            pDb->GetData (5, stD.iInflectionType);
            rc = eAddEnding (sEnding, stD);
        }
        pDb->Finalize();
    }
    catch (...)
    {
        ReportDbError();    
        rc = H_EXCEPTION;
    }

    return rc;

}   //  eLoad()

int CPersonalEndings::iHash (const StEndingDescriptor& stD)
{
    int iKey = stD.iInflectionType * PERSON_COUNT * NUM_COUNT * STRESS_LOCATION_COUNT * STEM_AUSLAUT_COUNT +
               stD.ePerson * NUM_COUNT * STRESS_LOCATION_COUNT * STEM_AUSLAUT_COUNT +
               stD.eNumber * STRESS_LOCATION_COUNT * STEM_AUSLAUT_COUNT +
               stD.eStress * STEM_AUSLAUT_COUNT + stD.eStemAuslaut;
    return iKey;
}

ET_ReturnCode CPersonalEndings::eAddEnding (const CEString& sEnding, const StEndingDescriptor& stDescriptor)
{
    //
    // Person
    //
    std::vector<ET_Person> vecPerson;
    if (PERSON_UNDEFINED == stDescriptor.ePerson)
    {
        ERROR_LOG (L"Undefined person.");
        return H_ERROR_INVALID_ARG;
    }
    else
    {
        vecPerson.push_back (stDescriptor.ePerson);
    }

    //
    // Number
    //
    if (NUM_UNDEFINED == stDescriptor.eNumber)
    {
        ERROR_LOG (L"Undefined number in personal ending.");
        return H_ERROR_INVALID_ARG;
    }

    //
    // Ending stressed/unstressed
    //
    std::vector<ET_StressLocation> vecStress;
    if (STRESS_LOCATION_UNDEFINED == stDescriptor.eStress)
    {
        for (int iStress = STRESS_LOCATION_UNDEFINED; iStress < (int)STRESS_LOCATION_COUNT; ++iStress)
        {
            vecStress.push_back ((ET_StressLocation)iStress);
        }
    }
    else
    {
        vecStress.push_back (stDescriptor.eStress);
    }

    //
    // Ending after sh/not after sh
    //
    std::vector<ET_StemAuslaut> vecStemAuslaut;
    if (STEM_AUSLAUT_UNDEFINED == stDescriptor.eStemAuslaut)
    {
        for (int iStemAuslaut = STEM_AUSLAUT_UNDEFINED; iStemAuslaut < (int)STEM_AUSLAUT_COUNT; ++iStemAuslaut)
        {
            vecStemAuslaut.push_back ((ET_StemAuslaut)iStemAuslaut);
        }
    }
    else
    {
        vecStemAuslaut.push_back (stDescriptor.eStemAuslaut);
    }

    for (int iS = 0; iS < (int)vecStress.size(); ++iS)
        for (int iSa = 0; iSa < (int)vecStemAuslaut.size(); ++iSa)
        {
            StEndingDescriptor stD (stDescriptor.iInflectionType, 
                                    stDescriptor.ePerson, 
                                    stDescriptor.eNumber, 
                                    vecStress[iS], 
                                    vecStemAuslaut[iSa]);
            int iKey = iHash (stD);
            m_mmEndings.insert (std::pair<int, CEString> (iKey, sEnding));
        }

    return H_NO_ERROR;

}   //  CPersonalEndings::eAddEnding (...)
