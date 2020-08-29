$(document).ready(function () {
    SetNavigation();
    ToUpperCase();
});

function SetNavigation() {
    var pathname = window.location.pathname;

    $('#navigation > li > a[href="' + pathname + '"]').parent().addClass('active');

    $('#navigation > li > ul.sub-menu > li > a[href="' + pathname + '"]').closest('.has-sub').addClass('active');
    $('#navigation > li > ul.sub-menu > li > a[href="' + pathname + '"]').parent().addClass('active');
}

function ToUpperCase() {
    $('input[type=text]').keyup(function () {
        this.value = this.value.toUpperCase();
    });
    $('textarea').keyup(function () {
        this.value = this.value.toUpperCase();
    });
    $('input[type=email]').keyup(function () {
        this.value = this.value.toLowerCase();
    });
    $('input[data-lowercase]').keyup(function () {
        this.value = this.value.toLowerCase();
    });
};

function ExibirImagemPrincipal(src) {
    $("#imgPrincipal").attr("src", src);
}

function AjaxModalBindTarget() {
    $(function () {
        $.ajaxSetup({ cache: false });

        $("a[data-modal-target]").off("click");

        $("a[data-modal-target]").on("click",
            function (e) {
                $('#myModalContent').load(this.href,
                    function () {
                        $('#myModal').modal({
                            keyboard: true
                        },
                            'show');
                        BindTarget(this);
                    });
                return false;
            });
    });
}
function BindTarget(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    $('#ElementTarget').load(result.url);
                } else {
                    $('#myModalContent').html(result);
                    BindTarget(dialog);
                }
            }
        });
        return false;
    });
}

function AjaxModalBindRedirect() {
    $(function () {
        $.ajaxSetup({ cache: false });

        $("a[data-modal-redirect]").off("click");

        $("a[data-modal-redirect]").on("click",
            function (e) {
                $('#myModalContent').load(this.href,
                    function () {
                        $('#myModal').modal({
                            keyboard: true
                        },
                            'show');
                        BindRedirect(this);
                    });
                return false;
            });
    });
}
function BindRedirect(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    window.location.replace(result.url);
                } else {
                    $('#myModalContent').html(result);
                    BindRedirect(dialog);
                }
            }
        });
        return false;
    });
}

function AjaxModalBindModal() {
    $(function () {
        $.ajaxSetup({ cache: false });

        $("a[data-modal-modal]").off("click");

        $("a[data-modal-modal]").on("click",
            function (e) {
                $('#myModalContent').load(this.href,
                    function () {
                        $('#myModal').modal({
                            keyboard: true
                        },
                            'show');
                        BindModal(this);
                    });
                return false;
            });
    });
}
function BindModal(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    $('#myModalContent').html(result);
                } else {
                    $('#myModalContent').html(result);
                    BindModal(dialog);
                }
            }
        });
        return false;
    });
}

function AjaxModalNoBind() {
    $(function () {
        $.ajaxSetup({ cache: false });

        $("a[data-modal-nobind]").off("click");

        $("a[data-modal-nobind]").on("click",
            function (e) {
                $('#myModalContent').load(this.href,
                    function () {
                        $('#myModal').modal({
                            keyboard: true
                        },
                            'show');
                    });
                return false;
            });
    });
}

function AjaxModalTarefa() {
    $(function () {
        $.ajaxSetup({ cache: false });

        $("a[data-modal-tarefa]").off("click");

        $("a[data-modal-tarefa]").on("click",
            function (e) {
                $('#myModalContent').load(this.href,
                    function () {
                        $('#myModal').modal({
                            keyboard: true
                        },
                            'show');
                        BindTarefa(this);
                    });
                return false;
            });
    });
}
function BindTarefa(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    $('#ElementTargetTarefas').load(result.url);
                } else {
                    $('#myModalContent').html(result);
                    BindTarefa(dialog);
                }
            }
        });
        return false;
    });
}

