#include "stdafx.h"
#include "Analyzer_Helpers.h"

bool operator< (stInflexion& soInfl1, stInflexion& soInfl2)
{
    if (soInfl1.sEnding < soInfl2.sEnding)
    {
        return true;
    }
    if ((soInfl1.sEnding == soInfl2.sEnding) && (soInfl1.iStress < soInfl2.iStress))
    {
        return true;
    }
    if ((soInfl1.sEnding == soInfl2.sEnding) && (soInfl1.iStress == soInfl2.iStress) && (soInfl1.iGram < soInfl2.iGram))
    {
        return true;
    }
    return false;
}

bool operator> (stInflexion& soInfl1, stInflexion& soInfl2)
{
    if (soInfl1.sEnding > soInfl2.sEnding)
    {
        return true;
    }
    if ((soInfl1.sEnding == soInfl2.sEnding) && (soInfl1.iStress > soInfl2.iStress))
    {
        return true;
    }
    if ((soInfl1.sEnding == soInfl2.sEnding) && (soInfl1.iStress == soInfl2.iStress) && (soInfl1.iGram > soInfl2.iGram))
    {
        return true;
    }
    return false;
}

bool operator== (stInflexion& soInfl1, stInflexion& soInfl2)
{
    if ((soInfl1.sEnding == soInfl2.sEnding) && (soInfl1.iStress == soInfl2.iStress) && (soInfl1.iGram == soInfl2.iGram))
    {
        return true;
    }
    return false;
}

CEndingsTable::CEndingsTable()
{
    m_sStemFinal = m_sLemmaFinal = L"";
    m_iCutRight = m_iLexemeCount = 0;
    bSorted = true;
}

CEndingsTable::CEndingsTable(const CEndingsTable& Source) : 
    m_iLexemeCount (Source.m_iLexemeCount),
    m_iCutRight (Source.m_iCutRight),
    m_sStemFinal (Source.m_sStemFinal),
    m_sLemmaFinal (Source.m_sLemmaFinal),
    m_vecEndings (Source.m_vecEndings),
    bSorted (Source.bSorted)
{
}


CEndingsTable::~CEndingsTable()
{
    Clear();
}

void CEndingsTable::Clear()
{
    m_vecEndings.clear();
    m_sStemFinal = m_sLemmaFinal = L"";
    m_iCutRight = m_iLexemeCount = 0;
    bSorted = true;
}

int CEndingsTable::iAddEnding(int iHash, CEString sEnding, int iStressPos)
{
    stInflexion soNewEnding = {sEnding, iHash, iStressPos};
    for (int i = 0; i < m_vecEndings.size(); ++i)
    {
        if (m_vecEndings[i] == soNewEnding)
        {
            return 0;      // We don't want to write several identical endings
        }
    }
    bSorted = false;
    if (bSorted && m_vecEndings.size() > 0)
    {
        if (m_vecEndings[m_vecEndings.size() - 1] < soNewEnding)
        {
            bSorted = true;
        }
    }
    else if (m_vecEndings.size() <= 0)
    {
        bSorted = true;
    }
    m_vecEndings.push_back(soNewEnding);
    return 0;
}

int CEndingsTable::iAddEnding(long lHash, CEString sEnding, int iStressPos)
{
    return iAddEnding((int)lHash, sEnding, iStressPos);
}

void CEndingsTable::vSort()
{
    // heapsort, adapted version of http://www.codecodex.com/wiki/Heapsort
    if (bSorted)
    {
        return;
    }

    unsigned int uiN = m_vecEndings.size(), i = uiN / 2, uiParent, uiChild;
    stInflexion soTmp;

    for (;;)    /* Loops until arr is sorted */
    {
        if (i > 0)  /* First stage - Sorting the heap */
        {
            --i;                        /* Save its index to i */
            soTmp = m_vecEndings[i];    /* Save parent value to t */
        }
        else        /* Second stage - Extracting elements in-place */
        {
            --uiN;                          /* Make the new heap smaller */
            if (uiN == 0) return;           /* When the heap is empty, we are done */
            soTmp = m_vecEndings[uiN];      /* Save last value (it will be overwritten) */
            m_vecEndings[uiN] = m_vecEndings[0]; /* Save largest value at the end of arr */
        }
  
        uiParent = i;           /* We will start pushing down t from parent */
        uiChild = i * 2 + 1;    /* parent's left child */
  
        /* Sift operation - pushing the value of t down the heap */
        while (uiChild < uiN)
        {  
            if (uiChild + 1 < uiN && m_vecEndings[uiChild + 1] > m_vecEndings[uiChild])
            {  
                ++uiChild; /* Choose the largest child */
            }  
            if (m_vecEndings[uiChild] > soTmp)  /* If any child is bigger than the parent */
            {
                m_vecEndings[uiParent] = m_vecEndings[uiChild]; /* Move the largest child up */
                uiParent = uiChild;                             /* Move parent pointer to this child */
                uiChild = uiParent * 2 + 1;                     /* the previous line is wrong */
            }
            else
            {
                break; /* t's place is found */
            }
        }
        m_vecEndings[uiParent] = soTmp; /* We save t in the heap */
    }
    bSorted = true;
}

