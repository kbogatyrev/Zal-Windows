// This is the main DLL file.

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#define _SILENCE_CXX17_STRSTREAM_DEPRECATION_WARNING

#define NOMINMAX

#include <memory>
#include <msclr\marshal.h>
#include "GramHasher.h"
#include "MainLibManaged.h"

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace MainLibManaged;
using namespace std;

CEString sFromManagedString(String^ sSource)
{
    // This conversion requires an instance of marshal_context class
    //marshal_context^ mc = gcnew marshal_context();
    //CEString cestrResult(mc->marshal_as<const wchar_t *>(sSource));


    // Pin memory so GC can't move it while native function is called
    pin_ptr<const wchar_t> pSource = PtrToStringChars(sSource);
    CEString cestrResult(pSource);
    return cestrResult;
}

const CEString sFromManagedString(const String^ sSource)
{
    // This conversion requires an instance of marshal_context class
    //marshal_context^ mc = gcnew marshal_context();
    //CEString cestrResult(mc->marshal_as<const wchar_t *>(sSource));


    // Pin memory so GC can't move it while native function is called
    pin_ptr<const wchar_t> pSource = PtrToStringChars(sSource);
    CEString cestrResult(pSource);
    return cestrResult;
}

/*
extern "C"
{
    ET_ReturnCode GetDictionary(shared_ptr<CDictionary> *&);        // the only external function defined in MainLib
}
*/
CWordFormManaged::CWordFormManaged(CWordForm * pWf) : m_pWordForm(pWf)
{}

CWordFormManaged::~CWordFormManaged()
{}

CLexemeManaged^ CWordFormManaged::Lexeme()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew CLexemeManaged(m_pWordForm->pGetLexemePtr());
}

CWordForm * CWordFormManaged::pGetUnmanagedItf()
{
    return m_pWordForm;
}

String^ CWordFormManaged::sWordForm()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String (m_pWordForm->sWordForm());
}

void CWordFormManaged::SetWordForm(String^ sWordForm)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    m_pWordForm->SetWordForm(sFromManagedString(sWordForm));
}

long long CWordFormManaged::llWordFormDbId()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    return m_pWordForm->llDbId();
}

String^ CWordFormManaged::sStem()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String(m_pWordForm->sStem());
}

void CWordFormManaged::SetStem(String^ sStem)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    m_pWordForm->SetStem(sFromManagedString(sStem));
}

__int64 CWordFormManaged::llLexemeId()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->pGetLexemePtr()->llLexemeId();
}

EM_PartOfSpeech CWordFormManaged::ePos()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_PartOfSpeech)m_pWordForm->ePos();
}

void CWordFormManaged::SetPos(EM_PartOfSpeech ePos)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    m_pWordForm->SetPos((ET_PartOfSpeech)ePos);
}

EM_Case CWordFormManaged::eCase()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Case)m_pWordForm->eCase();
}

void CWordFormManaged::SetCase(EM_Case eCase)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetCase((ET_Case)eCase);
}

EM_Number CWordFormManaged::eNumber()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Number)m_pWordForm->eNumber();
}

void CWordFormManaged::SetNumber(EM_Number eNumber)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetNumber((ET_Number)eNumber);
}

EM_Subparadigm CWordFormManaged::eSubparadigm()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Subparadigm)m_pWordForm->eSubparadigm();
}

void CWordFormManaged::SetSubparadigm(EM_Subparadigm eSubparadigm)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetSubparadigm((ET_Subparadigm)eSubparadigm);
}

EM_Gender CWordFormManaged::eGender()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Gender)m_pWordForm->eGender();
}

void CWordFormManaged::SetGender(EM_Gender eGender)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetGender((ET_Gender)eGender);
}

EM_Person CWordFormManaged::ePerson()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Person)m_pWordForm->ePerson();
}

void CWordFormManaged::SetPerson(EM_Person ePerson)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetPerson((ET_Person)ePerson);
}

EM_Animacy CWordFormManaged::eAnimacy()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Animacy)m_pWordForm->eAnimacy();
}

void CWordFormManaged::SetAnimacy(EM_Animacy eAnimacy)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetAnimacy((ET_Animacy)eAnimacy);
}

EM_Reflexive CWordFormManaged::eReflexive()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Reflexive)m_pWordForm->eReflexive();
}

void CWordFormManaged::SetReflexivity(EM_Reflexive eReflexive)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetReflexivity((ET_Reflexivity)eReflexive);
}

EM_Aspect CWordFormManaged::eAspect()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Aspect)m_pWordForm->eAspect();
}

void CWordFormManaged::SetAspect(EM_Aspect eAspect)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetAspect((ET_Aspect)eAspect);
}

EM_Status CWordFormManaged::eStatus()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Status)m_pWordForm->eStatus();
}

void CWordFormManaged::SetStatus(EM_Status eStatus)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetStatus((ET_Status)eStatus);
}

bool CWordFormManaged::bIrregular()      // came from the DB as opposed to being generated by the app
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->bIrregular();
}

void CWordFormManaged::SetIrregular(bool bIrregular)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetIrregular(bIrregular);
}

String^ CWordFormManaged::sLeadComment()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String(m_pWordForm->sLeadComment());
}

void CWordFormManaged::SetLeadComment(String^ sLeadComment)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetLeadComment(sFromManagedString(sLeadComment));
}

String^ CWordFormManaged::sTrailingComment()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String(m_pWordForm->sTrailingComment());
}

void CWordFormManaged::SetTrailingComment(String^ sTrailingComment)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetTrailingComment(sFromManagedString(sTrailingComment));
}

bool CWordFormManaged::bIsEdited()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->bIsEdited();
}

void CWordFormManaged::SetIsEdited(bool bIsEdited)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->SetIsEdited(bIsEdited);
}

bool CWordFormManaged::bIsVariant()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->bIsVariant();
}

void CWordFormManaged::SetIsVariant(bool bIsVariant)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->SetIsVariant(bIsVariant);
}

EM_ReturnCode CWordFormManaged::eGetFirstStressPos(int% iPos, EM_StressType% eType)
{
    if (nullptr == m_pWordForm)
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
    if (nullptr == m_pWordForm)
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

EM_ReturnCode CWordFormManaged::eSetStressPositions(Collections::Generic::Dictionary<int, EM_StressType>^ dctStressPositions)
{
    if (nullptr == m_pWordForm)
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

    ET_ReturnCode eRet = m_pWordForm->eSetStressPositions(mapStressPositions);

    return (EM_ReturnCode)eRet;
}


String^ CWordFormManaged::sGramHash()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String(m_pWordForm->sGramHash());
}

EM_ReturnCode CWordFormManaged::eInitFromHash(String^ sHash)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_ReturnCode)m_pWordForm->eInitFromHash(sFromManagedString(sHash));
}

