// This is the main DLL file.

#include "stdafx.h"
#include <msclr\marshal.h>
#include "GramHasher.h"
#include "MainLibManaged.h"

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace std;
using namespace MainLibManaged;

extern "C"
{
    ET_ReturnCode GetDictionary(Hlib::IDictionary *&);        // the only external function defined in MainLib
}

CWordFormManaged::CWordFormManaged(IWordForm * pWf) : m_pWordForm(pWf)
{}

CWordFormManaged::~CWordFormManaged()
{}

CLexemeManaged^ CWordFormManaged::Lexeme()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew CLexemeManaged(m_pWordForm->pLexeme());
}

String^ CWordFormManaged::sWordForm()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String (m_pWordForm->sWordForm());
}

String^ CWordFormManaged::sLemma()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String(m_pWordForm->sLemma());
}

__int64 CWordFormManaged::llLexemeId()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->llLexemeId();
}

EM_PartOfSpeech CWordFormManaged::ePos()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_PartOfSpeech)m_pWordForm->ePos();
}

EM_Case CWordFormManaged::eCase()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Case)m_pWordForm->eCase();
}

EM_Number CWordFormManaged::eNumber()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Number)m_pWordForm->eNumber();
}

EM_Subparadigm CWordFormManaged::eSubparadigm()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Subparadigm)m_pWordForm->eSubparadigm();
}

EM_Gender CWordFormManaged::eGender()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Gender)m_pWordForm->eGender();
}

EM_Person CWordFormManaged::ePerson()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Person)m_pWordForm->ePerson();
}

EM_Animacy CWordFormManaged::eAnimacy()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Animacy)m_pWordForm->eAnimacy();
}

EM_Reflexive CWordFormManaged::eReflexive()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Reflexive)m_pWordForm->eReflexive();
}

EM_Aspect CWordFormManaged::eAspect()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Aspect)m_pWordForm->eAspect();
}

EM_Status CWordFormManaged::eStatus()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Status)m_pWordForm->eStatus();
}

bool CWordFormManaged::bIrregular()      // came from the DB as opposed to being generated by the app
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->bIrregular();
}

EM_ReturnCode CWordFormManaged::eGetFirstStressPos(int% iPos, EM_StressType% eType)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    int cppiPos = -1;
    ET_StressType cppeType = ET_StressType::STRESS_TYPE_UNDEFINED;
    ET_ReturnCode eRet = m_pWordForm->eGetFirstStressPos(cppiPos, cppeType);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
        eType = (EM_StressType)cppeType;
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CWordFormManaged::eGetNextStressPos(int% iPos, EM_StressType% eType)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    int cppiPos = -1;
    ET_StressType cppeType = ET_StressType::STRESS_TYPE_UNDEFINED;
    ET_ReturnCode eRet = m_pWordForm->eGetNextStressPos(cppiPos, cppeType);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
        eType = (EM_StressType)cppeType;
    }

    return (EM_ReturnCode)eRet;
}

int CWordFormManaged::iGramHash()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->iGramHash();
}

EM_ReturnCode CWordFormManaged::eInitFromHash(int iHash)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_ReturnCode)m_pWordForm->eInitFromHash(iHash);
}

CDictionaryManaged::CDictionaryManaged()
{
    Hlib::IDictionary * pD = NULL;
    ET_ReturnCode rc = GetDictionary(pD);
    m_pDictionary = pD;
}

CDictionaryManaged::~CDictionaryManaged()
{}

EM_ReturnCode CDictionaryManaged::eSetDbPath(String^ sDbPath)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    // This conversion requires an instance of marshal_context class
    marshal_context^ mc = gcnew marshal_context();
    CEString cestrDbPath(mc->marshal_as<const wchar_t *>(sDbPath));
    delete mc;

    return (EM_ReturnCode)m_pDictionary->eSetDbPath(cestrDbPath);
}

String^ CDictionaryManaged::sGetDbPath()
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return gcnew String(m_pDictionary->sGetDbPath());
}

EM_ReturnCode CDictionaryManaged::eGetLexemeById(int iId)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)m_pDictionary->eGetLexemeById(iId);
}

