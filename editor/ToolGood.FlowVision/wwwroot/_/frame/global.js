$(function () {
    window.getParameter = function (val) {
        var re = new RegExp("[?&]" + val + "=([^&?]*)", "ig");
        var url = location.search;
        return ((url.match(re)) ? (decodeURI(url.match(re)[0].substr(val.length + 2))) : '');
    }
    window.openMsg = function (msg, callBack) {
        if (msg == null) { msg = "[null]"; }
        layer.msg(msg, { time: 2000 }, function (index) { callBack && callBack(); layer.close(index); });
    }
    window.openTopMsg = function (msg, callBack) {
        if (window != top) { top.openMsg(msg, callBack); } else { openMsg(msg, callBack); }
    }
    window.openAlert = function (msg, title, callBack) {
        if (msg == null) { msg = "[null]"; }
        if (typeof title === "function") {
            layer.alert(msg, function (index) { callBack && callBack(); layer.close(index); });
        } else {
            layer.alert(msg, { title: title }, function (index) { callBack && callBack(); layer.close(index); });
        }
    }
    window.openTopAlert = function (msg, title, callBack) {
        if (window != top) {
            top.openAlert(msg, title, callBack);
        } else {
            openAlert(msg, title, callBack);
        }
    }
    window.openWarn = function (msg, callBack) {
        openAlert(msg, '警告', 0, callBack);
    }
    window.openTopWarn = function (msg, callBack) {
        if (window != top) {
            top.openWarn(msg, callBack);
        } else {
            openWarn(msg, callBack);
        }
    }
    window.openConfirm = function (msg, title, confirmCallback, noConfirmCallback) {
        if (msg == null) { msg = "[null]"; }
        if (typeof title === "function") {
            noConfirmCallback = confirmCallback;
            confirmCallback = title;
            title = "提示";
        }
        layer.confirm(msg, { title: title }, function (index) {
            confirmCallback && confirmCallback();
            layer.close(index);
        }, function () {
            noConfirmCallback && noConfirmCallback();
        });
    }
    window.openTopConfirm = function (msg, title, confirmCallback, noConfirmCallback) {
        if (window != top) {
            top.openConfirm(msg, title, confirmCallback, noConfirmCallback);
        } else {
            openConfirm(msg, title, confirmCallback, noConfirmCallback);
        }
    }
    window.getParentWindow = function (index) {
        if (window != top) {
            var index = parent.layer.getFrameIndex(window.name);
            return top.getParentWindow(index);
        } else {
            var $ = jQuery;
            //判断是否有上级的弹层，如果有，就刷新上级弹层
            if ($("#layui-layer" + index).prevAll('.layui-layer').length > 0) {
                var id = $($("#layui-layer" + index).prevAll('.layui-layer')[0]).attr('id').replace('layui-layer', '');
                return $("#layui-layer-iframe" + id)[0].contentWindow;
            } else {
                $(".sub-window-container").each(function () {
                    if ($(this).css("display") != "none") {
                        return $(this).children("iframe")[0].contentWindow;
                    }
                })
                return parent;
            }
        }
    }
    window.closeWindowAndReload = function (funName, index) {
        if (window != top) {
            index = parent.layer.getFrameIndex(window.name);
            top.closeWindowAndReload(funName, index);
            parent.layer.close(index);
        } else {
            var $ = jQuery;
            //判断是否有上级的弹层，如果有，就刷新上级弹层
            if ($("#layui-layer" + index).prevAll('.layui-layer').length > 0) {
                var id = $($("#layui-layer" + index).prevAll('.layui-layer')[0]).attr('id').replace('layui-layer', '');
                var target = $('#layui-layer-iframe' + id);
                if (funName && target[0].contentWindow[funName]) {
                    target[0].contentWindow[funName]();
                } else if (target[0].contentWindow.reload) {
                    target[0].contentWindow.reload();
                } else {
                    target[0].contentWindow.location.reload();
                }
            } else {
                $(".sub-window-container").each(function () {
                    if ($(this).css("display") != "none") {
                        var target = $(this).children("iframe");
                        if (funName && target[0].contentWindow[funName]) {
                            target[0].contentWindow[funName]();
                        } else if (target[0].contentWindow.reload) {
                            target[0].contentWindow.reload();
                        } else {
                            target[0].contentWindow.location.reload();
                        }
                    }
                })
            }
            parent.layer.close(index);
        }
    }

    window.closeWindow = function () {
        var index = parent.layer.getFrameIndex(window.name);
        parent.layer.close(index);
    }
    window.closeTabContent = function () {
        var id = top.$("ul.layui-tab-title li.layui-this").attr("lay-id");
        var element = top.layui.element;
        element.tabDelete('myTab', id);
    }
    window.closeAllWindows = function () { top.layer.closeAll(); }
    window.openLoading = function () { layer.load(2); }
    window.closeLoading = function () { layer.closeAll("loading"); }
    window.openWindow = function (title, url, success, end) {
        function QueryString(val, url) {
            var re = new RegExp("" + val + "=([^&?#]*)", "ig");
            return ((url.match(re)) ? (decodeURI(url.match(re)[0].substr(val.length + 1))) : '');
        }
        var w = QueryString('w', url), h = QueryString('h', url);
        w = ((w == null || w == '')) ? ($('body').width() - 100) : w;
        h = ((h == null || h == '')) ? ($('body').height() - 100) : h;
        if ($('body').width() * 0.95 < w) { w = $('body').width() * 0.95; }
        if ($('body').height() * 0.95 < h) { h = $('body').height() * 0.95; }

        layer.open({
            type: 2,
            area: [w + 'px', h + 'px'],
            fix: true,
            shade: 0.4,
            title: title,
            anim: -1,
            content: [url],
            scrollbar: true,
            shadeClose: false,
            maxmin: true,
            success: function (layero, index) {
                success && success(layero, index, window[layero.find('iframe')[0]['name']]);
            },
            end: function () {
                end && end();
            }
        });
    }
    window.openTopWindow = function (title, url, success, end) {
        if (url.charAt(0) == '.' && url.charAt(1) == '.' && url.charAt(1) == '/') {
            url = location.pathname + '/../' + url;
        } else if (url.charAt(0) == '.' && url.charAt(1) == '/') {
            url = location.pathname + '/.' + url;
        }
        url = url.replace("//", "/");
        if (window != top) {
            top.openWindow(title, url, success, end);
        } else {
            openWindow(title, url, success, end);
        }
        return false;
    }
    window.trrigerOpenTopWindow = function (id, data, success, end) {
        var t = $(obj).attr("title") || $(obj).attr("name") || $(obj).text();
        var a = $(obj).attr("href") || $(obj).attr("src") || $(obj).attr("alt");
        if (data) {
            for (var item in data) {
                if (data[item]) {
                    a += a.indexOf("?") >= 0 ? "&" : "?";
                    a += item + "=" + encodeURI(data[item]);
                }
            }
        }
        openTopWindow(t, a, success, end);
    }
    window.trrigerOpenWindow = function (id, data, success, end) {
        var t = $(obj).attr("title") || $(obj).attr("name") || $(obj).text();
        var a = $(obj).attr("href") || $(obj).attr("src") || $(obj).attr("alt");
        if (data) {
            for (var item in data) {
                if (data[item]) {
                    a += a.indexOf("?") >= 0 ? "&" : "?";
                    a += item + "=" + encodeURI(data[item]);
                }
            }
        }
        openWindow(t, a, success, end);
    }
    window.standardSearchText = function (str) {
        // \u202D\u202c 从excel表格直接复制到input框
        // \u0085 代表下一行的字符
        // \u00A0 不换行空格 相当与  看上去和空格一样，但是在HTML中不自动换行， 主要用在office中,让一个单词在结尾处不会换行显示,快捷键ctrl+shift+space ;
        // \u2028 行分隔符
        // \u2029 段落分隔符
        // \uFEFF 字节顺序标记(零宽非连接符)
        // \u200E 从左至右书写标记
        // \u200F 从右至左书写标记
        // \u200D 零宽连接符
        // \u2006 另一种空格符
        // \u3000 全角空格(中文符号)
        str = str.replace(/[\x00-\x1F\x7f]/ig, '');// ASCII码 不可见字符
        str = str.replace(/[\u00A0\u1680\u2000-\u200a\u202F\u205F\u3000]/ig, ' ');// 换成普通空格符
        return str.replace(/[\u180E\u200b-\u200f\u2028-\u202e\u2060\uFEFF]/ig, '').trim(); //清空两端空格
    }
    window.standardText = function (str) {
        str = str.replace(/[\x00-\x08\x0B\x0C\x0E-\x1F\x7f\u0085]/ig, '');// ASCII码 排除 \t \r \n
        str = str.replace(/[\u00A0\u1680\u2000-\u200a\u202F\u205F\u3000]/ig, ' ');// 换成普通空格符
        return str.replace(/[\u180E\u200b-\u200f\u2028-\u202e\u2060\uFEFF]/ig, '').trim(); //清空两端空格
    }
    window.standardInt = function (newValue, min, max) {
        if (min && min > 0) {
            newValue = newValue.replace(/[^\d]/g, '').replace(/^0+(\d.*)$/g, '$1');
        } else {
            newValue = newValue.replace(/[^-\d]/g, '').replace(/^(-?)0+(\d.*)$/g, '$1$2').replace(/^(.+)-/g, '$1');
        }
        if (min && min != '' && newValue != "") { if (parseInt(newValue) < min) { newValue = min } }
        if (max && max != '' && newValue != "") { if (parseInt(newValue) > max) { newValue = max } }
        return newValue;
    }
    window.standardNum = function (newValue, min, max) {
        if (min && min > 0) {
            newValue = newValue.replace(/[^\.\d]/g, '').replace(/^0+(\d.*)$/g, '$1').replace(/(\..*?)\./g, '$1').replace(/^(\.)(\d*)$/g, '0$1$2');
        } else {
            newValue = newValue.replace(/[^-\.\d]/g, '').replace(/^(-?)0+(\d.*)$/g, '$1$2').replace(/^(.+)-/g, '$1').replace(/(\..*?)\./g, '$1').replace(/^(-?)(\.)(\d*)$/g, '$10$2$3')
        }
        if (min && min != '' && newValue != "") { if (parseFloat(newValue) < min) { newValue = min } }
        if (max && max != '' && newValue != "") { if (parseFloat(newValue) > max) { newValue = max } }
        return newValue;
    }
    window.dateFormat = function (date, fmt) {
        if (fmt == undefined) { fmt = "yyyy-MM-dd HH:mm:ss"; }
        var o = {
            "M+": date.getMonth() + 1, //月份
            "d+": date.getDate(), //日
            "h+": date.getHours() % 12 == 0 ? 12 : date.getHours() % 12, //小时
            "H+": date.getHours(), //小时
            "m+": date.getMinutes(), //分
            "s+": date.getSeconds(), //秒
            "q+": Math.floor((date.getMonth() + 3) / 3), //季度
            "S": date.getMilliseconds() //毫秒
        };
        var week = { "0": "/u65e5", "1": "/u4e00", "2": "/u4e8c", "3": "/u4e09", "4": "/u56db", "5": "/u4e94", "6": "/u516d" };
        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        if (/(E+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[date.getDay() + ""]);
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            }
        }
        return fmt;
    }
    window.standardDate = function (newValue, min, max) {
        function isValidDate(date) { return date instanceof Date && !isNaN(date.getTime()) }
        newValue = newValue.replace(/[年月]/g, '-');
        newValue = newValue.replace(/日/g, ' ');
        newValue = newValue.replace(/[时分]/g, ':');
        newValue = newValue.replace(/秒/g, '');
        newValue = newValue.replace(/\//g, '-');
        newValue = newValue.replace(/T/g, ' ');
        newValue = newValue.replace(/ +/g, ' ');
        //newValue = newValue.trim();
        newValue = standardSearchText(newValue);

        if (/^\d{1}-\d{1,2}-\d{1,2}( |$)/.test(newValue)) {
            var y = new Date().getFullYear().toString().substr(0, 3);
            newValue = y + newValue;
        } else if (/^\d{2}-\d{1,2}-\d{1,2}( |$)/.test(newValue)) {
            var y = new Date().getFullYear().toString().substr(0, 2);
            newValue = y + newValue;
        } else if (/^\d{1,2}-\d{1,2}( |$)/.test(newValue)) {
            var y = new Date().getFullYear().toString();
            newValue = y + "-" + newValue;
        }
        if (/^\d{4}-\d{1,2}-\d{1,2} \d{1,2}$/.test(newValue)) { newValue = newValue + ":0:0"; }
        if (/^\d{4}-\d{1,2}-\d{1,2}( \d{1,2}:\d{1,2}(:\d{1,2})?)?$/.test(newValue)) {
            newValue = newValue.replace(/-/g, '/');//除去8小时时区差
            dt = new Date(newValue);
            if (isValidDate(dt)) {
                if (min && min != '') {
                    minDt = new Date(min.replace(/-/g, '/'));
                    if (isValidDate(minDt) && dt < minDt) { dt = minDt }
                }
                if (max && max != '') {
                    maxDt = new Date(max.replace(/-/g, '/'));
                    if (isValidDate(maxDt) && dt > maxDt) { dt = maxDt }
                }
                return dateFormat(dt)
            }
        }
        return '';
    }
});

