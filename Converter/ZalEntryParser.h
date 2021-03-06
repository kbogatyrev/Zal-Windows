#pragma once

#include "Globals.h"
#include "SqliteWrapper.h"
//#include "LexemeHasher.h"

using namespace Hlib;

//static wstring str_CyrillicLowercase = L"-абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
//static wstring str_Vowels = L"аеёиоуыэюя";

/*
enum ET_PartOfSpeech 
{
    POS_START,
    POS_NOUN = POS_START, 
    POS_VERB, 
    POS_ADJ, 
    POS_PRONOUN,
    POS_NUM,
    POS_ADV, 
    POS_COMPAR, 
    POS_PREDIC, 
    POS_PREP, 
    POS_CONJ, 
    POS_PARTICLE, 
    POS_INTERJ, 
    POS_PARENTH,     // ?
    POS_END,
    POS_UNDEFINED
};
static void operator++ (ET_PartOfSpeech& eo_pos) 
{
    eo_pos = (ET_PartOfSpeech)(eo_pos + 1);
}
*/

enum ET_Category 
{
    GT_START,
    GT_NOUN = GT_START,
    GT_VERB, 
    GT_ADJ, 
    GT_UNINFLECTED,
    GT_END
};

enum ET_MainSymbol
{
    MS_START,
    MS_M = MS_START,
    MS_MO,
    MS_ZH,
    MS_ZHO,
    MS_S,
    MS_SO,
    MS_MO_ZHO,
    MS_MN,
    MS_MN_NEOD,
    MS_MN_ODUSH,
    MS_MN_OT,
    MS_P,
    MS_MS,
    MS_MS_P,
    MS_MS_PREDIK,
    MS_CHISL,
    MS_CHISL_P,
    MS_SV,
    MS_NSV,
    MS_SV_NSV,
    MS_N,
    MS_PREDL,
    MS_SOJUZ,
    MS_PREDIK,
    MS_VVODN,
    MS_SRAVN,
    MS_CHAST,
    MS_MEZHD,
    MS_NULL,
    MS_END,
    MS_UNDEFINED
};
static void operator++ (ET_MainSymbol& eMs) 
{
    eMs = (ET_MainSymbol)(eMs + 1);
}

/*
enum ET_AccentType
{
    AT_START,
    AT_A = AT_START,
    AT_A1,
    AT_B,
    AT_B1,
    AT_C,
    AT_C1,
    AT_C2,
    AT_D,
    AT_D1,
    AT_E,
    AT_F,
    AT_F1,
    AT_F2,
    AT_END,
    AT_UNDEFINED
};
static void operator++ (ET_AccentType& eo_at) 
{
    eo_at = (ET_AccentType)(eo_at + 1);
}
*/

//enum ET_StressType
//{
//    STRESS_PRIMARY,
//    STRESS_SECONDARY
//};

struct StHeadword
{
    vector<int> vecIHomonyms;
    CEString sRawSource;      // with stress marks
    CEString sSource;
    CEString sPluralOf;
    CEString sComment;
    CEString sUsage;
    map<int, ET_StressType> mapStress;
    CEString sVariant;        // subentry in square brackets
    CEString sVariantComment; // ак<ант м 1а [//_устар._ ак<анф]
                              //               ^^^^^^^^^
    CEString sSeeRef;         // "_см._"
    CEString sOrigination;    // valid for subentries: барж<а ж 4в [//б<аржа ж 4а]
                              //                       ^^^^^^
    __int64 llWordId;

    StHeadword() : llWordId(0)
    {}

    bool bSaveToDb (CSqlite * pDbHandle, __int64& llId);
    bool bSaveStressData (CSqlite * pDbHandle, __int64 llHeadwordId, int iPosition, ET_StressType eType);

friend bool operator== (const StHeadword&, const StHeadword &);
friend bool operator!= (const StHeadword&, const StHeadword &);
friend bool operator < (const StHeadword&, const StHeadword &);

};

struct StCommonDeviation
{
    bool bOptional;
    int iType;
};

struct StInflectionGroup
{
    bool bHasData;

