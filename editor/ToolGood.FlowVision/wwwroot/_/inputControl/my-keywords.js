ace_keywords2 = [
    { key: "if", value: "判断真假" },
    { key: "ifError", value: "判断是否出错" },
    { key: "isError", value: "判断是否出错" },
    { key: "isNull", value: "判断是否为空" },
    { key: "isNullOrError", value: "判断是否空或错误" },
    { key: "isNumber", value: "判断是否数值" },
    { key: "isText", value: "判断是否文字" },
    { key: "isNonText", value: "判断是否非文字" },
    { key: "isLogical", value: "判断是否逻辑值" },
    { key: "isEven", value: "判断是否偶数" },
    { key: "isOdd", value: "判断是否奇数" },
    { key: "and", value: "并操作" },
    { key: "or", value: "或操作" },
    { key: "not", value: "求反" },
    { key: "true", value: "逻辑值TRUE" },
    { key: "false", value: "逻辑值FALSE" },
    { key: "e", value: "常数e" },
    { key: "pi", value: "常数pi" },
    { key: "abs", value: "绝对值" },
    { key: "quotient", value: "商的整数部分" },
    { key: "mod", value: "余数" },
    { key: "sign", value: "数值的符号" },
    { key: "sqrt", value: "正平方根" },
    { key: "trunc", value: "数值截尾取整" },
    { key: "int", value: "取整数" },
    { key: "gcd", value: "最大公约数" },
    { key: "lcm", value: "最小公倍数" },
    { key: "combin", value: "计算集合组合数" },
    { key: "permut", value: "计算集合排列数" },
    { key: "fixed", value: "数值转文本带小数" },
    { key: "degrees", value: "弧度转度" },
    { key: "radians", value: "度转弧度" },
    { key: "cos", value: "余弦值" },
    { key: "cosh", value: "双曲余弦值" },
    { key: "sin", value: "正弦值" },
    { key: "sinh", value: "双曲正弦值" },
    { key: "tan", value: "正切值" },
    { key: "tanh", value: "双曲正切值" },
    { key: "acos", value: "反余弦值" },
    { key: "acosh", value: "反双曲余弦值" },
    { key: "asin", value: "反正弦值" },
    { key: "asinh", value: "反双曲正弦值" },
    { key: "atan", value: "反正切值" },
    { key: "atanh", value: "反双曲正切值" },
    { key: "atan2", value: "反正切" },
    { key: "round", value: "四舍五入" },
    { key: "roundDown", value: "向下舍入数值" },
    { key: "roundUp", value: "向上舍入数值" },
    { key: "ceiling", value: "向上舍入" },
    { key: "floor", value: "向下舍入" },
    { key: "even", value: "向上取偶数" },
    { key: "odd", value: "向上取奇数" },
    { key: "mRound", value: "四舍五入" },
    { key: "rand", value: "0到1的随机数" },
    { key: "randBetween", value: "随机整数" },
    { key: "fact", value: "阶乘" },
    { key: "factdouble", value: "双倍阶乘" },
    { key: "power", value: "数的乘幂" },
    { key: "exp", value: "e的指定数乘幂" },
    { key: "ln", value: "自然对数" },
    { key: "log", value: "对数" },
    { key: "log10", value: "10的对数" },
    { key: "multiNomial", value: "多项式分布" },
    { key: "product", value: "所有数值相乘,乘积值" },
    { key: "sqrtPi", value: "数与PI的乘积的平方根" },
    { key: "sumQq", value: "平方和" },
    { key: "asc", value: "转半角" },
    { key: "jis", value: "转全角" },
    { key: "wideChar", value: "转全角" },
    { key: "char", value: "数字代码转字符" },
    { key: "clean", value: "删除不可见字符" },
    { key: "code", value: "字符转数值代码" },
    { key: "concatenate", value: "合并到文本" },
    { key: "exact", value: "判断文本是否相同" },
    { key: "find", value: "查找文本" },
    { key: "left", value: "截取左边的字符" },
    { key: "len", value: "字符个数" },
    { key: "mid", value: "截取文本" },
    { key: "proper", value: "每个单词首字母大写" },
    { key: "replace", value: "替换字符" },
    { key: "rept", value: "重复文本" },
    { key: "right", value: "截取右边的字符" },
    { key: "rmb", value: "转大写数值文本" },
    { key: "search", value: "查找文本" },
    { key: "substitute", value: "替换字符" },
    { key: "t", value: "转文本" },
    { key: "text", value: "数值转换为文本" },
    { key: "trim", value: "删除文本中的空格" },
    { key: "lower", value: "英文转小写" },
    { key: "tolower", value: "英文转小写" },
    { key: "upper", value: "英文转大写" },
    { key: "toupper", value: "英文转大写" },
    { key: "value", value: "文本转数值" },
    { key: "now", value: "当前日期和时间" },
    { key: "today", value: "今日" },
    { key: "dateValue", value: "文本转日期" },
    { key: "timeValue", value: "文本转时间" },
    { key: "date", value: "日期" },
    { key: "time", value: "时间" },
    { key: "year", value: "年" },
    { key: "month", value: "月" },
    { key: "day", value: "日" },
    { key: "hour", value: "小时" },
    { key: "minute", value: "分钟" },
    { key: "second", value: "秒" },
    { key: "weekday", value: "星期几" },
    { key: "dateDif", value: "相隔天数" },
    { key: "days360", value: "相隔天数" },
    { key: "eDate", value: "计算以后日期" },
    { key: "eoMonth", value: "计算以后月份最后一天" },
    { key: "netWorkdays", value: "计算工作天数" },
    { key: "workDay", value: "计算以后工作日期" },
    { key: "weekNum", value: "求周数" },
    { key: "max", value: "最大值" },
    { key: "median", value: "中值" },
    { key: "min", value: "最小值" },
    { key: "quartile", value: "集合四分位数" },
    { key: "mode", value: "频率最高的数值" },
    { key: "large", value: "第k个最大值" },
    { key: "small", value: "第k个最小值" },
    { key: "percentile", value: "第k个百分位值" },
    { key: "percentRank", value: "中值的百分比排位" },
    { key: "average", value: "平均值" },
    { key: "averageIf", value: "平均值" },
    { key: "geoMean", value: "几何平均值" },
    { key: "harMean", value: "调和平均值" },
    { key: "count", value: "集合个数" },
    { key: "countIf", value: "集合个数" },
    { key: "sum", value: "求和" },
    { key: "sumIf", value: "求和" },
    { key: "aveDev", value: "平均差" },
    { key: "stDev", value: "样本估算标准偏差" },
    { key: "stDevp", value: "样本总体标准偏差" },
    { key: "devSq", value: "偏差的平方和" },
    { key: "var", value: "样本估算方差" },
    { key: "varp", value: "样本总体计算方差" },
    { key: "normDist", value: "正态累积分布" },
    { key: "normInv", value: "反正态累积分布" },
    { key: "normSDist", value: "标准正态累积分布" },
    { key: "normSInv", value: "反标准正态累积分布" },
    { key: "betaDist", value: "Beta累积分布" },
    { key: "betaInv", value: "反Beta累积分布" },
    { key: "binomDist", value: "一元二项式分布" },
    { key: "exponDist", value: "指数分布" },
    { key: "fDist", value: "F概率分布" },
    { key: "fInv", value: "反F概率分布" },
    { key: "fisher", value: "Fisher变换" },
    { key: "fisherInv", value: "反fisher变换" },
    { key: "gammaDist", value: "伽玛函数" },
    { key: "gammaInv", value: "反伽玛函数的" },
    { key: "gammaLn", value: "伽玛函数的自然对数" },
    { key: "hypgeomDist", value: "超几何分布" },
    { key: "logInv", value: "反对数累积分布" },
    { key: "logNormDist", value: "反对数正态分布" },
    { key: "negBinomDist", value: "负二项式分布" },
    { key: "poisson", value: "Poisson分布" },
    { key: "tDist", value: "t分布" },
    { key: "tInv", value: "反t分布" },
    { key: "weibull", value: "Weibull分布" },
    { key: "lookUp", value: "查找函数" },
    { key: "regexRepalce", value: "正则替换" },
    { key: "isRegex", value: "正则匹配" },
    { key: "isMatch", value: "正则匹配" },
    { key: "trimStart", value: "消空文本左边" },
    { key: "lTrim", value: "消空文本左边" },
    { key: "trimEnd", value: "消空文本右边" },
    { key: "rTrim", value: "消空文本右边" },
    { key: "indexOf", value: "查找文本位置" },
    { key: "lastIndexOf", value: "查找文本位置" },
    { key: "split", value: "分割文本" },
    { key: "join", value: "合并文本" },
    { key: "subString", value: "切割文本" },
    { key: "startsWith", value: "匹配开头" },
    { key: "endsWith", value: "匹配结尾" },
    { key: "isNullOrEmpty", value: "判断null或空" },
    { key: "isNullOrWhiteSpace", value: "判断null、空、空白字符" },
    { key: "removeStart", value: "去除左边匹配文本" },
    { key: "removeEnd", value: "去除右边匹配文本" },
    { key: "json", value: "转json格式" },
    { key: "error", value: "抛错" },
    { key: "has", value: "包含" },
    { key: "in", value: "在...之中" },
    { key: "addYears", value: "时间增加年" },
    { key: "addMonths", value: "时间增加月" },
    { key: "addDays", value: "时间增加日" },
    { key: "addHours", value: "时间增加时" },
    { key: "addMinutes", value: "时间增加分" },
    { key: "addSeconds", value: "时间增加秒" },
]