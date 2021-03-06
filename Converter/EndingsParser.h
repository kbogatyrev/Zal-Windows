#pragma once

#include "Globals.h"
#include "SqliteWrapper.h"

using namespace Hlib;

class CEndingsParser
{
public:
    CEndingsParser (const CEString& sEntry, CSqlite * pDb) 
        : m_sSource (sEntry), m_pDb (pDb)
    {
        Init();
    };

    bool bParse();

private:

protected:
    CSqlite * m_pDb;
    CEString m_sSource;
    CEString m_sEnding;
    ET_EndingClass m_eClass;
    ET_Gender m_eGender;
    ET_Case m_eCase;
    int m_iInflectionType;
    ET_Animacy m_eAnimacy;
    ET_StressLocation m_eStress;
    ET_Conjugation m_eConjugation;
    ET_Number m_eNumber;
    ET_Person m_ePerson;
    bool m_bStemAugment;
    int m_iStemAugmentType;
    int m_iCommonDeviation;
    ET_StemAuslaut m_eStemAuslaut;
    ET_Reflexive m_eRefl;
    int m_iEndingType;             // currently only used with imperative

protected:
    void Init();
    bool bSaveToDb();

};
