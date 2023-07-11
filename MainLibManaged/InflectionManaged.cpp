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

    shared_ptr<CInflection> CInflectionManaged::spGetInstance()
    {
        shared_ptr<CInflection> spInflection;
        auto rc = Singleton::pGetInstance()->eGetInflection(m_iHandle, spInflection);
        if (rc != H_NO_ERROR || nullptr == spInflection)
        {
            throw gcnew Exception(L"Unable to retrieve inflection instance.");
        }
        return spInflection;
    }

    long long CInflectionManaged::llInflectionId()
    {
        return spGetInstance()->llInflectionId();
    }

    void CInflectionManaged::SetInflectionId(int iValue)
    {
        spGetInstance()->SetInflectionId(iValue);
    }

    //void CInflectionManaged::SetLexeme(CLexemeManaged^ lexeme)
    //{
    //    m_pInflection->SetLexeme(lexeme->m_pLexeme);
    //}
    
    EM_ReturnCode CInflectionManaged::eGetLexeme(CLexemeManaged^% lexeme)
    {
        shared_ptr<CLexeme> spLexeme;
        auto ret = spGetInstance()->eGetLexeme(spLexeme);
        if (ret != H_NO_ERROR || nullptr == spLexeme)
        {
            throw gcnew Exception(L"Failed to get parent lexeme.");
        }

        auto iHandle = Singleton::pGetInstance()->iAddLexeme(spLexeme);
        if (iHandle < 0)
        {
            throw gcnew Exception(L"Failed to add a lexeme.");
        }
        lexeme = gcnew CLexemeManaged(iHandle);
        return EM_ReturnCode::H_NO_ERROR;
    }

    bool CInflectionManaged::bPrimaryInflectionGroup()
    {
        return spGetInstance()->bPrimaryInflectionGroup();
    }

    void CInflectionManaged::SetPrimaryInflectionGroup(bool bValue)
    {
        return spGetInstance()->SetPrimaryInflectionGroup(bValue);
    }

    int CInflectionManaged::iType()
    {
        return spGetInstance()->iType();
    }

    void CInflectionManaged::SetType(int iValue)
    {
        return spGetInstance()->SetType(iValue);
    }

    bool CInflectionManaged::bHasFleetingVowel()
    {
        return spGetInstance()->bHasFleetingVowel();
    }

    void CInflectionManaged::SetHasFleetingVowel(bool bValue)
    {
        spGetInstance()->SetHasFleetingVowel(bValue);
    }

    EM_AccentType CInflectionManaged::eAccentType1()
    {
        return (EM_AccentType)spGetInstance()->eAccentType1();
    }

    void CInflectionManaged::SetAccentType1(EM_AccentType eValue)
    {
        spGetInstance()->SetAccentType1((ET_AccentType)eValue);
    }

    EM_AccentType CInflectionManaged::eAccentType2()
    {
        return (EM_AccentType)spGetInstance()->eAccentType2();
    }

    void CInflectionManaged::SetAccentType2(EM_AccentType eValue)
    {
        spGetInstance()->SetAccentType2((ET_AccentType)eValue);
    }

    bool CInflectionManaged::bShortFormsRestricted()
    {
        return spGetInstance()->bShortFormsRestricted();
    }

    void CInflectionManaged::SetShortFormsRestricted(bool bValue)
    {
        spGetInstance()->SetShortFormsRestricted(bValue);
    }

    bool CInflectionManaged::bPastParticipleRestricted()
    {
        return spGetInstance()->bPastParticipleRestricted();
    }

    void CInflectionManaged::SetPastParticipleRestricted(bool bValue)
    {
        spGetInstance()->SetPastParticipleRestricted(bValue);
    }

    bool CInflectionManaged::bShortFormsIncomplete()
    {
        return spGetInstance()->bShortFormsIncomplete();
    }

    void CInflectionManaged::SetShortFormsIncomplete(bool bValue)
    {
        spGetInstance()->SetShortFormsIncomplete(bValue);
    }

    bool CInflectionManaged::bNoPassivePastParticiple()
    {
        return spGetInstance()->bNoPassivePastParticiple();
    }

    void CInflectionManaged::SetNoPassivePastParticiple(bool bValue)
    {
        spGetInstance()->SetNoPassivePastParticiple(bValue);
    }

    bool CInflectionManaged::bFleetingVowel()
    {
        return spGetInstance()->bFleetingVowel();
    }

    void CInflectionManaged::SetFleetingVowel(bool bValue)
    {
        spGetInstance()->SetFleetingVowel(bValue);
    }

    int CInflectionManaged::iStemAugment()
    {
        return spGetInstance()->iStemAugment();
    }

    void CInflectionManaged::SetStemAugment(int iValue)
    {
        return spGetInstance()->SetStemAugment(iValue);
    }


    EM_ReturnCode CInflectionManaged::eAddCommonDeviation(int iValue, bool bIsOptional)
    {
        ET_ReturnCode eRet = spGetInstance()->eAddCommonDeviation(iValue, bIsOptional);

        return (EM_ReturnCode)eRet;
    }

    bool CInflectionManaged::bFindCommonDeviation(int iNum, bool% bIsOptionalDotNet)
    {
        bool bIsOptionalCpp = false;
        bool bRet = spGetInstance()->bFindCommonDeviation(iNum, bIsOptionalCpp);
        bIsOptionalDotNet = bIsOptionalCpp;

        return bRet;
    }

    void CInflectionManaged::ClearCommonDeviations()
    {
        spGetInstance()->ClearCommonDeviations();
    }

    String^ CInflectionManaged::sHash()
    {
        return gcnew String(spGetInstance()->sHash());
    }

    String^ CInflectionManaged::sStoredHash()
    {
        return m_sStoredEntryHash;
    }

    String^ CInflectionManaged::sParadigmHash()
    {
        return gcnew String(spGetInstance()->sParadigmHash());
    }

    EM_ReturnCode CInflectionManaged::eWordFormFromHash(String^ sHash, int iAt, CWordFormManaged^% wf)
    {
        shared_ptr<CWordForm> spWf;
        ET_ReturnCode eRet = spGetInstance()->eWordFormFromHash(sFromManagedString(sHash), iAt, spWf);
        if (H_NO_ERROR == eRet)
        {
            if (spWf)
            {
                auto iHandle = Singleton::pGetInstance()->iAddWordForm(spWf);
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
        ET_ReturnCode eRet = spGetInstance()->eCreateWordForm(spIwf);
        if (H_NO_ERROR == eRet && spIwf)
        {
            auto iHandle = Singleton::pGetInstance()->iAddWordForm(spIwf);
            if (iHandle < 0)
            {
                throw gcnew Exception(L"Failed to add a word form.");
            }
            pWf = gcnew CWordFormManaged(iHandle);
        }

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CInflectionManaged::eRemoveWordForm(String^ sHash, int iAt)
    {
        ET_ReturnCode eRet = spGetInstance()->eRemoveWordForm(sFromManagedString(sHash), iAt);
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CInflectionManaged::eRemoveIrregularForms(String^ sHash)
    {
        ET_ReturnCode eRet = spGetInstance()->eRemoveIrregularForms(sFromManagedString(sHash));
        return (EM_ReturnCode)eRet;
    }

    void CInflectionManaged::AddModifiedForm(CWordFormManaged^% Wf)
    {
        shared_ptr<CWordForm> spWordForm = Wf->spGetInstance();
        spGetInstance()->AddModifiedForm(spWordForm);
    }

    bool CInflectionManaged::bHasIrregularForm(String^ sGramHash)
    {
        return spGetInstance()->bHasIrregularForm(sFromManagedString(sGramHash));
    }

    bool CInflectionManaged::bNoRegularForms(String^ sGramHash)
    {
        return spGetInstance()->bNoRegularForms(sFromManagedString(sGramHash));
    }

    EM_ReturnCode CInflectionManaged::eGetFirstWordForm(CWordFormManaged^% wf)
    {
        shared_ptr<CWordForm> spWf;
        ET_ReturnCode eRet = spGetInstance()->eGetFirstWordForm(spWf);
        if (H_NO_ERROR == eRet && spWf)
        {
            auto iHandle = Singleton::pGetInstance()->iAddWordForm(spWf);
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
        ET_ReturnCode eRet = spGetInstance()->eGetNextWordForm(spWf);
        if (H_NO_ERROR == eRet && spWf)
        {
            auto iHandle = Singleton::pGetInstance()->iAddWordForm(spWf);
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
        ET_ReturnCode eRet = spGetInstance()->eGetFirstIrregularForm(sFromManagedString(sHash), spWf, cppbIsOptional);
        if (H_NO_ERROR == eRet)
        {
            auto iHandle = Singleton::pGetInstance()->iAddWordForm(spWf);
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
        ET_ReturnCode eRet = spGetInstance()->eGetNextIrregularForm(spWf, cppbIsOptional);
        if (H_NO_ERROR == eRet)
        {
            if (spWf)
            {
                auto iHandle = Singleton::pGetInstance()->iAddWordForm(spWf);
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
        return spGetInstance()->iFormCount(sFromManagedString(sHash));
    }

    bool CInflectionManaged::bHasCommonDeviation(int iCd)
    {
        return spGetInstance()->bHasCommonDeviation(iCd);
    }

    bool CInflectionManaged::bDeviationOptional(int iCd)
    {
        return spGetInstance()->bDeviationOptional(iCd);
    }

    EM_ReturnCode CInflectionManaged::eFormExists(String^ sGramHash)
    {
        return (EM_ReturnCode)spGetInstance()->eFormExists(sFromManagedString(sGramHash));
    }

    EM_ReturnCode CInflectionManaged::eSetFormExists(String^ sGramHash, bool bExists)
    {
        return (EM_ReturnCode)spGetInstance()->eSetFormExists(sFromManagedString(sGramHash), bExists);
    }

    EM_ReturnCode CInflectionManaged::eIsFormDifficult(String^ sGramHash)
    {
        return (EM_ReturnCode)spGetInstance()->eIsFormDifficult(sFromManagedString(sGramHash));
    }

    EM_ReturnCode CInflectionManaged::eSetFormDifficult(String^ sGramHash, bool bIsDifficult)
    {
        return (EM_ReturnCode)spGetInstance()->eSetFormDifficult(sFromManagedString(sGramHash), bIsDifficult);
    }

    EM_ReturnCode CInflectionManaged::eDifficultFormsHashes(List<String^>^% lstHashes)
    {
        vector<CEString> vecHashes;
        ET_ReturnCode eRet = spGetInstance()->eDifficultFormsHashes(vecHashes);
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
        return (EM_ReturnCode)spGetInstance()->eIsFormAssumed(sFromManagedString(sGramHash));
    }

    EM_ReturnCode CInflectionManaged::eSetHasAssumedForms(bool bIsAssumed)
    {
        return (EM_ReturnCode)spGetInstance()->eSetHasAssumedForms(bIsAssumed);
    }

    bool CInflectionManaged::bIsMultistressedCompound()
    {
        return spGetInstance()->bIsMultistressedCompound();
    }

    EM_ReturnCode CInflectionManaged::eGenerateParadigm()
    {
        return (EM_ReturnCode)spGetInstance()->eGenerateParadigm();
    }

    EM_ReturnCode CInflectionManaged::eSaveTestData()
    {
        return (EM_ReturnCode)spGetInstance()->eSaveTestData();
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
        return (EM_ReturnCode)spGetInstance()->eDeleteIrregularForm(sFromManagedString(sFormHash));
    }

    EM_ReturnCode CInflectionManaged::eSaveIrregularForms(String^ sGramHash)
    {
        auto spInflection = spGetInstance();
        if (nullptr == spInflection)
        {
            throw gcnew Exception(L"Inflection object is NULL.");
        }
        return (EM_ReturnCode)spInflection->eSaveIrregularForms(sFromManagedString(sGramHash));
    }

    EM_ReturnCode CInflectionManaged::eMakeGraphicStem()
    {
        auto spInflection = spGetInstance();
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
        auto eRet = spGetInstance()->eMakeGraphicStem(sFromManagedString(sSource), sGs);
        if (eRet != H_NO_ERROR)
        {
            return (EM_ReturnCode)eRet;
        }

        sGraphicStem = gcnew String(sGs);
        return (EM_ReturnCode)eRet;
    }

}       // namespace MainLibManaged
