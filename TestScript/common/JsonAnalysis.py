#!/usr/bin/env python3
# -*- coding: utf-8 -*-


import copy


def _getDefault(value):
    if isinstance(value, (frozenset, list, set, tuple,)):
        return value[0]
    else:
        return value


def buildDefaultParameter(parameters):
    p = {}
    p["factoryCode"] = _getDefault(parameters["factoryCode"])
    p["appCode"] = _getDefault(parameters["appCode"])
    p["json"] = {}
    p["formulas"] = []
    for key in parameters["formulas"]:
        p["formulas"].append(
            {"name": key, "formula": parameters["formulas"][key]})
    p["previous"] = parameters["previous"]
    p["attachData"] = parameters["attachData"]
    p["machineInfos"] = parameters["machineInfos"]
    for key in parameters["json"]:
        p["json"][key] = _getDefault(parameters["json"][key])
    return p


def _getDefault_1(parameters, name, setValue,start=1):
    # 获取其他的可选方案
    if isinstance(parameters[name], (frozenset, list, set, tuple,)):
        value = parameters[name]
        for i in range(start, len(value)):
            setValue.append({name: value[i]})


def _getDefault_2(parameters, name, setValue,start=1):
    # 获取其他的可选方案  json
    if isinstance(parameters[name], (frozenset, list, set, tuple,)):
        value = parameters[name]
        for i in range(start, len(value)):
            setValue.append({"json": {name: value[i]}})


def buildOtherParameters(parameters):
    ps = []
    _getDefault_1(parameters, "factoryCode", ps)
    _getDefault_1(parameters, "appCode", ps)
    value = parameters["json"]
    for key in value:
        _getDefault_2(value, key, ps)
    for i in range(len(parameters["batch"])):
        ps.append(parameters["batch"][i])
    return ps

def buildParameters(parameters):
    ps = []
    _getDefault_1(parameters, "factoryCode", ps,0)
    _getDefault_1(parameters, "appCode", ps,0)
    value = parameters["json"]
    for key in value:
        _getDefault_2(value, key, ps,0)
    for i in range(len(parameters["batch"])):
        ps.append(parameters["batch"][i])
    return ps

def checkError(p, errors):
    for i in range(len(errors)):
        isError = True
        error = errors[i]
        for key in error:
            value = error[key]
            if isinstance(value, dict):
                for key2 in value:
                    value2 = value[key2]
                    if isinstance(value2, (frozenset, list, set, tuple,)):
                        if p[key][key2] not in value2:
                            isError = False
                            break
                    elif p[key][key2] != value[key2]:
                        isError = False
                        break
                if isError == False:
                    break
            else:
                if isinstance(value, (frozenset, list, set, tuple,)):
                    if p[key] not in value:
                        isError = False
                        break
                elif p[key] != value:
                    isError = False
                    break
        if isError:
            p["isError"]=True
            return True
    p["isError"]=False
    return False


def buildError(p):
    errors = []
    if "errors" in p:
        for key in range(len(p["errors"])):
            errors.append(p["errors"][key])
    if "batch" in p:
        for key in range(len(p["batch"])):
            obj = p["batch"][key]
            if "isError" in obj:
                if obj["isError"]:
                    obj2 = copy.deepcopy(obj)
                    obj2.pop("isError")
                    errors.append(obj2)
    return errors


def buildParametersWithError(p, params, errors):
    for key in params:
        value = params[key]
        if isinstance(value, dict):
            for key2 in value:
                if key not in p:
                    p[key] = {}
                p[key][key2] = value[key2]
        else:
            p[key] = value
    if "isError" not in p:
        checkError(p, errors)

def setParameters(p, params):
    for key in params:
        value = params[key]
        if isinstance(value, dict):
            for key2 in value:
                p[key][key2] = value[key2]
        else:
            p[key] = value