var IdUsuario = null;

$(function () {
    getDados();

    $("#Cep").blur(function () {
        var cep = $(this).val().replace(/\D/g, '');

        if (cep !== "") {
            var validacep = /^[0-9]{8}$/;
            if (validacep.test(cep)) {
                $("#Rua").val("...");
                $("#Bairro").val("...");
                $("#Cidade").val("...");
                $("#Estado").val("...");
                $("#ibge").val("...");
                $("#Pais").val("...");

                $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {
                    if (!("erro" in dados)) {
                        $("#Rua").val(dados.logradouro);
                        $("#Bairro").val(dados.bairro);
                        $("#Cidade").val(dados.localidade);
                        $("#Estado").val(dados.uf);
                        $("#ibge").val(dados.ibge);
                        $("#Pais").val("Brasil");
                    }
                    else {
                        limpaFormularioCEP();
                        alert("CEP não encontrado.");
                    }
                });
            }
            else {
                limpaFormularioCEP();
                alert("Formato de CEP inválido.");
            }
        }
        else {
            limpaFormularioCEP();
        }
    });
});

function getDados() {

    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/Home/Index",
        data: {},
        beforeSend: function () {
            $(".loader-card").removeClass("hidden");
            $("#table-content").addClass("hidden");
            $(".table-nodata").addClass("hidden");
        },
        success: function (result) {
            var data = JSON.parse(result.Data.Data);

            var html = "";
            if (data !== null) {

                if (data.ListUsuarios.length > 0) {

                    data.ListUsuarios.forEach(function (item) {
                        html += "<tr>";
                        html += "     <td class='text-left'>" + item.Nome + "</td>";
                        html += "     <td class='text-left'>" + item.Marca + "</td>";
                        html += "     <td class='text-left'>" + item.Modelo + "</td>";
                        html += "     <td class='text-left'>" + item.Placa + "</td>";
                        html += "     <td class='text-left'>" + getStatus(item.StatusCaminhao) + "</td>";
                        html += "     <td class='text-left'>" + getStatus(item.StatusMotorista) + "</td>";
                        html += "     <td class='text-left'>" + getHtmButton(item.Id) + "</td>";
                        html += "</tr>";
                    });

                    $('#table-dados').html(html);

                    $("#myInput").on("keyup", function () {
                        var value = $(this).val().toLowerCase();
                        $("#table-dados tr").filter(function () {
                            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
                        });
                    });

                    $('[data-toggle="popover"]').popover();
                    $('[data-toggle="tooltip"]').tooltip();

                    $(".loader-card").addClass("hidden");
                    $("#table-content").removeClass("hidden");
                    $(".table-nodata").addClass("hidden");

                } else {
                    $(".loader-card").addClass("hidden");
                    $("#table-content").removeClass("hidden");
                    $(".table-nodata").removeClass("hidden");
                }

            } else {
                $(".table-nodata").removeClass("hidden");
            }
        },
        error: function (errorResult) {

 
        },
        complete: function () {
            $(".table-users").removeClass("hidden");
            $(".loader-card").addClass("hidden");
            $(".card-nodata").addClass("hidden");
        }
    });
};

$("#btn-modal").click(function () {
    clear();
    $("#showModal").modal('show');
});

$("#btn-create").click(function () {
    $(".form-save").submit();
});

$(".form-save").submit(function (e) {
    e.preventDefault();
    $(this).validate();
    if ($(this).valid()) {
        save();
    }
});

function save() {

    var data = $(".form-save").serialize();

    $.ajax({
        dataType: "json",
        method: "POST",
        url: "/Home/Save",
        data: data,
        success: function (result) {

            if (result.Success) {

                $("#success").removeClass("hidden");
                $("#message-success").html(result.Message);

                $("#danger").addClass("hidden");
                $("#message-danger").html("");

                $("#showModal").modal('hide');
                clear();
                getDados();
            }
            else {
                $("#showModal").modal('hide');
                $("#danger").removeClass("hidden");
                $("#message-danger").html("Erro ao salvar dados!");

                $("#success").addClass("hidden");
                $("#message-success").html("");
            }
        },
        error: function (result) {
            $("#showModal").modal('hide');
            $("#danger").removeClass("hidden");
            $("#message-danger").html("Erro ao salvar dados!");

            $("#success").addClass("hidden");
            $("#message-success").html("");
            getDados();
        },
        beforeSend: function () {
        },
        complete: function () {
            $("#showModal").modal('hide');
        }
    });
}

