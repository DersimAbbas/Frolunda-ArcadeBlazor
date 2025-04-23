window.stripeInterop = (function () {
    let stripe;
    let card;

    function initStripe(publishableKey) {
        stripe = Stripe(publishableKey);
        const elements = stripe.elements();
        card = elements.create("card");
        card.mount("#card-element");

        card.on("change", function (event) {
            const displayError = document.getElementById("card-errors");
            displayError.textContent = event.error ? event.error.message : "";
        });
    }

    async function createPaymentMethod(name, email) {
        const result = await stripe.createPaymentMethod({
            type: "card",
            card: card,
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