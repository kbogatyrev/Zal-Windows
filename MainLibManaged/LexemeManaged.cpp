#pragma once

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#define _SILENCE_CXX17_STRSTREAM_DEPRECATION_WARNING

#define NOMINMAX

#include <memory>
#include <msclr\marshal.h>
#include "GramHasher.h"
//#include "MainLibManaged.h"

//#include "Dictionary.h"
//#include "Lexeme.h"
//#include "Inflection.h"
//#include "WordForm.h"
//#include "Gramhasher.h"
//#include "Parser.h"
//#include "Analytics.h"
//#include "Verifier.h"
//#include "AspectPair.h"
//#include "..\Hlib\SqliteWrapper.h"
//#include "EnumsManaged.h"

#include "UtilsManaged.h"
#include "InflectionManaged.h"
#include "LexemeManaged.h"

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace MainLibManaged;
using namespace std;

#define NOMINMAX


//
// Managed wrapper for Lexeme class
//
CLexemeManaged::CLexemeManaged()
{}

CLexemeManaged::CLexemeManaged(shared_ptr<CLexeme> spLexeme) : m_pLexeme(&spLexeme)
{}

CLexemeManaged::~CLexemeManaged()
{
    delete m_pLexeme;
    m_pLexeme = nullptr;
    //    m_sStoredEntryHash = "";
}

shared_ptr<CLexeme> CLexemeManaged::spLexeme()
{
    return *m_pLexeme;
}

EM_ReturnCode CLexemeManaged::eCreateInflectionEnumerator(CInflectionEnumeratorManaged^% pIeManaged)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Dictionary objectis NULL.");
    }

    shared_ptr<CInflectionEnumerator> spInflectionEnumerator;
    ET_ReturnCode eRet = (*m_pLexeme)->eCreateInflectionEnumerator(spInflectionEnumerator);
    if (H_NO_ERROR == eRet)
    {
        if (spInflectionEnumerator)
        {
            pIeManaged = gcnew CInflectionEnumeratorManaged(spInflectionEnumerator);
        }
        else
        {
            return EM_ReturnCode::H_ERROR_UNEXPECTED;
        }
    }
    return (EM_ReturnCode)eRet;
}


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

__int64 CLexemeManaged::llLexemeId()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->llLexemeId();
}

__int64 CLexemeManaged::llHeadwordId()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->llHeadwordId();
}

EM_Gender CLexemeManaged::eGender()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Gender)(*m_pLexeme)->eInflectionTypeToGender();
}

String^ CLexemeManaged::sGraphicStem()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sGraphicStem());
}

void CLexemeManaged::SetGraphicStem(String^ sGraphicStem)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetGraphicStem(sFromManagedString(sGraphicStem));
}

bool CLexemeManaged::bHasIrregularForms()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bHasIrregularForms();
}

void CLexemeManaged::SetHasIrregularForms(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetHasIrregularForms(bValue);
}

bool CLexemeManaged::bHasSecondaryStress()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bHasSecondaryStress();
}

//////////////////////////////////////////////////////////////////////////////////

bool CLexemeManaged::bHasYoAlternation()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bHasYoAlternation();
}

void CLexemeManaged::SetHasYoAlternation(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetHasYoAlternation(bValue);
}

bool CLexemeManaged::bHasOAlternation()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bHasOAlternation();
}

void CLexemeManaged::SetHasOAlternation(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetHasOAlternation(bValue);
}

String^ CLexemeManaged::sSourceForm()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sSourceForm());
}

void CLexemeManaged::SetSourceForm(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetSourceForm(sFromManagedString(sValue));
}

bool CLexemeManaged::bHasHomonyms()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->stGetProperties().vecHomonyms.size() > 0;
}

List<int>^ CLexemeManaged::arrHomonyms()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    List<int>^ lHomonyms = gcnew List<int>();
    for (int iHomonymNumber : (*m_pLexeme)->stGetProperties().vecHomonyms)
    {
        lHomonyms->Add(iHomonymNumber);
    }

    return lHomonyms;
}

void CLexemeManaged::SetHomonyms(List<int>^ arrHomonyms)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    StLexemeProperties& stProperties = (*m_pLexeme)->stGetPropertiesForWriteAccess();
    for each (int iHomonymNumber in arrHomonyms)
    {
        stProperties.vecHomonyms.push_back(iHomonymNumber);
    }
}

//    CSqlite * CLexemeManaged::pGetDb()
 //{}

String^ CLexemeManaged::sHeadwordComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sHeadwordComment());
}

