#pragma once

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
#include "EnumsManaged.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace Hlib;

namespace MainLibManaged
{
//    public delegate void DelegateProgress(int iPercentDone, bool bOperationComplete);

    ref class CLexemeManaged;
    ref class CLexemeEnumeratorManaged;
    ref class CInflectionManaged;
    ref class CInflectionEnumeratorManaged;
    ref class CParserManaged;
    ref class CAnalyticsManaged;
    ref class CVerifierManaged;

    public ref class CWordFormManaged
    {
    protected:
        int64_t m_iHandle{ -1 };    // seq # of this instance in parent container

    public:
        CWordFormManaged(int64_t iHandle);
        ~CWordFormManaged();

        CWordForm* pGetInstance();

        EM_ReturnCode eGetInflection(CInflectionManaged^% inflection);

        String^ sWordForm();

        void SetWordForm(String^);
        EM_ReturnCode eSetStressPositions(Collections::Generic::Dictionary<int, EM_StressType>^ dctStressPositions);

        long long llWordFormDbId();

        String^ sStem();
        void SetStem(String^);

        __int64 llLexemeId();

        EM_PartOfSpeech ePos();
        void SetPos(EM_PartOfSpeech);

        EM_Case eCase();
        void SetCase(EM_Case);

        EM_Number eNumber();
        void SetNumber(EM_Number);

        EM_Subparadigm eSubparadigm();
        void SetSubparadigm(EM_Subparadigm);

        EM_Gender eGender();
        void SetGender(EM_Gender);

        EM_Person ePerson();
        void SetPerson(EM_Person);

        EM_Animacy eAnimacy();
        void SetAnimacy(EM_Animacy);

        EM_Reflexive eReflexive();
        void SetReflexivity(EM_Reflexive);

        EM_Aspect eAspect();
        void SetAspect(EM_Aspect);

        EM_Status eStatus();
        void SetStatus(EM_Status);

        bool bIrregular();      // came from the DB as opposed to being generated by the app
        void SetIrregular(bool);

        String^ sLeadComment();
        void SetLeadComment(String^ sLeadComment);

        String^ sTrailingComment();
        void SetTrailingComment(String^ sTrailingComment);

        bool bIsEdited();
        void SetIsEdited(bool bIsEdited);

        bool bIsVariant();
        void SetIsVariant(bool bIsVariant);

        EM_ReturnCode eGetFirstStressPos(int% iPos, EM_StressType% eType);
        EM_ReturnCode eGetNextStressPos(int% iPos, EM_StressType% eType);

        String^ sGramHash();
        EM_ReturnCode eInitFromHash(String^ sHash);

        EM_ReturnCode eSaveIrregularForm();

    };      //  public ref class CWordFormManaged

}       // namespace