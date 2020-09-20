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

IWordForm * CWordFormManaged::pGetUnmanagedItf()
{
    return m_pWordForm;
}

String^ CWordFormManaged::sWordForm()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String (m_pWordForm->sWordForm());
}

void CWordFormManaged::SetWordForm(String^ sWordForm)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    m_pWordForm->SetWordForm(sFromManagedString(sWordForm));
}

long long CWordFormManaged::llWordFormDbId()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    return m_pWordForm->llDbId();
}

String^ CWordFormManaged::sStem()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String(m_pWordForm->sStem());
}

void CWordFormManaged::SetStem(String^ sStem)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    m_pWordForm->SetStem(sFromManagedString(sStem));
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

void CWordFormManaged::SetPos(EM_PartOfSpeech ePos)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"Word form object is NULL.");
    }

    m_pWordForm->SetPos((ET_PartOfSpeech)ePos);
}

EM_Case CWordFormManaged::eCase()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Case)m_pWordForm->eCase();
}

void CWordFormManaged::SetCase(EM_Case eCase)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetCase((ET_Case)eCase);
}

EM_Number CWordFormManaged::eNumber()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Number)m_pWordForm->eNumber();
}

void CWordFormManaged::SetNumber(EM_Number eNumber)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetNumber((ET_Number)eNumber);
}

EM_Subparadigm CWordFormManaged::eSubparadigm()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Subparadigm)m_pWordForm->eSubparadigm();
}

void CWordFormManaged::SetSubparadigm(EM_Subparadigm eSubparadigm)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetSubparadigm((ET_Subparadigm)eSubparadigm);
}

EM_Gender CWordFormManaged::eGender()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Gender)m_pWordForm->eGender();
}

void CWordFormManaged::SetGender(EM_Gender eGender)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetGender((ET_Gender)eGender);
}

EM_Person CWordFormManaged::ePerson()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Person)m_pWordForm->ePerson();
}

void CWordFormManaged::SetPerson(EM_Person ePerson)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetPerson((ET_Person)ePerson);
}

EM_Animacy CWordFormManaged::eAnimacy()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Animacy)m_pWordForm->eAnimacy();
}

void CWordFormManaged::SetAnimacy(EM_Animacy eAnimacy)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetAnimacy((ET_Animacy)eAnimacy);
}

EM_Reflexive CWordFormManaged::eReflexive()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Reflexive)m_pWordForm->eReflexive();
}

void CWordFormManaged::SetReflexive(EM_Reflexive eReflexive)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetReflexive((ET_Reflexivity)eReflexive);
}

EM_Aspect CWordFormManaged::eAspect()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Aspect)m_pWordForm->eAspect();
}

void CWordFormManaged::SetAspect(EM_Aspect eAspect)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetAspect((ET_Aspect)eAspect);
}

EM_Status CWordFormManaged::eStatus()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_Status)m_pWordForm->eStatus();
}

void CWordFormManaged::SetStatus(EM_Status eStatus)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetStatus((ET_Status)eStatus);
}

bool CWordFormManaged::bIrregular()      // came from the DB as opposed to being generated by the app
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->bIrregular();
}

void CWordFormManaged::SetIrregular(bool bIrregular)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetIrregular(bIrregular);
}

String^ CWordFormManaged::sLeadComment()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String(m_pWordForm->sLeadComment());
}

void CWordFormManaged::SetLeadComment(String^ sLeadComment)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetLeadComment(sFromManagedString(sLeadComment));
}

String^ CWordFormManaged::sTrailingComment()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String(m_pWordForm->sTrailingComment());
}

void CWordFormManaged::SetTrailingComment(String^ sTrailingComment)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    m_pWordForm->SetTrailingComment(sFromManagedString(sTrailingComment));
}

bool CWordFormManaged::bIsEdited()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->bIsEdited();
}

void CWordFormManaged::SetIsEdited(bool bIsEdited)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->SetIsEdited(bIsEdited);
}

bool CWordFormManaged::bIsVariant()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->bIsVariant();
}

void CWordFormManaged::SetIsVariant(bool bIsVariant)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return m_pWordForm->SetIsVariant(bIsVariant);
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

EM_ReturnCode CWordFormManaged::eSetStressPositions(Collections::Generic::Dictionary<int, EM_StressType>^ dctStressPositions)
{
    if (NULL == m_pWordForm)
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
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return gcnew String(m_pWordForm->sGramHash());
}

