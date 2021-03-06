#include <msclr\marshal.h>
#include <string>
#include "ExtString.h"

#using <mscorlib.dll>
using namespace msclr::interop;
using namespace std;
using namespace System;
using namespace DBHandlerLib;

#pragma managed

public ref class CT_ConversionLibTest
{

public:
    CT_ConversionLibTest() {};
    ~CT_ConversionLibTest() {};
    int test();
    int testInsert (String ^ dnstr_);
    int testStringConversion (String ^ dnstr_);
    int testStringParse (String ^ dnstr_);
    int testError();

private:
    SqliteWrapperClass ^ po_sqlite;

};

int CT_ConversionLibTest::test()
{
    po_sqlite = gcnew SqliteWrapperClass();    
    po_sqlite->Open (gcnew String (L"test.db")); 
    po_sqlite->Exec (gcnew String (L"CREATE TABLE Example (Key TEXT, Description TEXT)"));

	return 0;
}

int CT_ConversionLibTest::testInsert (String ^ dnstr_)
{
    String ^ dnstr_Insert = L"INSERT INTO Example VALUES ('";
    dnstr_Insert += dnstr_;
    dnstr_Insert += L"', 'test')";
    po_sqlite->Exec (dnstr_Insert);

    testStringParse (dnstr_);

	return 0;
}

int CT_ConversionLibTest::testStringConversion (String ^ dnstr_)
{
    //
    // Method 1:
    //
    marshal_context ^ co_context = gcnew marshal_context();
    const wchar_t * sz_result;
    sz_result = co_context->marshal_as<const wchar_t *> (dnstr_);
    wstring wstr_(sz_result);
    delete co_context;

    //
    // Method2:
    //
//    CString cstr_ (dotNetStr_);

	return 0;
}

int CT_ConversionLibTest::testStringParse (String ^ dnstr_)
{
    marshal_context ^ co_context = gcnew marshal_context();
    const wchar_t * sz_result;
    sz_result = co_context->marshal_as<const wchar_t *> (dnstr_);

    CT_ExtString xstr_ (sz_result, _T(" "), _T(""), _T(""), _T("()[]"));

    delete co_context;

    for (int i_ = 0; i_ < xstr_.i_NFields(); ++i_)
    {
        wstring str_field = xstr_.str_GetField (i_);
        int fff = 0;
    }

    return 0;
}

int CT_ConversionLibTest::testError()
{
    ERROR_LOG (_T("Test Error Log Тест журнала ошибок"));

    return 0;
}
