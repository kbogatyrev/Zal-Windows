#pragma once

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#define _SILENCE_CXX17_STRSTREAM_DEPRECATION_WARNING

#define NOMINMAX

#include <memory>
#include <msclr\marshal.h>
#include "Singleton.h"
#include "GramHasher.h"

#include "UtilsManaged.h"
#include "LexemeManaged.h"
#include "InflectionManaged.h"
#include "ParserManaged.h"
#include "AnalyticsManaged.h"
#include "VerifierManaged.h"
#include "DictionaryManaged.h"

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace MainLibManaged;
using namespace std;


namespace MainLibManaged
{
    CLexemeEnumeratorManaged::CLexemeEnumeratorManaged(int64_t iHandle) : m_iHandle(iHandle)
    {}

    CLexemeEnumeratorManaged::~CLexemeEnumeratorManaged()
    {
        m_iHandle = -1;
    }

    shared_ptr<CLexemeEnumerator> CLexemeEnumeratorManaged::spGetInstance()
    {
        shared_ptr<CLexemeEnumerator> spLexemeEnumerator;
        auto rc = Singleton::pGetInstance()->eGetLexemeEnumerator(m_iHandle, spLexemeEnumerator);
        if (rc != H_NO_ERROR || nullptr == spLexemeEnumerator)
        {
            throw gcnew Exception(L"Unable to retrieve inflection enumerator instance.");
        }
        return spLexemeEnumerator;
    }

    EM_ReturnCode CLexemeEnumeratorManaged::eReset()
    {
        return (EM_ReturnCode)spGetInstance()->eReset();
    }

