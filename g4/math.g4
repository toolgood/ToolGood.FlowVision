grammar math;

prog: expr EOF;

expr:
	expr '.' ISNUMBER '(' ')'									# ISNUMBER_fun
	| expr '.' ISTEXT '(' ')'									# ISTEXT_fun
	| expr '.' ISNONTEXT '(' ')'								# ISNONTEXT_fun
	| expr '.' ISLOGICAL '(' ')'								# ISLOGICAL_fun
	| expr '.' ISEVEN '(' ')'									# ISEVEN_fun
	| expr '.' ISODD '(' ')'									# ISODD_fun
	| expr '.' ISERROR '(' expr? ')'							# ISERROR_fun
	| expr '.' ISNULL '(' expr? ')'								# ISNULL_fun
	| expr '.' ISNULLORERROR '(' expr? ')'						# ISNULLORERROR_fun
	| expr '.' INT '(' ')'										# INT_fun
	| expr '.' ASC '(' ')'										# ASC_fun
	| expr '.' JIS '(' ')'										# JIS_fun
	| expr '.' CHAR '(' ')'										# CHAR_fun
	| expr '.' CLEAN '(' ')'									# CLEAN_fun
	| expr '.' CODE '(' ')'										# CODE_fun
	| expr '.' CONCATENATE '(' (expr (',' expr)*)? ')'			# CONCATENATE_fun
	| expr '.' EXACT '(' expr ')'								# EXACT_fun
	| expr '.' FIND '(' expr (',' expr)? ')'					# FIND_fun
	| expr '.' LEFT '(' expr? ')'								# LEFT_fun
	| expr '.' LEN '(' ')'										# LEN_fun
	| expr '.' LOWER '(' ')'									# LOWER_fun
	| expr '.' MID '(' expr ',' expr ')'						# MID_fun
	| expr '.' PROPER '(' ')'									# PROPER_fun
	| expr '.' REPLACE '(' expr ',' expr (',' expr)? ')'		# REPLACE_fun
	| expr '.' REPT '(' expr ')'								# REPT_fun
	| expr '.' RIGHT '(' expr? ')'								# RIGHT_fun
	| expr '.' RMB '(' ')'										# RMB_fun
	| expr '.' SEARCH '(' expr (',' expr)? ')'					# SEARCH_fun
	| expr '.' SUBSTITUTE '(' expr ',' expr (',' expr)? ')'		# SUBSTITUTE_fun
	| expr '.' T '(' ')'										# T_fun
	| expr '.' TEXT '(' expr ')'								# TEXT_fun
	| expr '.' TRIM '(' ')'										# TRIM_fun
	| expr '.' UPPER '(' ')'									# UPPER_fun
	| expr '.' VALUE '(' ')'									# VALUE_fun
	| expr '.' DATEVALUE '(' ')'								# DATEVALUE_fun
	| expr '.' TIMEVALUE '(' ')'								# TIMEVALUE_fun
	| expr '.' YEAR ('(' ')')?									# YEAR_fun
	| expr '.' MONTH ('(' ')')?									# MONTH_fun
	| expr '.' DAY ('(' ')')?									# DAY_fun
	| expr '.' HOUR ('(' ')')?									# HOUR_fun
	| expr '.' MINUTE ('(' ')')?								# MINUTE_fun
	| expr '.' SECOND ('(' ')')?								# SECOND_fun
	| expr '.' REGEXREPALCE '(' expr ',' expr ')'				# REGEXREPALCE_fun
	| expr '.' ISREGEX '(' expr ')'								# ISREGEX_fun
	| expr '.' TRIMSTART '(' expr? ')'							# TRIMSTART_fun
	| expr '.' TRIMEND '(' expr? ')'							# TRIMEND_fun
	| expr '.' INDEXOF '(' expr (',' expr (',' expr)?)? ')'		# INDEXOF_fun
	| expr '.' LASTINDEXOF '(' expr (',' expr (',' expr)?)? ')'	# LASTINDEXOF_fun
	| expr '.' SPLIT '(' expr ')'								# SPLIT_fun
	| expr '.' JOIN '(' expr (',' expr)* ')'					# JOIN_fun
	| expr '.' SUBSTRING '(' expr (',' expr)? ')'				# SUBSTRING_fun
	| expr '.' STARTSWITH '(' expr (',' expr)? ')'				# STARTSWITH_fun
	| expr '.' ENDSWITH '(' expr (',' expr)? ')'				# ENDSWITH_fun
	| expr '.' ISNULLOREMPTY '(' ')'							# ISNULLOREMPTY_fun
	| expr '.' ISNULLORWHITESPACE '(' ')'						# ISNULLORWHITESPACE_fun
	| expr '.' REMOVESTART '(' expr (',' expr)? ')'				# REMOVESTART_fun
	| expr '.' REMOVEEND '(' expr (',' expr)? ')'				# REMOVEEND_fun
	| expr '.' JSON '(' ')'										# JSON_fun
	| expr '.' LOOKUP '(' expr ',' expr ')'						# LOOKUP_fun
	| expr '.' ADDYEARS '(' expr ')'							# ADDYEARS_fun
	| expr '.' ADDMONTHS '(' expr ')'							# ADDMONTHS_fun
	| expr '.' ADDDAYS '(' expr ')'								# ADDDAYS_fun
	| expr '.' ADDHOURS '(' expr ')'							# ADDHOURS_fun
	| expr '.' ADDMINUTES '(' expr ')'							# ADDMINUTES_fun
	| expr '.' ADDSECONDS '(' expr ')'							# ADDSECONDS_fun
	| expr '.' IN '(' expr ')'									# IN_fun
	| expr '.' HAS '(' expr ')'									# HAS_fun
	| expr '[' expr ']'											# GetJsonValue_fun
	| expr '[' parameter2 ']'									# GetJsonValue_fun
	| expr '.' parameter2										# GetJsonValue_fun
	//  
	| '(' expr ')'												# Bracket_fun
	| '!' expr													# NOT_fun
	| expr '%'													# Percentage_fun
	| expr op = ('*' | '/' | '%') expr							# MulDiv_fun
	| expr op = ('+' | '-' | '&') expr							# AddSub_fun
	| expr op = ('>' | '>=' | '<' | '<=') expr					# Judge_fun
	| expr op = ('=' | '==' | '===' | '!==' | '!=' | '<>') expr	# Judge_fun
	| expr op = ('&&' | AND) expr								# AndOr_fun
	| expr op = ('||' | OR) expr								# AndOr_fun
	| expr '?' expr ':' expr									# IF_fun
	//  
	| IF '(' expr ',' expr (',' expr)? ')'					# IF_fun
	| ISNUMBER '(' expr ')'									# ISNUMBER_fun
	| ISTEXT '(' expr ')'									# ISTEXT_fun
	| ISERROR '(' expr (',' expr)? ')'						# ISERROR_fun
	| ISNONTEXT '(' expr ')'								# ISNONTEXT_fun
	| ISLOGICAL '(' expr ')'								# ISLOGICAL_fun
	| ISEVEN '(' expr ')'									# ISEVEN_fun
	| ISODD '(' expr ')'									# ISODD_fun
	| IFERROR '(' expr ',' expr (',' expr)? ')'				# IFERROR_fun
	| ISNULL '(' expr (',' expr)? ')'						# ISNULL_fun
	| ISNULLORERROR '(' expr (',' expr)? ')'				# ISNULLORERROR_fun
	| AND '(' expr (',' expr)* ')'							# AND_fun
	| OR '(' expr (',' expr)* ')'							# OR_fun
	| NOT '(' expr ')'										# NOT_fun
	| TRUE ('(' ')')?										# TRUE_fun
	| FALSE ('(' ')')?										# FALSE_fun
	| E ('(' ')')?											# E_fun
	| PI ('(' ')')?											# PI_fun
	| ABS '(' expr ')'										# ABS_fun
	| QUOTIENT '(' expr (',' expr) ')'						# QUOTIENT_fun
	| MOD '(' expr (',' expr) ')'							# MOD_fun
	| SIGN '(' expr ')'										# SIGN_fun
	| SQRT '(' expr ')'										# SQRT_fun
	| TRUNC '(' expr ')'									# TRUNC_fun
	| INT '(' expr ')'										# INT_fun
	| GCD '(' expr (',' expr)+ ')'							# GCD_fun
	| LCM '(' expr (',' expr)+ ')'							# LCM_fun
	| COMBIN '(' expr ',' expr ')'							# COMBIN_fun
	| PERMUT '(' expr ',' expr ')'							# PERMUT_fun
	| DEGREES '(' expr ')'									# DEGREES_fun
	| RADIANS '(' expr ')'									# RADIANS_fun
	| COS '(' expr ')'										# COS_fun
	| COSH '(' expr ')'										# COSH_fun
	| SIN '(' expr ')'										# SIN_fun
	| SINH '(' expr ')'										# SINH_fun
	| TAN '(' expr ')'										# TAN_fun
	| TANH '(' expr ')'										# TANH_fun
	| ACOS '(' expr ')'										# ACOS_fun
	| ACOSH '(' expr ')'									# ACOSH_fun
	| ASIN '(' expr ')'										# ASIN_fun
	| ASINH '(' expr ')'									# ASINH_fun
	| ATAN '(' expr ')'										# ATAN_fun
	| ATANH '(' expr ')'									# ATANH_fun
	| ATAN2 '(' expr ',' expr ')'							# ATAN2_fun
	| ROUND '(' expr (',' expr)? ')'						# ROUND_fun
	| ROUNDDOWN '(' expr ',' expr ')'						# ROUNDDOWN_fun
	| ROUNDUP '(' expr ',' expr ')'							# ROUNDUP_fun
	| CEILING '(' expr (',' expr)? ')'						# CEILING_fun
	| FLOOR '(' expr (',' expr)? ')'						# FLOOR_fun
	| EVEN '(' expr ')'										# EVEN_fun
	| ODD '(' expr ')'										# ODD_fun
	| MROUND '(' expr ',' expr ')'							# MROUND_fun
	| RAND '(' ')'											# RAND_fun
	| RANDBETWEEN '(' expr ',' expr ')'						# RANDBETWEEN_fun
	| FACT '(' expr ')'										# FACT_fun
	| FACTDOUBLE '(' expr ')'								# FACTDOUBLE_fun
	| POWER '(' expr ',' expr ')'							# POWER_fun
	| EXP '(' expr ')'										# EXP_fun
	| LN '(' expr ')'										# LN_fun
	| LOG '(' expr (',' expr)? ')'							# LOG_fun
	| LOG10 '(' expr ')'									# LOG10_fun
	| MULTINOMIAL '(' expr (',' expr)* ')'					# MULTINOMIAL_fun
	| PRODUCT '(' expr (',' expr)* ')'						# PRODUCT_fun
	| SQRTPI '(' expr ')'									# SQRTPI_fun
	| SUMSQ '(' expr (',' expr)* ')'						# SUMSQ_fun
	| ASC '(' expr ')'										# ASC_fun
	| JIS '(' expr ')'										# JIS_fun
	| CHAR '(' expr ')'										# CHAR_fun
	| CLEAN '(' expr ')'									# CLEAN_fun
	| CODE '(' expr ')'										# CODE_fun
	| CONCATENATE '(' expr (',' expr)* ')'					# CONCATENATE_fun
	| EXACT '(' expr ',' expr ')'							# EXACT_fun
	| FIND '(' expr ',' expr (',' expr)? ')'				# FIND_fun
	| FIXED '(' expr (',' expr (',' expr)?)? ')'			# FIXED_fun
	| LEFT '(' expr (',' expr)? ')'							# LEFT_fun
	| LEN '(' expr ')'										# LEN_fun
	| LOWER '(' expr ')'									# LOWER_fun
	| MID '(' expr ',' expr ',' expr ')'					# MID_fun
	| PROPER '(' expr ')'									# PROPER_fun
	| REPLACE '(' expr ',' expr ',' expr (',' expr)? ')'	# REPLACE_fun
	| REPT '(' expr ',' expr ')'							# REPT_fun
	| RIGHT '(' expr (',' expr)? ')'						# RIGHT_fun
	| RMB '(' expr ')'										# RMB_fun
	| SEARCH '(' expr ',' expr (',' expr)? ')'				# SEARCH_fun
	| SUBSTITUTE '(' expr ',' expr ',' expr (',' expr)? ')'	# SUBSTITUTE_fun
	| T '(' expr ')'										# T_fun
	| TEXT '(' expr ',' expr ')'							# TEXT_fun
	| TRIM '(' expr ')'										# TRIM_fun
	| UPPER '(' expr ')'									# UPPER_fun
	| VALUE '(' expr ')'									# VALUE_fun
	| DATEVALUE '(' expr ')'								# DATEVALUE_fun
	| TIMEVALUE '(' expr ')'								# TIMEVALUE_fun
	| DATE '(' expr ',' expr ',' expr (
		',' expr (',' expr (',' expr)?)?
	)? ')'														# DATE_fun
	| TIME '(' expr ',' expr (',' expr)? ')'					# TIME_fun
	| NOW '(' ')'												# NOW_fun
	| TODAY '(' ')'												# TODAY_fun
	| YEAR '(' expr ')'											# YEAR_fun
	| MONTH '(' expr ')'										# MONTH_fun
	| DAY '(' expr ')'											# DAY_fun
	| HOUR '(' expr ')'											# HOUR_fun
	| MINUTE '(' expr ')'										# MINUTE_fun
	| SECOND '(' expr ')'										# SECOND_fun
	| WEEKDAY '(' expr (',' expr)? ')'							# WEEKDAY_fun
	| DATEDIF '(' expr ',' expr ',' expr ')'					# DATEDIF_fun
	| DAYS360 '(' expr ',' expr (',' expr)? ')'					# DAYS360_fun
	| EDATE '(' expr ',' expr ')'								# EDATE_fun
	| EOMONTH '(' expr ',' expr ')'								# EOMONTH_fun
	| NETWORKDAYS '(' expr ',' expr (',' expr)? ')'				# NETWORKDAYS_fun
	| WORKDAY '(' expr ',' expr (',' expr)? ')'					# WORKDAY_fun
	| WEEKNUM '(' expr (',' expr)? ')'							# WEEKNUM_fun
	| MAX '(' expr (',' expr)+ ')'								# MAX_fun
	| MEDIAN '(' expr (',' expr)+ ')'							# MEDIAN_fun
	| MIN '(' expr (',' expr)+ ')'								# MIN_fun
	| QUARTILE '(' expr ',' expr ')'							# QUARTILE_fun
	| MODE '(' expr (',' expr)* ')'								# MODE_fun
	| LARGE '(' expr ',' expr ')'								# LARGE_fun
	| SMALL '(' expr ',' expr ')'								# SMALL_fun
	| PERCENTILE '(' expr ',' expr ')'							# PERCENTILE_fun
	| PERCENTRANK '(' expr ',' expr ')'							# PERCENTRANK_fun
	| AVERAGE '(' expr (',' expr)* ')'							# AVERAGE_fun
	| AVERAGEIF '(' expr ',' expr (',' expr)? ')'				# AVERAGEIF_fun
	| GEOMEAN '(' expr (',' expr)* ')'							# GEOMEAN_fun
	| HARMEAN '(' expr (',' expr)* ')'							# HARMEAN_fun
	| COUNT '(' expr (',' expr)* ')'							# COUNT_fun
	| COUNTIF '(' expr (',' expr)* ')'							# COUNTIF_fun
	| SUM '(' expr (',' expr)* ')'								# SUM_fun
	| SUMIF '(' expr ',' expr (',' expr)? ')'					# SUMIF_fun
	| AVEDEV '(' expr (',' expr)* ')'							# AVEDEV_fun
	| STDEV '(' expr (',' expr)* ')'							# STDEV_fun
	| STDEVP '(' expr (',' expr)* ')'							# STDEVP_fun
	| DEVSQ '(' expr (',' expr)* ')'							# DEVSQ_fun
	| VAR '(' expr (',' expr)* ')'								# VAR_fun
	| VARP '(' expr (',' expr)* ')'								# VARP_fun
	| NORMDIST '(' expr ',' expr ',' expr ',' expr ')'			# NORMDIST_fun
	| NORMINV '(' expr ',' expr ',' expr ')'					# NORMINV_fun
	| NORMSDIST '(' expr ')'									# NORMSDIST_fun
	| NORMSINV '(' expr ')'										# NORMSINV_fun
	| BETADIST '(' expr ',' expr ',' expr ')'					# BETADIST_fun
	| BETAINV '(' expr ',' expr ',' expr ')'					# BETAINV_fun
	| BINOMDIST '(' expr ',' expr ',' expr ',' expr ')'			# BINOMDIST_fun
	| EXPONDIST '(' expr ',' expr ',' expr ')'					# EXPONDIST_fun
	| FDIST '(' expr ',' expr ',' expr ')'						# FDIST_fun
	| FINV '(' expr ',' expr ',' expr ')'						# FINV_fun
	| FISHER '(' expr ')'										# FISHER_fun
	| FISHERINV '(' expr ')'									# FISHERINV_fun
	| GAMMADIST '(' expr ',' expr ',' expr ',' expr ')'			# GAMMADIST_fun
	| GAMMAINV '(' expr ',' expr ',' expr ')'					# GAMMAINV_fun
	| GAMMALN '(' expr ')'										# GAMMALN_fun
	| HYPGEOMDIST '(' expr ',' expr ',' expr ',' expr ')'		# HYPGEOMDIST_fun
	| LOGINV '(' expr ',' expr ',' expr ')'						# LOGINV_fun
	| LOGNORMDIST '(' expr ',' expr ',' expr ')'				# LOGNORMDIST_fun
	| NEGBINOMDIST '(' expr ',' expr ',' expr ')'				# NEGBINOMDIST_fun
	| POISSON '(' expr ',' expr ',' expr ')'					# POISSON_fun
	| TDIST '(' expr ',' expr ',' expr ')'						# TDIST_fun
	| TINV '(' expr ',' expr ')'								# TINV_fun
	| WEIBULL '(' expr ',' expr ',' expr ',' expr ')'			# WEIBULL_fun
	| REGEXREPALCE '(' expr ',' expr ',' expr ')'				# REGEXREPALCE_fun
	| ISREGEX '(' expr ',' expr ')'								# ISREGEX_fun
	| TRIMSTART '(' expr (',' expr)? ')'						# TRIMSTART_fun
	| TRIMEND '(' expr (',' expr)? ')'							# TRIMEND_fun
	| INDEXOF '(' expr ',' expr (',' expr (',' expr)?)? ')'		# INDEXOF_fun
	| LASTINDEXOF '(' expr ',' expr (',' expr (',' expr)?)? ')'	# LASTINDEXOF_fun
	| SPLIT '(' expr ',' expr ')'								# SPLIT_fun
	| JOIN '(' expr (',' expr)+ ')'								# JOIN_fun
	| SUBSTRING '(' expr ',' expr (',' expr)? ')'				# SUBSTRING_fun
	| STARTSWITH '(' expr ',' expr (',' expr)? ')'				# STARTSWITH_fun
	| ENDSWITH '(' expr ',' expr (',' expr)? ')'				# ENDSWITH_fun
	| ISNULLOREMPTY '(' expr ')'								# ISNULLOREMPTY_fun
	| ISNULLORWHITESPACE '(' expr ')'							# ISNULLORWHITESPACE_fun
	| REMOVESTART '(' expr (',' expr (',' expr)?)? ')'			# REMOVESTART_fun
	| REMOVEEND '(' expr (',' expr (',' expr)?)? ')'			# REMOVEEND_fun
	| JSON '(' expr ')'											# JSON_fun
	| LOOKUP '(' expr ',' expr (',' expr)? ')'					# LOOKUP_fun
	| ERROR '(' expr? ')'										# ERROR_fun
	| PARAM '(' expr (',' expr)? ')'							# PARAM_fun
	| ADDYEARS '(' expr ',' expr ')'							# ADDYEARS_fun
	| ADDMONTHS '(' expr ',' expr ')'							# ADDMONTHS_fun
	| ADDDAYS '(' expr ',' expr ')'								# ADDDAYS_fun
	| ADDHOURS '(' expr ',' expr ')'							# ADDHOURS_fun
	| ADDMINUTES '(' expr ',' expr ')'							# ADDMINUTES_fun
	| ADDSECONDS '(' expr ',' expr ')'							# ADDSECONDS_fun
	| '{' arrayJson (',' arrayJson)* ','* '}'					# ArrayJson_fun
	| '[' PARAMETER ']'											# PARAMETER_fun
	| '[' expr ']'												# PARAMETER_fun
	| PARAMETER													# PARAMETER_fun
	| PARAMETER2												# PARAMETER_fun
	| num unit?													# NUM_fun
	| STRING													# STRING_fun
	| NULL														# NULL_fun;

