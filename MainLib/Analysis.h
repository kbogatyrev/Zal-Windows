#pragma once

#include "SqliteWrapper.h"
#include "Globals.h"

using namespace std;
using namespace std::tr1;

/*
class CT_Wordform
{
public:
    wstring str_wordform;
    wstring str_lemma;
    __int64 ll_lexeme_id;
    ET_PartOfSpeech eo_POS;
    ET_Case eo_Case;
    ET_Number eo_Number;
    ET_AdjForm eo_AdjForm;
    ET_Gender eo_Gender;
    ET_Tense eo_Tense;
    ET_Person eo_Person;
    ET_Animateness eo_Animateness;
    ET_Reflexive eo_Reflexive;
    ET_Voice eo_Voice;
    ET_SpecialForm eo_Special;
    
    CT_Wordform();
    CT_Wordform(wstring);
    CT_Wordform(wstring, __int64);
    ~CT_Wordform();

    void GramClear();
    void Clear();
    int i_GramNumber();
    int i_DecodeNumber(int i_gram);
    int i_DecodeString(wstring str_gram);
};
*/

class CT_EndingsTable
{
public:
    wstring str_Stem;
    vector<wstring>* pvec_Endings;
    vector<int>* pvec_Gram;

    CT_EndingsTable();
    ~CT_EndingsTable();

    void Clear();
    int i_AddEnding(int i_Gram, wstring str_Ending);
    int i_db_id(CT_Sqlite* pco_dbHandle); // Tries to find the table in the db, returns -1 if there's no such table
    int i_db_Write(CT_Sqlite* pco_dbHandle, int i_id); // Write the whole table to db with id = i_id
};

wstring str_int2wstring(int i_input);
void v_LearnSubtableID_Endings(sqlite3_stmt *po_statement, void *i_subtable_id);
void v_LearnSubtableID_Stems(sqlite3_stmt *po_statement, void *v_subtable_id);
void v_LearnGram_Endings(sqlite3_stmt *po_statement, void *v_gram);
void v_LearnStemsId(sqlite3_stmt *po_statement, void *v_stems_id);
int i_LCP(wstring* str_words, wstring* str_pfx, int i_words, int i_pfx);
int i_Analyze(CT_Sqlite* pco_dbHandle, wstring str_wordform);
int i_LookUpStems(CT_Sqlite* pco_dbHandle, vector<int>* pvec_stems_id, wstring str_left);
int i_CheckEndings(CT_Sqlite* pco_dbHandle, vector<CT_Wordform>* pvec_possible_wordforms,
                   vector<int>* pvec_stems_id, wstring str_right);
int i_WriteStemsAndEndings(CT_Sqlite* pco_dbHandle, __int64 ll_lexeme_id, int last_endings_id);