EM_ReturnCode CWordFormManaged::eInitFromHash(String^ sHash)
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_ReturnCode)m_pWordForm->eInitFromHash(sFromManagedString(sHash));
}

EM_ReturnCode CWordFormManaged::eSaveIrregularForm()
{
    if (NULL == m_pWordForm)
    {
        throw gcnew Exception(L"WordForm object is NULL.");
    }

    return (EM_ReturnCode)m_pWordForm->eSaveIrregularForm();
}

/*
EM_ReturnCode CWordFormManaged::eSetIrregularStressPositions(Dictionary<int, EM_StressType>^ dictPositions)
{
    if (NULL == m_pWordForm)
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
*/

CDictionaryManaged::CDictionaryManaged()
{
    Hlib::IDictionary * pD = NULL;
    ET_ReturnCode rc = GetDictionary(pD);
    m_pDictionary = pD;
}

CDictionaryManaged::~CDictionaryManaged()
{
    delete m_pDictionary;
}

EM_ReturnCode CDictionaryManaged::eSetDbPath(String^ sDbPath)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)m_pDictionary->eSetDbPath(sFromManagedString(sDbPath));
}

String^ CDictionaryManaged::sGetDbPath()
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return gcnew String(m_pDictionary->sGetDbPath());
}

CLexemeManaged^ CDictionaryManaged::CreateLexemeForEdit()
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary itf handle is NULL.");
    }

    ILexeme * pLexeme = NULL;
    ET_ReturnCode eRet = m_pDictionary->eCreateLexemeForEdit(pLexeme);
    if (eRet != H_NO_ERROR || NULL == pLexeme)
    {
        throw gcnew Exception(L"Unable to create lexeme.");
    }

    CLexemeManaged^ pManaged = gcnew CLexemeManaged();
    pManaged->m_pLexeme = pLexeme;

    return pManaged;
}

CLexemeManaged^ CDictionaryManaged::CopyLexemeForEdit(CLexemeManaged^ source)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary itf handle is NULL.");
    }

    ILexeme * pCopy = NULL;
    ET_ReturnCode eRet = m_pDictionary->eCopyLexemeForEdit(source->m_pLexeme, pCopy);
    if (eRet != H_NO_ERROR || NULL == pCopy)
    {
        throw gcnew Exception(L"Unable to copy lexeme.");
    }

    CLexemeManaged^ pManaged = gcnew CLexemeManaged();
    pManaged->m_pLexeme = pCopy;
    pManaged->m_sStoredLexemeHash = gcnew String(pCopy->sHash());

    return pManaged;
}

EM_ReturnCode CDictionaryManaged::eGetLexemeById(long long llId, CLexemeManaged^% lexeme)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ILexeme * pLexeme = NULL;
    ET_ReturnCode eRet = m_pDictionary->eGetLexemeById(llId, pLexeme);
    if (H_NO_ERROR == eRet || H_NO_MORE == eRet)
    {
        if (pLexeme)
        {
            lexeme = gcnew CLexemeManaged(pLexeme);
        }
    }
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eGetLexemesByHash(String^ sHash)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)m_pDictionary->eGetLexemesByHash(sFromManagedString(sHash));
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
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return (EM_ReturnCode)m_pDictionary->eGetLexemesByInitialForm(sFromManagedString(sInitForm));
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

EM_ReturnCode CDictionaryManaged::eCountLexemes(Int64% iLexemes)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    int64_t cppiLexemes = 0;
    ET_ReturnCode eRet = m_pDictionary->eCountLexemes(cppiLexemes);
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
//    if (NULL == m_pDictionary)
//    {
//        throw gcnew Exception(L"Dictionary object is NULL.");
//    }
//
//    ET_ReturnCode eRet = m_pDictionary->eSaveLexeme(l->m_pLexeme);
//
//    return (EM_ReturnCode)eRet;
//}