function AjaxModalContato() {
    $(function () {
        $.ajaxSetup({ cache: false });

        $("a[data-modal-contato]").off("click");

        $("a[data-modal-contato]").on("click",
            function (e) {
                $('#myModalContent').load(this.href,
                    function () {
                        $('#myModal').modal({
                            keyboard: true
                        },
                            'show');
                        BindContato(this);
                    });
                return false;
            });
    });
}
function BindContato(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    $('#ElementTargetContatos').load(result.url);
                } else {
                    $('#myModalContent').html(result);
                    BindContato(dialog);
                }
            }
        });
        return false;
    });
}

function BuscarCep() {
    function limpa_formulário_cep() {
        $("#Logradouro").val("");
        $("#Bairro").val("");
        $("#Cidade").val("");
        $("#Pais").val("");
        $("#Estado").val("");
        $("#EnderecoExterior").prop("checked", false);
    }

    $("#Cep").blur(function () {

        var cep = $(this).val().replace(/\D/g, '');

        if (cep != "") {

            var validacep = /^[0-9]{8}$/;

            if (validacep.test(cep)) {

                $("#Logradouro").val("...");
                $("#Bairro").val("...");
                $("#Cidade").val("...");
                $("#Pais").val("...");
                $("#Estado").val("...");

                $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                    function (dados) {

                        if (!("erro" in dados)) {
                            $("#Logradouro").val(dados.logradouro.toUpperCase());
                            $("#Bairro").val(dados.bairro.toUpperCase());
                            $("#Cidade").val(dados.localidade.toUpperCase());
                            $("#Pais").val("BR");
                            $("#Estado").val(dados.uf);
                            $("#EnderecoExterior").prop("checked", false);
                        }
                        else {
                            limpa_formulário_cep();
                            $("#Logradouro").val("CEP não encontrado");
                        }
                    });
            }
            else {
                limpa_formulário_cep();
            }
        }
        else {
            limpa_formulário_cep();
        }
    });
}

function TratarEnderecoExterior() {

    $("#EnderecoExterior").click(function () {
        if ($(this).prop("checked")) {
            $("#Cep").val("99999-999");
            $("#Logradouro").val("");
            $("#LogradouroNumero").val("");
            $("#Complemento").val("");
            $("#Bairro").val("");
            $("#Cidade").val("");
            $("#Pais").val("EX");
            $("#Estado").val("EX");
        }
        else {
            $("#Cep").val("");
            $("#Logradouro").val("");
            $("#LogradouroNumero").val("");
            $("#Complemento").val("");
            $("#Bairro").val("");
            $("#Cidade").val("");
            $("#Pais").val("");
            $("#Estado").val("");
        }
    });
}

function ObterDadosCliente() {
    function LimparDadosCliente() {
        $("#Contribuinte").prop("checked", false);
        $("#ClienteTelefone").val("");
        $("#ClienteNiver").val("");
        $("#ClienteEmail").val("");
    }

    function TratarDataNascimento(dataNascimento) {
        if (dataNascimento === "" || dataNascimento === null) {
            return "";
        }
        else {
            var data = new Date(dataNascimento);
            return data.toLocaleDateString("pt-BR");
        }
    }

    $("#ClienteNome").blur(function () {
        var nome = $(this).val();

        if (nome !== "") {
            $("#ClienteTelefone").val("...");
            $("#ClienteNiver").val("...");
            $("#ClienteEmail").val("...");

            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/cliente-obterdados",
                data: { nome: nome },
                success: function (data) {
                    if (data.success) {
                        var cliente = JSON.parse(data.cliente);
                        $("#Contribuinte").prop("checked", cliente["Contribuinte"]);
                        $("#ClienteTelefone").val(cliente["Telefone3"]).mask("(99) 9999-9999?9");
                        $("#ClienteNiver").val(TratarDataNascimento(cliente["DataNascimento"]));
                        $("#ClienteNiver").datepicker("setDate", TratarDataNascimento(cliente["DataNascimento"]));
                        $("#ClienteEmail").val(cliente["Email"]);
                    }
                    else {
                        LimparDadosCliente();
                    }
                }
            });
        }
        else {
            LimparDadosCliente();
        }
    });
}