vector<int> CEndingsTable::vecDbId(CSqlite* pco_dbHandle)
// Tries to find the table in the db, returns -1 if there's no such table.
// If everything goes well, the output vector will contain just 1 element. But who knows what can happen.
{
    vector<int> vecSubtableId, vecEntriesCount, vecResult;
    if (pco_dbHandle == NULL || m_vecEndings.empty())
    {
        return vecResult;
    }
    
    CEString sQuery, sCount, sGram, sStress;
    int iEntriesCount, iSubtableId;

    vector<stInflexion>::iterator iterEndings = m_vecEndings.begin();
    sGram = CEString::sToString(iterEndings->iGram);
    sStress = CEString::sToString(iterEndings->iStress);
    sQuery += L"Select * from endings_for_analysis as a0 where ending = \"" 
        + iterEndings->sEnding + L"\" and gram_hash = " + sGram 
        + L" and stress_pos = " + sStress;
    ++iterEndings;
    if (m_vecEndings.size() > 1)
    {
        for (int i_ = 1;
             iterEndings != m_vecEndings.end();
             ++iterEndings, ++i_)
        {
            sCount = CEString::sToString(i_);
            sGram = CEString::sToString(iterEndings->iGram);
            sStress = CEString::sToString(iterEndings->iStress);
            sQuery += L" and exists (select * from endings_for_analysis as a" 
                      + sCount + L" where ending = \"" + iterEndings->sEnding 
                      + L"\" and gram_hash = " + sGram 
                      + L" and stress_pos = " + sStress
                      + L" and a" + sCount + L".subtable_id = a0.subtable_id)";
        }
    }
    pco_dbHandle->PrepareForSelect(sQuery);
    while (pco_dbHandle->bGetRow())
    {
        pco_dbHandle->GetData(1, iSubtableId);
        vecSubtableId.push_back(iSubtableId);
    }
    pco_dbHandle->Finalize();

    // Now that we've learned the ids of possible subtables we must ensure they 
    // don't contain any entries except those specified by the input vectors.
    for (vector<int>::iterator iterSubtableId = vecSubtableId.begin();
         iterSubtableId != vecSubtableId.end();
         ++iterSubtableId)
    {
        sQuery = L"Select entries_count from endings_meta where subtable_id = " 
                 + CEString::sToString(*iterSubtableId);
        pco_dbHandle->PrepareForSelect(sQuery);
        while (pco_dbHandle->bGetRow())
        {
            pco_dbHandle->GetData(0, iEntriesCount);
            vecEntriesCount.push_back(iEntriesCount);
        }
        pco_dbHandle->Finalize();
        if (!vecEntriesCount.empty() && *(vecEntriesCount.begin()) == m_vecEndings.size())
        {
            vecResult.push_back(*iterSubtableId);
            break;
        }
        vecEntriesCount.clear();
    }
    return vecResult;
}

int CEndingsTable::iDbId(vector<CEndingsTable>& vecDb)
// Tries to find the table in the list of ending tables, returns -1 if there's no such table.
// (Returns only the first coincidence.)
{
    for (int iTable = 0; iTable < vecDb.size(); ++iTable)
    {
        if (vecDb[iTable].m_vecEndings.size() != m_vecEndings.size())
        {
            continue;
        }
        vecDb[iTable].vSort();
        this->vSort();
        bool bCoincidence = true;
        vector<stInflexion>::iterator itThisEnding = this->m_vecEndings.begin();
        vector<stInflexion>::iterator itThatEnding = vecDb[iTable].m_vecEndings.begin();
        for (; itThisEnding != m_vecEndings.end(); ++itThisEnding, ++itThatEnding)
        {
            if (!(*itThisEnding == *itThatEnding))
            {
                bCoincidence = false;
                break;
            }
        }
        if (bCoincidence)
        {
            return iTable;
        }
    }

    return -1;
}

