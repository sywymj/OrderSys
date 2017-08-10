
query = function (domID, url, urlParms) {
    var urlParms = { pageIndex: 1, pageCount: 10 } || urlParms;
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
                //通过这种方法可将字符串转换为对象
                var jdata = eval('(' + data + ')');
                console.log(jdata.ErrCode + ":" + jdata.ErrMsg);
                $.alert(jdata.Msg);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })
}

addDom = function (id, data) {
    $('#' + id).append(data);
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
