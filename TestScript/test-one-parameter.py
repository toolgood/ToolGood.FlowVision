import random
import time
from common import SqliteHelper
from common import JsonAnalysis
import requests
import json
import copy
import sys
import os
commonpath = os.path.abspath(os.path.dirname(__file__))
sys.path.append(commonpath)

parameters = {
    "factoryCode": ["default"],
    "appCode": ["Flow"],
    "json": {
        "姓名":"测试",
        "性别":["男","女"],
        "年龄":[20,30,50,60],
        "学历":["专科以下","专科","本科","本科以上"],
        "手机":"123456789",
        "月收入":[1500,2500,3500,5500,10500,20500],
        "地址":"地址",
        "有房":["有","无"],
        "有车":["有","无"],
        "信用卡数量":[0,1,5,10,11]
    },
    "formulas": {"result": "额度"},
    "previous": "",
    "attachData": "",
    "machineInfos": "",

    "batch": [
        {"json": {"学历": "测试", "月收入": "4000"}, "isError": True},
    ],
    "errors": [
        {"json": {"学历": "无", "月收入": "4000"}},
    ],
}

def checkUrl(p, str, isError):
    url = "http://localhost:5063/api/EvalFormulas"
    p["json"] = json.dumps(p["json"], ensure_ascii=False)
    p["formulas"] = json.dumps(p["formulas"], ensure_ascii=False)
    r = requests.get(url, params=p)
    content = r.content.decode()
    result = json.loads(content)
    if result["code"] == 0 and isError == False:
        print("\033[1;31m" + str + " is error:"+content+"\033[0m")
    elif result["code"] == 1 and isError:
        print("\033[1;31m" + str + " is error"+content+"\033[0m")
    else:
        print(content)


ps = JsonAnalysis.buildDefaultParameter(parameters)
others = JsonAnalysis.buildOtherParameters(parameters)
errors = JsonAnalysis.buildError(parameters)

start = time.perf_counter()
checkUrl(copy.deepcopy(ps), "默认", False)
for i in range(len(others)):
    test = copy.deepcopy(ps)
    JsonAnalysis.buildParametersWithError( test, others[i], errors)
    checkUrl(test, json.dumps(others[i], ensure_ascii=False), test["isError"])

end = time.perf_counter()
print("运行时间为", round(end-start), 'seconds')