(function (factory) {
    if (typeof define === 'function' && define.amd) { // AMD. Register as an anonymous module.
        define(['jquery'], factory);
    } else if (typeof exports === 'object') { // Node/CommonJS
        var jQuery = require('jquery');
        module.exports = factory(jQuery);
    } else { // Browser globals (zepto supported)
        factory(window.jQuery || window.Zepto || window.$); // Zepto supported on browsers as well
    }
}(function ($) {
    /**
     * 序列化
     * @param {any} type 0、直接获取文本，1、获取文本格式化，2、获取文本格式化 去除 \t\r\n
     */
    $.fn.serializeJson = function (type) {
        var serializeObj = {};
        var map = {};
        var array = this.serializeArray();
        var txtFunc = standardText;
        if (type) {
            if (type == 0) {
                txtFunc = getText;
            } else if (type == 1) {
                txtFunc = standardText;
            } else if (type == 2) {
                txtFunc = standardSearchText;
            }
        }
        $(array).each(function () {
            var names = splitName(this.name);
            setData(serializeObj, map, names, 0, this.value, txtFunc);
        })
        return serializeObj;
    };
    /**
     * 去除禁用
     * */
    $.fn.removeDisabled = function () {
        $(this).prop("disabled", false);
        $(this).removeClass("layui-disabled");
        $(this).removeClass("disabled");
    }
    /**
     * 禁用
     * */
    $.fn.addDisabled = function () {
        $(this).prop("disabled", true);
        $(this).addClass("layui-disabled");
        $(this).addClass("disabled");
    }

    function setData(tarData, map, names, index, value, txtFunc) {
        var name = names[index];
        if (index == names.length - 1) {
            if (name.isArray) {
                if (tarData[name.name] == null) { tarData[name.name] = new Array(); }
                setValue(tarData, name.name, value, txtFunc);
            } else {
                setValue(tarData, name.name, value, txtFunc);
            }
        } else if (name.isArray && name.hasTag) {
            if (tarData[name.name] == null) { tarData[name.name] = new Array(); }
            if (map[name.allPath] == null) {
                var obj = {};
                tarData[name.name].push(obj);
                map[name.allPath] = obj;
            }
            setData(map[name.allPath], map, names, index + 1, value, txtFunc);
        } else if (name.isArray) {
            //console.log("input name is error, no tag, set the input name like 'a[tag].b' ");
            if (tarData[name.name] == null) { tarData[name.name] = new Array(); }
            setValue(tarData, name.name, value, txtFunc);
        } else {
            if (tarData[name.name] == null) { tarData[name.name] = {}; }
            setData(tarData[name.name], map, names, index + 1, value, txtFunc);
        }
    }
    function setValue(tarData, name, value, txtFunc) {
        if (tarData[name] == null || tarData[name] == undefined) {
            tarData[name] = txtFunc(value);
        } else if ($.isArray(tarData[name])) {
            tarData[name].push(txtFunc(value));
        } else {
            tarData[name] = [tarData[name], txtFunc(value)];
        }
    }
    function splitName(txt) {
        var ts = txt.split(".");
        var names = [];
        var path = "";
        for (var i = 0; i < ts.length; i++) {
            names.push(getName(ts[i], path));
            path += ts[i] + ".";
        }
        return names;
    }
    function getName(txt, path) {
        var m = /^(.*)\[([^\]]+)\]$/.exec(txt);
        if (m != null) {
            var dataName = m[1];
            return { isArray: true, hasTag: true, name: dataName, allPath: path + "." + txt };
        }
        var m2 = /^(.*)\[\]$/.exec(txt);
        if (m2 != null) {
            var dataName = m2[1];
            return { isArray: true, hasTag: false, name: dataName };
        }
        return { isArray: false, name: txt };
    }

    function getText(str) {
        return str;
    }
}));

