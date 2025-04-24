window.stripeInterop = (function () {
    let stripe;
    let cardNumber, cardExpiry, cardCvc, postalCode;

    function initStripe(publishableKey) {
        stripe = Stripe(publishableKey);
        const elements = stripe.elements();

        cardNumber = elements.create("cardNumber");
        cardNumber.mount("#card-number-element");

        cardExpiry = elements.create("cardExpiry");
        cardExpiry.mount("#card-expiry-element");

        cardCvc = elements.create("cardCvc");
        cardCvc.mount("#card-cvc-element");

        postalCode = elements.create("postalCode");
        postalCode.mount("#card-postal-element");

        [cardNumber, cardExpiry, cardCvc, postalCode].forEach(element => {
            element.on("change", function (event) {
                const displayError = document.getElementById("card-errors");
                displayError.textContent = event.error ? event.error.message : "";
            });
        });
    }

    async function createPaymentMethod(name, email) {
        const result = await stripe.createPaymentMethod({
            type: "card",
            card: cardNumber,
            billing_details: {
                name: name,
                email: email
            }
        });

        if (result.error) {
            document.getElementById("card-errors").textContent = result.error.message;
            return null;
        }

        return result.paymentMethod.id;
    }

    return {
        initStripe,
        createPaymentMethod
    };
})();