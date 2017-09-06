$.openMyPopup = function (popup, className) {
    myTop++;
    //弹出时禁止下拉
    $("html,body").addClass("banScroll");
    //$(document.body).pullRefresh().setStopped(true);

    popup = $(popup);
    popup.show();
    popup.width();
    popup.addClass("weui-popup-container-visible");
    var modal = popup.children(".weui-popup-modal");
    modal.width();
    modal.transitionEnd(function () {
        modal.trigger("open");
    });
}


$.closeMyPopup = function (container, remove) {
    myTop--;
    if (myTop == 0) {
        $("html,body").removeClass("banScroll");
    }
    container = $(container || ".weui-popup-container-visible");
    remove && container.remove();
    container.hide();
    container.removeClass("weui-popup-container-visible")
};