#pragma once

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#define _SILENCE_CXX17_STRSTREAM_DEPRECATION_WARNING

#define NOMINMAX

#include <memory>
#include <msclr\marshal.h>
#include "GramHasher.h"
#include "Singleton.h"
#include "UtilsManaged.h"
#include "VerifierManaged.h"

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace MainLibManaged;
using namespace std;

CVerifierManaged::CVerifierManaged()
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

shared_ptr<CVerifier> CVerifierManaged::spGetInstance()
{
    shared_ptr<CVerifier> spVerifier;
    auto rc = Singleton::pGetInstance()->eGetVerifier(spVerifier);
    if (rc != H_NO_ERROR || nullptr == spVerifier)
    {
        throw gcnew Exception(L"Unable to retrieve verifier instance.");
    }
    return spVerifier;
}

EM_ReturnCode CVerifierManaged::eVerify(String^ sLexemeHash)
{
    auto spVerifier = spGetInstance();
    if (nullptr == spVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }
    return (EM_ReturnCode)spVerifier->eVerify(sFromManagedString(sLexemeHash));
}

EM_TestResult CVerifierManaged::eResult()
{
    auto spVerifier = spGetInstance();
    if (nullptr == spVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }
    return (EM_TestResult)spVerifier->eResult();
}

int CVerifierManaged::iCount()
{
    auto spVerifier = spGetInstance();
    if (nullptr == spVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }
    return spVerifier->iCount();
}

EM_ReturnCode CVerifierManaged::eLoadStoredLexemes()
{
    auto spVerifier = spGetInstance();
    if (nullptr == spVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }
    return (EM_ReturnCode)spVerifier->eLoadStoredLexemes();
}

EM_ReturnCode CVerifierManaged::eDeleteStoredLexeme(String^ sLexeme)
{
    auto spVerifier = spGetInstance();
    if (nullptr == spVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }
    return (EM_ReturnCode)spVerifier->eDeleteStoredLexeme(sFromManagedString(sLexeme));
}

EM_ReturnCode CVerifierManaged::eGetFirstLexemeData(String^% sHash, String^% sHeadword)
{
    auto spVerifier = spGetInstance();
    if (nullptr == spVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    CEString cestrHash;
    CEString cestrHeadword;
    ET_ReturnCode eRet = spVerifier->eGetFirstLexemeData(cestrHash, cestrHeadword);
    if (H_NO_ERROR == eRet)
    {
        sHash = gcnew String(cestrHash);
        sHeadword = gcnew String(cestrHeadword);
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CVerifierManaged::eGetNextLexemeData(String^% sHash, String^% sHeadword)
{
    auto spVerifier = spGetInstance();
    if (nullptr == spVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    CEString cestrHash;
    CEString cestrHeadword;
    ET_ReturnCode eRet = spVerifier->eGetNextLexemeData(cestrHash, cestrHeadword);
    if (H_NO_ERROR == eRet)
    {
        sHash = gcnew String(cestrHash);
        sHeadword = gcnew String(cestrHeadword);
    }
    return (EM_ReturnCode)eRet;
}
