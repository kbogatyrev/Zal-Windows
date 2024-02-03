#pragma once

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#define _SILENCE_CXX17_STRSTREAM_DEPRECATION_WARNING

#define NOMINMAX

#include <memory>
#include <msclr\marshal.h>
#include "GramHasher.h"
#include "Singleton.h"

#include "UtilsManaged.h"
#include "LexemeManaged.h"
#include "WordFormManaged.h"
#include "InflectionManaged.h"

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
//using namespace MainLibManaged;
using namespace std;

namespace MainLibManaged
{

//    Singleton m_singleton;

    CInflectionManaged::CInflectionManaged(int64_t iHandle) : m_iHandle(iHandle)
    {}

    CInflectionManaged::~CInflectionManaged()
    {
        m_iHandle = -1;
    }

    CInflection* CInflectionManaged::pGetInstance()
    {
        CInflection* pInflection;
        auto rc = Singleton::pGetInstance()->eGetInflection(m_iHandle, pInflection);
        if (rc != H_NO_ERROR || nullptr == pInflection)
        {
            throw gcnew Exception(L"Unable to retrieve inflection instance.");
        }
        return pInflection;
    }

    long long CInflectionManaged::llInflectionId()
    {
        return pGetInstance()->llInflectionId();
    }

    void CInflectionManaged::SetInflectionId(int iValue)
    {
        pGetInstance()->SetInflectionId(iValue);
    }

    //void CInflectionManaged::SetLexeme(CLexemeManaged^ lexeme)
    //{
    //    m_pInflection->SetLexeme(lexeme->m_pLexeme);
    //}
    
    EM_ReturnCode CInflectionManaged::eGetLexeme(CLexemeManaged^% lexeme)
    {
        CLexeme* pLexeme;
        auto ret = pGetInstance()->eGetLexeme(pLexeme);
        if (ret != H_NO_ERROR || nullptr == pLexeme)
        {
            throw gcnew Exception(L"Failed to get parent lexeme.");
        }

        auto iHandle = Singleton::pGetInstance()->iAddLexeme(pLexeme);
        if (iHandle < 0)
        {
            throw gcnew Exception(L"Failed to add a lexeme.");
        }
        lexeme = gcnew CLexemeManaged(iHandle);
        return EM_ReturnCode::H_NO_ERROR;
    }

    bool CInflectionManaged::bPrimaryInflectionGroup()
    {
        return pGetInstance()->bPrimaryInflectionGroup();
    }

    void CInflectionManaged::SetPrimaryInflectionGroup(bool bValue)
    {
        return pGetInstance()->SetPrimaryInflectionGroup(bValue);
    }

    int CInflectionManaged::iType()
    {
        return pGetInstance()->iType();
    }

    void CInflectionManaged::SetType(int iValue)
    {
        return pGetInstance()->SetType(iValue);
    }

    bool CInflectionManaged::bHasFleetingVowel()
    {
        return pGetInstance()->bHasFleetingVowel();
    }

    void CInflectionManaged::SetHasFleetingVowel(bool bValue)
    {
        pGetInstance()->SetHasFleetingVowel(bValue);
    }

    EM_AccentType CInflectionManaged::eAccentType1()
    {
        return (EM_AccentType)pGetInstance()->eAccentType1();
    }

    void CInflectionManaged::SetAccentType1(EM_AccentType eValue)
    {
        pGetInstance()->SetAccentType1((ET_AccentType)eValue);
    }

    EM_AccentType CInflectionManaged::eAccentType2()
    {
        return (EM_AccentType)pGetInstance()->eAccentType2();
    }

    void CInflectionManaged::SetAccentType2(EM_AccentType eValue)
    {
        pGetInstance()->SetAccentType2((ET_AccentType)eValue);
    }

    bool CInflectionManaged::bShortFormsRestricted()
    {
        return pGetInstance()->bShortFormsRestricted();
    }

    void CInflectionManaged::SetShortFormsRestricted(bool bValue)
    {
        pGetInstance()->SetShortFormsRestricted(bValue);
    }

    bool CInflectionManaged::bPastParticipleRestricted()
    {
        return pGetInstance()->bPastParticipleRestricted();
    }

    void CInflectionManaged::SetPastParticipleRestricted(bool bValue)
    {
        pGetInstance()->SetPastParticipleRestricted(bValue);
    }