EM_ReturnCode CDictionaryManaged::eSaveHeadword(CLexemeManaged^ l)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = m_pDictionary->eSaveHeadword(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveAspectPairInfo(CLexemeManaged^ l)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = m_pDictionary->eSaveAspectPairInfo(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveP2Info(CLexemeManaged^ l)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = m_pDictionary->eSaveP2Info(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveDescriptorInfo(CLexemeManaged^ l)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = m_pDictionary->eSaveDescriptorInfo(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveInflectionInfo(CLexemeManaged^ l)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = m_pDictionary->eSaveInflectionInfo(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eSaveCommonDeviation(CLexemeManaged^ l)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = m_pDictionary->eSaveCommonDeviation(l->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

int CDictionaryManaged::nLexemesFound()
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    return m_pDictionary->nLexemesFound();
}

void CDictionaryManaged::Clear()
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    m_pDictionary->Clear();
}

EM_ReturnCode CDictionaryManaged::Clear(CLexemeManaged^ pLexeme)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    ET_ReturnCode eRet = m_pDictionary->Clear(pLexeme->m_pLexeme);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CDictionaryManaged::eCreateLexemeEnumerator(CLexemeEnumeratorManaged^% pLeManaged)
{
    if (NULL == m_pDictionary)
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
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    IParser * pParser = NULL;
    ET_ReturnCode eRet = m_pDictionary->eGetParser(pParser);
    if (H_NO_ERROR == eRet)
    {
        if (pParser)
        {
            pParserManaged = gcnew CParserManaged(pParser);
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
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    IAnalytics* pAnalytics = NULL;
    ET_ReturnCode eRet = m_pDictionary->eGetAnalytics(pAnalytics);
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
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    IVerifier * pVerifier = NULL;
    ET_ReturnCode eRet = m_pDictionary->eGetVerifier(pVerifier);
    if (H_NO_ERROR == eRet)
    {
        if (pVerifier)
        {
            verifier = gcnew CVerifierManaged(pVerifier);
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
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    GCHandle gch = GCHandle::Alloc(progressCallback);
    IntPtr iptr = Marshal::GetFunctionPointerForDelegate(progressCallback);
    PROGRESS_CALLBACK_CLR pProgress = static_cast<PROGRESS_CALLBACK_CLR>(iptr.ToPointer());

    return (EM_ReturnCode)m_pDictionary->eExportTestData(sFromManagedString(sPath), *pProgress);
}

EM_ReturnCode CDictionaryManaged::eImportTestData(String^ sPath, DelegateProgress^ progressCallback)
{
    if (NULL == m_pDictionary)
    {
        throw gcnew Exception(L"Dictionary object is NULL.");
    }

    GCHandle gch = GCHandle::Alloc(progressCallback);
    IntPtr iptr = Marshal::GetFunctionPointerForDelegate(progressCallback);
    PROGRESS_CALLBACK_CLR pProgress = static_cast<PROGRESS_CALLBACK_CLR>(iptr.ToPointer());

    return (EM_ReturnCode)m_pDictionary->eImportTestData(sFromManagedString(sPath), *pProgress);
}


//
// Managed wrapper for Lexeme class
//
CLexemeManaged::CLexemeManaged()
{
    m_sStoredLexemeHash = "";
}

CLexemeManaged::CLexemeManaged(String^ sGramHash)
{
    m_sStoredLexemeHash = "";
}

CLexemeManaged::CLexemeManaged(ILexeme * pLexeme) : m_pLexeme(pLexeme)
{
    m_sStoredLexemeHash = gcnew String(m_pLexeme->sHash());
}

CLexemeManaged::~CLexemeManaged()
{
    delete m_pLexeme;
    m_pLexeme = NULL;
    m_sStoredLexemeHash = "";
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
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->llLexemeId();
}


EM_Gender CLexemeManaged::eGender()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Gender)m_pLexeme->eInflectionTypeToGender();
}
 
String^ CLexemeManaged::sGraphicStem()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sGraphicStem());
}

void CLexemeManaged::SetGraphicStem(String^ sGraphicStem)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetGraphicStem(sFromManagedString(sGraphicStem));
}

bool CLexemeManaged::bHasIrregularForms()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasIrregularForms();
}

void CLexemeManaged::SetHasIrregularForms(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHasIrregularForms(bValue);
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
 
void CLexemeManaged::SetHasFleetingVowel(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHasFleetingVowel(bValue);
}

bool CLexemeManaged::bHasYoAlternation()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasYoAlternation();
}
 
void CLexemeManaged::SetHasYoAlternation(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHasYoAlternation(bValue);
}

bool CLexemeManaged::bHasOAlternation()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasOAlternation();
}
 
void CLexemeManaged::SetHasOAlternation(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHasOAlternation(bValue);
}

String^ CLexemeManaged::sSourceForm()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sSourceForm());
}
 
void CLexemeManaged::SetSourceForm(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSourceForm(sFromManagedString(sValue));
}

bool CLexemeManaged::bHasHomonyms()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->stGetProperties().vecHomonyms.size() > 0;
}

