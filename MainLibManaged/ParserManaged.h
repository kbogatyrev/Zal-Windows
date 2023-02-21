#pragma once

#include "EnumsManaged.h"
#include "Dictionary.h"
#include "Lexeme.h"
#include "Inflection.h"
#include "WordForm.h"
#include "Gramhasher.h"
#include "Parser.h"
#include "Analytics.h"
#include "Verifier.h"
//#include "AspectPair.h"
#include "..\Hlib\SqliteWrapper.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace Hlib;

namespace MainLibManaged
{
    ref class CWordFormManaged;

    public ref class CParserManaged
    {
        shared_ptr<Hlib::CParser>* m_pParser;

    public:
        CParserManaged(shared_ptr<CParser>);
        ~CParserManaged();

        EM_ReturnCode eParseWord(String^ sWord);
        EM_ReturnCode eGetFirstWordForm(CWordFormManaged^% pWordFrom);
        EM_ReturnCode eGetNextWordForm(CWordFormManaged^% pWordFrom);
    };

}       // namespace