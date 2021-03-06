#include "Lexeme.h"
#include "WordForm.h"
#include "Analyzer_Helpers.h"

using namespace std;
using namespace std::tr1;

#pragma once
#include "resource.h"       // main symbols
//#include "MainLib_i.h"

#include "VCUE_Collection.h"
#include "VCUE_Copy.h"

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

typedef CComEnumOnSTL <IEnumVARIANT, 
                       &__uuidof(IEnumVARIANT), 
                       VARIANT,
                       VCUE::GenericCopy<VARIANT, 
                                         std::vector<CComVariant>::value_type>,
                       std::vector<CComVariant> > EnumeratorType;

typedef VCUE::ICollectionOnSTLCopyImpl <ILexPreprocessor, 
                                        std::vector<CComVariant>, 
                                        VARIANT,
                                        VCUE::GenericCopy<VARIANT, 
                                                          std::vector<CComVariant>::value_type>,
                                        EnumeratorType> WordFormCollection;

class ATL_NO_VTABLE CT_LexPreprocessor :
    public CComObjectRootEx<CComSingleThreadModel>,
    public CComCoClass<CT_LexPreprocessor, &CLSID_ZalLexPreprocessor>,
    public IDispatchImpl<WordFormCollection, &IID_ILexPreprocessor, &LIBID_MainLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
    CT_LexPreprocessor()
    {
        vInit();
    }

    DECLARE_REGISTRY_RESOURCEID(IDR_LEXPREPROCESSOR)
    //DECLARE_NO_REGISTRY()


    BEGIN_COM_MAP(CT_LexPreprocessor)
        COM_INTERFACE_ENTRY(ILexPreprocessor)
//        COM_INTERFACE_ENTRY(IError)
        COM_INTERFACE_ENTRY(IDispatch)
    END_COM_MAP()

    DECLARE_PROTECT_FINAL_CONSTRUCT()

    ET_ReturnCode FinalConstruct()
    {
        m_pDb = NULL;
        sp_dict = NULL;
        return H_NO_ERROR;
    }

    void FinalRelease()
    {
        delete m_pDb;
    }

public:

//  ILexPreprocessor
    STDMETHOD (LoadDb) (BSTR bstrPath);
    STDMETHOD (PrepareLexeme) (__int64 llLexemeId, BOOL bStress);
    STDMETHOD (PrepareLexemes) (__int64 llFirstLexemeId, __int64 llLastLexemeId, BOOL bStress);
    STDMETHOD (PrepareDictionary) ();

    CEString sDbPath;

private:
    CSqlite* m_pDb;
    CComPtr<IDictionary> sp_dict;
    map<CEString, ET_MainSymbol> map_MainSymbol;
    vector<CEndingsTable> vecDb;
    int iDbSize;
    
    void vInit();
    int iClassifyStems();
    CEString sInsertStress(int iLetter, CEString s_);
    int iDeleteStress(CEString& s_);
    void vDeleteRepeats(vector<CEString>& vecStrings);
    int iLCP(CEString* arr_sWords, CEString** sPfx, int iWords, int iPfx);

    // Helper functions for common DB queries
    // (Stored in LexPreprocessorQueries.cpp)
    void vLongStemsBySubtable(int iSubtable, int iMinLen, vector<CEString>& vecStems, CEString& sFirstLemma);
    void vInsertCommonSfx(CEString **parr_str_sfx, int iSfx, int iSubtable, int iLexemeCount, int iCutRight, CEString sLemmaEnding);
    void vInsertStemsAndLinks(CEString **parr_str_stems, CEString sLemma, int *arr_iSubtableId, int iStems, __int64 llLexemeId);
};

OBJECT_ENTRY_AUTO(__uuidof(ZalLexPreprocessor), CT_LexPreprocessor)