List<int>^ CLexemeManaged::arrHomonyms()
{
    if (NULL == m_pLexeme)
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
    if (NULL == m_pLexeme)
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
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sHeadwordComment());
}

void CLexemeManaged::SetHeadwordComment(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHeadwordComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sHeadwordVariant()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sHeadwordVariant());
}

void CLexemeManaged::SetHeadwordVariant(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHeadwordVariant(sFromManagedString(sValue));
}

String^ CLexemeManaged::sHeadwordVariantComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sHeadwordVariantComment());
}

void CLexemeManaged::SetHeadwordVariantComment(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHeadwordVariantComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sPluralOf()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sPluralOf());
}

void CLexemeManaged::SetPluralOf(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetPluralOf(sFromManagedString(sValue));
}

String^ CLexemeManaged::sUsage()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sUsage());
}

void CLexemeManaged::SetUsage(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetUsage(sFromManagedString(sValue));
}

String^ CLexemeManaged::sSeeRef()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sSeeRef());
}

void CLexemeManaged::SetSeeRef(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSeeRef(sFromManagedString(sValue));
}

String^ CLexemeManaged::sBackRef()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sBackRef());
}

void CLexemeManaged::SetBackRef(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetBackRef(sFromManagedString(sValue));
}

bool CLexemeManaged::bIsUnstressed()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsUnstressed();
}
 
void CLexemeManaged::SetIsUnstressed(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->SetIsUnstressed(bValue);
}

bool CLexemeManaged::bIsVariant()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsVariant();
}
 
void CLexemeManaged::SetIsVariant(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetIsVariant(bValue);
}

String^ CLexemeManaged::sMainSymbol()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sMainSymbol());
}
 
void CLexemeManaged::SetMainSymbol(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetMainSymbol(sFromManagedString(sValue));
}

bool CLexemeManaged::bIsPluralOf()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsPluralOf();
}
 
void CLexemeManaged::SetIsPluralOf(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetIsPluralOf(bValue);
}

bool CLexemeManaged::bTransitive()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bTransitive();
}
 
void CLexemeManaged::SetTransitive(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->SetTransitive(bValue);
}

EM_Reflexive CLexemeManaged::eIsReflexive()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Reflexive)m_pLexeme->eIsReflexive();
}
 
void CLexemeManaged::SetIsReflexive(EM_Reflexive eValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetIsReflexive((ET_Reflexivity)eValue);
}

String^ CLexemeManaged::sMainSymbolPluralOf()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sMainSymbolPluralOf());
}
 
void CLexemeManaged::SetMainSymbolPluralOf(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetMainSymbolPluralOf(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltMainSymbol()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAltMainSymbol());
}
 
void CLexemeManaged::SetAltMainSymbol(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAltMainSymbol(sFromManagedString(sValue));
}

EM_Aspect CLexemeManaged::eAspect()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_Aspect)m_pLexeme->eAspect();
}
 
void CLexemeManaged::SetAspect(EM_Aspect eValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->SetAspect((ET_Aspect)eValue);
}

String^ CLexemeManaged::sInflectionType()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sInflectionType());
}
 
void CLexemeManaged::SetInflectionType(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetInflectionType(sFromManagedString(sValue));
}

EM_PartOfSpeech CLexemeManaged::ePartOfSpeech()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_PartOfSpeech)m_pLexeme->ePartOfSpeech();
}
 
void CLexemeManaged::SetPartOfSpeech(EM_PartOfSpeech eValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetPartOfSpeech((ET_PartOfSpeech)eValue);
}

String^ CLexemeManaged::sComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sComment());
}
 
void CLexemeManaged::SetComment(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltMainSymbolComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAltMainSymbolComment());
}
 
void CLexemeManaged::SetAltMainSymbolComment(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAltMainSymbolComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sAltInflectionComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAltInflectionComment());
}
 
void CLexemeManaged::SetAltInflectionComment(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAltInflectionComment(sFromManagedString(sValue));
}

String^ CLexemeManaged::sVerbStemAlternation()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sVerbStemAlternation());
}
 
void CLexemeManaged::SetVerbStemAlternation(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetVerbStemAlternation(sFromManagedString(sValue));
}

bool CLexemeManaged::bPartPastPassZhd()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bPartPastPassZhd();
}
 
void CLexemeManaged::SetPartPastPassZhd(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetPartPastPassZhd(bValue);
}

int CLexemeManaged::iSection()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->iSection();
}
 
