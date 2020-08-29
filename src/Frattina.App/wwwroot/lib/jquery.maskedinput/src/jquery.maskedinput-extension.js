$(document).ready(function () {
    $(".mask-date").mask("99/99/9999");

    $(".mask-datetime").mask("99/99/9999 99:99");

    $(".mask-phone").mask("(99) 9999-9999?9").on("focusout", function () {
        var len = this.value.replace(/\D/g, '').length;
        $(this).mask(len > 10 ? "(99) 99999-999?9" : "(99) 9999-9999?9");
    });

    $(".mask-cpf").mask("999.999.999-99");

    $(".mask-cnpj").mask("99.999.999/9999-99");

    $(".mask-cep").mask("99999-999");
});