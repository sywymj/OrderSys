var loading = false;//用来监听鼠标下滚事件
var loadingID = '.loading';
var refreshID = '.refreshing'
var endID = '.end';
var end = false;

doQuery = function (domID, url, urlParmsObj, callback) {
    if (end) { return; }

    //add loading
    showLoading(domID);

    var argumentLength = arguments.length
    var urlParms = urlParmsObj || { PageIndex: 1, PageSize: 20 };

    $.ajax({
        type: "GET",
        url: url,
        data: urlParms,
        success: function (data) {
            //判断返回值不是 json 格式
            if (!data.match("^\{(\n?.+:.+,?\n?){1,}\}$")) {
                hideEnding(domID);
                appendDom(domID, data);
            }
            else {
                var jdata = ajaxTips(data);
                if (jdata.RspTypeCode == 6) {
                    //底部提示没有数据了
                    showEnding(domID,jdata.Msg);
                }
                if (jdata.RspTypeCode == 5) {
                    //底部提示没有数据了
                    showEnding(domID,jdata.Msg);
                }
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
            hideLoading(domID);
        }
    })
}

doGetPartial = function (domID, url, urlParmsObj) {
    clearDom(domID);
    var urlParms = urlParmsObj;
    $.ajax({
        type: "GET",
        url: url,
        async: false, //默认设置为true，所有请求均为异步请求。
        data: urlParms,
        success: function (data) {
            //判断返回值不是 json 格式
            if (!data.match("^\{(\n?.+:.+,?\n?){1,}\}$")) {
                appendDom(domID, data);
            }
            else {
                var jdata = ajaxTips(data);
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

ajaxTips = function (json,callback) {
    //通过这种方法可将字符串转换为对象
    var jdata = eval('(' + json + ')');
    console.log(jdata.ErrCode + ":" + jdata.ErrMsg);
    if (jdata.RspTypeCode == -1) {
        //错误消息提示
        $.toast(jdata.Msg, "forbidden");
    } else if (jdata.RspTypeCode == 1) {
        //提示信息提示
        $.toast(jdata.Msg);
    } else if (jdata.RspTypeCode == 4) {
        //跳转页面
        window.location.href = jdata.data;
    }
    if (arguments.length == 2) {
        callback(jdata);
    }
    return jdata
}

appendDom = function (DomID, Domdata) {
    $('#' + DomID).append(Domdata);
}

clearDom = function (DomID) {
    $('#' + DomID).empty();
}

//在某个dom同辈添加loading
showLoading = function (domID) {
    $('#' + domID).siblings(loadingID).show();
    loading = true;
}

//清除某个dom同辈面的loading
hideLoading = function (domID) {
    $('#' + domID).siblings(loadingID).hide();
    loading = false;
}

showRefresh = function (domID) {
    $('#' + domID).siblings(refreshID).show();
}

hideRefresh = function (domID) {
    $('#' + domID).siblings(refreshID).hide();
}

showEnding = function (domID,content) {
    $('#' + domID).siblings(endID).show();
    $('#' + domID).siblings(endID).children("div").first().text(content);
    end = true;
}

hideEnding = function (domID) {
    $('#' + domID).siblings(endID).hide();
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
    if (tabID == "querymystarted-btn") {
        querymystarted();//修改这里的回调函数
    } else if (tabID == "query_myappointing_btn") {
        querymyappointing();
    } else if (tabID == "query_myappointed_btn") {
        querymyappointed();
    }

});

//上拉刷新，重新加载数据
$(document.body).pullToRefresh().on("pull-to-refresh", function () {

    var tabID = $(".weui_bar_item_on").attr("id");
    //修改这里的id名称
    if (tabID == "querymystarted-btn") {
        pageIndex = 1;
        end = false;
        querymystarted();
    } else if (tabID == "startemyorder-btn") {
        doClearStartForm();
    } else if (tabID == "query_myappointing_btn") {
        pageIndex = 1;
        end = false;
        querymyappointing();
    } else if (tabID == "query_myappointed_btn") {
        pageIndex = 1;
        end = false;
        querymyappointed();
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
