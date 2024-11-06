var urlBase = ""; /*/PortalViaDoc*/
// Metodos de validacion JS //
function validarNumSig(evento) {//permite digitar solo numero y guion 001-001-000000988
    //---------------------------------------------------------------------------------------// 
    var key = evento.keyCode || evento.which;
    var tecla = String.fromCharCode(key).toLowerCase();
    var letras = "1239045678-";
    var especiales = [8, 37, 39];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}

function validarLetras(evento) {//permite digitar solo letras
    //---------------------------------------------------------------------------------------//  
    var key = evento.keyCode || evento.which;
    var tecla = String.fromCharCode(key).toLowerCase();
    var letras = " áéíóúabcdefghijklmnñopqrstuvwxyzÁÉÍÓÚABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
    var punto = ".";
    var especiales = [8, 37, 39, 46];

    tecla_especial = false;
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}


function validaNumero(evento) {//permite digitar solo numeros
    //---------------------------------------------------------------------------------------//  
    try {
        var key = evento.keyCode || evento.which;
        if (key == 8) {
            return true;
        }

        if (((key > 0 && key < 48) || (key > 57))) { //&& (key != 46) && key < 255
            if (event) {
                evento.keyCode = 0;
                return false;

            }
            else if (evento.which) {
                return false;
            }
        }
    } catch (ex) {
		console.error(ex);
    }
}


function llamaModal(iconName, mensaje, timer) {
    Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: timer
    });

    Toast.fire({
        icon: iconName,
        title: "  " + mensaje
    });
}


