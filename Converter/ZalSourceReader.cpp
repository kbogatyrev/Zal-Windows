/*

*/

#include "stdafx.h"
#include "SqliteWrapper.h"
#include "ZalEntryParser.h"
#include "EndingsParser.h"
#include "LexemeHasher.h"
#include "ZalSourceReader.h"

using namespace Hlib;

HRESULT CZalSourceReader::FinalConstruct()
{
    CoInitialize (NULL);

    m_pInFile = NULL;
    m_pOutFile = NULL;
    m_pDb = NULL;
    m_iMaxEntries = 0;

    return H_NO_ERROR;
}

void CZalSourceReader::FinalRelease()
{
    // TODO: check for and report errors
    CoUninitialize();
}

HRESULT CZalSourceReader::ConvertSourceFile (BSTR bstrSourcePath, BSTR bstrDbPath, BSTR bstrUnprocessedPath, int iMaxEntries, BOOL bEndings)
{
    USES_CONVERSION;

    HRESULT hr = H_NO_ERROR;

    StatusUpdate (0);

    bShow (L"### Conversion started...");

    int iDebugFlag =  _CrtSetDbgFlag( _CRTDBG_REPORT_FLAG );
    iDebugFlag |= _CRTDBG_LEAK_CHECK_DF;
    _CrtSetDbgFlag( iDebugFlag );

    m_iMaxEntries = iMaxEntries;

    errno_t iError = _tfopen_s (&m_pInFile, OLE2W (bstrSourcePath), L"r, ccs=UNICODE");
    if (0 != iError)
    {
        CString csMsg;
        csMsg.Format (L"Unable to open source, error %d", iError);
        ERROR_LOG ((LPCTSTR)csMsg);
        return E_FAIL;
    }

    m_pDb = new CSqlite;
    try
    {
        m_pDb->Open (OLE2W (bstrDbPath));
    }
    catch (CException& ex)
    {
        CEString sMsg (ex.szGetDescription());
        CEString sError;
        try
        {
            m_pDb->GetLastError (sError);
            sMsg += L", error %d: ";
            sMsg += sError;
        }
        catch (...)
        {
            sMsg = L"Apparent DB error ";
        }
    
        CString csMsg;
        csMsg.Format (sMsg, m_pDb->iGetLastError());
        ERROR_LOG ((LPCTSTR)csMsg);

        return MAKE_HRESULT (SEVERITY_ERROR, FACILITY_ITF, 0xF000 + m_pDb->iGetLastError());
    }

    iError = _tfopen_s (&m_pOutFile, OLE2W (bstrUnprocessedPath), L"w, ccs=UNICODE");
    if (0 != iError)
    {
        CString csMsg;
        csMsg.Format (L"Unable to open unprocessed entries file, error %d", iError);
        ERROR_LOG ((LPCTSTR)csMsg);
        return E_FAIL;
    }

    if (bEndings)
    {
        return hConvertEndings();
    }
    else
    {
        return hConvertStems();
    }

}   //  ConvertSourceFile(...)

