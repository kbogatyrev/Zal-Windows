// MainLibManaged.h

#pragma once

#include "IDictionary.h"
#include "ILexeme.h"
#include "IWordForm.h"
#include "Gramhasher.h"
#include "IParser.h"
#include "IAnalytics.h"
#include "IVerifier.h"
#include "IAspectPair.h"
#include "..\Hlib-Windows\SqliteWrapper.h"
#include "EnumsManaged.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace Hlib;

namespace MainLibManaged 
{
    public delegate void DelegateProgress(int iPercentDone, bool bOperationComplete);

    ref class CLexemeManaged;
    ref class CLexemeEnumeratorManaged;
    ref class CParserManaged;
    ref class CAnalyticsManaged;
    ref class CVerifierManaged;

    public ref class CWordFormManaged
    {
        IWordForm * m_pWordForm;

    public:
        CWordFormManaged(IWordForm *);
        ~CWordFormManaged();

        CLexemeManaged^ Lexeme();

        IWordForm * pGetUnmanagedItf();

        String^ sWordForm();

        void SetWordForm(String^);
        EM_ReturnCode eSetStressPositions(Collections::Generic::Dictionary<int, EM_StressType>^ dctStressPositions);

        long long llWordFormDbId();

        String^ sStem();
        void SetStem(String^);

        __int64 llLexemeId();

        EM_PartOfSpeech ePos();
        void SetPos(EM_PartOfSpeech);

        EM_Case eCase();
        void SetCase(EM_Case);

        EM_Number eNumber();
        void SetNumber(EM_Number);

        EM_Subparadigm eSubparadigm();
        void SetSubparadigm(EM_Subparadigm);

        EM_Gender eGender();
        void SetGender(EM_Gender);

        EM_Person ePerson();
        void SetPerson(EM_Person);

        EM_Animacy eAnimacy();
        void SetAnimacy(EM_Animacy);

        EM_Reflexive eReflexive();
        void SetReflexive(EM_Reflexive);

        EM_Aspect eAspect();
        void SetAspect(EM_Aspect);

        EM_Status eStatus();
        void SetStatus(EM_Status);

        bool bIrregular();      // came from the DB as opposed to being generated by the app
        void SetIrregular(bool);

        String^ sLeadComment();
        void SetLeadComment(String^ sLeadComment);

        String^ sTrailingComment();
        void SetTrailingComment(String^ sTrailingComment);
        
        bool bIsEdited();
        void SetIsEdited(bool bIsEdited);

        bool bIsVariant();
        void SetIsVariant(bool bIsVariant);

        EM_ReturnCode eGetFirstStressPos(int% iPos, EM_StressType% eType);
        EM_ReturnCode eGetNextStressPos(int% iPos, EM_StressType% eType);

        String^ sGramHash();
        EM_ReturnCode eInitFromHash(String^ sHash);

        EM_ReturnCode eSaveIrregularForm();

    };      //  public ref class CWordFormManaged