function editar(e) {
    clear();

    $.ajax({
        dataType: "json",
        method: "POST",
        url: "/Home/Editar",
        data: {
            Id: $(e).attr('Id')
        },
        success: function (result) {
            if (result.Success) {

                var data = JSON.parse(result.Data);

                $("#showModal").modal('show');
                $("#Id").val(data.Id);
                $("#IdUsuario").val(data.Id);
                $("#Nome").val(data.Nome);
                $("#Marca").val(data.Marca);
                $("#Modelo").val(data.Modelo);
                $("#Placa").val(data.Placa);
                $("#Eixos").val(data.Eixos);
                $("#Cep").val(data.Cep);
                $("#Estado").val(data.Estado);
                $("#Cidade").val(data.Cidade);
                $("#Bairro").val(data.Bairro);
                $("#Rua").val(data.Rua);
                $("#Numero").val(data.Numero);
                $("#Observacoes").val(data.Observacoes);
                $("#StatusMotorista").attr("checked", data.Ativo);
                $("#StatusCaminhao").attr("checked", data.Ativo);
            }
            else {

                $("#danger").removeClass("hidden");
                $("#message-danger").html("Erro ao Editar!");

                $("#success").addClass("hidden");
                $("#message-success").html("");
            }
        },
        error: function (result) {
            $("#danger").removeClass("hidden");
            $("#message-danger").html("Erro ao Editar!");

            $("#success").addClass("hidden");
            $("#message-success").html("");
        },
        beforeSend: function () {
        },
        complete: function () {
        }
    });
       
};

$("#btn-success").click(function () {
    $("#success").addClass("hidden");
    $("#message-success").html("");
});

$("#btn-danger").click(function () {
    $("#danger").addClass("hidden");
    $("#message-danger").html("");
});

function deletar(e) {
    IdUsuario = $(e).attr('Id');
    $("#showModal-del").modal('show');
};

$("#btn-del").click(function () {
    deletarRegistro();
});

function deletarRegistro() {
    
    $.ajax({
        dataType: "json",
        method: "POST",
        url: "/Home/Delete",
        data: {
            IdUsuario: IdUsuario
        },
        success: function (result) {

            if (result.Success) {

                $("#success").removeClass("hidden");
                $("#message-success").html(result.Message);

                $("#danger").addClass("hidden");
                $("#message-danger").html("");

                $("#showModal-del").modal('hide');
                clear();
                getDados();
            }
            else {
                $("#showModal-del").modal('hide');
                $("#danger").removeClass("hidden");
                $("#message-danger").html("Erro ao deletar registro!");

                $("#success").addClass("hidden");
                $("#message-success").html("");
            }
        },
        error: function (result) {
            $("#showModal-del").modal('hide');
            $("#danger").removeClass("hidden");
            $("#message-danger").html("Erro ao deletar registro!");

            $("#success").addClass("hidden");
            $("#message-success").html("");
            getDados();
        },
        beforeSend: function () {
        },
        complete: function () {
            $("#showModal-del").modal('hide');
        }
    });
}

function getStatus(status) {
    return status ? "<span class='label label-success'>ATIVO</span>" : "<span class='label label-warning'>INATIVO</span>";
}

function getHtmButton(row) {

    var html = "";
    html += "<a  Id='" + row + "' onclick='deletar(this)' href='javascript:void(0)'><span data-toggle='tooltip' data-placement='top' data-title='Deletar' class='fa fa-fw fa-lg fa fa-trash text-danger'></span></a>";
    html += "<a  Id='" + row + "' onclick='editar(this)' href='javascript:void(0)'><span data-toggle='tooltip' data-placement='top' data-title='Detalhes / Editar' class='fa fa-pencil fa-fw fa-lg text-success'></span></a>";
    return html;
};

function clear() {
    $("#Id").val("");
    $("#IdUsuario").val("");
    $("#Nome").val("");
    $("#StatusMotorista").attr("checked", "checked");
    $("#Marca").val("");
    $("#Modelo").val("");
    $("#Placa").val("");
    $("#Eixos").val("");
    $("#StatusCaminhao").attr("checked", "checked");
    $("#Cep").val("");
    $("#Estado").val("");
    $("#Cidade").val("");
    $("#Bairro").val("");
    $("#Rua").val("");
    $("#Numero").val("");
    $("#Observacoes").val("");
}