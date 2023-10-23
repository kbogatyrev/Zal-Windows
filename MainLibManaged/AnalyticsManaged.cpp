#define NOMINMAX

#include <memory>
#include <msclr\marshal.h>
#include "GramHasher.h"

#include "UtilsManaged.h"
#include "AnalyticsManaged.h"

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace MainLibManaged;
using namespace std;

//
//      CAnalyticsManaged

CAnalyticsManaged::CAnalyticsManaged(shared_ptr<CAnalytics> spAnalytics) : m_pAnalytics(&spAnalytics)
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
    auto eRet = (EM_ReturnCode)(*m_pAnalytics)->eParseText(sFromManagedString(sName), sFromManagedString(sMetaData), sFromManagedString(sText), llId, bIsProse);
    lParsedTextId = llId;

    return eRet;
}

