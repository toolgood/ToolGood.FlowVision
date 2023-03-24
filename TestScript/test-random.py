import random
import time
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
        {"json": {"学历": "测试"}},
    ],
}

def checkUrl(p, str2, isError):
    str2 = json.dumps(str2, ensure_ascii=False)
    p["json"] = json.dumps(p["json"], ensure_ascii=False)
    p["formulas"] = json.dumps(p["formulas"], ensure_ascii=False)

    url = "http://localhost:5063/api/EvalFormulas"
    r = requests.get(url, params=p)
    content = r.content.decode()
    result = json.loads(content)
    if result["code"] == 0 and isError == False:
        print("\033[1;31m" + str2 + "\033[0m")
        print("\033[1;32m" + content + "\033[0m")
        print("\033[1;33m" + p["json"] + "\033[0m")
        print("\033[1;34m" + str(p["isError"]) + "\033[0m")
        return False
    elif result["code"] == 1 and isError:
        print("\033[1;31m" + str2 + "\033[0m")
        print("\033[1;32m" + content + "\033[0m")
        print("\033[1;33m" + p["json"] + "\033[0m")
        print("\033[1;34m" + str(p["isError"]) + "\033[0m")
        return False
    else:
        print(content)
        return True

ps = JsonAnalysis.buildDefaultParameter(parameters)
others = JsonAnalysis.buildParameters(parameters)
errors = JsonAnalysis.buildError(parameters)

start = time.perf_counter()
for i in range(100):
    test = copy.deepcopy(ps)
    otherLen = random.randint(1, len(others)*2)
    change = {}
    for j in range(otherLen):
        k = random.randint(0, len(others)-1)
        JsonAnalysis.buildParametersWithError(change, others[k], [])
    if "isError" in change:
        change.pop("isError")
    JsonAnalysis.buildParametersWithError(test, change, errors)
    ok = checkUrl(test, change, test["isError"])
    if ok == False: # 用于调试
        test = copy.deepcopy(ps)
        if "isError" in change:
            change.pop("isError")
        JsonAnalysis.buildParametersWithError(test, change, errors)

end = time.perf_counter()
print("运行时间为", round(end-start), 'seconds')