    bool CInflectionManaged::bShortFormsIncomplete()
    {
        return pGetInstance()->bShortFormsIncomplete();
    }

    void CInflectionManaged::SetShortFormsIncomplete(bool bValue)
    {
        pGetInstance()->SetShortFormsIncomplete(bValue);
    }

    bool CInflectionManaged::bNoPassivePastParticiple()
    {
        return pGetInstance()->bNoPassivePastParticiple();
    }

    void CInflectionManaged::SetNoPassivePastParticiple(bool bValue)
    {
        pGetInstance()->SetNoPassivePastParticiple(bValue);
    }

    bool CInflectionManaged::bFleetingVowel()
    {
        return pGetInstance()->bFleetingVowel();
    }

    void CInflectionManaged::SetFleetingVowel(bool bValue)
    {
        pGetInstance()->SetFleetingVowel(bValue);
    }

    int CInflectionManaged::iStemAugment()
    {
        return pGetInstance()->iStemAugment();
    }

    void CInflectionManaged::SetStemAugment(int iValue)
    {
        return pGetInstance()->SetStemAugment(iValue);
    }


    EM_ReturnCode CInflectionManaged::eAddCommonDeviation(int iValue, bool bIsOptional)
    {
        ET_ReturnCode eRet = pGetInstance()->eAddCommonDeviation(iValue, bIsOptional);

        return (EM_ReturnCode)eRet;
    }

    bool CInflectionManaged::bFindCommonDeviation(int iNum, bool% bIsOptionalDotNet)
    {
        bool bIsOptionalCpp = false;
        bool bRet = pGetInstance()->bFindCommonDeviation(iNum, bIsOptionalCpp);
        bIsOptionalDotNet = bIsOptionalCpp;

        return bRet;
    }

    void CInflectionManaged::ClearCommonDeviations()
    {
        pGetInstance()->ClearCommonDeviations();
    }

    String^ CInflectionManaged::sHash()
    {
        return gcnew String(pGetInstance()->sHash());
    }

    String^ CInflectionManaged::sStoredHash()
    {
        return m_sStoredEntryHash;
    }

    String^ CInflectionManaged::sParadigmHash()
    {
        return gcnew String(pGetInstance()->sParadigmHash());
    }