EM_ReturnCode CDictionaryManaged::eGetLexemesByHash(String^ sHash)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    // This conversion requires an instance of marshal_context class
    marshal_context^ mc = gcnew marshal_context();
    CEString cestrHash(mc->marshal_as<const wchar_t *>(sHash));
    delete mc;

    return (EM_ReturnCode)m_pDictionary->eGetLexemesByHash(cestrHash);
}

EM_ReturnCode CDictionaryManaged::eGetLexemesByGraphicStem(String^ sGrStem)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    // This conversion requires an instance of marshal_context class
    marshal_context^ mc = gcnew marshal_context();
    CEString cestrGrStem(mc->marshal_as<const wchar_t *>(sGrStem));
    delete mc;

    return (EM_ReturnCode)m_pDictionary->eGetLexemesByGraphicStem(cestrGrStem);
}

EM_ReturnCode CDictionaryManaged::eGetLexemesByInitialForm(String^ sInitForm)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    // This conversion requires an instance of marshal_context class
    marshal_context^ mc = gcnew marshal_context();
    CEString cestrInitForm(mc->marshal_as<const wchar_t *>(sInitForm));
    delete mc;

    return (EM_ReturnCode)m_pDictionary->eGetLexemesByInitialForm(cestrInitForm);
}

EM_ReturnCode CDictionaryManaged::eGenerateAllForms()
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)m_pDictionary->eGenerateAllForms();
}

EM_ReturnCode CDictionaryManaged::eGenerateFormsForSelectedLexemes()
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)m_pDictionary->eGenerateFormsForSelectedLexemes();
}

EM_ReturnCode CDictionaryManaged::eCountLexemes(int% iLexemes)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    int cppiLexemes = 0;
    ET_ReturnCode eRet = m_pDictionary->eCountLexemes(cppiLexemes);
    if (H_NO_ERROR == eRet)
    {
        iLexemes = cppiLexemes;
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eGetFirstLexeme(CLexemeManaged^% lexeme)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ILexeme * pLexeme = NULL;
    ET_ReturnCode eRet = m_pDictionary->eGetFirstLexeme(pLexeme);
    if (H_NO_ERROR == eRet)
    {
        if (pLexeme)
        {
            lexeme = gcnew CLexemeManaged(pLexeme);
        }
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eGetNextLexeme(CLexemeManaged^% lexeme)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ILexeme * pLexeme = NULL;
    ET_ReturnCode eRet = m_pDictionary->eGetNextLexeme(pLexeme);
    if (H_NO_ERROR == eRet)
    {
        if (pLexeme)
        {
            lexeme = gcnew CLexemeManaged(pLexeme);
        }
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eAnalyze(String^ sForm)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    // This conversion requires an instance of marshal_context class
    marshal_context^ mc = gcnew marshal_context();
    CEString cestrForm(mc->marshal_as<const wchar_t *>(sForm));
    delete mc;

    return (EM_ReturnCode)m_pDictionary->eAnalyze(cestrForm);
}

void CDictionaryManaged::Clear()
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    m_pDictionary->Clear();
}

EM_ReturnCode CDictionaryManaged::eGetVerifier(IVerifier *& pVerifier)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)m_pDictionary->eGetVerifier(pVerifier);
}

EM_ReturnCode CDictionaryManaged::eExportTestData(String^ sPath, DelegateProgress^ progressCallback)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    // This conversion requires an instance of marshal_context class
    marshal_context^ mc = gcnew marshal_context();
    CEString cestrDbPath(mc->marshal_as<const wchar_t *>(sPath));
    delete mc;

    GCHandle gch = GCHandle::Alloc(progressCallback);
    IntPtr iptr = Marshal::GetFunctionPointerForDelegate(progressCallback);
    CProgressCallback * pProgress = static_cast<CProgressCallback*>(iptr.ToPointer());

    return (EM_ReturnCode)m_pDictionary->eExportTestData(cestrDbPath, *pProgress);
}

