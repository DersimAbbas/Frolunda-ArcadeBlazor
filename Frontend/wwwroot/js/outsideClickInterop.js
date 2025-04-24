window.outsideClickInterop = (function () {
    let handler = null;

    return {
        register: function (elementId, dotNetHelper, dotNetMethod, exceptionIds = []) {
            if (handler) {
                document.removeEventListener('click', handler);
            }

            handler = function (e) {
                const element = document.getElementById(elementId);
                const exceptions = exceptionIds
                    .map(id => document.getElementById(id))
                    .filter(el => el !== null);

                const isClickInside = element && element.contains(e.target);
                const isClickOnException = exceptions.some(el => el.contains(e.target));

                if (!isClickInside && !isClickOnException) {
                    dotNetHelper.invokeMethodAsync(dotNetMethod);
                }
            };

            document.addEventListener('click', handler);
        },
        unregister: function () {
            if (handler) {
                document.removeEventListener('click', handler);
                handler = null;
            }
        }
    };
})();