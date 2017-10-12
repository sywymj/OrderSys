var template = '#template';
var loadingID = '.loading';
var refreshID = '.refreshing'
var endID = '.end';
var pageID = '.fresh';

function Query(container,pageSize) {
    this.container = container;
    this.pageSize = pageSize || this.pageSize;
}
Query.prototype = {
    container:"",
    pageIndex: 1,
    pageSize: 10,
    isLoading: false,
    isEnd: false,
    status: "",         //工单状态
    bookingTime: "",    //截止日期
    priority: "",       //紧急程度
    content:"",         //内容
    tabId: "",

    setNextPage: function () {
        this.pageIndex++;
    }
}

function Article(container) {
    this.container = container;
}
Article.prototype = {
    container: "",
    isLoading: false,
    isEnd: false,
}

doQuery1 = function (query,url,parms, callback) {
    //if (typeof query !== 'Query') { throw new Error('query: query must be a Object "Query"'); }
    if (query.isLoading) { return; }

    var container = query.container;
    if (query.pageIndex == 1) {
        //debugger;
        //重新刷新
        query.isLoading = false;
        query.isEnd = false;
        clearDom(container);
        $(container).siblings(loadingID).remove();
        $(container).siblings(endID).remove();
    }

    if (query.isEnd) { return; }
    hidePaging(container);
    showLoading(container, 'append');
    query.isLoading = true;

    $.ajax({
        type: "GET",
        url: url,
        data: parms,//筛选对象
        success: function (data) {
            //判断返回值不是 json 格式
            //debugger;
            if (!data.match("^\{(\n?.+:.+,?\n?){1,}\}$")) {
                appendDom(container, data);
                showPaging(container);
            }
            else {
                var jdata = ajaxTips(data, container, callback);
                if (jdata.RspTypeCode == 5 || jdata.RspTypeCode == 6) {
                    //没数据
                    query.isEnd = true;
                }
            }
            query.setNextPage();//这里要重新考虑一下，现在是每加载一次，页面就加1
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        },
        complete: function () {
            ////debugger;
            query.isLoading = false;
            hideLoading(container, 'append');//直接remove
        }
    })
}