    EM_ReturnCode CInflectionManaged::eWordFormFromHash(String^ sHash, int iAt, CWordFormManaged^% wf)
    {
        CWordForm* pWf;
        ET_ReturnCode eRet = pGetInstance()->eWordFormFromHash(sFromManagedString(sHash), iAt, pWf);
        if (H_NO_ERROR == eRet)
        {
            if (pWf)
            {
                auto iHandle = Singleton::pGetInstance()->iAddWordForm(pWf);
                if (iHandle < 0)
                {
                    throw gcnew Exception(L"Failed to add a word form.");
                }
                wf = gcnew CWordFormManaged(iHandle);
            }
        }
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CInflectionManaged::eCreateWordForm(CWordFormManaged^% pWf)
    {
        shared_ptr<CWordForm> spIwf;
        ET_ReturnCode eRet = pGetInstance()->eCreateWordForm(spIwf);
        if (H_NO_ERROR == eRet && spIwf)
        {
            auto iHandle = Singleton::pGetInstance()->iAddWordForm(spIwf.get());
            if (iHandle < 0)
            {
                throw gcnew Exception(L"Failed to add a new form.");
            }
            auto rc = Singleton::pGetInstance()->eStoreCreatedWordForm(spIwf);
            if (rc != H_NO_ERROR)
            {
                throw gcnew Exception(L"Failed to store a new form.");
            }
            pWf = gcnew CWordFormManaged(iHandle);
        }

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CInflectionManaged::eRemoveWordForm(String^ sHash, int iAt)
    {
        ET_ReturnCode eRet = pGetInstance()->eRemoveWordForm(sFromManagedString(sHash), iAt);
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CInflectionManaged::eRemoveIrregularForms(String^ sHash)
    {
        ET_ReturnCode eRet = pGetInstance()->eRemoveIrregularForms(sFromManagedString(sHash));
        return (EM_ReturnCode)eRet;
    }

    void CInflectionManaged::AddModifiedForm(CWordFormManaged^% Wf)
    {
        CWordForm* pWordForm = Wf->pGetInstance();
        auto spWordForm = make_shared<CWordForm>(*pWordForm);
        pGetInstance()->AddModifiedForm(spWordForm);
    }

    bool CInflectionManaged::bHasIrregularForm(String^ sGramHash)
    {
        return pGetInstance()->bHasIrregularForm(sFromManagedString(sGramHash));
    }

    bool CInflectionManaged::bNoRegularForms(String^ sGramHash)
    {
        return pGetInstance()->bNoRegularForms(sFromManagedString(sGramHash));
    }

    EM_ReturnCode CInflectionManaged::eGetFirstWordForm(CWordFormManaged^% wf)
    {
        shared_ptr<CWordForm> spWf;
        ET_ReturnCode eRet = pGetInstance()->eGetFirstWordForm(spWf);
        if (H_NO_ERROR == eRet && spWf)
        {
            auto iHandle = Singleton::pGetInstance()->iAddWordForm(spWf.get());
            if (iHandle < 0)
            {
                throw gcnew Exception(L"Failed to add a word form.");
            }
            wf = gcnew CWordFormManaged(iHandle);
        }
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CInflectionManaged::eGetNextWordForm(CWordFormManaged^% wf)
    {
        shared_ptr<CWordForm> spWf;
        ET_ReturnCode eRet = pGetInstance()->eGetNextWordForm(spWf);
        if (H_NO_ERROR == eRet && spWf)
        {
            auto iHandle = Singleton::pGetInstance()->iAddWordForm(spWf.get());
            if (iHandle < 0)
            {
                throw gcnew Exception(L"Failed to add a word form.");
            }
            wf = gcnew CWordFormManaged(iHandle);
        }
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CInflectionManaged::eGetFirstIrregularForm(String^ sHash, CWordFormManaged^% wf, bool% bIsOptional)
    {
        shared_ptr<CWordForm> spWf;
        bool cppbIsOptional = false;
        ET_ReturnCode eRet = pGetInstance()->eGetFirstIrregularForm(sFromManagedString(sHash), spWf, cppbIsOptional);
        if (H_NO_ERROR == eRet)
        {
            auto iHandle = Singleton::pGetInstance()->iAddWordForm(spWf.get());
            if (iHandle < 0)
            {
                throw gcnew Exception(L"Failed to add an irregular form.");
            }
            wf = gcnew CWordFormManaged(iHandle);
            bIsOptional = cppbIsOptional;
        }
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CInflectionManaged::eGetNextIrregularForm(CWordFormManaged^% wf, bool% bIsOptional)
    {
        shared_ptr<CWordForm> spWf;
        bool cppbIsOptional = false;
        ET_ReturnCode eRet = pGetInstance()->eGetNextIrregularForm(spWf, cppbIsOptional);
        if (H_NO_ERROR == eRet)
        {
            if (spWf)
            {
                auto iHandle = Singleton::pGetInstance()->iAddWordForm(spWf.get());
                if (iHandle < 0)
                {
                    throw gcnew Exception(L"Failed to add an irregular form.");
                }
                wf = gcnew CWordFormManaged(iHandle);
                bIsOptional = cppbIsOptional;
            }
        }
        return (EM_ReturnCode)eRet;
    }

    int CInflectionManaged::iFormCount(String^ sHash)
    {
        return pGetInstance()->iFormCount(sFromManagedString(sHash));
    }

    bool CInflectionManaged::bHasCommonDeviation(int iCd)
    {
        return pGetInstance()->bHasCommonDeviation(iCd);
    }

    bool CInflectionManaged::bDeviationOptional(int iCd)
    {
        return pGetInstance()->bDeviationOptional(iCd);
    }

    EM_ReturnCode CInflectionManaged::eFormExists(String^ sGramHash)
    {
        return (EM_ReturnCode)pGetInstance()->eFormExists(sFromManagedString(sGramHash));
    }

    EM_ReturnCode CInflectionManaged::eSetFormExists(String^ sGramHash, bool bExists)
    {
        return (EM_ReturnCode)pGetInstance()->eSetFormExists(sFromManagedString(sGramHash), bExists);
    }

    EM_ReturnCode CInflectionManaged::eIsFormDifficult(String^ sGramHash)
    {
        return (EM_ReturnCode)pGetInstance()->eIsFormDifficult(sFromManagedString(sGramHash));
    }

    EM_ReturnCode CInflectionManaged::eSetFormDifficult(String^ sGramHash, bool bIsDifficult)
    {
        return (EM_ReturnCode)pGetInstance()->eSetFormDifficult(sFromManagedString(sGramHash), bIsDifficult);
    }

    EM_ReturnCode CInflectionManaged::eDifficultFormsHashes(List<String^>^% lstHashes)
    {
        vector<CEString> vecHashes;
        ET_ReturnCode eRet = pGetInstance()->eDifficultFormsHashes(vecHashes);
        if (ET_ReturnCode::H_NO_ERROR == eRet)
        {
            for (auto sHash : vecHashes)
            {
                lstHashes->Add(gcnew String(sHash));
            }
        }
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CInflectionManaged::eIsFormAssumed(String^ sGramHash)
    {
        return (EM_ReturnCode)pGetInstance()->eIsFormAssumed(sFromManagedString(sGramHash));
    }

    EM_ReturnCode CInflectionManaged::eSetHasAssumedForms(bool bIsAssumed)
    {
        return (EM_ReturnCode)pGetInstance()->eSetHasAssumedForms(bIsAssumed);
    }

    bool CInflectionManaged::bIsMultistressedCompound()
    {
        return pGetInstance()->bIsMultistressedCompound();
    }

    EM_ReturnCode CInflectionManaged::eGenerateParadigm()
    {
        return (EM_ReturnCode)pGetInstance()->eGenerateParadigm();
    }

    EM_ReturnCode CInflectionManaged::eSaveTestData()
    {
        return (EM_ReturnCode)pGetInstance()->eSaveTestData();
    }

    /*
    EM_ReturnCode CLexemeManaged::eCheckLexemeProperties() // for manual input/editing
    {
        if (nullptr == m_pLexeme)
        {
            throw gcnew Exception(L"Lexeme object is NULL.");
        }

        return (EM_ReturnCode)(*m_pLexeme)->eCheckLexemeProperties();
    }
    */

    EM_ReturnCode CInflectionManaged::eDeleteIrregularForm(String^ sFormHash)
    {
        return (EM_ReturnCode)pGetInstance()->eDeleteIrregularForm(sFromManagedString(sFormHash));
    }

    EM_ReturnCode CInflectionManaged::eSaveIrregularForms(String^ sGramHash)
    {
        auto pSingleton = Singleton::pGetInstance();
        if (pSingleton->iNCreatedForms() < 1)
        {
            return (EM_ReturnCode)H_FALSE;     // Error message??
        }

        auto spInflection = pGetInstance();
        if (nullptr == spInflection)
        {
            throw gcnew Exception(L"Inflection object is NULL.");
        }

        shared_ptr<CWordForm> spStoredForm;
        auto rc = pSingleton->eGetFirstCreatedWordForm(spStoredForm);
        if (rc != H_NO_ERROR)
        {
            throw gcnew Exception(L"Unable to retrieve first created form.");
        }

        while (H_NO_ERROR == rc)
        {
            spInflection->AddModifiedForm(spStoredForm);
            rc = pSingleton->eGetNextCreatedWordForm(spStoredForm);
        }

        pSingleton->ClearCreatedForms();

        return (EM_ReturnCode)spInflection->eSaveIrregularForms(sFromManagedString(sGramHash));
    }

    EM_ReturnCode CInflectionManaged::eMakeGraphicStem()
    {
        auto spInflection = pGetInstance();
        if (nullptr == spInflection)
        {
            throw gcnew Exception(L"Inflection object is NULL.");
        }

        auto eRet = spInflection->eMakeGraphicStem();
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CInflectionManaged::eMakeGraphicStem(const String^ sSource, String^% sGraphicStem)
    {
        CEString sGs;
        auto eRet = pGetInstance()->eMakeGraphicStem(sFromManagedString(sSource), sGs);
        if (eRet != H_NO_ERROR)
        {
            return (EM_ReturnCode)eRet;
        }

        sGraphicStem = gcnew String(sGs);
        return (EM_ReturnCode)eRet;
    }

}       // namespace MainLibManaged
