#pragma once

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#define _SILENCE_CXX17_STRSTREAM_DEPRECATION_WARNING

#define NOMINMAX

#include <memory>
#include <msclr\marshal.h>
#include "GramHasher.h"
#include "Singleton.h"

#include "UtilsManaged.h"
#include "InflectionManaged.h"
#include "LexemeManaged.h"

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace MainLibManaged;
using namespace std;

#define NOMINMAX

CInflectionEnumeratorManaged::CInflectionEnumeratorManaged(int64_t iHandle) : m_iHandle(iHandle)
{}

CInflectionEnumeratorManaged::~CInflectionEnumeratorManaged()
{
    m_iHandle = -1;
}

shared_ptr<CInflectionEnumerator> CInflectionEnumeratorManaged::spGetInstance()
{
    shared_ptr<CInflectionEnumerator> spInflectionEnumerator;
    auto rc = Singleton::pGetInstance()->eGetInflectionEnumerator(m_iHandle, spInflectionEnumerator);
    if (rc != H_NO_ERROR || nullptr == spInflectionEnumerator)
    {
        throw gcnew Exception(L"Unable to retrieve inflectionenumerator instance.");
    }
    return spInflectionEnumerator;
}

EM_ReturnCode CInflectionEnumeratorManaged::eReset()
{
    return (EM_ReturnCode)spGetInstance()->eReset();
}

EM_ReturnCode CInflectionEnumeratorManaged::eGetFirstInflection(CInflectionManaged^% inflection)
{
    shared_ptr<CInflection> spInflection;
    ET_ReturnCode eRet = spGetInstance()->eGetFirstInflection(spInflection);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (spInflection)
        {
            auto iHandle = Singleton::pGetInstance()->iAddInflection(spInflection);
            inflection = gcnew CInflectionManaged(iHandle);
        }
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CInflectionEnumeratorManaged::eGetNextInflection(CInflectionManaged^% inflection)
{
    shared_ptr<CInflection> spInflection;
    ET_ReturnCode eRet = spGetInstance()->eGetNextInflection(spInflection);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (spInflection)
        {
            auto iHandle = Singleton::pGetInstance()->iAddInflection(spInflection);
            inflection = gcnew CInflectionManaged(iHandle);
        }
    }
    return (EM_ReturnCode)eRet;
}

//
// Managed wrapper for Lexeme class
//
//CLexemeManaged::CLexemeManaged()
//{}

//CLexemeManaged::CLexemeManaged(CLexeme* pLexeme) 
//    : m_pLexeme(pLexeme)
//{}

CLexemeManaged::CLexemeManaged(int64_t iHandle) : m_iHandle(iHandle)
{
}
CLexemeManaged::~CLexemeManaged()
{
    Singleton::pGetInstance()->RemoveLexeme(m_iHandle);
    m_iHandle = -1;
}

shared_ptr<CLexeme> CLexemeManaged::spGetInstance()
{
    shared_ptr<CLexeme> spLexeme;
    auto rc = Singleton::pGetInstance()->eGetLexeme(m_iHandle, spLexeme);
    if (rc != H_NO_ERROR || nullptr == spLexeme)
    {
        throw gcnew Exception(L"Unable to retrieve lexeme instance.");
    }
    return spLexeme;
}

EM_ReturnCode CLexemeManaged::eCreateInflectionEnumerator(CInflectionEnumeratorManaged^% pLeManaged)
{
    shared_ptr<CInflectionEnumerator> spInflectionEnumerator;
    ET_ReturnCode rc = spGetInstance()->eCreateInflectionEnumerator(spInflectionEnumerator);
    if (H_NO_ERROR == rc && spInflectionEnumerator)
    {
        auto iHandle = Singleton::pGetInstance()->iAddInflectionEnumerator(spInflectionEnumerator);
        pLeManaged = gcnew CInflectionEnumeratorManaged(iHandle);
    }
    else
    {
        return EM_ReturnCode::H_ERROR_UNEXPECTED;
    }
    return (EM_ReturnCode)rc;
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
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return spLexeme->llLexemeId();
}

__int64 CLexemeManaged::llHeadwordId()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return spLexeme->llHeadwordId();
}

EM_Gender CLexemeManaged::eGender()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return (EM_Gender)spLexeme->eInflectionTypeToGender();
}

String^ CLexemeManaged::sGraphicStem()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(spLexeme->sGraphicStem());
}

void CLexemeManaged::SetGraphicStem(String^ sGraphicStem)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    spLexeme->SetGraphicStem(sFromManagedString(sGraphicStem));
}