    public ref class CLexemeManaged
    {
    public:
        ILexeme * m_pLexeme;
        String^ m_sStoredLexemeHash;

        CLexemeManaged();
        CLexemeManaged(ILexeme *);
        CLexemeManaged(String^ sGramHash);
        ~CLexemeManaged();

//        const StLexemeProperties& stGetProperties();
//        StLexemeProperties& stGetPropertiesForWriteAccess();
        __int64 llLexemeId();
        EM_Gender eGender();
//        EM_Animacy eAnimacy();
        String^ sGraphicStem();
        void SetGraphicStem(String^);
        bool bHasIrregularForms();
        void SetHasIrregularForms(bool);
        bool bHasSecondaryStress();
        bool bHasFleetingVowel();
        void SetHasFleetingVowel(bool);
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
        String^ sRestrictedForms();
        void SetRestrictedForms(String^);
        String^ sContexts();
        void SetContexts(String^);
        String^ sTrailingComment();
        void SetTrailingComment(String^);
        int iInflectionId();
        void SetInflectionId(int);
        bool bPrimaryInflectionGroup();
        void SetPrimaryInflectionGroup(bool);
        int iType();
        void SetType(int);
        EM_AccentType eAccentType1();
        void SetAccentType1(EM_AccentType);
        EM_AccentType eAccentType2();
        void SetAccentType2(EM_AccentType);
        bool bShortFormsRestricted();
        void SetShortFormsRestricted(bool);
        bool bPastParticipleRestricted();
        void SetPastParticipleRestricted(bool);
        bool bNoLongForms();
        void SetNoLongForms(bool);
        bool bShortFormsIncomplete();
        void SetShortFormsIncomplete(bool);
        bool bNoPassivePastParticiple();
        void SetNoPassivePastParticiple(bool);
        bool bFleetingVowel();
        void SetFleetingVowel(bool);
        int iStemAugment();
        void SetStemAugment(int);
        String^ s1SgStem();
        void Set1SgStem(String^);
        String^ s3SgStem();
        void Set3SgStem(String^);
        String^ sInfinitive();
        void SetInfinitive(String^);
        String^ sInfStem();
        void SetInfStem(String^);
        int iInflectedParts();
        void SetInflectedParts(int);
        bool bIsSecondPart();
        void SetSecondPart(bool);

        EM_ReturnCode eAddCommonDeviation(int iValue, bool bIsOptional);
        bool bFindCommonDeviation(int iNum, bool% bIsOptional);
        bool bFindStandardAlternation(String^ sKey, String^% sValue);
        EM_ReturnCode eGetStemStressPositions(String^ sStem, List<int>% listPositions);
        EM_ReturnCode eGetAlternatingPreverb(String^ sVerbForm, String^% sPreverb, bool% bVoicing);
        String^ sHash();
        String^ sStoredHash();
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
        EM_ReturnCode eIsFormDifficult(String^ sGramHash);
        bool bIsMultistressedCompound();
        EM_ReturnCode eGetSourceFormWithStress(String^% sSourceForm, bool bIsVariant);
        EM_ReturnCode eGetFirstStemStressPos(int% iPos);
        EM_ReturnCode eGetNextStemStressPos(int% iPos);
        EM_ReturnCode eGetFirstSecondaryStemStressPos(int% iPos);
        EM_ReturnCode eGetNextSecondaryStemStressPos(int% iPos);

//        EM_ReturnCode eSetDb(String^ sDbPath);
        EM_ReturnCode eGenerateParadigm();
//        EM_ReturnCode eBuildAspectPair();
        EM_ReturnCode eSaveTestData();
        EM_ReturnCode eCheckLexemeProperties(); // for manual input/editing
        EM_ReturnCode eDeleteIrregularForm(String^ sFormHash);
        EM_ReturnCode eSaveIrregularForms(String^ sGramHash);
        EM_ReturnCode eMakeGraphicStem();
        EM_ReturnCode eMakeGraphicStem(const String^ sSource, String^% sGraphicStem);

        EM_ReturnCode eGetErrorMsg(String^% sErrorMsg);

    protected:
//        EM_ReturnCode eGetAspectPair(String^%, int%, bool bIsVariant);

    };  //  public ref class CLexemeManaged

