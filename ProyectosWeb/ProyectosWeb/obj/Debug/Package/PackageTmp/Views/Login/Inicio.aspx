<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="ProyectosWeb.Views.Login.Inicio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <!-- media-queries.js -->
<!--[if lt IE 9]>
	<script src="http://css3-mediaqueries-js.googlecode.com/svn/trunk/css3-mediaqueries.js"></script>    
<![endif]-->
<!-- html5.js -->
<!--[if lt IE 9]>
	<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
<link href="../../LoginSources/font/stylesheet.css" rel="stylesheet" type="text/css" />	
<link href="../../LoginSources/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
<link href="../../LoginSources/css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
<link href="../../LoginSources/css/styles.css" rel="stylesheet" type="text/css" />
<link href="../../LoginSources/css/media-queries.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="../../LoginSources/fancybox/jquery.fancybox-1.3.4.css" media="screen" />

<meta name="viewport" content="width=device-width" />
 
<link rel="shortcut icon" href="../../LoginSources/favicon.ico" type="image/x-icon">

<link href='http://fonts.googleapis.com/css?family=Exo:400,800' rel='stylesheet' type='text/css'>

</head>
 <body data-spy="scroll">
    <form id="form1" runat="server">
     <asp:HiddenField ID="HidValidEmailRestoreSeg" runat="server" Value="" />
     <asp:HiddenField ID="hidClicks" runat="server"  Value="0" />
      <asp:HiddenField ID="Hidsistemaval" runat="server"  Value="0" />

<!-- TOP MENU NAVIGATION -->
<div class="navbar navbar-fixed-top">
	<div class="navbar-inner">
		<div class="container">
	
			<a class="brand pull-left" href="#">
			SICT-App
			</a>
	
			<a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">
				<span class="icon-bar"></span>
				<span class="icon-bar"></span>
				<span class="icon-bar"></span>
			</a>
		
			<div class="nav-collapse collapse">
				<ul id="nav-list" class="nav pull-right">
					<li><a href="#home">Inicio</a></li>
					<li><a href="#about">Acerca de</a></li>
				</ul>
			</div>
		
		</div>
	</div>
</div>


<!-- MAIN CONTENT -->
<div class="container content container-fluid" id="home">



	<!-- HOME -->
	<div class="row-fluid">
  
		<!-- PHONES IMAGE FOR DESKTOP MEDIA QUERY -->
		<div class="span5 visible-desktop">
			<img src="../../Img/SolIcon.png">
		</div>
	
		<!-- APP DETAILS -->
		<div class="span7">
	
			<!-- ICON -->			
			
			<!-- APP NAME -->
			<div id="app-name">
				<h2>Control de Tareas</h2>
			</div>
			
			<!-- VERSION -->
            
			<!-- TAGLINE -->
			<div id="tagline">
				<asp:Label ID="LabelNav" runat="server"></asp:Label>
			</div>
			<!-- DESCRIPTION -->
			<div id="description">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                     <table id="Table2"><tr> <td>
			<table id="Table4">
            
                            <tr><td>
                                    Usuario
                                </td>
                                </tr>
                                <tr>                                
                                <td>
                                     <asp:LinkButton ID="LinkRestorePassUsu" OnClick="linkRestorePassUss" runat="server" >Olvidaste tu Nombre de Usuario?</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>                                
                                <td>
                                    <asp:TextBox ID="TextBoxUsuario" MaxLength="30" runat="server" TabIndex="1"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr><td>
                                    Contraseña
                                </td></tr>
                                <tr>                                
                                <td>
                                     <asp:LinkButton ID="LinkRestorePass" OnClick="linkRestorePassUss" runat="server" >Olvidaste tu Contraseña?</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input id="PasswordUsuario" maxlength="12" type="password" runat="server" tabindex="2" />
                                </td>
                            </tr>
                            <tr>
                            <td>
                                    Sistema 
                                </td>
                                </tr>
                            <tr>                                
                                <td>
                                    <asp:DropDownList ID="DropDownList1" AutoPostBack="false" runat="server" 
                                        TabIndex="4">
                                    </asp:DropDownList>
                                </td>
                            </tr>                            
                            <tr>
                                <td>
                                   
                                    <asp:Button class="btn btn-primary" ID="ButtonIniciarSesion" runat="server" 
                                        Text="Iniciar Sesion" OnClick="ButtonIniciarSesion_Click" TabIndex="3" />
                                
                                    
                                </td>
                            </tr>
                        </table>
                        
                        <table>
                        <tr>
                                <td>
                                    <asp:Label ID="msgusu" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="msgpass" runat="server" Font-Bold="True"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblcontrolarAcceso" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        </td><td>
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
                         <table>
                        <tr>
                             <td><asp:LinkButton ID="LinkButton1" OnClick="linkIniciaSesion" runat="server" >Iniciar Sesión</asp:LinkButton></td>                         
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
                                    Contraseña  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>                               
                            </tr>
                             <tr>
                              <td>
                                    <input id="PasswordUsRestore" maxlength="12" type="password" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Confirmar Contraseña 
                                </td>                               
                            </tr>
                             <tr>
                             <td>
                                    <input id="PasswordUsRestoreConfirm" maxlength="12" type="password" runat="server" />
                                </td> 
                                 </tr>                                                                                                              
                            <tr>
                                <td>
                                    <asp:Button ID="RestablecerPasswordEmail" OnClick="RestablecerPasswordEmail_Click"
                                        runat="server" Text="Restablecer" class="btn btn-primary" />
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
                                    <asp:Label ID="Label8" runat="server" Text="El link ha expirado :D, inicie de nuevo con el proceso de recuperación de datos. " Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                              <tr>
                            <td>
                                   <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Views/Login/Inicio.aspx" runat="server">Iniciar Sesion</asp:HyperLink>
                                </td>
                                </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>                                  
    </div>            
		
			<!-- ABOUT & UPDATES -->
	<div class="row-fluid" id="about">
	
		<div class="span6">
			<h2 class="page-title" id="scroll_up">
				Acerca de
				<a href="#home" class="arrow-top">
				<img src="../../LoginSources/img/arrow-top.png">
				</a>
			</h2>			
			
		</div>
	
		
	
	</div>
			
		</div>
	</div>									