doGetPartial1 = function (article,url,parms, callback) {
    //debugger;
    if (article.isLoading) { return; }

    var container = article.container;
    clearDom(container);
    clearDom(container + "~" + ".loading");//清除旁边的加载中等元素
    //clearDom(container + "~" + "div");//清除旁边的加载中等元素

    showLoading(container, 'append');
    article.isLoading = true;
    debugger;
    $.ajax({
        type: "GET",
        url: url,
        data: parms,
        async: false, //默认设置为true，所有请求均为异步请求。
        success: function (data) {
            debugger;
            //判断返回值不是 json 格式
            if (!data.match("^\{(\n?.+:.+,?\n?){1,}\}$")) {
                appendDom(container, data);
            }
            else {
                var jdata = ajaxTips(data, container);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        },
        complete: function () {
            debugger;
            hideLoading(container, 'append');//直接remove
            article.isLoading = false;
        }
    })
}

doPost = function (url, postObj, callback) {
    $.showLoading();
    $.ajax({
        type: "POST", //GET或POST,
        async: true, //默认设置为true，所有请求均为异步请求。
        url: url,
        data: postObj,
        dataType: "text", //xml、html、script、jsonp、text
        beforeSend: function () { },
        success: function (data) {
            $.hideLoading();
            var jdata = ajaxTips(data,callback);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.hideLoading();
        },
        complete: function () {
        }
    });
}

doGet = function (url, urlParmsObj, callback) {
    $.showLoading();
    $.ajax({
        type: "GET", //GET或POST,
        async: true, //默认设置为true，所有请求均为异步请求。
        url: url,
        data: urlParmsObj,
        dataType: "text", //xml、html、script、jsonp、text
        beforeSend: function () { },
        success: function (data) {
            //我这里跳转之后，怎么返回我想要回的页面
            $.hideLoading();
            var jdata = ajaxTips(data, callback);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.hideLoading();
        },
        complete: function () {
            //hideLoading(domID);
            $.hideLoading();
        }
    });
}

//container:数据div，只负责【显示】提示信息
ajaxTips = function (json, container, callback) {
    $.toast.prototype.defaults.duration = 1000;
    if (typeof container === "function") {
        callback = container;
        container = 'undefined';
    }
    //debugger;
    //通过这种方法可将字符串转换为对象
    var jdata = json;
    if (typeof json === "string") {
        jdata = eval('(' + json + ')');
    }
    
    if (jdata.RspTypeCode == -1) {
        //错误消息提示
        if (jdata.ErrCode == "401") {
            debugger;
            $.alert(jdata.ErrMsg, "警告", function () {
                top.location.href = "/Login.html";
            });
        } else if (jdata.ErrCode == "403") {
            $.alert(jdata.ErrMsg, "警告", function () {
                callback && callback(jdata);
            });
        } else {
            //错误消息提示
            console.log(jdata.ErrCode + ":" + jdata.ErrMsg);
            $.alert(jdata.Msg, "警告", function () {
                callback && callback(jdata);
            });
        }
        return jdata;
    } else if (jdata.RspTypeCode == 1) {
        //提示信息提示
        $.alert(jdata.Msg, '成功', function () {
            callback && callback(jdata);
        });
        return jdata;
        //$.toast("操作成功", function () {
        //    console.log('close');
        //});
        //$.toptip(jdata.Msg, 'success');
    } else if (jdata.RspTypeCode == 4) {
        //跳转页面
        window.location.href = jdata.data;
        return jdata;
    } else if (jdata.RspTypeCode == 5) {
        //没数据
        showEnding(container, jdata.Msg);
    } else if (jdata.RspTypeCode == 6) {
        //数据加载完
        showEnding(container, jdata.Msg);
    }

    callback && callback(jdata);
    return jdata;
}

appendDom = function (selector, Domdata) {
    $(selector).append(Domdata);
}

clearDom = function (selector) {
    $(selector).empty();
}

//显示loading dom
//container:数据div
showLoading = function (container, type) {
    //type ：
    //  'mid' => 浮于页面中间，
    //  'append' => 页面内部append于div
    //debugger;
    type = type || 'append';
    var loadingDom;
    if (type == 'append') {
        loadingDom = $(template + '>' + loadingID).clone(true).appendTo($(container).parent());
        loadingDom.show();
    } else if (type == 'mid') {
        
    }
}

//清除loading dom
//container:数据div
hideLoading = function (container, type) {
    type = type || 'append';
    if (type == 'append') {
        $(container).siblings(loadingID).remove();
    } else if (type == 'mid') {
        $.hideLoading();
        return;
    }
}

showPaging = function(container){
    $(container).siblings(pageID).show();
}

hidePaging = function(container){
    $(container).siblings(pageID).hide();
}

showEnding = function (container, content) {
    var endDom;
    endDom = $(template + '>' + endID).clone(true).appendTo($(container).parent());
    content && endDom.children("div").first().text(content);
    endDom.show();
}

hideEnding = function (container) {
    $(container).siblings(endID).remove();
}

setTab = function (m, n) {
    var menu = document.getElementById("tab" + m).getElementsByTagName("li");
    var div = document.getElementById("tablist" + m).getElementsByTagName("div");
    var showdiv = [];
    for (i = 0; j = div[i]; i++) {
        if ((" " + div[i].className + " ").indexOf(" tablist ") != -1) {
            showdiv.push(div[i]);
        }
    }
    for (i = 0; i < menu.length; i++) {
        menu[i].className = i == n ? "weui_navbar_item weui_bar_item_on" : "weui_navbar_item";
        showdiv[i].style.display = i == n ? "block" : "none";
    }
}

//下拉分页，加载分页数据
//$(document.body).infinite().on("infinite", function () {
//    return;
//    //debugger;
//    if (loading) return;
//    var tabID = $(".weui_bar_item_on").attr("id");
//    //修改这里的id名称
//    if (tabID == "query_mystarted_btn") {
//        querymystarted();//修改这里的回调函数
//    } else if (tabID == "query_myappointing_btn") {
//        querymyappointing();
//    } else if (tabID == "query_myappointed_btn") {
//        querymyappointed();
//    } else if (tabID == "query_myappointed_btn") {
//        querymyappointed();
//    } else if (tabID == "query_myappointing_btn") {
//        querymyappointing();
//    } else if (tabID == "query_myreciving_btn") {
//        querymyreciving();
//    } else if (tabID == "query_myhandling_btn") {
//        querymyhandling();
//    } else if (tabID == "query_myhandled_btn") {
//        querymyhandled();
//    }

//});

//上拉刷新，重新加载数据
//$(document.body).pullToRefresh().on("pull-to-refresh", function () {
//    return;
//    if (myTop > 0) {
//        $(document.body).pullToRefreshDone();
//        return;
//    }
//    var tabID = $(".weui_bar_item_on").attr("id");
//    //修改这里的id名称
//    if (tabID == "query_mystarted_btn") {
//        startedQuery.isEnd = false;
//        startedQuery.pageIndex = 1;
//        querymystarted();
//    } else if (tabID == "startemyorder_btn") {
//        doClearStartForm();
//    } else if (tabID == "query_myappointing_btn") {
//        appointingQuery.isEnd = false;
//        appointingQuery.pageIndex = 1;
//        querymyappointing();
//    } else if (tabID == "query_myappointed_btn") {
//        appointedQuery.isEnd = false;
//        appointedQuery.pageIndex = 1;
//        querymyappointed();
//    } else if (tabID == "query_myreciving_btn") {
//        recivingQuery.isEnd = false;
//        recivingQuery.pageIndex = 1;
//        querymyreciving();
//    } else if (tabID == "query_myhandling_btn") {
//        handlingQuery.isEnd = false;
//        handlingQuery.pageIndex = 1;
//        querymyhandling();
//    } else if (tabID == "query_myhandled_btn") {
//        handledQuery.isEnd = false;
//        handledQuery.pageIndex = 1;
//        querymyhandled();
//    }

//    $(document.body).pullToRefreshDone();
//});

//此方法没用
formatDic = function (value, data) {
    var arr = data;
    $.each(data, function (i, item) {
        if (value == item.k) {
            return item.v;
        }
    });
}

getQueryFilters = function () {
    var filter = new Object();
    filter.status = $("#filterStatus").children(".now").attr("value");
    filter.priority = $("#filterPriority").children(".now").attr("value");
    filter.bookingTime = $("#filterBookingTime").val();
    filter.content = $("#filterContent").val();
    return filter;
}

setQueryFilters = function (query) {
    debugger;
    var article = new Article('#filter');
    doGetPartial1(article, '/Weixin/Home/FilterIndex', { tabid: query.tabID });

    $("#filterStatus").children("div").removeClass("now");
    $.each($("#filterStatus").children("div"), function (i, n) {
        if ($(this).attr("value") == query.status) {
            $(this).addClass("now");
        }
    });

    $("#filterPriority").children("div").removeClass("now");
    $.each($("#filterPriority").children("div"), function (i, n) {
        if ($(this).attr("value") == query.priority) {
            $(this).addClass("now");
        }
    })

    $("#filterBookingTime").val(query.bookingTime);
    $("#filterContent").val(query.content);
}

$.smartScroll = function (container, selectorScrollable) {
    // 如果没有滚动容器选择器，或者已经绑定了滚动时间，忽略
    if (!selectorScrollable || container.data('isBindScroll')) {
        return;
    }

    // 是否是搓浏览器
    // 自己在这里添加判断和筛选
    var isSBBrowser;

    var data = {
        posY: 0,
        maxscroll: 0
    };

    // 事件处理
    container.on({
        touchstart: function (event) {
            var events = event.touches[0] || event;

            // 先求得是不是滚动元素或者滚动元素的子元素
            var elTarget = $(event.target);

            if (!elTarget.length) {
                return;
            }

            var elScroll;

            // 获取标记的滚动元素，自身或子元素皆可
            if (elTarget.is(selectorScrollable)) {
                elScroll = elTarget;
            } else if ((elScroll = elTarget.parents(selectorScrollable)).length == 0) {
                elScroll = null;
            }

            if (!elScroll) {
                return;
            }

            // 当前滚动元素标记
            data.elScroll = elScroll;

            // 垂直位置标记
            data.posY = events.pageY;
            data.scrollY = elScroll.scrollTop();
            // 是否可以滚动
            data.maxscroll = elScroll[0].scrollHeight - elScroll[0].clientHeight;
        },
        touchmove: function () {
            // 如果不足于滚动，则禁止触发整个窗体元素的滚动
            if (data.maxscroll <= 0 || isSBBrowser) {
                // 禁止滚动
                event.preventDefault();
            }
            // 滚动元素
            var elScroll = data.elScroll;
            // 当前的滚动高度
            var scrollTop = elScroll.scrollTop();

            // 现在移动的垂直位置，用来判断是往上移动还是往下
            var events = event.touches[0] || event;
            // 移动距离
            var distanceY = events.pageY - data.posY;

            if (isSBBrowser) {
                elScroll.scrollTop(data.scrollY - distanceY);
                elScroll.trigger('scroll');
                return;
            }

            // 上下边缘检测
            if (distanceY > 0 && scrollTop == 0) {
                // 往上滑，并且到头
                // 禁止滚动的默认行为
                event.preventDefault();
                return;
            }

            // 下边缘检测
            if (distanceY < 0 && (scrollTop + 1 >= data.maxscroll)) {
                // 往下滑，并且到头
                // 禁止滚动的默认行为
                event.preventDefault();
                return;
            }
        },
        touchend: function () {
            data.maxscroll = 0;
        }
    });

    // 防止多次重复绑定
    container.data('isBindScroll', true);
};



