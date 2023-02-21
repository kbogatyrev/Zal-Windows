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
#include "LexemeManaged.h"
#include "InflectionManaged.h"

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace MainLibManaged;
using namespace std;

CInflectionManaged::CInflectionManaged()
{}

//CInflectionManaged::CInflectionManaged(String^ sGramHash)
//{
//    m_sStoredEntryHash = "";
//}

CInflectionManaged::CInflectionManaged(shared_ptr<CLexeme> pLexeme)
{
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme pointer is NULL.");
    }
}

CInflectionManaged::CInflectionManaged(shared_ptr<CInflection> spInflection) : m_pInflection(&spInflection)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection pointer is NULL.");
    }
}

CInflectionManaged::~CInflectionManaged()
{}

shared_ptr<CInflection> CInflectionManaged::spInflection()
{
    return *m_pInflection;
}

long long CInflectionManaged::llInflectionId()
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (*m_pInflection)->llInflectionId();
}

void CInflectionManaged::SetInflectionId(int iValue)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    (*m_pInflection)->SetInflectionId(iValue);
}

void CInflectionManaged::SetLexeme(CLexemeManaged^ lexeme)
{
    (*m_pInflection)->SetLexeme(*lexeme->m_pLexeme);
}

EM_ReturnCode CInflectionManaged::eGetLexeme(CLexemeManaged^% lexeme)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    shared_ptr<CLexeme> spLexeme;
    auto ret = (*m_pInflection)->eGetLexeme(spLexeme);
    if (ret != H_NO_ERROR || nullptr == spLexeme)
    {
        throw gcnew Exception(L"Failed to get ILexeme ITF.");
    }

    lexeme = gcnew CLexemeManaged(spLexeme);
    return EM_ReturnCode::H_NO_ERROR;
}

bool CInflectionManaged::bPrimaryInflectionGroup()
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pInflection)->bPrimaryInflectionGroup();
}

void CInflectionManaged::SetPrimaryInflectionGroup(bool bValue)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pInflection)->SetPrimaryInflectionGroup(bValue);
}

int CInflectionManaged::iType()
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (*m_pInflection)->iType();
}

void CInflectionManaged::SetType(int iValue)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (*m_pInflection)->SetType(iValue);
}

bool CInflectionManaged::bHasFleetingVowel()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (*m_pInflection)->bHasFleetingVowel();
}

void CInflectionManaged::SetHasFleetingVowel(bool bValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    (*m_pInflection)->SetHasFleetingVowel(bValue);
}

EM_AccentType CInflectionManaged::eAccentType1()
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (EM_AccentType)(*m_pInflection)->eAccentType1();
}

void CInflectionManaged::SetAccentType1(EM_AccentType eValue)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    (*m_pInflection)->SetAccentType1((ET_AccentType)eValue);
}

EM_AccentType CInflectionManaged::eAccentType2()
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (EM_AccentType)(*m_pInflection)->eAccentType2();
}

void CInflectionManaged::SetAccentType2(EM_AccentType eValue)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    (*m_pInflection)->SetAccentType2((ET_AccentType)eValue);
}

bool CInflectionManaged::bShortFormsRestricted()
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (*m_pInflection)->bShortFormsRestricted();
}

void CInflectionManaged::SetShortFormsRestricted(bool bValue)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    (*m_pInflection)->SetShortFormsRestricted(bValue);
}

bool CInflectionManaged::bPastParticipleRestricted()
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (*m_pInflection)->bPastParticipleRestricted();
}

void CInflectionManaged::SetPastParticipleRestricted(bool bValue)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    (*m_pInflection)->SetPastParticipleRestricted(bValue);
}

bool CInflectionManaged::bShortFormsIncomplete()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (*m_pInflection)->bShortFormsIncomplete();
}

void CInflectionManaged::SetShortFormsIncomplete(bool bValue)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    (*m_pInflection)->SetShortFormsIncomplete(bValue);
}

bool CInflectionManaged::bNoPassivePastParticiple()
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (*m_pInflection)->bNoPassivePastParticiple();
}

void CInflectionManaged::SetNoPassivePastParticiple(bool bValue)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    (*m_pInflection)->SetNoPassivePastParticiple(bValue);
}

bool CInflectionManaged::bFleetingVowel()
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (*m_pInflection)->bFleetingVowel();
}

void CInflectionManaged::SetFleetingVowel(bool bValue)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    (*m_pInflection)->SetFleetingVowel(bValue);
}

int CInflectionManaged::iStemAugment()
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (*m_pInflection)->iStemAugment();
}

void CInflectionManaged::SetStemAugment(int iValue)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (*m_pInflection)->SetStemAugment(iValue);
}

EM_ReturnCode CInflectionManaged::eAddCommonDeviation(int iValue, bool bIsOptional)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pInflection)->eAddCommonDeviation(iValue, bIsOptional);
    return (EM_ReturnCode)eRet;
}

bool CInflectionManaged::bFindCommonDeviation(int iNum, bool% bIsOptionalDotNet)
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    bool bIsOptionalCpp = false;
    bool bRet = (*m_pInflection)->bFindCommonDeviation(iNum, bIsOptionalCpp);
    bIsOptionalDotNet = bIsOptionalCpp;

    return bRet;
}

void CInflectionManaged::ClearCommonDeviations()
{
    if (nullptr == m_pInflection || nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    (*m_pInflection)->ClearCommonDeviations();
}

CInflectionEnumeratorManaged::CInflectionEnumeratorManaged(shared_ptr<CInflectionEnumerator> spIe) 
    : m_pInflectionEnumerator(&spIe)
{}

CInflectionEnumeratorManaged::~CInflectionEnumeratorManaged()
{
    if (nullptr == m_pInflectionEnumerator)
    {
        throw gcnew Exception(L"Inflection enumerator object is NULL.");
    }
    delete m_pInflectionEnumerator;
}

EM_ReturnCode CInflectionEnumeratorManaged::eReset()
{
    if (nullptr == m_pInflectionEnumerator)
    {
        throw gcnew Exception(L"Inflection enumerator object is NULL.");
    }
    return (EM_ReturnCode)(*m_pInflectionEnumerator)->eReset();
}

EM_ReturnCode CInflectionEnumeratorManaged::eGetFirstInflection(CInflectionManaged^% pInflectionItf)
{
    if (nullptr == m_pInflectionEnumerator)
    {
        throw gcnew Exception(L"Inflection enumerator object is NULL.");
    }

    shared_ptr<CInflection> spInflection;
    ET_ReturnCode eRet = (*m_pInflectionEnumerator)->eGetFirstInflection(spInflection);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (spInflection)
        {
            pInflectionItf = gcnew CInflectionManaged(spInflection);
        }
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CInflectionEnumeratorManaged::eGetNextInflection(CInflectionManaged^% pInflectionItf)
{
    if (nullptr == m_pInflectionEnumerator)
    {
        throw gcnew Exception(L"Inflection enumerator object is NULL.");
    }

    shared_ptr<CInflection> spInflection;
    ET_ReturnCode eRet = (*m_pInflectionEnumerator)->eGetNextInflection(spInflection);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (spInflection)
        {
            pInflectionItf = gcnew CInflectionManaged(spInflection);
        }
    }
    return (EM_ReturnCode)eRet;
}
