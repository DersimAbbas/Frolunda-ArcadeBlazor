window.cartInterop = (function () {
    let handler = null;

    return {
        registerOutsideClick: function (cartElementId, dotNetHelper, toggleButtonId) {
            if (handler) {
                document.removeEventListener('click', handler);
            }

            handler = function (e) {
                const cart = document.getElementById(cartElementId);
                const toggleBtn = document.getElementById(toggleButtonId);

                if (
                    cart &&
                    !cart.contains(e.target) &&
                    toggleBtn &&
                    !toggleBtn.contains(e.target)
                ) {
                    dotNetHelper.invokeMethodAsync('CloseCart');
                }
            };

            document.addEventListener('click', handler);
        },
        unregisterOutsideClick: function () {
            if (handler) {
                document.removeEventListener('click', handler);
                handler = null;
            }
        }
    };
})();