EM_ReturnCode CWordFormManaged::eSaveIrregularForm()
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_ReturnCode)m_pWordForm->eSaveIrregularForm();
}


EM_ReturnCode CWordFormManaged::eSetIrregularStressPositions(Dictionary<int, EM_StressType>^ dictPositions)
{
    if (nullptr == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    map<int, ET_StressType> mapPositions;
    for each (KeyValuePair<int, EM_StressType> pairPosition in dictPositions)
    {
        mapPositions[pairPosition.Key] = (ET_StressType)pairPosition.Value;
    }

    auto eRet = m_pWordForm->eSaveIrregularStress(mapPositions);

    return (EM_ReturnCode)eRet;
}

/*
CDictionaryManaged::CDictionaryManaged()
{
    Hlib::CDictionary * pD = nullptr;
    ET_ReturnCode rc = GetDictionary(pD);
    m_pDictionary = pD;
}

CDictionaryManaged::~CDictionaryManaged()
{
//    It has a smart pointer wrapper now
//    delete m_pDictionary;
}

EM_ReturnCode CDictionaryManaged::eSetDbPath(String^ sDbPath)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)(*m_pDictionary)->eSetDbPath(sFromManagedString(sDbPath));
}

String^ CDictionaryManaged::sGetDbPath()
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return gcnew String((*m_pDictionary)->sGetDbPath());
}

CLexemeManaged^ CDictionaryManaged::CreateLexemeForEdit()
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary itf handle is NULL.");
    }

    shared_ptr<CLexeme> spLexeme;
    shared_ptr<CInflection> spInflection;
    ET_ReturnCode eRet = (*m_pDictionary)->eCreateLexemeForEdit(spLexeme, spInflection);
    if (eRet != H_NO_ERROR || nullptr == spLexeme)
    {
        throw gcnew Exception(L"Unable to create lexeme.");
    }

    CLexemeManaged^ pManaged = gcnew CLexemeManaged();
//    pManaged->m_pLexeme = pLexeme;

    return pManaged;
}

EM_ReturnCode CDictionaryManaged::eCopyEntryForEdit(CLexemeManaged^ sourceL, CInflectionManaged^ sourceI,
                                                    CLexemeManaged^ targetL, CInflectionManaged^ targetI)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary itf handle is NULL.");
    }

    CLexeme * pCopyL = nullptr;
    CInflection * pCopyI = nullptr;
    ET_ReturnCode eRet = m_pDictionary->eCopyEntryForEdit(sourceL->sp, sourceI->pGetInflectionPtr(), pCopyL, pCopyI);
    if (eRet != H_NO_ERROR || nullptr == pCopyL || nullptr == pCopyI)
    {
        throw gcnew Exception(L"Unable to copy lexeme.");
    }

    CLexemeManaged^ pManagedL = gcnew CLexemeManaged();
    pManagedL->m_pLexeme = pCopyL;

    CInflectionManaged^ pManagedI = gcnew CInflectionManaged();
    pManagedI->m_pInflection = pCopyI;

    pManagedI->m_sStoredEntryHash = gcnew String(pCopyI->sHash());

    return EM_ReturnCode::H_NO_ERROR;
}

EM_ReturnCode CDictionaryManaged::eGetLexemeById(long long llId, CLexemeManaged^% lexeme)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    shared_ptr<CLexeme> pLexeme = nullptr;
    ET_ReturnCode eRet = (*m_pDictionary)->eGetLexemeById(llId, pLexeme);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (pLexeme)
        {
            lexeme = gcnew CLexemeManaged(pLexeme.get());
        }
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eGetLexemesByHash(String^ sHash)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)(*m_pDictionary)->eGetLexemesByHash(sFromManagedString(sHash));
}

EM_ReturnCode CDictionaryManaged::eGetLexemesByGraphicStem(String^ sGrStem)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)m_pDictionary->eGetLexemesByGraphicStem(sFromManagedString(sGrStem));
}

EM_ReturnCode CDictionaryManaged::eGetLexemesByInitialForm(String^ sInitForm)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)(*m_pDictionary)->eGetLexemesByInitialForm(sFromManagedString(sInitForm));
}

EM_ReturnCode CDictionaryManaged::eGenerateAllForms()
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)(*m_pDictionary)->eGenerateAllForms();
}

EM_ReturnCode CDictionaryManaged::eGenerateFormsForSelectedLexemes()
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)m_pDictionary->eGenerateFormsForSelectedLexemes();
}

EM_ReturnCode CDictionaryManaged::eCountLexemes(Int64% iLexemes)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    int64_t cppiLexemes = 0;
    ET_ReturnCode eRet = (*m_pDictionary)->eCountLexemes(cppiLexemes);
    if (H_NO_ERROR == eRet)
    {
        iLexemes = cppiLexemes;
    }

    return (EM_ReturnCode)eRet;
}

//EM_ReturnCode eVerifyLexemeProperties(CLexemeManaged^);
//EM_ReturnCode eSourceFormExists(CLexemeManaged^, bool%);


//EM_ReturnCode CDictionaryManaged::eSaveLexeme(CLexemeManaged^ l)
//{
if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
//    {
//        throw gcnew Exception(L"Dictionary object is NULL.");
//    }
//
//    ET_ReturnCode eRet = m_pDictionary->eSaveLexeme(l->m_pLexeme);
//
//    return (EM_ReturnCode)eRet;
//}

EM_ReturnCode CDictionaryManaged::eUpdateHeadword(CLexemeManaged^ l)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pDictionary)->eUpdateHeadword(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveNewHeadword(CLexemeManaged^ l)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pDictionary)->eSaveNewHeadword(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveHeadwordStress(CLexemeManaged^ l)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pDictionary)->eSaveHeadwordStress(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveHomonyms(CLexemeManaged^ l)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pDictionary)->eSaveHomonyms(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveAspectPairInfo(CLexemeManaged^ l)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pDictionary)->eSaveAspectPairInfo(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveP2Info(CLexemeManaged^ l)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pDictionary)->eSaveP2Info(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eUpdateDescriptorInfo(CLexemeManaged^ l)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pDictionary)->eUpdateDescriptorInfo(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveDescriptorInfo(CLexemeManaged^ l)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pDictionary)->eSaveDescriptorInfo(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}


EM_ReturnCode CDictionaryManaged::eSaveInflectionInfo(CInflectionManaged^ i)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pDictionary)->eSaveInflectionInfo(i->m_pInflection);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveCommonDeviation(CInflectionManaged^ i)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pDictionary)->eSaveCommonDeviation(i->m_pInflection);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eDeleteLexeme(CLexemeManaged^ l)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = (*m_pDictionary)->eDeleteLexeme(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

int CDictionaryManaged::nLexemesFound()
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (*m_pDictionary)->nLexemesFound();
}

void CDictionaryManaged::Clear()
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    (*m_pDictionary)->Clear();
}

EM_ReturnCode CDictionaryManaged::Clear(CLexemeManaged^ pLexeme)
{
    if (nullptr == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = m_pDictionary->Clear(pLexeme->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eCreateLexemeEnumerator(CLexemeEnumeratorManaged^% pLeManaged)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary objectis NULL.");
    }

    ILexemeEnumerator * pLexemeEnumerator = nullptr;
    ET_ReturnCode eRet = m_pDictionary->eCreateLexemeEnumerator(pLexemeEnumerator);
    if (H_NO_ERROR == eRet)
    {
        if (pLexemeEnumerator)
        {
            pLeManaged = gcnew CLexemeEnumeratorManaged(pLexemeEnumerator);
        }
        else
        {
            return EM_ReturnCode::H_ERROR_UNEXPECTED;
        }
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eGetParser(CParserManaged^% pParserManaged)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    shared_ptr<CParser> spParser;
    ET_ReturnCode eRet = m_pDictionary->eGetParser(spParser);
    if (H_NO_ERROR == eRet)
    {
        if (spParser)
        {
            pParserManaged = gcnew CParserManaged(spParser);
        }
        else
        {
            return EM_ReturnCode::H_ERROR_UNEXPECTED;
        }
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eGetAnalytics(CAnalyticsManaged^% pAnalyticsManaged)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    shared_ptr<CAnalytics> spAnalytics;
    ET_ReturnCode eRet = (*m_pDictionary)->eGetAnalytics(pAnalytics);
    if (H_NO_ERROR == eRet)
    {
        if (pAnalytics)
        {
            pAnalyticsManaged = gcnew CAnalyticsManaged(pAnalytics);
        }
        else
        {
            return EM_ReturnCode::H_ERROR_UNEXPECTED;
        }
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eGetVerifier(CVerifierManaged^% verifier)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    shared_ptr<CVerifier> spVerifier;
    ET_ReturnCode eRet = m_pDictionary->eGetVerifier(spVerifier);
    if (H_NO_ERROR == eRet)
    {
        if (spVerifier)
        {
            verifier = gcnew CVerifierManaged(spVerifier);
        }
        else
        {
            return EM_ReturnCode::H_ERROR_UNEXPECTED;
        }
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eExportTestData(String^ sPath, DelegateProgress^ progressCallback)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    GCHandle gch = GCHandle::Alloc(progressCallback);
    IntPtr iptr = Marshal::GetFunctionPointerForDelegate(progressCallback);
    PROGRESS_CALLBACK_CLR pProgress = static_cast<PROGRESS_CALLBACK_CLR>(iptr.ToPointer());

    return (EM_ReturnCode)(*m_pDictionary)->eExportTestData(sFromManagedString(sPath), *pProgress);
}

EM_ReturnCode CDictionaryManaged::eImportTestData(String^ sPath, DelegateProgress^ progressCallback)
{
    if (nullptr == m_pDictionary || nullptr == *m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    GCHandle gch = GCHandle::Alloc(progressCallback);
    IntPtr iptr = Marshal::GetFunctionPointerForDelegate(progressCallback);
    PROGRESS_CALLBACK_CLR pProgress = static_cast<PROGRESS_CALLBACK_CLR>(iptr.ToPointer());

    return (EM_ReturnCode)(*m_pDictionary)->eImportTestData(sFromManagedString(sPath), *pProgress);
}
*/

//
// Managed wrapper for Lexeme class
//
CLexemeManaged::CLexemeManaged()
{}

CLexemeManaged::CLexemeManaged(ILexeme* pLexeme) : m_pLexeme(pLexeme)
{}

CLexemeManaged::~CLexemeManaged()
{
    delete m_pLexeme;
    m_pLexeme = nullptr;
//    m_sStoredEntryHash = "";
}

EM_ReturnCode CLexemeManaged::eCreateInflectionEnumerator(CInflectionEnumeratorManaged^% pIeManaged)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Dictionary objectis NULL.");
    }

    IInflectionEnumerator* pInflectionEnumerator = nullptr;
    ET_ReturnCode eRet = m_pLexeme->eCreateInflectionEnumerator(pInflectionEnumerator);
    if (H_NO_ERROR == eRet)
    {
        if (pInflectionEnumerator)
        {
            pIeManaged = gcnew CInflectionEnumeratorManaged(pInflectionEnumerator);
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

    return m_pLexeme->llLexemeId();
}

__int64 CLexemeManaged::llHeadwordId()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->llHeadwordId();
}

