var loading = false;//用来监听鼠标下滚事件
var loadingID = '.loading';
var refreshID = '.refreshing'
var endID = '.end';
var end = false;

doQuery = function (container, url, urlParmsObj, callback) {
    if (end) { return; }

    //add loading
    showLoading(container);

    var argumentLength = arguments.length
    var urlParms = urlParmsObj || { PageIndex: 1, PageSize: 20 };

    $.ajax({
        type: "GET",
        url: url,
        data: urlParms,
        success: function (data) {
            //判断返回值不是 json 格式
            if (!data.match("^\{(\n?.+:.+,?\n?){1,}\}$")) {
                hideEnding(container);
                appendDom(container, data);
            }
            else {
                var jdata = ajaxTips(data, container);

            }
            //整个分页效果显示完后的回调函数
            if (argumentLength == 4) {
                callback();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        },
        complete: function () {
            hideLoading(container);
        }
    })
}

doGetPartial = function (container, url, urlParmsObj) {
    clearDom(container);
    var urlParms = urlParmsObj;
    $.ajax({
        type: "GET",
        url: url,
        async: false, //默认设置为true，所有请求均为异步请求。
        data: urlParms,
        success: function (data) {
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
        }
    })
}

doSubmit = function (url, postObj,callback) {
    //showLoading(domID);
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
        }
    });
}

doGet = function (url, urlParmsObj, callback) {
    $.ajax({
        type: "GET",
        url: url,
        data: urlParmsObj,
        success: function (data) {
            var jdata = ajaxTips(data, callback);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })
}

doPost = function (url, postData, callback) {
    $.ajax({
        type: "POST",
        url: url,
        data: postData,
        success: function (data) {
            var jdata = ajaxTips(data, callback);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })
}

getDicData = function (url) {
    var dicData;
    $.ajax({
        type: "GET",
        url: url,
        async: false, //默认设置为true，所有请求均为异步请求。
        success: function (data) {
            dicData = ajaxTips(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })

    return dicData.Data;
}

ajaxTips = function (json, container, callback) {
    if (typeof container === "function") {
        container = undefined;
        callback = container;
    }
    debugger;
    //通过这种方法可将字符串转换为对象
    var jdata = eval('(' + json + ')');

    if (jdata.RspTypeCode == -1) {
        //错误消息提示
        console.log(jdata.ErrCode + ":" + jdata.ErrMsg);
        $.toast(jdata.Msg, "forbidden");
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
    callback && callback();
    return jdata
}

appendDom = function (selector, Domdata) {
    $(selector).append(Domdata);
}

clearDom = function (selector) {
    $(selector).empty();
}

//在某个dom同辈添加loading
showLoading = function (container) {
    $(container).siblings(loadingID).show();
    loading = true;
}

//清除某个dom同辈面的loading
hideLoading = function (container) {
    $(container).siblings(loadingID).hide();
    loading = false;
}

showRefresh = function (container) {
    $(container).siblings(refreshID).show();
}

hideRefresh = function (container) {
    $(container).siblings(refreshID).hide();
}

showEnding = function (container,content) {
    $(container).siblings(endID).show();
    $(container).siblings(endID).children("div").first().text(content);
    end = true;
}

hideEnding = function (container) {
    $(container).siblings(endID).hide();
    end = false;
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

    var tabID = $(".weui_bar_item_on").attr("id");
    //修改这里的id名称
    if (tabID == "query_mystarted_btn") {
        debugger;
        pageIndex = 1;
        end = false;
        querymystarted();
    } else if (tabID == "startemyorder_btn") {
        doClearStartForm();
    } else if (tabID == "query_myappointing_btn") {
        pageIndex = 1;
        end = false;
        querymyappointing();
    } else if (tabID == "query_myappointed_btn") {
        pageIndex = 1;
        end = false;
        querymyappointed();
    } else if (tabID == "query_myreciving_btn") {
        pageIndex = 1;
        end = false;
        querymyreciving();
    } else if (tabID == "query_myhandling_btn") {
        pageIndex = 1;
        end = false;
        querymyhandling();
    } else if (tabID == "query_myhandled_btn") {
        pageIndex = 1;
        end = false;
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
