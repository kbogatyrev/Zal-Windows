#pragma once

#define NOMINMAX

#include "UtilsManaged.h"

using namespace System;
using namespace Hlib;
using namespace msclr::interop;

namespace MainLibManaged
{
    CEString sFromManagedString(String^ sSource)
    {
        // This conversion requires an instance of marshal_context class
        //marshal_context^ mc = gcnew marshal_context();
        //CEString cestrResult(mc->marshal_as<const wchar_t *>(sSource));


        // Pin memory so GC can't move it while native function is called
        pin_ptr<const wchar_t> pSource = PtrToStringChars(sSource);
        CEString cestrResult(pSource);
        return cestrResult;
    }

    const CEString sFromManagedString(const String^ sSource)
    {
        // This conversion requires an instance of marshal_context class
        //marshal_context^ mc = gcnew marshal_context();
        //CEString cestrResult(mc->marshal_as<const wchar_t *>(sSource));


        // Pin memory so GC can't move it while native function is called
        pin_ptr<const wchar_t> pSource = PtrToStringChars(sSource);
        CEString cestrResult(pSource);
        return cestrResult;
    }
}