<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Main.aspx.cs"
    Inherits="ProyectosWeb.Main" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/beta/0911/Start.debug.js"></script>
    <script src="http://ajax.microsoft.com/ajax/beta/0911/extended/ExtendedControls.debug.js"
        type="text/javascript"></script>  
    <link rel="stylesheet" href="//code.jquery.com/ui/1.10.4/themes/smoothness/jquery-ui.css">
    <link rel="stylesheet" href="Style/Style.css" />
    <link rel="stylesheet" href="css/Seguridad/Seguridad.css" /> 
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
    <script src="scripts/ConfirmDialog/jquery.easy-confirm-dialog.js"></script>
    <script runat="server" type="text/c#">                          
</script>
    <script>      
        $(function () {
            var activo = parseInt($('[id*=accordionInd]').val());            
            $("#accordion").accordion({ collapsible: true,
                animated: 'slide',
                autoHeight: false,
                navigation: true, active: activo
            });
        });     
    </script>
<!-- include jQuery and jQueryUI libraries --><%--
<script type="text/javascript" src="http://code.jquery.com/jquery-1.9.1.js"></script>
<script type="text/javascript" src="http://code.jquery.com/ui/1.10.1/jquery-ui.js"></script>
<link rel="stylesheet" type="text/css" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css"/>--%>
<script type="text/javascript" src="validacion/jquery.validate.js"></script>
<script src="scripts/seguridad/Seguridad.js" ></script>

<!-- include plugin -->
<script type="text/javascript" src="jquery-tree-checkboxes/minified/jquery.tree.min.js"></script>
<link rel="stylesheet" type="text/css" href="jquery-tree-checkboxes/minified/jquery.tree.min.css" />
<!-- initialize checkboxTree plugin -->
<script type="text/javascript">
    //<!--
    $(document).ready(function () {
        $('#tree').tree({
            /* specify here your options */
        });
        $('#DivTreePerfil').tree({

        });
        $('#DivTreeModSis').tree({

        });        
    });   
    //-->
</script>
   <%-- <link href="../../LoginSources/font/stylesheet.css" rel="stylesheet" type="text/css" />--%>	