EM_Gender CLexemeManaged::eGender()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Gender)m_pLexeme->eInflectionTypeToGender();
}
 
String^ CLexemeManaged::sGraphicStem()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sGraphicStem());
}

void CLexemeManaged::SetGraphicStem(String^ sGraphicStem)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetGraphicStem(sFromManagedString(sGraphicStem));
}

bool CLexemeManaged::bHasIrregularForms()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasIrregularForms();
}

void CLexemeManaged::SetHasIrregularForms(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHasIrregularForms(bValue);
}

bool CLexemeManaged::bHasSecondaryStress()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasSecondaryStress();
}

bool CInflectionManaged::bHasFleetingVowel()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->bHasFleetingVowel();
}
 
void CInflectionManaged::SetHasFleetingVowel(bool bValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    m_pInflection->SetHasFleetingVowel(bValue);
}

//////////////////////////////////////////////////////////////////////////////////

bool CLexemeManaged::bHasYoAlternation()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasYoAlternation();
}
 
void CLexemeManaged::SetHasYoAlternation(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHasYoAlternation(bValue);
}

bool CLexemeManaged::bHasOAlternation()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasOAlternation();
}
 
void CLexemeManaged::SetHasOAlternation(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHasOAlternation(bValue);
}

String^ CLexemeManaged::sSourceForm()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sSourceForm());
}
 
void CLexemeManaged::SetSourceForm(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSourceForm(sFromManagedString(sValue));
}

bool CLexemeManaged::bHasHomonyms()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->stGetProperties().vecHomonyms.size() > 0;
}

List<int>^ CLexemeManaged::arrHomonyms()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    List<int>^ lHomonyms = gcnew List<int>();
    for (int iHomonymNumber : m_pLexeme->stGetProperties().vecHomonyms)
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

    StLexemeProperties& stProperties = m_pLexeme->stGetPropertiesForWriteAccess();
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

    return gcnew String(m_pLexeme->sHeadwordComment());
}

void CLexemeManaged::SetHeadwordComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHeadwordComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sHeadwordVariant()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sHeadwordVariant());
}

void CLexemeManaged::SetHeadwordVariant(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHeadwordVariant(sFromManagedString(sValue));
}

String^ CLexemeManaged::sHeadwordVariantComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sHeadwordVariantComment());
}

