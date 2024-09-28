function CreditCard() {


    if ($("#card_number").length > 0) {
        $("#card_number").validateCreditCard(function (e) {

            $(this).removeClass();
            $(this).addClass("form-control");

            if (e.card_type !== null)
                $(this).addClass(e.card_type.name);

            if (e.valid)
                $(this).addClass("valid");
            else
                $(this).removeClass("valid");

            if (e.length_valid && !e.valid) {
                $(this).addClass("invalid");
            } else {
                $(this).removeClass("invalid");
            }
        });
    }
}

function BuscaCep() {

    const cep = document.querySelector("input[name=ZipCode]");

    cep.addEventListener('blur', e => {
        const value = cep.value.replace(/[^0-9]+/, '');
        const url = `https://viacep.com.br/ws/${value}/json/`;

        fetch(url)
            .then(response => response.json())
            .then(json => {

                if (json.logradouro) {
                    document.querySelector('input[name=StreetAddress]').value = json.logradouro;
                    document.querySelector('input[name=Neighborhood]').value = json.bairro;
                    document.querySelector('input[name=City]').value = json.localidade;
                    document.querySelector('input[name=State]').value = json.uf;
                }

            });

    });
}

