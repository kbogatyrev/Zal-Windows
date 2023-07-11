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

    public ref class CInflectionEnumeratorManaged
    {
    public:
        CInflectionEnumeratorManaged(int64_t iHandle);
        ~CInflectionEnumeratorManaged();

        shared_ptr<CInflectionEnumerator> spGetInstance();

        EM_ReturnCode eReset();

        EM_ReturnCode eGetFirstInflection(CInflectionManaged^% pInflection);
        EM_ReturnCode eGetNextInflection(CInflectionManaged^% pInflection);

    protected:
        int64_t m_iHandle;
    };

    public ref class CLexemeManaged
    {
    protected:
        uint64_t m_iHandle;
        int m_iCurrentInflection{ -1 };

    public:
        CLexemeManaged(int64_t iHandle);
        ~CLexemeManaged();

        EM_ReturnCode eCreateInflectionEnumerator(CInflectionEnumeratorManaged^%);
        shared_ptr<CLexeme> spGetInstance();
        __int64 llLexemeId();
        __int64 llHeadwordId();
        EM_Gender eGender();
        String^ sGraphicStem();
        void SetGraphicStem(String^);
        bool bHasIrregularForms();
        void SetHasIrregularForms(bool);
        bool bHasSecondaryStress();
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
        bool bNoLongForms();
        void SetNoLongForms(bool);
        String^ s1SgStem();
        void Set1SgStem(String^);
        String^ s3SgStem();
        void Set3SgStem(String^);
        String^ sInfinitive();
        void SetInfinitive(String^);
        String^ sInfStem();
        void SetInfStem(String^);
        bool bIsSecondPart();
        void SetIsSecondPart(bool);
        bool bSpryazhSm();
        String^ sSpryazhSmRefSource();
        EM_Subparadigm eSubparadigm();
        void SetSubparadigm(EM_Subparadigm);    // currently only used with numerals
        bool bFindStandardAlternation(String^ sKey, String^% sValue);
        EM_ReturnCode eGetStemStressPositions(String^ sStem, List<int>% listPositions);
        bool bHasDifficultForms();
        EM_ReturnCode eGetSourceFormWithStress(String^% sSourceForm, bool bIsVariant);
        EM_ReturnCode eGetFirstStemStressPos(int% iPos);
        EM_ReturnCode eGetNextStemStressPos(int% iPos);
        EM_ReturnCode eGetFirstSecondaryStemStressPos(int% iPos);
        EM_ReturnCode eGetNextSecondaryStemStressPos(int% iPos);
        EM_ReturnCode eCheckLexemeProperties(); // for manual input/editing
        EM_ReturnCode eGetErrorMsg(String^% sErrorMsg);
    };  //  public ref class CLexemeManaged

}       // namespace
