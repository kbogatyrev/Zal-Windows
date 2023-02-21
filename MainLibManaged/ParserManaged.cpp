#pragma once

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#define _SILENCE_CXX17_STRSTREAM_DEPRECATION_WARNING

#define NOMINMAX

#include <memory>
#include <msclr\marshal.h>
#include "UtilsManaged.h"
#include "GramHasher.h"
#include "WordFormManaged.h"

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

#include "ParserManaged.h"

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace MainLibManaged;
using namespace std;

CParserManaged::CParserManaged(shared_ptr<CParser> spParser) : m_pParser(&spParser)
{}

CParserManaged::~CParserManaged()
{
    delete m_pParser;

    //    this->!CParserManaged();
}

EM_ReturnCode CParserManaged::eParseWord(String^ sForm)
{
    if (nullptr == m_pParser)
    {
        throw gcnew Exception(L"Parser object is NULL.");
    }

    return (EM_ReturnCode)(*m_pParser)->eParseWord(sFromManagedString(sForm));
}

EM_ReturnCode CParserManaged::eGetFirstWordForm(CWordFormManaged^% pManagedWordFrom)
{
    if (nullptr == m_pParser)
    {
        throw gcnew Exception(L"Parser object is NULL.");
    }

    shared_ptr<CWordForm> spWordForm;
    ET_ReturnCode eRet = (*m_pParser)->eGetFirstWordForm(spWordForm);
    if (H_NO_ERROR == eRet)
    {
        if (spWordForm)
        {
            pManagedWordFrom = gcnew CWordFormManaged(spWordForm);
        }
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CParserManaged::eGetNextWordForm(CWordFormManaged^% pManagedWordFrom)
{
    if (nullptr == m_pParser)
    {
        throw gcnew Exception(L"Parser object is NULL.");
    }

    shared_ptr<CWordForm> spWordForm;
    ET_ReturnCode eRet = (*m_pParser)->eGetNextWordForm(spWordForm);
    if (H_NO_ERROR == eRet)
    {
        if (spWordForm)
        {
            pManagedWordFrom = gcnew CWordFormManaged(spWordForm);
        }
        else
        {
            return EM_ReturnCode::H_ERROR_UNEXPECTED;
        }
    }

    return (EM_ReturnCode)eRet;
}
