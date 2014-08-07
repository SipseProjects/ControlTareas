<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Main.aspx.cs"
    Inherits="ProyectosWeb.Main" %>

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
    
    <script>
        $(function () {
            $("#accordion").accordion({ collapsible: true,
                animated: 'slide',
                autoHeight: false,
                navigation: true
        });
        });     
    </script>
<!-- include jQuery and jQueryUI libraries --><%--
<script type="text/javascript" src="http://code.jquery.com/jquery-1.9.1.js"></script>
<script type="text/javascript" src="http://code.jquery.com/ui/1.10.1/jquery-ui.js"></script>
<link rel="stylesheet" type="text/css" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css"/>--%>

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
    <script type="text/javascript">
        Sys.debug = true;
        Sys.require(Sys.components.filteredTextBox, function () {
            $("#TextBoxTelefono").filteredTextBox({
                FilterType: Sys.Extended.UI.FilterTypes.Numbers
            });
            $("#TextBoxNomUsuario").filteredTextBox({
                ValidChars: " ",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
            });
            $("#TextBoxApellidos").filteredTextBox({                                   
                ValidChars: " ",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
            });
            $("#TextBoxNomGrupo").filteredTextBox({
                ValidChars: " ",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
            });
            $("#TextBoxNomPerfil").filteredTextBox({
                ValidChars: " ",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
            });
            $("#TextBoxUsuarioUpdate").filteredTextBox({
                InValidChars: " ",
                ValidChars: ".",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
            });
            $("#PasswordUsUpdate").filteredTextBox({
                InValidChars: " ",
                ValidChars: ".",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
            });
            $("#PasswordConfirmUpdate").filteredTextBox({
                InValidChars: " ",
                ValidChars: ".",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
            });
            $("#TextBoxUsuario").filteredTextBox({
                InValidChars: " ",
                ValidChars: ".",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
            });
            $("#PasswordUs").filteredTextBox({
                InValidChars: " ",
                ValidChars: ".",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
            });
            $("#PasswordConfirm").filteredTextBox({
                InValidChars: " ",
                ValidChars: ".",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
            });
            $("#PasswordUsRestore").filteredTextBox({
                InValidChars: " ",
                ValidChars: ".",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
            });
            $("#PasswordUsRestoreConfirm").filteredTextBox({
                InValidChars: " ",
                ValidChars: ".",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
            });
            $("#TextBoxEmailRegistrado").filteredTextBox({
                InValidChars: " ",
                ValidChars: ".@",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
            });

        }); 
    </script>    
    <script type="text/javascript" src="validacion/jquery.validate.js"></script>
    <script type="text/javascript">        

        $(function () {
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
            }


            $('#<%= ButtonUpdateSistema.ClientID%>').on('click', function () {
                validaSistema();
            });

            function validaSistema() {
                $("#BodyForm").validate({
                    ignore: "",
                    rules: {
                        'TextBoxClaveSis': { required: true },
                        'TextBoxNombreSis': { required: true },
                        'TextBoxClienteSis': { required: true },
                        'TextBoxDescSis': { required: true },
                        'TextBoxFechaoIniSis': { required: true },
                        'TextBoxFechaFinEsSis': { required: true }
                    },
                    messages: {
                        'TextBoxClaveSis': { required: 'Ingrese una clave' },
                        'TextBoxNombreSis': { required: 'Ingrese un nombre' },
                        'TextBoxClienteSis': { required: 'Ingrese un nombre de cliente' },
                        'TextBoxDescSis': { required: 'Ingrese una descripción' },
                        'TextBoxFechaoIniSis': { required: 'Ingrese una fecha de Inicio' },
                        'TextBoxFechaFinEsSis': { required: 'Ingrese una fecha estimada' }
                    },
                    errorPlacement: function (error, element) {
                        error.insertAfter(element);
                        error.addClass('message');  // add a class to the wrapper
                        error.css("color", "red");
                    },
                    debug: true,
                    submitHandler: function (ButtonUpdateSistema) {
                        ButtonUpdateSistema.submit();
                    }
                });
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
                            'TextBoxTelefono': { number: true, maxlength: 12 },
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
                            'TextBoxTelefono': { number: 'Solo se permiten numeros', maxlength: 'Maximo 12 digitos' },
                            'PasswordUs': { required: 'Ingrese una contraseña', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'La contraseña debe ser minimo 5 caracteres' },
                            'PasswordConfirm': { required: 'La contraseña no coincide', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'La contraseña debe ser minimo 5 caracteres', equalTo: 'La contraseña no coincide' },
                            'HiddenValidUser': { required: '', maxlength: 'Maximo 30 caracteres' },
                            'TextAreaTecnologias': { maxlength: 'Maximo 30 caracteres' },
                            'HidValidEmailReg': { required: '' }
                        },
                        errorPlacement: function (error, element) {
                            error.insertAfter(element)
                            error.addClass('message');  // add a class to the wrapper
                            error.css("color", "red");
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
                            'TextBoxDescripcionGrupo': { required: true, maxlength: 30
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
                            'TextBoxDescripcionPerfil': { required: true, maxlength: 30
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
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
    <div id="header" align="center">
        <img src="../../Img/SolIcon.png" alt="Empresa" height="100px" width="20%" style="float: left;">
        <h1>
            Encabezado</h1> 
            <p align="right">
            <asp:Label ID="usuarioLogin" runat="server" Text=""></asp:Label>
            <asp:LinkButton ID="LinkButtonSalir" OnClick="cerrarSesiononclick" runat="server" >Salir</asp:LinkButton>
            </p>
    </div>
    <div id="Container" runat="server">
        <div id="LeftSideMenu">
            <div id="accordion" runat="server">                       
                <h3 id="Proyecto1" runat="server" align="left">
                    Seguridad</h3>
                <div id="DivProyecto1" runat="server">
                    <p align="left">
                        Catalogos</p>
                    <ul>
                        <li id="lblUsuariosSegLi" class="liacordion" ><asp:Label runat="server" ID="lblUsuariosSeg">Usuarios</asp:Label>
                            <ul>                           
                                <li id="LinkUsuariosSegLi" class="liacordion" >
                                    <asp:LinkButton  ID="LinkUsuariosSeg" CommandName="subUsuarios,,1" runat="server" OnClick="UsuariosOnClick">Usuarios</asp:LinkButton></li>
                                <li id="LinkcuentaUsSegLi" class="liacordion">
                                    <asp:LinkButton ID="LinkcuentaUsSeg" CommandName="sub,,2" runat="server" OnClick="CuentaUsuarioOnClick">Cuenta</asp:LinkButton></li>                                                                
                            </ul>
                        </li>
                        <li class="liacordion" >
                            <asp:LinkButton ID="LinkGruposSeg" CommandName="Grupos,,3" runat="server" OnClick="GruposOnClick">Grupos</asp:LinkButton></li>
                        <li class="liacordion">
                            <asp:LinkButton ID="LinkPerfilesSeg" CommandName="Perfiles,,4" runat="server" OnClick="PerfilesOnClick">Perfiles</asp:LinkButton></li>
                        <li class="liacordion" ><asp:Label runat="server" ID="lblRelacionesSeg">Relaciones</asp:Label>
                            <ul>
                                <li class="liacordion">
                                    <asp:LinkButton ID="LinkRelacionesUsSeg" CommandName="subRelaciones,,5" runat="server" OnClick="RelacionesUsuariosOnClick">Usuarios</asp:LinkButton></li>
                                <li class="liacordion">
                                    <asp:LinkButton ID="LinkRelacionesGruSeg" CommandName="subRelaciones,,6" runat="server" OnClick="RelacionesGruposOnClick">Grupos</asp:LinkButton></li>
                                <li class="liacordion">
                                    <asp:LinkButton   ID="LinkRelacionesPerfSeg" CommandName="subRelaciones,,7" runat="server" OnClick="RelacionesPerfilesOnClick">Perfiles</asp:LinkButton></li>
                            </ul>
                        </li >
                        <li class="liacordion"><a href="#">Fases</a></li>
                    </ul>
                        <asp:Label runat="server" ID="LblControlAcceso">Control de acceso</asp:Label>
                    <ul>
                        <li class="liacordion"><asp:LinkButton ID="LB2CASistem" runat="server" CommandName="subSistemas,,8" OnClick="ControlAccesSistemaOnClick">Sistemas</asp:LinkButton></li>
                        <li class="liacordion"><asp:LinkButton ID="LB3CAModul" runat="server" CommandName="subCAModulo,,9" OnClick="ControlAccesModuloOnClick">Módulos</asp:LinkButton></li>
                        <li class="liacordion"><asp:LinkButton ID="LB4CAWindo" runat="server" CommandName="subCAPantalla,,10" OnClick="ControlAccesPantallasOnClick">Pantallas</asp:LinkButton></li>
                        <li class="liacordion"><asp:LinkButton ID="LB5CAOption" runat="server" CommandName="subCAOpcion,,11" OnClick="ControlAccesOpcionesOnClick">Opciones</asp:LinkButton></li>
                        <li class="liacordion"><asp:Label runat="server" CommandName="op4RelAcceso" ID="LBLRelAcceso">Relaciones</asp:Label>
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
                <h3 id="Proyecto2" runat="server">
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
                <h3 id="Proyecto3" runat="server">
                    Consultas y reporte</h3>
                <div id="DivProyecto3" runat="server"> 
                    <ul>
                        <li class="liacordion">Horas: planeadas, reales</li>
                        <li class="liacordion">Avance</li>
                        <li class="liacordion">Fase</li>
                        <li class="liacordion">Usuarios-Persona</li>
                        <li class="liacordion">Cliente</li>
                        <li class="liacordion">Semaforo de proyectos</li>
                        <li class="liacordion">Retrasos</li>
                    </ul>
                </div>
                <h3 id="Proyecto4" runat="server">
                    Seguimiento de tarea</h3>
                <div id="DivProyecto4" runat="server">
                    <ul>
                        <li class="liacordion"><asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Actualizar Tarea</asp:LinkButton></li>
                    </ul>
                </div>
            </div>
        </div>
        <div id="Content" runat="server">
            <p align="center">
                <asp:Label ID="Label1" runat="server" Text="Gestor de " Font-Bold="True"></asp:Label>
                <asp:Label ID="LabelNav" runat="server" Font-Bold="True"></asp:Label>
            </p>            
            <div id="ContentTop" runat="server">
                <asp:MultiView ID="MultiView1Seg" runat="server">
                    <asp:View ID="View1Seg" runat="server">
                        <table id="Table2">
                            <tr>
                                <td>
                                    Nombre :
                                </td>
                                <td>
                                    <asp:TextBox MaxLength="30" ID="TextBoxNomUsuario" runat="server">    
                                    </asp:TextBox>
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
                                <td>
                                    Nombre : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox MaxLength="30" ID="TextBoxNomGrupo" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Descripcion : &nbsp;&nbsp;&nbsp;<asp:TextBox ID="TextBoxDescripcionGrupo" MaxLength="30"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="View3Seg" runat="server">
                        <table id="Table3">
                            <tr>
                                <td>
                                    Nombre : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox MaxLength="30" ID="TextBoxNomPerfil" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Descripcion : &nbsp;&nbsp;
                                    <asp:TextBox MaxLength="30" ID="TextBoxDescripcionPerfil" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Puede Registrar :
                                    <asp:RadioButtonList ID="RadioButtonListAltaPerfil" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Puede Eliminar :
                                    <asp:RadioButtonList ID="RadioButtonListEliminarPerfil" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Puede Modificar :
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
                                        <asp:Button ID="ButtonAgregarGU" OnClientClick="return false" runat="server" Text="Asignar"
                                            BackColor="#0066FF" ForeColor="White" />
                                        <asp:Button ID="ButtonEliminarGU" runat="server" OnClientClick="return false" Text="Desasignar"
                                            BackColor="#0066FF" ForeColor="White" />
                                    </div>
                                    <br />
                                    <asp:Button ID="ButtonGuardarGU" runat="server" OnClick="guardarusurioGrupo" Text="Guardar"
                                        BackColor="#0066FF" ForeColor="White" />
                                </td>
                                <td>
                                    <asp:ListBox ID="ListBoxGruposSeg" runat="server"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="View5Seg" runat="server">
                        <table id="Table4">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label3" runat="server" Text="Actualizacion de Usuario y Contraseña"></asp:Label>
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
                                    <asp:TextBox ID="TextBoxUsuarioUpdate" MaxLength="30" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contraseña : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <input id="PasswordUsUpdate" maxlength="12" type="password" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Confirmar Contraseña :
                                </td>
                                <td>
                                    <input id="PasswordConfirmUpdate" maxlength="12" type="password" runat="server" />
                                </td>
                            </tr>                            
                            <tr>
                                <td>
                                    <asp:Button ID="ButtonActualizaUsSeg" runat="server" Text="Actualizar" OnClick="ButtonActualizaUsuSeg_Click" />
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
                                    <asp:Button ID="ButtonEnviarEmailSeg" runat="server" Text="Enviar E-mail" OnClick="ButtonEnviarEmailSeg_Click" />
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
                                    <asp:Button ID="RestablecerPasswordEmail" OnClick="RestablecerPasswordEmail_Click"
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
                                    Descripcion :</td><td>
                                    <asp:TextBox ID="TextBoxDescSis" MaxLength="45" runat="server"></asp:TextBox>
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
                        <asp:Button ID="ButtonUpdateSistema" runat="server" Text="Actualizar"
                        OnClick="ButtonUpdateSistemaseg_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="ButtonCancelSistema"
                             runat="server" Text="Cancelar" OnClick="ButtonCancelSistemaseg_Click" />
                             &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="ButtonConsultaSistemasSeg"
                             runat="server" Text="Consultas" OnClick="ButtonConsultaSistemasSeg_Click" />
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
                             runat="server" Text="Registrar" OnClick="ButtonGuardarModuloSeg_Click"  />
                             <asp:Button ID="Button4"
                             runat="server" Text="Actualizar" OnClick="ButtonAtualizaModuloSeg_Click"  />
                             <asp:Button ID="ButtonEliminaModulo"
                             runat="server" Text="Eliminar" OnClick="ButtonEliminaModuloSeg_Click"  />
                             </div>
                             <div><asp:Label  ID="LabUpdateModulo"   runat="server"></asp:Label></div>                              
                    </asp:View>
                    <asp:View ID="ViewPnntalla" runat="server">                                                                  
<div id="tree" runat="server">
     <ul id="ulconttree" runat="server">
        <%--<li id="limodulotree" runat="server"><asp:CheckBox ID="CB1DivProyecto1" runat="server"  Text="Seguridad"/>
            <ul>
                <li><asp:CheckBox ID="CB1Usuarios" runat="server"  Text="Usuarios"/>
                    <ul>
                        <li><asp:CheckBox ID="CB1Usuarios2" runat="server"  Text="Usuarios"/></li>
                        <li><asp:CheckBox ID="CB1Cuenta" runat="server"  Text="Cuenta"/></li>
                    </ul>
                    </li>
                    <li><asp:CheckBox ID="CB1Grupos" runat="server"  Text="Grupos"/>
                    </li>
                    <li><asp:CheckBox ID="CB1Pefiles" runat="server"  Text="Perfiles"/>
                    </li>
                    <li><asp:CheckBox ID="CB1Relaciones" runat="server"  Text="Relaciones"/>
                    </li>
            </ul>            
            </li>
            <li><asp:CheckBox ID="CheckBox9" runat="server"  Text="Seguridad"/>
            <ul>
                <li><asp:CheckBox ID="CheckBox32" runat="server"  Text="Seguridad"/>
                    <ul>
                        <li><asp:CheckBox ID="CheckBox33" runat="server"  Text="Seguridad"/></li>
                    </ul>
                    </li>
            </ul>
            <ul>
                <li><asp:CheckBox ID="CheckBox34" runat="server"  Text="Seguridad"/>
                    <ul>
                        <li><asp:CheckBox ID="CheckBox35" runat="server"  Text="Seguridad"/></li>
                        <li><asp:CheckBox ID="CheckBox36" runat="server"  Text="Seguridad"/></li>
                        <li><asp:CheckBox ID="CheckBox37" runat="server"  Text="Seguridad"/>
                            <ul>
                                <li><asp:CheckBox ID="CheckBox38" runat="server"  Text="Seguridad"/></li>
                                <li><asp:CheckBox ID="CheckBox39" runat="server"  Text="Seguridad"/></li>
                            </ul>
                            </li>
                        <li><asp:CheckBox ID="CheckBox40" runat="server"  Text="Seguridad"/></li>
                        <li><asp:CheckBox ID="CheckBox41" runat="server"  Text="Seguridad"/></li>
                        <li><asp:CheckBox ID="CheckBox42" runat="server"  Text="Seguridad"/></li>
                    </ul>
                    </li>
            </ul>
            </li>--%>
            </ul>
</div>
 <div><asp:Button ID="ButtonregistraPantalla"
                             runat="server" Text="Registrar" OnClick="ButtonRegistrarPantallasSeg_Click"  />
                             <asp:Button ID="ButtonUpdatepantalla"
                             runat="server" Text="Actualizar" OnClick="ButtonAtualizaPantallasSeg_Click"  />
                             <asp:Button ID="ButtonDeletePantallaOpcion"
                             runat="server" Text="Eliminar" OnClick="ButtonDeletePantallaOpcion_Click"  />
                             </div>
                             <div><asp:Label  ID="LblupdatePantalla"   runat="server"></asp:Label></div>
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
 <div><asp:Button ID="Button5"
                             runat="server" Text="Asignar" OnClick="ButtonRegistrarRelAccesoSeg_Click"  />
                             <asp:Button ID="Button6"
                             runat="server" Text="Actualizar" OnClick="ButtonAtualizaRelAccesoSeg_Click"  />
                             </div>
                             <div><asp:Label  ID="LblStatusAccePerfil"   runat="server"></asp:Label></div>
                             </div>
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
 <div><asp:Button ID="ButtonAsignarModSis"
                             runat="server" Text="Asignar" OnClick="ButtonRegistrarRelAccesoSisModSeg_Click"  />
                             <asp:Button ID="ButtonDesasignarModSis"
                             runat="server" Text="Actualizar" OnClick="ButtonDeleteRelAccesoSisModSeg_Click"  />
                             </div>
                             <div><asp:Label  ID="Label6"   runat="server"></asp:Label></div>
                             </div>
                    </asp:View>
                </asp:MultiView>
                <asp:MultiView ID="MultiView2" runat="server">
                    <asp:View ID="View4" runat="server">
                        <table id="TableCont">
                            <tr>
                                <td>
                                    Clave : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="TextBoxClave"
                                        runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    Cliente :
                                    <asp:TextBox ID="TextBoxCliente" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    Fecha de Inicio : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="TextBoxFechaInicio"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Descripcion :<asp:TextBox ID="TextBoxDescripcion" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    Estado
                                </td>
                                <td>
                                    Fecha fin estimada :
                                    <asp:TextBox ID="TextBoxFechaFinEst" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nombre : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="TextBoxNombre" runat="server"></asp:TextBox>
                                </td>
                                <td><asp:Label ID="LabelDependencia" runat="server" Text=""></asp:Label>
                                    ID Dependencia :<asp:DropDownList ID="DropDownListDep" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Fecha fin real : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="TextBoxFechaFinReal" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <p><asp:Label ID="LabelHrs" runat="server" Text="Registrar horas : "></asp:Label>
                            <asp:DropDownList ID="DropDownListHoras" runat="server">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                            </asp:DropDownList>
                        </p>
                    </asp:View>
                </asp:MultiView>
                <div runat="server" id="opcionesRegSeg">
                    <asp:Button ID="Button4seg" validate="required:true" runat="server" Text="Insertar"
                        OnClick="Button4seg_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button5seg"
                            validate="required:false" runat="server" Text="Buscar" OnClick="Button5seg_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                                ID="Button6seg" runat="server" validate="required:false" Text="Mostrar todo"
                                OnClick="Button6seg_Click" />
                </div>
                <div runat="server" id="opcionesRegTarea" >
                    <asp:Button ID="Button1" runat="server" Text="Insertar" OnClick="Button1_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                        ID="Button2" runat="server" Text="Buscar" OnClick="Button2_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                            ID="Button3" runat="server" Text="Mostrar todo" OnClick="Button3_Click" />
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
            </div>
        </div>
        <div id="RightSideMenu">
            <p>
            </p>
        </div>
    </div>
    </form>
    
</body>
</html>