<link href="LoginSources/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
<link href="LoginSources/css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
<link href="LoginSources/css/styles.css" rel="stylesheet" type="text/css" />
<%--<link href="../../LoginSources/css/media-queries.css" rel="stylesheet" type="text/css" />--%> 
       
    <script type="text/javascript" src="validacion/jquery.validate.js"></script>
    <script type="text/javascript">

        $(function () {

          $('a').mouseenter(function () {  
                var col=$(this).css('color');
                if (col == 'rgb(0, 0, 0)') {
                    $(this).css('color', 'rgb(0, 85, 128)');
            }
        });
        $('a').mouseleave(function () {
            var col = $(this).css('color');
            if (col != 'rgb(0, 0, 255)' && col == 'rgb(0, 0, 0)' || col== 'rgb(0, 85, 128)') {
                $(this).css('color', 'black');
            }
        });

        setInterval(tiempoTarea, 1000);
        function tiempoTarea() {
            $.ajax({
                type: "POST",
                url: "Main.aspx/tiempo",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $("[id*=LabelSeguimientoTarea]").text(data.d.toString());
                },
                failure: function () {

                }
            });
        }

        //        $("#ButtonEliminaModulo").on('click', function () {
        //            var b = $("[id*=ButtonEliminaModulo]");
        //            dialogo(b);
        //        });
        //        $("#ButtonDeletePantallaOpcion").on('click', function () {
        //            var b = $("[id*=ButtonDeletePantallaOpcion]");
        //            dialogo(b);
        //        });

        function dialogo(boton) {
            event.preventDefault();
            $("#dialog-confirm").dialog({
                height: 200,
                width: 300,
                modal: true,
                buttons: {
                    "Si": function () {
                        $(this).dialog("close");
                        boton.click();
                    },

                    "No": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
  


            $('#<%=LinkProyecto.ClientID%>').on('click', function () {
                $('#<%=LabelNav.ClientID%>').val("Proyectos");
            });

            $('#ButtonAgregarGU').bind('click', function () {
                var options = $('[id*=ListBoxGruposSeg] option:selected');
                for (var i = 0; i < options.length; i++) {
                    var opt = $(options[i]).clone();
                    $(options[i]).remove();     
                    $('[id*=ListBoxGruposAsigSeg]').append(opt);
                }
            }); 
            $('#ButtonEliminarGU').bind('click', function () {
                var options = $('[id*=ListBoxGruposAsigSeg] option:selected');
                for (var i = 0; i < options.length; i++) {
                    var opt = $(options[i]).clone();
                    $(options[i]).remove();
                    $('[id*=ListBoxGruposSeg]').append(opt);
                }
            });

            $("#TextBoxFechaoIniSis").datepicker({ dateFormat: "yy/mm/dd" });
            $("#TextBoxFechaFinEsSis").datepicker({ dateFormat: "yy/mm/dd" });
            $("#TextBoxFinRealSis").datepicker({ dateFormat: "yy/mm/dd" });

            $('#TextBoxFinRCR').datepicker({ dateFormat: "yy/mm/dd" });
            $('#TextBoxFechaFinEstimCR ').datepicker({ dateFormat: "yy/mm/dd" });
            $('#TextBoxFechaInicioCR ').datepicker({ dateFormat: "yy/mm/dd" });            

        });

        $(document).ready(function () {

            function showConfirm(event) {
                event.stopPropagation();
                return false;
            }

            $('#<%=ButtonGuardarGU.ClientID%>').bind("click", function () {
                $("[id*=ListBoxGruposAsigSeg] option").attr("selected", "selected");
                $("[id*=ListBoxGruposSeg] option").attr("selected", "selected");
            });

            var g = $('#<%=LabelNav.ClientID%>').text();

            var usuarioDisponible = "No";

            $('#<%= PasswordConfirm.ClientID%>').keyup(function () {
                var pass = $("#<%=PasswordUs.ClientID%>");
                var confpass = $('#<%= PasswordConfirm.ClientID%>');
                confirmarPass(confpass, pass);
            });

            $('#<%= PasswordConfirmUpdate.ClientID%>').keyup(function () {
                var pass = $("#<%=PasswordUsUpdate.ClientID%>");
                var confpass = $('#<%= PasswordConfirmUpdate.ClientID%>');
                confirmarPass(confpass, pass);
            });

            $('#<%= PasswordUsRestoreConfirm.ClientID%>').keyup(function () {
                var pass = $("#<%=PasswordUsRestore.ClientID%>");
                var confpass = $('#<%= PasswordUsRestoreConfirm.ClientID%>');
                confirmarPass(confpass, pass);
            });

            function confirmarPass(confpass, pass) {
                if (confpass.val().length > 0) {
                    if (confpass.val() == pass.val()) {
                        confpass.css("border-color", "green");
                    } else {
                        confpass.css("border-color", "red");
                    }
                }
            }

            $('#<%= TextBoxUsuario.ClientID%>').keyup(function () {
                var usuariot = $("#<%=TextBoxUsuario.ClientID%>");
                var sc = $("#<%=validarUsuario.ClientID%>");
                var hid = $('#<%=HiddenValidUser.ClientID %>');
                validarUs(usuariot, hid, sc, sc);

            });


            $('#<%= TextBoxUsuarioUpdate.ClientID%>').keyup(function () {
                var usuariot = $("#<%=TextBoxUsuarioUpdate.ClientID%>");
                var sc = $("#<%=validarUsuarioUpdate.ClientID%>");
                var hid = $('#<%=HidValidUserUpdate.ClientID %>');
                $("[id$=msgactualizado]").fadeIn();
                validarUs(usuariot, hid, sc, sc);

            });

            function validarUs(usuariot, hid, validaus, sc) {
                usuariot.css("border-color", "none");
                if (usuariot.val().length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "Main.aspx/CheckUserName",
                        data: '{userName: "' + usuariot.val() + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            switch (data.d) {
                                case "false":
                                    hid.val("1");
                                    sc.text("Usuario Disponible");
                                    sc.css("color", "green");
                                    usuariot.css("color", "green");
                                    usuariot.css("border-color", "green");
                                    break;
                                case "true":
                                    hid.val("");
                                    sc.text("Usuario No Disponible");
                                    usuariot.css("color", "red");
                                    sc.css("color", "red");
                                    usuariot.css("border-color", "red");
                                    break;
                            }
                        },
                        failure: function () {

                        }
                    });
                } else {
                    sc.text("");
                    usuariot.css("border-color", "red");
                }
            }

            $('#<%= TextBoxEmailRegistrado.ClientID%>').keyup(function () {
                var usuariot = $("#<%=TextBoxEmailRegistrado.ClientID%>");
                var sc = $("#<%=LabEmailReg.ClientID%>");
                var hid = $('#<%=HidValidEmailRestoreSeg.ClientID %>');
                validarAjax(usuariot, hid, sc, "CheckEmail", "Email válido", "Email no válido", "green", "red");
            });

            $('#<%= TextBoxEmail.ClientID%>').keyup(function () {
                var usuariot = $("#<%=TextBoxEmail.ClientID%>");
                var sc = $("#<%=LabEmailReg1.ClientID%>");
                var hid = $('#<%=HidValidEmailReg.ClientID %>');
                var color1 = "red";
                if (usuariot.val().length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "Main.aspx/CheckEmail",
                        data: '{email: "' + usuariot.val() + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            switch (data.d) {
                                case "true":
                                    hid.val("");
                                    sc.text("El email ya existe");
                                    sc.css("color", color1);
                                    break;
                                case "false":
                                    hid.val("1");
                                    sc.text("");
                                    break;
                                case "error":
                                    break;
                            }
                        },
                        failure: function () {

                        }
                    });
                } else {
                    sc.text("");
                }
            });

            function validarAjax(usuariot, hid, sc, metodo, msgexiste, msgNoexiste, color1, color2) {
                if (usuariot.val().length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "Main.aspx/" + metodo,
                        data: '{email: "' + usuariot.val() + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            switch (data.d) {
                                case "true":
                                    hid.val("1");
                                    sc.text(msgexiste);
                                    sc.css("color", color1);
                                    usuariot.css("color", color1);
                                    usuariot.css("border-color", color1);
                                    break;
                                case "false":
                                    hid.val("");
                                    sc.text(msgNoexiste);
                                    usuariot.css("color", color2);
                                    sc.css("color", color2);
                                    usuariot.css("border-color", color2);
                                    break;
                                case "error":
                                    break;
                            }
                        },
                        failure: function () {

                        }
                    });
                } else {
                    sc.text("");
                    usuariot.css("border-color", "red");
                }

            }

            $('#<%= Button5seg.ClientID%>').on('click', function (data) {
                quitarValidacion(g);
            });
            $('#<%= Button6seg.ClientID%>').on('click', function (data) {
                quitarValidacion(g);
            });

            $('#<%= ButtonCancelSistema.ClientID%>').on('click', function (data) {
                quitarValidacionSistema();
            });
            $('#<%= ButtonConsultaSistemasSeg.ClientID%>').on('click', function (data) {
                quitarValidacionSistema();
            });

            function quitarValidacion(g) {
                if (g == "Usuarios") {
                    $("#TextBoxUsuario").rules("remove");
                    $("#TextBoxApellidos").rules("remove");
                    $("#TextBoxNomUsuario").rules("remove");
                    $("#TextBoxEmail").rules("remove");
                    $("#TextBoxTelefono").rules("remove");
                    $("#PasswordUs").rules("remove");
                    $("#PasswordConfirm").rules("remove");
                    $("#HiddenValidUser").rules("remove");
                    $("#TextAreaTecnologias").rules("remove");
                    $("#HidValidEmailReg").rules("remove");
                } else if (g == "Grupos") {
                    $("#TextBoxNomGrupo").rules("remove");
                    $("#TextBoxDescripcionGrupo").rules("remove");
                } else if (g == "Perfiles") {
                    $("#TextBoxNomPerfil").rules("remove");
                    $("#TextBoxDescripcionPerfil").rules("remove");
                }
                else if (g == "Sistemas") {
                    quitarValidacionSistema();
                }
            }

            function quitarValidacionSistema() {
                $("#TextBoxClaveSis").rules("remove");
                $("#TextBoxNombreSis").rules("remove");
                $("#TextBoxDescSis").rules("remove");
                $("#TextBoxClienteSis").rules("remove");
                $("#TextBoxFechaoIniSis").rules("remove");
                $("#TextBoxFechaFinEsSis").rules("remove");
                $("#TextBoxFinRealSis").rules("remove");
            }


            $('#<%= ButtonUpdateSistema.ClientID%>').on('click', function () {
                validaSistema();
            });

            function validaSistema() {
             var status=   $('#<%= lblUpdateSistema.ClientID%>');
                jQuery.validator.addMethod("greaterThan",
function (value, element, params) {

    if (!/Invalid|NaN/.test(new Date(value))) {
        return new Date(value) > new Date($(params).val());
    }

    return isNaN(value) && isNaN($(params).val())
        || (Number(value) > Number($(params).val()));
}, 'Must be greater than {0}.');

                var f = $('#<%= TextBoxFechaoIniSis.ClientID%>').val();
                var f2 = $('#<%= TextBoxFinRealSis.ClientID%>').val();
                
                var s = 'sa';
                if (f2 !== '' && f !== '') {
                    if (new Date(f2) > new Date(f)) {
                        s = 'de';
                    }
                }
                if (f2 == '' || s == 'de') {
                    status.text('');
                    $("#BodyForm").validate({
                        ignore: "",
                        rules: {
                            'TextBoxClaveSis': { required: true },
                            'TextBoxNombreSis': { required: true },
                            'TextBoxClienteSis': { required: true },
                            'TextBoxDescSis': { required: true },
                            'TextBoxFechaoIniSis': { required: true },
                            'TextBoxFechaFinEsSis': { required: true, greaterThan: '#TextBoxFechaoIniSis' }

                        },
                        messages: {
                            'TextBoxClaveSis': { required: 'Ingrese una clave' },
                            'TextBoxNombreSis': { required: 'Ingrese un nombre' },
                            'TextBoxClienteSis': { required: 'Ingrese un nombre de cliente' },
                            'TextBoxDescSis': { required: 'Ingrese una descripción' },
                            'TextBoxFechaoIniSis': { required: 'Ingrese una fecha de Inicio' },
                            'TextBoxFechaFinEsSis': { required: 'Ingrese una fecha estimada', greaterThan: 'La Fecha debe de ser mayor a la Fecha Inicio' }

                        },
                        errorPlacement: function (error, element) {
                            error.insertAfter(element);
                            error.addClass('message');  // add a class to the wrapper
                            error.css("color", "red");
                            error.css("display", "-webkit-inline-box");
                        },
                        debug: true,
                        submitHandler: function (ButtonUpdateSistema) {
                            ButtonUpdateSistema.submit();
                        }
                    });
                } else {

                    status.css("color", "red");
                    status.text('La Fecha Fin Real debe ser mayor a la Fecha Inicio');
                    event.preventDefault();
                }
            }

            $('#<%= ButtonEnviarEmailSeg.ClientID%>').on('click', function () {
                $("#BodyForm").validate({
                    ignore: "",
                    rules: {
                        'TextBoxEmailRegistrado': { email: true },
                        'HidValidEmailRestoreSeg': { required: true }
                    },
                    messages: {
                        'TextBoxEmailRegistrado': { email: 'Ingrese un correo valido. Ejemplo: cristian@hotmail.com' },
                        'HidValidEmailRestoreSeg': { required: '' }
                    },
                    errorPlacement: function (error, element) {
                        error.insertAfter(element)
                        error.addClass('message');  // add a class to the wrapper
                        error.css("color", "red");
                        error.css("display", "-webkit-inline-box");
                    },
                    debug: true,
                    submitHandler: function (ButtonEnviarEmailSeg) {
                        ButtonEnviarEmailSeg.submit();
                    }
                });
            });

            $('#<%= ButtonActualizaUsSeg.ClientID%>').on('click', function () {
                $("#BodyForm").validate({
                    ignore: "",
                    rules: {
                        'TextBoxUsuarioUpdate': { required: true, minlength: 5 },
                        'PasswordUsUpdate': { required: true, maxlength: 12, minlength: 5 },
                        'PasswordConfirmUpdate': { required: true, maxlength: 12, minlength: 5, equalTo: '#PasswordUsUpdate' },
                        'HidValidUserUpdate': { required: true, maxlength: 30 }
                    },
                    messages: {
                        'TextBoxUsuarioUpdate': { required: 'Ingrese un nombre de usuario', minlength: 'El Nombre de Usuario debe ser minimo 5 caracteres' },
                        'PasswordUsUpdate': { required: 'Ingrese una contraseña', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'La contraseña debe ser minimo 5 caracteres' },
                        'PasswordConfirmUpdate': { required: 'La contraseña no coincide', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'La contraseña debe ser minimo 5 caracteres', equalTo: 'La contraseña no coincide' },
                        'HidValidUserUpdate': { required: '', maxlength: 'Maximo 30 caracteres' }
                    },
                    errorPlacement: function (error, element) {
                        error.insertAfter(element)
                        error.addClass('message');  // add a class to the wrapper
                        error.css("color", "red");
                        error.css("display", "-webkit-inline-box");
                    },
                    debug: true,
                    submitHandler: function (ButtonActualizaUsSeg) {
                        ButtonActualizaUsSeg.submit();

                    }
                });
            });

            $('#<%= RestablecerPasswordEmail.ClientID%>').on('click', function () {
                $("#BodyForm").validate({
                    ignore: "",
                    rules: {
                        'PasswordUsRestore': { required: true, maxlength: 12, minlength: 5 },
                        'PasswordUsRestoreConfirm': { required: true, maxlength: 12, minlength: 5, equalTo: '#PasswordUsRestore' }
                    },
                    messages: {
                        'PasswordUsRestore': { required: 'Ingrese una contraseña', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'La contraseña debe ser minimo 5 caracteres' },
                        'PasswordUsRestoreConfirm': { required: 'La contraseña no coincide', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'La contraseña debe ser minimo 5 caracteres', equalTo: 'La contraseña no coincide' }
                    },
                    errorPlacement: function (error, element) {
                        error.insertAfter(element)
                        error.addClass('message');  // add a class to the wrapper
                        error.css("color", "red");
                        error.css("display", "-webkit-inline-box");
                    },
                    debug: true,
                    submitHandler: function (RestablecerPasswordEmail) {
                        RestablecerPasswordEmail.submit();

                    }
                });
            });

            $('#<%= Button4seg.ClientID%>').on('click', function () {
                var g = $('#<%=LabelNav.ClientID%>').text();
                if (g == "Usuarios") {
                    $("#BodyForm").validate({
                        ignore: "",
                        rules: {
                            'TextBoxUsuario': { required: true, minlength: 5
                            },
                            'TextBoxApellidos': { required: true, number: false, maxlength: 30 },
                            'TextBoxNomUsuario': { required: true, maxlength: 30 },
                            'TextBoxEmail': { required: true, email: true, maxlength: 30 },
                            'TextBoxTelefono': { number: true, maxlength: 12, minlength: 10 },
                            'PasswordUs': { required: true, maxlength: 12, minlength: 5 },
                            'PasswordConfirm': { required: true, maxlength: 12, minlength: 5, equalTo: '#PasswordUs' },
                            'HiddenValidUser': { required: true, maxlength: 30 },
                            'TextAreaTecnologias': { maxlength: 30 },
                            'HidValidEmailReg': { required: true }
                        },
                        messages: {
                            'TextBoxUsuario': { required: 'Ingrese un nombre de usuario', minlength: 'El Nombre de Usuario debe ser minimo 5 caracteres' },
                            'TextBoxApellidos': { required: 'Ingrese su apellido completo', number: 'No debe ingresar datos Numericos', maxlength: 'los caracteres maximos son 30' },
                            'TextBoxNomUsuario': { required: 'Ingrese un nombre' },
                            'TextBoxEmail': { required: 'Ingrese un correo electrónico', email: 'Ingrese un correo valido. Ejemplo: cristian@hotmail.com' },
                            'TextBoxTelefono': { number: 'Solo se permiten numeros', maxlength: 'Maximo 12 digitos', minlength: 'Ingrese mínimo 10 digitos' },
                            'PasswordUs': { required: 'Ingrese una contraseña', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'La contraseña debe ser minimo 5 caracteres' },
                            'PasswordConfirm': { required: 'La contraseña no coincide', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'La contraseña debe ser minimo 5 caracteres', equalTo: 'La contraseña no coincide' },
                            'HiddenValidUser': { required: '', maxlength: 'Maximo 30 caracteres' },
                            'TextAreaTecnologias': { maxlength: 'Maximo 30 caracteres' },
                            'HidValidEmailReg': { required: '' }
                        },
                        errorPlacement: function (error, element) {
                            error.insertAfter(element);
                            error.addClass('message');  // add a class to the wrapper
                            error.css("color", "red");
                            error.css("display", "-webkit-inline-box");
                        },

                        debug: true,

                        submitHandler: function (Button4seg) {

                            Button4seg.submit();

                        }
                    });
                }
                else if (g == "Grupos") {
                    $("#BodyForm").validate({
                        rules: {
                            'TextBoxNomGrupo': { required: true, maxlength: 30
                            },
                            'TextBoxDescripcionGrupo': { required: true, maxlength: 65
                            }
                        },
                        messages: {
                            'TextBoxNomGrupo': { required: 'Ingrese un nombre del Grupo' },
                            'TextBoxDescripcionGrupo': { required: 'Ingrese una descripcion del Grupo' }
                        },
                        errorPlacement: function (error, element) {
                            error.insertAfter(element)
                            error.addClass('message');  // add a class to the wrapper
                            error.css("color", "red");
                            error.css("display", "-webkit-inline-box");
                        },

                        debug: true,
                        submitHandler: function (Button5seg) {
                            Button5seg.submit();
                        }
                    });
                }
                else if (g == "Perfiles") {
                    $("#BodyForm").validate({
                        rules: {
                            'TextBoxNomPerfil': { required: true, maxlength: 30
                            },
                            'TextBoxDescripcionPerfil': { required: true, maxlength: 65
                            }
                        },
                        messages: {
                            'TextBoxNomPerfil': { required: 'Ingrese un nombre del Perfil' },
                            'TextBoxDescripcionPerfil': { required: 'Ingrese una descripcion del Perfil' }
                        },
                        errorPlacement: function (error, element) {
                            error.insertAfter(element)
                            error.addClass('message');  // add a class to the wrapper
                            error.css("color", "red");
                            error.css("display", "-webkit-inline-box");
                        },

                        debug: true,
                        submitHandler: function (Button6seg) {
                            Button6seg.submit();
                        }
                    });
                }
                else if (g == "Sistemas") {
                    validaSistema();
                }

            });
            //            Modulo Tarea
            //            $('#<%= Button1.ClientID%>').on('click', function () {
            //                if ((g == "Componentes") || (g == "CasosUso") || g == ("Requerimientos")
            //                || (g == "Tareas") || (g == "Proyectos")) {
            //                    $("#BodyForm").validate({
            //                        rules: {
            //                            'TextBoxClave': { required: true, maxlength: 30
            //                            },
            //                            'TextBoxNombre': { required: true, maxlength: 30
            //                            },
            //                            'TextBoxCliente': { required: true, maxlength: 30
            //                            },
            //                            'TextBoxFechaInicio': { required: true
            //                            }
            //                        },
            //                        messages: {
            //                            'TextBoxClave': { required: 'Ingrese una Clave' },
            //                            'TextBoxNombre': { required: 'Ingrese un Nombre' },
            //                            'TextBoxCliente': { required: 'Ingrese nombre del Cliente' },
            //                            'TextBoxFechaInicio': { required: 'Ingrese fecha de inicio'
            //                            }
            //                        },
            //                        errorPlacement: function (error, element) {
            //                            error.insertAfter(element);
            //                            error.addClass('message');  // add a class to the wrapper
            //                            error.css("color", "red");
            //                            error.css("display", "block");
            //                        },

            //                        debug: true,
            //                        submitHandler: function (Button1) {
            //                            Button1.submit();
            //                        }
            //                    });
            //                }
            //            });

            //            $('#<%= Button2.ClientID%>').on('click', function () {
            //                invalidarTarea();
            //            });
            //            $('#<%= Button3.ClientID%>').on('click', function () {
            //                invalidarTarea();
            //            });

            //            function invalidarTarea() {
            //                if ((g == "Componentes") || (g == "CasosUso") || g == ("Requerimientos")
            //                || (g == "Tareas") || (g == "Proyectos")) {
            //                    $('#TextBoxClave').rules("remove");
            //                    $('#TextBoxNombre').rules("remove");
            //                    $('#TextBoxCliente').rules("remove");
            //                    $('#TextBoxFechaInicio').rules("remove");
            //                }
            //            }

        });       
    </script>    
    <script>
        $(function () {
            $("#TextBoxFechaInicio").datepicker({ dateFormat: "yy-mm-dd" });
            $("#TextBoxFechaFinEst").datepicker({ dateFormat: "yy-mm-dd" });
            $("#TextBoxFechaFinReal").datepicker({ dateFormat: "yy-mm-dd" });
        });
    </script>
