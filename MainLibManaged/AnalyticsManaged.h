#pragma once

#include <memory>
#include "EnumsManaged.h"
#include "Dictionary.h"
#include "Lexeme.h"
#include "Inflection.h"
#include "WordForm.h"
#include "Gramhasher.h"
#include "Parser.h"
#include "Analytics.h"
#include "Verifier.h"
//#include "AspectPair.h"
#include "..\Hlib\SqliteWrapper.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace Hlib;

namespace MainLibManaged
{

    public ref class CAnalyticsManaged
    {
        shared_ptr<Hlib::CAnalytics>* m_pAnalytics;

    public:
        CAnalyticsManaged(shared_ptr<CAnalytics>);
        ~CAnalyticsManaged();

        EM_ReturnCode eParseText(String^ sName, String^ sMetaData, String^ sText, long long% lParseTextId, bool bIsProse);
    };
}       // namespace