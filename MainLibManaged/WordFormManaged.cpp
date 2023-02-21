//#include "Analytics.h"
//#include "Verifier.h"
//#include "AspectPair.h"
//#include "..\Hlib\SqliteWrapper.h"
//#include "EnumsManaged.h"

#include "UtilsManaged.h"
#include "LexemeManaged.h"
#include "WordFormManaged.h"

using namespace System;
//using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace MainLibManaged;
using namespace std;

CWordFormManaged::CWordFormManaged(shared_ptr<CWordForm> spWf) : m_pWordForm(&spWf)
{}

CWordFormManaged::~CWordFormManaged()
{}

CLexemeManaged^ CWordFormManaged::Lexeme()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew CLexemeManaged((*m_pWordForm)->spLexeme());
}

shared_ptr<CWordForm> CWordFormManaged::spWordForm()
{
    return *m_pWordForm;
}

String^ CWordFormManaged::sWordForm()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String((*m_pWordForm)->sWordForm());
}

void CWordFormManaged::SetWordForm(String^ sWordForm)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    (*m_pWordForm)->SetWordForm(sFromManagedString(sWordForm));
}

long long CWordFormManaged::llWordFormDbId()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    return (*m_pWordForm)->llDbId();
}

String^ CWordFormManaged::sStem()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String((*m_pWordForm)->sStem());
}

void CWordFormManaged::SetStem(String^ sStem)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    (*m_pWordForm)->SetStem(sFromManagedString(sStem));
}

__int64 CWordFormManaged::llLexemeId()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (*m_pWordForm)->spLexeme()->llLexemeId();
}

EM_PartOfSpeech CWordFormManaged::ePos()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_PartOfSpeech)(*m_pWordForm)->ePos();
}

void CWordFormManaged::SetPos(EM_PartOfSpeech ePos)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    (*m_pWordForm)->SetPos((ET_PartOfSpeech)ePos);
}

EM_Case CWordFormManaged::eCase()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Case)(*m_pWordForm)->eCase();
}

void CWordFormManaged::SetCase(EM_Case eCase)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetCase((ET_Case)eCase);
}

EM_Number CWordFormManaged::eNumber()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Number)(*m_pWordForm)->eNumber();
}

void CWordFormManaged::SetNumber(EM_Number eNumber)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetNumber((ET_Number)eNumber);
}

EM_Subparadigm CWordFormManaged::eSubparadigm()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Subparadigm)(*m_pWordForm)->eSubparadigm();
}

void CWordFormManaged::SetSubparadigm(EM_Subparadigm eSubparadigm)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetSubparadigm((ET_Subparadigm)eSubparadigm);
}

EM_Gender CWordFormManaged::eGender()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Gender)(*m_pWordForm)->eGender();
}

void CWordFormManaged::SetGender(EM_Gender eGender)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetGender((ET_Gender)eGender);
}

EM_Person CWordFormManaged::ePerson()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Person)(*m_pWordForm)->ePerson();
}

void CWordFormManaged::SetPerson(EM_Person ePerson)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetPerson((ET_Person)ePerson);
}

EM_Animacy CWordFormManaged::eAnimacy()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Animacy)(*m_pWordForm)->eAnimacy();
}

void CWordFormManaged::SetAnimacy(EM_Animacy eAnimacy)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetAnimacy((ET_Animacy)eAnimacy);
}

EM_Reflexive CWordFormManaged::eReflexive()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Reflexive)(*m_pWordForm)->eReflexive();
}

void CWordFormManaged::SetReflexivity(EM_Reflexive eReflexive)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetReflexivity((ET_Reflexivity)eReflexive);
}

EM_Aspect CWordFormManaged::eAspect()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Aspect)(*m_pWordForm)->eAspect();
}

void CWordFormManaged::SetAspect(EM_Aspect eAspect)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetAspect((ET_Aspect)eAspect);
}

EM_Status CWordFormManaged::eStatus()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Status)(*m_pWordForm)->eStatus();
}