HRESULT CZalSourceReader::hConvertEndings()
{
    USES_CONVERSION;

    HRESULT hr = H_NO_ERROR;

    StatusUpdate(0);

    bShow (L"### Conversion started...");

    int iDebugFlag =  _CrtSetDbgFlag(_CRTDBG_REPORT_FLAG);
    iDebugFlag |= _CRTDBG_LEAK_CHECK_DF;
    _CrtSetDbgFlag(iDebugFlag);

    long lFileLength = _filelength (_fileno (m_pInFile))/sizeof (TCHAR);
    long lCharsRead = 0;
    int iPercentDone = 0;
    int iEntriesRead = 0;

    int iError = 0;

    m_pDb->BeginTransaction();

    TCHAR szLineBuf[10000];
    BOOL uiAbort = false;

    for (; (!feof (m_pInFile) && !uiAbort); ++iEntriesRead)
    {
        if (iEntriesRead == m_iMaxEntries)
        {
            break;
        }

        TCHAR * szRet = _fgetts (szLineBuf, 10000, m_pInFile);
        if (NULL == szRet)
        {
            iError = ferror (m_pInFile);
            if (0 != iError)
            {
                break;
            }
            else
            {
                continue;
            }
        }

        CEString sLine (szLineBuf);
        lCharsRead += sLine.uiLength();

        if (L';' == sLine[0])
        {
            continue;
        }

        unsigned int uiCommentStart = sLine.uiFind(L";");
        if (uiCommentStart != ecNotFound)
        {
            sLine.sErase(uiCommentStart);
        }

        sLine.SetBreakChars (L" \n");
        if (sLine.uiGetNumOfFields() < 1)
        {
            continue;
        }

        sLine.Trim();

        //if ((iEntriesRead % 10000) == 0 && iEntriesRead > 0)
        //{
        //    Sqlite.CommitTransaction();
        //    Sqlite.BeginTransaction();
        //}

        if ((iEntriesRead % 10) == 0)
        {
            CEString sWord (sLine);
            sWord.SetBreakChars (L" ");
            CString csWord (sWord.sGetField (0));
            ShowCurrentWord (csWord.AllocSysString());
        }

        CEndingsParser Entry (sLine, m_pDb);
        bool bParseResult = Entry.bParse();
        int iPd = (int) (((double)lCharsRead/(double)lFileLength) * 100);
        if (iPd > iPercentDone)
        {
            iPercentDone = min (iPd, 100);
            StatusUpdate (iPercentDone);
        }
    }

    m_pDb->CommitTransaction();

    StatusUpdate (100);

    return H_NO_ERROR;

}   //  hConvertEndings(...)

HRESULT CZalSourceReader::hConvertStems()
{
    BOOL uiAbort = false;
    long lFileLength = _filelength (_fileno (m_pInFile))/sizeof (TCHAR);
    long lCharsRead = 0;
    int iPercentDone = 0;
    int iEntriesRead = 0;
    int iError = 0;

    TCHAR szLineBuf[10000];

    for (; (!feof (m_pInFile) && !uiAbort); ++iEntriesRead)
    {
        if (iEntriesRead == m_iMaxEntries)
        {
            break;
        }

        TCHAR * szRet = _fgetts (szLineBuf, 10000, m_pInFile);
        if (NULL == szRet)
        {
            iError = ferror (m_pInFile);
            if (0 != iError)
            {
                break;
            }
            else
            {
                continue;
            }
        }

        CEString sLine (szLineBuf);
        lCharsRead += sLine.uiLength();

        if (L';' == sLine[0])
        {
            continue;
        }

        sLine.SetBreakChars (L" \n");
        if (sLine.uiGetNumOfFields() < 1)
        {
            continue;
        }

        sLine.Trim();

        //if ((iEntriesRead % 10000) == 0 && iEntriesRead > 0)
        //{
        //    Sqlite.CommitTransaction();
        //    Sqlite.BeginTransaction();
        //}

        if ((iEntriesRead % 10) == 0)
        {
            CEString sWord (sLine);
            sWord.SetBreakChars (L" ");
            CString csWord (sWord.sGetField (0));
            ShowCurrentWord (csWord.AllocSysString());
        }

        CZalEntryParser Entry (sLine, m_mmapDictionary);
        bool bParseResult = Entry.bParse();
        if (!bParseResult)
        {
            wstring sMsg(L"*** Parsing error: \n");
            sMsg += sLine;
            ERROR_LOG (sMsg.c_str());
            iError = _fputts (sLine, m_pOutFile);
            if (0 != iError)
            {
                ERROR_LOG (L"*** Error writing unprocessed entry. \n");
            }

            if (m_iMaxEntries > 0)
            {
                iPercentDone = (int) (((double)iEntriesRead/(double)m_iMaxEntries) * 100);
            }
            else
            {
                int iPd = (int) (((double)lCharsRead/(double)lFileLength) * 100);
                if (iPd > iPercentDone)
                {
                    iPercentDone = min (iPd, 100);
                    StatusUpdate (iPercentDone);
                }
            }
        }

        StatusCheck (uiAbort);

    }   // for (; (!feof (ioInStream) && !uiAbort); ++iEntriesRead)

    m_pDb->BeginTransaction();

    bool bRet = bSaveToDb();
    if (!bRet)
    {
        return H_ERROR_GENERAL;
    }

    bRet = bResolveReferences();
    if (!bRet)
    {
        return H_ERROR_GENERAL;
    }

    bRet = bUpdateReferences();
    if (!bRet)
    {
        return H_ERROR_GENERAL;
    }

    m_pDb->CommitTransaction();

    if (!uiAbort)
    {
        StatusUpdate (100);
    }

    return H_NO_ERROR;

}   //  ConvertStems (...)

