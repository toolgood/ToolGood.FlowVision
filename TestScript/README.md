# 测试脚本

用于测试FlowVision规则是否设计合理
- `data.sav` 为案例二的数据库
- `test-one-parameter.py` 测试脚本，单个参数变化，只检测规则是否能运行
- `test-random.py` 测试脚本，多个参数变化，只检测规则是否能运行


# 脚本说明
`test-one-parameter.py`和`test-random.py`脚本主要修改以下

``` py
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
```
`factoryCode`设置厂区编码，默认`default`。
`appCode` 设置流程编码
`json`设置传入的参数，方括号枚举可能的值。
`formulas`设置返回的公式
`batch`可以批量设置参数，其中`isError`表示这例参数会出错。
`errors`匹配错误的参数