void CLexemeManaged::SetSection(int iValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSection(iValue);
}

bool CLexemeManaged::bNoComparative()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bNoComparative();
}
 
void CLexemeManaged::SetNoComparative(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetNoComparative(bValue);
}

bool CLexemeManaged::bAssumedForms()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bAssumedForms();
}
 
void CLexemeManaged::SetAssumedForms(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAssumedForms(bValue);
}

//bool CLexemeManaged::bYoAlternation()
//{
//    if (NULL == m_pLexeme)
//    {
//        throw gcnew Exception(L"Lexeme object is NULL.");
//    }

//    return m_pLexeme->bYoAlternation();
//}
 
//void CLexemeManaged::SetYoAlternation(bool bValue)
//{
//    if (NULL == m_pLexeme)
//    {
//        throw gcnew Exception(L"Lexeme object is NULL.");
//    }

//    m_pLexeme->SetYoAlternation(bValue);
//}

//bool CLexemeManaged::bOAlternation()
//{
//    if (NULL == m_pLexeme)
//    {
//        throw gcnew Exception(L"Lexeme object is NULL.");
//    }

//    return m_pLexeme->bOAlternation();
//}
 
bool CLexemeManaged::bSecondGenitive()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bSecondGenitive();
}
 
void CLexemeManaged::SetSecondGenitive(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSecondGenitive(bValue);
}
 
bool CLexemeManaged::bSecondPrepositional()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bSecondPrepositional();
}

void CLexemeManaged::SetSecondPrepositional(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSecondPrepositional(bValue);
}

bool CLexemeManaged::bSecondPrepositionalOptional()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bSecondPrepositionalOptional();
}
 
void CLexemeManaged::SetSecondPrepositionalOptional(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSecondPrepositionalOptional(bValue);
}

String^ CLexemeManaged::sP2Preposition()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sP2Preposition());
}
 
void CLexemeManaged::SetP2Preposition(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetP2Preposition(sFromManagedString(sValue));
}

bool CLexemeManaged::bHasAspectPair()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasAspectPair();
}
 
void CLexemeManaged::SetHasAspectPair(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->SetHasAspectPair(bValue);
}

bool CLexemeManaged::bHasAltAspectPair()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasAltAspectPair();
}

int CLexemeManaged::iAspectPairType()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return m_pLexeme->iAspectPairType();
}
 
void CLexemeManaged::SetAspectPairType(int iValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    m_pLexeme->SetAspectPairType(iValue);
}

int CLexemeManaged::iAltAspectPairType()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    return m_pLexeme->iAltAspectPairType();
}

void CLexemeManaged::SetAltAspectPairType(int iValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }
    m_pLexeme->SetAltAspectPairType(iValue);
}

String^ CLexemeManaged::sAltAspectPairComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAltAspectPairComment());
}

void CLexemeManaged::SetAltAspectPairComment(String^ sValue)
{
    if (NULL == m_pLexeme)
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

    if (NULL == m_pLexeme)
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
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sAspectPairData());
}

void CLexemeManaged::SetAspectPairData(String^ sAspectPairData)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAspectPairData(sFromManagedString(sAspectPairData));
}

bool CLexemeManaged::bHasIrregularVariants()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasIrregularVariants();
}
 
void CLexemeManaged::SetHasIrregularVariants(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetHasIrregularVariants(bValue);
}

String^ CLexemeManaged::sRestrictedForms()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sRestrictedForms());
}

void CLexemeManaged::SetRestrictedForms(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetRestrictedForms(sFromManagedString(sValue));
}
 
String^ CLexemeManaged::sContexts()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sContexts());
}
 
void CLexemeManaged::SetContexts(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetContexts(sFromManagedString(sValue));
}

String^ CLexemeManaged::sTrailingComment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sTrailingComment());
}
 
void CLexemeManaged::SetTrailingComment(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetTrailingComment(sFromManagedString(sValue));
}

int CLexemeManaged::iInflectionId()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->iInflectionId();
}
 
void CLexemeManaged::SetInflectionId(int iValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetInflectionId(iValue);
}

bool CLexemeManaged::bPrimaryInflectionGroup()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bPrimaryInflectionGroup();
}
 
void CLexemeManaged::SetPrimaryInflectionGroup(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->SetPrimaryInflectionGroup(bValue);
}

int CLexemeManaged::iType()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->iType();
}
 
void CLexemeManaged::SetType(int iValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->SetType(iValue);
}

