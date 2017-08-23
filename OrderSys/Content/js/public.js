var loading = false;//用来监听鼠标下滚事件
var template = '#template';
var loadingID = '.loading';
var refreshID = '.refreshing'
var endID = '.end';

function Query(container,pageSize) {
    this.container = container;
    this.pageSize = pageSize || 5;
}
Query.prototype = {
    container:"",
    pageIndex: 1,
    pageSize: 5,
    isLoading: false,
    isEnd: false,

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
    loading = true;
    //if (typeof query !== 'Query') { throw new Error('query: query must be a Object "Query"'); }
    var container = query.container;

    if (query.pageIndex == 1) {
        //重新刷新
        query.isLoading = false;
        query.isEnd = false;
        clearDom(container);
        clearDom(container+"~"+"div");//清除旁边的加载中等元素
    }

    if (query.isLoading) { return;}
    if (query.isEnd) { return; }

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
            hideLoading(container, 'append');//直接remove
            query.isLoading = false;
            loading = false;
        }
    })
}

doGetPartial1 = function (article,url,parms, callback) {
    //debugger;
    if (article.isLoading) { return; }

    var container = article.container;
    clearDom(container);
    clearDom(container + "~" + "div");//清除旁边的加载中等元素

    showLoading(container, 'append');
    article.isLoading = true;
    //debugger;
    $.ajax({
        type: "GET",
        url: url,
        data: parms,
        async: false, //默认设置为true，所有请求均为异步请求。
        success: function (data) {
            //debugger;
            //判断返回值不是 json 格式
            if (!data.match("^\{(\n?.+:.+,?\n?){1,}\}$")) {
                appendDom(container, data);
            }
            else {
                var jdata = ajaxTips(data, container,callback);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        },
        complete: function () {
            hideLoading(container, 'append');//直接remove
            article.isLoading = false;
        }
    })
}

doSubmit = function (url, postObj,callback) {
    $.showLoading();
    $.ajax({
        type: "POST", //GET或POST,
        async: true, //默认设置为true，所有请求均为异步请求。
        url: url,
        data: postObj,
        dataType: "text", //xml、html、script、jsonp、text
        beforeSend: function () { },
        complete: function () { },
        success: function (data) {
            //我这里跳转之后，怎么返回我想要回的页面
            var jdata = ajaxTips(data,callback);
        },
        error: function () { },
        complete: function () {
            //hideLoading(domID);
            $.hideLoading();
        }
    });
}

doGet = function (url, urlParmsObj, callback) {
    //$.showLoading();
    $.ajax({
        type: "GET", //GET或POST,
        async: true, //默认设置为true，所有请求均为异步请求。
        url: url,
        data: urlParmsObj,
        dataType: "text", //xml、html、script、jsonp、text
        beforeSend: function () { },
        complete: function () { },
        success: function (data) {
            //我这里跳转之后，怎么返回我想要回的页面
            var jdata = ajaxTips(data, callback);
        },
        error: function () { },
        complete: function () {
            debugger;
            //hideLoading(domID);
            //$.hideLoading();
        }
    });
}

//container:数据div，只负责【显示】提示信息
ajaxTips = function (json, container, callback) {
    if (typeof container === "function") {
        callback = container;
        container = 'undefined';
    }
    //debugger;
    //通过这种方法可将字符串转换为对象
    var jdata = eval('(' + json + ')');

    if (jdata.RspTypeCode == -1) {
        //错误消息提示
        console.log(jdata.ErrCode + ":" + jdata.ErrMsg);
        $.alert(jdata.Msg, "错误");
    } else if (jdata.RspTypeCode == 1) {
        //提示信息提示
        $.toast(jdata.Msg);
    } else if (jdata.RspTypeCode == 4) {
        //跳转页面
        window.location.href = jdata.data;
    } else if (jdata.RspTypeCode == 5) {
        //没数据
        showEnding(container, jdata.Msg);
    } else if (jdata.RspTypeCode == 6) {
        //数据加载完
        showEnding(container, jdata.Msg);
    }
    callback && callback(jdata);
    return jdata
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

showEnding = function (container, content) {
    var endDom;
    endDom = $(template + '>' + endID).clone(true).appendTo($(container).parent());
    endDom.children("div").first().text(content);
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
$(document.body).infinite().on("infinite", function () {
    //debugger;
    if (loading) return;
    var tabID = $(".weui_bar_item_on").attr("id");
    //修改这里的id名称
    if (tabID == "query_mystarted_btn") {
        querymystarted();//修改这里的回调函数
    } else if (tabID == "query_myappointing_btn") {
        querymyappointing();
    } else if (tabID == "query_myappointed_btn") {
        querymyappointed();
    } else if (tabID == "query_myappointed_btn") {
        querymyappointed();
    } else if (tabID == "query_myappointing_btn") {
        querymyappointing();
    } else if (tabID == "query_myreciving_btn") {
        querymyreciving();
    } else if (tabID == "query_myhandling_btn") {
        querymyhandling();
    } else if (tabID == "query_myhandled_btn") {
        querymyhandled();
    }

});

//上拉刷新，重新加载数据
$(document.body).pullToRefresh().on("pull-to-refresh", function () {
    if (myTop > 0) {
        $(document.body).pullToRefreshDone();
        return;
    }
    var tabID = $(".weui_bar_item_on").attr("id");
    //修改这里的id名称
    if (tabID == "query_mystarted_btn") {
        startedQuery.isEnd = false;
        startedQuery.pageIndex = 1;
        querymystarted();
    } else if (tabID == "startemyorder_btn") {
        doClearStartForm();
    } else if (tabID == "query_myappointing_btn") {
        appointingQuery.isEnd = false;
        appointingQuery.pageIndex = 1;
        querymyappointing();
    } else if (tabID == "query_myappointed_btn") {
        appointedQuery.isEnd = false;
        appointedQuery.pageIndex = 1;
        querymyappointed();
    } else if (tabID == "query_myreciving_btn") {
        recivingQuery.isEnd = false;
        recivingQuery.pageIndex = 1;
        querymyreciving();
    } else if (tabID == "query_myhandling_btn") {
        handlingQuery.isEnd = false;
        handlingQuery.pageIndex = 1;
        querymyhandling();
    } else if (tabID == "query_myhandled_btn") {
        handledQuery.isEnd = false;
        handledQuery.pageIndex = 1;
        querymyhandled();
    }

    $(document.body).pullToRefreshDone();
});

//此方法没用
formatDic = function (value, data) {
    var arr = data;
    $.each(data, function (i, item) {
        if (value == item.k) {
            return item.v;
        }
    });
}

/*
*   根据习惯：ending,loading都是放在container div 后面（数据、内容部分）
*   
*
*/

