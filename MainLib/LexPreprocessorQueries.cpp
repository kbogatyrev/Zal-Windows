#include "stdafx.h"
#include "LexPreprocessor.h"
#include "Dictionary.h"
#include "Lexeme.h"

void CT_LexPreprocessor::vLongStemsBySubtable(int iSubtable,
                                              int iMinLen,
                                              vector<CEString>& vecStems,
                                              CEString& sFirstLemma)
{
    CEString sQuery = L"Select * from stems_for_analysis where endings_link = " + 
        CEString::sToString<int>(iSubtable);
    CEString sStem;
    sFirstLemma = L"";

    m_pDb->PrepareForSelect(sQuery);
    while (m_pDb->bGetRow())
    {
        m_pDb->GetData(2, sStem);
        if (sStem.uiLength() > iMinLen)
        {
            vecStems.push_back(sStem);
            if (sFirstLemma.uiLength() <= 0)
            {
                m_pDb->GetData(5, sFirstLemma);
            }
        }
    }
    m_pDb->Finalize();
}

void CT_LexPreprocessor::vInsertCommonSfx(CEString **parr_sSfx,
                                          int iSfx,
                                          int iSubtable,
                                          int iLexemeCount,
                                          int iCutRight,
                                          CEString sLemmaEnding)
{
    CEString sSfx = L"";
    for (int iS = 0; iS < iSfx; ++iS)
    {
        CEString sCurSfx = (*parr_sSfx)[iS];
//        reverse(str_cur_sfx.begin(), str_cur_sfx.end());
        sCurSfx.Reverse();
        sSfx += sCurSfx;
        if (iS < iSfx - 1)
        {
            sSfx += L"|";
        }
    }

    m_pDb->PrepareForInsert(L"subtables_for_heuristics", 5);
    m_pDb->Bind(1, iSubtable);
    m_pDb->Bind(2, iLexemeCount);
    m_pDb->Bind(3, sSfx);
    m_pDb->Bind(4, iCutRight);
    m_pDb->Bind(5, sLemmaEnding);
    m_pDb->InsertRow();
    m_pDb->Finalize();
}

void CT_LexPreprocessor::vInsertStemsAndLinks(CEString **parr_sStems,
                                              CEString sLemma,
                                              int *arr_iSubtableId,
                                              int iStems,
                                              __int64 llLexemeId)
{
    for (int iS = 0; iS < iStems; ++iS)
    {
        CEString sStem = (*parr_sStems)[iS];
        int iStressPos = iDeleteStress(sStem);
        if (iStressPos >= sStem.uiLength())
        {
            // This in fact means that the ending is stressed
            iStressPos = -1;
        }
        m_pDb->PrepareForInsert(L"stems_for_analysis", 5);
        m_pDb->Bind(1, llLexemeId);
        m_pDb->Bind(2, sStem);
        m_pDb->Bind(3, iStressPos);
        m_pDb->Bind(4, arr_iSubtableId[iS]);
        m_pDb->Bind(5, sLemma);
        m_pDb->InsertRow();
        m_pDb->Finalize();
    }
}