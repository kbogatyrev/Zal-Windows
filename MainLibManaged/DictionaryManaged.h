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

    public ref class CDictionaryManaged
    {
    protected:
        shared_ptr<Hlib::CDictionary>* m_pDictionary;

    public:
        CDictionaryManaged();
        ~CDictionaryManaged();

        EM_ReturnCode eSetDbPath(String^ sDbPath);
        String^ sGetDbPath();
        CLexemeManaged^ CreateLexemeForEdit();
        EM_ReturnCode eCopyEntryForEdit(CLexemeManaged^ pSourceL, CInflectionManaged^ pSourceI,
            CLexemeManaged^ pTargetL, CInflectionManaged^ pTargetI);

        EM_ReturnCode eGetLexemeById(long long Id, CLexemeManaged^% pLexeme);
        EM_ReturnCode eGetLexemesByHash(String^);
        //        EM_ReturnCode eGetLexemesByGraphicStem(String^);
        EM_ReturnCode eGetLexemesByInitialForm(String^);
        EM_ReturnCode eGenerateAllForms();
        //        EM_ReturnCode eGenerateFormsForSelectedLexemes();
        EM_ReturnCode eCountLexemes(Int64% iLexemes);

        //        EM_ReturnCode eVerifyLexemeProperties(CLexemeManaged^);
        //        EM_ReturnCode eSourceFormExists(CLexemeManaged^, bool%);

                // Save manually edited entries
        EM_ReturnCode eUpdateHeadword(CLexemeManaged^);
        EM_ReturnCode eSaveNewHeadword(CLexemeManaged^);
        EM_ReturnCode eSaveHeadwordStress(CLexemeManaged^);
        EM_ReturnCode eSaveHomonyms(CLexemeManaged^);
        EM_ReturnCode eSaveAspectPairInfo(CLexemeManaged^);
        EM_ReturnCode eSaveP2Info(CLexemeManaged^);
        EM_ReturnCode eUpdateDescriptorInfo(CLexemeManaged^);
        EM_ReturnCode eSaveDescriptorInfo(CLexemeManaged^ pLexeme);
        EM_ReturnCode eSaveCommonDeviation(CInflectionManaged^);
        EM_ReturnCode eSaveInflectionInfo(CInflectionManaged^);
        EM_ReturnCode eDeleteLexeme(CLexemeManaged^);

        int nLexemesFound();

        void Clear();
        EM_ReturnCode Clear(CLexemeManaged^ pLexeme);

        //        EM_ReturnCode eCreateLexemeEnumerator(CLexemeEnumeratorManaged^%);

        EM_ReturnCode eGetParser(CParserManaged^%);
        EM_ReturnCode eGetAnalytics(CAnalyticsManaged^%);
        EM_ReturnCode eGetVerifier(CVerifierManaged^%);
        EM_ReturnCode eExportTestData(String^ sPath, DelegateProgress^);
        EM_ReturnCode eImportTestData(String^ sPath, DelegateProgress^);

    };      //  CDictionaryManaged

} // namespace