    int iType;
    int iInflectedParts;   // 0: the last part is inflected (normal)
                           // 1: the first part is inflected (какой-то)
                           // 2: both parts are inflected (конёк-горбунок)

    ET_AccentType eAccentType1;
    ET_AccentType eAccentType2;

    bool bX;
    bool bBoxedX;
    bool bFleetingVowel;
//    bool b_StemAugment;
    int iStemAugmentType;
    CEString sComment;

    vector<StCommonDeviation> vecCommonDeviations;

    StInflectionGroup() :
        bHasData (false),
        iType (-1), 
        iInflectedParts (0), 
        eAccentType1 (AT_UNDEFINED),
        eAccentType2 (AT_UNDEFINED),
        bX (false),
        bBoxedX (false),
        bFleetingVowel (false),
        iStemAugmentType (-1)
    {}
};

struct StDescriptor
{
    StDescriptor() : 
        bVariant (false),
        bPluralOf (false), 
        bIntransitive (false),
        bReflexive (false),
//        eMainSymbol (MS_UNDEFINED),
        eInflectionSymbol (MS_UNDEFINED),
        ePartOfSpeech (POS_UNDEFINED), 
        bPastPartPassZhD (false),
        iSection (-1),
        bNoComparative (false),
        bNoLongForms (false),
        bAssumedForms (false),
        bYo (false),
        bO (false),
        bGen2 (false),
        bLoc2 (false),
        bLoc2Optional (false),
        bHasAspectPair (false),
        bHasIrregularForms (false),
        bHasDifficulties (false),
        bHasDeficiencies (false),
        bIsImpersonal (false), 
        bIsIterative (false)
    {}

    bool bVariant;
    CEString sMainSymbol;
    CEString sInflectionSymbol; 
            // usually same as MainSymbol, but may differ, e.g., б<абий п <мс 6*а>
    CEString sAltInflectionSymbol;
    bool bPluralOf;
    bool bIntransitive;
    bool bReflexive;
//    ET_MainSymbol e_MainSymbol;
    ET_MainSymbol eInflectionSymbol;
    CEString sMainSymbolPluralOf;
    CEString sAltMainSymbol;
    ET_PartOfSpeech ePartOfSpeech;
    CEString sComment;
    CEString sCognate;          // (_от_ ...); (_женск. к_ ...)
    CEString sAltMainSymbolComment;
    CEString sAltInflectionComment;
    CEString sGraphicStem;

    StInflectionGroup stInflectionGroup;
    StInflectionGroup stAltInflectionGroup;

    CEString sVerbAlternation;
    bool bPastPartPassZhD;

    int iSection;  // number after "#"

    bool bNoComparative;
    bool bNoLongForms;
    bool bAssumedForms;
    bool bYo;
    bool bO;
    bool bGen2;
    bool bLoc2;
    bool bLoc2Optional;
    bool bHasIrregularForms;
    bool bHasDifficulties;
    bool bHasDeficiencies;

    bool bIsIterative;           // _многокр._
    bool bIsImpersonal;          // _безл._

    CEString sLoc2Preposition;
        
    CEString sDeficient;         // List of nonexistent forms
    CEString sDifficulties;      // List of forms that usually are not used
    CEString sObsolete;      // List of obsolete forms
    CEString sColloquial;        // List of colloquial forms ("<ехай")
    CEString sIrregularForms;    // @ [triangle], list of pairs <_parameter_ value>
    CEString sAltIrregularForms; // @ [triangle], forms parallel to the regular ones; list of pairs <_parameter_ value>
    CEString sRestrictedForms;   // % [diamond]
    CEString sContexts;          // Context(s) after :
    bool bHasAspectPair;
    vector<int> vecAspectPairType;             // Number after $ (negative if Roman, 0 if $ lacks a number)
                                               // a.k.a. hatched circle; aspect pair info
    vector<CEString> vecAspectPairComment;     // Comment after $, if any
    int iSharp;                   // Number after # -- redundant, see i_Section

    CEString sTrailingComment;

//    ST_XRef st_XRef;

