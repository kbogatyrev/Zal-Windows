#pragma once

#define NOMINMAX

#include <msclr\marshal.h>
#include "EString.h"

using namespace System;
using namespace Hlib;
using namespace msclr::interop;

namespace MainLibManaged
{
    public delegate void DelegateProgress(int iPercentDone, bool bOperationComplete);
    CEString sFromManagedString(String^ sSource);
    const CEString sFromManagedString(const String^ sSource);
}