void CLexemeManaged::SetHeadwordComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetHeadwordComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sHeadwordVariant()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sHeadwordVariant());
}

void CLexemeManaged::SetHeadwordVariant(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetHeadwordVariant(sFromManagedString(sValue));
}

String^ CLexemeManaged::sHeadwordVariantComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sHeadwordVariantComment());
}

void CLexemeManaged::SetHeadwordVariantComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetHeadwordVariantComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sPluralOf()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sPluralOf());
}

void CLexemeManaged::SetPluralOf(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetPluralOf(sFromManagedString(sValue));
}

String^ CLexemeManaged::sUsage()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sUsage());
}

void CLexemeManaged::SetUsage(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetUsage(sFromManagedString(sValue));
}

String^ CLexemeManaged::sSeeRef()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sSeeRef());
}

void CLexemeManaged::SetSeeRef(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetSeeRef(sFromManagedString(sValue));
}

String^ CLexemeManaged::sBackRef()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sBackRef());
}

void CLexemeManaged::SetBackRef(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetBackRef(sFromManagedString(sValue));
}

bool CLexemeManaged::bIsUnstressed()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bIsUnstressed();
}

void CLexemeManaged::SetIsUnstressed(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->SetIsUnstressed(bValue);
}

bool CLexemeManaged::bIsVariant()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bIsVariant();
}

void CLexemeManaged::SetIsVariant(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetIsVariant(bValue);
}

String^ CLexemeManaged::sMainSymbol()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sMainSymbol());
}

void CLexemeManaged::SetMainSymbol(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetMainSymbol(sFromManagedString(sValue));
}

bool CLexemeManaged::bIsPluralOf()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bIsPluralOf();
}

void CLexemeManaged::SetIsPluralOf(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetIsPluralOf(bValue);
}

bool CLexemeManaged::bTransitive()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bTransitive();
}

void CLexemeManaged::SetTransitive(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->SetTransitive(bValue);
}

EM_Reflexive CLexemeManaged::eIsReflexive()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Reflexive)(*m_pLexeme)->eIsReflexive();
}

void CLexemeManaged::SetIsReflexive(EM_Reflexive eValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetIsReflexive((ET_Reflexivity)eValue);
}

String^ CLexemeManaged::sMainSymbolPluralOf()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sMainSymbolPluralOf());
}

void CLexemeManaged::SetMainSymbolPluralOf(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetMainSymbolPluralOf(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltMainSymbol()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sAltMainSymbol());
}

void CLexemeManaged::SetAltMainSymbol(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetAltMainSymbol(sFromManagedString(sValue));
}

EM_Aspect CLexemeManaged::eAspect()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Aspect)(*m_pLexeme)->eAspect();
}

void CLexemeManaged::SetAspect(EM_Aspect eValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->SetAspect((ET_Aspect)eValue);
}

String^ CLexemeManaged::sInflectionType()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sInflectionType());
}

void CLexemeManaged::SetInflectionType(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetInflectionType(sFromManagedString(sValue));
}

EM_PartOfSpeech CLexemeManaged::ePartOfSpeech()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_PartOfSpeech)(*m_pLexeme)->ePartOfSpeech();
}

void CLexemeManaged::SetPartOfSpeech(EM_PartOfSpeech eValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetPartOfSpeech((ET_PartOfSpeech)eValue);
}

String^ CLexemeManaged::sComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sComment());
}

void CLexemeManaged::SetComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltMainSymbolComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sAltMainSymbolComment());
}

void CLexemeManaged::SetAltMainSymbolComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetAltMainSymbolComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltInflectionComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sAltInflectionComment());
}

void CLexemeManaged::SetAltInflectionComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetAltInflectionComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sVerbStemAlternation()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sVerbStemAlternation());
}

void CLexemeManaged::SetVerbStemAlternation(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetVerbStemAlternation(sFromManagedString(sValue));
}

bool CLexemeManaged::bPartPastPassZhd()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bPartPastPassZhd();
}

void CLexemeManaged::SetPartPastPassZhd(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetPartPastPassZhd(bValue);
}

int CLexemeManaged::iSection()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->iSection();
}

void CLexemeManaged::SetSection(int iValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetSection(iValue);
}

bool CLexemeManaged::bNoComparative()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bNoComparative();
}

void CLexemeManaged::SetNoComparative(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetNoComparative(bValue);
}

bool CLexemeManaged::bSpryazhSm()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bSpryazhSm();
}