</head>
<body runat="server">
    <form id="BodyForm" runat="server">
    
    <asp:HiddenField ID="hidAccordionIndex" runat="server" Value="0" />
    <asp:HiddenField ID="HiddenValidUser" runat="server" Value="" />
    <asp:HiddenField ID="HidValidUserUpdate" runat="server" Value="" />
    <asp:HiddenField ID="HiddenRowIndexSegUpd" runat="server" Value="" />
    <asp:HiddenField ID="HidUsuSeleccionadoSeg" runat="server" Value="0" />
    <asp:HiddenField ID="HidValidEmailReg" runat="server" Value="" />    
    <asp:HiddenField ID="HidValidEmailRestoreSeg" runat="server" Value="" />
    <asp:HiddenField ID="hidClicks" runat="server"  Value="0" />
    <asp:HiddenField ID="HidSistemaUpdate" runat="server"  Value="0" /> 
    <asp:HiddenField ID="hidopcionpant" runat="server"  Value="0" />  
    <asp:HiddenField ID="hidcontchecks" runat="server"  Value="0" /> 
    <asp:HiddenField ID="hidpantallaid" runat="server"  Value="" />
     <asp:HiddenField ID="hidcontchecksubop" runat="server"  Value="0" />   
     <asp:HiddenField ID="hidLiidButton" runat="server"  Value="" /> 
      <asp:HiddenField ID="hidindexpantalla" runat="server"  Value="0" /> 
      <asp:HiddenField ID="HidnoEliminar" runat="server"  Value="0" />
      <asp:HiddenField ID="HidnoEliminarSistema" runat="server"  Value="0" />
      <asp:HiddenField ID="accordionInd" runat="server"  Value="0" />

    <div id="header" align="center">
        <img src="../../Img/SolIcon.png" alt="Empresa" height="100px" width="20%" style="float: left;"/>
        <h1>Sistema de Control de Tareas </h1> 
            <p align="right">
            <asp:Label ID="usuarioLogin" runat="server" Text=""></asp:Label>
            <asp:LinkButton ID="LinkButtonSalir" OnClick="cerrarSesiononclick" runat="server" >Salir</asp:LinkButton>
            </p>
    </div>
    <div id="Container" runat="server">
        <div id="LeftSideMenu">
            <div id="accordion" runat="server">                       
                <h3 id="Proyecto1"  class="letras" runat="server" align="left">
                    Seguridad</h3> 
                <div id="DivProyecto1"  runat="server">                    
                         <asp:Label runat="server" CssClass="letras" ID="LlbCatalogoSeg">Catalogos</asp:Label>
                    <ul>
                        <li id="lblUsuariosSegLi" class="liacordion" ><asp:Label runat="server" CssClass="letras" ID="lblUsuariosSeg">Usuarios</asp:Label>
                            <ul>                           
                                <li id="LinkUsuariosSegLi" class="liacordion" >
                                    <asp:LinkButton CssClass="linkselect"  ID="LinkUsuariosSeg" CommandName="subUsuarios,,1" runat="server" OnClick="UsuariosOnClick">Usuarios</asp:LinkButton></li>
                                <li id="LinkcuentaUsSegLi" class="liacordion">
                                    <asp:LinkButton CssClass="linkselect"  ID="LinkcuentaUsSeg" CommandName="sub,,2" runat="server" OnClick="CuentaUsuarioOnClick">Cuenta</asp:LinkButton></li>                                                                
                            </ul>
                        </li>
                        <li class="liacordion" >
                            <asp:LinkButton ID="LinkGruposSeg" CommandName="Grupos,,3" runat="server" OnClick="GruposOnClick">Grupos</asp:LinkButton></li>
                        <li class="liacordion">
                            <asp:LinkButton ID="LinkPerfilesSeg" CommandName="Perfiles,,4" runat="server" OnClick="PerfilesOnClick">Perfiles</asp:LinkButton></li>
                        <li class="liacordion" ><asp:Label runat="server" CssClass="letras" ID="lblRelacionesSeg">Relaciones</asp:Label>
                            <ul>
                                <li class="liacordion">
                                    <asp:LinkButton ID="LinkRelacionesUsSeg" CommandName="subRelaciones,,5" runat="server" OnClick="RelacionesUsuariosOnClick">Usuarios</asp:LinkButton></li>
                                <li class="liacordion">
                                    <asp:LinkButton ID="LinkRelacionesGruSeg" CommandName="subRelaciones,,6" runat="server" OnClick="RelacionesGruposOnClick">Grupos</asp:LinkButton></li>
                                <li class="liacordion">
                                    <asp:LinkButton   ID="LinkRelacionesPerfSeg" CommandName="subRelaciones,,7" runat="server" OnClick="RelacionesPerfilesOnClick">Perfiles</asp:LinkButton></li>
                            </ul>
                        </li >                     
                    </ul>
                        <asp:Label runat="server" CssClass="letras" ID="LblControlAcceso">Control de acceso</asp:Label>
                    <ul>
                        <li class="liacordion"><asp:LinkButton ID="LB2CASistem" runat="server" CommandName="subSistemas,,8" OnClick="ControlAccesSistemaOnClick">Sistemas</asp:LinkButton></li>
                        <li class="liacordion"><asp:LinkButton ID="LB3CAModul" runat="server" CommandName="subCAModulo,,9" OnClick="ControlAccesModuloOnClick">Módulos</asp:LinkButton></li>
                        <li class="liacordion"><asp:LinkButton ID="LB4CAWindo" runat="server" CommandName="subCAPantalla,,10" OnClick="ControlAccesPantallasOnClick">Pantallas</asp:LinkButton></li>
                        <li class="liacordion"><asp:LinkButton ID="LB5CAOption" runat="server" CommandName="subCAOpcion,,11" OnClick="ControlAccesOpcionesOnClick">Opciones</asp:LinkButton></li>
                        <li class="liacordion"><asp:Label runat="server" CommandName="op4RelAcceso" CssClass="letras"  ID="LBLRelAcceso">Relaciones</asp:Label>
                            <ul><li class="liacordion">
                                    <asp:LinkButton ID="LBCASistema" CommandName="op5CASisMod,12,12" runat="server" OnClick="RelAccesoModSistemaOnClick">Sistemas</asp:LinkButton></li>                               
                                <li class="liacordion">
                                    <asp:LinkButton ID="LBCAPerfil" class="acordionButton" CommandName="op5CAPerfil,11,13" runat="server" OnClick="RelAccesoPerfilesOnClick">Perfiles</asp:LinkButton></li>
                                <li class="liacordion">
                                    <asp:LinkButton ID="LBCAUsu" CommandName="op5CAUsu,11,14" runat="server" OnClick="RelAccesoUsuariosOnClick">Usuarios</asp:LinkButton></li>
                               </ul>
                        </li>                       
                    </ul>
                </div>
                <h3 id="Proyecto2" class="letras" runat="server">
                    Tarea</h3>
                <div id="DivProyecto2" runat="server">
                    <ul>
                        <li class="liacordion">
                            <asp:LinkButton  ID="LinkProyecto" runat="server" CommandName="OpcionTarea,,15" OnClick="ProyectoOnClick">Proyecto</asp:LinkButton></li>
                        <li class="liacordion">
                            <asp:LinkButton ID="LinkRequerimiento" runat="server" CommandName="OpcionTarea,,16" OnClick="RequerimientoOnClick">Requerimiento</asp:LinkButton></li>
                        <li class="liacordion">
                            <asp:LinkButton ID="LinkCasosUso" runat="server" CommandName="OpcionTarea,,17" OnClick="CasosUsoOnClick">Casos de uso</asp:LinkButton></li>
                        <li class="liacordion">
                            <asp:LinkButton ID="LinkComponente" runat="server" CommandName="OpcionTarea,,18" OnClick="ComponenteOnClick">Componente</asp:LinkButton></li>
                        <li class="liacordion">
                            <asp:LinkButton ID="LinkTarea" runat="server" CommandName="OpcionTarea,,19" OnClick="TareaOnClick">Tarea</asp:LinkButton></li>
                    </ul>
                </div>
                <h3 id="Proyecto3" class="letras" runat="server">
                    Consultas y reporte</h3>
                <div id="DivProyecto3" runat="server"> 
                    <ul>
                         <li class="liacordion">
                            <asp:LinkButton ID="LinkButtonCRConsulta" runat="server" CommandName="OpcionCRConsulta,,21" OnClick="ConsultasOnClick">Consultas</asp:LinkButton></li>                    
                    </ul>
                </div>
                <h3 id="Proyecto4" class="letras" runat="server">
                    Seguimiento de tarea</h3>
                <div id="DivProyecto4" runat="server">
                    <ul>
                        <li class="liacordion"><asp:LinkButton ID="LinkButton1" CommandName="OpcionSeguimientoTarea,,20" runat="server" onclick="LinkButton1_Click">Actualizar Tarea</asp:LinkButton></li>
                    </ul>
                </div>
            </div>
        </div>
        <div id="Content" runat="server">
            <p align="center">
              <%--  <asp:Label ID="Label1" runat="server" Text="Gestor de " Font-Bold="True"></asp:Label>--%>
                <asp:Label ID="LabelNav" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                <asp:Label ID="LblSinAcessoOpciones" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
            </p>            
            <div id="ContentTop" runat="server">
                <asp:MultiView ID="MultiView1Seg" runat="server">
                    <asp:View ID="View1Seg" runat="server">
                  
                        <table id="Table2">
                        <tr>
                                <td colspan="2">
                                   La busqueda de un Usuario se puede realizar por: Nombre Y/O Apellidos.
                                </td>                           
                            </tr>
                            <tr>
                                <td>
                                    Nombre :
                                </td>
                                <td>
                                    <asp:TextBox MaxLength="30" ID="TextBoxNomUsuario" runat="server">    
                                    </asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr><td colspan="2">
                            </td>                                
                            </tr>
                            <tr>
                                <td>
                                    Apellidos :
                                </td>
                                <td>
                                    <asp:TextBox MaxLength="30" ID="TextBoxApellidos" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Usuario :
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBoxUsuario" MaxLength="30" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contraseña : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <input id="PasswordUs" maxlength="12" type="password" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Confirmar Contraseña :
                                </td>
                                <td>
                                    <input id="PasswordConfirm" maxlength="12" type="password" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Es Empleado :
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="RadioButtonEmpleado" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Text="Si" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Movil :
                                </td>
                                <td>
                                    <asp:TextBox MaxLength="12" ID="TextBoxTelefono" TextMode="Phone" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Email :
                                </td>
                                <td>
                                    <asp:TextBox MaxLength="30" ID="TextBoxEmail" TextMode="Email" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tecnologias :
                                </td>
                                <td>
                                    <textarea runat="server" id="TextAreaTecnologias" name="TextAreaTecnologias" cols="20"
                                        rows="2"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="validarUsuario" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="LabEmailReg1" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        
                    </asp:View>
                    <asp:View ID="View2Seg" runat="server">
                        <table id="Table1Seg">
                         <tr>
                                <td colspan="2"> La busqueda de se puede realizar por: Nombre Y/O Descripción.
                                 </td>
                            </tr>
                            <tr>
                                <td>
                                    Nombre :     </td>    <td>
                                    <asp:TextBox MaxLength="30" ID="TextBoxNomGrupo" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Descripción :     </td>    <td>
                                    <asp:TextBox ID="TextBoxDescripcionGrupo" MaxLength="65"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="View3Seg" runat="server">
                        <table id="Table3">
                        <tr>
                                <td colspan="2"> La busqueda de se puede realizar por: Nombre Y/O Descripción.
                                 </td>
                            </tr>
                            <tr>
                                <td>
                                    Nombre :     </td>    <td>
                                    <asp:TextBox MaxLength="30" ID="TextBoxNomPerfil" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Descripción :    </td>    <td>
                                    <asp:TextBox MaxLength="65" ID="TextBoxDescripcionPerfil" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Puede Registrar : </td>    <td>
                                    <asp:RadioButtonList ID="RadioButtonListAltaPerfil" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Puede Eliminar : </td>    <td>
                                    <asp:RadioButtonList ID="RadioButtonListEliminarPerfil" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Puede Modificar : </td>    <td>
                                    <asp:RadioButtonList ID="RadioButtonListModificaPerfil" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewRelacionesSeg" runat="server">
                        <table id="Table1">
                            <tr>
                                <td>
                                    <asp:Label ID="LabelUsuariosSeg" runat="server" Text="Usuarios"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="DropDownListadoSeg" AutoPostBack="True" OnSelectedIndexChanged="SeleccionDropDownList"
                                        runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="LabelGrupAsigSeg" runat="server" Text="Grupos Asignados"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="LabelGruposSeg" runat="server" Text="Grupos No Asignados"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="ListBoxGruposAsigSeg" runat="server"></asp:ListBox>
                                </td>
                                <td>
                                    <div>
                                        <asp:Button ID="ButtonAgregarGU" class="btn btn-primary" OnClientClick="return false" runat="server" Text="Asignar"
                                            BackColor="#0066FF" ForeColor="White" />
                                        <asp:Button ID="ButtonEliminarGU" class="btn btn-primary" runat="server" OnClientClick="return false" Text="Desasignar"
                                            BackColor="#0066FF" ForeColor="White" />
                                    </div>
                                    <br />
                                    <asp:Button ID="ButtonGuardarGU" class="btn btn-primary" runat="server" OnClick="guardarusurioGrupo" Text="Guardar"
                                        BackColor="#0066FF" ForeColor="White" />
                                </td>
                                <td>
                                    <asp:ListBox ID="ListBoxGruposSeg" runat="server"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                            <td colspan="2"><asp:Label ID="lblStatusRelacionUs" Text="" runat="server"> </asp:Label>  </td>
                            </tr>    
                        </table>
                    </asp:View>
                    <asp:View ID="View5Seg" runat="server">
                        <table id="Table4">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label3" runat="server" Text="Actualización de Contraseña"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Usuario :
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBoxUsuarioUpdate" Enabled="false" MaxLength="30" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contraseña Nueva : 
                                </td>
                                <td>
                                    <input id="PasswordUsUpdate" maxlength="12" type="password" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Confirmar Contraseña Nueva:
                                </td>
                                <td>
                                    <input id="PasswordConfirmUpdate" maxlength="12" type="password" runat="server" />
                                </td>
                            </tr>                            
                            <tr>
                                <td>
                                    <asp:Button ID="ButtonActualizaUsSeg" class="btn btn-primary" runat="server" Text="Actualizar" OnClick="ButtonActualizaUsuSeg_Click" />
                                </td>
                            </tr>
                        </table>
                        <table>
                        <tr>
                                <td>
                                    <asp:Label ID="validarUsuarioUpdate" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="msgactualizado" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewIngresaEmail" runat="server">
                        <table id="Table5">
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="¿No puedes Iniciar Sesion?"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Introduce tu dirección de correo electrónico registrada y te enviaremos instrucciones de ayuda."></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Correo electrónico registrado :
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="TextBoxEmailRegistrado" MaxLength="30" runat="server"></asp:TextBox>
                                </td>
                            </tr>                            
                            <tr>
                                <td>
                                    <asp:Button ID="ButtonEnviarEmailSeg" class="btn btn-primary" runat="server" Text="Enviar E-mail" OnClick="ButtonEnviarEmailSeg_Click" />
                                </td>
                            </tr>
                        </table>
                        <table>
                        <tr>
                                <td>
                                    <asp:Label ID="LabEmailReg" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="View5RestablecerDatosUs" runat="server">
                        <table id="Table6">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label4" runat="server" Text="Restablecer Contraseña"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Usuario :
                                </td>
                                <td>
                                    <asp:Label ID="LabUserRestore" runat="server" Font-Bold="True" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contraseña : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <input id="PasswordUsRestore" maxlength="12" type="password" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Confirmar Contraseña :
                                </td>
                                <td>
                                    <input id="PasswordUsRestoreConfirm" maxlength="12" type="password" runat="server" />
                                </td>
                            </tr>                                                                                                               
                            <tr>
                                <td>
                                    <asp:Button ID="RestablecerPasswordEmail" class="btn btn-primary" OnClick="RestablecerPasswordEmail_Click"
                                        runat="server" Text="Restablecer" />
                                </td>
                                <td>
                                   <asp:HyperLink ID="HyperLinkSesion" NavigateUrl="~/Views/Login/Inicio.aspx" runat="server">Iniciar Sesion</asp:HyperLink>
                                </td>
                            </tr>                            
                        </table>
                        <table>
                        <tr>
                                <td>
                                    <asp:Label ID="LabRestorePass" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewExpirado" runat="server">
                        <table id="Table7">
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="El link ha expirado :D" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewSistema" runat="server">
                       <table id="Table8">
                       <tr>
                                <td colspan="2">
                                    La busqueda de un sistema se realiza por: Clave Y/O Nombre.
                                </td>
                            </tr>
                            <tr>                                
                                    <td>Clave : </td><td><asp:TextBox MaxLength="30" ID="TextBoxClaveSis"
                                        runat="server"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                <td>Nombre : </td><td>
                                    <asp:TextBox ID="TextBoxNombreSis" MaxLength="30" runat="server"></asp:TextBox>
                                </td>
                                </tr> 
                                <tr>
                                <td>Cliente : </td><td>
                                    <asp:TextBox ID="TextBoxClienteSis" MaxLength="45" runat="server"></asp:TextBox>
                                </td>
                                </tr>                               
                            <tr>
                                <td>
                                    Descripción :</td><td>
                                    <asp:TextBox ID="TextBoxDescSis" MaxLength="65" runat="server"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                    Fecha inicio : </td><td>
                                    <asp:TextBox ID="TextBoxFechaoIniSis" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                                <tr>
                                <td>
                                    Fecha fin estimada : </td><td>
                                    <asp:TextBox ID="TextBoxFechaFinEsSis" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha fin real : </td><td>
                                    <asp:TextBox ID="TextBoxFinRealSis" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tecnologias : </td><td>
                                    <asp:TextBox MaxLength="40" ID="TextBoxTecSistema" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                            <td>                                                
                   </td>
                            </tr>
                        </table> 
                        <div runat="server" id="opcionesUpdateSistem">
                        <asp:Button ID="ButtonUpdateSistema" class="btn btn-primary" runat="server" Text="Actualizar"
                        OnClick="ButtonUpdateSistemaseg_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="ButtonCancelSistema"
                             runat="server" class="btn btn-primary" Text="Cancelar" OnClick="ButtonCancelSistemaseg_Click" />
                             &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="ButtonConsultaSistemasSeg"
                             runat="server" class="btn btn-primary" Text="Consultas" OnClick="ButtonConsultaSistemasSeg_Click" />
                        </div>
                              <table>  
                            <tr>
                                <td>
                                    <asp:Label  ID="lblUpdateSistema"   runat="server"></asp:Label></td>
                            </tr>
                        </table>                                      
                    </asp:View>
                    <asp:View ID="ViewModulo" runat="server">
                        <div> 
                                    <asp:CheckBoxList ID="CheckBoxListModulo" runat="server">
                                    </asp:CheckBoxList> </div>
                                    <div><asp:Button ID="ButtonGuardarModuloSeg"
                             runat="server" Text="Registrar" class="btn btn-primary" OnClick="ButtonGuardarModuloSeg_Click"  />                                                            
                             <asp:Button ID="ButtonDesactivaModulo" 
                             runat="server" Text="Desactivar" class="btn btn-primary" OnClick="ButtonDesactivaModuloSeg_Click"  />
                             <asp:Button ID="ButtonEliminaModulo" 
                             runat="server" Text="Activar" class="btn btn-primary" OnClick="ButtonEliminaModuloSeg_Click"  />
                             <asp:Button ID="Button4"
                             runat="server" Text="Actualizar" class="btn btn-primary" OnClick="ButtonAtualizaModuloSeg_Click"  />                             
                             </div>                                                        
                             <div><asp:Label  ID="LabUpdateModulo"   runat="server"></asp:Label></div>                                                          
                    </asp:View>
                    <asp:View ID="ViewPnntalla" runat="server">                                                                  
