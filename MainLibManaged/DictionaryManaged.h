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
    ref class CLexemeManaged;
    ref class CLexemeEnumeratorManaged;
    ref class CInflectionManaged;
    ref class CInflectionEnumeratorManaged;
    ref class CParserManaged;
    ref class CAnalyticsManaged;
    ref class CVerifierManaged;

    public ref class CLexemeEnumeratorManaged
    {
    public:
        CLexemeEnumeratorManaged(int64_t iHandle);
        ~CLexemeEnumeratorManaged();

        shared_ptr<CLexemeEnumerator> spGetInstance();

        EM_ReturnCode eReset();

        EM_ReturnCode eGetFirstLexeme(CLexemeManaged^% pLexeme);
        EM_ReturnCode eGetNextLexeme(CLexemeManaged^% pLexeme);

    protected:
        int64_t m_iHandle;
    };

    public ref class CDictionaryManaged
    {
    public:
        ~CDictionaryManaged();

        shared_ptr<CDictionary> spGetInstance();

        EM_ReturnCode eSetDbPath(String^ sDbPath);
        String^ sGetDbPath();
        CLexemeManaged^ CreateLexemeForEdit();
        EM_ReturnCode eCopyEntryForEdit(CLexemeManaged^ pSourceL, CLexemeManaged^ pTargetL);

        EM_ReturnCode eGetLexemeById(long long Id, CLexemeManaged^% pLexeme);
        EM_ReturnCode eGetLexemesByHash(String^);
        EM_ReturnCode eGetLexemesByInitialForm(String^);
        EM_ReturnCode eGenerateAllForms();
        EM_ReturnCode eCountLexemes(Int64% iLexemes);


        int nLexemesFound();

        void Clear();

        EM_ReturnCode eCreateLexemeEnumerator(CLexemeEnumeratorManaged^%);

        EM_ReturnCode eGetParser(CParserManaged^%);
        EM_ReturnCode eGetAnalytics(CAnalyticsManaged^%);
        EM_ReturnCode eGetVerifier(CVerifierManaged^%);

    };      //  CDictionaryManaged

} // namespace