EM_ReturnCode CDictionaryManaged::eImportTestData(String^ sPath, DelegateProgress^ progressCallback)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    // This conversion requires an instance of marshal_context class
    marshal_context^ mc = gcnew marshal_context();
    CEString cestrDbPath(mc->marshal_as<const wchar_t *>(sPath));
    delete mc;

    GCHandle gch = GCHandle::Alloc(progressCallback);
    IntPtr iptr = Marshal::GetFunctionPointerForDelegate(progressCallback);
    CProgressCallback * pProgress = static_cast<CProgressCallback*>(iptr.ToPointer());

    return (EM_ReturnCode)m_pDictionary->eImportTestData(cestrDbPath, *pProgress);
}


//
// Managed wrapper for Lexeme class
//
CLexemeManaged::CLexemeManaged(ILexeme * pLexeme) : m_pLexeme(pLexeme)
{}

CLexemeManaged::~CLexemeManaged()
{}

/*
const StLexemeProperties& CLexemeManaged::stGetProperties()
{
//    m_pLexeme->stGetProperties();
//    &&&& Need to create a new properties object and assign
}

StLexemeProperties& CLexemeManaged::stGetPropertiesForWriteAccess()
{
//    &&&& Need to create a new properties object and assign
}
*/

EM_Gender CLexemeManaged::eGender()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Gender)m_pLexeme->eGender();
}
 
EM_Animacy CLexemeManaged::eAnimacy()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Animacy)m_pLexeme->eAnimacy();
}
 
String^ CLexemeManaged::sGraphicStem()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sGraphicStem());
}
 
bool CLexemeManaged::bHasIrregularForms()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasIrregularForms();
}

bool CLexemeManaged::bHasSecondaryStress()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasSecondaryStress();
}
bool CLexemeManaged::bHasFleetingVowel()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasFleetingVowel();
}
 
bool CLexemeManaged::bHasYoAlternation()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasYoAlternation();
}
 
bool CLexemeManaged::bHasOAlternation()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasOAlternation();
}
 
String^ CLexemeManaged::sSourceForm()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sSourceForm());
}
 
 //    CSqlite * CLexemeManaged::pGetDb()
 //{}
 
String^ CLexemeManaged::sHeadwordComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sHeadwordComment());
}
 
bool CLexemeManaged::bIsUnstressed()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsUnstressed();
}
 
bool CLexemeManaged::bIsVariant()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsVariant();
}
 
String^ CLexemeManaged::sMainSymbol()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sMainSymbol());
}
 
bool CLexemeManaged::bIsPluralOf()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsPluralOf();
}
 
bool CLexemeManaged::bTransitive()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bTransitive();
}
 
EM_Reflexive CLexemeManaged::eIsReflexive()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Reflexive)m_pLexeme->eIsReflexive();
}
 
String^ CLexemeManaged::sMainSymbolPluralOf()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sMainSymbolPluralOf());
}
 
String^ CLexemeManaged::sAltMainSymbol()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAltMainSymbol());
}
 
EM_Aspect CLexemeManaged::eAspect()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Aspect)m_pLexeme->eAspect();
}
 
String^ CLexemeManaged::sInflectionType()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sInflectionType());
}
 
EM_PartOfSpeech CLexemeManaged::ePartOfSpeech()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_PartOfSpeech)m_pLexeme->ePartOfSpeech();
}
 
String^ CLexemeManaged::sComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sComment());
}
 
String^ CLexemeManaged::sAltMainSymbolComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAltMainSymbolComment());
}
 
String^ CLexemeManaged::sAltInflectionComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAltInflectionComment());
}
 
String^ CLexemeManaged::sVerbStemAlternation()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sVerbStemAlternation());
}
 
bool CLexemeManaged::bPartPastPassZhd()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bPartPastPassZhd();
}
 
int CLexemeManaged::iSection()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->iSection();
}
 
bool CLexemeManaged::bNoComparative()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bNoComparative();
}
 
bool CLexemeManaged::bAssumedForms()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bAssumedForms();
}
 
bool CLexemeManaged::bYoAlternation()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bYoAlternation();
}
 
bool CLexemeManaged::bOAlternation()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bOAlternation();
}
 
bool CLexemeManaged::bSecondGenitive()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bSecondGenitive();
}
 
bool CLexemeManaged::bSecondLocative()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bSecondLocative();
}
 