function CargarDocumentos() {
    var cont = 0;
    var Actualiza = '0'
    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;

            var radioValue = $("input[name='customRadio']:checked").val();
            if (radioValue == 1) {
                Actualiza = $('#customRadio1').val();
            }

            $.ajax({
                url: urlBase + '/Documentos/CargarDocumentos/',
                type: 'POST',
                dataType: "JSON",
                async: false,
                data: {
                    'txtIdEmpresa': $("#txtIdEmpresa").val(),
                    'txtIdTipoDocumento': $("#txtIdTipoDocumento").val(),
                    'txtNumDocumento': $("#txtNumDocumento").val(),
                    'txtClaveAcceso': $("#txtClaveAcceso").val(),
                    'txtIdentificacion': $("#txtIdentificacion").val(),
                    'txtNombre': $("#txtNombre").val(),
                    'txtAutorizacion': Actualiza,
                    'txtFechaInicio': $("#txtFechaInicio").val(),
                    'txtFechaFin': $("#txtFechaFin").val(),

                },
                success: function (data) {
                    $('#divGrigDocumentos').html(data);
                    $(".cartprocess").hide();
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar", 2500);
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function CargarEstadisticas() {
    var cont = 0;
    if ($("#txtFechaInicio").val().trim() != '' && $("#txtFechaFin").val().trim() != '') {
        $(".cartprocess").fadeTo(30, 0.40, function () {
            if (cont == 0) {
                cont = 1;
                $.ajax({
                    url: urlBase + '/Estadisticas/ConsultarEstadisticas/',
                    type: 'POST',
                    dataType: "html",
                    async: false,
                    data: {
                        'txtIdEmpresa': $("#txtIdEmpresa").val(),
                        'txtFechaInicio': $("#txtFechaInicio").val(),
                        'txtFechaFin': $("#txtFechaFin").val()
                    },
                    success: function (data) {

                        $('#divGrigEstadisticas').html(data);
                        $(".cartprocess").hide();
                    },
                    error: function () {
                        $(".cartprocess").hide();
                        llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar o comuníquese con el departamento de sistemas", 2500);
                    }
                });
            }
        });
    } else {
        llamaModal('warning', "Por favor seleccione las fechas de búsqueda", 2500);
    }
}

function CargarDocumentosAutorizar() {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/Autorizaciones/CargarDocumentosAutorizar/',
                type: 'POST',
                dataType: "html",
                async: false,
                data: {
                    'txtIdEmpresa': $("#txtIdEmpresa").val(),
                    'txtIdTipoDocumento': $("#txtIdTipoDocumento").val(),
                    'txtNumDocumento': $("#txtNumDocumento").val(),
                },
                success: function (data) {
                    $('#divGrigDocumentosAutorizar').html(data);
                    $(".cartprocess").hide();
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function EnviaAutorizacion(txClaveAcceso, txtIdTipoDocumento, txtIdCompania) {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/Autorizaciones/AutorizarEnLinea/',
                type: 'POST',
                dataType: "json",
                data: {
                    'txtClaveAcceso': txClaveAcceso,
                    'txtIdTipoDocumento': txtIdTipoDocumento,
                    'txtIdCompania': txtIdCompania,
                },
                async: false,
                success: function (data) {
                    llamaModal('success', data, 3500);
                    CargarDocumentosAutorizar();
                    $(".cartprocess").hide();
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar", 2500);
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function DetalleEstadisticas(compania, fecha, fechaHasta, tipoDocumento, estado) {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/Estadisticas/ConsultarDetalles/',
                type: 'POST',
                dataType: "html",
                data: {
                    'compania': compania,
                    'fecha': fecha,
                    'fechaHasta': fechaHasta,
                    'tipoDocumento': tipoDocumento,
                    'ciEstado': estado,
                },
                async: false,
                success: function (data) {
                    $('#divDetalleEstadisticas').html(data);
                    $("#modal-default").modal('show');
                    $(".cartprocess").hide();
                },
                error: function (jqXHR, status, err) {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar", 2500);
                    $(".cartprocess").hide();
                }
            });
        }
    });
}


function EnviaEmailCliente() {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/Documentos/EnviaCorreo/',
                type: 'POST',
                dataType: "JSON",
                data: {
                    'ciEstado': $("#ciEstado").val(),
                    'ciIdCompania': $("#ciCompania").val(),
                    'txTipoDocumento': $("#txDocumento").val(),
                    'txClaveAcceso': $("#txClaveAcceso").val(),
                    'txEmail': $("#txEmail").val()
                },
                async: false,
                success: function (data) {
                    if (data == "") {
                        llamaModal('success', "Correo enviado con éxito", 3000);
                    }
                    else {
                        llamaModal('warning', data, 5000);
                    }

                    $("#envioCorreo").modal('hide');
                    $(".cartprocess").hide();
                },
                error: function (jqXHR, status, err) {
                    llamaModal('error', "Se presentó un error inesperado, por favor vuelva a intentar", 3000);
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function EnviaEmailClienteHistorico() {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/DocumentosHistoricos/EnviaCorreo/',
                type: 'POST',
                dataType: "JSON",
                data: {
                    'empresaHistorico': $("#txtIdEmpresaHistorico").val(),
                    'ciEstado': $("#ciEstado").val(),
                    'ciIdCompania': $("#ciCompania").val(),
                    'txTipoDocumento': $("#txDocumento").val(),
                    'txClaveAcceso': $("#txClaveAcceso").val(),
                    'txEmail': $("#txEmail").val()
                },
                async: false,
                success: function (data) {
                    if (data == "") {
                        llamaModal('success', "Correo enviado con éxito", 3000);
                    }
                    else {
                        llamaModal('warning', data, 3000);
                    }

                    $("#envioCorreo").modal('hide');
                    $(".cartprocess").hide();
                },
                error: function (jqXHR, status, err) {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar", 3000);
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function EnviaPortalCliente(ciEstado, ciIdCompania, txTipoDocumento, txClaveAcceso) {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/Documentos/EnviaPortal/',
                type: 'POST',
                dataType: "JSON",
                data: {
                    'ciEstado': ciEstado,
                    'ciIdCompania': ciIdCompania,
                    'txTipoDocumento': txTipoDocumento,
                    'txClaveAcceso': txClaveAcceso,
                },
                async: false,
                success: function (data) {
                    if (data == "") {
                        llamaModal('success', "El documento se envió con éxito", 3000);
                    }
                    else {
                        llamaModal('warning', data, 3000);
                    }

                    $(".cartprocess").hide();
                },
                error: function (jqXHR, status, err) {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar", 3000);
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function CorrigueDetalle() {
    var ciTipoDocumentos = $('#ciTipoDocumentos').val();
    var txClaveAcceso = $('#txClaveAcceso').val();
    var newDetail = $('#txDetalleNuevo').val();
    var codSecuancial = $('#codSecuancial').val();


    $.ajax({
        url: urlBase + '/Documentos/ObtieneXMLAutori/',
        type: 'POST',
        dataType: "JSON",
        data: {
            'txtClaveAcceso': txClaveAcceso,
            'txtTipoDocumento': ciTipoDocumentos,
            'nuevoDetalle': newDetail,
            'codigoSec': codSecuancial
        },
        async: false,
        success: function (data) {
            if (data) {
                $('#ddlDetalles').empty();

                if (data.Detalles && data.Detalles.length > 0) {
                    for (var i = 0; i < data.Detalles.length; i++) {
                        var item = data.Detalles[i];
                        $('#ddlDetalles').append(
                            $('<option></option>').val(item.CodigoPrincipal).html(item.Descripcion)
                        );
                    }
                }

                if (data.Autorizada === 'False') {
                    $('#txDetalleNuevo').attr('maxlength', 300);
                    llamaModal('warning', "El documento no se encuentra autorizado solo se permiten 300 caracteres en el detalle", 6000);
                }

                $("#corrigeDetalle").modal('show');
                if (data.Cod == "111") {
                    llamaModal('success', "El detalle se modificó correctamente", 3000);
                    $("#corrigeDetalle").modal('hide');
                    $('#codSecuancial').val('');
                    $('#txDetalleNuevo').val('');
                }
            } else {
                llamaModal('warning', "No se encontraron detalles para mostrar", 3000);
            }
        },
        error: function (jqXHR, status, err) {
            llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar", 3000);
        }
    });
}

function EnviaPortalClienteHistorico(idEmpresaHistorico, ciEstado, ciIdCompania, txTipoDocumento, txClaveAcceso) {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/DocumentosHistoricos/EnviaPortal/',
                type: 'POST',
                dataType: "JSON",
                data: {
                    'empresaHistorico': idEmpresaHistorico,
                    'ciEstado': ciEstado,
                    'ciIdCompania': ciIdCompania,
                    'txTipoDocumento': txTipoDocumento,
                    'txClaveAcceso': txClaveAcceso,
                },
                async: false,
                success: function (data) {
                    if (data == "") {
                        llamaModal('success', "El documento se envió con éxito", 3000);
                    }
                    else {
                        llamaModal('warning', data, 3000);
                    }

                    $(".cartprocess").hide();
                },
                error: function (jqXHR, status, err) {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar", 3000);
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function RecargarEmpresa(idEmpresaHistorico) {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/DocumentosHistoricos/RecargarEmpresas/',
                type: 'POST',
                dataType: "JSON",
                data: {
                    'empresaHistorico': idEmpresaHistorico,
                },
                async: false,
                success: function (data) {
                    $("#txtIdEmpresa").empty();
                    var appenddata1 = "";
                    $.each(data, function (i, item) {
                        appenddata1 += "<option value = '" + data[i].razonSocial + " '>" + data[i].nombreComercial + " </option>";;
                    });

                    $("#txtIdEmpresa").append(appenddata1);
                    $(".cartprocess").hide();
                },
                error: function (jqXHR, status, err) {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar", 3000);
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function CargarConfiguracionSmtp() {
    var cont = 0;


    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/Smtp/CargarConfiguracionSmtp/',
                type: 'POST',
                dataType: "html",
                async: false,
                data: {
                    'txRazonSocial': $("#txtIdEmpresa").val(),
                },
                success: function (data) {
                    if (data == '"NO_EXISTE_REGISTRO"') {
                        llamaModal('success', "No Existe Registro Smtp");
                        LimpiarModSmtp();
                        $('#divGrigConfiguracionSmtp').hide();
                        $("#divAgregar").show();
                    }
                    else {
                        $('#divGrigConfiguracionSmtp').html(data);
                        $('#divGrigConfiguracionSmtp').show();
                        $("#divAgregar").hide();
                    }
                    $(".cartprocess").hide();
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function CargarCompania() {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/Compania/CargarCompania/',
                type: 'POST',
                dataType: "html",
                async: false,
                data: {
                    'txRazonSocial': $("#txtIdEmpresa").val(),
                },
                success: function (data) {
                    $('#divGrigCompania').html(data);
                    $(".cartprocess").hide();
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function Insertar_Actualizar_Compania() {
    let tipomodal = $('#TipoModal').val();
    let blob_url = '';

    if ($('#exampleInputFile')[0].files[0] != null) {
        blob_url = $('#exampleInputFile')[0].files[0].name;
        UrlImage();
    }
    else {
        blob_url = "";
    }

    var objCompania = {
        UiCompania: $('#txClaveActivacion').val(),
        TxRuc: $('#txRuc').val(),
        TxRazonSocial: $('#txRazonSocial').val(),
        TxNombreComercial: $('#txNombreComercial').val(),
        TxDireccionMatriz: $('#txDireccionMatriz').val(),
        TxContribuyenteEspecial: $('#txContribuyenteEspecial').val(),
        TxObligadoContabilidad: $('#txObligadoContabilidad').val(),
        TxAgenteRetencion: $('#txAgenteRetencion').val(),
        TxRegimenMicroempresas: $('#txRegimenMicroempresas').val(),
        TxContribuyenteRimpe: $('#txContribuyenteRimpe').val(),
        TipoAmbiente: $('#txTipoAmbiente').val(),
        CiEstado: $('#txEstado').val()
    }

    $.ajax({
        url: urlBase + '/Compania/GuardaCompania/',
        type: 'POST',
        dataType: "json",
        async: false,
        data: {
            objCompania, blob_url, tipomodal,
        },
        success: function (data) {
            llamaModal('success', data);
        },
        error: function () {
            llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
        }
    });
}

function UrlImage() {

    var inputFileImage = document.getElementById('exampleInputFile');
    var file = inputFileImage.files[0];
    var fd = new FormData();
    fd.append('exampleInputFile', file);

    $.ajax({
        type: 'POST',
        contentType: false,
        processData: false,
        data: fd,
        url: urlBase + '/Compania/ObtenerUrl/',
        success: function (result) {
            if (result.result) {
                console.log("Succes");
            }
        }
    });
}
function CargarSucursal(idCompania, TxRazonSocial) {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/Compania/CargarSucursal/',
                type: 'POST',
                dataType: "html",
                async: false,
                data: {
                    'txIdCompania': idCompania,
                },
                success: function (data) {

                    $('#divGrigSucursal').html(data);
                    $(".cartprocess").hide();
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function CargarCertificado() {
    var cont = 0;


}

function CargarTiempoServicio() {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/Proceso/CargarTiempoProceso/',
                type: 'POST',
                dataType: "html",
                async: false,
                data: {
                    'txTipoServicio': $("#txtTipoServicio").val(),
                },
                success: function (data) {
                    $('#divGrigTiempoProceso').html(data);
                    $(".cartprocess").hide();
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function CargarUrlSri() {
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/UrlSri/CargarUrlSri/',
                type: 'POST',
                dataType: "json",
                async: false,
                data: {
                    'txTipoAmbiente': $("#txtIdTipoAmbiente").val(),
                },
                success: function (data) {
                    $("#txUrlRecepcion").val(data.urlRecepcion);
                    $("#txUrlAutorizacion").val(data.urlAutorizacion);
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function CheckTblManUser(tbl) {
    var cadena = "";
    var thisInput = $(tbl).find("input[type=checkbox]");


    for (i = 0; i < thisInput.length; ++i) {
        if (thisInput[i].checked) {
            cadena = thisInput[i].value + "|" + cadena;
        }
    }
    if (cadena == "")
        llamaModal('warning', "No se a elegido perfil para el nuevo Usuario");

    return cadena;
}

function ManteUser(opcion, tbl) {

    if (opcion == 'NU') {
        var usuario = $('#txtUsuario').val();
        var password = $('#txtPassword').val();
        var RepPassword = $('#txtRepetir_Password').val();
        var EstadoUser = "";
        var ChechCadena = "";
    } else if (opcion == 'MU') {
        var CodUser = $('#txtIdUserMOD').val();
        var usuario = $('#txtUsuarioMOD').val();
        var password = $('#txtPasswordMODCON').val();
        var RepPassword = $('#txtRePassworMODCON').val();
        var EstadoUser = $('#txtEstado').val();
        var ChechCadena = "";
    }


    if (password == RepPassword) {
        if (opcion == 'NU') {
            if (usuario != null && password != null && RepPassword != null) {
                ChechCadena = CheckTblManUser(tbl);
                if (ChechCadena != null) {
                    var data = opcion + "|" + CodUser + "|" + usuario + "|" + password + "|" + EstadoUser;
                    var dato_Perfil = ChechCadena;

                    $.ajax({
                        url: urlBase + '/Usuario/MantenimientoUsers/',
                        type: 'POST',
                        dataType: "json",
                        async: false,
                        data: {
                            data,
                            dato_Perfil
                        },
                        success: function (data) {
                            llamaModal('success', data);
                            $('#txtUsuario').val('');
                            $('#txtPassword').val('');
                            $('#txtRepetir_Password').val('');
                            window.location.reload(true);
                        },
                        error: function () {
                            llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                        }
                    });
                }
            }
        }
        if (opcion == 'MU' && EstadoUser == 'A') {
            if (usuario != null && password != null && RepPassword != null) {
                ChechCadena = CheckTblManUser(tbl);
                if (ChechCadena != null) {


                    var data = opcion + "|" + CodUser + "|" + usuario + "|" + password + "|" + EstadoUser;
                    var dato_Perfil = ChechCadena;

                    $.ajax({
                        url: urlBase + '/Usuario/MantenimientoUsers/',
                        type: 'POST',
                        dataType: "json",
                        async: false,
                        data: {
                            data,
                            dato_Perfil
                        },
                        success: function (data) {
                            llamaModal('success', "Usurio actualizado");
                            window.location.reload(true);
                        },
                        error: function () {
                            llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                        }
                    });
                }
            }
        }

        if (opcion == 'MU' && EstadoUser == 'I') {
            if (usuario != null && password != null && RepPassword != null) {
                var data = opcion + "|" + CodUser + "|" + usuario + "|" + password + "|" + EstadoUser;
                var dato_Perfil = '';

                $.ajax({
                    url: urlBase + '/Usuario/MantenimientoUsers/',
                    type: 'POST',
                    dataType: "json",
                    async: false,
                    data: {
                        data,
                        dato_Perfil
                    },
                    success: function (data) {
                        llamaModal('success', "El Usuario se elimino correctamente");
                        window.location.reload(true);
                    },
                    error: function () {
                        llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                    }
                });
            }
        }
    }
    else {
        llamaModal('warning', "Las Contraseña no son iguales ");
    }
}

function ConsutaMod(opcion) {
    var User = $('#txtUsuarioMOD').val();
    if (opcion = "CP") {

        $.ajax({
            url: urlBase + '/Usuario/ConsultaMod/',
            type: 'POST',
            dataType: "json",
            async: false,
            data: {
                opcion,
                User
            },
            success: function (data) {
                var dataRes = data.split('|')
                if (dataRes[0] != "") {
                    llamaModal('success', dataRes[0]);
                }
                $('#txtIdUserMODCON').val(dataRes[1]);
                $('#txtIdUserMOD').val(dataRes[1])
                $('#txtUsuarioMODCON').val(dataRes[2]);
                PerfilCheck(dataRes[3]);
            },
            error: function () {
                llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
            }
        });
    }
}

function PerfilCheck(perfiles) {
    var y = perfiles.split(',')
    var thisInput = $('#tblPerfiUserMod').find("input[type=checkbox]")

    for (j = 0; j < y.length; j++) {
        for (i = 0; i < thisInput.length; i++) {
            if (thisInput[i].value == y[j]) {
                thisInput[i].checked = true;
            }
        }
    }
}


function GuardarCertificado() {
    var InsertData = "";
    var CiCompañia = $('#CiCompañia').val();
    var FchDesde = $('#txtFchDesde').val();
    var FchHasta = $('#txtFchHasta').val();
    var Estado = $('#txEstado').val();
    var file = ($("#exampleInputFile"))[0].files[0].name;
    var Passeword = $('#txtPassword').val();


    if ($('#subio').val() == "1") {
        InsertData = CiCompañia + "|" + FchDesde + "|" + FchHasta + "|" + Estado + "|" + file + "|" + Passeword;

        $.ajax({
            url: urlBase + '/Certificado/InsertCertificado/',
            type: 'POST',
            dataType: "json",
            async: false,
            data: {
                InsertData
            },
            success: function (data) {
                llamaModal('success', data);
                window.location.reload(true);
            },
            error: function () {
                llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                window.location.reload(true);
            }
        });
    }
}

function verificaclaveCert() {
    var compdata = "";
    var Passeword = $('#txtPassword').val();
    var RucCompania = $('#txtRuc_compania').val();
    var file = ($("#exampleInputFile"))[0].files[0];


    if (file == undefined) {
        $('#subio').val("0")
        llamaModal('warning', "No ha escoguido certificado");
    }

    if (file != undefined) {
        $('#subio').val("1")
        file = ($("#exampleInputFile"))[0].files[0].name
    }

    if ($('#subio').val() == "1") {
        if (Passeword != "") {
            if (RucCompania.length != "") {

                compdata = Passeword + "|" + RucCompania + "|" + file + "|" + $('#subio').val();
                CopiarCert()
                $.ajax({
                    url: urlBase + '/Certificado/verificaClave/',
                    type: 'POST',
                    dataType: "json",
                    async: false,
                    data: {
                        compdata
                    },
                    success: function (data) {
                        var dataRes = data.split('|');
                        console.log(dataRes[1]);
                        if (dataRes[1] != '') {
                            llamaModal('success', dataRes[0]);
                            $('#txtFchDesde').val(dataRes[1]);
                            $('#txtFchHasta').val(dataRes[2]);
                            $('#txtClaveActivacion').val(dataRes[3]);
                            $('#CiCompañia').val(dataRes[4]);
                        }
                        else {
                            llamaModal('warning', "Clave del Certifcado Incorrecta");
                        }

                    },
                    error: function () {
                        llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                        $(".cartprocess").hide();
                    }
                });
            }
            else {

                llamaModal('warning', "INGRESE EL RUC DE LA COMPAÑIA");
            }
        }
        else {
            llamaModal('warning', "INGRESE EL PASSWORD DEL CERTIFICADO");
        }


    }
}

function CopiarCert() {

    var inputFileImage = document.getElementById('exampleInputFile');

    var file = inputFileImage.files[0];
    var name = inputFileImage.files[0].name;
    var fd = new FormData();
    fd.append('exampleInputFile', file);
    //inputFileImage.className = name;
    console.log("nombre " + name);

    $.ajax({
        type: 'POST',
        contentType: false,
        processData: false,
        data: fd,
        url: urlBase + '/Certificado/ObtenerCert/',
        success: function (result) {
            if (result.result) {
                console.log("Succes");
            }
        }
    });
}
function GuardarSmtp() {
    var SmtpData = '';

    SmtpData =
        $('#txtIdEmpresaCompania').val() + "|" +
        $('#txHostServidor').val() + "|" +
        $('#txPuertoServidor').val() + "|" +
        $('#txEnableSsl').val() + "|" +
        $('#txEmailCredencial').val() + "|" +
        $('#txPassCredencial').val() + "|" +
        $('#txMailAdressFrom').val() + "|" +
        $('#txTo').val() + "|" +
        $('#txCc').val() + "|" +
        $('#txAsunto').val() + "|" +
        $('#txNotificacion').val() + "|" +
        $('#txtxUrlPortalTo').val() + "|" +
        $('#txUrlCompania').val();

    $.ajax({
        url: urlBase + '/Smtp/GuardarConfiguracionSmtp/',
        type: 'POST',
        dataType: "json",
        async: false,
        data: {
            SmtpData,
        },
        success: function (data) {
            llamaModal('success', data);
        },
        error: function () {
            llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
            $(".cartprocess").hide();
        }
    });
}

function LimpiarModSmtp() {
    $('#txtIdEmpresaCompania').val($('#txtIdEmpresa').val());
    $('#txtIdCompania').val('');
    $('#txHostServidor').val('');
    $("#txPuertoServidor").val('');
    $('#txEmailCredencial').val('');
    $("#txPassCredencial").val('');
    $("#txMailAdressFrom").val('');
    $("#txTo").val('');
    $("#txCc").val('');
    $("#txAsunto").val('');
    $("#txUrlPortal").val('');
    $("#txUrlCompania").val('');
    $('#txEnableSsl').val('');
    $('#txNotificacion').val('');
}

function GenerarPDF(TipoZip) {
    var Data = '';
    var cont = 0;
    var Compania = $('#txtIdEmpresa').val();
    var Documento = $('#txtIdTipoDocumento').val();
    var FechaDesde = $('#txtFechaInicio').val();
    var FechaHasta = $('#txtFechaFin').val()

    Data = Compania + "|" + Documento + "|" + FechaDesde + "|" + FechaHasta;

    if (TipoZip == 'PDF') {
        $(".cartprocess").fadeTo(30, 0.40, function () {
            if (cont == 0) {
                cont = 1;
                $.ajax({
                    url: urlBase + '/GenerarPDFZip/GenerePDF/',
                    type: 'POST',
                    dataType: "json",
                    async: false,
                    data: {
                        Data
                    },
                    success: function (data) {
                        var datares = data.split('|');

                        if (datares[0] != '' && datares[1] != '' && datares[2] != '') {
                            llamaModal('success', datares[0]);
                            descargarBase64(datares[1], datares[2]);
                        }
                        else {
                            llamaModal('warning', datares[0]);
                        }

                        $(".cartprocess").hide();
                    },
                    error: function () {
                        llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                        $(".cartprocess").hide();
                    }
                });
            }
        });
    }

    if (TipoZip == 'XML') {
        $(".cartprocess").fadeTo(30, 0.40, function () {
            if (cont == 0) {
                cont = 1;
                $.ajax({
                    url: urlBase + '/GenerarPDFZip/GenereXML/',
                    type: 'POST',
                    dataType: "json",
                    async: false,
                    data: {
                        Data
                    },
                    success: function (data) {
                        var datares = data.split('|');

                        if (datares[0] != '' && datares[1] != '' && datares[2] != '') {
                            llamaModal('success', datares[0]);
                            descargarBase64(datares[1], datares[2]);
                        }
                        else {
                            llamaModal('warning', datares[0]);
                        }

                        $(".cartprocess").hide();
                    },
                    error: function () {
                        llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                        $(".cartprocess").hide();
                    }
                });
            }
        });
    }

}

function descargarBase64(name, Doc) {

    const linkSource = 'data:application/zip;base64,' + Doc;
    const downloadLink = document.createElement("a");
    const fileName = name;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
}

function DescaExcel() {
    var cont = 0;
    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/Documentos/DescargaExcel/',
                type: 'POST',
                dataType: "json",
                async: false,
                data: {

                },
                success: function (data) {
                    var datares = data.split('|');

                    if (datares[0] != '' && datares[0] != '' && datares[2] != '') {
                        llamaModal('success', datares[0]);
                        descargarBase64(datares[1], datares[2]);
                    }
                    else {
                        llamaModal('warning', datares[0]);
                    }
                    $(".cartprocess").hide();
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function InsetarSucursal() {

    var DataSucu = '';
    var cicompania = $('#TxRazonSocialSucursal').val();
    var idSucursal = $('#txCodSucursal').val();
    var direccion = $('#txDireccionMatrizSuc').val();
    var Estado = $('#txEstadoSurc').val();

    DataSucu = cicompania + "|" + idSucursal + "|" + direccion + "|" + Estado;

    $.ajax({
        url: urlBase + '/Compania/InsertaSucursal/',
        type: 'POST',
        dataType: "json",
        async: false,
        data: {
            DataSucu
        },
        success: function (data) {
            llamaModal('success', data);
        },
        error: function () {
            llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
        }
    });
}

function ReporteEyer() {
    $.ajax({
        url: urlBase + '/Bienvenido/ReporteAyer/',
        type: 'POST',
        dataType: "json",
        async: false,
        data: {
        },
        success: function (datos) {
            var report = datos.split('|');
            var data = google.visualization.arrayToDataTable([
                ['Task', ''],
                ['Factura', parseInt(report[0])],
                ['Nota de Credito', parseInt(report[1])],
                ['Nota de Debito', parseInt(report[2])],
                ['Guia de Remision', parseInt(report[3])],
                ['Comp Retencion', parseInt(report[4])],
                ['Liquidacion', parseInt(report[5])]
            ]);

            var options = {
                title: 'Reporte Ayer'
            };

            var chart = new google.visualization.PieChart(document.getElementById('piechart'));

            chart.draw(data, options);
        },
        error: function () {
            llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
        }
    });
}

function ReporteActual() {
    $.ajax({
        url: urlBase + '/Bienvenido/ReporteActual/',
        type: 'POST',
        dataType: "json",
        async: false,
        data: {
        },
        success: function (datos) {
            var report = datos.split('|');
            var data = google.visualization.arrayToDataTable([
                ['Task', ''],
                ['Factura', parseInt(report[0])],
                ['Nota de Credito', parseInt(report[1])],
                ['Nota de Debito', parseInt(report[2])],
                ['Guia de Remision', parseInt(report[3])],
                ['Comp Retencion', parseInt(report[4])],
                ['Liquidacion', parseInt(report[5])]
            ]);

            var options = {
                title: 'Reporte Hoy'
            };

            var chart = new google.visualization.PieChart(document.getElementById('piechart_hoy'));

            chart.draw(data, options);
        },
        error: function () {
            llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
        }
    });
}

function BtnBuscar() {
    var compania = $('#txtIdEmpresa').val();
    var documento = $('#txtIdTipoDocumento').val();
    var fechadesde = $('#txtFechaInicio').val();
    var fechahasta = $('#txtFechaFin').val();
    var numdoc = $('#txtNumDocumento').val();
    var cont = 0;

    var datos = compania + "|" + documento + "|" + fechadesde + "|" + fechahasta + "|" + numdoc;
    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/ReprocesoDoc/ConsultaDoc/',
                type: 'POST',
                dataType: "html",
                async: false,
                data: {
                    datos
                },
                success: function (data) {
                    $('#divGrigReproceso').html(data);
                    $('#divbtnReprocesoTodo').show()
                    $(".cartprocess").hide();
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function Reproceso(CLaveAcceso, ciEstado) {
    var datos = $('#txtIdEmpresa').val() + "|" + $('#txtIdTipoDocumento').val() + "|" + CLaveAcceso + "|" + ciEstado;
    $.ajax({
        url: urlBase + '/ReprocesoDoc/Reproceso_Uno/',
        type: 'POST',
        dataType: "json",
        async: false,
        data: {
            datos
        },
        success: function (data) {
            llamaModal('success', data);
            BtnBuscar();
        },
        error: function () {
            llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
        }
    });
}

function Reproceso_Todo() {
    var datos = $('#txtIdEmpresa').val() + "|" + $('#txtIdTipoDocumento').val();
    var cont = 0;

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/ReprocesoDoc/ReprocesoTodo/',
                type: 'POST',
                dataType: "json",
                async: false,
                data: {
                    datos
                },
                success: function (data) {
                    llamaModal('success', data);
                    BtnBuscar();
                    $(".cartprocess").hide();
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                    $(".cartprocess").hide();
                }
            });
        }
    });
}

function EliminaCompania(TxRazonSocial) {
    var cont = 0;

    var datos = TxRazonSocial + "|" + 'I';

    $(".cartprocess").fadeTo(30, 0.40, function () {
        if (cont == 0) {
            cont = 1;
            $.ajax({
                url: urlBase + '/Compania/EliminarCompañia/',
                type: 'POST',
                dataType: "json",
                async: false,
                data: {
                    datos
                },
                success: function (data) {
                    llamaModal('success', data);
                    CargarCompania()
                    $(".cartprocess").hide();
                },
                error: function () {
                    llamaModal('warning', "Se presentó un error inesperado, por favor vuelva a intentar");
                    $(".cartprocess").hide();
                }
            });
        }
    });
}