HRESULT CZalSourceReader::PreprocessSourceFile (BSTR bstrSourcePath, BSTR bstrOutputDir)
{
    USES_CONVERSION;

    HRESULT hr = H_NO_ERROR;

    wchar_t * arrMainSymbol[] = { L"м", L"мо", L"ж", L"жо", L"с", L"со", L"мо-жо", L"мн.",
     L"мн. неод.", L"мн. одуш.", L"мн. _от_", L"п", L"мс", L"мс-п", L"числ.",
     L"числ.-п", L"св", L"нсв", L"св-нсв", L"межд.", L"н", L"предик.", L"предл.",
     L"част.", L"союз", L"вводн.", L"сравн." };

    for (int i_ = 0; i_ < sizeof (arrMainSymbol)/sizeof (wchar_t *); ++i_)
    {
        m_setMainSymbols.insert (arrMainSymbol[i_]);
    }

    CEString sOutputDir (OLE2W (bstrOutputDir));

    StatusUpdate (0);

    bShow (L"### Pre-processing started...");

    FILE * ioInStream = NULL;
    errno_t iError = _tfopen_s (&ioInStream, OLE2W (bstrSourcePath), L"r, ccs=UNICODE");
    if (0 != iError)
    {
        CString csMsg;
        csMsg.Format (L"Unable to open file %s, error %d", CString (bstrSourcePath), iError);
        ERROR_LOG ((LPCTSTR)csMsg);
        return E_FAIL;
    }

    FILE * ioOutStream = NULL;
    CEString sPath = sOutputDir + L"\\out.txt";
    iError = _tfopen_s (&ioOutStream, sPath, L"w, ccs=UNICODE");
    if (0 != iError)
    {
        CString csMsg;
        csMsg.Format (L"Unable to open %s, error %d", sPath, iError);
        ERROR_LOG ((LPCTSTR)csMsg);
        return E_FAIL;
    }

    FILE * ioSemicolons = NULL;
    sPath = sOutputDir + L"\\semicolons.txt";
    iError = _tfopen_s (&ioSemicolons, sPath, L"w, ccs=UNICODE");
    if (0 != iError)
    {
        CString csMsg;
        csMsg.Format (L"Unable to open file %s, error %d", sPath, iError);
        ERROR_LOG ((LPCTSTR)csMsg);
        return E_FAIL;
    }

    long lFileLength = _filelength (_fileno (ioInStream))/sizeof (TCHAR);
    long lCharsRead = 0;
    int iPercentDone = 0;
    int iEntriesRead = 0;

    TCHAR szLineBuf[10000];
    BOOL uiAbort = false;
    for (; (!feof (ioInStream) && !uiAbort); ++iEntriesRead)
    {
        TCHAR * szRet = _fgetts (szLineBuf, 10000, ioInStream);
        if (NULL == szRet)
        {
            int iError = ferror (ioInStream);
            if (0 != iError)
            {
                break;
            }
        }

        CEString sLine (szLineBuf);
        lCharsRead += sLine.uiLength();

        if (L';' == sLine[0])
        {
            continue;
        }

        bool bSuspiciousEntry = false;
        bool bRet = bCheckSemicolons (sLine, bSuspiciousEntry);
        if (bSuspiciousEntry)
        {
            iError = _fputts (sLine, ioSemicolons);
            continue;
        }
    
        iError = _fputts (sLine, ioOutStream);
        int iPd = (int) (((double)lCharsRead/(double)lFileLength) * 100);
        if (iPd > iPercentDone)
        {
            iPercentDone = iPd;
        }

        StatusUpdate (iPercentDone);
        StatusCheck (uiAbort);
    }

    if (!uiAbort)
    {
        StatusUpdate (100);
    }

    return H_NO_ERROR;

}   //  PreprocessSourceFile (...)