void CLexemeManaged::SetHeadwordVariantComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHeadwordVariantComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sPluralOf()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sPluralOf());
}

void CLexemeManaged::SetPluralOf(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetPluralOf(sFromManagedString(sValue));
}

String^ CLexemeManaged::sUsage()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sUsage());
}

void CLexemeManaged::SetUsage(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetUsage(sFromManagedString(sValue));
}

String^ CLexemeManaged::sSeeRef()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sSeeRef());
}

void CLexemeManaged::SetSeeRef(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSeeRef(sFromManagedString(sValue));
}

String^ CLexemeManaged::sBackRef()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sBackRef());
}

void CLexemeManaged::SetBackRef(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetBackRef(sFromManagedString(sValue));
}

bool CLexemeManaged::bIsUnstressed()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsUnstressed();
}
 
void CLexemeManaged::SetIsUnstressed(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->SetIsUnstressed(bValue);
}

bool CLexemeManaged::bIsVariant()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsVariant();
}
 
void CLexemeManaged::SetIsVariant(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetIsVariant(bValue);
}

String^ CLexemeManaged::sMainSymbol()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sMainSymbol());
}
 
void CLexemeManaged::SetMainSymbol(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetMainSymbol(sFromManagedString(sValue));
}

bool CLexemeManaged::bIsPluralOf()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsPluralOf();
}
 
void CLexemeManaged::SetIsPluralOf(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetIsPluralOf(bValue);
}

bool CLexemeManaged::bTransitive()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bTransitive();
}
 
void CLexemeManaged::SetTransitive(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->SetTransitive(bValue);
}

EM_Reflexive CLexemeManaged::eIsReflexive()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Reflexive)m_pLexeme->eIsReflexive();
}
 
void CLexemeManaged::SetIsReflexive(EM_Reflexive eValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetIsReflexive((ET_Reflexivity)eValue);
}

String^ CLexemeManaged::sMainSymbolPluralOf()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sMainSymbolPluralOf());
}
 
void CLexemeManaged::SetMainSymbolPluralOf(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetMainSymbolPluralOf(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltMainSymbol()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAltMainSymbol());
}
 
void CLexemeManaged::SetAltMainSymbol(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAltMainSymbol(sFromManagedString(sValue));
}

EM_Aspect CLexemeManaged::eAspect()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Aspect)m_pLexeme->eAspect();
}
 
void CLexemeManaged::SetAspect(EM_Aspect eValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->SetAspect((ET_Aspect)eValue);
}

String^ CLexemeManaged::sInflectionType()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sInflectionType());
}
 
void CLexemeManaged::SetInflectionType(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetInflectionType(sFromManagedString(sValue));
}

EM_PartOfSpeech CLexemeManaged::ePartOfSpeech()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_PartOfSpeech)m_pLexeme->ePartOfSpeech();
}
 
void CLexemeManaged::SetPartOfSpeech(EM_PartOfSpeech eValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetPartOfSpeech((ET_PartOfSpeech)eValue);
}

String^ CLexemeManaged::sComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sComment());
}
 
void CLexemeManaged::SetComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltMainSymbolComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAltMainSymbolComment());
}
 
void CLexemeManaged::SetAltMainSymbolComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAltMainSymbolComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltInflectionComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAltInflectionComment());
}
 
void CLexemeManaged::SetAltInflectionComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAltInflectionComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sVerbStemAlternation()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sVerbStemAlternation());
}
 
void CLexemeManaged::SetVerbStemAlternation(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetVerbStemAlternation(sFromManagedString(sValue));
}

bool CLexemeManaged::bPartPastPassZhd()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bPartPastPassZhd();
}
 
void CLexemeManaged::SetPartPastPassZhd(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetPartPastPassZhd(bValue);
}

int CLexemeManaged::iSection()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->iSection();
}
 
void CLexemeManaged::SetSection(int iValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSection(iValue);
}

bool CLexemeManaged::bNoComparative()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bNoComparative();
}
 
void CLexemeManaged::SetNoComparative(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetNoComparative(bValue);
}

bool CLexemeManaged::bAssumedForms()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bAssumedForms();
}
 
void CLexemeManaged::SetAssumedForms(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAssumedForms(bValue);
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

    return m_pLexeme->bSecondGenitive();
}
 
void CLexemeManaged::SetSecondGenitive(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSecondGenitive(bValue);
}
 
bool CLexemeManaged::bSecondPrepositional()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bSecondPrepositional();
}

void CLexemeManaged::SetSecondPrepositional(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSecondPrepositional(bValue);
}

bool CLexemeManaged::bSecondPrepositionalOptional()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bSecondPrepositionalOptional();
}
 
void CLexemeManaged::SetSecondPrepositionalOptional(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSecondPrepositionalOptional(bValue);
}

String^ CLexemeManaged::sP2Preposition()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sP2Preposition());
}
 
void CLexemeManaged::SetP2Preposition(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetP2Preposition(sFromManagedString(sValue));
}

bool CLexemeManaged::bHasAspectPair()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasAspectPair();
}
 
void CLexemeManaged::SetHasAspectPair(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->SetHasAspectPair(bValue);
}

bool CLexemeManaged::bHasAltAspectPair()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasAltAspectPair();
}

int CLexemeManaged::iAspectPairType()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return m_pLexeme->iAspectPairType();
}
 
void CLexemeManaged::SetAspectPairType(int iValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    m_pLexeme->SetAspectPairType(iValue);
}

int CLexemeManaged::iAltAspectPairType()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return m_pLexeme->iAltAspectPairType();
}

void CLexemeManaged::SetAltAspectPairType(int iValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    m_pLexeme->SetAltAspectPairType(iValue);
}

String^ CLexemeManaged::sAltAspectPairComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAltAspectPairComment());
}

void CLexemeManaged::SetAltAspectPairComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAltAspectPairComment(sFromManagedString(sValue));
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
    ET_ReturnCode eRet = m_pLexeme->eGetAspectPair(sAp, iSp);
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
    ET_ReturnCode eRet = m_pLexeme->eGetAltAspectPair(sAp, iSp);
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

    return gcnew String(m_pLexeme->sAspectPairData());
}

void CLexemeManaged::SetAspectPairData(String^ sAspectPairData)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAspectPairData(sFromManagedString(sAspectPairData));
}

bool CLexemeManaged::bHasIrregularVariants()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasIrregularVariants();
}
 
void CLexemeManaged::SetHasIrregularVariants(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHasIrregularVariants(bValue);
}

String^ CLexemeManaged::sRestrictedContexts()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sRestrictedContexts());
}

void CLexemeManaged::SetRestrictedContexts(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetRestrictedContexts(sFromManagedString(sValue));
}
 
String^ CLexemeManaged::sContexts()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sContexts());
}
 
