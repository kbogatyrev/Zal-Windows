#pragma once

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#define _SILENCE_CXX17_STRSTREAM_DEPRECATION_WARNING

#define NOMINMAX

#include <memory>
#include <msclr\marshal.h>
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
#include "VerifierManaged.h"

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace MainLibManaged;
using namespace std;

CVerifierManaged::CVerifierManaged(shared_ptr<CVerifier> spVerifier) : m_pVerifier(&spVerifier)
{}

CVerifierManaged::~CVerifierManaged()
{}

//EM_ReturnCode CVerifierManaged::eSetHeadword(String^ sHeadword)
//{
//    if (nullptr == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return (EM_ReturnCode)m_pVerifier->eSetHeadword(sFromManagedString(sHeadword));
//}
//
//String^ CVerifierManaged::sGetHeadword()
//{
//    if (nullptr == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return gcnew String(m_pVerifier->sGetHeadword());
//}
//
//void CVerifierManaged::SetLexemeHash(String^ sHash)
//{
//    if (nullptr == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return m_pVerifier->SetLexemeHash(sFromManagedString(sHash));
//}
//
//String^ CVerifierManaged::sGetLexemeHash()
//{
//    if (nullptr == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return gcnew String(m_pVerifier->sGetLexemeHash());
//}

EM_ReturnCode CVerifierManaged::eVerify(String^ sLexemeHash)
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_ReturnCode)(*m_pVerifier)->eVerify(sFromManagedString(sLexemeHash));
}

EM_TestResult CVerifierManaged::eResult()
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_TestResult)(*m_pVerifier)->eResult();
}

int CVerifierManaged::iCount()
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (*m_pVerifier)->iCount();
}

EM_ReturnCode CVerifierManaged::eLoadStoredLexemes()
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_ReturnCode)(*m_pVerifier)->eLoadStoredLexemes();
}

EM_ReturnCode CVerifierManaged::eDeleteStoredLexeme(String^ sLexeme)
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_ReturnCode)(*m_pVerifier)->eDeleteStoredLexeme(sFromManagedString(sLexeme));
}

EM_ReturnCode CVerifierManaged::eGetFirstLexemeData(String^% sHash, String^% sHeadword)
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    CEString cestrHash;
    CEString cestrHeadword;
    ET_ReturnCode eRet = (*m_pVerifier)->eGetFirstLexemeData(cestrHash, cestrHeadword);
    if (H_NO_ERROR == eRet)
    {
        sHash = gcnew String(cestrHash);
        sHeadword = gcnew String(cestrHeadword);
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CVerifierManaged::eGetNextLexemeData(String^% sHash, String^% sHeadword)
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    CEString cestrHash;
    CEString cestrHeadword;
    ET_ReturnCode eRet = (*m_pVerifier)->eGetNextLexemeData(cestrHash, cestrHeadword);
    if (H_NO_ERROR == eRet)
    {
        sHash = gcnew String(cestrHash);
        sHeadword = gcnew String(cestrHeadword);
    }

    return (EM_ReturnCode)eRet;
}
