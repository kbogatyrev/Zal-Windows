#include "StdAfx.h"
#include "ExtString.h"
//#include "ZalEntryParser.h"
#include "Analysis.h"
#include "Synthesis.h"

int i_LCP(wstring* str_words, wstring* str_pfx, int i_words, int i_pfx)
{
	// Divides the input into up to i_pfx groups and finds their longest common prefixes.
	// They may differ in length, but don't exceed the length of the shortest word.
	if (str_words == NULL || i_words <= 0 || i_pfx < 0)
	{
		return -1;
	}

	bool b_class_changed = false, b_try_again = false, *b_disabled;
	int i_len, i_minlen = 65536, i_numpfx = 1, i_numpfx_prev = 1, i_w_finished = 0, *i_pfxclass, *i_based_on;
	wstring *str_curpfx, *str_cursymb;
	wstring str_s;
	str_curpfx = new wstring[i_pfx];
	str_cursymb = new wstring[i_pfx];
	str_curpfx[0] = str_cursymb[0] = L"";
	i_pfxclass = new int[i_words]; // Stores current prefix class number of the i_th string or -1 if corresponding prefix is finished
	i_based_on = new int[i_words]; // Temporarily stores the number of class i_th class is based on
	b_disabled = new bool[i_words];
	memset(i_pfxclass, 0, i_words * sizeof(int)); // Initially, there is only one prefix class
	memset(i_based_on, 0, i_words * sizeof(int));
	memset(b_disabled, 0, i_words * sizeof(bool)); // All the words are enabled

	// Find minimal word length
	for (int i_ = 0; i_ < i_words; i_++)
	{
		i_len = str_words[i_].length();
		if (i_minlen > i_len)
		{
			i_minlen = i_len;
		}
	}

	// Find the prefixes
	for (int i_l = 0; i_l < i_minlen; i_l++)
	{
		for (int i_w = 0; i_w < i_words; i_w++)
		{
			if (b_disabled[i_w] == true)
			{
				continue; // The word is disabled
			}
			str_s = str_words[i_w].substr(i_l, 1); //current symbol of the current word
			if (str_cursymb[i_pfxclass[i_w]].length() == 0)
			{
				str_cursymb[i_pfxclass[i_w]] = str_s;
			}
			else
			{
				if (str_s != str_cursymb[i_pfxclass[i_w]])
				{
					// Maybe we can just change its class to one of those recently introduced?
					for (int i_ = i_pfxclass[i_w] + 1; i_ < i_numpfx; i_++)
					{
						if (str_cursymb[i_] == str_s && i_based_on[i_] == i_pfxclass[i_w])
						{
							i_pfxclass[i_w] = i_;
							b_class_changed = true;
							break;
						}
					}

					if (b_class_changed == true)
					{
						b_class_changed = false;
						continue;
					}
					//if not:
					if (++i_numpfx > i_pfx)
					{
						//Finalize this prefix and initialize variables
						for (int i_ = 0; i_ < i_words; i_++)
						{
							if (i_based_on[i_] == i_pfxclass[i_w])
							{
								b_disabled[i_] = true;
								i_w_finished++; //One more word finished
							}
							i_pfxclass[i_] = i_based_on[i_];
						}
						for (int i_ = 0; i_ < i_pfx - 1; i_++)
						{
							str_cursymb[i_] = L"";
						}
						i_numpfx = i_numpfx_prev;
						i_l--;
						b_try_again = true; // Try again with the same symbol position, but more words disabled
						break;
					}
					else
					{
						// Introduce a new prefix class
						str_curpfx[i_numpfx - 1] = str_curpfx[i_pfxclass[i_w]];
						str_cursymb[i_numpfx - 1] = str_s;
						i_based_on[i_w] = i_pfxclass[i_w];
						i_pfxclass[i_w] = i_numpfx - 1;
					}
				}
			}
		}
		// Maybe we need to try the same position in the word again
		if (b_try_again == true)
		{
			b_try_again = false;
			if (i_w_finished == i_words)
			{
				break;
			}
			else
			{
				continue;
			}
		}
		// If not, add recently obtained symbols to the prefixes
		for (int i_ = 0; i_ < i_numpfx; i_++)
		{
			str_curpfx[i_] += str_cursymb[i_];
			str_cursymb[i_] = L"";
		}
		// Let recently formed classes become independent
		memcpy(i_based_on, i_pfxclass, i_words * sizeof(int));
		i_numpfx_prev = i_numpfx;
	}

    delete[] str_pfx;
    str_pfx = new wstring[i_numpfx_prev];
    for (int i_ = 0; i_ < i_numpfx_prev; i_++)
    {
        str_pfx[i_] = str_curpfx[i_];
    }
	delete[] str_curpfx;
	delete[] str_cursymb;
	delete[] i_pfxclass;
	return i_numpfx_prev;
}