HRESULT CZalSourceReader::SearchSourceFile (BSTR bstrSourcePath, 
                                            BSTR bstrSearchString, 
                                            BOOL bRegexSearch)
{
    USES_CONVERSION;

    StatusUpdate (0);

    bShow (L"### Search started...");

    FILE * ioInStream = NULL;
    errno_t iError = _tfopen_s (&ioInStream, OLE2W (bstrSourcePath), L"r, ccs=UNICODE");
    if (0 != iError)
    {
        CString csMsg;
        csMsg.Format (L"Unable to open file %s, error %d", CString (bstrSourcePath), iError);
        ERROR_LOG ((LPCTSTR)csMsg);
        return E_FAIL;
    }

    wchar_t szDirPath[512];
    DWORD dwRet = GetTempPath (512, szDirPath);
    if ((dwRet > 512) || (0 == dwRet))
    {
        CString csMsg;
        csMsg.Format (L"Error retrieving temp. directory path, error %d", GetLastError());
        ERROR_LOG ((LPCTSTR)csMsg);
        return E_FAIL;
    }

    wchar_t szPath[512];
    UINT uiRet = GetTempFileName (szDirPath, // dir path
                                  L"Zal_",    // temp file name prefix 
                                  0,          // create unique name 
                                  szPath);   // buffer for name 
    if (0 == uiRet)
    {
        CString csMsg;
        csMsg.Format (L"Unable to create temporary file name in %s, error %d", szDirPath, uiRet);
        ERROR_LOG ((LPCTSTR)csMsg);
        return E_FAIL;
    }

    FILE * ioOutStream = NULL;
    iError = _tfopen_s (&ioOutStream, szPath, L"w, ccs=UNICODE");
    if (0 != iError)
    {
        CString csMsg;
        csMsg.Format (L"Unable to open %s, error %d", szPath, iError);
        ERROR_LOG ((LPCTSTR)csMsg);
        return E_FAIL;
    }

    m_sTempFile = szPath;

    long lFileLength = _filelength (_fileno (ioInStream))/sizeof (TCHAR);
    long lCharsRead = 0;
    int iPercentDone = 0;
    int iEntriesRead = 0;

    CEString sHeader (L";   ");
    if (bRegexSearch)
    {
        sHeader += L"*** Regex expression = ";
    }
    else
    {
        sHeader += L"*** Match string = ";
    }
    sHeader += OLE2W (bstrSearchString);
    sHeader += L"\n\n";
    iError = _fputts (sHeader, ioOutStream);

    TCHAR szLineBuf[10000];
    BOOL uiAbort = false;
    for (; (!feof (ioInStream) && !uiAbort); ++iEntriesRead)
    {
        TCHAR * szRet = _fgetts (szLineBuf, 10000, ioInStream);
        if (NULL == szRet)
        {
            int iError = ferror (ioInStream);
            if (0 != iError)
            {
                break;
            }
        }

        CEString sLine (szLineBuf);
        lCharsRead += sLine.uiLength();

        bool bMatch = false;
        CEString sSearch = OLE2W (bstrSearchString);
        if (bRegexSearch)
        {
            CEString sLine (sLine);
            sLine.SetBreakChars (L" \n");
            if (sLine.uiGetNumOfFields() < 1)
            {
                CString csMsg (L"Empty line: ");
                csMsg += sLine;
                ERROR_LOG ((LPCTSTR)csMsg);
                continue;
            }

            sLine.Trim();

            bRegexSearch = sLine.bRegexMatch (sSearch);
        }
        else
        {
            unsigned int uiMatch = sLine.uiFind (sSearch);
            if (ecNotFound != uiMatch)
            {
                bMatch = true;
            }
        }

        if (bMatch)
        {
            iError = _fputts (sLine, ioOutStream);
            sLine = CEString (L"*** NO_LOG *** ") + sLine; // tell UI that we don't want it to be logged
            bool b_ = bShow (sLine);
        }

        int iPd = (int) (((double)lCharsRead/(double)lFileLength) * 100);
        if (iPd > iPercentDone)
        {
            iPercentDone = iPd;
        }

        StatusUpdate (iPercentDone);
        StatusCheck (uiAbort);
        if ((iEntriesRead % 10) == 0)
        {
            CEString sWord (sLine);
            sWord.SetBreakChars (L" ");
            CString csWord (sWord.sGetField (0));
            ShowCurrentWord (csWord.AllocSysString());
        }
    }

    if (!uiAbort)
    {
        StatusUpdate (100);
    }

    fclose (ioOutStream);

    return H_NO_ERROR;

}   // SearchSourceFile (...)