void CLexemeManaged::SetContexts(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetContexts(sFromManagedString(sValue));
}

String^ CLexemeManaged::sTrailingComment()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sTrailingComment());
}
 
void CLexemeManaged::SetTrailingComment(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetTrailingComment(sFromManagedString(sValue));
}

//////////////////////////////////////////////////////////////////////////

CInflectionManaged::CInflectionManaged()
{}

//CInflectionManaged::CInflectionManaged(String^ sGramHash)
//{
//    m_sStoredEntryHash = "";
//}

CInflectionManaged::CInflectionManaged(ILexeme * pLexeme)
{
    if (nullptr == pLexeme)
    {
        throw gcnew Exception(L"Lexeme pointer is NULL.");
    }

    &&&&
}

CInflectionManaged::CInflectionManaged(IInflection * pInflection) : m_pInflection(pInflection)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection pointer is NULL.");
    }

    ILexeme* pLexeme = nullptr;
    auto rc = m_pInflection->eGetLexemeItf(pLexeme);
    if (rc != H_NO_ERROR || nullptr == pLexeme)
    {
        throw gcnew Exception(L"Unable to retrieve lexeme pointer.");
    }
}

CInflectionManaged::~CInflectionManaged()
{}

long long CInflectionManaged::llInflectionId()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->llInflectionId();
}
 
void CInflectionManaged::SetInflectionId(int iValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    m_pInflection->SetInflectionId(iValue);
}

void CInflectionManaged::SetLexeme(CLexemeManaged^ lexeme)
{
    m_pInflection->SetLexeme(lexeme->m_pLexeme);
}

EM_ReturnCode CInflectionManaged::eGetLexeme(CLexemeManaged^% lexeme)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    ILexeme* pLexeme = nullptr;
    auto ret = m_pInflection->eGetLexemeItf(pLexeme);
    if (ret != H_NO_ERROR || nullptr == pLexeme)
    {
        throw gcnew Exception(L"Failed to get ILexeme ITF.");
    }

    lexeme = gcnew CLexemeManaged(pLexeme);
    return EM_ReturnCode::H_NO_ERROR;
}

bool CInflectionManaged::bPrimaryInflectionGroup()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pInflection->bPrimaryInflectionGroup();
}
 
void CInflectionManaged::SetPrimaryInflectionGroup(bool bValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pInflection->SetPrimaryInflectionGroup(bValue);
}

int CInflectionManaged::iType()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->iType();
}
 
void CInflectionManaged::SetType(int iValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->SetType(iValue);
}

EM_AccentType CInflectionManaged::eAccentType1()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (EM_AccentType)m_pInflection->eAccentType1();
}
 
void CInflectionManaged::SetAccentType1(EM_AccentType eValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    m_pInflection->SetAccentType1((ET_AccentType)eValue);
}

EM_AccentType CInflectionManaged::eAccentType2()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (EM_AccentType)m_pInflection->eAccentType2();
}
 
void CInflectionManaged::SetAccentType2(EM_AccentType eValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    m_pInflection->SetAccentType2((ET_AccentType)eValue);
}

bool CInflectionManaged::bShortFormsRestricted()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->bShortFormsRestricted();
}
 
void CInflectionManaged::SetShortFormsRestricted(bool bValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    m_pInflection->SetShortFormsRestricted(bValue);
}

bool CInflectionManaged::bPastParticipleRestricted()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->bPastParticipleRestricted();
}
 
void CInflectionManaged::SetPastParticipleRestricted(bool bValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    m_pInflection->SetPastParticipleRestricted(bValue);
}

bool CLexemeManaged::bNoLongForms()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bNoLongForms();
}
 
void CLexemeManaged::SetNoLongForms(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetNoLongForms(bValue);
}

bool CInflectionManaged::bShortFormsIncomplete()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->bShortFormsIncomplete();
}
 
void CInflectionManaged::SetShortFormsIncomplete(bool bValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    m_pInflection->SetShortFormsIncomplete(bValue);
}

bool CInflectionManaged::bNoPassivePastParticiple()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->bNoPassivePastParticiple();
}
 
void CInflectionManaged::SetNoPassivePastParticiple(bool bValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    m_pInflection->SetNoPassivePastParticiple(bValue);
}

bool CInflectionManaged::bFleetingVowel()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->bFleetingVowel();
}
 
void CInflectionManaged::SetFleetingVowel(bool bValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    m_pInflection->SetFleetingVowel(bValue);
}

int CInflectionManaged::iStemAugment()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->iStemAugment();
}
 
void CInflectionManaged::SetStemAugment(int iValue)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->SetStemAugment(iValue);
}

String^ CLexemeManaged::s1SgStem()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->s1SgStem());
}
 
void CLexemeManaged::Set1SgStem(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->Set1SgStem(sFromManagedString(sValue));
}

String^ CLexemeManaged::s3SgStem()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->s3SgStem());
}

void CLexemeManaged::Set3SgStem(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->Set3SgStem(sFromManagedString(sValue));
}

String^ CLexemeManaged::sInfinitive()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sInfinitive());
}
 
void CLexemeManaged::SetInfinitive(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetInfinitive(sFromManagedString(sValue));
}

String^ CLexemeManaged::sInfStem()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sInfStem());
}

void CLexemeManaged::SetInfStem(String^ sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetInfStem(sFromManagedString(sValue));
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

    return m_pLexeme->bIsSecondPart();
}

void CLexemeManaged::SetIsSecondPart(bool bValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetIsSecondPart(bValue);
}

EM_ReturnCode CInflectionManaged::eAddCommonDeviation(int iValue, bool bIsOptional)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    ET_ReturnCode eRet = m_pInflection->eAddCommonDeviation(iValue, bIsOptional);
    return (EM_ReturnCode)eRet;
}

bool CInflectionManaged::bFindCommonDeviation(int iNum, bool% bIsOptionalDotNet)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    bool bIsOptionalCpp = false;
    bool bRet = m_pInflection->bFindCommonDeviation(iNum, bIsOptionalCpp);
    bIsOptionalDotNet = bIsOptionalCpp;

    return bRet;
}
 
void CInflectionManaged::ClearCommonDeviations()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    m_pInflection->ClearCommonDeviations();
}

bool CLexemeManaged::bFindStandardAlternation(String^ sKey, String^% sValue)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString cestrValue;
    bool bRet = m_pLexeme->bFindStandardAlternation(sFromManagedString(sKey), cestrValue);
    sValue = gcnew String(sValue);

    return bRet;
}

EM_ReturnCode CLexemeManaged::eGetStemStressPositions(String^ sStem, List<int>% listPositions)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    vector<int> vecPositions;
    ET_ReturnCode eRet = m_pLexeme->eGetStemStressPositions(sFromManagedString(sStem), vecPositions);
    if (H_NO_ERROR == eRet)
    {
        for (vector<int>::iterator it = vecPositions.begin(); it != vecPositions.end(); ++it)
        {
            listPositions.Add(*it);
        }
    }

    return (EM_ReturnCode)eRet;
}

