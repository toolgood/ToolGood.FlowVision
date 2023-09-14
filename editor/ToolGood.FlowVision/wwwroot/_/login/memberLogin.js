var _index_loading; //加载中层
$(function () {
    $("#tname").keyup(function (key) { if (key.keyCode == 13) { $("#tpwd").prop("readonly", false); $("#tpwd").focus(); } });
    $("#tpwd").keyup(function (key) { if (key.keyCode == 13) { $("#vcode").focus(); } });
    $("#vcode").keyup(function (key) { if (key.keyCode == 13) { login(); } });
    $("#vcode-img").click(function () {
        var url = "/Members/Account/VerifyCode?r=" + Math.random();
        $(this).attr("src", url);
        $("#vcode").val("");
        $("#vcode").focus();
    });
    if (window.sessionStorage) {
        window.sessionStorage.clear();
    }

    setTimeout(function () {
        $("#hidden").hide();
    }, 500);

    //测试是否支持本地存储
    function _testIsSupportStorage() {
        try {
            var a = "1";
            localStorage.setItem('_test_', a);
            _supportStorageState = !!localStorage.getItem('_test_') ? 1 : 0;
        } catch (ex) {//ios 的safari浏览器无痕模式会禁用本地存储，而且会报错
            _supportStorageState = -2;
        }

        if (this._supportStorageState == -2) {
            new openTopMsg('请关闭当前浏览器无痕浏览模式，以保证当前页面正常浏览。');
        } else if (this._supportStorageState == 0) {//不支持方法的调用模式，重写常用的几个方法，经测试傻逼UC就不支持下面这些方法，只能直接赋值
            try {
                localStorage.setItem = function (key, value) {
                    localStorage[key] = value;
                    localStorage.length++;
                }
                localStorage.getItem = function (key) {
                    return localStorage[key];
                }
                localStorage.removeItem = function (key, value) {
                    delete localStorage[key];
                    localStorage.length--;
                }

                sessionStorage.setItem = function (key, value) {
                    sessionStorage[key] = value;
                    sessionStorage.length++;
                }
                sessionStorage.getItem = function (key) {
                    return sessionStorage[key];
                }
                sessionStorage.removeItem = function (key, value) {
                    delete sessionStorage[key];
                    sessionStorage.length--;
                }
            } catch (ex) {
            }
        }
    }
    _testIsSupportStorage();
});
function HidenLoading() { layer.close(_index_loading); }
function openLoading() { _index_loading = layer.load(1); }
function MessageAlert(msg, title, icon, callBack) {
    layer.msg(msg, {
        title: title,
        icon: icon,
        time: 2000 //2秒关闭（如果不配置，默认是3秒）
    }, function (index) {
        callBack && callBack();
        layer.close(index);
    });
}
function MessageSuccess(msg, callBack) {
    MessageAlert(msg, '成功', 1, callBack);
}

window.login=function() {
    if ($("#tname").val().length < 2) { layer.alert("请输入邮箱"); $("#tname").focus(); return; }
    if ($("#tpwd").val().length < 3) { layer.alert("请输入密码"); $("#tpwd").focus(); return; }
    if ($("#vcode").val().length < 5) { layer.alert("请输入验证码"); $("#vcode").focus(); return; }
    var data = {
        "__RequestVerificationToken": $("input[name='__RequestVerificationToken']").val(),
        TName: $("#tname").val(),
        TPwd: $("#tpwd").val(),
        Vcode: $("#vcode").val(),
        Fingerprint: window.fingerprint
    };
    openLoading();

    if ($("#password").length > 0) {
        var str = "";
        for (var i = 0; i < $("#tpwd").val().length; i++) { str += "●"; }
        $("#tpwd").val(str);
        $("#tpwd").attr("type", "text");
    }

    $.login("/Members/ajax/Account/Login", data, function (r) {
        HidenLoading();
        if (r.state == "SUCCESS") {
            var url = getQueryString("url");
            if (url == null) { url = "/Members"; }
            MessageSuccess("登录成功，即将跳转，耐心等候。", function () {
                if (sessionStorage) {
                    sessionStorage.setItem("o", "1");
                }
                location.href = url;
            });
        } else {
            $("#tpwd").attr("type", "password");

            HidenLoading();
            layer.alert(r.message);
            $("#tpwd").val(data.TPwd);
            var url = "/Members/Account/VerifyCode?r=" + Math.random();
            $("#vcode-img").attr("src", url);
            $("#vcode").val("");
        }
    }, function (r) {
        $("#tpwd").attr("type", "password");
        HidenLoading();
        layer.alert("网络连接出错！");
        $("#tpwd").val(data.TPwd);
        var url = "/Members/Account/VerifyCode?r=" + Math.random();
        $("#vcode-img").attr("src", url);
        $("#vcode").val("");
    });
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}