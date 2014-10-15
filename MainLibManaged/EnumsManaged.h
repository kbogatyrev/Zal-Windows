namespace MainLibManaged
{
    public enum class EM_Ternary
    {
        TERNARY_FALSE,
        TERNARY_TRUE,
        TERNARY_BOTH
    };

    public enum class EM_ReturnCode
    {
        H_NO_ERROR = 0,
        H_FALSE,
        H_TRUE,
        H_NO_MORE,
        H_ERROR_GENERAL =       -1,
        H_ERROR_UNEXPECTED =    -2,
        H_ERROR_POINTER =       -3,
        H_ERROR_INVALID_ARG =   -4,
        H_ERROR_DB =            -5,
        H_EXCEPTION =           -6,            // an exception was caught and handled upstream
        H_CTOR =                -7,            // exception thrown from a ctor
        H_NOTIMPL =             -8,            // interface method not implemented (but may be in the future)
        H_UNAVAILABLE =         -9             // interface method not implemented (and never will be) for this consumer

    };

    public ref class CErrorCode
    {
    public:
        static bool bError(EM_ReturnCode eRc)
        {
            if ((int)eRc >= 0) return false;
            return true;
        }
    };

    public enum class EM_PartOfSpeech
    {
        POS_UNDEFINED,
        POS_NOUN,
        POS_VERB,
        POS_ADJ,
        POS_PRONOUN,
        POS_PRONOUN_ADJ,
        POS_PRONOUN_PREDIC,
        POS_NUM,
        POS_NUM_ADJ,
        POS_ADV,
        POS_COMPAR,
        POS_PREDIC,
        POS_PREP,
        POS_CONJ,
        POS_PARTICLE,
        POS_INTERJ,
        POS_PARENTH,
        POS_NULL,
        POS_COUNT
    };

    public enum class EM_InflectionType
    {
        INFLECTION_TYPE_UNDEFINED = -1,
        CONJUGATIONS_COUNT = 16
    };

    public enum class EM_Case
    {
        CASE_UNDEFINED,
        CASE_NOM,
        CASE_ACC,
        CASE_GEN,
        CASE_PART,
        CASE_DAT,
        CASE_INST,
        CASE_PREP,
        CASE_LOC,
        CASE_NUM,
        CASE_COUNT
    };

    public enum class EM_Number
    {
        NUM_UNDEFINED,
        NUM_SG,
        NUM_PL,
        NUM_COUNT
    };

    public enum class EM_Subparadigm
    {
        SUBPARADIGM_UNDEFINED,
        SUBPARADIGM_NOUN,
        SUBPARADIGM_SHORT_ADJ,
        SUBPARADIGM_LONG_ADJ,
        SUBPARADIGM_COMPARATIVE,
        SUBPARADIGM_INFINITIVE,
        SUBPARADIGM_PRESENT_TENSE,
        SUBPARADIGM_PAST_TENSE,
        SUBPARADIGM_IMPERATIVE,
        SUBPARADIGM_ADVERBIAL_PRESENT,
        SUBPARADIGM_ADVERBIAL_PAST,
        SUBPARADIGM_PART_PRES_ACT,
        SUBPARADIGM_PART_PRES_PASS_LONG,
        SUBPARADIGM_PART_PRES_PASS_SHORT,
        SUBPARADIGM_PART_PAST_ACT,
        SUBPARADIGM_PART_PAST_PASS_LONG,
        SUBPARADIGM_PART_PAST_PASS_SHORT,
        SUBPARADIGM_COUNT
    };

    public enum class EM_Gender
    {
        GENDER_UNDEFINED,
        GENDER_M,
        GENDER_F,
        GENDER_N,
        GENDER_COUNT
    };

    public enum class EM_Aspect
    {
        ASPECT_UNDEFINED,
        ASPECT_IMPERFECTIVE,
        ASPECT_PERFECTIVE,
        ASPECT_COUNT
    };

    public enum class EM_Person
    {
        PERSON_UNDEFINED,
        PERSON_1,
        PERSON_2,
        PERSON_3,
        PERSON_COUNT
    };

    public enum class EM_Animacy
    {
        ANIM_UNDEFINED,
        ANIM_YES,
        ANIM_NO,
        ANIM_COUNT
    };

    public enum class EM_Voice
    {
        VOICE_UNDEFINED,
        VOICE_ACTIVE,
        VOICE_PASSIVE,
        VOICE_COUNT
    };

    public enum class EM_Reflexive
    {
        REFL_UNDEFINED,
        REFL_YES,
        REFL_NO,
        REFL_COUNT
    };

    public enum class EM_AccentType
    {
        AT_UNDEFINED,
        AT_A,
        AT_A1,
        AT_B,
        AT_B1,
        AT_C,
        AT_C1,
        AT_C2,
        AT_D,
        AT_D1,
        AT_E,
        AT_F,
        AT_F1,
        AT_F2,
        AT_COUNT
    };

    public enum class EM_EndingClass
    {
        ENDING_CLASS_UNDEFINED,
        ENDING_CLASS_NOUN,
        ENDING_CLASS_ADJECTIVE,
        ENDING_CLASS_PRONOUN,
        ENDING_CLASS_SHORT_ADJECTIVE,
        ENDING_CLASS_PRESENT_TENSE,
        ENDING_CLASS_PAST_TENSE,
        ENDING_CLASS_COMPARATIVE,
        ENDING_CLASS_COUNT
    };

    public enum class EM_StressLocation
    {
        STRESS_LOCATION_UNDEFINED,
        STRESS_LOCATION_STEM,
        STRESS_LOCATION_ENDING,
        STRESS_LOCATION_COUNT
    };

    public enum class EM_StressType
    {
        STRESS_TYPE_UNDEFINED,
        STRESS_PRIMARY,
        STRESS_SECONDARY,
        STRESS_TYPE_COUNT
    };

    public enum class EM_StemAuslaut
    {
        STEM_AUSLAUT_UNDEFINED,
        STEM_AUSLAUT_SH,
        STEM_AUSLAUT_NOT_SH,
        STEM_AUSLAUT_COUNT
    };

    public enum class EM_Status
    {
        STATUS_UNDEFINED,
        STATUS_COMMON,
        STATUS_OBSOLETE,
        STATUS_RARE,
        STATUS_QUESTIONABLE,
        STATUS_INCORRECT,
        STATUS_COUNT
    };

    public enum class EM_TestResult
    {
        TEST_RESULT_UNDEFINED,
        TEST_RESULT_OK,
        TEST_RESULT_FAIL,
        TEST_RESULT_INCOMPLETE,
        TEST_RESULT_COUNT
    };

}   // namespace MainLibManaged