HRESULT CZalSourceReader::SaveOutput (BSTR bstrPath)
{
    BOOL uiRet = MoveFileEx (m_sTempFile, 
                             CString (bstrPath), 
                             MOVEFILE_REPLACE_EXISTING | MOVEFILE_COPY_ALLOWED);
    if (0 == uiRet)
    {
        CString csMsg;
        csMsg.Format (L"Unable to move temp file %s to %s, error %d", 
                      m_sTempFile, 
                      CString (bstrPath),
                      GetLastError());
        ERROR_LOG ((LPCTSTR)csMsg);
        return E_FAIL;
    }
    return H_NO_ERROR;

}   //  SaveOutput (...)

bool CZalSourceReader::bCheckSemicolons (CEString& sEntry, bool& bSuspiciousEntry)
{
    bSuspiciousEntry = false;
    CEString sCopy (sEntry);

    CEString sLine (sEntry);
    sLine.SetBreakChars (L";");
    if (sLine.uiNFields() < 2)
    {
        return true;
    }

    //bool bMatch = sLine.b_RegexMatch (L"^.+?[^\\(].+?;.+?[^\\)].*");

    // We should omit semicolons in parentheses or curly brackets.
    // Here, we take into account than parentheses can occur inside
    // curly brackets, but the converse doesn't hold.
    bool bMatch = sLine.bRegexMatch (L"^([^\\{]*\\{[^\\}]*\\})*?([^\\{\\(]*\\([^\\)]*\\))*?[^\\{\\(;]*;.+");
    if (!bMatch)
    {
        return true;
    }

//    sLine.Reset();
    sLine.SetBreakChars (L";");
    for (int iAt = 0; !bSuspiciousEntry && (iAt < (int)sLine.uiNFields()); ++iAt)
    {
        CEString sSegment (sLine.sGetField (iAt));
        //if (sSegment.i_NFields() > 1)
        //{
        //    wstring str_parenth = sSegment.str_GetToken (sSegment.i_NTokens()-2);
        //    if (L"(" == str_parenth)
        //    {
        //        continue;
        //    }
        //}

        if (iAt < (int)sLine.uiNFields()-1)
        {
            CEString sNext (sLine.sGetField (iAt+1));
            sNext.SetBreakChars (L" \n");
            sNext.Trim();
            // If it was a semicolon inside a group, go further
            int iOffset = sLine.uiGetFieldOffset (iAt+1);
            CEString sRest (sLine.sSubstr (iOffset, sLine.uiLength() - iOffset));
            sRest.SetBreakChars (L" \n");
            ERROR_LOG(L"sRest: " + sRest);
            if (sRest.bRegexMatch (L"^[^\\{]*?\\}") || sRest.bRegexMatch (L"^[^\\(]*?\\)"))
            {
                continue;
            }

            if ((_istdigit (sNext[0])) || (m_setMainSymbols.count (sNext.sGetField (0)) > 0))
            {
//                StToken stT;
//                bool b_ = sLine.bGetField (iAt+1, ec_TokenText, st_t);
                StToken stT = sLine.stGetField (iAt+1, ecTokenText);
                //if (!b_)
                //{
                //    ERROR_LOG (L"GetField() returned error");
                //    return false;
                //}
                int iSemicolonPos = stT.uiOffset - 1;
                if (L';' != sLine[iSemicolonPos])
                {
                    ERROR_LOG (L"Semicolon expected");
                    return false;
                }
                if (stT.uiOffset < 2)
                {
                    ERROR_LOG (L"Bad offset");
                    return false;
                }
                sCopy[iSemicolonPos] = L'|';
            }
            else
            {
                bSuspiciousEntry = true;
            }
        }
    }   //  for (int iAt = 0; iAt < sLine.i_NFields(); ++iAt)

    sEntry = sCopy;

    return true;

}   //  bCheckSemicolons (...)

