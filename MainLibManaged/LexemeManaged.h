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

#define NOMINMAX

using namespace System;
using namespace System::Collections::Generic;
using namespace Hlib;

namespace MainLibManaged
{
    ref class CLexemeEnumeratorManaged;
    ref class CInflectionManaged;
    ref class CInflectionEnumeratorManaged;
    ref class CParserManaged;
    ref class CAnalyticsManaged;
    ref class CVerifierManaged;

    public ref class CLexemeManaged
    {
    public:
        shared_ptr<CLexeme>* m_pLexeme;
        //        String^ m_sStoredLexemeHash;

        CLexemeManaged();
        CLexemeManaged(shared_ptr<CLexeme>);
        //        CLexemeManaged(String^ sGramHash);
        ~CLexemeManaged();

        EM_ReturnCode eCreateInflectionEnumerator(CInflectionEnumeratorManaged^%);

        //        const StLexemeProperties& stGetProperties();
//        StLexemeProperties& stGetPropertiesForWriteAccess();
        shared_ptr<CLexeme> spLexeme();
        __int64 llLexemeId();
        __int64 llHeadwordId();
        EM_Gender eGender();
        //        EM_Animacy eAnimacy();
        String^ sGraphicStem();
        void SetGraphicStem(String^);
        bool bHasIrregularForms();
        void SetHasIrregularForms(bool);
        bool bHasSecondaryStress();
        //        bool bHasFleetingVowel();
        //        void SetHasFleetingVowel(bool);
        bool bHasYoAlternation();
        void SetHasYoAlternation(bool);
        bool bHasOAlternation();
        void SetHasOAlternation(bool);
        String^ sSourceForm();
        void SetSourceForm(String^);
        bool bHasHomonyms();
        List<int>^ arrHomonyms();
        void SetHomonyms(List<int>^);
        String^ sHeadwordComment();
        void SetHeadwordComment(String^);
        String^ sHeadwordVariant();
        void SetHeadwordVariant(String^);
        String^ sHeadwordVariantComment();
        void SetHeadwordVariantComment(String^);
        String^ sPluralOf();
        void SetPluralOf(String^);
        String^ sUsage();
        void SetUsage(String^);
        String^ sSeeRef();
        void SetSeeRef(String^);
        String^ sBackRef();
        void SetBackRef(String^);
        bool bIsUnstressed();
        void SetIsUnstressed(bool);
        bool bIsVariant();
        void SetIsVariant(bool);
        String^ sMainSymbol();
        void SetMainSymbol(String^);
        bool bIsPluralOf();
        void SetIsPluralOf(bool);
        bool bTransitive();
        void SetTransitive(bool);
        EM_Reflexive eIsReflexive();
        void SetIsReflexive(EM_Reflexive);
        String^ sMainSymbolPluralOf();
        void SetMainSymbolPluralOf(String^);
        String^ sAltMainSymbol();
        void SetAltMainSymbol(String^);
        EM_Aspect eAspect();
        void SetAspect(EM_Aspect);
        String^ sInflectionType();
        void SetInflectionType(String^);
        EM_PartOfSpeech ePartOfSpeech();
        void SetPartOfSpeech(EM_PartOfSpeech);
        String^ sComment();
        void SetComment(String^);
        String^ sAltMainSymbolComment();
        void SetAltMainSymbolComment(String^);
        String^ sAltInflectionComment();
        void SetAltInflectionComment(String^);
        String^ sVerbStemAlternation();
        void SetVerbStemAlternation(String^);
        bool bPartPastPassZhd();
        void SetPartPastPassZhd(bool);
        int iSection();
        void SetSection(int);
        bool bNoComparative();
        void SetNoComparative(bool);
        bool bAssumedForms();
        void SetAssumedForms(bool);
        //        bool bYoAlternation();
        //        void SetYoAlternation(bool);
        //        bool bOAlternation();
        //        void SetHasOAlternation(bool);
        bool bSecondGenitive();
        void SetSecondGenitive(bool);
        bool bSecondPrepositional();
        void SetSecondPrepositional(bool);
        bool bSecondPrepositionalOptional();
        void SetSecondPrepositionalOptional(bool);
        String^ sP2Preposition();
        void SetP2Preposition(String^);
        bool bHasAspectPair();
        void SetHasAspectPair(bool);
        bool bHasAltAspectPair();
        int iAspectPairType();
        void SetAspectPairType(int);
        int iAltAspectPairType();
        void SetAltAspectPairType(int);
        String^ sAltAspectPairComment();
        void SetAltAspectPairComment(String^);
        EM_ReturnCode eGetAspectPair(String^% sAspectPair, int% iStressPos);
        EM_ReturnCode eGetAltAspectPair(String^% sAltAspectPair, int% iAltStressPos);
        String^ sAspectPairData();
        void SetAspectPairData(String^);
        bool bHasIrregularVariants();
        void SetHasIrregularVariants(bool);
        String^ sRestrictedContexts();
        void SetRestrictedContexts(String^);
        String^ sContexts();
        void SetContexts(String^);
        String^ sTrailingComment();
        void SetTrailingComment(String^);
        //        long long llInflectionId();
        //        void SetInflectionId(int);
        //        bool bPrimaryInflectionGroup();
        //        void SetPrimaryInflectionGroup(bool);
        //        int iType();
        //        void SetType(int);
        //        EM_AccentType eAccentType1();
        //        void SetAccentType1(EM_AccentType);
        //        EM_AccentType eAccentType2();
        //        void SetAccentType2(EM_AccentType);
        //        bool bShortFormsRestricted();
        //        void SetShortFormsRestricted(bool);
        //        bool bPastParticipleRestricted();
        //        void SetPastParticipleRestricted(bool);
        bool bNoLongForms();
        void SetNoLongForms(bool);
        //        bool bShortFormsIncomplete();
        //        void SetShortFormsIncomplete(bool);
        //        bool bNoPassivePastParticiple();
        //        void SetNoPassivePastParticiple(bool);
        //        bool bFleetingVowel();
        //        void SetFleetingVowel(bool);
        //        int iStemAugment();
        //        void SetStemAugment(int);
        String^ s1SgStem();
        void Set1SgStem(String^);
        String^ s3SgStem();
        void Set3SgStem(String^);
        String^ sInfinitive();
        void SetInfinitive(String^);
        String^ sInfStem();
        void SetInfStem(String^);
        //        int iInflectedParts();
        //        void SetInflectedParts(int);
        bool bIsSecondPart();
        void SetIsSecondPart(bool);
        bool bSpryazhSm();
        String^ sSpryazhSmRefSource();
        EM_Subparadigm eSubparadigm();
        void SetSubparadigm(EM_Subparadigm);    // currently only used with numerals

        //        EM_ReturnCode eAddCommonDeviation(int iValue, bool bIsOptional);
        //        void ClearCommonDeviations();
        //        bool bFindCommonDeviation(int iNum, bool% bIsOptional);
        bool bFindStandardAlternation(String^ sKey, String^% sValue);
        EM_ReturnCode eGetStemStressPositions(String^ sStem, List<int>% listPositions);
        //        EM_ReturnCode eGetAlternatingPreverb(String^ sVerbForm, String^% sPreverb, bool% bVoicing);
        //        String^ sHash();
        //        String^ sStoredHash();
        //        String^ sParadigmHash();
        //        EM_ReturnCode eWordFormFromHash(String^ sHash, int iAt, CWordFormManaged^% Wf);
        //        EM_ReturnCode eCreateWordForm(CWordFormManaged^% wf);
        //        EM_ReturnCode eRemoveWordForm(String^ sHash, int iAt);
        //        EM_ReturnCode eRemoveWordForms(String^ sHash);
        //        void AddWordForm(CWordFormManaged^% Wf);
        //        bool bHasIrregularForm(String^ sGramHash);
        //        bool bNoRegularForms(String^ sGramHash);
        //        EM_ReturnCode eGetFirstWordForm(CWordFormManaged^% wf);
        //        EM_ReturnCode eGetNextWordForm(CWordFormManaged^% wf);
        //        EM_ReturnCode eGetFirstIrregularForm(String^ sHash, CWordFormManaged^%, bool% bIsOptional);
        //        EM_ReturnCode eGetNextIrregularForm(CWordFormManaged^%, bool% bIsOptional);
        //        int iFormCount(String^ sHash);
        //        bool bHasCommonDeviation(int iCd);
        //        bool bDeviationOptional(int iCd);
        //        EM_ReturnCode eFormExists(String^ sGramHash);
        //        EM_ReturnCode eSetFormExists(String^ sGramHash, bool bExists);
        //        EM_ReturnCode eIsFormDifficult(String^ sGramHash);
        //        EM_ReturnCode eSetFormDifficult(String^ sGramHash, bool bIsDifficult);
        bool bHasDifficultForms();
        //        EM_ReturnCode eDifficultFormsHashes(List<String^>^% hashes);
        //        EM_ReturnCode eIsFormAssumed(String^ sGramHash);
        //        EM_ReturnCode eSetHasAssumedForms(bool bHasAssumendForms);
        //        bool bIsMultistressedCompound();
        EM_ReturnCode eGetSourceFormWithStress(String^% sSourceForm, bool bIsVariant);
        EM_ReturnCode eGetFirstStemStressPos(int% iPos);
        EM_ReturnCode eGetNextStemStressPos(int% iPos);
        EM_ReturnCode eGetFirstSecondaryStemStressPos(int% iPos);
        EM_ReturnCode eGetNextSecondaryStemStressPos(int% iPos);

        //        EM_ReturnCode eSetDb(String^ sDbPath);
        //        EM_ReturnCode eGenerateParadigm();
        //        EM_ReturnCode eBuildAspectPair();
        //        EM_ReturnCode eSaveTestData();
        //        EM_ReturnCode eCheckLexemeProperties(); // for manual input/editing
        //        EM_ReturnCode eDeleteIrregularForm(String^ sFormHash);
        //        EM_ReturnCode eSaveIrregularForms(String^ sGramHash);
        //        EM_ReturnCode eMakeGraphicStem();
        //        EM_ReturnCode eMakeGraphicStem(const String^ sSource, String^% sGraphicStem);

        EM_ReturnCode eGetErrorMsg(String^% sErrorMsg);

    protected:
        //        EM_ReturnCode eGetAspectPair(String^%, int%, bool bIsVariant);

    };  //  public ref class CLexemeManaged

    public ref class CLexemeEnumeratorManaged
    {
        shared_ptr<Hlib::CLexemeEnumerator>* m_pLexemeEnumerator;

    public:
        CLexemeEnumeratorManaged(shared_ptr<CLexemeEnumerator>);
        ~CLexemeEnumeratorManaged();

        EM_ReturnCode eReset();

        EM_ReturnCode eGetFirstLexeme(CLexemeManaged^% pLexemeItf);
        EM_ReturnCode eGetNextLexeme(CLexemeManaged^% pLexemeItf);
    };

}       // namespace