bool CLexemeManaged::bSecondLocativeOptional()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bSecondLocativeOptional();
}
 
String^ CLexemeManaged::sLoc2Preposition()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sLoc2Preposition());
}
 
bool CLexemeManaged::bHasAspectPair()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasAspectPair();
}
 
int CLexemeManaged::iAspectPairType()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return m_pLexeme->iAspectPairType();
}
 
String^ CLexemeManaged::sAspectPairComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAspectPairComment());
}
 
String^ CLexemeManaged::sQuestionableForms()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sQuestionableForms());
}
 
bool CLexemeManaged::bHasIrregularVariants()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasIrregularVariants();
}
 
bool CLexemeManaged::bHasDeficiencies()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasDeficiencies();
}
 
String^ CLexemeManaged::sRestrictedForms()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sRestrictedForms());
}
 
String^ CLexemeManaged::sContexts()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sContexts());
}
 
String^ CLexemeManaged::sTrailingComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sTrailingComment());
}
 
int CLexemeManaged::iInflectionId()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->iInflectionId();
}
 
bool CLexemeManaged::bPrimaryInflectionGroup()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bPrimaryInflectionGroup();
}
 
int CLexemeManaged::iType()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->iType();
}
 
EM_AccentType CLexemeManaged::eAccentType1()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_AccentType)m_pLexeme->eAccentType1();
}
 
EM_AccentType CLexemeManaged::eAccentType2()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_AccentType)m_pLexeme->eAccentType2();
}
 
bool CLexemeManaged::bShortFormsRestricted()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bShortFormsRestricted();
}
 
bool CLexemeManaged::bPastParticipleRestricted()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bPastParticipleRestricted();
}
 
bool CLexemeManaged::bNoLongForms()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bNoLongForms();
}
 
bool CLexemeManaged::bNoShortForms()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bNoShortForms();
}
 
bool CLexemeManaged::bNoPastParticiple()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bNoPastParticiple();
}
 
bool CLexemeManaged::bFleetingVowel()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bFleetingVowel();
}
 
int CLexemeManaged::iStemAugment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->iStemAugment();
}
 
String^ CLexemeManaged::s1SgStem()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->s1SgStem());
}
 
String^ CLexemeManaged::s3SgStem()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->s3SgStem());
}
 
String^ CLexemeManaged::sInfinitive()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sInfinitive());
}
 
String^ CLexemeManaged::sInfStem()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sInfStem());
}
 
bool CLexemeManaged::bFindCommonDeviation(int iNum, bool% bIsOptionalDotNet)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    bool bIsOptionalCpp = false;
    bool bRet = m_pLexeme->bFindCommonDeviation(iNum, bIsOptionalCpp);
    bIsOptionalDotNet = bIsOptionalCpp;

    return bRet;
}
 
bool CLexemeManaged::bFindStandardAlternation(String^ sKey, String^% sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    // This conversion requires an instance of marshal_context class
    marshal_context^ mc = gcnew marshal_context();
    CEString cestrKey(mc->marshal_as<const wchar_t *>(sKey));
    delete mc;

    CEString cestrValue;
    bool bRet = m_pLexeme->bFindStandardAlternation(cestrKey, cestrValue);
    sValue = gcnew String(sValue);

    return bRet;
}

EM_ReturnCode CLexemeManaged::eGetStemStressPositions(String^ sLemma, List<int>% listPositions)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    // This conversion requires an instance of marshal_context class
    marshal_context^ mc = gcnew marshal_context();
    CEString cestrLemma(mc->marshal_as<const wchar_t *>(sLemma));
    delete mc;

    vector<int> vecPositions;
    ET_ReturnCode eRet = m_pLexeme->eGetStemStressPositions(cestrLemma, vecPositions);
    if (H_NO_ERROR == eRet)
    {
        for (vector<int>::iterator it = vecPositions.begin(); it != vecPositions.end(); ++it)
        {
            listPositions.Add(*it);
        }
    }

    return (EM_ReturnCode)eRet;
}
 