/****************************************************************
 *    右键菜单
 ****************************************************************/

//$(function () {
//    window.showContextmenu = function (obj, win, e) {
//        function getOffset(win, x, y) {
//            if (win == top) { return { x: x, y: y }; }
//            var fs = win.parent.document.getElementsByTagName("iframe");
//            for (var i = 0; i < fs.length; i++) {
//                var f = fs[i];
//                if (f.contentWindow.window == win) {
//                    var offset = $(f).offset();
//                    var xx = offset.left + x;
//                    var yy = offset.top + y;
//                    return getOffset(win.parent, xx, yy)
//                }
//            }
//        }
//        if (window != top) { top.showContextmenu(obj, win, e); return; }
//        var v = getOffset(win, e.clientX, e.clientY);
//        var mouseX = v.x;
//        var mouseY = v.y;
//        var winWidth = $(document).width();
//        var winHeight = $(document).height();
//        var menuWidth = $("#contextmenu-div").width();
//        var menuHeight = $("#contextmenu-div").height();
//        var minEdgeMargin = 10;
//        if (mouseX + menuWidth + minEdgeMargin >= winWidth && mouseY + menuHeight + minEdgeMargin >= winHeight) {
//            menuLeft = mouseX - menuWidth - minEdgeMargin;
//            menuTop = mouseY - menuHeight;// - minEdgeMargin;
//        } else if (mouseX + menuWidth + minEdgeMargin >= winWidth) {
//            menuLeft = mouseX - menuWidth - minEdgeMargin;
//            menuTop = mouseY;//+ minEdgeMargin;
//        } else if (mouseY + menuHeight + minEdgeMargin >= winHeight) {
//            menuLeft = mouseX + minEdgeMargin;
//            menuTop = mouseY - menuHeight;//- minEdgeMargin;
//        } else {
//            menuLeft = mouseX + minEdgeMargin;
//            menuTop = mouseY;//+ minEdgeMargin;
//        };
//        if ($("#contextmenu-div").length == 0) { $(document.body).append("<ul id='contextmenu-div' class='contextmenu'></ul>"); }
//        $("#contextmenu-div").css({ "left": menuLeft + "px", "top": menuTop + "px" }).html("").append(obj);
//        setTimeout(function () { $("#contextmenu-div").show(); }, 100);
//    }
//    window.hideContextmenu = function () {if (window != top) { if (top.hideContextmenu) { top.hideContextmenu() }; return; } $("#contextmenu-div").hide(); }