/////////////////////////////////////////////////////////////////////
/*
CT_Wordform::~CT_Wordform() {}

CT_Wordform::CT_Wordform()
{
    eo_POS = POS_UNDEFINED;
    eo_Case = CASE_UNDEFINED;
    eo_Number = NUM_UNDEFINED;
    eo_AdjForm = FORM_UNDEFINED;
    eo_Gender = G_UNDEFINED;
    eo_Tense = T_UNDEFINED;
    eo_Person = P_UNDEFINED;
    eo_Reflexive = REFL_UNDEFINED;
    eo_Voice = VOICE_UNDEFINED;
    eo_Animateness = ANIM_UNDEFINED;
    eo_Special = SPECIAL_UNDEFINED;
}

CT_Wordform::CT_Wordform(wstring str_wf)
{
    eo_POS = POS_UNDEFINED;
    eo_Case = CASE_UNDEFINED;
    eo_Number = NUM_UNDEFINED;
    eo_AdjForm = FORM_UNDEFINED;
    eo_Gender = G_UNDEFINED;
    eo_Tense = T_UNDEFINED;
    eo_Person = P_UNDEFINED;
    eo_Reflexive = REFL_UNDEFINED;
    eo_Voice = VOICE_UNDEFINED;
    eo_Animateness = ANIM_UNDEFINED;
    eo_Special = SPECIAL_UNDEFINED;
    str_wordform = str_wf;
}

CT_Wordform::CT_Wordform(wstring str_wf, __int64 ll_id)
{
    eo_POS = POS_UNDEFINED;
    eo_Case = CASE_UNDEFINED;
    eo_Number = NUM_UNDEFINED;
    eo_AdjForm = FORM_UNDEFINED;
    eo_Gender = G_UNDEFINED;
    eo_Tense = T_UNDEFINED;
    eo_Person = P_UNDEFINED;
    eo_Reflexive = REFL_UNDEFINED;
    eo_Voice = VOICE_UNDEFINED;
    eo_Animateness = ANIM_UNDEFINED;
    eo_Special = SPECIAL_UNDEFINED;
    str_wordform = str_wf;
    ll_lexeme_id = ll_id;
}

void CT_Wordform::GramClear()
{
    eo_Case = CASE_UNDEFINED;
    eo_Number = NUM_UNDEFINED;
    eo_AdjForm = FORM_UNDEFINED;
    eo_Gender = G_UNDEFINED;
    eo_Tense = T_UNDEFINED;
    eo_Person = P_UNDEFINED;
    eo_Reflexive = REFL_UNDEFINED;
    eo_Voice = VOICE_UNDEFINED;
    eo_Animateness = ANIM_UNDEFINED;
    eo_Special = SPECIAL_UNDEFINED;
    return;
}

void CT_Wordform::Clear()
{
    GramClear();
    eo_POS = POS_UNDEFINED;
    str_wordform = L"";
    str_lemma = L"";
    ll_lexeme_id = -1;
    return;
}

int CT_Wordform::i_GramNumber()
{
    if (eo_POS == POS_UNDEFINED || eo_POS == POS_ADV || eo_POS == POS_PREP ||
        eo_POS == POS_CONJ || eo_POS == POS_PARTICLE || eo_POS == POS_COMPAR ||
        eo_POS == POS_INTERJ || eo_POS == POS_PARENTH)
    {
        return 0;
    }
    if (eo_POS == POS_NOUN) // 40 possibilities
    {
        return eo_Case * 3 + eo_Number;
    }
    if (eo_POS == POS_ADJ || eo_POS == POS_NUM || eo_POS == POS_PRONOUN) // 264 possibilities
    {
        if (eo_Case != CASE_ACC || eo_Animateness != ANIM_YES)
        {
            return eo_AdjForm * 132 + eo_Case * 12 + eo_Number * 4 + eo_Gender;
        }
        else
        {
            return eo_AdjForm * 132 + 10 * 12 + eo_Number * 4 + eo_Gender;
        }
    }
    if (eo_POS == POS_VERB)
    {
        if (eo_Special == SPECIAL_UNDEFINED)
        {
            return -1;
        }
        if (eo_Special == SPECIAL_NORMAL) // 144 possibilities
        {
            if (eo_Tense != T_PAST)
            {
                return eo_Tense * 36 + eo_Number * 12 + eo_Person * 3 + eo_Reflexive;
            }
            else
            {
                return 3 * 36 + eo_Number * 12 + eo_Gender * 3 + eo_Reflexive;
            }
        }
        if (eo_Special == SPECIAL_IMPERATIVE) // 36 possibilities
        {
            return eo_Number * 12 + eo_Person * 3 + eo_Reflexive + 144;
        }
        if (eo_Special == SPECIAL_PARTICIPLE) // 4752 possibilities
        {
            if (eo_Case != CASE_ACC || eo_Animateness != ANIM_YES)
            {
                return eo_Tense * 1188 + eo_Case * 108 + eo_Number * 36 + eo_Gender * 9 + eo_Voice * 3 + eo_Reflexive + 180;
            }
            else
            {
                return eo_Tense * 1188 + 10 * 108 + eo_Number * 36 + eo_Gender * 9 + eo_Voice * 3 + eo_Reflexive + 180;
            }
        }
        if (eo_Special == SPECIAL_CONVERB) // 12 possibilities
        {
            return eo_Tense * 3 + eo_Reflexive + 4932;
        }
        if (eo_Special == SPECIAL_INFINITIVE) // 2 possibilities
        {
            return eo_Reflexive + 4944;
        }
    }
    if (eo_Special == SPECIAL_COMPARATIVE) // 1 possibility
    {
        return 4946;
    }
    return -1;
}

int CT_Wordform::i_DecodeNumber(int i_gram)
{
    if (i_gram < 0 || i_gram > 4946)
    {
        return -1;
    }
    GramClear();
    if (eo_POS == POS_UNDEFINED || eo_POS == POS_ADV || eo_POS == POS_PREP ||
        eo_POS == POS_CONJ || eo_POS == POS_PARTICLE || eo_POS == POS_COMPAR ||
        eo_POS == POS_INTERJ || eo_POS == POS_PARENTH)
    {
        return 0;
    }
    if (eo_POS == POS_NOUN) // 40 possibilities
    {
        eo_Number = (ET_Number)(i_gram % 3);
        eo_Case = (ET_Case)((i_gram - eo_Number) / 3);
        return 0;
    }
    if (eo_POS == POS_ADJ || eo_POS == POS_NUM || eo_POS == POS_PRONOUN) // 264 possibilities
    {
        eo_Gender = (ET_Gender)(i_gram % 4);
        i_gram = (i_gram - eo_Gender) / 4;
        eo_Number = (ET_Number)(i_gram % 3);
        i_gram = (i_gram - eo_Number) / 3;
        int i_tmp_case = i_gram % 11;
        if (i_tmp_case == 10)
        {
            eo_Case = CASE_ACC;
            eo_Animateness = ANIM_YES;
        }
        else
        {
            eo_Case = (ET_Case)i_tmp_case;
            if (eo_Case != CASE_UNDEFINED)
            {
                eo_Animateness = ANIM_NO;
            }
        }
        i_gram = (i_gram - i_tmp_case) / 11;
        eo_AdjForm = (ET_AdjForm)i_gram;
    }
    if (eo_POS == POS_VERB)
    {
        if (i_gram < 144) // 144 possibilities
        {
            eo_Special = SPECIAL_NORMAL;
            eo_Reflexive = (ET_Reflexive)(i_gram % 3);
            i_gram = (i_gram - eo_Reflexive) / 3;
            if (i_gram < 108)
            {
                eo_Person = (ET_Person)(i_gram % 4);
                i_gram = (i_gram - eo_Person) / 4;
                eo_Number = (ET_Number)(i_gram % 3);
                i_gram = (i_gram - eo_Number) / 3;
                eo_Tense = (ET_Tense)i_gram;
                return 0;
            }
            else
            {
                eo_Gender = (ET_Gender)(i_gram % 4);
                i_gram = (i_gram - eo_Gender) / 4;
                eo_Number = (ET_Number)(i_gram % 3);
                eo_Tense = (ET_Tense)3;
                return 0;
            }
        }
        if (i_gram >= 144 && i_gram < 180) // 36 possibilities
        {
            i_gram -=144;
            eo_Special = SPECIAL_IMPERATIVE;
            eo_Reflexive = (ET_Reflexive)(i_gram % 3);
            i_gram = (i_gram - eo_Reflexive) / 3;
            eo_Person = (ET_Person)(i_gram % 4);
            i_gram = (i_gram - eo_Person) / 4;
            eo_Number = (ET_Number)i_gram;
            return 0;
        }
        if (eo_Special == SPECIAL_PARTICIPLE) // 4752 possibilities
        {
            i_gram -= 180;
            eo_Special = SPECIAL_PARTICIPLE;
            eo_Reflexive = (ET_Reflexive)(i_gram % 3);
            i_gram = (i_gram - eo_Reflexive) / 3;
            eo_Voice = (ET_Voice)(i_gram % 3);
            i_gram = (i_gram - eo_Voice) / 3;
            eo_Gender = (ET_Gender)(i_gram % 4);
            i_gram = (i_gram - eo_Gender) / 4;
            eo_Number = (ET_Number)(i_gram % 3);
            i_gram = (i_gram - eo_Number) / 3;
            int i_tmp_case = i_gram % 11;
            if (i_tmp_case == 10)
            {
                eo_Case = CASE_ACC;
                eo_Animateness = ANIM_YES;
            }
            else
            {
                eo_Case = (ET_Case)i_tmp_case;
                if (eo_Case != CASE_UNDEFINED)
                {
                    eo_Animateness = ANIM_NO;
                }
            }
            i_gram = (i_gram - i_tmp_case) / 11;
            eo_Tense = (ET_Tense)i_gram;
            return 0;
        }
        if (i_gram >= 4932 && i_gram < 4944) // 12 possibilities
        {
            i_gram -= 4932;
            eo_Special = SPECIAL_CONVERB;
            eo_Reflexive = (ET_Reflexive)(i_gram % 3);
            i_gram = (i_gram - eo_Reflexive) / 3;
            eo_Tense = (ET_Tense)i_gram;
            return 0;
        }
        if (i_gram >= 4944 && i_gram < 4946) // 2 possibilities
        {
            i_gram -= 4944;
            eo_Special = SPECIAL_INFINITIVE;
            eo_Reflexive = (ET_Reflexive)i_gram;
            return 0;
        }
    }
    if (i_gram == 4946)
    {
        eo_Special = SPECIAL_COMPARATIVE;
        return 0;
    }
    return -1;
}

int CT_Wordform::i_DecodeString(wstring str_gram)
{
    // Partially depends on POS.

    // Warning!
    // 1. The list may be incomplete.
    // 2. Verb reflexivity is recognized only if str_lemma is present.

    GramClear();
    if (str_gram.length() == 0)
    {
        return 0;
    }
    wsmatch result;
    wstring str_parameter;
    bool b_match;
    while (str_gram.length() > 0)
    {
        b_match = regex_match(str_gram, result, (const wregex)L"\\s*([^\\s\\.]*\\.?)\\s*(.*)");
        str_parameter = result[1];
        str_gram = result[2];
        if (str_parameter == L"1")
        {
            eo_Person = P_1;
        }
        if (str_parameter == L"2")
        {
            eo_Person = P_2;
        }
        if (str_parameter == L"3")
        {
            eo_Person = P_3;
        }
        if (str_parameter == L"ед.")
        {
            eo_Number = NUM_SG;
        }
        if (str_parameter == L"мн.")
        {
            eo_Number = NUM_PL;
        }
        if (str_parameter == L"И.")
        {
            eo_Case = CASE_NOM;
        }
        if (str_parameter == L"Р.")
        {
            eo_Case = CASE_GEN;
        }
        if (str_parameter == L"Д.")
        {
            eo_Case = CASE_DAT;
        }
        if (str_parameter == L"В.")
        {
            eo_Case = CASE_ACC;
        }
        if (str_parameter == L"Т.")
        {
            eo_Case = CASE_INST;
        }
        if (str_parameter == L"П.")
        {
            eo_Case = CASE_PREP;
        }
        if (str_parameter == L"Р2")
        {
            eo_Case = CASE_PART;
        }
        if (str_parameter == L"П2")
        {
            eo_Case = CASE_LOC;
        }
        if (str_parameter == L"кф")
        {
            eo_AdjForm = FORM_SHORT;
        }
        if (str_parameter == L"буд.")
        {
            eo_Tense = T_FUTURE;
        }
        if (str_parameter == L"наст.")
        {
            eo_Tense = T_PRESENT;
        }
        if (str_parameter == L"прош.")
        {
            eo_Tense = T_PAST;
        }
        if (str_parameter == L"прич.")
        {
            eo_Special = SPECIAL_PARTICIPLE;
        }
        if (str_parameter == L"деепр.")
        {
            eo_Special = SPECIAL_CONVERB;
        }
        if (str_parameter == L"повел.")
        {
            eo_Special = SPECIAL_IMPERATIVE;
        }
        if (str_parameter == L"страд.")
        {
            eo_Voice = VOICE_PASSIVE;
        }
        if (str_parameter == L"м")
        {
            eo_Gender = G_M;
        }
        if (str_parameter == L"ж")
        {
            eo_Gender = G_F;
        }
        if (str_parameter == L"с")
        {
            eo_Gender = G_N;
        }
        if (str_parameter == L"одуш.")
        {
            eo_Animateness = ANIM_YES;
        }
        if (str_parameter == L"сравн.")
        {
            eo_Special = SPECIAL_COMPARATIVE;
        }
    }
    if (eo_POS == POS_ADV)
    {
        if (eo_AdjForm == FORM_UNDEFINED)
        {
            eo_AdjForm = FORM_LONG;
        }
        if (eo_Case == CASE_ACC && eo_Animateness == ANIM_UNDEFINED)
        {
            eo_Animateness = ANIM_NO;
        }
    }
    if (eo_POS == POS_VERB)
    {
        if (eo_Special == SPECIAL_UNDEFINED)
        {
            eo_Special = SPECIAL_NORMAL;
        }
        if (eo_Special == SPECIAL_IMPERATIVE && eo_Person == P_UNDEFINED)
        {
            eo_Person = P_2;
        }
        if (eo_Special == SPECIAL_PARTICIPLE && eo_Case == CASE_ACC && eo_Animateness == ANIM_UNDEFINED)
        {
            eo_Animateness = ANIM_NO;
        }
        if (eo_Special == SPECIAL_PARTICIPLE && eo_Voice == VOICE_UNDEFINED)
        {
            eo_Voice = VOICE_ACTIVE;
        }
        b_match = regex_match(str_lemma, result, (const wregex)L".*(ся|сь)");
        if (b_match == true)
        {
            eo_Reflexive = REFL_YES;
        }
        else
        {
            eo_Reflexive = REFL_NO;
        }
    }
    return 0;
}

//////////////////////////////////////////////////////////////////////////////////////////////////

CT_EndingsTable::CT_EndingsTable()
{
    pvec_Endings = new vector<wstring>;
    pvec_Gram = new vector<int>;
}

CT_EndingsTable::~CT_EndingsTable()
{
    delete pvec_Endings;
    delete pvec_Gram;
}

void CT_EndingsTable::Clear()
{
    pvec_Endings->clear();
    pvec_Gram->clear();
    str_Stem = L"";
}

int CT_EndingsTable::i_AddEnding(int i_Gram, wstring str_Ending)
{
    pvec_Endings->push_back(str_Ending);
    pvec_Gram->push_back(i_Gram);
    return 0;
}

int CT_EndingsTable::i_db_id(CT_Sqlite* pco_dbHandle) // Tries to find the table in the db, returns -1 if there's no such table
{
    // It works, checked by TA on 2009.07.27.
    // !!! There can be several tables! So an integer should be changed to a vector.
    if (pco_dbHandle == NULL)
    {
        return -3;
    }
    if (pvec_Endings->empty() == true || pvec_Gram->empty() == true)
    {
        return -2;
    }
    wstring str_query, str_count, str_gram;
    int i_subtable_id = -1;
    str_gram = str_int2wstring(pvec_Gram->at(0));
    str_query += L"Select * from endings as a0 where ending = \"" 
        + pvec_Endings->at(0) + L"\" and gram = " + str_gram;
    for (unsigned int i_ = 1; i_ < pvec_Endings->size(); i_++)
    {
        str_count = str_int2wstring(i_);
        str_gram = str_int2wstring(pvec_Gram->at(i_));
        str_query += L" and exists (select * from endings as a" + str_count 
            + L" where ending = \"" + pvec_Endings->at(i_) + L"\" and gram = " 
            + str_gram + L" and a" + str_count + L".subtable_id = a0.subtable_id)";
    }
    ERROR_LOG(L"*** CT_EndingsTable::i_db_id works: str_query = '" + str_query + L"'.");
    pco_dbHandle->v_Exec(str_query, v_LearnSubtableID_Endings, &i_subtable_id);
    return i_subtable_id;
}

int CT_EndingsTable::i_db_Write(CT_Sqlite* pco_dbHandle, int i_subtable_id)
{
    if (pco_dbHandle == NULL || i_subtable_id < 0)
    {
        return -1;
    }
    int i_inserted = 0;
    vector<wstring>::iterator iter_Ending;
    vector<int>::iterator iter_Gram;
    for (iter_Ending = pvec_Endings->begin(), iter_Gram = pvec_Gram->begin();
        (iter_Ending != pvec_Endings->end()) && (iter_Gram != pvec_Gram->end());
        iter_Ending++, iter_Gram++, i_inserted++)
    {
        // For each couple <ending, grammatical parameters>, insert it into the endings table.
        pco_dbHandle->v_PrepareForInsert(L"endings", 3);
        pco_dbHandle->v_Bind(1, i_subtable_id);  // 0-based
        pco_dbHandle->v_Bind(2, *iter_Gram);
        pco_dbHandle->v_Bind(3, *iter_Ending);
        pco_dbHandle->v_Finalize();
    }
    return i_inserted;
}


///////////////////////////////////////////////////////////////////////////////////////////////////

int i_Analyze(CT_Sqlite* pco_dbHandle, wstring str_wordform)
{
    if (str_wordform.length() <= 0 || pco_dbHandle == NULL)
    {
        return -1;
    }
    wstring str_left, str_right;
    vector<int>* pvec_stems_id;
    vector<CT_Wordform>* pvec_possible_wordforms;
    pvec_possible_wordforms = new vector<CT_Wordform>;
    pvec_stems_id = new vector<int>;
    for (int i_left = str_wordform.length(); i_left >= 0; i_left--)
    {
        str_left = str_wordform.substr(0, i_left);
        str_right = str_wordform.substr(i_left, str_wordform.length() - i_left);
        pvec_stems_id->clear();
        i_LookUpStems(pco_dbHandle, pvec_stems_id, str_left);
        if (pvec_stems_id->empty() == true)
        {
            continue;
        }
        i_CheckEndings(pco_dbHandle, pvec_possible_wordforms, pvec_stems_id, str_right);
    }
    if (pvec_possible_wordforms->empty() == true)
    {
        return 1;
    }
    return 0;
}

int i_LookUpStems(CT_Sqlite* pco_dbHandle, vector<int>* pvec_stems_id, wstring str_left)
{
    // Search the DB for graphic stems equal to str_left; write their IDs to vec_stems_id
    if (pco_dbHandle == NULL || pvec_stems_id == NULL)
    {
        return -1;
    }
    wstring str_query = L"Select * from GrStems where stem = \"" + str_left + L"\"";
    pco_dbHandle->v_Exec(str_query, v_LearnStemsId, pvec_stems_id);
    return pvec_stems_id->size();
}

int i_CheckEndings(CT_Sqlite* pco_dbHandle, vector<CT_Wordform>* pvec_possible_wordforms, vector<int>* pvec_stems_id, wstring str_right)
{
    CT_Wordform co_tmp_wf;
    static vector<int> vec_gram;
    wstring str_query;
    int i_subtable_id;
    for (vector<int>::iterator iter_stems = pvec_stems_id->begin();
            iter_stems != pvec_stems_id->end(); iter_stems++)
    {
        // For each *iter_stems look up the endings table ID in DB, then in this table try to find
        // endings which are equal to str_right. For each ending found, write the parameters
        // to co_tmp_wf and then push_back co_tmp_wf to pvec_possible_wordforms:
        vec_gram.clear();
        str_query = L"Select * from stems where id = " + str_int2wstring(*iter_stems);
        pco_dbHandle->v_Exec(str_query, v_LearnSubtableID_Stems, &i_subtable_id);
        str_query = L"Select * from endings where subtable_id = " + str_int2wstring(i_subtable_id) 
                    + L" and ending = \"" + str_right + L"\"";
        pco_dbHandle->v_Exec(str_query, v_LearnGram_Endings, &vec_gram);
        for (vector<int>::iterator iter_endings = vec_gram.begin();
            iter_endings != vec_gram.end(); iter_endings++)
        {
            co_tmp_wf.i_DecodeNumber(*iter_endings);
            //co_tmp_wf.ll_lexeme_id = ...
            //...
            pvec_possible_wordforms->push_back(co_tmp_wf);
            co_tmp_wf.Clear();
        }
    }
    vec_gram.clear();
    return 0;
}

int i_WriteStemsAndEndings(CT_Sqlite* pco_dbHandle, __int64 ll_lexeme_id, int last_subtable_id)
{
    if (ll_lexeme_id < 0)
    {
        return -1;
    }

    wstring str_Stem;
    //wstring str_Stem = str_GrStem(ll_lexeme_id);
    //if (str_Stem == L"")
    //{
        wsmatch result;
        wstring *str_Paradigm, *str_Stems, str_ending;
        int *i_Gram;
        bool b_match;
        // Generate paradigm (i_th element of str_Paradigm has form i_Gram[i])
        int i_Wordforms = i_GenerateParadigm(pco_dbHandle, ll_lexeme_id, str_Paradigm, i_Gram);
        if (i_Wordforms <= 0)
        {
            return 1;
        }
        // Create several gr_stems
        int i_Stems = i_LCP(str_Paradigm, str_Stems, i_Wordforms, 3);
        // Split every wordform into stem and ending
        for (int i_wf = 0; i_wf < i_Wordforms; i_wf++)
        {
            for (int i_s = 0; i_s < i_Stems; i_s++)
            {
                b_match = regex_match(str_Paradigm[i_wf], result, (const wregex)(str_Stems[i_s] + L"(.*)"));
                if (b_match == true)
                {
                    str_ending = (wstring)result[1];
                    // Write the ending, its grammatical meaning and i_s to the temporary "endings table"
                    break;
                }
            }
        }
        // Check whether the temporary "endings tables" coincide with the existing real tables
    //}
    return 0;
}

wstring str_int2wstring(int i_input)
{
    wstringstream ss_tmp;
    ss_tmp << i_input;
    return ss_tmp.str();
}

void v_LearnSubtableID_Endings(sqlite3_stmt *po_statement, void *v_subtable_id)
{
    if (v_subtable_id == NULL)
    {
        return;
    }
    int *i_subtable_id = (int *)v_subtable_id;
    *i_subtable_id = sqlite3_column_int(po_statement, 1); // Provided the subtable_id column is #1.
    ERROR_LOG(L"*** v_LearnSubtableID_Endings works.");
    return;
}

void v_LearnSubtableID_Stems(sqlite3_stmt *po_statement, void *v_subtable_id)
{
    if (v_subtable_id == NULL)
    {
        return;
    }
    vector<int> *pvec_subtable_id = (vector<int> *)v_subtable_id;
    pvec_subtable_id->push_back(sqlite3_column_int(po_statement, 3)); // Provided the subtable_id column is #3.
    ERROR_LOG(L"*** v_LearnSubtableID_Stems works.");
    return;
}

void v_LearnGram_Endings(sqlite3_stmt *po_statement, void *v_gram)
{
    if (v_gram == NULL)
    {
        return;
    }
    vector<int> *pvec_gram = (vector<int> *)v_gram;
    pvec_gram->push_back(sqlite3_column_int(po_statement, 2)); // Provided the gram column is #3.
    ERROR_LOG(L"*** v_LearnGram_Endings works.");
    return;
}

void v_LearnStemsId(sqlite3_stmt *po_statement, void *v_stems_id)
{
    if (v_stems_id == NULL)
    {
        return;
    }
    vector<int>* pvec_stems_id = (vector<int>*)v_stems_id;
    pvec_stems_id->push_back(sqlite3_column_int(po_statement, 0));
    return;
}
*/