HRESULT CZalSourceReader::StatusUpdate (int iProgress)
{
    HRESULT hr = H_NO_ERROR;

    int iNConnections = m_vec.GetSize();
    for (int iC = 0; iC < iNConnections; ++iC)
    {
        Lock();
        CComPtr<IUnknown> spUnkConnection = m_vec.GetAt (iC);
        CComQIPtr<IZalNotification2> spQiConnection = spUnkConnection;
        Unlock();
        if (spQiConnection)
        {
            hr = spQiConnection->ProgressNotification (iProgress);
        }
    }

    return hr;

}   // StatusUpdate (...)

HRESULT CZalSourceReader::ShowCurrentWord (BSTR bstrWord)
{
    HRESULT hr = H_NO_ERROR;

    int iNConnections = m_vec.GetSize();
    for (int iC = 0; iC < iNConnections; ++iC)
    {
        Lock();
        CComPtr<IUnknown> spUnkConnection = m_vec.GetAt (iC);
        CComQIPtr<IZalNotification2> spQiConnection = spUnkConnection;
        Unlock();
        if (spQiConnection)
        {
            hr = spQiConnection->ShowCurrentWord (bstrWord);
        }
    }

    return hr;

}   //  ShowCurrentWord (...)

HRESULT CZalSourceReader::StatusCheck (BOOL& bCancel)
{
    HRESULT hr = H_NO_ERROR;

    int iNConnections = m_vec.GetSize();
    for (int iC = 0; iC < iNConnections; ++iC)
    {
        Lock();
        CComPtr<IUnknown> spUnkConnection = m_vec.GetAt (iC);
        CComQIPtr<IZalNotification2> spQiConnection = spUnkConnection;
        Unlock();
        if (spQiConnection)
        {
            hr = spQiConnection->StatusCheck (&bCancel);
        }
    }

    return hr;
}

bool CZalSourceReader::bShow (const CEString& sMsg)
{
    LPTSTR szName = _T("\\\\.\\pipe\\ZalConversionLog"); 
  
    HANDLE hPipe = NULL;
    while (1) 
    { 
        hPipe = CreateFile (szName, 
                            GENERIC_WRITE,
                            0,              // no sharing 
                            NULL,           // default security attributes
                            OPEN_EXISTING,  // opens existing pipe 
                            0,              // default attributes 
                            NULL);          // no template file 
 
      if (INVALID_HANDLE_VALUE != hPipe) 
      {
          break;
      }
 
      if (GetLastError() != ERROR_PIPE_BUSY) 
      {
          return false;
      }
 
      if (!WaitNamedPipe (szName, 500)) 
      { 
         return false;
      } 
   } 
 
/*
   dwMode = PIPE_READMODE_MESSAGE; 
   fSuccess = SetNamedPipeHandleState( 
      hPipe,    // pipe handle 
      &dwMode,  // new pipe mode 
      NULL,     // don't set maximum bytes 
      NULL);    // don't set maximum time 
   if (!fSuccess) 
   {
      printf("SetNamedPipeHandleState failed"); 
      return 0;
   }
*/

    DWORD dwWritten = 0;
    UINT uiRet = WriteFile (hPipe,            // pipe handle 
                            sMsg,             // message 
                            sMsg.uiLength() * sizeof (wchar_t),  // message length 
                            &dwWritten,       // bytes written 
                            NULL);            // not overlapped 
   CloseHandle (hPipe); 
 
   return uiRet ? true : false; 

}   //  bShow (...)