bool CLexemeManaged::bHasIrregularForms()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return spLexeme->bHasIrregularForms();
}

void CLexemeManaged::SetHasIrregularForms(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    spLexeme->SetHasIrregularForms(bValue);
}

bool CLexemeManaged::bHasSecondaryStress()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return spLexeme->bHasSecondaryStress();
}

//////////////////////////////////////////////////////////////////////////////////

bool CLexemeManaged::bHasYoAlternation()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return spLexeme->bHasYoAlternation();
}

void CLexemeManaged::SetHasYoAlternation(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    spLexeme->SetHasYoAlternation(bValue);
}

bool CLexemeManaged::bHasOAlternation()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return spLexeme->bHasOAlternation();
}

void CLexemeManaged::SetHasOAlternation(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    spLexeme->SetHasOAlternation(bValue);
}

String^ CLexemeManaged::sSourceForm()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(spLexeme->sSourceForm());
}

void CLexemeManaged::SetSourceForm(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    spLexeme->SetSourceForm(sFromManagedString(sValue));
}

bool CLexemeManaged::bHasHomonyms()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return spLexeme->stGetProperties().vecHomonyms.size() > 0;
}

List<int>^ CLexemeManaged::arrHomonyms()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    List<int>^ lHomonyms = gcnew List<int>();
    for (int iHomonymNumber : spLexeme->stGetProperties().vecHomonyms)
    {
        lHomonyms->Add(iHomonymNumber);
    }
    return lHomonyms;
}

void CLexemeManaged::SetHomonyms(List<int>^ arrHomonyms)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    StLexemeProperties& stProperties = spLexeme->stGetPropertiesForWriteAccess();
    for each (int iHomonymNumber in arrHomonyms)
    {
        stProperties.vecHomonyms.push_back(iHomonymNumber);
    }
}

//    CSqlite * CLexemeManaged::pGetDb()
 //{}

String^ CLexemeManaged::sHeadwordComment()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sHeadwordComment());
}

void CLexemeManaged::SetHeadwordComment(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetHeadwordComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sHeadwordVariant()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sHeadwordVariant());
}

void CLexemeManaged::SetHeadwordVariant(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetHeadwordVariant(sFromManagedString(sValue));
}

String^ CLexemeManaged::sHeadwordVariantComment()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sHeadwordVariantComment());
}

void CLexemeManaged::SetHeadwordVariantComment(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetHeadwordVariantComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sPluralOf()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sPluralOf());
}

void CLexemeManaged::SetPluralOf(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetPluralOf(sFromManagedString(sValue));
}

String^ CLexemeManaged::sUsage()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sUsage());
}

void CLexemeManaged::SetUsage(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetUsage(sFromManagedString(sValue));
}

String^ CLexemeManaged::sSeeRef()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sSeeRef());
}

void CLexemeManaged::SetSeeRef(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetSeeRef(sFromManagedString(sValue));
}

String^ CLexemeManaged::sBackRef()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sBackRef());
}

void CLexemeManaged::SetBackRef(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetBackRef(sFromManagedString(sValue));
}

bool CLexemeManaged::bIsUnstressed()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bIsUnstressed();
}

void CLexemeManaged::SetIsUnstressed(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->SetIsUnstressed(bValue);
}

bool CLexemeManaged::bIsVariant()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bIsVariant();
}

void CLexemeManaged::SetIsVariant(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetIsVariant(bValue);
}

String^ CLexemeManaged::sMainSymbol()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sMainSymbol());
}

void CLexemeManaged::SetMainSymbol(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetMainSymbol(sFromManagedString(sValue));
}

bool CLexemeManaged::bIsPluralOf()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bIsPluralOf();
}

void CLexemeManaged::SetIsPluralOf(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetIsPluralOf(bValue);
}

bool CLexemeManaged::bTransitive()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bTransitive();
}

void CLexemeManaged::SetTransitive(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->SetTransitive(bValue);
}

EM_Reflexive CLexemeManaged::eIsReflexive()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Reflexive)spLexeme->eIsReflexive();
}

void CLexemeManaged::SetIsReflexive(EM_Reflexive eValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetIsReflexive((ET_Reflexivity)eValue);
}

String^ CLexemeManaged::sMainSymbolPluralOf()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sMainSymbolPluralOf());
}

void CLexemeManaged::SetMainSymbolPluralOf(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetMainSymbolPluralOf(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltMainSymbol()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sAltMainSymbol());
}

void CLexemeManaged::SetAltMainSymbol(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetAltMainSymbol(sFromManagedString(sValue));
}