EM_AccentType CLexemeManaged::eAccentType1()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_AccentType)m_pLexeme->eAccentType1();
}
 
void CLexemeManaged::SetAccentType1(EM_AccentType eValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAccentType1((ET_AccentType)eValue);
}

EM_AccentType CLexemeManaged::eAccentType2()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_AccentType)m_pLexeme->eAccentType2();
}
 
void CLexemeManaged::SetAccentType2(EM_AccentType eValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetAccentType2((ET_AccentType)eValue);
}

bool CLexemeManaged::bShortFormsRestricted()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bShortFormsRestricted();
}
 
void CLexemeManaged::SetShortFormsRestricted(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetShortFormsRestricted(bValue);
}

bool CLexemeManaged::bPastParticipleRestricted()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bPastParticipleRestricted();
}
 
void CLexemeManaged::SetPastParticipleRestricted(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetPastParticipleRestricted(bValue);
}

bool CLexemeManaged::bNoLongForms()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bNoLongForms();
}
 
void CLexemeManaged::SetNoLongForms(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetNoLongForms(bValue);
}

bool CLexemeManaged::bShortFormsIncomplete()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bShortFormsIncomplete();
}
 
void CLexemeManaged::SetShortFormsIncomplete(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetShortFormsIncomplete(bValue);
}

bool CLexemeManaged::bNoPassivePastParticiple()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bNoPassivePastParticiple();
}
 
void CLexemeManaged::SetNoPassivePastParticiple(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetNoPassivePastParticiple(bValue);
}

bool CLexemeManaged::bFleetingVowel()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bFleetingVowel();
}
 
void CLexemeManaged::SetFleetingVowel(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetFleetingVowel(bValue);
}

int CLexemeManaged::iStemAugment()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->iStemAugment();
}
 
void CLexemeManaged::SetStemAugment(int iValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->SetStemAugment(iValue);
}

String^ CLexemeManaged::s1SgStem()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->s1SgStem());
}
 
void CLexemeManaged::Set1SgStem(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->Set1SgStem(sFromManagedString(sValue));
}

String^ CLexemeManaged::s3SgStem()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->s3SgStem());
}

void CLexemeManaged::Set3SgStem(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->Set3SgStem(sFromManagedString(sValue));
}

String^ CLexemeManaged::sInfinitive()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sInfinitive());
}
 
void CLexemeManaged::SetInfinitive(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetInfinitive(sFromManagedString(sValue));
}

String^ CLexemeManaged::sInfStem()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sInfStem());
}

void CLexemeManaged::SetInfStem(String^ sValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetInfStem(sFromManagedString(sValue));
}

int CLexemeManaged::iInflectedParts()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->iInflectedParts();
}

void CLexemeManaged::SetInflectedParts(int iValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetInflectedParts(iValue);
}

bool CLexemeManaged::bIsSecondPart()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsSecondPart();
}

void CLexemeManaged::SetSecondPart(bool bValue)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    m_pLexeme->SetSecondPart(bValue);
}

EM_ReturnCode CLexemeManaged::eAddCommonDeviation(int iValue, bool bIsOptional)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    ET_ReturnCode eRet = m_pLexeme->eAddCommonDeviation(iValue, bIsOptional);
    return (EM_ReturnCode)eRet;
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

    CEString cestrValue;
    bool bRet = m_pLexeme->bFindStandardAlternation(sFromManagedString(sKey), cestrValue);
    sValue = gcnew String(sValue);

    return bRet;
}

EM_ReturnCode CLexemeManaged::eGetStemStressPositions(String^ sStem, List<int>% listPositions)
{
    if (NULL == m_pLexeme)
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
 
EM_ReturnCode CLexemeManaged::eGetAlternatingPreverb(String^ sVerbForm, String^% sPreverb, bool% bVoicing)
{
    if (NULL == m_pLexeme)
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
 
String^ CLexemeManaged::sHash()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return gcnew String(m_pLexeme->sHash());
}
 
String^ CLexemeManaged::sStoredHash()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_sStoredLexemeHash;
}