void CWordFormManaged::SetStatus(EM_Status eStatus)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetStatus((ET_Status)eStatus);
}

bool CWordFormManaged::bIrregular()      // came from the DB as opposed to being generated by the app
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (*m_pWordForm)->bIrregular();
}

void CWordFormManaged::SetIrregular(bool bIrregular)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetIrregular(bIrregular);
}

String^ CWordFormManaged::sLeadComment()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String((*m_pWordForm)->sLeadComment());
}

void CWordFormManaged::SetLeadComment(String^ sLeadComment)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetLeadComment(sFromManagedString(sLeadComment));
}

String^ CWordFormManaged::sTrailingComment()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String((*m_pWordForm)->sTrailingComment());
}

void CWordFormManaged::SetTrailingComment(String^ sTrailingComment)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    (*m_pWordForm)->SetTrailingComment(sFromManagedString(sTrailingComment));
}

bool CWordFormManaged::bIsEdited()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (*m_pWordForm)->bIsEdited();
}

void CWordFormManaged::SetIsEdited(bool bIsEdited)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (*m_pWordForm)->SetIsEdited(bIsEdited);
}

bool CWordFormManaged::bIsVariant()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (*m_pWordForm)->bIsVariant();
}

void CWordFormManaged::SetIsVariant(bool bIsVariant)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (*m_pWordForm)->SetIsVariant(bIsVariant);
}

EM_ReturnCode CWordFormManaged::eGetFirstStressPos(int% iPos, EM_StressType% eType)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    int cppiPos = -1;
    ET_StressType cppeType = ET_StressType::STRESS_TYPE_UNDEFINED;
    ET_ReturnCode eRet = (*m_pWordForm)->eGetFirstStressPos(cppiPos, cppeType);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
        eType = (EM_StressType)cppeType;
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CWordFormManaged::eGetNextStressPos(int% iPos, EM_StressType% eType)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    int cppiPos = -1;
    ET_StressType cppeType = ET_StressType::STRESS_TYPE_UNDEFINED;
    ET_ReturnCode eRet = (*m_pWordForm)->eGetNextStressPos(cppiPos, cppeType);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
        eType = (EM_StressType)cppeType;
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CWordFormManaged::eSetStressPositions(Collections::Generic::Dictionary<int, EM_StressType>^ dctStressPositions)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    Collections::Generic::Dictionary<int, EM_StressType>::Enumerator^ enumerator = dctStressPositions->GetEnumerator();
    map<int, ET_StressType> mapStressPositions;
    while (enumerator->MoveNext())
    {
        int iPos = enumerator->Current.Key;
        EM_StressType eType = enumerator->Current.Value;
        mapStressPositions[iPos] = (ET_StressType)eType;
    }

    ET_ReturnCode eRet = (*m_pWordForm)->eSetStressPositions(mapStressPositions);

    return (EM_ReturnCode)eRet;
}

String^ CWordFormManaged::sGramHash()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String((*m_pWordForm)->sGramHash());
}

EM_ReturnCode CWordFormManaged::eInitFromHash(String^ sHash)
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_ReturnCode)(*m_pWordForm)->eInitFromHash(sFromManagedString(sHash));
}

EM_ReturnCode CWordFormManaged::eSaveIrregularForm()
{
    if (nullptr == m_pWordForm || nullptr == *m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_ReturnCode)(*m_pWordForm)->eSaveIrregularForm();
}

//EM_ReturnCode CWordFormManaged::eSetIrregularStressPositions(Dictionary<int, EM_StressType>^ dictPositions)
//{
//    if (nullptr == m_pWordForm)
//    {
//        throw gcnew Exception(L"WordForm object is NULL.");
//    }

//    map<int, ET_StressType> mapPositions;
//    for each (KeyValuePair<int, EM_StressType> pairPosition in dictPositions)
//    {
//        mapPositions[pairPosition.Key] = (ET_StressType)pairPosition.Value;
//    }

//    auto eRet = m_pWordForm->eSaveIrregularStress(mapPositions);

//    return (EM_ReturnCode)eRet;
//}
