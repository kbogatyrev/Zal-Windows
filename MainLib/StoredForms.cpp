#include "StdAfx.h"
#include "StoredForms.h"
#include "WordForm.h"

HRESULT CT_StoredForms::get_LexemeId (LONG * pl_id)
{
    *pl_id = i_LexemeId;

    return S_OK;
}

HRESULT CT_StoredForms::get_Headword (BSTR * pbstr_source)
{
    CComBSTR sp_bstr (str_Headword.c_str());
    *pbstr_source = sp_bstr.Detach();

    return S_OK;
}

HRESULT CT_StoredForms::get_VariantCount (LONG l_gramHash, LONG l_at, LONG * pl_count)
{
    *pl_count = m_coll.count (l_gramHash);

    return S_OK;
}

HRESULT CT_StoredForms::get_Source (long l_at, BSTR * pbstr_source)
{
    CComBSTR sp_bstr (str_Headword.c_str());
    *pbstr_source = sp_bstr.Detach();

    return S_OK;
}

HRESULT CT_StoredForms::get_WordForm (long l_gramHash, long l_at, IWordForm ** ppVal)
{
    HRESULT h_r = S_OK;

    pair<multimap<int, CComVariant>::iterator, multimap<int, CComVariant>::iterator> pair_range;
    pair_range = m_coll.equal_range (l_gramHash);
    if (pair_range.first == pair_range.second)
    {
        ATLASSERT(0);
        wstring str_msg (L"Unable to locate word form in collection.");
        ERROR_LOG (str_msg);
        return E_FAIL;
    }

    if (distance (pair_range.first, pair_range.second) - 1 < l_at)
    {
        ATLASSERT(0);
        wstring str_msg (L"Bad form number.");
        ERROR_LOG (str_msg);
        return E_INVALIDARG;
    }

    multimap<int, CComVariant>::iterator it_wf (pair_range.first);
    advance (it_wf, l_at);
    if ((*it_wf).first != l_gramHash)
    {
        ATLASSERT(0);
        wstring str_msg (L"Error extracting map element.");
        ERROR_LOG (str_msg);
        return E_INVALIDARG;
    }

    CComQIPtr<IWordForm> sp_wf = (*it_wf).second.pdispVal;
    if (NULL == sp_wf)
    {
        ATLASSERT(0);
        wstring str_msg (L"QI for IWordForm failed.");
        ERROR_LOG (str_msg);
        return E_POINTER;
    }

    *ppVal = sp_wf.Detach();

// TODO stress

    return E_NOTIMPL;
}
