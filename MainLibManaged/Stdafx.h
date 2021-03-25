// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#define _SILENCE_CXX17_CODECVT_HEADER_DEPRECATION_WARNING
#define _SILENCE_CXX17_STRSTREAM_DEPRECATION_WARNING

#pragma once

#define _CRTDBG_MAP_ALLOC
#include <stdlib.h>
#include <crtdbg.h>

#define _ATL_APARTMENT_THREADED
#define _ATL_NO_AUTOMATIC_NAMESPACE

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS    // some CString constructors will be explicit

#define S_TRUE MAKE_ET_ReturnCode (SEVERITY_SUCCESS, FACILITY_ITF, 10001)
#define S_BOTH MAKE_ET_ReturnCode (SEVERITY_SUCCESS, FACILITY_ITF, 10002)
#define S_MORE MAKE_ET_ReturnCode (SEVERITY_SUCCESS, FACILITY_ITF, 10003)


//
// ATL
//
//#include "resource.h"
//#include <atlbase.h>
//#include <atlcom.h>
//#include <atlctl.h>
//#include <atlstr.h>

//using namespace ATL;


//
// STL
//
#include <vector>
#include <string>
#include <map>
#include <set>
#include <algorithm>
#include <iostream>
#include <sstream>
#include <strstream>
#include <ctime>
#include <tchar.h>

//
// TR1
//
//#include <regex>

//using namespace std::tr1;

//
//
// CRT
#include <io.h>


//
// Zal
//
#include "Logging.h"

#define CYRILLIC

#include "Enums.h"
#include "Exception.h"
#include "EString.h"
#include "Callbacks.h"
#include "GramHasher.h"
//#include "SqliteWrapper.h"

#include <stdlib.h>
#include <crtdbg.h>

using namespace std;

