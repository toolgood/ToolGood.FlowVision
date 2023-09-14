/**
 * 页面菜单管理
 */
layui.define([
    'layer',
    'jquery',
], function (exports) {
    var layer = layui.layer,
        $ = layui.$
        ;
    var moduleManager = {//tab页面管理器
        //返回当前显示切页对象
        getCurrent: function () {
            var jCurrentTab = $('.J_menuTab.active'),
                dataUrl = jCurrentTab.data('url')
                ;

            return this.get(dataUrl);
        },
        //新增一个切页
        add: function (moduleItem) {
            moduleManager._container[moduleItem.url] = moduleItem;
        },
        //根据url返回某一个切页
        get: function (url) {
            return moduleManager._container[url];
        },
        //隐藏所有
        hideOthers: function (url) {
            for (var key in moduleManager._container) {
                if (key !== url) {
                    moduleManager._container[key].hide();
                }
            }
        },
        //关闭某一个
        remove: function (url) {
            delete moduleManager._container[url];
        },
        //关闭所有
        removeAll: function () {
            for (var key in moduleManager._container) {
                moduleManager._container[key].close();
            }
        },
        //关闭其他
        removeOthers: function (url) {
            // console.log(moduleManager._container)
            for (var key in moduleManager._container) {
                if (key !== url) {
                    // console.log(key, url)
                    moduleManager._container[key].close(false);
                }
            }
            // console.log(moduleManager._container)
        },
        _container: {}
    },
    jMainContent = $('.content-main')
    ;
	/**
	 * 页面类
	 * @param {String} url url
	 * @param {String} name     菜单名称
	 * @param {Integer} style   内容加载方式(0:ajax; 1:iframe)
	 * @param {Boolean} isFixed 是否固定加载该页面，如果是，不显示关闭按钮
	 */
    function ModuleItem(url, name, style, isFixed) {
        this.url = url;
        this.name = name || '';
        this.style = 1;// style !== undefined ? style : 1;
        this.isFixed = isFixed;
        this.id = this._getGuid();
        this.isShow = true;
        this._init();
    }

    ModuleItem.prototype = {
        _init: function () {
            var _this = this;

            _this.tabTarget = $('.J_menuTab[data-url="' + _this.url + '"]');

            // 添加选项卡
            if (!_this.tabTarget.length) {
                var str = '<a href="' + _this.url + '" class="active J_menuTab" data-url="' + _this.url + '"><span>' + _this.name + '</span></a>';
                _this.tabTarget = $(str);

                if (!_this.isFixed) {
                    _this.tabTarget.append('<i class="layui-icon layui-unselect layui-tab-close">ဆ</i>');
                }
                $('.J_menuTabs .page-tabs-content').append(_this.tabTarget);
            }

            this._render();
            this._bindEvent();
        },
        //返回随机串
        _getGuid: function () {
            var guid = '';

            for (var i = 1; i <= 32; i++) {
                var n = Math.floor(Math.random() * 16.0).toString(16);

                guid += n;

                if ((i == 8) || (i == 12) || (i == 16) || (i == 20)) {
                    guid += '-';
                }
            }

            return guid;
        },

        //渲染内容
        _render: function () {
            var _this = this;

            //layer.load();

            if (_this.style == 1) {//iframe
                _renderIframe().then(onRender);
            } else {
                _renderAjax().then(onRender);
            }

            function onRender(content) {
                var jContainer = $('<div class="sub-window-container" id="' + _this.id + '"></div>')

                if (_this.contentTarget) {//如果是刷新，先删除
                    _this.contentTarget.remove();
                }

                _this.contentTarget = jContainer.append(content);
                // console.log(jMainContent.length)
                jMainContent.append(_this.contentTarget);

                if (_this.isShow) {//同时加载有先后的因素，所以判断一下当前是否是显示状态
                    _this.show();
                } else {
                    _this.hide();
                }
            }

            //ajax方式加载
            function _renderAjax() {
                var def = $.Deferred();
                $.loadModule(_this.url).then(function (htmlStr) {
                    layer.closeAll('loading');//显示模板

                    var reg = /\<body[^\>]*?\>([\w\W]*?)\<\/body\>/,
                        testResult = reg.exec(htmlStr)
                        ;

                    if (testResult && testResult.length > 1) {
                        htmlStr = testResult[1];
                    }

                    def.resolve(htmlStr);
                }, function () {
                    layer.closeAll('loading');//显示模板

                    htmlStr = '<div cl>加载失败</div>';
                    def.resolve(htmlStr);
                });
                return def.promise()
            }

            //iframe方式加载
            function _renderIframe() {
                var def = $.Deferred();
                // 添加选项卡对应的iframe
                var jIframe = $('<iframe class="J_iframe" scrolling="yes" width="100%" height="100%" src="' + _this.url + '" frameborder="0" data-url="' + _this.url + '"></iframe>');
                jIframe.on("load", function () {
                    layer.closeAll('loading');//显示模板
                    _this.show();
                });
                def.resolve(jIframe);

                return def.promise()
            }
        },

        //绑定tab事件
        _bindEvent: function () {
            var _this = this;
            _this.tabTarget
                //单机
                .on('click', function (e) {
                    e && e.preventDefault();

                    _this.show();
                    return false;
                })
                //双击
                .on('dblclick', function (e) {
                    e && e.preventDefault();

                    _this.refresh();
                    return false;
                })
                .find('i').on('click', function (e) {
                    e && e.preventDefault();
                    e && e.stopPropagation();

                    _this.close();
                })
        },

        //内容缓存
        _cacheContent: null,

        //显示该页面
        show: function () {
            var _this = this;
            moduleManager.hideOthers(_this.url);//隐藏其它tab
            _this.tabTarget.addClass('active');
            if (!_this.isShow) {
                if (_this.style == 1) {
                    _this.contentTarget.show();
                } else {
                    jMainContent.append(_this.contentTarget);
                }
            }
            _this.isShow = true;

            var jCurrNode = $('.J_menuItem')
                .parents('.layui-this').removeClass('layui-this').end()
                .filter('[href="' + _this.url + '"]')
                ;
            //if ($('.layui-side-scroll').find('span').length) {
            //    $('.layui-side-scroll').find('span').remove()
            //}
            jCurrNode.parent('dd,li').addClass('layui-this')
            if (jCurrNode.parent('dd,li').find('span').length == 0) {
                jCurrNode.parent('dd,li').append('<span></span>')
            }

            // jCurrNode.parents('ul').addClass('layui-show')

            _this.scrollTo();
        },

        //隐藏
        hide: function () {
            var _this = this;

            _this.tabTarget.removeClass('active');
            if (_this.style == 1) {
                _this.contentTarget && _this.contentTarget.hide();
            } else {
                _this.contentTarget
                    .find('script').remove()//把js移除一下，否则渲染再次的时候重新执行
                    ;

                _this._cacheContent = _this.contentTarget;
                _this.contentTarget && _this.contentTarget.remove();
            }

            _this.isShow = false;
        },

        //滚动到该tab页面去
        scrollTo: function () {
            var _this = this;

            var marginLeftVal = calSumWidth(_this.tabTarget.prevAll()), marginRightVal = calSumWidth(_this.tabTarget.nextAll());
            // 可视区域非tab宽度
            var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".J_menuTabs"));
            //可视区域tab宽度
            var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
            //实际滚动宽度
            var scrollVal = 0;
            if ($(".page-tabs-content").outerWidth() < visibleWidth) {
                scrollVal = 0;
            } else if (marginRightVal <= (visibleWidth - _this.tabTarget.outerWidth(true) - _this.tabTarget.next().outerWidth(true))) {
                if ((visibleWidth - _this.tabTarget.next().outerWidth(true)) > marginRightVal) {
                    scrollVal = marginLeftVal;
                    while ((scrollVal - _this.tabTarget.outerWidth()) > ($(".page-tabs-content").outerWidth() - visibleWidth)) {
                        scrollVal -= _this.tabTarget.prev().outerWidth();
                        tabElement = _this.tabTarget.prev();
                    }
                }
            } else if (marginLeftVal > (visibleWidth - _this.tabTarget.outerWidth(true) - _this.tabTarget.prev().outerWidth(true))) {
                scrollVal = marginLeftVal - _this.tabTarget.prev().outerWidth(true);
            }
            $('.page-tabs-content').animate({
                marginLeft: 0 - scrollVal + 'px'
            }, "fast");
        },

        //刷新该tab页
        refresh: function () {
            this._render();
        },

        //关闭该tab
        close: function (isAnimate) {
            var _this = this;

            var currentWidth = _this.tabTarget.width();

            if (_this.tabTarget.find('i').length === 0) {//固定列不删掉
                return;
            }

            //当前元素处于选中状态
            if (_this.tabTarget.hasClass('active')) {
                // 当前元素后面有同辈元素，使后面的一个元素处于活动状态
                if (_this.tabTarget.next('.J_menuTab').size()) {
                    var nextUrl = _this.tabTarget.next('.J_menuTab:eq(0)').data('url');

                    moduleManager.get(nextUrl).show();
                }

                //当前元素后面没有同辈元素，使当前元素的上一个元素处于活动状态
                if (_this.tabTarget.prev('.J_menuTab').size()) {
                    var prevUrl = _this.tabTarget.prev('.J_menuTab:last').data('url');

                    moduleManager.get(prevUrl).show();
                }
            }

            //tab位移计算
            var marginLeftVal = parseInt($('.page-tabs-content').css('margin-left'));
            if (marginLeftVal < 0) {
                var styles = {
                    marginLeft: (marginLeftVal + currentWidth) + 'px'
                }
                if (isAnimate) {
                    $('.page-tabs-content').animate(styles, "fast");
                } else {
                    $('.page-tabs-content').css(styles);
                }
            }

            _this.tabTarget.remove();
            _this.contentTarget.remove();

            moduleManager.remove(_this.url);
        }
    }

    //增加一个切页
    window.addMenuItem = function (e) {
        e && e.preventDefault();
        var jThis = $(this),
            style = jThis.data('style'),//打开方式
            dataUrl = jThis.data('url') || jThis.attr('href'),
            menuName = jThis.clone().find("strong").remove().end().find("i").remove().end().text(),
            menuTabName = jThis.data('name') || jThis.find('cite').data('tabname'),
            moduleItem,
            isFixedOpen = jThis.attr('fixed') !== undefined
            ;
        if (dataUrl == undefined || $.trim(dataUrl).length == 0) return false;

        moduleItem = moduleManager.get(dataUrl);

        if (!moduleItem) {//不存在，新增
            menuName = (menuTabName) ? menuTabName : menuName;
            moduleItem = new ModuleItem(dataUrl, menuName, style, isFixedOpen);

            moduleManager.add(moduleItem);
        } else {
            moduleItem.show();
        }
        // return false;
    }
    window.addMenuTab = function (menuTabName, dataUrl) {
        moduleItem = moduleManager.get(dataUrl);
        if (!moduleItem) {//不存在，新增
            menuName = (menuTabName) ? menuTabName : menuName;
            moduleItem = new ModuleItem(dataUrl, menuName, undefined, false);
            moduleManager.add(moduleItem);
        } else {
            moduleItem.show();
        }
    }

    window.removeTab = function (url) {
        delete moduleManager._container[url].close();
    }

    window.refreshTab = function(url){
        for (var index in moduleManager._container) {
            if(index.indexOf(url) > -1) {
                ModuleItem.prototype.refresh.apply(moduleManager._container[index]);
            }
        }
    }
    window.setTabName = function (name) {
        $("nav.J_menuTabs a.active span").html(name)
    }

    //绑定事件
    $('.J_menuItem').on('click', addMenuItem).filter('[fixed]').trigger('click');

    //计算元素集合的总宽度
    function calSumWidth(elements) {
        var width = 0;
        $(elements).each(function () {
            width += $(this).outerWidth(true);
        });
        return width;
    }

    //查看左侧隐藏的选项卡
    function scrollTabLeft() {
        var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
        // 可视区域非tab宽度
        var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".J_menuTabs"));
        //可视区域tab宽度
        var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
        //实际滚动宽度
        var scrollVal = 0;
        if ($(".page-tabs-content").width() < visibleWidth) {
            return false;
        } else {
            var tabElement = $(".J_menuTab:first");
            var offsetVal = 0;
            while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {//找到离当前tab最近的元素
                offsetVal += $(tabElement).outerWidth(true);
                tabElement = $(tabElement).next();
            }
            offsetVal = 0;
            if (calSumWidth($(tabElement).prevAll()) > visibleWidth) {
                while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                    offsetVal += $(tabElement).outerWidth(true);
                    tabElement = $(tabElement).prev();
                }
                scrollVal = calSumWidth($(tabElement).prevAll());
            }
        }
        $('.page-tabs-content').animate({
            marginLeft: 0 - scrollVal + 'px'
        }, "fast");
    }

    //查看右侧隐藏的选项卡
    function scrollTabRight() {
        var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
        // 可视区域非tab宽度
        var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".J_menuTabs"));
        //可视区域tab宽度
        var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
        //实际滚动宽度
        var scrollVal = 0;
        if ($(".page-tabs-content").width() < visibleWidth) {
            return false;
        } else {
            var tabElement = $(".J_menuTab:first");
            var offsetVal = 0;
            while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {//找到离当前tab最近的元素
                offsetVal += $(tabElement).outerWidth(true);
                tabElement = $(tabElement).next();
            }
            offsetVal = 0;
            while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                offsetVal += $(tabElement).outerWidth(true);
                tabElement = $(tabElement).next();
            }
            scrollVal = calSumWidth($(tabElement).prevAll());
            if (scrollVal > 0) {
                $('.page-tabs-content').animate({
                    marginLeft: 0 - scrollVal + 'px'
                }, "fast");
            }
        }
    }

    // 左移按扭
    $('.J_tabLeft').on('click', scrollTabLeft);

    // 右移按扭
    $('.J_tabRight').on('click', scrollTabRight);

    //定位到当前选项卡
    $('.J_tabShowActive').on('click', function () {
        moduleManager.getCurrent().scrollTo();
        $(this).removeClass('layui-this').parents('.layui-nav-child').removeClass('layui-show')
    });

    // 关闭全部
    $('.J_tabCloseAll').on('click', function () {
        moduleManager.removeAll();
        $('.page-tabs-content').css("marginLeft", "0px");
        $(this).removeClass('layui-this').parents('.layui-nav-child').removeClass('layui-show')
    });

    //关闭其他选项卡
    $('.J_tabCloseOther').on('click', function () {
        moduleManager.removeOthers(moduleManager.getCurrent().url);
        $('.page-tabs-content').css("marginLeft", "0px");
        $(this).removeClass('layui-this').parents('.layui-nav-child').removeClass('layui-show')
    });

    exports('js/contabs', {});
});