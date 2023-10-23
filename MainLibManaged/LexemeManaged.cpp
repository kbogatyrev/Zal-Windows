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
            auto iHandle = Singleton::pGetInstance()->iAddInflection(spInflection.get());
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
            auto iHandle = Singleton::pGetInstance()->iAddInflection(spInflection.get());
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

CLexeme* CLexemeManaged::pGetInstance()
{
    CLexeme* pLexeme;
    auto rc = Singleton::pGetInstance()->eGetLexeme(m_iHandle, pLexeme);
    if (rc != H_NO_ERROR || nullptr == pLexeme)
    {
        throw gcnew Exception(L"Unable to retrieve lexeme instance.");
    }
    return pLexeme;
}

EM_ReturnCode CLexemeManaged::eCreateInflectionEnumerator(CInflectionEnumeratorManaged^% pLeManaged)
{
    shared_ptr<CInflectionEnumerator> spInflectionEnumerator;
    ET_ReturnCode rc = pGetInstance()->eCreateInflectionEnumerator(spInflectionEnumerator);
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
    auto spLexeme = pGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return spLexeme->llLexemeId();
}

__int64 CLexemeManaged::llHeadwordId()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->llHeadwordId();
}

EM_Gender CLexemeManaged::eGender()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return (EM_Gender)pLexeme->eInflectionTypeToGender();
}

String^ CLexemeManaged::sGraphicStem()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(pLexeme->sGraphicStem());
}

void CLexemeManaged::SetGraphicStem(String^ sGraphicStem)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetGraphicStem(sFromManagedString(sGraphicStem));
}

bool CLexemeManaged::bHasIrregularForms()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bHasIrregularForms();
}

void CLexemeManaged::SetHasIrregularForms(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetHasIrregularForms(bValue);
}

bool CLexemeManaged::bHasSecondaryStress()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bHasSecondaryStress();
}

//////////////////////////////////////////////////////////////////////////////////

bool CLexemeManaged::bHasYoAlternation()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bHasYoAlternation();
}

void CLexemeManaged::SetHasYoAlternation(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetHasYoAlternation(bValue);
}

bool CLexemeManaged::bHasOAlternation()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bHasOAlternation();
}

void CLexemeManaged::SetHasOAlternation(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetHasOAlternation(bValue);
}

String^ CLexemeManaged::sSourceForm()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(pLexeme->sSourceForm());
}

void CLexemeManaged::SetSourceForm(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetSourceForm(sFromManagedString(sValue));
}

bool CLexemeManaged::bHasHomonyms()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->stGetProperties().vecHomonyms.size() > 0;
}

List<int>^ CLexemeManaged::arrHomonyms()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    List<int>^ lHomonyms = gcnew List<int>();
    for (int iHomonymNumber : pLexeme->stGetProperties().vecHomonyms)
    {
        lHomonyms->Add(iHomonymNumber);
    }
    return lHomonyms;
}

void CLexemeManaged::SetHomonyms(List<int>^ arrHomonyms)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    StLexemeProperties& stProperties = pLexeme->stGetPropertiesForWriteAccess();
    for each (int iHomonymNumber in arrHomonyms)
    {
        stProperties.vecHomonyms.push_back(iHomonymNumber);
    }
}

//    CSqlite * CLexemeManaged::pGetDb()
 //{}

String^ CLexemeManaged::sHeadwordComment()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sHeadwordComment());
}

void CLexemeManaged::SetHeadwordComment(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetHeadwordComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sHeadwordVariant()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sHeadwordVariant());
}

void CLexemeManaged::SetHeadwordVariant(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetHeadwordVariant(sFromManagedString(sValue));
}

String^ CLexemeManaged::sHeadwordVariantComment()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sHeadwordVariantComment());
}

void CLexemeManaged::SetHeadwordVariantComment(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetHeadwordVariantComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sPluralOf()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sPluralOf());
}

void CLexemeManaged::SetPluralOf(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetPluralOf(sFromManagedString(sValue));
}

String^ CLexemeManaged::sUsage()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sUsage());
}

void CLexemeManaged::SetUsage(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetUsage(sFromManagedString(sValue));
}

String^ CLexemeManaged::sSeeRef()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sSeeRef());
}

void CLexemeManaged::SetSeeRef(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetSeeRef(sFromManagedString(sValue));
}

String^ CLexemeManaged::sBackRef()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sBackRef());
}

void CLexemeManaged::SetBackRef(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetBackRef(sFromManagedString(sValue));
}

bool CLexemeManaged::bIsUnstressed()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return pLexeme->bIsUnstressed();
}

void CLexemeManaged::SetIsUnstressed(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return pLexeme->SetIsUnstressed(bValue);
}

bool CLexemeManaged::bIsVariant()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return pLexeme->bIsVariant();
}

void CLexemeManaged::SetIsVariant(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetIsVariant(bValue);
}

String^ CLexemeManaged::sMainSymbol()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sMainSymbol());
}

void CLexemeManaged::SetMainSymbol(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetMainSymbol(sFromManagedString(sValue));
}