/*
EM_ReturnCode CLexemeManaged::eGetAlternatingPreverb(String^ sVerbForm, String^% sPreverb, bool% bVoicing)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString cestrPreverb;
    bool cppbVoicing;
    ET_ReturnCode eRet = m_pLexeme->eGetAlternatingPreverb(sFromManagedString(sVerbForm), cestrPreverb, cppbVoicing);
    if (H_NO_ERROR == eRet)
    {
        sPreverb = gcnew String(cestrPreverb);
        bVoicing = cppbVoicing;
    }

    return (EM_ReturnCode)eRet;
}
*/

String^ CInflectionManaged::sHash()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return gcnew String(m_pInflection->sHash());
}
 
String^ CInflectionManaged::sStoredHash()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_sStoredEntryHash;
}

String^ CInflectionManaged::sParadigmHash()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pInflection->sParadigmHash());
}

EM_ReturnCode CInflectionManaged::eWordFormFromHash(String^ sHash, int iAt, CWordFormManaged^% wf)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    IWordForm * pWf = NULL;
    ET_ReturnCode eRet = m_pInflection->eWordFormFromHash(sFromManagedString(sHash), iAt, pWf);
    if (H_NO_ERROR == eRet)
    {
        if (pWf)
        {
            wf = gcnew CWordFormManaged(pWf);
        }
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CInflectionManaged::eCreateWordForm(CWordFormManaged ^% pWf)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    IWordForm * pIwf = NULL;
    ET_ReturnCode eRet = m_pInflection->eCreateWordForm(pIwf);

    pWf = gcnew CWordFormManaged(pIwf);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CInflectionManaged::eRemoveWordForm(String^ sHash, int iAt)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    ET_ReturnCode eRet = m_pInflection->eRemoveWordForm(sFromManagedString(sHash), iAt);
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CInflectionManaged::eRemoveWordForms(String^ sHash)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    ET_ReturnCode eRet = m_pInflection->eRemoveWordForms(sFromManagedString(sHash));
    return (EM_ReturnCode)eRet;
}

void CInflectionManaged::AddWordForm(CWordFormManaged^% Wf)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    IWordForm* pItf = Wf->pGetUnmanagedItf();
    m_pInflection->AddWordForm(pItf);
}

bool CInflectionManaged::bHasIrregularForm(String^ sGramHash)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->bHasIrregularForm(sFromManagedString(sGramHash));
}
 
bool CInflectionManaged::bNoRegularForms(String^ sGramHash)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->bNoRegularForms(sFromManagedString(sGramHash));
}
 
EM_ReturnCode CInflectionManaged::eGetFirstWordForm(CWordFormManaged^% wf)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    IWordForm * pWf = nullptr;
    ET_ReturnCode eRet = m_pInflection->eGetFirstWordForm(pWf);
    if (H_NO_ERROR == eRet)
    {
        if (pWf)
        {
            wf = gcnew CWordFormManaged(pWf);
        }
    }

    return (EM_ReturnCode)eRet;
}
 
EM_ReturnCode CInflectionManaged::eGetNextWordForm(CWordFormManaged^% wf)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    IWordForm * pWf = NULL;
    ET_ReturnCode eRet = m_pInflection->eGetNextWordForm(pWf);
    if (H_NO_ERROR == eRet)
    {
        if (pWf)
        {
            wf = gcnew CWordFormManaged(pWf);
        }
    }

    return (EM_ReturnCode)eRet;
}
 
EM_ReturnCode CInflectionManaged::eGetFirstIrregularForm(String^ sHash, CWordFormManaged^% wf, bool% bIsOptional)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    IWordForm * pWf = NULL;
    bool cppbIsOptional = false;
    ET_ReturnCode eRet = m_pInflection->eGetFirstIrregularForm(sFromManagedString(sHash), pWf, cppbIsOptional);
    if (H_NO_ERROR == eRet)
    {
        if (pWf)
        {
            wf = gcnew CWordFormManaged(pWf);
//            wf = gcnew CWordFormManaged(pWf);
        }
    }

    return (EM_ReturnCode)eRet;
}
 
EM_ReturnCode CInflectionManaged::eGetNextIrregularForm(CWordFormManaged^% wf, bool% bIsOptional)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }
    
    IWordForm * pWf = NULL;
    bool cppbIsOptional = false;
    ET_ReturnCode eRet = m_pInflection->eGetNextIrregularForm(pWf, cppbIsOptional);
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
 
int CInflectionManaged::iFormCount(String^ sHash)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pInflection->iFormCount(sFromManagedString(sHash));
}
 
bool CInflectionManaged::bHasCommonDeviation(int iCd)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pInflection->bHasCommonDeviation(iCd);
}
 
bool CInflectionManaged::bDeviationOptional(int iCd)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pInflection->bDeviationOptional(iCd);
}

EM_ReturnCode CInflectionManaged::eFormExists(String^ sGramHash)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (EM_ReturnCode)m_pInflection->eFormExists(sFromManagedString(sGramHash));
}

EM_ReturnCode CInflectionManaged::eSetFormExists(String^ sGramHash, bool bExists)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"m_pInflection object is NULL.");
    }

    return (EM_ReturnCode)m_pInflection->eSetFormExists(sFromManagedString(sGramHash), bExists);
}

EM_ReturnCode CInflectionManaged::eIsFormDifficult(String^ sGramHash)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"m_pInflection object is NULL.");
    }

    return (EM_ReturnCode)m_pInflection->eIsFormDifficult(sFromManagedString(sGramHash));
}

EM_ReturnCode CInflectionManaged::eSetFormDifficult(String^ sGramHash, bool bIsDifficult)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"m_pInflection object is NULL.");
    }

    return (EM_ReturnCode)m_pInflection->eSetFormDifficult(sFromManagedString(sGramHash), bIsDifficult);
}

bool CLexemeManaged::bHasDifficultForms()
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasDifficultForms();
}

EM_ReturnCode CInflectionManaged::eDifficultFormsHashes(List<String^>^% lstHashes)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    vector<CEString> vecHashes;
    ET_ReturnCode eRet = m_pInflection->eDifficultFormsHashes(vecHashes);
    if (ET_ReturnCode::H_NO_ERROR == eRet)
    {
        for (auto sHash : vecHashes)
        {
            lstHashes->Add(gcnew String(sHash));
        }
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CInflectionManaged::eIsFormAssumed(String^ sGramHash)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (EM_ReturnCode)m_pInflection->eIsFormAssumed(sFromManagedString(sGramHash));
}