num: '-'? NUM;
unit: UNIT | T;

arrayJson: ((NUM | STRING | parameter2) ':')? expr;

parameter2:
	E
	| IF
	| IFERROR
	| ISNUMBER
	| ISTEXT
	| ISERROR
	| ISNONTEXT
	| ISLOGICAL
	| ISEVEN
	| ISODD
	| ISNULL
	| ISNULLORERROR
	| AND
	| OR
	| NOT
	| TRUE
	| FALSE
	| PI
	| ABS
	| QUOTIENT
	| MOD
	| SIGN
	| SQRT
	| TRUNC
	| INT
	| GCD
	| LCM
	| COMBIN
	| PERMUT
	| DEGREES
	| RADIANS
	| COS
	| COSH
	| SIN
	| SINH
	| TAN
	| TANH
	| ACOS
	| ACOSH
	| ASIN
	| ASINH
	| ATAN
	| ATANH
	| ATAN2
	| ROUND
	| ROUNDDOWN
	| ROUNDUP
	| CEILING
	| FLOOR
	| EVEN
	| ODD
	| MROUND
	| RAND
	| RANDBETWEEN
	| FACT
	| FACTDOUBLE
	| POWER
	| EXP
	| LN
	| LOG
	| LOG10
	| MULTINOMIAL
	| PRODUCT
	| SQRTPI
	| SUMSQ
	| ASC
	| JIS
	| CHAR
	| CLEAN
	| CODE
	| CONCATENATE
	| EXACT
	| FIND
	| FIXED
	| LEFT
	| LEN
	| LOWER
	| MID
	| PROPER
	| REPLACE
	| REPT
	| RIGHT
	| RMB
	| SEARCH
	| SUBSTITUTE
	| T
	| TEXT
	| TRIM
	| UPPER
	| VALUE
	| DATEVALUE
	| TIMEVALUE
	| DATE
	| TIME
	| NOW
	| TODAY
	| YEAR
	| MONTH
	| DAY
	| HOUR
	| MINUTE
	| SECOND
	| WEEKDAY
	| DATEDIF
	| DAYS360
	| EDATE
	| EOMONTH
	| NETWORKDAYS
	| WORKDAY
	| WEEKNUM
	| MAX
	| MEDIAN
	| MIN
	| QUARTILE
	| MODE
	| LARGE
	| SMALL
	| PERCENTILE
	| PERCENTRANK
	| AVERAGE
	| AVERAGEIF
	| GEOMEAN
	| HARMEAN
	| COUNT
	| COUNTIF
	| SUM
	| SUMIF
	| AVEDEV
	| STDEV
	| STDEVP
	| DEVSQ
	| VAR
	| VARP
	| NORMDIST
	| NORMINV
	| NORMSDIST
	| NORMSINV
	| BETADIST
	| BETAINV
	| BINOMDIST
	| EXPONDIST
	| FDIST
	| FINV
	| FISHER
	| FISHERINV
	| GAMMADIST
	| GAMMAINV
	| GAMMALN
	| HYPGEOMDIST
	| LOGINV
	| LOGNORMDIST
	| NEGBINOMDIST
	| POISSON
	| TDIST
	| TINV
	| WEIBULL
	| REGEXREPALCE
	| ISREGEX
	| TRIMSTART
	| TRIMEND
	| INDEXOF
	| LASTINDEXOF
	| SPLIT
	| JOIN
	| SUBSTRING
	| STARTSWITH
	| ENDSWITH
	| ISNULLOREMPTY
	| ISNULLORWHITESPACE
	| REMOVESTART
	| REMOVEEND
	| JSON
	| LOOKUP
	| ERROR
	| NULL
	| IN
	| HAS
	| UNIT
	| PARAM
	| ADDYEARS
	| ADDMONTHS
	| ADDDAYS
	| ADDHOURS
	| ADDMINUTES
	| ADDSECONDS
	| PARAMETER;