EM_ReturnCode CLexemeManaged::eGetAlternatingPreverb(String^ sVerbForm, String^% sPreverb, bool% bVoicing)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    // This conversion requires an instance of marshal_context class
    marshal_context^ mc = gcnew marshal_context();
    CEString cestrVerbForm(mc->marshal_as<const wchar_t *>(sVerbForm));
    delete mc;

    CEString cestrPreverb;
    bool cppbVoicing;
    ET_ReturnCode eRet = m_pLexeme->eGetAlternatingPreverb(cestrVerbForm, cestrPreverb, cppbVoicing);
    if (H_NO_ERROR == eRet)
    {
        sPreverb = gcnew String(cestrPreverb);
        bVoicing = cppbVoicing;
    }

    return (EM_ReturnCode)eRet;
}
 
String^ CLexemeManaged::sHash()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sHash());
}
 
EM_ReturnCode CLexemeManaged::eWordFormFromHash(int iHash, unsigned int uiAt, CWordFormManaged^% wf)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    IWordForm * pWf = NULL;
    ET_ReturnCode eRet = m_pLexeme->eWordFormFromHash(iHash, uiAt, pWf);
    if (H_NO_ERROR == eRet)
    {
        if (pWf)
        {
            wf = gcnew CWordFormManaged(pWf);
        }
    }

    return (EM_ReturnCode)eRet;
}
 
bool CLexemeManaged::bHasIrregularForm(int iGramHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasIrregularForm(iGramHash);
}
 
bool CLexemeManaged::bNoRegularForms(int iGramHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bNoRegularForms(iGramHash);
}
 
EM_ReturnCode CLexemeManaged::eGetFirstWordForm(CWordFormManaged^% wf)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    IWordForm * pWf = NULL;
    ET_ReturnCode eRet = m_pLexeme->eGetFirstWordForm(pWf);
    if (H_NO_ERROR == eRet)
    {
        if (pWf)
        {
            wf = gcnew CWordFormManaged(pWf);
        }
    }

    return (EM_ReturnCode)eRet;
}
 
EM_ReturnCode CLexemeManaged::eGetNextWordForm(CWordFormManaged^% wf)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    IWordForm * pWf = NULL;
    ET_ReturnCode eRet = m_pLexeme->eGetNextWordForm(pWf);
    if (H_NO_ERROR == eRet)
    {
        if (pWf)
        {
            wf = gcnew CWordFormManaged(pWf);
        }
    }

    return (EM_ReturnCode)eRet;
}
 
EM_ReturnCode CLexemeManaged::eGetFirstIrregularForm(int iHash, CWordFormManaged^% wf, bool% bIsOptional)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    IWordForm * pWf = NULL;
    bool cppbIsOptional = false;
    ET_ReturnCode eRet = m_pLexeme->eGetFirstIrregularForm(iHash, pWf, cppbIsOptional);
    if (H_NO_ERROR == eRet)
    {
        if (pWf)
        {
            wf = gcnew CWordFormManaged(pWf);
            wf = gcnew CWordFormManaged(pWf);
        }
    }

    return (EM_ReturnCode)eRet;
}
 
EM_ReturnCode CLexemeManaged::eGetNextIrregularForm(CWordFormManaged^% wf, bool% bIsOptional)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    
    IWordForm * pWf = NULL;
    bool cppbIsOptional = false;
    ET_ReturnCode eRet = m_pLexeme->eGetNextIrregularForm(pWf, cppbIsOptional);
    if (H_NO_ERROR == eRet)
    {
        if (pWf)
        {
            wf = gcnew CWordFormManaged(pWf);
            bIsOptional = cppbIsOptional;
        }
    }

    return (EM_ReturnCode)eRet;
}
 
unsigned int CLexemeManaged::uiFormCount(int iHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->uiFormCount(iHash);
}
 
bool CLexemeManaged::bHasCommonDeviation(int iCd)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasCommonDeviation(iCd);
}
 
bool CLexemeManaged::bDeviationOptional(int iCd)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bDeviationOptional(iCd);
}
 
bool CLexemeManaged::bIsFormMissing(int iGramHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsFormMissing(iGramHash);
}
 
bool CLexemeManaged::bIsMultistressedCompound()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsMultistressedCompound();
}
 