<div id="tree" runat="server">
     <ul id="ulconttree" runat="server">        
            </ul>
</div>
 <div><asp:Button ID="ButtonregistraPantalla"
                             runat="server" Text="Registrar" class="btn btn-primary" OnClick="ButtonRegistrarPantallasSeg_Click"  />                                                          
                             <asp:Button ID="Button7"
                             runat="server" Text="Desactivar" class="btn btn-primary" OnClick="ButtonDesactivarPantallaOpcion_Click"  />
                             <asp:Button ID="ButtonDeletePantallaOpcion"
                             runat="server" Text="Activar" class="btn btn-primary" OnClick="ButtonDeletePantallaOpcion_Click"  />
                             <asp:Button ID="ButtonUpdatepantalla" class="btn btn-primary"
                             runat="server" Text="Actualizar" OnClick="ButtonAtualizaPantallasSeg_Click"  />
                             </div>
                             <div><asp:Label  ID="LblupdatePantalla"   runat="server"></asp:Label></div>
                             <div><asp:Label  ID="LblSinRelacion"   runat="server"></asp:Label></div>
                    </asp:View>
                    <asp:View ID="ViewRelAccesoPerfil" runat="server">
                 <div id="divPerfilAcList" class="PerfilAcList">  
                  <asp:DropDownList ID="DropDownAccesoRelaciones" AutoPostBack="True" OnSelectedIndexChanged="SeleccionDropDownListAcceso"
                                        runat="server">
                                    </asp:DropDownList></div> 
                                   <div class="contTreePerfil">                                                                 