EM_ReturnCode CInflectionManaged::eSetHasAssumedForms(bool bIsAssumed)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (EM_ReturnCode)m_pInflection->eSetHasAssumedForms(bIsAssumed);
}

bool CInflectionManaged::bIsMultistressedCompound()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return m_pInflection->bIsMultistressedCompound();
}
 
EM_ReturnCode CLexemeManaged::eGetSourceFormWithStress(String^% sSourceForm, bool bIsVariant)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString sSource;
    ET_ReturnCode eRet = m_pLexeme->eGetSourceFormWithStress(sSource, bIsVariant);

    sSourceForm = gcnew String(sSource);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetFirstStemStressPos(int% iPos)
{
    if (nullptr == m_pLexeme)
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
    if (nullptr == m_pLexeme)
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
 
EM_ReturnCode CLexemeManaged::eGetFirstSecondaryStemStressPos(int% iPos)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = m_pLexeme->eGetFirstSecondaryStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetNextSecondaryStemStressPos(int% iPos)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    int cppiPos = 0;
    ET_ReturnCode eRet = m_pLexeme->eGetNextSecondaryStemStressPos(cppiPos);
    if (H_NO_ERROR == eRet)
    {
        iPos = cppiPos;
    }

    return (EM_ReturnCode)eRet;
}

//EM_ReturnCode CLexemeManaged::eSetDb(const CEString& sDbPath)
//{
//    if (nullptr == m_pLexeme)
//    {
//        throw gcnew Exception(L"Lexeme object is NULL.");
//    }
//}

EM_ReturnCode CInflectionManaged::eGenerateParadigm()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (EM_ReturnCode)m_pInflection->eGenerateParadigm();
}

EM_ReturnCode CInflectionManaged::eSaveTestData()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (EM_ReturnCode)m_pInflection->eSaveTestData();
}

EM_ReturnCode CLexemeManaged::eCheckLexemeProperties() // for manual input/editing
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_ReturnCode)m_pLexeme->eCheckLexemeProperties();
}

EM_ReturnCode CInflectionManaged::eDeleteIrregularForm(String^ sFormHash)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (EM_ReturnCode)m_pInflection->eDeleteIrregularForm(sFromManagedString(sFormHash));
}

EM_ReturnCode CInflectionManaged::eSaveIrregularForms(String^ sGramHash)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    return (EM_ReturnCode)m_pInflection->eSaveIrregularForms(sFromManagedString(sGramHash));
}