EM_ReturnCode CLexemeManaged::eWordFormFromHash(String^ sHash, int iAt, CWordFormManaged^% wf)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    IWordForm * pWf = NULL;
    ET_ReturnCode eRet = m_pLexeme->eWordFormFromHash(sFromManagedString(sHash), iAt, pWf);
    if (H_NO_ERROR == eRet)
    {
        if (pWf)
        {
            wf = gcnew CWordFormManaged(pWf);
        }
    }

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eCreateWordForm(CWordFormManaged ^% pWf)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    IWordForm * pIwf = NULL;
    ET_ReturnCode eRet = m_pLexeme->eCreateWordForm(pIwf);

    pWf = gcnew CWordFormManaged(pIwf);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eRemoveWordForm(String^ sHash, int iAt)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    ET_ReturnCode eRet = m_pLexeme->eRemoveWordForm(sFromManagedString(sHash), iAt);
    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eRemoveWordForms(String^ sHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    ET_ReturnCode eRet = m_pLexeme->eRemoveWordForms(sFromManagedString(sHash));
    return (EM_ReturnCode)eRet;
}

void CLexemeManaged::AddWordForm(CWordFormManaged^% Wf)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    IWordForm* pItf = Wf->pGetUnmanagedItf();
    m_pLexeme->AddWordForm(pItf);
}

bool CLexemeManaged::bHasIrregularForm(String^ sGramHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bHasIrregularForm(sFromManagedString(sGramHash));
}
 
bool CLexemeManaged::bNoRegularForms(String^ sGramHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bNoRegularForms(sFromManagedString(sGramHash));
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
 
EM_ReturnCode CLexemeManaged::eGetFirstIrregularForm(String^ sHash, CWordFormManaged^% wf, bool% bIsOptional)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    IWordForm * pWf = NULL;
    bool cppbIsOptional = false;
    ET_ReturnCode eRet = m_pLexeme->eGetFirstIrregularForm(sFromManagedString(sHash), pWf, cppbIsOptional);
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
 
int CLexemeManaged::iFormCount(String^ sHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->iFormCount(sFromManagedString(sHash));
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

EM_ReturnCode CLexemeManaged::eFormExists(String^ sGramHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_ReturnCode)m_pLexeme->eFormExists(sFromManagedString(sGramHash));
}

EM_ReturnCode CLexemeManaged::eIsFormDifficult(String^ sGramHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_ReturnCode)m_pLexeme->eIsFormDifficult(sFromManagedString(sGramHash));
}

EM_ReturnCode CLexemeManaged::eIsFormAssumed(String^ sGramHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_ReturnCode)m_pLexeme->eIsFormAssumed(sFromManagedString(sGramHash));
}

bool CLexemeManaged::bIsMultistressedCompound()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return m_pLexeme->bIsMultistressedCompound();
}
 
EM_ReturnCode CLexemeManaged::eGetSourceFormWithStress(String^% sSourceForm, bool bIsVariant)
{
    if (NULL == m_pLexeme)
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
 
EM_ReturnCode CLexemeManaged::eGetFirstSecondaryStemStressPos(int% iPos)
{
    if (NULL == m_pLexeme)
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
    if (NULL == m_pLexeme)
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

EM_ReturnCode CLexemeManaged::eCheckLexemeProperties() // for manual input/editing
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_ReturnCode)m_pLexeme->eCheckLexemeProperties();
}

EM_ReturnCode CLexemeManaged::eDeleteIrregularForm(String^ sFormHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_ReturnCode)m_pLexeme->eDeleteIrregularForm(sFromManagedString(sFormHash));
}

EM_ReturnCode CLexemeManaged::eSaveIrregularForms(String^ sGramHash)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    return (EM_ReturnCode)m_pLexeme->eSaveIrregularForms(sFromManagedString(sGramHash));
}

EM_ReturnCode CLexemeManaged::eMakeGraphicStem()
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    auto eRet = m_pLexeme->eMakeGraphicStem();

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eMakeGraphicStem(const String^ sSource, String^% sGraphicStem)
{
    if (NULL == m_pLexeme)
    {
        throw gcnew Exception(L"Lexeme object is NULL.");
    }

    CEString sGs;
    auto eRet = m_pLexeme->eMakeGraphicStem(sFromManagedString(sSource), sGs);
    if (eRet != H_NO_ERROR)
    {
        return (EM_ReturnCode)eRet;
    }

    sGraphicStem = gcnew String(sGs);

    return (EM_ReturnCode)eRet;
}

EM_ReturnCode CLexemeManaged::eGetErrorMsg(String^% sErrorMsg)
{
    if (NULL == m_pLexeme)
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

    m_pHasher->m_eReflexivity = (ET_Reflexivity)eRefl;
}

void CGramHasherManaged::SetAspect(EM_Aspect eAspect)
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    m_pHasher->m_eAspect = (ET_Aspect)eAspect;
}

