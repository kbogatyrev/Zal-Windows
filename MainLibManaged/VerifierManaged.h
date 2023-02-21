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
    public ref class CVerifierManaged
    {
        shared_ptr<Hlib::CVerifier>* m_pVerifier;

    public:
        CVerifierManaged(shared_ptr<CVerifier>);
        ~CVerifierManaged();

        EM_ReturnCode eVerify(String^ sLexemeHash);
        EM_TestResult eResult();

        int iCount();

        EM_ReturnCode eLoadStoredLexemes();
        EM_ReturnCode eDeleteStoredLexeme(String^);

        EM_ReturnCode eGetFirstLexemeData(String^% sLexemeHash, String^% sHeadword);
        EM_ReturnCode eGetNextLexemeData(String^% sLexemeHash, String^% sHeadword);
    };

}       //  namespace