int CEndingsTable::iDbWrite(CSqlite* pco_dbHandle, bool bSearchFirst, int iSubtableId)
// If a table already exists (and bSearchFirst == true), return its subtable_id.
// If not, write the whole table to the db, then return its subtable_id.
// Return -1 or -2 on error.
{
    if (pco_dbHandle == NULL)
    {
        return -1;
    }
    int iInserted = 0;
    if (bSearchFirst)
    {
        vector<int> vecSearchResult;
        vecSearchResult = vecDbId(pco_dbHandle);
        if (!vecSearchResult.empty())
        {
            return *(vecSearchResult.begin());
            // TODO: What if it contains more than one element?
        }
    }

    // If we've reached this mark, it means that there's no such table in the db yet.
    // We should get the number of the last table in the db and save our table there.
    vector<stInflexion>::iterator iterEnding;
    if (iSubtableId < 0)
    {
        iSubtableId = pco_dbHandle->iLastID(L"endings_meta") + 1;
    }

    for (iterEnding = m_vecEndings.begin();
         iterEnding != m_vecEndings.end();
         ++iterEnding, ++iInserted)
    {
        // For each tuple <ending, grammatical parameters, stress position>,
        // insert it into the endings table.
        pco_dbHandle->PrepareForInsert(L"endings_for_analysis", 4);
        pco_dbHandle->Bind(1, iSubtableId);  // 0-based
        pco_dbHandle->Bind(2, iterEnding->sEnding);
        pco_dbHandle->Bind(3, iterEnding->iGram);
        pco_dbHandle->Bind(4, iterEnding->iStress);
        pco_dbHandle->InsertRow();
        pco_dbHandle->Finalize();
    }
    // Now, write auxiliary information to endings_meta
    pco_dbHandle->PrepareForInsert(L"endings_meta", 2);
    pco_dbHandle->Bind(1, iSubtableId);
    pco_dbHandle->Bind(2, iInserted);
    pco_dbHandle->InsertRow();
    pco_dbHandle->Finalize();
    return iSubtableId;
}

int CEndingsTable::iDbWrite(vector<CEndingsTable>& vecDb, bool bSearchFirst)
// If a table already exists in the list (and bSearchFirst == true), return its subtable_id.
// If not, write the whole table to the list, then return its subtable_id.
// Return -1 or -2 on error.
{
    int iInserted = 0;
    if (bSearchFirst)
    {
        int iSearchResult;
        iSearchResult = iDbId(vecDb);
        if (iSearchResult >= 0)
        {
            return iSearchResult;
        }
    }

    // If we've reached this mark, it means that there's no such table in the list yet.
    CEndingsTable coCopy(*this);
    vecDb.push_back(coCopy);

    return vecDb.size() - 1;
}

vector<int> CEndingsTable::m_vecFind(CEString sEnding, int iStressPos)
{
    vector<int> vecResult;
    if (m_vecEndings.empty())
    {
        return vecResult;
    }

    /*
    vector<stInflexion>::iterator iterEndings;
    for (iterEndings = m_vecEndings.begin();
         iterEndings != m_vecEndings.end();
         ++iterEndings)
    {
        if ((iStressPos < -1 || (iStressPos >= -1 && iterEndings->iStress == iStressPos)) &&
            iterEndings->sEnding == sEnding)
        {
            vecResult.push_back(iterEndings->iGram);
        }
    }
    */

    // Binary search in the sorted vector
    if (!bSorted)
    {
        vSort();
    }
    int iLeft = 0, iRight = m_vecEndings.size() - 1, iMiddle = 0;
    while (iLeft < iRight)
    {
        iMiddle = (iLeft + iRight) / 2;
        if (iMiddle == iLeft || iMiddle == iRight)
        {
            break;
        }
        if (m_vecEndings[iMiddle].sEnding < sEnding ||
            (iStressPos >= -1 && m_vecEndings[iMiddle].sEnding == sEnding && m_vecEndings[iMiddle].iStress < iStressPos))
        {
            iLeft = iMiddle;
        }
        else
        {
            iRight = iMiddle;
        }
    }
    if (m_vecEndings[iLeft].sEnding == sEnding &&
        (iStressPos < -1 || m_vecEndings[iLeft].iStress == iStressPos))
    {
        iRight = iLeft;
    }
    for (int iIter = iRight; iIter < m_vecEndings.size(); ++iIter)
    {
        if (m_vecEndings[iIter].sEnding != sEnding ||
            (iStressPos >= -1 && m_vecEndings[iIter].iStress != iStressPos))
        {
            break;
        }
        vecResult.push_back(m_vecEndings[iIter].iGram);
    }
    return vecResult;
}

