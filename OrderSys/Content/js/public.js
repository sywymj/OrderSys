
query = function (domID, url, urlParmsObj) {
    var urlParms = urlParmsObj || { pageIndex: 1, pageCount: 20 };
    $.ajax({
        type: "GET",
        url: url,
        data: urlParms,
        success: function (data) {
            //判断返回值不是 json 格式
            if (!data.match("^\{(\n?.+:.+,?\n?){1,}\}$")) {
                addDom(domID, data);
            }
            else {
                var jdata = ajaxTips(data);
                if (jdata.RspType == 0) {
                    //底部提示没有消息了
                }
            }
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

ajaxTips = function (json) {
    //通过这种方法可将字符串转换为对象
    var jdata = eval('(' + json + ')');
    console.log(jdata.ErrCode + ":" + jdata.ErrMsg);
    if (jdata.RspTypeCode == -1) {
        //错误消息提示
        $.alert(jdata.Msg);
    } else if (jdata.RspTypeCode == 1) {
        //提示信息提示
        $.alert(jdata.Msg);
    } else if (jdata.RspTypeCode == 4) {
        //跳转页面
        window.location.href = jdata.data;
    }
    return jdata
}

addDom = function (id, data) {
    $('#' + id).append(data);
}

clearDom = function (id) {
    $('#' + id).html('');
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

//此方法没用
formatDic = function (value, data) {
    var arr = data;
    $.each(data, function (i, item) {
        if (value == item.k) {
            return item.v;
        }
    });
}
