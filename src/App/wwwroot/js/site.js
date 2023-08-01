function OpenModal() {
    $(document).ready(function () {
        $(function () {
            $.ajaxSetup({ cache: false });

            $("a[data-modal]").on("click", // Quando o link (a) que possuir a propriedade (data-modal) for clicado (on(click)) executa a função
                function (e) {
                    $('#myModalContent').load(this.href,
                        function () {
                            $('#myModal').modal({
                                keyboard: true
                            },
                                'show');
                            bindForm(this);
                        });
                    return false;
                });
        });
        function bindForm(dialog) {
            $('form', dialog).submit(function () {
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.success) {
                            $('#myModal').modal('hide');
                            $('#AddressTarget').load(result.url); // Carrega o resultado HTML para a div demarcada
                        } else {
                            $('#myModalContent').html(result);
                            bindForm(dialog);
                        }
                    }
                });
                return false;
            });
        }
    });
}

function SearchZipCode() {
    $(document).ready(function () {
        function cleanZipCodeForm() {
            // Limpa valores do formulário de cep.
            $("#Address_PublicPlace").val("");
            $("#Address_Neighborhood").val("");
            $("#Address_City").val("");
            $("#Address_State").val("");
        }
        //Quando o campo cep perde o foco.
        $("#Address_ZipCode").blur(function () {
            //Nova variável "cep" somente com dígitos.
            var zipCode = $(this).val().replace(/\D/g, '');
            //Verifica se campo cep possui valor informado.
            if (zipCode != "") {
                //Expressão regular para validar o CEP.
                var validacep = /^[0-9]{8}$/;
                //Valida o formato do CEP.
                if (validacep.test(zipCode)) {
                    //Preenche os campos com "..." enquanto consulta webservice.
                    $("#Address_PublicPlace").val("...");
                    $("#Address_Neighborhood").val("...");
                    $("#Address_City").val("...");
                    $("#Address_State").val("...");
                    //Consulta o webservice viacep.com.br/
                    $.getJSON("https://viacep.com.br/ws/" + zipCode + "/json/?callback=?",
                        function (dados) {
                            if (!("erro" in dados)) {
                                //Atualiza os campos com os valores da consulta.
                                $("#Address_PublicPlace").val(dados.logradouro);
                                $("#Address_Neighborhood").val(dados.bairro);
                                $("#Address_City").val(dados.localidade);
                                $("#Address_State").val(dados.uf);
                            }
                            else {
                                //CEP pesquisado não foi encontrado.
                                cleanZipCodeForm();
                                alert("CEP não encontrado.");
                            }
                        }
                    );
                }
                else {
                    //cep é inválido.
                    cleanZipCodeForm();
                    alert("Formato de CEP inválido.");
                }
            }
            else {
                //cep sem valor, limpa formulário.
                cleanZipCodeForm();
            }
        });
    });
}

$(document).ready(function () {
    $("#msg_box").fadeOut(2500);
});