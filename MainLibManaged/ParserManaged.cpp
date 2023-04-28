#pragma once

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#define _SILENCE_CXX17_STRSTREAM_DEPRECATION_WARNING

#define NOMINMAX

#include <memory>
#include <msclr\marshal.h>
#include "UtilsManaged.h"
#include "GramHasher.h"
#include "WordFormManaged.h"
#include "Singleton.h"

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

namespace MainLibManaged
{
    CParserManaged::CParserManaged(shared_ptr<CParser> spParser)
    {}

    CParserManaged::~CParserManaged()
    {}

    shared_ptr<CParser> CParserManaged::spGetInstance()
    {
        shared_ptr<CParser> spParser;
        auto rc = Singleton::pGetInstance()->eGetParser(spParser);
        if (rc != H_NO_ERROR || nullptr == spParser)
        {
            throw gcnew Exception(L"Unable to retrieve parser instance.");
        }
        return spParser;
    }

    EM_ReturnCode CParserManaged::eParseWord(String^ sForm)
    {
        return (EM_ReturnCode)(spGetInstance()->eParseWord(sFromManagedString(sForm)));
    }

    EM_ReturnCode CParserManaged::eGetFirstWordForm(CWordFormManaged^% pManagedWordFrom)
    {
        shared_ptr<CWordForm> spWordForm;
        ET_ReturnCode eRet = spGetInstance()->eGetFirstWordForm(spWordForm);
        if (H_NO_ERROR == eRet)
        {
            if (spWordForm)
            {
                auto iHandle = Singleton::pGetInstance()->iAddWordForm(spWordForm);
                if (iHandle < 0)
                {
                    if (iHandle < 0)
                    {
                        throw gcnew Exception(L"Failed to add a word form.");
                    }
                }
                pManagedWordFrom = gcnew CWordFormManaged(iHandle);
            }
        }
        return (EM_ReturnCode)eRet;
    }

    EM_ReturnCode CParserManaged::eGetNextWordForm(CWordFormManaged^% pManagedWordFrom)
    {
        shared_ptr<CWordForm> spWordForm;
        ET_ReturnCode eRet = spGetInstance()->eGetNextWordForm(spWordForm);
        if (H_NO_ERROR == eRet)
        {
            if (spWordForm)
            {
                auto iHandle = Singleton::pGetInstance()->iAddWordForm(spWordForm);
                if (iHandle < 0)
                {
                    if (iHandle < 0)
                    {
                        throw gcnew Exception(L"Failed to add a word form.");
                    }
                }
                pManagedWordFrom = gcnew CWordFormManaged(iHandle);
            }
        }
        return (EM_ReturnCode)eRet;
    }
}       // namespace MainLibManaged