EM_ReturnCode CLexemeManaged::eGetFirstStemStressPos(int% iPos)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = m_pLexeme->eGetFirstStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }

    return (EM_ReturnCode)eRet;
}
 
EM_ReturnCode CLexemeManaged::eGetNextStemStressPos(int% iPos)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = m_pLexeme->eGetNextStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }

    return (EM_ReturnCode)eRet;
}
 
//EM_ReturnCode CLexemeManaged::eSetDb(const CEString& sDbPath)
//{
//    if (NULL == m_pLexeme)
//    {
//        throw gcnew Exception(L"Lexeme object is NULL.");
//    }
//}

EM_ReturnCode CLexemeManaged::eGenerateParadigm()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_ReturnCode)m_pLexeme->eGenerateParadigm();
}

EM_ReturnCode CLexemeManaged::eSaveTestData()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_ReturnCode)m_pLexeme->eSaveTestData();
}


//
// Managed wrapper for the Gram hasher class
//

CGramHasherManaged::CGramHasherManaged()
{
    m_pHasher = new CGramHasher;
}

CGramHasherManaged::~CGramHasherManaged()
{
    delete m_pHasher;
}

void  CGramHasherManaged::SetPartOfSpeech(EM_PartOfSpeech ePos)
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_ePos = (ET_PartOfSpeech)ePos;
}

void CGramHasherManaged::SetSubparadigm(EM_Subparadigm eSp)
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eSubparadigm = (ET_Subparadigm)eSp;
}

void CGramHasherManaged::SetCase(EM_Case eCase)
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eCase = (ET_Case)eCase;
}

void CGramHasherManaged::SetNumber(EM_Number eNumber)
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eNumber = (ET_Number)eNumber;
}

void CGramHasherManaged::SetGender(EM_Gender eGender)
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eGender = (ET_Gender)eGender;
}

void CGramHasherManaged::SetPerson(EM_Person ePerson)
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_ePerson = (ET_Person)ePerson;
}

void CGramHasherManaged::SetAnimacy(EM_Animacy eAnimacy)
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eAnimacy = (ET_Animacy)eAnimacy;
}

void CGramHasherManaged::SetReflexivity(EM_Reflexive eRefl)
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eReflexive = (ET_Reflexive)eRefl;
}

void CGramHasherManaged::SetAspect(EM_Aspect eAspect)
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eAspect = (ET_Aspect)eAspect;
}

int CGramHasherManaged::iHash()
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    return m_pHasher->iGramHash();
}

/*
EM_ReturnCode CSqliteManaged::eExportTable(string sPathToTextFile, array<String^>^ arrTableNames)
{
    if (NULL == m_pDb)
    {
        throw gcnew Exception(L"Sqlite` object is NULL.");
    }

    // This conversion requires an instance of marshal_context class
    marshal_context^ mc = gcnew marshal_context();

    vector<CEString> vecTableNames;
    for (int iTab = 0; iTab < arrTableNames->GetLength(0); ++iTab)
    {
        CEString cestrTableName(mc->marshal_as<const wchar_t *>(arrTableNames[iTab]));
        vecTableNames.push_back(cestrTableName);
    }

//    CEString cestrPathToTextFile(mc->marshal_as<const wchar_t *>(sPathToTextFile));

    
    DelegateUpdateProgressBar(int iPercentDone);

    ProgressCallback ^ progressCallback =
        gcnew ProgressCallback(this, m_pDb->);
    GCHandle gcHandle = GCHandle::Alloc(bytesTransferredCb);
    IntPtr callbackPtr = Marshal::GetFunctionPointerForDelegate(bytesTransferredCb);
    typedef void(__stdcall *BYTES_TRANSFERRED_CALLBACK)(int iBytesTransferred);
    BYTES_TRANSFERRED_CALLBACK pUnmanagedCallback = (BYTES_TRANSFERRED_CALLBACK)(callbackPtr.ToPointer());
    

//    bool bRet = m_pDb->bExportTables(cestrPathToTextFile, vecTableNames, CProgressCallback& Progress);

//    delete mc;

    return EM_ReturnCode::H_NO_ERROR;
}
*/