EM_ReturnCode CInflectionManaged::eMakeGraphicStem()
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    auto eRet = m_pInflection->eMakeGraphicStem();

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CInflectionManaged::eMakeGraphicStem(const String^ sSource, String^% sGraphicStem)
{
    if (nullptr == m_pInflection)
    {
        throw gcnew Exception(L"Inflection object is NULL.");
    }

    CEString sGs;
    auto eRet = m_pInflection->eMakeGraphicStem(sFromManagedString(sSource), sGs);
    if (eRet != H_NO_ERROR)
    {
        return (EM_ReturnCode)eRet;
    }

    sGraphicStem = gcnew String(sGs);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetErrorMsg(String^% sErrorMsg)
{
    if (nullptr == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString sNativeErrMsg;
    ET_ReturnCode eRet = m_pLexeme->eGetErrorMsg(sNativeErrMsg);
    if (eRet != H_NO_ERROR)
    {
        return (EM_ReturnCode)eRet;
    }

    sErrorMsg = gcnew String(sNativeErrMsg);

    return (EM_ReturnCode)H_NO_ERROR;
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
    if (nullptr == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_ePos = (ET_PartOfSpeech)ePos;
}

void CGramHasherManaged::SetSubparadigm(EM_Subparadigm eSp)
{
    if (nullptr == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eSubparadigm = (ET_Subparadigm)eSp;
}

void CGramHasherManaged::SetCase(EM_Case eCase)
{
    if (nullptr == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eCase = (ET_Case)eCase;
}

void CGramHasherManaged::SetNumber(EM_Number eNumber)
{
    if (nullptr == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eNumber = (ET_Number)eNumber;
}

void CGramHasherManaged::SetGender(EM_Gender eGender)
{
    if (nullptr == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eGender = (ET_Gender)eGender;
}

void CGramHasherManaged::SetPerson(EM_Person ePerson)
{
    if (nullptr == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_ePerson = (ET_Person)ePerson;
}

void CGramHasherManaged::SetAnimacy(EM_Animacy eAnimacy)
{
    if (nullptr == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eAnimacy = (ET_Animacy)eAnimacy;
}

void CGramHasherManaged::SetReflexivity(EM_Reflexive eRefl)
{
    if (nullptr == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eReflexivity = (ET_Reflexivity)eRefl;
}

void CGramHasherManaged::SetAspect(EM_Aspect eAspect)
{
    if (nullptr == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eAspect = (ET_Aspect)eAspect;
}

String^ CGramHasherManaged::sHash()
{
    if (nullptr == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    return gcnew String(m_pHasher->sGramHash());
}

/*
EM_ReturnCode CSqliteManaged::eExportTable(string sPathToTextFile, array<String^>^ arrTableNames)
{
    if (nullptr == m_pDb)
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

CLexemeEnumeratorManaged::CLexemeEnumeratorManaged(ILexemeEnumerator* pLe) : m_pLexemeEnumerator(pLe)
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
    return (EM_ReturnCode) m_pLexemeEnumerator->eReset();
}

EM_ReturnCode CLexemeEnumeratorManaged::eGetFirstLexeme(CLexemeManaged^% pLexemeItf)
{
    if (nullptr == m_pLexemeEnumerator)
    {
        throw gcnew Exception(L"Lexeme enumerator object is NULL.");
    }

    ILexeme* pLexeme = NULL;
    ET_ReturnCode eRet = m_pLexemeEnumerator->eGetFirstLexeme(pLexeme);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (pLexeme)
        {
            pLexemeItf = gcnew CLexemeManaged(pLexeme);
        }
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeEnumeratorManaged::eGetNextLexeme(CLexemeManaged^% pLexemeItf)
{
    if (nullptr == m_pLexemeEnumerator)
    {
        throw gcnew Exception(L"Lexeme enumerator object is NULL.");
    }

    ILexeme* pLexeme = NULL;
    ET_ReturnCode eRet = m_pLexemeEnumerator->eGetNextLexeme(pLexeme);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (pLexeme)
        {
            pLexemeItf = gcnew CLexemeManaged(pLexeme);
        }
    }
    return (EM_ReturnCode)eRet;
}

CInflectionEnumeratorManaged::CInflectionEnumeratorManaged(IInflectionEnumerator* pIe) : m_pInflectionEnumerator(pIe)
{}

CInflectionEnumeratorManaged::~CInflectionEnumeratorManaged()
{
    if (nullptr == m_pInflectionEnumerator)
    {
        throw gcnew Exception(L"Inflection enumerator object is NULL.");
    }
    delete m_pInflectionEnumerator;
}

EM_ReturnCode CInflectionEnumeratorManaged::eReset()
{
    if (nullptr == m_pInflectionEnumerator)
    {
        throw gcnew Exception(L"Inflection enumerator object is NULL.");
    }
    return (EM_ReturnCode)m_pInflectionEnumerator->eReset();
}

EM_ReturnCode CInflectionEnumeratorManaged::eGetFirstInflection(CInflectionManaged^% pInflectionItf)
{
    if (nullptr == m_pInflectionEnumerator)
    {
        throw gcnew Exception(L"Inflection enumerator object is NULL.");
    }

    IInflection* pInflection = NULL;
    ET_ReturnCode eRet = m_pInflectionEnumerator->eGetFirstInflection(pInflection);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (pInflection)
        {
            pInflectionItf = gcnew CInflectionManaged(pInflection);
        }
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CInflectionEnumeratorManaged::eGetNextInflection(CInflectionManaged^% pInflectionItf)
{
    if (nullptr == m_pInflectionEnumerator)
    {
        throw gcnew Exception(L"Inflection enumerator object is NULL.");
    }

    IInflection* pInflection = NULL;
    ET_ReturnCode eRet = m_pInflectionEnumerator->eGetNextInflection(pInflection);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (pInflection)
        {
            pInflectionItf = gcnew CInflectionManaged(pInflection);
        }
    }
    return (EM_ReturnCode)eRet;
}

CParserManaged::CParserManaged(IParser * pParser) : m_pParser(pParser)
{}

CParserManaged::~CParserManaged()
{
    delete m_pParser;

//    this->!CParserManaged();
}

EM_ReturnCode CParserManaged::eParseWord(String^ sForm)
{
    if (nullptr == m_pParser)
    {
        throw gcnew Exception(L"Parser object is NULL.");
    }

    return (EM_ReturnCode)m_pParser->eParseWord(sFromManagedString(sForm));
}

EM_ReturnCode CParserManaged::eGetFirstWordForm(CWordFormManaged^% pManagedWordFrom)
{
    if (nullptr == m_pParser)
    {
        throw gcnew Exception(L"Parser object is NULL.");
    }

    IWordForm * pWordForm = NULL;
    ET_ReturnCode eRet = m_pParser->eGetFirstWordForm(pWordForm);
    if (H_NO_ERROR == eRet)
    {
        if (pWordForm)
        {
            pManagedWordFrom = gcnew CWordFormManaged(pWordForm);
        }
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CParserManaged::eGetNextWordForm(CWordFormManaged^% pManagedWordFrom)
{
    if (nullptr == m_pParser)
    {
        throw gcnew Exception(L"Parser object is NULL.");
    }

    IWordForm * pWordForm = NULL;
    ET_ReturnCode eRet = m_pParser->eGetNextWordForm(pWordForm);
    if (H_NO_ERROR == eRet)
    {
        if (pWordForm)
        {
            pManagedWordFrom = gcnew CWordFormManaged(pWordForm);
        }
        else
        {
            return EM_ReturnCode::H_ERROR_UNEXPECTED;
        }
    }

    return (EM_ReturnCode)eRet;
}


//
//      CAnalyticsManaged

CAnalyticsManaged::CAnalyticsManaged(IAnalytics* pAnalytics) : m_pAnalytics(pAnalytics)
{}

CAnalyticsManaged::~CAnalyticsManaged()
{
    delete m_pAnalytics;
}

EM_ReturnCode CAnalyticsManaged::eParseText(String^ sName, String^ sMetaData, String^ sText, long long% lParsedTextId, bool bIsProse)
{
    if (nullptr == m_pAnalytics)
    {
        throw gcnew Exception(L"Analytics object is NULL.");
    }

    long long llId = 0;
    auto eRet = (EM_ReturnCode)m_pAnalytics->eParseText(sFromManagedString(sName), sFromManagedString(sMetaData), sFromManagedString(sText), llId, bIsProse);
    lParsedTextId = llId;

    return eRet;
}

/////////

CVerifierManaged::CVerifierManaged(IVerifier * pVerifier) : m_pVerifier(pVerifier)
{}

CVerifierManaged::~CVerifierManaged()
{}

//EM_ReturnCode CVerifierManaged::eSetHeadword(String^ sHeadword)
//{
//    if (nullptr == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return (EM_ReturnCode)m_pVerifier->eSetHeadword(sFromManagedString(sHeadword));
//}
//
//String^ CVerifierManaged::sGetHeadword()
//{
//    if (nullptr == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return gcnew String(m_pVerifier->sGetHeadword());
//}
//
//void CVerifierManaged::SetLexemeHash(String^ sHash)
//{
//    if (nullptr == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return m_pVerifier->SetLexemeHash(sFromManagedString(sHash));
//}
//
//String^ CVerifierManaged::sGetLexemeHash()
//{
//    if (nullptr == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return gcnew String(m_pVerifier->sGetLexemeHash());
//}

EM_ReturnCode CVerifierManaged::eVerify(String^ sLexemeHash)
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_ReturnCode)m_pVerifier->eVerify(sFromManagedString(sLexemeHash));
}

EM_TestResult CVerifierManaged::eResult()
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_TestResult)m_pVerifier->eResult();
}

int CVerifierManaged::iCount()
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return m_pVerifier->iCount();
}

EM_ReturnCode CVerifierManaged::eLoadStoredLexemes()
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_ReturnCode)m_pVerifier->eLoadStoredLexemes();
}

EM_ReturnCode CVerifierManaged::eDeleteStoredLexeme(String^ sLexeme)
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_ReturnCode)m_pVerifier->eDeleteStoredLexeme(sFromManagedString(sLexeme));
}

EM_ReturnCode CVerifierManaged::eGetFirstLexemeData(String^% sHash, String^% sHeadword)
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    CEString cestrHash;
    CEString cestrHeadword;
    ET_ReturnCode eRet = m_pVerifier->eGetFirstLexemeData(cestrHash, cestrHeadword);
    if (H_NO_ERROR == eRet)
    {
        sHash = gcnew String(cestrHash);
        sHeadword = gcnew String(cestrHeadword);
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CVerifierManaged::eGetNextLexemeData(String^% sHash, String^% sHeadword)
{
    if (nullptr == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    CEString cestrHash;
    CEString cestrHeadword;
    ET_ReturnCode eRet = m_pVerifier->eGetNextLexemeData(cestrHash, cestrHeadword);
    if (H_NO_ERROR == eRet)
    {
        sHash = gcnew String(cestrHash);
        sHeadword = gcnew String(cestrHeadword);
    }

    return (EM_ReturnCode)eRet;
}