<div id="DivTreePerfil" runat="server">
     <ul id="ulTreeAccePerfil" runat="server">       
            </ul>
</div>
 <div><asp:Button ID="Button5" class="btn btn-primary"
                             runat="server" Text="Asignar" OnClick="ButtonRegistrarRelAccesoSeg_Click"  />
                             <asp:Button ID="Button6" class="btn btn-primary"
                             runat="server" Text="Actualizar" OnClick="ButtonAtualizaRelAccesoSeg_Click"  />
                             </div>
                             <div><asp:Label  ID="LblStatusAccePerfil"   runat="server"></asp:Label></div>
                             </div>
                             <div style="float:left;"><asp:Label  ID="LblSinRelacionRel"   runat="server"></asp:Label></div>
                    </asp:View>
                    <asp:View ID="ViewModuloSistema" runat="server">
                 <div id="div1" class="PerfilAcList">  
                  <asp:DropDownList ID="DropDownListModSis" AutoPostBack="True" OnSelectedIndexChanged="SeleccionDropDownModuloSistema"
                                        runat="server">
                                    </asp:DropDownList></div> 
                                   <div class="contTreePerfil">                                                                 
<div id="DivTreeModSis" runat="server">
     <ul id="ulModSis" runat="server">       
            </ul>
</div>
 <div><asp:Button ID="ButtonAsignarModSis" class="btn btn-primary"
                             runat="server" Text="Asignar" OnClick="ButtonRegistrarRelAccesoSisModSeg_Click"  />
                             <asp:Button ID="ButtonDesasignarModSis" class="btn btn-primary"
                             runat="server" Text="Actualizar" OnClick="ButtonDeleteRelAccesoSisModSeg_Click"  />
                             </div>
                             <div><asp:Label  ID="LblStatusModSis" Text=""  runat="server"></asp:Label></div>
                             </div>
                    </asp:View>
                </asp:MultiView>
                <asp:MultiView ID="MultiView2" runat="server">
                    <asp:View ID="View4" runat="server">
                        <table id="TableCont">
                        <tr>
                                <td>
                                    Clave :   </td>
                                <td>
                                    Cliente : </td> 
                                <td>
                                    Fecha de Inicio : </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="TextBoxClave"
                                       MaxLength="30" runat="server"></asp:TextBox>
                                </td>
                                 <td>
                                    <asp:TextBox 
                                    MaxLength="30" ID="TextBoxCliente" runat="server"></asp:TextBox>
                                </td>
                                  <td>
                                    <asp:TextBox ID="TextBoxFechaInicio"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Descripcion :</td>
                                <td>
                                    Estado : </td>
                                <td>
                                    Fecha fin estimada : </td> 
                            </tr>
                            <tr>  
                            <td>
                                    <asp:TextBox ID="TextBoxDescripcion" MaxLength="30" runat="server"></asp:TextBox>
                                </td>
                                 <td>
                                    <asp:DropDownList ID="DropDownList1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                 <td>
                                    <asp:TextBox ID="TextBoxFechaFinEst" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nombre :</td> 
                                <td><asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                    ID Dependencia :</td>
                                <td>
                                    Fecha fin real : </td>
                            </tr>
                            <tr>
                               <td>
                                    <asp:TextBox MaxLength="30" ID="TextBoxNombre" runat="server"></asp:TextBox>
                                </td>
                                 <td>
                                    <asp:DropDownList ID="DropDownListDep" runat="server">
                                    </asp:DropDownList>
                                </td>
                                 <td>
                                    <asp:TextBox ID="TextBoxFechaFinReal" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <p><asp:Label ID="LabelHrs" runat="server" Text="Registrar horas : "></asp:Label> 
                            <asp:DropDownList ID="DropDownListHoras" runat="server">
                                <asp:ListItem Value="01">1</asp:ListItem>
                                <asp:ListItem Value="02">2</asp:ListItem>
                                <asp:ListItem Value="03">3</asp:ListItem>
                                <asp:ListItem Value="04">4</asp:ListItem>
                                <asp:ListItem Value="05">5</asp:ListItem>
                                <asp:ListItem Value="06">6</asp:ListItem>
                                <asp:ListItem Value="07">7</asp:ListItem>
                                <asp:ListItem Value="08">8</asp:ListItem>
                                <asp:ListItem Value="09">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="16">16</asp:ListItem>
                            </asp:DropDownList>
                        </p>
                    </asp:View>
                </asp:MultiView>
                <asp:MultiView ID="MultiViewConsultaReporte" runat="server">
                    <asp:View ID="ViewConsultaBusqueda" runat="server">
                    <table>
                    <tr>
                    <td>
                    Fecha Inicio:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxFechaInicioCR" runat="server"></asp:TextBox>
                    </td>                                         
                    <td>
                    Fecha Fin Estimada:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxFechaFinEstimCR" runat="server"></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    Fecha Fin Real:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxFinRCR" runat="server"></asp:TextBox>

                    </td>
                    <td>
                    Usuario:
                    </td>
                    <td>                       
                        <asp:DropDownList ID="DropDownListUsuarioCR" runat="server">
                        <asp:ListItem>dedede</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    Tarea:
                    </td>
                    <td>                       
                         <asp:TextBox ID="TextBoxTarea" runat="server"></asp:TextBox>
                    </td>
                    <td>
                    Estado:
                    </td>
                    <td>                       
                        <asp:DropDownList ID="DropDownListEstado" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="0">Activo</asp:ListItem>
                        <asp:ListItem Value="1">InActivo</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                    <td>
                   Opciones Tarea:
                    </td>
                    <td>                       
                        <asp:DropDownList ID="DropDownListOpcTareasCR" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Componente</asp:ListItem>
                        <asp:ListItem>Casos de Uso</asp:ListItem>
                        <asp:ListItem>Requerimiento</asp:ListItem>
                         <asp:ListItem>Proyecto</asp:ListItem>
                          
                           
                            
                        </asp:DropDownList>
                    </td>
                    <td>
                    Proyecto:
                    </td>
                    <td>                       
                        <asp:DropDownList ID="DropDownListSistemaCR" runat="server">
                       
                        </asp:DropDownList>
                    </td>
                    </tr>
                    </table>                   
                     <asp:Button ID="ButtonBusquedaCR" class="btn btn-primary"  runat="server" Text="Buscar" 
          onclick="ButtonBusquedaCR_Click"  /> 
         <br style="height:80;" />
                        <asp:GridView  ID="GridViewCRConsulta" runat="server" Width="498px" BackColor="White" 
                            BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                           
                            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                            <RowStyle BackColor="White" ForeColor="#003399" />
                            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            <SortedAscendingCellStyle BackColor="#EDF6F6" />
                            <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                            <SortedDescendingCellStyle BackColor="#D6DFDF" />
                            <SortedDescendingHeaderStyle BackColor="#002876" />
                           
                        </asp:GridView>   
                        <asp:DataGrid ID="DataGridtareas" runat="server">
                        </asp:DataGrid>
                        <br />
                        <asp:Button ID="ButtonExportarExcelCR" class="btn btn-primary"  runat="server" Text="Exportar a Excel" 
          onclick="ButtonExportarExcelCR_Click"  /> 
           <br />
                        <asp:Label ID="LblStatusDescargaCR" runat="server" Text=""></asp:Label>                        
                    </asp:View>
                </asp:MultiView>
                <asp:MultiView ID="MultiViewSeguimientoTarea" runat="server">
                <asp:View ID="ViewActualizaTarea" runat="server">                
   <br />
   <h3>Cronometro de Tareas</h3>      
    <asp:Label ID="LabelSeguimientoTarea" runat="server" Font-Size="XX-Large" Text="00:00:00"></asp:Label><br />
  <span> </span>
      <asp:Button ID="ButtonIniciar" class="btn btn-primary"  runat="server" Text="Iniciar" 
          onclick="ButtonIniciar_Click" Height="40px" />
      <asp:Button ID="ButtonReset" class="btn btn-primary" runat="server" 
          Text="Resetear" onclick="ButtonReset_Click" Height="40px" />
      <asp:Button ID="ButtonEnviar" class="btn btn-primary" runat="server" Text="Registrar tiempo" Height="40px" 
          Width="110px" onclick="ButtonEnviar_Click" />       
   <h4>Actualizar Tareas por archivo :</h4><br />
        <asp:FileUpload ID="FileUploadTareas"  runat="server" /><br />
        <asp:Button ID="ButtonUpload" class="btn btn-primary" runat="server" Text="Subir archivo" 
            onclick="ButtonUpload_Click" /><asp:Label ID="LabelSubir" runat="server" 
            BackColor="Red"></asp:Label>
        <asp:GridView ID="GridView2" runat="server">
        </asp:GridView>
                    </asp:View>
                </asp:MultiView>                
                 <div id="dialog-confirm" style="display:none" title="Proceder Eliminación?">
            <p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>Esta seguro de proceder con la eliminación?</p>
                </div>
                <div runat="server" id="opcionesRegSeg">
                    <asp:Button ID="Button4seg" class="btn btn-primary" validate="required:true" runat="server" Text="Insertar"
                        OnClick="Button4seg_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button5seg" class="btn btn-primary"
                            validate="required:false" runat="server" Text="Buscar" OnClick="Button5seg_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                                ID="Button6seg" runat="server" class="btn btn-primary" validate="required:false" Text="Mostrar todo"
                                OnClick="Button6seg_Click" />
                                 </div>
                <div runat="server" id="opcionesRegTarea" >
                    <asp:Button ID="Button1" class="btn btn-primary" runat="server" Text="Insertar" OnClick="Button1_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                        ID="Button2" class="btn btn-primary" runat="server" Text="Buscar" OnClick="Button2_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                            ID="Button3" class="btn btn-primary" runat="server" Text="Mostrar todo" OnClick="Button3_Click" />
                </div>
                
            </div>
            <div id="ContentBot">                
                <asp:MultiView ID="MultiViewTareaGrid" runat="server">
                    <asp:View ID="View5GridTarea" runat="server">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowEditing="GridView1_RowEditing"
                    OnRowDeleting="GridView1_RowDeleting" OnPageIndexChanging="GridView1_PageIndexChanging"
                    OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating"
                    BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                    CellPadding="4" ForeColor="Black" GridLines="Vertical">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField HeaderText="ID" ReadOnly="True" DataField="IDProyectos" />
                        <asp:BoundField HeaderText="Clave" DataField="ClaveProyecto" />
                        <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                        <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" />
                        <asp:BoundField HeaderText="Cliente" DataField="Cliente" />
                        <asp:BoundField HeaderText="Fecha registro" DataField="FechaRegistro" DataFormatString="{0:yyyy/MM/dd}"
                            ApplyFormatInEditMode="true" ReadOnly="True" />
                        <asp:BoundField HeaderText="Fecha inicio" DataField="FechaInicio" DataFormatString="{0:yyyy/MM/dd}"
                            ApplyFormatInEditMode="true" />
                        <asp:BoundField HeaderText="Fecha fin estimada" DataField="FechaFinEstimada" DataFormatString="{0:yyyy/MM/dd}"
                            ApplyFormatInEditMode="true" />
                        <asp:BoundField HeaderText="Fecha fin real" DataField="FechaFinReal" DataFormatString="{0:yyyy/MM/dd}"
                            ApplyFormatInEditMode="true" />
                        <asp:BoundField HeaderText="Tecnologias" DataField="Tecnologias" />
                        <asp:CommandField ShowEditButton="true" />
                        <asp:CommandField ShowDeleteButton="true" />
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#F7F7DE" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" Width="10px" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                </asp:GridView>
                    </asp:View>
                </asp:MultiView>

                <asp:MultiView ID="MultiView2SegGrid" runat="server">
                    <asp:View ID="View1" runat="server">
                        <asp:GridView ID="GridView2Seg" runat="server" AutoGenerateColumns="False" OnRowEditing="GridView2Seg_RowEditing"
                            OnRowDeleting="GridView2Seg_RowDeleting" OnPageIndexChanging="GridView2Seg_PageIndexChanging"
                            OnRowCancelingEdit="GridView2Seg_RowCancelingEdit" OnRowUpdating="GridView2Seg_RowUpdating"
                            BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                            CellPadding="4" ForeColor="Black" GridLines="Vertical">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField HeaderText="ID" ReadOnly="True" DataField="IDProyectos" />
                                <asp:BoundField HeaderText="Clave" ReadOnly="True" DataField="ClaveProyecto" />
                                <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                                <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" />
                                <asp:BoundField HeaderText="Cliente" DataField="Cliente" />
                                <asp:BoundField HeaderText="Fecha registro" DataField="FechaRegistro" DataFormatString="{0:yyyy/MM/dd}"
                                    ApplyFormatInEditMode="true" />
                                <asp:BoundField HeaderText="Fecha inicio" DataField="FechaInicio" DataFormatString="{0:yyyy/MM/dd}"
                                    ApplyFormatInEditMode="true" />
                                <asp:BoundField HeaderText="Fecha fin estimada" DataField="FechaFinEstimada" DataFormatString="{0:yyyy/MM/dd}"
                                    ApplyFormatInEditMode="true" />
                                <asp:BoundField HeaderText="Fecha fin real" DataField="FechaFinReal" DataFormatString="{0:yyyy/MM/dd}"
                                    ApplyFormatInEditMode="true" />
                                <asp:BoundField HeaderText="Tecnologias" DataField="Tecnologias" />
                                <asp:CommandField ShowEditButton="true" />
                                <asp:CommandField ShowDeleteButton="true" />
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" Width="10px" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />
                        </asp:GridView>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <asp:GridView ID="GridView2SegGrupo" runat="server" AutoGenerateColumns="False" OnRowEditing="GridView2Seg_RowEditing"
                            OnRowDeleting="GridView2Seg_RowDeleting" OnPageIndexChanging="GridView2Seg_PageIndexChanging"
                            OnRowCancelingEdit="GridView2Seg_RowCancelingEdit" OnRowUpdating="GridView2Seg_RowUpdating"
                            BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                            CellPadding="4" ForeColor="Black" GridLines="Vertical">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField HeaderText="ID" ReadOnly="True" DataField="IDProyectos" />
                                <asp:BoundField HeaderText="Clave" DataField="ClaveProyecto" />
                                <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                                <asp:CommandField ShowEditButton="true" />
                                <asp:CommandField ShowDeleteButton="true" />
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" Width="10px" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />
                        </asp:GridView>
                    </asp:View>
                    <asp:View ID="View3" runat="server">
                        <asp:GridView ID="GridView2SegPerfil" runat="server" AutoGenerateColumns="False"
                            OnRowEditing="GridView2Seg_RowEditing" OnRowDeleting="GridView2Seg_RowDeleting"
                            OnPageIndexChanging="GridView2Seg_PageIndexChanging" OnRowCancelingEdit="GridView2Seg_RowCancelingEdit"
                            OnRowUpdating="GridView2Seg_RowUpdating" BackColor="White" BorderColor="#DEDFDE"
                            BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField HeaderText="ID" ReadOnly="True" DataField="IDProyectos" />
                                <asp:BoundField HeaderText="Clave" DataField="ClaveProyecto" />
                                <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                                <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" />
                                <asp:BoundField HeaderText="Cliente" DataFormatString="{0:yyyy/MM/dd}" ApplyFormatInEditMode="true"
                                    DataField="Cliente" />
                                <asp:BoundField HeaderText="Tecnologias" DataField="Tecnologias" DataFormatString="{0:Y}"
                                    ApplyFormatInEditMode="true" />
                                <asp:CommandField ShowEditButton="true" />
                                <asp:CommandField ShowDeleteButton="true" />
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" Width="10px" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />
                        </asp:GridView>
                    </asp:View>
                    <asp:View ID="ViewSistemaGrid" runat="server">
                        <asp:GridView ID="GridViewsistema" runat="server" AutoGenerateColumns="False" OnRowEditing="GridView2Seg_RowEditing"
                            OnRowDeleting="GridView2Seg_RowDeleting" OnPageIndexChanging="GridView2Seg_PageIndexChanging"
                            OnRowCancelingEdit="GridView2Seg_RowCancelingEdit" OnRowUpdating="GridView2Seg_RowUpdating"
                            BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                            CellPadding="4" ForeColor="Black" GridLines="Vertical">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField HeaderText="ID" ReadOnly="True" DataField="IDSistemas" />
                                <asp:BoundField HeaderText="Clave" ReadOnly="True" DataField="ClaveSistemas" />
                                <asp:BoundField HeaderText="Nombre" ReadOnly="True" DataField="Nombre" />
                                <asp:BoundField HeaderText="Descripcion" ReadOnly="True" DataField="Descripcion" />
                                <asp:BoundField HeaderText="Cliente" ReadOnly="True" DataField="Cliente" />
                                <asp:BoundField HeaderText="Fecha registro" ReadOnly="True" DataField="FechaRegistro" DataFormatString="{0:yyyy/MM/dd}"
                                    ApplyFormatInEditMode="true" />
                                <asp:BoundField HeaderText="Fecha inicio" ReadOnly="True" DataField="FechaInicio" DataFormatString="{0:yyyy/MM/dd}"
                                    ApplyFormatInEditMode="true" />
                                <asp:BoundField HeaderText="Fecha fin estimada" ReadOnly="True" DataField="FechaFinEstimada" DataFormatString="{0:yyyy/MM/dd}"
                                    ApplyFormatInEditMode="true" />
                                <asp:BoundField HeaderText="Fecha fin real" ReadOnly="True" DataField="FechaFinReal" DataFormatString="{0:yyyy/MM/dd}"
                                    ApplyFormatInEditMode="true" />
                                <asp:BoundField HeaderText="Tecnologias" ReadOnly="True" DataField="Tecnologias" />
                                <asp:CommandField ShowEditButton="true" />
                                <asp:CommandField ShowDeleteButton="true"/>
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" Width="10px" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />
                        </asp:GridView>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
        <div id="RightSideMenu">
            <p>
            </p>
        </div>
    </div>
     <script type="text/javascript">
         Sys.debug = true;
         Sys.require(Sys.components.filteredTextBox, function () {
             $('#TextBoxTelefono').filteredTextBox({
                 FilterType: Sys.Extended.UI.FilterTypes.Numbers
             });
             $('#TextBoxNomUsuario').filteredTextBox({
                 ValidChars: " ",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
             });
             $('#TextBoxApellidos').filteredTextBox({
                 ValidChars: " ",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
             });
             $('#TextBoxNomGrupo').filteredTextBox({
                 ValidChars: " ",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
             });
             $('#TextBoxNomPerfil').filteredTextBox({
                 ValidChars: " ",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
             });
             $('#TextBoxUsuarioUpdate').filteredTextBox({
                 InValidChars: " ",
                 ValidChars: ".",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
             });
             $('#PasswordUsUpdate').filteredTextBox({
                 InValidChars: " ",
                 ValidChars: ".",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
             });
             $('#PasswordConfirmUpdate').filteredTextBox({
                 InValidChars: " ",
                 ValidChars: ".",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
             });
             $('#TextBoxUsuario').filteredTextBox({
                 InValidChars: " ",
                 ValidChars: ".",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
             });
             $('#PasswordUs').filteredTextBox({
                 InValidChars: " ",
                 ValidChars: ".",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
             });
             $('#PasswordConfirm').filteredTextBox({
                 InValidChars: " ",
                 ValidChars: ".",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
             });
             $('#PasswordUsRestore').filteredTextBox({
                 InValidChars: " ",
                 ValidChars: ".",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
             });
             $('#PasswordUsRestoreConfirm').filteredTextBox({
                 InValidChars: " ",
                 ValidChars: ".",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
             });
             $('#TextBoxEmailRegistrado').filteredTextBox({
                 InValidChars: " ",
                 ValidChars: ".@",
                 FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
             });
         });       

         $(window).bind('load', function () {             
                 var elimina = $('#<%=HidnoEliminar.ClientID%>').val();
                 if (elimina !== "0") {
                     $('#<%=HidnoEliminar.ClientID%>').val('0');
                     alert('El usuario tiene asociado un sistema y módulos, no puede ser eliminado, Contacte con el Administrador.');
                 }
                 var elimina2 = $('#<%=HidnoEliminarSistema.ClientID%>').val();
                 if (elimina2 !== "0") {
                     $('#<%=HidnoEliminarSistema.ClientID%>').val('0');
                     alert('El sistema tiene asociado perfiles y usuarios, no puede ser eliminado, Contacte con el Administrador.');
                 }             
         });
    </script>
    </form>
    
</body>
</html>
