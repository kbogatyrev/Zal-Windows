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
    ref class CLexemeEnumeratorManaged;
    ref class CLexemeManaged;
    ref class CWordFormManaged;
    ref class CParserManaged;
    ref class CAnalyticsManaged;
    ref class CVerifierManaged;

    public ref class CInflectionManaged
    {
    public:
        shared_ptr<CInflection>* m_pInflection;

        CInflectionManaged();
        CInflectionManaged(shared_ptr<CLexeme>);
        CInflectionManaged(shared_ptr<CInflection>); // copy ctor
        ~CInflectionManaged();

        shared_ptr<CInflection> spInflection();

        //        bool bHasIrregularForms();
        //        void SetHasIrregularForms(bool);
        String^ sHash();
        String^ sParadigmHash();
        String^ sStoredHash();

        void SetLexeme(CLexemeManaged^ lexeme);
        EM_ReturnCode eGetLexeme(CLexemeManaged^% lexeme);
        EM_ReturnCode eWordFormFromHash(String^ sHash, int iAt, CWordFormManaged^% Wf);
        EM_ReturnCode eCreateWordForm(CWordFormManaged^% wf);
        EM_ReturnCode eRemoveWordForm(String^ sHash, int iAt);
        EM_ReturnCode eRemoveWordForms(String^ sHash);
        void AddWordForm(CWordFormManaged^% Wf);
        bool bHasIrregularForm(String^ sGramHash);
        bool bNoRegularForms(String^ sGramHash);
        EM_ReturnCode eGetFirstWordForm(CWordFormManaged^% wf);
        EM_ReturnCode eGetNextWordForm(CWordFormManaged^% wf);
        EM_ReturnCode eGetFirstIrregularForm(String^ sHash, CWordFormManaged^%, bool% bIsOptional);
        EM_ReturnCode eGetNextIrregularForm(CWordFormManaged^%, bool% bIsOptional);
        int iFormCount(String^ sHash);
        bool bHasCommonDeviation(int iCd);
        bool bDeviationOptional(int iCd);
        EM_ReturnCode eFormExists(String^ sGramHash);
        EM_ReturnCode eSetFormExists(String^ sGramHash, bool bExists);
        EM_ReturnCode eIsFormDifficult(String^ sGramHash);
        EM_ReturnCode eSetFormDifficult(String^ sGramHash, bool bIsDifficult);
        EM_ReturnCode eGenerateParadigm();
        EM_ReturnCode eSaveTestData();
        EM_ReturnCode eDeleteIrregularForm(String^ sFormHash);
        EM_ReturnCode eSaveIrregularForms(String^ sGramHash);
        bool bHasFleetingVowel();
        void SetHasFleetingVowel(bool);
        int iType();
        void SetType(int);
        EM_AccentType eAccentType1();
        void SetAccentType1(EM_AccentType);
        EM_AccentType eAccentType2();
        void SetAccentType2(EM_AccentType);
        bool bShortFormsRestricted();
        void SetShortFormsRestricted(bool);
        bool bShortFormsIncomplete();
        void SetShortFormsIncomplete(bool);
        bool bPastParticipleRestricted();
        void SetPastParticipleRestricted(bool);
        long long llInflectionId();
        void SetInflectionId(int);
        bool bPrimaryInflectionGroup();
        void SetPrimaryInflectionGroup(bool);
        bool bNoPassivePastParticiple();
        void SetNoPassivePastParticiple(bool);
        bool bFleetingVowel();
        void SetFleetingVowel(bool);
        int iStemAugment();
        void SetStemAugment(int);
        EM_ReturnCode eAddCommonDeviation(int iValue, bool bIsOptional);
        void ClearCommonDeviations();
        bool bFindCommonDeviation(int iNum, bool% bIsOptional);
        EM_ReturnCode eDifficultFormsHashes(List<String^>^% hashes);
        EM_ReturnCode eIsFormAssumed(String^ sGramHash);
        EM_ReturnCode eSetHasAssumedForms(bool bHasAssumendForms);
        bool bIsMultistressedCompound();
        EM_ReturnCode eMakeGraphicStem();
        EM_ReturnCode eMakeGraphicStem(const String^ sSource, String^% sGraphicStem);

        // private:     -- TODO: accessors/mutators?
        String^ m_sStoredEntryHash;

    };

    public ref class CInflectionEnumeratorManaged
    {
        shared_ptr<Hlib::CInflectionEnumerator>* m_pInflectionEnumerator;

    public:
        CInflectionEnumeratorManaged(shared_ptr<CInflectionEnumerator>);
        ~CInflectionEnumeratorManaged();

        EM_ReturnCode eReset();

        EM_ReturnCode eGetFirstInflection(CInflectionManaged^% pInflection);
        EM_ReturnCode eGetNextInflection(CInflectionManaged^% pInflection);
    };


}       // namespace