//    $(document).on("mouseleave", "#contextmenu-div", function (event) {
//        event && event.stopPropagation();
//        setTimeout(function () { hideContextmenu(); }, 100);
//        return false;
//    });
//});

$(function () {
    window.layer = layui.layer;
    window.element = layui.element;
    window.treeSelect = layui.treeSelect;
    window.table = layui.table;
    window.treeTable = layui.treeTable;

    $(document).on("click", ".openwin", function (event) {
        event && event.stopPropagation();
        event.currentTarget.blur();
        $(this).prop("disenable", true);
        var t = $(this).attr("title") || $(this).attr("name") || $(this).text();
        var a = $(this).attr("href") || $(this).attr("src") || $(this).attr("alt");

        $(this).each(function () {
            $.each(this.attributes, function () {
                // this.attributes is not a plain object, but an array of attribute nodes, which contain both the name and value
                if (this.specified && this.name.indexOf("data-") == 0) {
                    a += a.indexOf("?") >= 0 ? "&" : "?";
                    a += this.name.replace("data-", "") + "=" + encodeURI(this.value);
                }
            });
        });
        openTopWindow(t, a, null, null, this)
        $(this).prop("disenable", false);
        return false;
    });
    $(document).on("click", ".opentab", function (event) {
        event && event.stopPropagation();
        event.currentTarget.blur();
        $(this).prop("disenable", true);
        var t = $(this).attr("title") || $(this).attr("name") || $(this).text();
        var a = $(this).attr("href") || $(this).attr("src") || $(this).attr("alt");

        $(this).each(function () {
            $.each(this.attributes, function () {
                // this.attributes is not a plain object, but an array of attribute nodes, which contain both the name and value
                if (this.specified && this.name.indexOf("data-") == 0) {
                    a += a.indexOf("?") >= 0 ? "&" : "?";
                    a += this.name.replace("data-", "") + "=" + encodeURI(this.value);
                }
            });
        });
        top.window.addMenuTab(t, a)
        $(this).prop("disenable", false);
        return false;
    });

    $(document).on("click", ".reset", function (event) {
        $(':input', '#searchForm')
            .not(':button,:submit,:reset,:hidden')
            .val('')
            .removeAttr('checked')
            .removeAttr('selected');
    });

    //$(document).click(function () { hideContextmenu(); });
    //$(document).contextmenu(function () { hideContextmenu(); return false; });

    //测试是否支持本地存储
    function _testIsSupportStorage() {
        try {
            localStorage.setItem('_test_', "1");
            _supportStorageState = !!localStorage.getItem('_test_') ? 1 : 0;
        } catch (ex) {//ios 的safari浏览器无痕模式会禁用本地存储，而且会报错
            _supportStorageState = -2;
        }

        if (_supportStorageState == -2) {
            new openTopMsg('请关闭当前浏览器无痕浏览模式，以保证当前页面正常浏览。');
        } else if (_supportStorageState == 0) {//不支持方法的调用模式，重写常用的几个方法
            try {
                localStorage.setItem = function (key, value) {
                    localStorage[key] = value;
                    localStorage.length++;
                }
                localStorage.getItem = function (key) {
                    return localStorage[key];
                }
                localStorage.removeItem = function (key) {
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
                sessionStorage.removeItem = function (key) {
                    delete sessionStorage[key];
                    sessionStorage.length--;
                }
            } catch (ex) {
            }
        }
    }
    _testIsSupportStorage();
});