    public ref class CDictionaryManaged
    {
        Hlib::IDictionary * m_pDictionary;      // we are using some non-itf methods

    public:
        CDictionaryManaged();
        ~CDictionaryManaged();

        EM_ReturnCode eSetDbPath(String^ sDbPath);
        String^ sGetDbPath();
        CLexemeManaged^ CreateLexemeForEdit();
        CLexemeManaged^ CopyLexemeForEdit(CLexemeManaged^);

        EM_ReturnCode eGetLexemeById(long long Id, CLexemeManaged^% pLexeme);
        EM_ReturnCode eGetLexemesByHash(String^);
        EM_ReturnCode eGetLexemesByGraphicStem(String^);
        EM_ReturnCode eGetLexemesByInitialForm(String^);
        EM_ReturnCode eGenerateAllForms();
        EM_ReturnCode eGenerateFormsForSelectedLexemes();
        EM_ReturnCode eCountLexemes(Int64% iLexemes);

//        EM_ReturnCode eVerifyLexemeProperties(CLexemeManaged^);
//        EM_ReturnCode eSourceFormExists(CLexemeManaged^, bool%);

        // Save manually edited entries
        EM_ReturnCode eSaveHeadword(CLexemeManaged^);
        EM_ReturnCode eSaveAspectPairInfo(CLexemeManaged^);
        EM_ReturnCode eSaveP2Info(CLexemeManaged^);
        EM_ReturnCode eSaveDescriptorInfo(CLexemeManaged^);
        EM_ReturnCode eSaveCommonDeviation(CLexemeManaged^);
        EM_ReturnCode eSaveInflectionInfo(CLexemeManaged^);

        int nLexemesFound();

        void Clear();
        EM_ReturnCode Clear(CLexemeManaged^ pLexeme);

        EM_ReturnCode eCreateLexemeEnumerator(CLexemeEnumeratorManaged^%);

        EM_ReturnCode eGetParser(CParserManaged^%); 
        EM_ReturnCode eGetAnalytics(CAnalyticsManaged^%);
        EM_ReturnCode eGetVerifier(CVerifierManaged^%);
        EM_ReturnCode eExportTestData(String^ sPath, DelegateProgress^);
        EM_ReturnCode eImportTestData(String^ sPath, DelegateProgress^);
    
    };      //  CDictionaryManaged

    public ref class CGramHasherManaged
    {
        Hlib::CGramHasher * m_pHasher;

    public:
        CGramHasherManaged();
        ~CGramHasherManaged();

        void SetPartOfSpeech(EM_PartOfSpeech);
        void SetSubparadigm(EM_Subparadigm);
        void SetCase(EM_Case);
        void SetNumber(EM_Number);
        void SetGender(EM_Gender);
        void SetPerson(EM_Person);
        void SetAnimacy(EM_Animacy);
        void SetReflexivity(EM_Reflexive);
        void SetAspect(EM_Aspect);
        String^ sHash();
    };

    public ref class CLexemeEnumeratorManaged
    {
        Hlib::ILexemeEnumerator* m_pLexemeEnumerator;

    public:
        CLexemeEnumeratorManaged(ILexemeEnumerator *);
        ~CLexemeEnumeratorManaged();

        EM_ReturnCode eReset();

        EM_ReturnCode eGetFirstLexeme(CLexemeManaged^% pLexemeItf);
        EM_ReturnCode eGetNextLexeme(CLexemeManaged^% pLexemeItf);
    };

    public ref class CParserManaged
    {
        Hlib::IParser * m_pParser;

    public:
        CParserManaged(IParser *);
        ~CParserManaged();

        EM_ReturnCode eParseWord(String^ sWord);
        EM_ReturnCode eGetFirstWordForm(CWordFormManaged^% pWordFrom);
        EM_ReturnCode eGetNextWordForm(CWordFormManaged^% pWordFrom);
    };

    public ref class CAnalyticsManaged
    {
        Hlib::IAnalytics* m_pAnalytics;

    public:
        CAnalyticsManaged(IAnalytics*);
        ~CAnalyticsManaged();

        EM_ReturnCode eParseText(String^ sName, String^ sMetaData, String^ sText, long long% lParseTextId);
    };

    public ref class CVerifierManaged
    {
        Hlib::IVerifier * m_pVerifier;

    public:
        CVerifierManaged(IVerifier *);
        ~CVerifierManaged();

        EM_ReturnCode eVerify(String^ sLexemeHash);
        EM_TestResult eResult();

        int iCount();

        EM_ReturnCode eLoadStoredLexemes();
        EM_ReturnCode eDeleteStoredLexeme(String^);

        EM_ReturnCode eGetFirstLexemeData(String^% sLexemeHash, String^% sHeadword);
        EM_ReturnCode eGetNextLexemeData(String^% sLexemeHash, String^% sHeadword);
    };

}       //  namespace MainLibManaged