EM_Aspect CLexemeManaged::eAspect()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Aspect)spLexeme->eAspect();
}

void CLexemeManaged::SetAspect(EM_Aspect eValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->SetAspect((ET_Aspect)eValue);
}

String^ CLexemeManaged::sInflectionType()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sInflectionType());
}

void CLexemeManaged::SetInflectionType(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetInflectionType(sFromManagedString(sValue));
}

EM_PartOfSpeech CLexemeManaged::ePartOfSpeech()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_PartOfSpeech)spLexeme->ePartOfSpeech();
}

void CLexemeManaged::SetPartOfSpeech(EM_PartOfSpeech eValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetPartOfSpeech((ET_PartOfSpeech)eValue);
}

String^ CLexemeManaged::sComment()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sComment());
}

void CLexemeManaged::SetComment(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltMainSymbolComment()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sAltMainSymbolComment());
}

void CLexemeManaged::SetAltMainSymbolComment(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetAltMainSymbolComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltInflectionComment()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sAltInflectionComment());
}

void CLexemeManaged::SetAltInflectionComment(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetAltInflectionComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sVerbStemAlternation()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sVerbStemAlternation());
}

void CLexemeManaged::SetVerbStemAlternation(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetVerbStemAlternation(sFromManagedString(sValue));
}

bool CLexemeManaged::bPartPastPassZhd()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bPartPastPassZhd();
}

void CLexemeManaged::SetPartPastPassZhd(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetPartPastPassZhd(bValue);
}

int CLexemeManaged::iSection()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->iSection();
}

void CLexemeManaged::SetSection(int iValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetSection(iValue);
}

bool CLexemeManaged::bNoComparative()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bNoComparative();
}

void CLexemeManaged::SetNoComparative(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetNoComparative(bValue);
}

bool CLexemeManaged::bSpryazhSm()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bSpryazhSm();
}

String^ CLexemeManaged::sSpryazhSmRefSource()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sSpryazhSmRefSource());
}


EM_Subparadigm CLexemeManaged::eSubparadigm()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Subparadigm)spLexeme->eSubparadigm();
}

void CLexemeManaged::SetSubparadigm(EM_Subparadigm eSp)    // currently only used with numerals
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetSubparadigm((ET_Subparadigm)eSp);
}

bool CLexemeManaged::bAssumedForms()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bAssumedForms();
}

void CLexemeManaged::SetAssumedForms(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetAssumedForms(bValue);
}

bool CLexemeManaged::bSecondGenitive()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bSecondGenitive();
}

void CLexemeManaged::SetSecondGenitive(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetSecondGenitive(bValue);
}

bool CLexemeManaged::bSecondPrepositional()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bSecondPrepositional();
}

void CLexemeManaged::SetSecondPrepositional(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetSecondPrepositional(bValue);
}

bool CLexemeManaged::bSecondPrepositionalOptional()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bSecondPrepositionalOptional();
}

void CLexemeManaged::SetSecondPrepositionalOptional(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetSecondPrepositionalOptional(bValue);
}

String^ CLexemeManaged::sP2Preposition()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sP2Preposition());
}

void CLexemeManaged::SetP2Preposition(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetP2Preposition(sFromManagedString(sValue));
}

bool CLexemeManaged::bNoLongForms()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bNoLongForms();
}

void CLexemeManaged::SetNoLongForms(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetNoLongForms(bValue);
}

String^ CLexemeManaged::s1SgStem()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->s1SgStem());
}

void CLexemeManaged::Set1SgStem(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->Set1SgStem(sFromManagedString(sValue));
}

String^ CLexemeManaged::s3SgStem()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->s3SgStem());
}

void CLexemeManaged::Set3SgStem(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->Set3SgStem(sFromManagedString(sValue));
}

String^ CLexemeManaged::sInfinitive()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sInfinitive());
}

void CLexemeManaged::SetInfinitive(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetInfinitive(sFromManagedString(sValue));
}

String^ CLexemeManaged::sInfStem()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sInfStem());
}

void CLexemeManaged::SetInfStem(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetInfStem(sFromManagedString(sValue));
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
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bIsSecondPart();
}

void CLexemeManaged::SetIsSecondPart(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetIsSecondPart(bValue);
}

bool CLexemeManaged::bFindStandardAlternation(String^ sKey, String^% sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString cestrValue;
    bool bRet = spLexeme->bFindStandardAlternation(sFromManagedString(sKey), cestrValue);
    sValue = gcnew String(sValue);

    return bRet;
}