bool CZalSourceReader::bSaveToDb()
{
    StLexemeHasher stLexemeHasher;
    multimap<StHeadword *, StDescriptor>::iterator itEntry = m_mmapDictionary.begin();
    StHeadword * pstCurrentHeadword = (*itEntry).first;
    __int64 llWordId = 0;
    bool bRet = pstCurrentHeadword->bSaveToDb(m_pDb, llWordId);
    if (!bRet)
    {
        return false;
    }
    (*itEntry).first->llWordId = llWordId;

    for (; itEntry != m_mmapDictionary.end(); ++itEntry)
    {
        if (pstCurrentHeadword != (*itEntry).first)
        {
            pstCurrentHeadword = (*itEntry).first;
            bRet = pstCurrentHeadword->bSaveToDb(m_pDb, (*itEntry).first->llWordId);
            if (!bRet)
            {
                return false;
            }
        }

		// 1. Concatenate source forms
        stLexemeHasher.sSourceForm = pstCurrentHeadword->sSource;
        vector<int>::iterator itHomonym = pstCurrentHeadword->vecIHomonyms.begin();
        for (; itHomonym != pstCurrentHeadword->vecIHomonyms.end(); ++itHomonym)
        {
            stLexemeHasher.sSourceForm += CEString::sToString (*itHomonym);
        }

        StDescriptor& stDescriptor = (*itEntry).second;
        __int64 llDescriptorId = 0;
        __int64 llInflectionId = 0;
        __int64 llAltInflectionId = 0;
        bRet = stDescriptor.bSaveToDb (m_pDb, pstCurrentHeadword->llWordId, llDescriptorId, llInflectionId, llAltInflectionId);
        if (!bRet)
        {
            return false;
        }

		// 2. Add headword stress positions
        stLexemeHasher.mapStress = pstCurrentHeadword->mapStress;

		// 3. Main symbol & comments
        stLexemeHasher.sMainSymbol = stDescriptor.sMainSymbol;
		stLexemeHasher.sComment = pstCurrentHeadword->sComment;
        stLexemeHasher.sComment += stDescriptor.sComment;
        stLexemeHasher.sComment += stDescriptor.sTrailingComment;

        if (stDescriptor.stInflectionGroup.bHasData)
        {
			// 4. Inflection data
            stLexemeHasher.iInflectionType = stDescriptor.stInflectionGroup.iType;
            stLexemeHasher.iAccentType1 = (int)stDescriptor.stInflectionGroup.eAccentType1;
            stLexemeHasher.iAccentType2 = (int)stDescriptor.stInflectionGroup.eAccentType2;
            stLexemeHasher.bSaveToDb (m_pDb, llDescriptorId, llInflectionId);
            if (stDescriptor.stAltInflectionGroup.bHasData)
            {
				// 4a. Alt. inflection data (if available, saved as a separate entry)
                stLexemeHasher.iInflectionType = stDescriptor.stAltInflectionGroup.iType;
                stLexemeHasher.iAccentType1 = (int)stDescriptor.stAltInflectionGroup.eAccentType1;
                stLexemeHasher.iAccentType2 = (int)stDescriptor.stAltInflectionGroup.eAccentType2;
                stLexemeHasher.bSaveToDb (m_pDb, llDescriptorId, llAltInflectionId);
            }
        }
        else
        {
            stLexemeHasher.bSaveToDb (m_pDb, llDescriptorId, llAltInflectionId);
        }
    }       // for ...

    return true;

}   //  bSaveToDb()