    bool bSaveToDb (CSqlite * pDbHandle, __int64 llWordId, __int64& llDescriptorId, __int64& llInflectionId, __int64& llAltInflectionId);
    bool bSaveInflectionGroup (CSqlite * pDbHandle, __int64 llDescriptorId, const StInflectionGroup& stData, bool bIsPrimary, __int64& llInflectionId);
    bool bSaveIrregularStressData (CSqlite * pDbHandle, __int64 llFormId, const map<int, ET_StressType>& mapStress);
    bool bSaveIrregularForms (CSqlite * pDbHandle, __int64 llDescriptorId, bool bIsAlternative);
    bool bSaveDifficultForms (CSqlite * pDbHandle, __int64 llDescriptorId, int iType);
    bool bSaveNonexistentForms (CSqlite * pDbHandle, __int64 llDescriptorId);

};  //  struct StDescriptor

class CZalEntryParser
{
public:
    CZalEntryParser (const CEString& sEntry, multimap<StHeadword *, StDescriptor>& mmapDictionary, bool bSecondPass = false) 
        : m_sSource (sEntry), m_mmapDictionary(mmapDictionary), m_bSecondPass (bSecondPass)
    {
        Init();
        m_pstHead = new StHeadword;
    };

    void SetOriginationRawHeadword (const CEString& sSourceHead);
    bool bParse (const CEString& sHead = L"", const CEString sUsageComment = L"");
//    bool bSaveLexemeHash (StHeadword&, StDescriptor&, __int64 llDescriptorId);

private:

protected:
    CSqlite * m_pDb;
    StHeadword * m_pstHead;
    StDescriptor m_stEntry;
    bool m_bSecondPass;
    bool m_bCommaAfterMainSymbol;
    CEString m_sSource;
    map<CEString, ET_PartOfSpeech> m_mapPos;
    map<CEString, ET_MainSymbol> m_mapMainSymbol;
    map<CEString, ET_AccentType> m_mapAccentType;

//    StHeadword st_Head;
//    vector<StDescriptor> m_vecDescriptors;
    multimap<StHeadword *, StDescriptor>& m_mmapDictionary;
//    vector<ST_XRef> vec_XReferences;

//    StLexemeHasher m_stLexemeHasher;

protected:
    void Init();

    bool bExtractHead (CEString& sSource);
    bool bExtractStress (CEString& sHeadWord); 
    bool bCheckMnOt (CEString& sSource);
    bool bGetMainSymbol (CEString& sSource, bool bInflectionTypeMismatch);
    bool bCheckMainSymbComment (CEString& sSource);
    bool bCheckInflectionType (CEString& sSource);
    bool bParseInflectionGroup (CEString& sDescriptor);
    bool bCheckInflectedParts (CEString& sSource);
    bool bCheckCircledDigits (CEString& sDescriptor, StInflectionGroup&);
    bool bCheckTilde (CEString& sDescriptor);
    bool bCheckSquareBrackets (CEString& sDescriptor);
    bool bCheckHatchedCircle (CEString& sDescriptor);
    bool bCheckVerbType (CEString& sDescriptor); // безл., многокр.
    bool bCheckCognate (CEString& sDescriptor);
    bool bCheckSharp (CEString& sDescriptor);
    bool bCheckDifficulties (CEString& sDescriptor);
    bool bCheckTriangle (CEString& sDescriptor, bool bAlternative = false);
    bool bCheckColon (CEString& sDescriptor);
    bool bCheckPercent (CEString& sDescriptor);
    bool bCheckDollar (CEString& sDescriptor);
    bool bCheckYo (CEString& sDescriptor);
    bool bCheckSecondCases (CEString& sDescriptor);
    bool bDeleteDoubleStress (CEString& sSource);
    bool bDetalizeDoubleSlashes(CEString& sSource);
    bool bDetalizeSlashes (CEString& xstr_source);
    bool bDetalizeCategories (CEString& sSource);
    bool bCheckComma (CEString& sDescriptor);
    bool bMakeGraphicStem();
//    bool bSaveToDb (StHeadword& stHead, vector<StDescriptor>& vecDescriptors);
    bool bSaveToDb();
//    bool b_SaveXReference (StHeadword&, ST_XRef&, __int64 ll_descriptorId);

};
