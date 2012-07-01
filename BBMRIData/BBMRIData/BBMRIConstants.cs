using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBMRIData
{
    public class MF_WORKFLOWS {
        public const int PATIENT_STATE = 111;
    }

    public class MF_STATES
    {
        public const int CONSENTED = 155;     
    }
    public class MF_TYPE
    {
        public const int VALUE_LIST = 0;
        public const int OBJECT = 1;
        public const int CLASS = 2;
        public const int PROPERTY = 3;
    }
    public class MF_VLIST
    {
        public const int GENDERS = 148;
    }
    public class  MF_OTYPE
    {
        public const int BIOBANK=121;	
        public const int DATA_POINT=151;	
        public const int DATA_SOURCE=136;	
        public const int DIAGNOSIS=155;	
        public const int DOCUMENT=0;	
        public const int DOCUMENT_COLLECTION=9;
        public const int LAB_MEASUREMENT_ID=152;
        public const int PARTICIPANT=126;
        public const int PARTICIPANT_EVENT=149;
        public const int REPORT=15;
        public const int SAMPLE = 153;
    }

    public class MF_CLASS
    {
        public const int ASSIGNMENT=-100 ;//	ASSIGNMENT
        public const int BIOBANK=34	; //BIOBANK
        public const int DATA_SOURCE = 52;//DATA SOURCE
        public const int DIAGNOSIS=86;//DIAGNOSIS
        public const int INFORMED_CONSENT=77;//	DOCUMENT
        public const int INSTRUCTION=84;//DOCUMENT
        public const int LAB_MEASUREMENT_ID=81;//	LAB MEASUREMENT ID
        public const int LABORATORY_DATA_POINT=80;//	DATA POINT
        public const int OTHER_DOCUMENT=0 ; //DOCUMENT
        public const int PARTICIPANT=38; //PARTICIPANT
        public const int PARTICIPANT_EVENT=79;//	PARTICIPANT EVENT
        public const int PARTICIPANT_DATA_FILE_MULTI_PARTICIPANT=82;//	DOCUMENT
        public const int PARTICIPANT_DATA_FILE_SINGLE_PARTICIPANT=78;//	DOCUMENT
        public const int REPORT=-101; //REPORT
        public const int SAMPLE=85; //	SAMPLE
        public const int UNCLASSIFIED_DOCUMENT=1;//	DOCUMENT
    }

    public class  MF_WFLOW
    {
        public const int PARTICIPANT_DATA_FILE_STATE = 112;
    }
    public class  MF_WFLOW_STATE
    {
        public const int STORED_AWAITS_PROCESSING = 158;
        public const int CONTENT_CHECKED_WAITS_DATA_PICK = 159;
        public const int DATA_PICKED = 160;
        public const int QC_FAILED_OR_OBSOLITE = 161;
    }
    public class MF_DATA_FORMAT
    {
        public const string QPATI_TEST_EXPORT = "QPATI Test export";
    }
    public class MF_PTYPE
    {
        public const int AGE_WHEN_SAMPLE_IS_TAKEN	=	1297	; //	TEXT	
        public const int ALTERNATIVE_TITLE	=	1171	; //	TEXT	
        public const int AUTHOR	=	1235	; //	TEXT	
        public const int AUTHORED_FROM_TEMPLATE	=	1206	; //	TEXT	
        public const int AUTHORIZER	=	1236	; //	CHOOSE FROM LIST 'PROJECT STATUSES'	
        public const int BBMRI_PARTICIPANT_ID	=	1257	; //	TEXT (MULTI-LINE)	
        public const int BBMRI_PARTICIPANT_NUMBER	=	1256	; //	CHOOSE FROM LIST 'STUDY PHASES'	
        public const int BBMRI_SAMPLE_ID = 1301; //
        public const int BIOBANK	=	1111	; //	CHOOSE FROM LIST 'COUNTRIES' (MULTI-SELECT)	
        public const int CITY_OR_LOCATION	=	1115	; //	TEXT	
        public const int CONSENT_VALID_UNTIL	=	1263	; //	CHOOSE FROM LIST 'THERAPEUTIC AREAS' (MULTI-SELECT)	
        public const int COUNTER	=	1100	; //	TEXT	DOCUMENT
        public const int COUNTRY	=	1035	; //	DATE	
        public const int COUNTRY_SPECIFIC_YES_NO	=	1107	; //	DATE	
        public const int CUSTOMER_PROJECT_CORRESPONDENCE_TOPIC	=	1087	; //	DATE	
        public const int DATA_POINT	=	1267	; //	TEXT (MULTI-LINE)	
        public const int DATA_POINT_UNIQ_ID	=	1274	; //	TEXT	DOCUMENT
        public const int DATA_SOURCE	=	1167	; //	TEXT
        public const int DATE	=	1241	; //	TEXT	
        public const int DATE_OF_CONSENT	=	1262	; //	TEXT	DOCUMENT
        public const int DATE_OF_CONTACT	=	1077	; //	TEXT	DOCUMENT
        public const int DATE_OF_DIAGNOSIS	=	1293	; //	TEXT	DOCUMENT
        public const int DATE_OF_SIGNING	=	1052	; //	TEXT	DOCUMENT
        public const int DATE_OF_SUBMISSION	=	1225	; //	TEXT	DOCUMENT
        public const int DATE_OF_VALUE	=	1277	; //	TEXT	DOCUMENT
        public const int DATE_RESPONSE_RECEIVED	=	1215	; //	TEXT	DOCUMENT
        public const int DESCIPTION	=	1170	; //	DATE	
        public const int DEVELOPMENT_STATUS	=	1104	; //	TEXT	DOCUMENT
        public const int DIA_TMF_ZONE	=	1249	; //	CHOOSE FROM LIST 'PROJECT TYPES'	
        public const int DIAGNOSIS_SNOMED	=	1292	; //	TEXT	DOCUMENT
        public const int DIAGNOSIS_TEXT	=	1291	; //	TEXT	DOCUMENT
        public const int DIRECTION_OF_COMMUNICATION	=	1222	; //	DATE	DOCUMENT
        public const int DOCTITLE_IC	=	1282	; //	CHOOSE FROM LIST 'PROJECT CORRESPONDENCE TOPICS' (MULTI-SELECT)	
        public const int DOCUMENT_DATE_OPTIONAL	=	1178	; //	TEXT	DOCUMENT
        public const int DOCUMENT_TITLE	=	1049	; //	TEXT (MULTI-LINE)	
        public const int DOCUMENT_TITLE_CLOSE_OUT_VISIT_REPORT	=	1073	; //	DATE	
        public const int DOCUMENT_TITLE_GENERAL_TMF_DOCUMENT	=	1176	; //	NUMBER (INTEGER)	
        public const int DOCUMENT_TITLE_MONITORING_VISIT_LETTER	=	1071	; //	CHOOSE FROM LIST 'DEVELOPMENT STATUSES'	
        public const int DOCUMENT_TITLE_MONITORING_VISIT_LOG	=	1062	; //	BOOLEAN (YES/NO)	
        public const int DOCUMENT_TITLE_OTHER_MONITORING_DOCUMENT	=	1078	; //	TEXT (MULTI-LINE)	
        public const int DOCUMENT_TITLE_PRE_STUDY_VISIT_REPORT	=	1063	; //	CHOOSE FROM LIST 'BIOBANKS' (MULTI-SELECT)	
        public const int DOCUMENT_TITLE_SITE_CONTACT__REPORT	=	1076	; //	TEXT	
        public const int DOCUMENT_TITLE_SITE_INITIATION_VISIT_REPORT	=	1067	; //	TEXT	
        public const int DOCUMENT_TITLE_SITE_SPECIFIC_EMAIL_CORRESPONDENCE	=	1093	; //	TEXT	
        public const int DOCUMENT_TITLE_SITE_VISIT_REPORT	=	1070	; //	CHOOSE FROM LIST 'SUBFOLDERS OR TOPICS (FREE DOCUMENTS)' (MULTI-SELECT)	
        public const int DOCUMENT_TITLE_TMF_DOCUMENT_SITE_SPECIFIC	=	1189	; //	CHOOSE FROM LIST 'TRAINING DOCUMENT TYPES' (MULTI-SELECT)	
        public const int DOCUMENT_TITLE_UNCLASSIFIED_CUSTOMER_DOCUMENT	=	1055	; //	TEXT (MULTI-LINE)	
        public const int EC_OR_RA	=	1223	; //	TEXT	
        public const int EMAIL_ADDRESS	=	1113	; //	TEXT	
        public const int EMAIL_DATE	=	1085	; //	NUMBER (INTEGER)	
        public const int EMAIL_FROM	=	1083	; //	NUMBER (INTEGER)	
        public const int EMAIL_SUBJECT	=	1084	; //	NUMBER (INTEGER)	
        public const int EMAIL_TO	=	1204	; //	CHOOSE FROM LIST 'PARTICIPANTS' (MULTI-SELECT)	
        public const int ESSENTIAL_ENGLISH_TRANSLATION	=	1053	; //	TEXT	
        public const int ETHICS_COMMITTEE_OR_REGULATORY_AUTHORITY	=	1213	; //	TEXT	
        public const int FAX_NUMBER	=	1135	; //	TEXT	
        public const int FILE_TYPE	=	1265	; //	CHOOSE FROM LIST 'ROLES IN TRIAL'	
        public const int FILENAME_BEFORE_M_FILES	=	1097	; //	CHOOSE FROM LIST 'SITE PERSONNEL ROLES' (MULTI-SELECT)	
        public const int FILING_COMMENT	=	1108	; //	CHOOSE FROM LIST 'USERS'	
        public const int FIRST_NAME	=	1142	; //	TEXT	
        public const int FORMAT = 1300;
        public const int FULL_NAME	=	1146	; //	TEXT	
        public const int GENDER	=	1254	; //	TEXT	
        public const int HETU	=	1255	; //	CHOOSE FROM LIST 'PARTICIPANTS'	
        public const int IMP	=	1057	; //	TEXT	
        public const int KEYWORDS	=	26	; //	CHOOSE FROM LIST 'PARTICIPANTS' (MULTI-SELECT)	
        public const int LAB_MEASUREMENT_ID	=	1269	; //	TEXT	
        public const int LANGUAGE	=	1212	; //	TEXT	DOCUMENT
        public const int LAST_NAME	=	1143	; //	CHOOSE FROM LIST 'DATA SOURCES' (MULTI-SELECT)	
        public const int LOG_READING_DATE	=	1098	; //	TEXT	DATA SOURCE
        public const int LOGIN_ACCOUNT	=	1141	; //	TEXT	
        public const int LOCAL_PARTICIPANT_ID = 1303;
        public const int LOCAL_SAMPLE_ID = 1302;
        public const int MAILING_ADDRESS	=	1287	; //	TEXT (MULTI-LINE)	
        public const int MEANING	=	1240	; //	TEXT	
        public const int MEASUREMENT_COMMON_NAME	=	1271	; //	TEXT	
        public const int MEASUREMENT_NUMBER	=	1270	; //	TEXT	DOCUMENT
        public const int MEASUREMENT_SHORT_ABBREVIATION	=	1272	; //	DATE	DOCUMENT
        public const int MONITOR	=	1148	; //	TEXT	DOCUMENT
        public const int NO_OF_MONITORING_VISITS	=	1129	; //	TEXT	
        public const int NO_OF_PARTICIPANT_VISITS	=	1128	; //	BOOLEAN (YES/NO)	
        public const int NO_OF_PARTICIPANTS_ENROLLED	=	1127	; //	BOOLEAN (YES/NO)	
        public const int NUMERIC_VALUE	=	1276	; //	CHOOSE FROM LIST 'TEST LISTS' (MULTI-SELECT)	
        public const int ONLY_AUTHORED_WITHIN_ETMF	=	1205	; //	CHOOSE FROM LIST 'LANGUAGES' (MULTI-SELECT)	
        public const int ORGAN_SNOMED	=	1296	; //	TEXT	
        public const int ORGAN_TEXT	=	1295	; //	DATE	
        public const int PARM_CODE	=	1025	; //	DATE	
        public const int PARTICIPANT	=	1132	; //	BOOLEAN (YES/NO)	
        public const int PARTICIPANT_EVENT	=	1261	; //	CHOOSE FROM LIST 'EC/RA DIRECTIONS'	
        public const int PARTICIPANT_FILE	=	1286	; //	TEXT	
        public const int PHONE_NUMBER_DIRECT	=	1133	; //	DATE	
        public const int PHONE_NUMBER_MOBILE	=	1134	; //	CHOOSE FROM LIST 'SUBMISSION DOCUMENT TYPES'	
        public const int PRINCIPAL_INVESTIGATOR_ON_SITE	=	1145	; //	DATE	
        public const int PRINCIPAL_INVESTIGATOR	=	1250	; //	DATE	
        public const int PROJECT_CODE_SHORT_NAME	=	1024	; //	NUMBER (INTEGER)	
        public const int PROJECT_COMBINED_TITLE	=	1059	; //	TEXT	
        public const int PROJECT_STATUS	=	1027	; //	CHOOSE FROM LIST 'PARTICIPANTS'	
        public const int PROJECT_SUMMARY_PROTOCOL_ABSTRACT	=	1028	; //	CHOOSE FROM LIST 'PARTICIPANTS'	
        public const int PROJECT_TITLE_LONG_READABLE_NAME	=	1023	; //	TEXT	
        public const int PROJECT_TYPE	=	1080	; //	TEXT (MULTI-LINE)	
        public const int PROJECT_WEBSITE	=	1036	; //	DATE	
        public const int REASON	=	1239	; //	TEXT	
        public const int REASON_OF_VISIT	=	1283	; //	CHOOSE FROM LIST 'USERS'	
        public const int REQUIRES_APPROVAL	=	1218	; //	TEXT	
        public const int ROLE_IN_STUDY	=	1137	; //	CHOOSE FROM LIST 'SAE DOCUMENT TYPES' (MULTI-SELECT)	
        public const int SAE_DOCUMENT_TYPE	=	1247	; //	CHOOSE FROM LIST 'DIA TMF ZONES'	
        public const int SAE_EVENT_ONSET	=	1228	; //	CHOOSE FROM LIST 'USERS' (MULTI-SELECT)	
        public const int SAE_EVENT_RESOLVED	=	1229	; //	CHOOSE FROM LIST 'USERS' (MULTI-SELECT)	
        public const int SAE_NUMBER	=	1230	; //	NUMBER (INTEGER)	
        public const int SAMPLE	=	1281	; //	CHOOSE FROM LIST 'GENDERS'	
        public const int SAMPLE_DIAGNOSIS	=	1289	; //	TEXT	
        public const int SECTION_NUMBER	=	1168	; //	NUMBER (INTEGER)	
        public const int SHORT_COMMENT_INTO_FILENAME_OPTIONAL	=	1164	; //	TEXT	
        public const int SIGNATURE_TITLE	=	1244	; //	CHOOSE FROM LIST 'PARTICIPANT EVENTS' (MULTI-SELECT)	
        public const int SIGNED_BY	=	1243	; //	DATE	
        public const int SITE_NUMBER	=	1163	; //	DATE	
        public const int SITE_PERSONNEL_ROLE	=	1139	; //	CHOOSE FROM LIST 'DATA FILE TYPES'	
        public const int SOURCE_FILE	=	1279	; //	CHOOSE FROM LIST 'DATA POINTS' (MULTI-SELECT)	
        public const int STREET_ADDRESS	=	1122	; //	CHOOSE FROM LIST 'LAB MEASUREMENT IDS' (MULTI-SELECT)	
        public const int STUDY_PHASE	=	1031	; //	NUMBER (INTEGER)	
        public const int SUBFOLDER_OR_TOPIC	=	1119	; //	TEXT	
        public const int SUBMISSION_DATE	=	1214	; //	TEXT	
        public const int SUBMISSION_DOCUMENT_TYPE	=	1227	; //	TEXT	
        public const int TEAM_MEMBERS	=	1251	; //	NUMBER (INTEGER)	
        public const int TEST_LIST	=	1210	; //	TEXT	
        public const int THERAPEUTIC_AREA	=	1040	; //	NUMBER (REAL)	
        public const int TIMESTAMP	=	1242	; //	DATE	
        public const int TITLE	=	1169	; //	BOOLEAN (YES/NO)	
        public const int TITLE_DATA_POINT	=	1275	; //	CHOOSE FROM LIST 'DOCUMENTS'	
        public const int TITLE_DATA_SOURCE	=	1172	; //	CHOOSE FROM LIST 'SAMPLES' (MULTI-SELECT)	
        public const int TITLE_DIAGNOSIS	=	1290	; //	TEXT	
        public const int TITLE_LAB_MEASUREMENT_ID	=	1231	; //	TEXT (MULTI-LINE)	
        public const int TITLE_PARTICIPANT	=	1273	; //	CHOOSE FROM LIST 'VISIT TYPES'	
        public const int TITLE_SAMPLE	=	1294	; //	CHOOSE FROM LIST 'DOCUMENTS'	
        public const int TITLE_SITE	=	1114	; //	TEXT (MULTI-LINE)	
        public const int TITLE_SITE_VISIT	=	1126	; //	CHOOSE FROM LIST 'DIAGNOSES' (MULTI-SELECT)	
        public const int TITLE_OR_SALUTATION	=	1144	; //	TEXT	
        public const int TRAINING_DOCUMENT_TYPE	=	1121	; //	TEXT	DIAGNOSIS
        public const int VISIT_END_DATE	=	1051	; //	TEXT	
        public const int VISIT_ID_OR_TITLE_OPTIONAL	=	1125	; //	DATE	
        public const int VISIT_START_DATE	=	1050	; //	TEXT	
        public const int VISIT_TYPE	=	1285	; //	TEXT	
        public const int WITHIN_BOUNDARIES	=	1278	; //	TEXT	
        public const int YOB	=	1252	; //	NUMBER (REAL)	

    }


}