bool CZalSourceReader::bResolveReferences()
{
    ItDictionary itEntry = m_mmapDictionary.begin();
    for (; itEntry != m_mmapDictionary.end(); ++itEntry)
    {
        if (!(*itEntry).first->sOrigination.bIsEmpty())
        {
            vector<StHeadword *> vecReferences;
            bool bRet = bFindReferences((*itEntry).first->sOrigination, vecReferences);
            if (!bRet)
            {
                // error msg
                continue;
            }

            if (vecReferences.empty())
            {
                continue;
            }

            if (vecReferences.size() > 1)
            {
                // say that this will need manual processing
                continue;
            }

            StHeadword * const pstHeadword = vecReferences[0];
            m_mapReferences[(*itEntry).first->sOrigination] = pstHeadword;
        }

        if (!(*itEntry).first->sSeeRef.bIsEmpty())
        {
            vector<StHeadword *> vecReferences;
            bool bRet = bFindReferences((*itEntry).first->sSeeRef, vecReferences);
            if (!bRet)
            {
                // error msg
                continue;
            }

            if (vecReferences.empty())
            {
                continue;
            }

            if (vecReferences.size() > 1)
            {
                // say that this will need manual processing
                continue;
            }

            StHeadword * const pstHeadword = vecReferences[0];
            m_mapReferences[(*itEntry).first->sSeeRef] = pstHeadword;
        }
    }       // for (; itEntry != m_mmapDictionary.end(); ++itEntry)

    return true;

}   //  bResolveReferences()

bool CZalSourceReader::bUpdateReferences()
{
    ItDictionary itEntry = m_mmapDictionary.begin();
    for (; itEntry != m_mmapDictionary.end(); ++itEntry)
    {
        if (!(*itEntry).first->sOrigination.bIsEmpty())
        {
            CEString sKey = (*itEntry).first->sOrigination;
            map<CEString, StHeadword *>::iterator it = m_mapReferences.find(sKey);
            if (it != m_mapReferences.end())
            {
                StHeadword * pstHeadWord = (*it).second;
                __int64 llRef = pstHeadWord->llWordId;
                __int64 llMyId = (*itEntry).first->llWordId;
                try
                {
                    vector<CEString> vecColumns;
                    vecColumns.push_back(L"back_ref");
                    m_pDb->PrepareForUpdate (L"headword", vecColumns, llMyId);
                    m_pDb->Bind (1, llRef);
                    m_pDb->UpdateRow();
                    m_pDb->Finalize();        
                }
                catch (...)
                {
                    CEString sMsg;
                    CEString sError;
                    try
                    {
                        m_pDb->GetLastError (sError);
                        sMsg += L", error %d: ";
                        sMsg += sError;
                    }
                    catch (...)
                    {
                        sMsg = L"Apparent DB error ";
                    }
                
                    CString cs_msg;
                    cs_msg.Format (sMsg, m_pDb->iGetLastError());
                    ERROR_LOG ((LPCTSTR)cs_msg);

                    return false;
                }

            }
            else
            {
                // ERROR!!!
            }
        }

        if (!(*itEntry).first->sSeeRef.bIsEmpty())
        {
            CEString sKey = (*itEntry).first->sSeeRef;
            map<CEString, StHeadword *>::iterator it = m_mapReferences.find(sKey);
            if (it != m_mapReferences.end())
            {
                StHeadword * pstHeadWord = (*it).second;
                __int64 llRef = pstHeadWord->llWordId;
                __int64 llMyId = (*itEntry).first->llWordId;
                try
                {
                    vector<CEString> vecColumns;
                    vecColumns.push_back(L"see_ref");
                    m_pDb->PrepareForUpdate (L"headword", vecColumns, llMyId);
                    m_pDb->Bind (1, llRef);
                    m_pDb->UpdateRow();
                    m_pDb->Finalize();        
                }
                catch (...)
                {
                    CEString sMsg;
                    CEString sError;
                    try
                    {
                        m_pDb->GetLastError (sError);
                        sMsg += L", error %d: ";
                        sMsg += sError;
                    }
                    catch (...)
                    {
                        sMsg = L"Apparent DB error ";
                    }
                
                    CString cs_msg;
                    cs_msg.Format (sMsg, m_pDb->iGetLastError());
                    ERROR_LOG ((LPCTSTR)cs_msg);

                    return false;
                }
            }
        }
    }

    return true;

}   //  bUpdateReferences()

bool CZalSourceReader::bFindReferences(const CEString& sKey, vector<StHeadword *>& vecResults)
{
    ItDictionary itCurEntry = m_mmapDictionary.begin();
    for (; itCurEntry != m_mmapDictionary.end(); ++itCurEntry)
    {
        if ((*itCurEntry).first->sRawSource == sKey)
        {
            vecResults.push_back((*itCurEntry).first);
        }
    }

    return true;
}