bool CLexemeManaged::bIsPluralOf()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return pLexeme->bIsPluralOf();
}

void CLexemeManaged::SetIsPluralOf(bool bValue)
{
    auto spLexeme = pGetInstance();
    if (nullptr == spLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    spLexeme->SetIsPluralOf(bValue);
}

bool CLexemeManaged::bTransitive()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return pLexeme->bTransitive();
}

void CLexemeManaged::SetTransitive(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return pLexeme->SetTransitive(bValue);
}

EM_Reflexive CLexemeManaged::eIsReflexive()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Reflexive)pLexeme->eIsReflexive();
}

void CLexemeManaged::SetIsReflexive(EM_Reflexive eValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetIsReflexive((ET_Reflexivity)eValue);
}

String^ CLexemeManaged::sMainSymbolPluralOf()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sMainSymbolPluralOf());
}

void CLexemeManaged::SetMainSymbolPluralOf(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetMainSymbolPluralOf(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltMainSymbol()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sAltMainSymbol());
}

void CLexemeManaged::SetAltMainSymbol(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetAltMainSymbol(sFromManagedString(sValue));
}

EM_Aspect CLexemeManaged::eAspect()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Aspect)pLexeme->eAspect();
}

void CLexemeManaged::SetAspect(EM_Aspect eValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return pLexeme->SetAspect((ET_Aspect)eValue);
}

String^ CLexemeManaged::sInflectionType()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sInflectionType());
}

void CLexemeManaged::SetInflectionType(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetInflectionType(sFromManagedString(sValue));
}

EM_PartOfSpeech CLexemeManaged::ePartOfSpeech()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_PartOfSpeech)pLexeme->ePartOfSpeech();
}

void CLexemeManaged::SetPartOfSpeech(EM_PartOfSpeech eValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetPartOfSpeech((ET_PartOfSpeech)eValue);
}

String^ CLexemeManaged::sComment()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sComment());
}

void CLexemeManaged::SetComment(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltMainSymbolComment()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sAltMainSymbolComment());
}

void CLexemeManaged::SetAltMainSymbolComment(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetAltMainSymbolComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltInflectionComment()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sAltInflectionComment());
}

void CLexemeManaged::SetAltInflectionComment(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetAltInflectionComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sVerbStemAlternation()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sVerbStemAlternation());
}

void CLexemeManaged::SetVerbStemAlternation(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetVerbStemAlternation(sFromManagedString(sValue));
}

bool CLexemeManaged::bPartPastPassZhd()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bPartPastPassZhd();
}

void CLexemeManaged::SetPartPastPassZhd(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetPartPastPassZhd(bValue);
}

int CLexemeManaged::iSection()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->iSection();
}

void CLexemeManaged::SetSection(int iValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetSection(iValue);
}

bool CLexemeManaged::bNoComparative()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bNoComparative();
}

void CLexemeManaged::SetNoComparative(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetNoComparative(bValue);
}

bool CLexemeManaged::bSpryazhSm()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bSpryazhSm();
}

String^ CLexemeManaged::sSpryazhSmRefSource()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(pLexeme->sSpryazhSmRefSource());
}


EM_Subparadigm CLexemeManaged::eSubparadigm()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return (EM_Subparadigm)pLexeme->eSubparadigm();
}

void CLexemeManaged::SetSubparadigm(EM_Subparadigm eSp)    // currently only used with numerals
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetSubparadigm((ET_Subparadigm)eSp);
}

bool CLexemeManaged::bAssumedForms()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bAssumedForms();
}

void CLexemeManaged::SetAssumedForms(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetAssumedForms(bValue);
}

bool CLexemeManaged::bSecondGenitive()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bSecondGenitive();
}

void CLexemeManaged::SetSecondGenitive(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetSecondGenitive(bValue);
}

bool CLexemeManaged::bSecondPrepositional()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bSecondPrepositional();
}

void CLexemeManaged::SetSecondPrepositional(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetSecondPrepositional(bValue);
}

bool CLexemeManaged::bSecondPrepositionalOptional()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bSecondPrepositionalOptional();
}

void CLexemeManaged::SetSecondPrepositionalOptional(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetSecondPrepositionalOptional(bValue);
}

String^ CLexemeManaged::sP2Preposition()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(pLexeme->sP2Preposition());
}

void CLexemeManaged::SetP2Preposition(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetP2Preposition(sFromManagedString(sValue));
}

bool CLexemeManaged::bNoLongForms()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bNoLongForms();
}

void CLexemeManaged::SetNoLongForms(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetNoLongForms(bValue);
}

String^ CLexemeManaged::s1SgStem()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(pLexeme->s1SgStem());
}

void CLexemeManaged::Set1SgStem(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->Set1SgStem(sFromManagedString(sValue));
}

String^ CLexemeManaged::s3SgStem()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(pLexeme->s3SgStem());
}

void CLexemeManaged::Set3SgStem(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->Set3SgStem(sFromManagedString(sValue));
}