String^ CLexemeManaged::sSpryazhSmRefSource()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sSpryazhSmRefSource());
}


EM_Subparadigm CLexemeManaged::eSubparadigm()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Subparadigm)(*m_pLexeme)->eSubparadigm();
}

void CLexemeManaged::SetSubparadigm(EM_Subparadigm eSp)    // currently only used with numerals
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetSubparadigm((ET_Subparadigm)eSp);
}

bool CLexemeManaged::bAssumedForms()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bAssumedForms();
}

void CLexemeManaged::SetAssumedForms(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetAssumedForms(bValue);
}

//bool CLexemeManaged::bYoAlternation()
//{
//    if (nullptr == m_pLexeme)
//    {
//        throw gcnew Exception(L"Lexeme object is NULL.");
//    }

//    return m_pLexeme->bYoAlternation();
//}

//void CLexemeManaged::SetYoAlternation(bool bValue)
//{
//    if (nullptr == m_pLexeme)
//    {
//        throw gcnew Exception(L"Lexeme object is NULL.");
//    }

//    m_pLexeme->SetYoAlternation(bValue);
//}

//bool CLexemeManaged::bOAlternation()
//{
//    if (nullptr == m_pLexeme)
//    {
//        throw gcnew Exception(L"Lexeme object is NULL.");
//    }

//    return m_pLexeme->bOAlternation();
//}

bool CLexemeManaged::bSecondGenitive()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bSecondGenitive();
}

void CLexemeManaged::SetSecondGenitive(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetSecondGenitive(bValue);
}

bool CLexemeManaged::bSecondPrepositional()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bSecondPrepositional();
}

void CLexemeManaged::SetSecondPrepositional(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetSecondPrepositional(bValue);
}

bool CLexemeManaged::bSecondPrepositionalOptional()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bSecondPrepositionalOptional();
}

void CLexemeManaged::SetSecondPrepositionalOptional(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetSecondPrepositionalOptional(bValue);
}

String^ CLexemeManaged::sP2Preposition()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sP2Preposition());
}

void CLexemeManaged::SetP2Preposition(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetP2Preposition(sFromManagedString(sValue));
}

bool CLexemeManaged::bNoLongForms()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bNoLongForms();
}

void CLexemeManaged::SetNoLongForms(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetNoLongForms(bValue);
}

String^ CLexemeManaged::s1SgStem()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->s1SgStem());
}

void CLexemeManaged::Set1SgStem(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->Set1SgStem(sFromManagedString(sValue));
}

String^ CLexemeManaged::s3SgStem()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->s3SgStem());
}

void CLexemeManaged::Set3SgStem(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->Set3SgStem(sFromManagedString(sValue));
}

String^ CLexemeManaged::sInfinitive()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sInfinitive());
}

void CLexemeManaged::SetInfinitive(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetInfinitive(sFromManagedString(sValue));
}

String^ CLexemeManaged::sInfStem()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sInfStem());
}

void CLexemeManaged::SetInfStem(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetInfStem(sFromManagedString(sValue));
}

//int CLexemeManaged::iInflectedParts()
//{
//    if (nullptr == m_pLexeme)
//    {
//        throw gcnew Exception(L"Lexeme object is NULL.");
//    }
//
//    return m_pLexeme->iInflectedParts();
//}
//
//void CLexemeManaged::SetInflectedParts(int iValue)
//{
//    if (nullptr == m_pLexeme)
//    {
//        throw gcnew Exception(L"Lexeme object is NULL.");
//    }
//
//    m_pLexeme->SetInflectedParts(iValue);
//}
//
bool CLexemeManaged::bIsSecondPart()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bIsSecondPart();
}

void CLexemeManaged::SetIsSecondPart(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetIsSecondPart(bValue);
}

bool CLexemeManaged::bHasAspectPair()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bHasAspectPair();
}

void CLexemeManaged::SetHasAspectPair(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->SetHasAspectPair(bValue);
}

bool CLexemeManaged::bHasAltAspectPair()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bHasAltAspectPair();
}

int CLexemeManaged::iAspectPairType()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return (*m_pLexeme)->iAspectPairType();
}

void CLexemeManaged::SetAspectPairType(int iValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    (*m_pLexeme)->SetAspectPairType(iValue);
}

int CLexemeManaged::iAltAspectPairType()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return (*m_pLexeme)->iAltAspectPairType();
}

void CLexemeManaged::SetAltAspectPairType(int iValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    (*m_pLexeme)->SetAltAspectPairType(iValue);
}