</div>


<!-- FOOTER -->
<div class="footer container container-fluid">	
	
	<!-- CREDIT - PLEASE LEAVE THIS LINK! -->
	<div id="credits">
		<a href="http://github.differential.io/flexapp">Theme</a> by <a href="http://carp.io">Carp</a>.
	</div>

</div>

<script src="http://code.jquery.com/jquery-1.7.2.min.js"></script>
<script src="../../LoginSources/js/bootstrap.min.js"></script>
<script src="../../LoginSources/js/bootstrap-collapse.js"></script>
<script src="../../LoginSources/js/bootstrap-scrollspy.js"></script>
<script src="../../LoginSources/fancybox/jquery.mousewheel-3.0.4.pack.js"></script>
<script src="../../LoginSources/fancybox/jquery.fancybox-1.3.4.pack.js"></script>
<script src="../../LoginSources/js/init.js"></script>
 <script type="text/javascript" src="http://ajax.microsoft.com/ajax/beta/0911/Start.debug.js"></script>
    <script src="http://ajax.microsoft.com/ajax/beta/0911/extended/ExtendedControls.debug.js"
        type="text/javascript"></script>
 <script type="text/javascript" src="../../validacion/jquery.validate.js"></script>
 
<script type="text/javascript" >
    $(document).ready(function () {

        
        $('#<%= ButtonIniciarSesion.ClientID%>').on('click', function () {
            var valor1 = $('#<%=TextBoxUsuario.ClientID%>');

            if (valor1.val() == "admin") {
                $('#<%= Hidsistemaval.ClientID%>').val('0');
                $('#DropDownList1 option:first-child').attr("value", "0");
            }
            $("#form1").validate({
                ignore: "",
                rules: {
                    'TextBoxUsuario': { required: true, minlength: 5 },
                    'PasswordUsuario': { required: true, maxlength: 12, minlength: 5 },
                    'DropDownList1': { required: true }
                },
                messages: {
                    'TextBoxUsuario': { required: '', minlength: 'El Nombre de Usuario debe ser minimo 5 caracteres' },
                    'PasswordUsuario': { required: '', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'La contraseña debe ser minimo 5 caracteres' },
                    'DropDownList1': { required: '' }
                },
                errorPlacement: function (error, element) {
                    element.css('border-color', 'red');
                    error.insertAfter(element);
                    error.addClass('message');  // add a class to the wrapper
                    error.css("color", "red");
                    error.css("display", "block");

                },
                debug: true,
                submitHandler: function (ButtonIniciarSesion) {
                    ButtonIniciarSesion.submit();
                }
            });
        });

        $('#<%= TextBoxUsuario.ClientID%>').keyup(function () {
            var u = $('#<%=TextBoxUsuario.ClientID%>');
            CambiarBorder(u);
        });

        $('#<%= PasswordUsuario.ClientID%>').keyup(function () {
            var p = $('#<%=PasswordUsuario.ClientID%>');
            CambiarBorder(p);
        });

        function CambiarBorder(p) {
            if (p.val().length < 1) {
                p.css("border-color", "red");
            } else {
                p.css("border-color", "blue");
            }
        }

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

        $('#<%= TextBoxEmailRegistrado.ClientID%>').keyup(function () {
            var usuariot = $("#<%=TextBoxEmailRegistrado.ClientID%>");
            var sc = $("#<%=LabEmailReg.ClientID%>");
            var hid = $('#<%=HidValidEmailRestoreSeg.ClientID %>');
            validarAjax(usuariot, hid, sc, "CheckEmail", "Email válido", "Email no válido", "green", "red");
            var p = $("#<%=TextBoxEmailRegistrado.ClientID%>");
            CambiarBorder(p);
        });


        function validarAjax(usuariot, hid, sc, metodo, msgexiste, msgNoexiste, color1, color2) {
            if (usuariot.val().length > 0) {

                $.ajax({
                    type: "POST",
                    url: "Inicio.aspx/CheckEmail",
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

        $('#<%= DropDownList1.ClientID%>').change(function () {
            var option = $('[id*=DropDownList1] option:selected');
            var valor = $('#<%=DropDownList1.ClientID%>');
            var valorus = $('#<%=TextBoxUsuario.ClientID%>');
            var sis;
            if (option.val().toString() !== "") {
                valor.css("border-color", "blue");
                sis = option.val().toString();
            } else {
                if (valorus.val() !== "admin") {
                    valor.css("border-color", "red");
                } else {
                    sis = "0";
                }
            }

            $('#<%= Hidsistemaval.ClientID%>').val(sis);

        });


        $('#<%= RestablecerPasswordEmail.ClientID%>').on('click', function () {

            $("#form1").validate({
                ignore: "",
                rules: {
                    'PasswordUsRestore': { required: true, maxlength: 12, minlength: 5 },
                    'PasswordUsRestoreConfirm': { required: true, maxlength: 12, minlength: 5, equalTo: '#PasswordUsRestore' }
                },
                messages: {
                    'PasswordUsRestore': { required: 'Ingrese una contraseña', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'Mínimo 5 caracteres' },
                    'PasswordUsRestoreConfirm': { required: 'La contraseña no coincide', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'Mínimo 5 caracteres', equalTo: 'La contraseña no coincide' }
                },
                errorPlacement: function (error, element) {
                    error.insertAfter(element)
                    error.addClass('message');  // add a class to the wrapper
                    error.css("color", "red");
                    error.css("display", "block");
                },
                debug: true,
                submitHandler: function (RestablecerPasswordEmail) {
                    RestablecerPasswordEmail.submit();

                }
            });
        });

        $('#<%= ButtonEnviarEmailSeg.ClientID%>').on('click', function () {

            $("#form1").validate({
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

        $(function () {
            var valor = $('#<%=DropDownList1.ClientID%>');
            var opciones = $('#DropDownList1 > option').length;
            
            var valorus = $('#<%=TextBoxUsuario.ClientID%>');                       

            if (valorus.val() == "admin") {
                $('#<%= Hidsistemaval.ClientID%>').val('0');
                $('#DropDownList1 option:first-child').attr("value", "0");
            } else if (opciones > 1) {
                $('#DropDownList1').prop('selectedIndex', 1);
                var option = $('[id*=DropDownList1] option:selected');
                $('#<%= Hidsistemaval.ClientID%>').val(option.val());
            }
        });

    });
            </script>
                <script type="text/javascript">
                    Sys.debug = true;
                    Sys.require(Sys.components.filteredTextBox, function () {                        
                        $("#TextBoxUsuario").filteredTextBox({
                            InValidChars: " ",
                            ValidChars: ".",
                            FilterType: Sys.Extended.UI.FilterTypes.Custom | Sys.Extended.UI.FilterTypes.UppercaseLetters | Sys.Extended.UI.FilterTypes.LowercaseLetters
                | Sys.Extended.UI.FilterTypes.Numbers
                        });
                        $("#PasswordUsuario").filteredTextBox({
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

                    }); 
    </script>
    </form>
</body>
</html>
