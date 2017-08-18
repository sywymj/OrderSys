$.openPopup = function (popup, className) {

    //$.closePopup();

    popup = $(popup);
    popup.show();
    popup.width();
    popup.addClass("weui-popup__container--visible");
    var modal = popup.find(className);
    modal.width();
    modal.transitionEnd(function () {
        modal.trigger("open");
    });
}


$.closePopup = function (container, remove) {
    container = $(container || ".weui-popup__container--visible");
    remove && container.remove();
    container.hide();
    container.removeClass("weui-popup__container--visible")
};