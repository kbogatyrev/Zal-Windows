#pragma once

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#define _SILENCE_CXX17_STRSTREAM_DEPRECATION_WARNING

#define NOMINMAX

#include <memory>
#include <msclr\marshal.h>
#include "MainLib.h"
#include "GramHasher.h"
//#include "MainLibManaged.h"

//#include "Dictionary.h"
//#include "Lexeme.h"
//#include "Inflection.h"
//#include "WordForm.h"
//#include "Gramhasher.h"
//#include "Parser.h"
//#include "Analytics.h"
//#include "Verifier.h"
//#include "AspectPair.h"
//#include "..\Hlib\SqliteWrapper.h"
//#include "EnumsManaged.h"

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
/*
    extern "C"
    {
        ET_ReturnCode GetDictionary(shared_ptr<CDictionary>*&);        // the only external function defined in MainLib
    }
*/
    Singleton m_singleton;

    CDictionaryManaged::CDictionaryManaged()
    {
        shared_ptr<Hlib::CDictionary> pD;
        ET_ReturnCode rc = m_singleton.GetDictionary(pD);
        m_pDictionary = &pD;
    }

    CDictionaryManaged::~CDictionaryManaged()
    {
        //    It has a smart pointer wrapper now
        //    delete m_pDictionary;
    }

    EM_ReturnCode CDictionaryManaged::eSetDbPath(String^ sDbPath)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        return (EM_ReturnCode)(*m_pDictionary)->eSetDbPath(sFromManagedString(sDbPath));
    }

    String^ CDictionaryManaged::sGetDbPath()
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        return gcnew String((*m_pDictionary)->sGetDbPath());
    }

    CLexemeManaged^ CDictionaryManaged::CreateLexemeForEdit()
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary itf handle is NULL.");
        }

        shared_ptr<CLexeme> spLexeme;
        shared_ptr<CInflection> spInflection;
        ET_ReturnCode eRet = (*m_pDictionary)->eCreateLexemeForEdit(spLexeme, spInflection);
        if (eRet != H_NO_ERROR || nullptr == spLexeme)
        {
            throw gcnew Exception(L"Unable to create lexeme.");
        }

        CLexemeManaged^ pManaged = gcnew CLexemeManaged();
        //    pManaged->m_pLexeme = pLexeme;

        return pManaged;
    }

    EM_ReturnCode CDictionaryManaged::eCopyEntryForEdit(CLexemeManaged^ sourceL, CInflectionManaged^ sourceI,
        CLexemeManaged^ targetL, CInflectionManaged^ targetI)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary itf handle is NULL.");
        }
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
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        shared_ptr<CLexeme> spLexeme;
        ET_ReturnCode eRet = (*m_pDictionary)->eGetLexemeById(llId, spLexeme);
        if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
        {
            if (spLexeme)
            {
                lexeme = gcnew CLexemeManaged(spLexeme);
            }
        }
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eGetLexemesByHash(String^ sHash)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        return (EM_ReturnCode)(*m_pDictionary)->eGetLexemesByHash(sFromManagedString(sHash));
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
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        return (EM_ReturnCode)(*m_pDictionary)->eGetLexemesByInitialForm(sFromManagedString(sInitForm));
    }

    EM_ReturnCode CDictionaryManaged::eGenerateAllForms()
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        return (EM_ReturnCode)(*m_pDictionary)->eGenerateAllForms();
    }

    /*
    EM_ReturnCode CDictionaryManaged::eGenerateFormsForSelectedLexemes()
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        return (EM_ReturnCode)m_pDictionary->eGenerateFormsForSelectedLexemes();
    }
    */

    EM_ReturnCode CDictionaryManaged::eCountLexemes(Int64% iLexemes)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        int64_t cppiLexemes = 0;
        ET_ReturnCode eRet = (*m_pDictionary)->eCountLexemes(cppiLexemes);
        if (H_NO_ERROR == eRet)
        {
            iLexemes = cppiLexemes;
        }

        return (EM_ReturnCode)eRet;
    }

    //EM_ReturnCode eVerifyLexemeProperties(CLexemeManaged^);
    //EM_ReturnCode eSourceFormExists(CLexemeManaged^, bool%);

    /*
    EM_ReturnCode CDictionaryManaged::eSaveLexeme(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        //    {
        //        throw gcnew Exception(L"Dictionary object is NULL.");
        //    }
        //
        //    ET_ReturnCode eRet = m_pDictionary->eSaveLexeme(l->m_pLexeme);
        //
        //    return (EM_ReturnCode)eRet;
        //}
    */

    EM_ReturnCode CDictionaryManaged::eUpdateHeadword(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->eUpdateHeadword(l->spLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveNewHeadword(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->eSaveNewHeadword(l->spLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveHeadwordStress(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->eSaveHeadwordStress(l->spLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveHomonyms(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->eSaveHomonyms(l->spLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveAspectPairInfo(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->eSaveAspectPairInfo(l->spLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveP2Info(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->eSaveP2Info(l->spLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eUpdateDescriptorInfo(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->eUpdateDescriptorInfo(l->spLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveDescriptorInfo(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->eSaveDescriptorInfo(l->spLexeme());

        return (EM_ReturnCode)eRet;
    }


    EM_ReturnCode CDictionaryManaged::eSaveInflectionInfo(CInflectionManaged^ i)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->eSaveInflectionInfo(i->spInflection());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eSaveCommonDeviation(CInflectionManaged^ i)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->eSaveCommonDeviation(i->spInflection());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eDeleteLexeme(CLexemeManaged^ l)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->eDeleteLexeme(l->spLexeme());

        return (EM_ReturnCode)eRet;
    }

    int CDictionaryManaged::nLexemesFound()
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        return (*m_pDictionary)->nLexemesFound();
    }

    void CDictionaryManaged::Clear()
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        (*m_pDictionary)->Clear();
    }

    EM_ReturnCode CDictionaryManaged::Clear(CLexemeManaged^ pLexeme)
    {
        if (nullptr == m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        ET_ReturnCode eRet = (*m_pDictionary)->Clear(pLexeme->spLexeme());

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eCreateLexemeEnumerator(CLexemeEnumeratorManaged^% pLeManaged)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary objectis NULL.");
        }

        shared_ptr<CLexemeEnumerator> spLexemeEnumerator;
        ET_ReturnCode eRet = (*m_pDictionary)->eCreateLexemeEnumerator(spLexemeEnumerator);
        if (H_NO_ERROR == eRet)
        {
            if (spLexemeEnumerator)
            {
                pLeManaged = gcnew CLexemeEnumeratorManaged(spLexemeEnumerator);
            }
            else
            {
                return EM_ReturnCode::H_ERROR_UNEXPECTED;
            }
        }
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eGetParser(CParserManaged^% pParserManaged)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        shared_ptr<CParser> spParser;
        ET_ReturnCode eRet = (*m_pDictionary)->eGetParser(spParser);
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

        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CDictionaryManaged::eGetAnalytics(CAnalyticsManaged^% pAnalyticsManaged)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        shared_ptr<CAnalytics> spAnalytics;
        ET_ReturnCode eRet = (*m_pDictionary)->eGetAnalytics(spAnalytics);
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
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        shared_ptr<CVerifier> spVerifier;
        ET_ReturnCode eRet = (*m_pDictionary)->eGetVerifier(spVerifier);
        if (H_NO_ERROR == eRet)
        {
            if (spVerifier)
            {
                verifier = gcnew CVerifierManaged(spVerifier);
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
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        GCHandle gch = GCHandle::Alloc(progressCallback);
        IntPtr iptr = Marshal::GetFunctionPointerForDelegate(progressCallback);
        PROGRESS_CALLBACK_CLR pProgress = static_cast<PROGRESS_CALLBACK_CLR>(iptr.ToPointer());

        return (EM_ReturnCode)(*m_pDictionary)->eExportTestData(sFromManagedString(sPath), *pProgress);
    }

    EM_ReturnCode CDictionaryManaged::eImportTestData(String^ sPath, DelegateProgress^ progressCallback)
    {
        if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
        {
            throw gcnew Exception(L"Dictionary object is NULL.");
        }

        GCHandle gch = GCHandle::Alloc(progressCallback);
        IntPtr iptr = Marshal::GetFunctionPointerForDelegate(progressCallback);
        PROGRESS_CALLBACK_CLR pProgress = static_cast<PROGRESS_CALLBACK_CLR>(iptr.ToPointer());

        return (EM_ReturnCode)(*m_pDictionary)->eImportTestData(sFromManagedString(sPath), *pProgress);
    }
    */
}