String^ CLexemeManaged::sAltAspectPairComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sAltAspectPairComment());
}

void CLexemeManaged::SetAltAspectPairComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetAltAspectPairComment(sFromManagedString(sValue));
}

EM_ReturnCode CLexemeManaged::eGetAspectPair(String^% sAspectPair, int% iStressPos)
{
    if (!bHasAspectPair())
    {
        return EM_ReturnCode::H_FALSE;
    }

    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString sAp;
    int iSp = -1;
    ET_ReturnCode eRet = (*m_pLexeme)->eGetAspectPair(sAp, iSp);
    if (eRet != H_NO_ERROR)
    {
        return (EM_ReturnCode)eRet;
    }

    sAspectPair = gcnew String(sAp);
    iStressPos = iSp;

    return (EM_ReturnCode)eRet;

}       //  eGetAspectPair()

EM_ReturnCode CLexemeManaged::eGetAltAspectPair(String^% sAltAspectPair, int% iAltStressPos)
{
    if (!bHasAltAspectPair())
    {
        return EM_ReturnCode::H_FALSE;
    }

    CEString sAp;
    int iSp = -1;
    ET_ReturnCode eRet = (*m_pLexeme)->eGetAltAspectPair(sAp, iSp);
    if (eRet != H_NO_ERROR)
    {
        return (EM_ReturnCode)eRet;
    }

    sAltAspectPair = gcnew String(sAp);
    iAltStressPos = iSp;

    return (EM_ReturnCode)eRet;

}       //  eGetAltAspectPair()

String^ CLexemeManaged::sAspectPairData()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sAspectPairData());
}

void CLexemeManaged::SetAspectPairData(String^ sAspectPairData)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetAspectPairData(sFromManagedString(sAspectPairData));
}

bool CLexemeManaged::bHasIrregularVariants()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (*m_pLexeme)->bHasIrregularVariants();
}

void CLexemeManaged::SetHasIrregularVariants(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetHasIrregularVariants(bValue);
}

String^ CLexemeManaged::sRestrictedContexts()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sRestrictedContexts());
}

void CLexemeManaged::SetRestrictedContexts(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetRestrictedContexts(sFromManagedString(sValue));
}

String^ CLexemeManaged::sContexts()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sContexts());
}

void CLexemeManaged::SetContexts(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetContexts(sFromManagedString(sValue));
}

String^ CLexemeManaged::sTrailingComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String((*m_pLexeme)->sTrailingComment());
}

void CLexemeManaged::SetTrailingComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    (*m_pLexeme)->SetTrailingComment(sFromManagedString(sValue));
}

CLexemeEnumeratorManaged::CLexemeEnumeratorManaged(shared_ptr<CLexemeEnumerator> spLe) 
    : m_pLexemeEnumerator(&spLe)
{}

CLexemeEnumeratorManaged::~CLexemeEnumeratorManaged()
{
    if (nullptr == m_pLexemeEnumerator)
    {
        throw gcnew Exception(L"Lexeme enumerator object is NULL.");
    }
    delete m_pLexemeEnumerator;
}

EM_ReturnCode CLexemeEnumeratorManaged::eReset()
{
    if (nullptr == m_pLexemeEnumerator)
    {
        throw gcnew Exception(L"Lexeme enumerator object is NULL.");
    }
    return (EM_ReturnCode)(*m_pLexemeEnumerator)->eReset();
}

EM_ReturnCode CLexemeEnumeratorManaged::eGetFirstLexeme(CLexemeManaged^% lexeme)
{
    if (nullptr == m_pLexemeEnumerator)
    {
        throw gcnew Exception(L"Lexeme enumerator object is NULL.");
    }

    shared_ptr<CLexeme> spLexeme;
    ET_ReturnCode eRet = (*m_pLexemeEnumerator)->eGetFirstLexeme(spLexeme);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (spLexeme)
        {
            lexeme = gcnew CLexemeManaged(spLexeme);
        }
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeEnumeratorManaged::eGetNextLexeme(CLexemeManaged^% lexeme)
{
    if (nullptr == m_pLexemeEnumerator)
    {
        throw gcnew Exception(L"Lexeme enumerator object is NULL.");
    }

    shared_ptr<CLexeme> spLexeme;
    ET_ReturnCode eRet = (*m_pLexemeEnumerator)->eGetNextLexeme(spLexeme);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (spLexeme)
        {
            lexeme = gcnew CLexemeManaged(spLexeme);
        }
    }
    return (EM_ReturnCode)eRet;
}
