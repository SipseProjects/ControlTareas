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
                ValidChars: ".",
                FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
            });

        }); 
    </script>
    <script type="text/javascript" src="validacion/jquery.validate.js"></script>
    <script type="text/javascript">
        $(function () {

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
        });

        $(document).ready(function () {

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
                                    hid.val("1");
                                    sc.text("El email ya existe");
                                    sc.css("color", color1);
                                    break;
                                case "false":
                                    hid.val("");
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
            }

            $('#<%= ButtonEnviarEmailSeg.ClientID%>').on('click', function () {
                $("#BodyForm").validate({
                    ignore: "",
                    rules: {
                        'TextBoxEmailRegistrado': { email: true },
                        'HidValidEmailRestoreSeg': { required: true }
                    },
                    messages: {
                        'TextBoxEmailRegistrado': { email: 'Ingrese un correo valido. Ejemplo: cristian@hotmail.com' }
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
                    debug: true,
                    submitHandler: function (ButtonActualizaUsSeg) {
                        ButtonActualizaUsSeg.submit();

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
                            'HidValidEmailReg': { required: true, email: true, maxlength: 30 }
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
                            'TextAreaTecnologias': { maxlength: 'Maximo 30 caracteres' }
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

                        debug: true,
                        submitHandler: function (Button6seg) {
                            Button6seg.submit();
                        }
                    });
                }
            });

        });
   