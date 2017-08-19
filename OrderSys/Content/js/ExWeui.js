$.openMyPopup = function (popup, className) {

    //$.closePopup();

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
    container = $(container || ".weui-popup-container-visible");
    remove && container.remove();
    container.hide();
    container.removeClass("weui-popup-container-visible")
};