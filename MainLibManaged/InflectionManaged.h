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
        CInflectionManaged(int64_t handle);
        ~CInflectionManaged();


        //        bool bHasIrregularForms();
        //        void SetHasIrregularForms(bool);
        String^ sHash();
        String^ sParadigmHash();
        String^ sStoredHash();

//        void SetLexeme(CLexemeManaged^ lexeme);
        EM_ReturnCode eGetLexeme(CLexemeManaged^% lexeme);
        EM_ReturnCode eWordFormFromHash(String^ sHash, int iAt, CWordFormManaged^% Wf);
        EM_ReturnCode eCreateWordForm(CWordFormManaged^% wf);
        EM_ReturnCode eRemoveWordForm(String^ sHash, int iAt);
        EM_ReturnCode eRemoveIrregularForms(String^ sHash);
        void AddModifiedForm(CWordFormManaged^% Wf);            // neded to add a new or modified(?) wf
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

        shared_ptr<CInflection> spGetInstance();

    protected:
        CInflectionManaged() {};
        int64_t m_iHandle{ -1 }; 
    };


}       // namespace
