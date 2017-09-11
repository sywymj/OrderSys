doGet = function (url, urlParmsObj, callback) {
    showLoading();
    $.ajax({
        type: "GET", //GET或POST,
        async: true, //默认设置为true，所有请求均为异步请求。
        url: url,
        data: urlParmsObj,
        dataType: "text", //xml、html、script、jsonp、text
        beforeSend: function () { },
        success: function (data) {
            //我这里跳转之后，怎么返回我想要回的页面
            var jdata = ajaxTips(data, callback);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
        complete: function () {
            hideLoading();
        }
    });
}

doGetSync = function (url, urlParmsObj, callback) {
    showLoading();
    $.ajax({
        type: "GET", //GET或POST,
        async: false, //默认设置为true，所有请求均为异步请求。
        url: url,
        data: urlParmsObj,
        dataType: "text", //xml、html、script、jsonp、text
        beforeSend: function () { },
        success: function (data) {
            //我这里跳转之后，怎么返回我想要回的页面
            var jdata = ajaxTips(data, callback);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
        complete: function () {
            hideLoading();
        }
    });
}

doPost = function (url, urlParmsObj, callback) {
    showLoading();
    $.ajax({
        type: "Post", //GET或POST,
        async: true, //默认设置为true，所有请求均为异步请求。
        url: url,
        data: urlParmsObj,
        dataType: "text", //xml、html、script、jsonp、text
        beforeSend: function () { },
        success: function (data) {
            //我这里跳转之后，怎么返回我想要回的页面
            var jdata = ajaxTips(data, callback);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
        complete: function () {
            hideLoading();
        }
    });
}

ajaxTips = function (json, container, callback) {
    if (typeof container === "function") {
        callback = container;
        container = 'undefined';
    }
    debugger;
    //通过这种方法可将字符串转换为对象
    var jdata = eval('(' + json + ')');
    if (jdata.RspTypeCode == -1) {
        //错误消息提示
        console.log(jdata.ErrCode + ":" + jdata.ErrMsg);
        mini.alert(jdata.Msg);
    } else if (jdata.RspTypeCode == 1) {
        //提示信息提示
        mini.showTips({
            content: jdata.Msg,
            state: 'success',
            x: 'center',
            y: 'top',
            timeout: 3000
        });
    } else if (jdata.RspTypeCode == 2) {
        //提示信息提示
        mini.showTips({
            content: jdata.Msg,
            state: 'success',
            x: 'center',
            y: 'top',
            timeout: 3000
        });
    }else if (jdata.RspTypeCode == 4) {
        //跳转页面
        window.location.href = jdata.data;
        return jdata;
    } else if (jdata.RspTypeCode == 5) {
        //没数据
    } else if (jdata.RspTypeCode == 6) {
        //数据加载完
    }

    callback && callback(jdata);
}

showLoading = function () {
    mini.mask({
        el: document.body,
        cls: 'mini-mask-loading',
        html: '加载中...'
    });
}

hideLoading = function () {
    mini.unmask(document.body);
}