String^ CLexemeManaged::sInfinitive()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(pLexeme->sInfinitive());
}

void CLexemeManaged::SetInfinitive(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetInfinitive(sFromManagedString(sValue));
}

String^ CLexemeManaged::sInfStem()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(pLexeme->sInfStem());
}

void CLexemeManaged::SetInfStem(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetInfStem(sFromManagedString(sValue));
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
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bIsSecondPart();
}

void CLexemeManaged::SetIsSecondPart(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetIsSecondPart(bValue);
}

bool CLexemeManaged::bFindStandardAlternation(String^ sKey, String^% sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString cestrValue;
    bool bRet = pLexeme->bFindStandardAlternation(sFromManagedString(sKey), cestrValue);
    sValue = gcnew String(sValue);

    return bRet;
}

EM_ReturnCode CLexemeManaged::eGetStemStressPositions(String^ sStem, List<int>% listPositions)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    vector<int> vecPositions;
    ET_ReturnCode eRet = pLexeme->eGetStemStressPositions(sFromManagedString(sStem), vecPositions);
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
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bHasDifficultForms();
}

EM_ReturnCode CLexemeManaged::eGetSourceFormWithStress(String^% sSourceForm, bool bIsVariant)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString sSource;
    ET_ReturnCode eRet = pLexeme->eGetSourceFormWithStress(sSource, bIsVariant);

    sSourceForm = gcnew String(sSource);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetFirstStemStressPos(int% iPos)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = pLexeme->eGetFirstStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetNextStemStressPos(int% iPos)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = pLexeme->eGetNextStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetFirstSecondaryStemStressPos(int% iPos)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = pLexeme->eGetFirstSecondaryStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetNextSecondaryStemStressPos(int% iPos)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = pLexeme->eGetNextSecondaryStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eCheckLexemeProperties() // for manual input/editing
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return (EM_ReturnCode)pLexeme->eCheckLexemeProperties();
}

EM_ReturnCode CLexemeManaged::eGetErrorMsg(String^% sErrorMsg)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString sNativeErrMsg;
    ET_ReturnCode eRet = pLexeme->eGetErrorMsg(sNativeErrMsg);
    if (eRet != H_NO_ERROR)
    {
        return (EM_ReturnCode)eRet;
    }

    sErrorMsg = gcnew String(sNativeErrMsg);

    return (EM_ReturnCode)H_NO_ERROR;
}

bool CLexemeManaged::bHasAspectPair()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bHasAspectPair();
}

void CLexemeManaged::SetHasAspectPair(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->SetHasAspectPair(bValue);
}

bool CLexemeManaged::bHasAltAspectPair()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bHasAltAspectPair();
}

int CLexemeManaged::iAspectPairType()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->iAspectPairType();
}

void CLexemeManaged::SetAspectPairType(int iValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetAspectPairType(iValue);
}

int CLexemeManaged::iAltAspectPairType()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->iAltAspectPairType();
}

void CLexemeManaged::SetAltAspectPairType(int iValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetAltAspectPairType(iValue);
}

String^ CLexemeManaged::sAltAspectPairComment()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sAltAspectPairComment());
}

void CLexemeManaged::SetAltAspectPairComment(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetAltAspectPairComment(sFromManagedString(sValue));
}

EM_ReturnCode CLexemeManaged::eGetAspectPair(String^% sAspectPair, int% iStressPos)
{
    if (!bHasAspectPair())
    {
        return EM_ReturnCode::H_FALSE;
    }

    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString sAp;
    int iSp = -1;
    ET_ReturnCode eRet = pLexeme->eGetAspectPair(sAp, iSp);
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

    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        return (EM_ReturnCode)EM_ReturnCode::H_ERROR_POINTER;
    }
    CEString sAp;
    int iSp = -1;
    ET_ReturnCode eRet = pLexeme->eGetAltAspectPair(sAp, iSp);
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
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sAspectPairData());
}

void CLexemeManaged::SetAspectPairData(String^ sAspectPairData)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    pLexeme->SetAspectPairData(sFromManagedString(sAspectPairData));
}

bool CLexemeManaged::bHasIrregularVariants()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return pLexeme->bHasIrregularVariants();
}

void CLexemeManaged::SetHasIrregularVariants(bool bValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetHasIrregularVariants(bValue);
}

String^ CLexemeManaged::sRestrictedContexts()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(pLexeme->sRestrictedContexts());
}

void CLexemeManaged::SetRestrictedContexts(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetRestrictedContexts(sFromManagedString(sValue));
}

String^ CLexemeManaged::sContexts()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(pLexeme->sContexts());
}

void CLexemeManaged::SetContexts(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetContexts(sFromManagedString(sValue));
}

String^ CLexemeManaged::sTrailingComment()
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return gcnew String(pLexeme->sTrailingComment());
}

void CLexemeManaged::SetTrailingComment(String^ sValue)
{
    auto pLexeme = pGetInstance();
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    pLexeme->SetTrailingComment(sFromManagedString(sValue));
}