String^ CGramHasherManaged::sHash()
{
    if (NULL == m_pHasher)
    {
        throw gcnew Exception(L"Gram hasher object is NULL.");
    }

    return gcnew String(m_pHasher->sGramHash());
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

CLexemeEnumeratorManaged::CLexemeEnumeratorManaged(ILexemeEnumerator* pLe) : m_pLexemeEnumerator(pLe)
{}

CLexemeEnumeratorManaged::~CLexemeEnumeratorManaged()
{
    if (NULL == m_pLexemeEnumerator)
    {
        throw gcnew Exception(L"Lexeme enumerator object is NULL.");
    }    
    delete m_pLexemeEnumerator;
}

EM_ReturnCode CLexemeEnumeratorManaged::eReset()
{
    if (NULL == m_pLexemeEnumerator)
    {
        throw gcnew Exception(L"Lexeme enumerator object is NULL.");
    }
    return (EM_ReturnCode) m_pLexemeEnumerator->eReset();
}

EM_ReturnCode CLexemeEnumeratorManaged::eGetFirstLexeme(CLexemeManaged^% pLexemeItf)
{
    if (NULL == m_pLexemeEnumerator)
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
    if (NULL == m_pLexemeEnumerator)
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

CParserManaged::CParserManaged(IParser * pParser) : m_pParser(pParser)
{}

CParserManaged::~CParserManaged()
{
    delete m_pParser;

//    this->!CParserManaged();
}

EM_ReturnCode CParserManaged::eParseWord(String^ sForm)
{
    if (NULL == m_pParser)
    {
        throw gcnew Exception(L"Parser object is NULL.");
    }

    return (EM_ReturnCode)m_pParser->eParseWord(sFromManagedString(sForm));
}

EM_ReturnCode CParserManaged::eGetFirstWordForm(CWordFormManaged^% pManagedWordFrom)
{
    if (NULL == m_pParser)
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
    if (NULL == m_pParser)
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

EM_ReturnCode CAnalyticsManaged::eParseText(String^ sName, String^ sMetaData, String^ sText, long long% lParsedTextId)
{
    if (NULL == m_pAnalytics)
    {
        throw gcnew Exception(L"Analytics object is NULL.");
    }

    long long llId = 0;
    auto eRet = (EM_ReturnCode)m_pAnalytics->eParseText(sFromManagedString(sName), sFromManagedString(sMetaData), sFromManagedString(sText), llId);
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
//    if (NULL == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return (EM_ReturnCode)m_pVerifier->eSetHeadword(sFromManagedString(sHeadword));
//}
//
//String^ CVerifierManaged::sGetHeadword()
//{
//    if (NULL == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return gcnew String(m_pVerifier->sGetHeadword());
//}
//
//void CVerifierManaged::SetLexemeHash(String^ sHash)
//{
//    if (NULL == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return m_pVerifier->SetLexemeHash(sFromManagedString(sHash));
//}
//
//String^ CVerifierManaged::sGetLexemeHash()
//{
//    if (NULL == m_pVerifier)
//    {
//        throw gcnew Exception(L"Verifier object is NULL.");
//    }
//
//    return gcnew String(m_pVerifier->sGetLexemeHash());
//}

EM_ReturnCode CVerifierManaged::eVerify(String^ sLexemeHash)
{
    if (NULL == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_ReturnCode)m_pVerifier->eVerify(sFromManagedString(sLexemeHash));
}

EM_TestResult CVerifierManaged::eResult()
{
    if (NULL == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_TestResult)m_pVerifier->eResult();
}

int CVerifierManaged::iCount()
{
    if (NULL == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return m_pVerifier->iCount();
}

EM_ReturnCode CVerifierManaged::eLoadStoredLexemes()
{
    if (NULL == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_ReturnCode)m_pVerifier->eLoadStoredLexemes();
}

EM_ReturnCode CVerifierManaged::eDeleteStoredLexeme(String^ sLexeme)
{
    if (NULL == m_pVerifier)
    {
        throw gcnew Exception(L"Verifier object is NULL.");
    }

    return (EM_ReturnCode)m_pVerifier->eDeleteStoredLexeme(sFromManagedString(sLexeme));
}

EM_ReturnCode CVerifierManaged::eGetFirstLexemeData(String^% sHash, String^% sHeadword)
{
    if (NULL == m_pVerifier)
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
    if (NULL == m_pVerifier)
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
