#pragma once
#include "resource.h"       // main symbols
#include "MainLib_i.h"

#include "VCUE_Collection.h"
#include "VCUE_Copy.h"

using namespace std;
using namespace std::tr1;

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif


// *** This was taken from ATLCollections Sample
// I'm not using some of the typedefs listed below 
// as they only make this convoluted code even more obscure.
// However, the MSFT's typedef names appear in comments. 
//                                                  -- kb

// We always need to provide the following information
//typedef std::vector<CComVariant> ContainerType;
//typedef VARIANT	ExposedType;
//typedef IEnumVARIANT EnumeratorInterface;

// Typically the copy policy can be calculated from the typedefs defined above:
//typedef VCUE::GenericCopy<ExposedType, ContainerType::value_type> CopyType;
// However, we may want to use a different class, as in this case:
//typedef VCUE::MapCopy<ContainerType, ExposedType>				CopyType;
	// (The advantage of MapCopy is that we don't need to provide implementations 
	//  of GenericCopy for all the different pairs of key and value types)

// Now we have all the information we need to fill in the template arguments on the implementation classes

namespace StoredDataContainer
{
    //
    // Map enumerator
    //
    typedef CComEnumOnSTL <IEnumVARIANT,                                             // enumerator interface
                           &__uuidof(IEnumVARIANT), 
                           VARIANT,                                                  // exposed type
                           VCUE::MapCopy<std::multimap<int, CComVariant>, VARIANT>,  // copy type
                           std::multimap<int, CComVariant> > MapEnumeratorType;      // container type

    //
    // Map-based container
    //
    typedef VCUE::ICollectionOnSTLCopyImpl <IStoredForms, 
                                            std::multimap<int, CComVariant>, 
                                            VARIANT,
                                            VCUE::MapCopy<std::multimap<int, CComVariant>, VARIANT>,
                                            MapEnumeratorType> MapCollection;

}   // namespace StoredDataContainer

//using namespace StoredDataContainer;

class ATL_NO_VTABLE CT_StoredForms :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CT_StoredForms, &CLSID_ZalStoredForms>,
    public StoredDataContainer::MapCollection,
    public IError
{
friend class CT_Dictionary;

public:
	CT_StoredForms()
	{
	}

    DECLARE_NO_REGISTRY()

BEGIN_COM_MAP(CT_StoredForms)
	COM_INTERFACE_ENTRY(IStoredForms)
    COM_INTERFACE_ENTRY(IError)
END_COM_MAP()

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
//        v_Init();
		return S_OK;
	}

	void FinalRelease()
    {}
//        delete pco_Db;
//	}

public:

// IError
    STDMETHOD (get_LastError) (BSTR * pbstr_description)
    {
        USES_CONVERSION;

        CT_Error * pco_error = CT_Error::pco_CreateInstance();
        if (!pco_error)
        {
            return E_POINTER;
        }

        CComBSTR sp_error (pco_error->str_GetLastError().c_str());
        *pbstr_description = sp_error.Detach();

        return S_OK;
    }

// IStoredForms
    STDMETHOD (get_LexemeId) (LONG * pl_id);
    STDMETHOD (get_Headword) (BSTR * bstr_source);
//    STDMETHOD (GetForms) ();
    STDMETHOD (get_VariantCount) (LONG l_gramHash, LONG l_at, LONG * pl_count);
    STDMETHOD (get_Source) (long l_at, BSTR * bstr_source);
    STDMETHOD (get_WordForm) (long l_gramHash, long l_at, IWordForm ** pp_wf);

private:
    int i_LexemeId;
    wstring str_Headword;
    wstring str_DbPath;
    CT_Sqlite * pco_Db;

};

OBJECT_ENTRY_AUTO(__uuidof(ZalStoredForms), CT_StoredForms)