EM_ReturnCode CLexemeManaged::eGetStemStressPositions(String^ sStem, List<int>% listPositions)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    vector<int> vecPositions;
    ET_ReturnCode eRet = spLexeme->eGetStemStressPositions(sFromManagedString(sStem), vecPositions);
    if (H_NO_ERROR == eRet)
    {
        for (vector<int>::iterator it = vecPositions.begin(); it != vecPositions.end(); ++it)
        {
            listPositions.Add(*it);
        }
    }

    return (EM_ReturnCode)eRet;
}

bool CLexemeManaged::bHasDifficultForms()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bHasDifficultForms();
}

EM_ReturnCode CLexemeManaged::eGetSourceFormWithStress(String^% sSourceForm, bool bIsVariant)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString sSource;
    ET_ReturnCode eRet = spLexeme->eGetSourceFormWithStress(sSource, bIsVariant);

    sSourceForm = gcnew String(sSource);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetFirstStemStressPos(int% iPos)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = spLexeme->eGetFirstStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetNextStemStressPos(int% iPos)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = spLexeme->eGetNextStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetFirstSecondaryStemStressPos(int% iPos)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = spLexeme->eGetFirstSecondaryStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetNextSecondaryStemStressPos(int% iPos)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = spLexeme->eGetNextSecondaryStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eCheckLexemeProperties() // for manual input/editing
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_ReturnCode)spLexeme->eCheckLexemeProperties();
}

EM_ReturnCode CLexemeManaged::eGetErrorMsg(String^% sErrorMsg)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString sNativeErrMsg;
    ET_ReturnCode eRet = spLexeme->eGetErrorMsg(sNativeErrMsg);
    if (eRet != H_NO_ERROR)
    {
        return (EM_ReturnCode)eRet;
    }

    sErrorMsg = gcnew String(sNativeErrMsg);

    return (EM_ReturnCode)H_NO_ERROR;
}

bool CLexemeManaged::bHasAspectPair()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bHasAspectPair();
}

void CLexemeManaged::SetHasAspectPair(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->SetHasAspectPair(bValue);
}

bool CLexemeManaged::bHasAltAspectPair()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bHasAltAspectPair();
}

int CLexemeManaged::iAspectPairType()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return spLexeme->iAspectPairType();
}

void CLexemeManaged::SetAspectPairType(int iValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    spLexeme->SetAspectPairType(iValue);
}

int CLexemeManaged::iAltAspectPairType()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return spLexeme->iAltAspectPairType();
}

void CLexemeManaged::SetAltAspectPairType(int iValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    spLexeme->SetAltAspectPairType(iValue);
}

String^ CLexemeManaged::sAltAspectPairComment()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sAltAspectPairComment());
}

void CLexemeManaged::SetAltAspectPairComment(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetAltAspectPairComment(sFromManagedString(sValue));
}

EM_ReturnCode CLexemeManaged::eGetAspectPair(String^% sAspectPair, int% iStressPos)
{
    if (!bHasAspectPair())
    {
        return EM_ReturnCode::H_FALSE;
    }

    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString sAp;
    int iSp = -1;
    ET_ReturnCode eRet = spLexeme->eGetAspectPair(sAp, iSp);
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

    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        return (EM_ReturnCode)EM_ReturnCode::H_ERROR_POINTER;
    }
    CEString sAp;
    int iSp = -1;
    ET_ReturnCode eRet = spLexeme->eGetAltAspectPair(sAp, iSp);
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
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sAspectPairData());
}

void CLexemeManaged::SetAspectPairData(String^ sAspectPairData)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetAspectPairData(sFromManagedString(sAspectPairData));
}

bool CLexemeManaged::bHasIrregularVariants()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return spLexeme->bHasIrregularVariants();
}

void CLexemeManaged::SetHasIrregularVariants(bool bValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetHasIrregularVariants(bValue);
}

String^ CLexemeManaged::sRestrictedContexts()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sRestrictedContexts());
}

void CLexemeManaged::SetRestrictedContexts(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetRestrictedContexts(sFromManagedString(sValue));
}

String^ CLexemeManaged::sContexts()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sContexts());
}

void CLexemeManaged::SetContexts(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetContexts(sFromManagedString(sValue));
}

String^ CLexemeManaged::sTrailingComment()
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(spLexeme->sTrailingComment());
}

void CLexemeManaged::SetTrailingComment(String^ sValue)
{
    auto spLexeme = spGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetTrailingComment(sFromManagedString(sValue));
}