    EM_ReturnCode CLexemeEnumeratorManaged::eGetFirstLexeme(CLexemeManaged^% lexeme)
    {
        shared_ptr<CLexeme> spLexeme;
        ET_ReturnCode eRet = spGetInstance()->eGetFirstLexeme(spLexeme);
        if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
        {
            if (spLexeme)
            {
                auto iHandle = Singleton::pGetInstance()->iAddLexeme(spLexeme);
                lexeme = gcnew CLexemeManaged(iHandle);
            }
        }
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CLexemeEnumeratorManaged::eGetNextLexeme(CLexemeManaged^% lexeme)
    {
        shared_ptr<CLexeme> spLexeme;
        ET_ReturnCode eRet = spGetInstance()->eGetNextLexeme(spLexeme);
        if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
        {
            if (spLexeme)
            {
                auto iHandle = Singleton::pGetInstance()->iAddLexeme(spLexeme);
                lexeme = gcnew CLexemeManaged(iHandle);
            }
        }
        return (EM_ReturnCode)eRet;
    }


 ////////////////////////////////////////////////////////////////////////////////////////////////


    CDictionaryManaged::~CDictionaryManaged()
    {}

    shared_ptr<CDictionary> CDictionaryManaged::spGetInstance()
    {
        shared_ptr<CDictionary> spDictionary;
        auto rc = Singleton::pGetInstance()->eGetDictionary(spDictionary);
        if (rc != H_NO_ERROR || nullptr == spDictionary)
        {
            throw gcnew Exception(L"Unable to retrieve dictionary instance.");
        }
        return spDictionary;
    }

    EM_ReturnCode CDictionaryManaged::eSetDbPath(String^ sDbPath)
    {
        return (EM_ReturnCode)spGetInstance()->eSetDbPath(sFromManagedString(sDbPath));
    }

    String^ CDictionaryManaged::sGetDbPath()
    {
        return gcnew String(spGetInstance()->sGetDbPath());
    }

    CLexemeManaged^ CDictionaryManaged::CreateLexemeForEdit()
    {
        shared_ptr<CLexeme> spLexeme;
//        shared_ptr<CInflection> spInflection;
        auto eRet = spGetInstance()->eCreateLexemeForEdit(spLexeme);
        if (eRet != H_NO_ERROR || nullptr == spLexeme)
        {
            throw gcnew Exception(L"Unable to create lexeme.");
        }

        auto iHandle = Singleton::pGetInstance()->iAddLexeme(spLexeme);

        CLexemeManaged^ pManaged = gcnew CLexemeManaged(iHandle);

        return pManaged;
    }

    EM_ReturnCode CDictionaryManaged::eCopyEntryForEdit(CLexemeManaged^ sourceL, CLexemeManaged^ targetL)
    {
        /*

            CLexeme * pCopyL = nullptr;
            CInflection * pCopyI = nullptr;
            ET_ReturnCode eRet = m_pDictionary->eCopyEntryForEdit(sourceL->sp, sourceI->pGetInflectionPtr(), pCopyL, pCopyI);
            if (eRet != H_NO_ERROR || nullptr == pCopyL || nullptr == pCopyI)
            {
                throw gcnew Exception(L"Unable to copy lexeme.");
            }

            CLexemeManaged^ pManagedL = gcnew CLexemeManaged();
            pManagedL->m_pLexeme = pCopyL;

            CInflectionManaged^ pManagedI = gcnew CInflectionManaged();
            pManagedI->m_pInflection = pCopyI;

            pManagedI->m_sStoredEntryHash = gcnew String(pCopyI->sHash());
        */
        return EM_ReturnCode::H_NO_ERROR;
    }

    EM_ReturnCode CDictionaryManaged::eGetLexemeById(long long llId, CLexemeManaged^% lexeme)
    {
        shared_ptr<CLexeme> spLexeme;
        auto eRet = spGetInstance()->eGetLexemeById(llId, spLexeme);
        if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
        {
            if (spLexeme)
            {
                lexeme = gcnew CLexemeManaged(Singleton::pGetInstance()->iAddLexeme(spLexeme));
            }
        }
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eGetLexemesByHash(String^ sHash)
    {
        return (EM_ReturnCode)spGetInstance()->eGetLexemesByHash(sFromManagedString(sHash));
    }

    /*
    EM_ReturnCode CDictionaryManaged::eGetLexemesByGraphicStem(String^ sGrStem)
    {
        if (NULL == m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        return (EM_ReturnCode)m_pDictionary->eGetLexemesByGraphicStem(sFromManagedString(sGrStem));
    }
    */

    EM_ReturnCode CDictionaryManaged::eGetLexemesByInitialForm(String^ sInitForm)
    {
        return (EM_ReturnCode)spGetInstance()->eGetLexemesByInitialForm(sFromManagedString(sInitForm));
    }

    EM_ReturnCode CDictionaryManaged::eGenerateAllForms()
    {
        return (EM_ReturnCode)spGetInstance()->eGenerateAllForms();
    }

    /*
    EM_ReturnCode CDictionaryManaged::eGenerateFormsForSelectedLexemes()
    {
        return (EM_ReturnCode)spGetInstance()->eGenerateFormsForSelectedLexemes();
    }
    */

    EM_ReturnCode CDictionaryManaged::eCountLexemes(Int64% riLexemes)
    {
        int64_t iLexemes = 0;
        ET_ReturnCode eRet = spGetInstance()->eCountLexemes(iLexemes);
        if (H_NO_ERROR == eRet)
        {
            riLexemes = iLexemes;
        }

        return (EM_ReturnCode)eRet;
    }

    //EM_ReturnCode eVerifyLexemeProperties(CLexemeManaged^);
    //EM_ReturnCode eSourceFormExists(CLexemeManaged^, bool%);

    /*
    EM_ReturnCode CDictionaryManaged::eSaveLexeme(CLexemeManaged^ l)
    {
       if (nullptr == m_pDictionary)
        //    {
        //        throw gcnew Exception(L"Dictionary object is NULL.");
        //    }
        //
        //    ET_ReturnCode eRet = m_pDictionary->eSaveLexeme(l->m_pLexeme);
        //
        //    return (EM_ReturnCode)eRet;
        //}
    */
    /*
    EM_ReturnCode CDictionaryManaged::eUpdateHeadword(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = m_pDictionary->eUpdateHeadword(l->pLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveNewHeadword(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = m_pDictionary->eSaveNewHeadword(l->pLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveHeadwordStress(CLexemeManaged^ l)
    {
       if (nullptr == m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = m_pDictionary->eSaveHeadwordStress(l->pLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveHomonyms(CLexemeManaged^ l)
    {
       if (nullptr == m_pDictionary 
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = m_pDictionary->eSaveHomonyms(l->pLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveAspectPairInfo(CLexemeManaged^ l)
    {
       if (nullptr == m_pDictionary 
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = m_pDictionary->eSaveAspectPairInfo(l->pLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveP2Info(CLexemeManaged^ l)
    {
       if (nullptr == m_pDictionary 
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = m_pDictionary->eSaveP2Info(l->pLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eUpdateDescriptorInfo(CLexemeManaged^ l)
    {
       if (nullptr == m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = m_pDictionary->eUpdateDescriptorInfo(l->pLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveDescriptorInfo(CLexemeManaged^ l)
    {
       if (nullptr == m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = m_pDictionary->eSaveDescriptorInfo(l->pLexeme());

        return (EM_ReturnCode)eRet;
    }
    */

    //EM_ReturnCode CDictionaryManaged::eSaveInflectionInfo(CInflectionManaged^ i)
    //{
    //   if (nullptr == m_pDictionary)
    //    {
    //        throw gcnew Exception(L"Dictionary object is NULL.");
    //    }

    //    ET_ReturnCode eRet = m_pDictionary->eSaveInflectionInfo(i->spInflection());

    //    return (EM_ReturnCode)eRet;
    //}

    //EM_ReturnCode CDictionaryManaged::eSaveCommonDeviation(CInflectionManaged^ i)
    //{
    //   if (nullptr == m_pDictionary)
    //    {
    //        throw gcnew Exception(L"Dictionary object is NULL.");
    //    }

    //    ET_ReturnCode eRet = m_pDictionary->eSaveCommonDeviation(i->spInflection());

    //    return (EM_ReturnCode)eRet;
    //}
/*
    EM_ReturnCode CDictionaryManaged::eDeleteLexeme(CLexemeManaged^ l)
    {
       if (nullptr == m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = m_pDictionary->eDeleteLexeme(l->pLexeme());

        return (EM_ReturnCode)eRet;
    }
*/
    int CDictionaryManaged::nLexemesFound()
    {
        return spGetInstance()->nLexemesFound();
    }

    void CDictionaryManaged::Clear()
    {
        spGetInstance()->Clear();
    }
/*
    EM_ReturnCode CDictionaryManaged::Clear(CLexemeManaged^ pLexeme)
    {
        if (nullptr == m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = m_pDictionary->Clear(pLexeme());

        return (EM_ReturnCode)eRet;
    }
*/

    EM_ReturnCode CDictionaryManaged::eCreateLexemeEnumerator(CLexemeEnumeratorManaged^% pLeManaged)
    {
        shared_ptr<CLexemeEnumerator> spLexemeEnumerator;
        ET_ReturnCode rc = spGetInstance()->eCreateLexemeEnumerator(spLexemeEnumerator);
        if (H_NO_ERROR == rc && spLexemeEnumerator)
        {
            auto iHandle = Singleton::pGetInstance()->iAddLexemeEnumerator(spLexemeEnumerator);
            pLeManaged = gcnew CLexemeEnumeratorManaged(iHandle);
        }
        else
        {
            return EM_ReturnCode::H_ERROR_UNEXPECTED;
        }
        return (EM_ReturnCode)rc;
    }

    EM_ReturnCode CDictionaryManaged::eGetParser(CParserManaged^% pParserManaged)
    {
        shared_ptr<CParser> spParser;
        ET_ReturnCode eRet = spGetInstance()->eGetParser(spParser);
        if (H_NO_ERROR == eRet)
        {
            if (spParser)
            {
                pParserManaged = gcnew CParserManaged(spParser);
            }
            else
            {
                return EM_ReturnCode::H_ERROR_UNEXPECTED;
            }
        }

        Singleton::pGetInstance()->SetParser(spParser);

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eGetAnalytics(CAnalyticsManaged^% pAnalyticsManaged)
    {
        shared_ptr<CAnalytics> spAnalytics;
        ET_ReturnCode eRet = spGetInstance()->eGetAnalytics(spAnalytics);
        if (H_NO_ERROR == eRet)
        {
            if (spAnalytics)
            {
                pAnalyticsManaged = gcnew CAnalyticsManaged(spAnalytics);
            }
            else
            {
                return EM_ReturnCode::H_ERROR_UNEXPECTED;
            }
        }
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eGetVerifier(CVerifierManaged^% verifier)
    {
        shared_ptr<CVerifier> spVerifier;
        ET_ReturnCode eRet = spGetInstance()->eGetVerifier(spVerifier);
        if (H_NO_ERROR == eRet)
        {
            if (spVerifier)
            {
                verifier = gcnew CVerifierManaged();
            }
            else
            {
                return EM_ReturnCode::H_ERROR_UNEXPECTED;
            }
        }
        return (EM_ReturnCode)eRet;
    }

/*
    EM_ReturnCode CDictionaryManaged::eExportTestData(String^ sPath, DelegateProgress^ progressCallback)
    {
       if (nullptr == m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        GCHandle gch = GCHandle::Alloc(progressCallback);
        IntPtr iptr = Marshal::GetFunctionPointerForDelegate(progressCallback);
        PROGRESS_CALLBACK_CLR pProgress = static_cast<PROGRESS_CALLBACK_CLR>(iptr.ToPointer());

        return (EM_ReturnCode)m_pDictionary->eExportTestData(sFromManagedString(sPath), *pProgress);
    }

    EM_ReturnCode CDictionaryManaged::eImportTestData(String^ sPath, DelegateProgress^ progressCallback)
    {
       if (nullptr == m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        GCHandle gch = GCHandle::Alloc(progressCallback);
        IntPtr iptr = Marshal::GetFunctionPointerForDelegate(progressCallback);
        PROGRESS_CALLBACK_CLR pProgress = static_cast<PROGRESS_CALLBACK_CLR>(iptr.ToPointer());

        return (EM_ReturnCode)m_pDictionary->eImportTestData(sFromManagedString(sPath), *pProgress);
    }
    */

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}       // namespace