SUB: '-';
NUM:
	'0' ('.' [0-9]+)?
	| [1-9][0-9]* ('.' [0-9]+)?
	| ('0' ('.' [0-9]+)? | [1-9][0-9]* ('.' [0-9]+)?) 'E' [+-]? [0-9][0-9]?;
STRING:
	'\'' (~'\'' | '\\\'')* '\''
	| '"' ( ~'"' | '\\"')* '"'
	| '`' ( ~'`' | '\\`')* '`';
NULL: 'NULL';
ERROR: 'ERROR';

UNIT:
	'M'
	| 'KM'
	| 'DM'
	| 'CM'
	| 'MM'
	| 'M2'
	| 'KM2'
	| 'DM2'
	| 'CM2'
	| 'MM2'
	| 'M3'
	| 'KM3'
	| 'DM3'
	| 'CM3'
	| 'MM3'
	| 'L'
	| 'ML'
	| 'G'
	| 'KG';
//  
IF: 'IF';
IFERROR: 'IFERROR';
ISNUMBER: 'ISNUMBER';
ISTEXT: 'ISTEXT';
ISERROR: 'ISERROR';
ISNONTEXT: 'ISNONTEXT';
ISLOGICAL: 'ISLOGICAL';
ISEVEN: 'ISEVEN';
ISODD: 'ISODD';
ISNULL: 'ISNULL';
ISNULLORERROR: 'ISNULLORERROR';
AND: 'AND';
OR: 'OR';
NOT: 'NOT';
TRUE: 'TRUE';
FALSE: 'FALSE';
//  
E: 'E';
PI: 'PI';
ABS: 'ABS';
QUOTIENT: 'QUOTIENT';
MOD: 'MOD';
SIGN: 'SIGN';
SQRT: 'SQRT';
TRUNC: 'TRUNC';
INT: 'INT';
GCD: 'GCD';
LCM: 'LCM';
COMBIN: 'COMBIN';
PERMUT: 'PERMUT';
DEGREES: 'DEGREES';
RADIANS: 'RADIANS';
COS: 'COS';
COSH: 'COSH';
SIN: 'SIN';
SINH: 'SINH';
TAN: 'TAN';
TANH: 'TANH';
ACOS: 'ACOS';
ACOSH: 'ACOSH';
ASIN: 'ASIN';
ASINH: 'ASINH';
ATAN: 'ATAN';
ATANH: 'ATANH';
ATAN2: 'ATAN2';
ROUND: 'ROUND';
ROUNDDOWN: 'ROUNDDOWN';
ROUNDUP: 'ROUNDUP';
CEILING: 'CEILING';
FLOOR: 'FLOOR';
EVEN: 'EVEN';
ODD: 'ODD';
MROUND: 'MROUND';
RAND: 'RAND';
RANDBETWEEN: 'RANDBETWEEN';
FACT: 'FACT';
FACTDOUBLE: 'FACTDOUBLE';
POWER: 'POWER';
EXP: 'EXP';
LN: 'LN';
LOG: 'LOG';
LOG10: 'LOG10';
MULTINOMIAL: 'MULTINOMIAL';
PRODUCT: 'PRODUCT';
SQRTPI: 'SQRTPI';
SUMSQ: 'SUMSQ';
//  
ASC: 'ASC';
JIS: 'JIS' | 'WIDECHAR';
CHAR: 'CHAR';
CLEAN: 'CLEAN';
CODE: 'CODE';
CONCATENATE: 'CONCATENATE';
EXACT: 'EXACT';
FIND: 'FIND';
FIXED: 'FIXED';
LEFT: 'LEFT';
LEN: 'LEN';
LOWER: 'LOWER' | 'TOLOWER';
MID: 'MID';
PROPER: 'PROPER';
REPLACE: 'REPLACE';
REPT: 'REPT';
RIGHT: 'RIGHT';
RMB: 'RMB';
SEARCH: 'SEARCH';
SUBSTITUTE: 'SUBSTITUTE';
T: 'T';
TEXT: 'TEXT';
TRIM: 'TRIM';
UPPER: 'UPPER' | 'TOUPPER';
VALUE: 'VALUE';
//  
DATEVALUE: 'DATEVALUE';
TIMEVALUE: 'TIMEVALUE';
DATE: 'DATE';
TIME: 'TIME';
NOW: 'NOW';
TODAY: 'TODAY';
YEAR: 'YEAR';
MONTH: 'MONTH';
DAY: 'DAY';
HOUR: 'HOUR';
MINUTE: 'MINUTE';
SECOND: 'SECOND';
WEEKDAY: 'WEEKDAY';
DATEDIF: 'DATEDIF';
DAYS360: 'DAYS360';
EDATE: 'EDATE';
EOMONTH: 'EOMONTH';
NETWORKDAYS: 'NETWORKDAYS';
WORKDAY: 'WORKDAY';
WEEKNUM: 'WEEKNUM';
//  
MAX: 'MAX';
MEDIAN: 'MEDIAN';
MIN: 'MIN';
QUARTILE: 'QUARTILE';
MODE: 'MODE';
LARGE: 'LARGE';
SMALL: 'SMALL';
PERCENTILE: 'PERCENTILE';
PERCENTRANK: 'PERCENTRANK';
AVERAGE: 'AVERAGE';
AVERAGEIF: 'AVERAGEIF';
GEOMEAN: 'GEOMEAN';
HARMEAN: 'HARMEAN';
COUNT: 'COUNT';
COUNTIF: 'COUNTIF';
SUM: 'SUM';
SUMIF: 'SUMIF';
AVEDEV: 'AVEDEV';
STDEV: 'STDEV';
STDEVP: 'STDEVP';
DEVSQ: 'DEVSQ';
VAR: 'VAR';
VARP: 'VARP';
NORMDIST: 'NORMDIST';
NORMINV: 'NORMINV';
NORMSDIST: 'NORMSDIST';
NORMSINV: 'NORMSINV';
BETADIST: 'BETADIST';
BETAINV: 'BETAINV';
BINOMDIST: 'BINOMDIST';
EXPONDIST: 'EXPONDIST';
FDIST: 'FDIST';
FINV: 'FINV';
FISHER: 'FISHER';
FISHERINV: 'FISHERINV';
GAMMADIST: 'GAMMADIST';
GAMMAINV: 'GAMMAINV';
GAMMALN: 'GAMMALN';
HYPGEOMDIST: 'HYPGEOMDIST';
LOGINV: 'LOGINV';
LOGNORMDIST: 'LOGNORMDIST';
NEGBINOMDIST: 'NEGBINOMDIST';
POISSON: 'POISSON';
TDIST: 'TDIST';
TINV: 'TINV';
WEIBULL: 'WEIBULL';
//  
REGEXREPALCE: 'REGEXREPALCE';
ISREGEX: 'ISREGEX' | 'ISMATCH';
TRIMSTART: 'TRIMSTART' | 'LTRIM';
TRIMEND: 'TRIMEND' | 'RTRIM';
INDEXOF: 'INDEXOF';
LASTINDEXOF: 'LASTINDEXOF';
SPLIT: 'SPLIT';
JOIN: 'JOIN';
SUBSTRING: 'SUBSTRING';
STARTSWITH: 'STARTSWITH';
ENDSWITH: 'ENDSWITH';
ISNULLOREMPTY: 'ISNULLOREMPTY';
ISNULLORWHITESPACE: 'ISNULLORWHITESPACE';
REMOVESTART: 'REMOVESTART';
REMOVEEND: 'REMOVEEND';
JSON: 'JSON';
LOOKUP: 'LOOKUP';
IN: 'ISIN' | 'IN';
HAS: 'HAS' | 'CONTAINS';
PARAM: 'PARAM' | 'PARAMETER';
ADDYEARS: 'ADDYEARS';
ADDMONTHS: 'ADDMONTHS';
ADDDAYS: 'ADDDAYS';
ADDHOURS: 'ADDHOURS';
ADDMINUTES: 'ADDMINUTES';
ADDSECONDS: 'ADDSECONDS';

PARAMETER: ([A-Z_] | FullWidthLetter) (
		[A-Z0-9_]
		| FullWidthLetter
	)*;
PARAMETER2:
	'#' (~('#'))+ '#'
	| '@' ([A-Z_] | FullWidthLetter) (
		[A-Z0-9_]
		| FullWidthLetter
	)*;

fragment FullWidthLetter:
	'\u00c0' ..'\u00d6'
	| '\u00d8' ..'\u00f6'
	| '\u00f8' ..'\u00ff'
	| '\u0100' ..'\u1fff'
	| '\u2c00' ..'\u2fff'
	| '\u3040' ..'\u318f'
	| '\u3300' ..'\u337f'
	| '\u3400' ..'\u3fff'
	| '\u4e00' ..'\u9fff'
	| '\ua000' ..'\ud7ff'
	| '\uf900' ..'\ufaff'
	| '\uff00' ..'\ufff0';
// | '\u10000'..'\u1F9FF' //not support four bytes chars | '\u20000'..'\u2FA1F'
WS: [ \t\r\n\u000C]+ -> skip;
COMMENT: '/*' .*? '*/' -> skip;
LINE_COMMENT: '//' ~[\r\n]* -> skip;