<%@ Page Title="" Language="C#" MasterPageFile="~/MasterLogin.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProyectosWeb.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<!-- media-queries.js -->
<!--[if lt IE 9]>
	<script src="http://css3-mediaqueries-js.googlecode.com/svn/trunk/css3-mediaqueries.js"></script>
<![endif]-->
<!-- html5.js -->
<!--[if lt IE 9]>
	<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
 
<script type="text/javascript" >
 $(document).ready(function () {
     $('#<%= ButtonIniciarSesion.ClientID%>').on('click', function () {
         $("#MasterLogin form1").validate({
             ignore: "",
             rules: {
                 'TextBoxUsuario': { required: true, minlength: 5 },
                 'PasswordUsuario': { required: true, maxlength: 12, minlength: 5 }
             },
             messages: {
                 'TextBoxUsuario': { required: 'Ingrese un nombre de usuario', minlength: 'El Nombre de Usuario debe ser minimo 5 caracteres' },
                 'PasswordUsuario': { required: 'Ingrese una contraseña', minlength: 'La contraseña debe ser maximo 12 caracteres', minlength: 'La contraseña debe ser minimo 5 caracteres' }
                 
             },
             errorPlacement: function (error, element) {
                 error.insertAfter(element)
                 error.addClass('message');  // add a class to the wrapper
                 error.css("color", "red");
             },
             debug: true,
             submitHandler: function (ButtonIniciasSesion) {
                 ButtonIniciasSesion.submit();

             }
         });
     });

     $('#<%= ButtonIniciarSesion.ClientID%>').keyup(function () {
         var us = $("#<%=TextBoxUsuario.ClientID%>");
         var p = $("#<%=PasswordUsuario.ClientID%>");
//         
                validarUs(us, us, p, p);

            });

            function validarUs(usuariot, hid, validaus, sc) {
                usuariot.css("border-color", "none");
                if (usuariot.val().length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "Main.aspx/verificarLogin",
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

        });
            </script>

<link href="/LoginSources/font/stylesheet.css" rel="stylesheet" type="text/css" />	
<link href="/LoginSources/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
<link href="/LoginSources/css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
<link href="/LoginSources/css/styles.css" rel="stylesheet" type="text/css" />
<link href="/LoginSources/css/media-queries.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="/LoginSources/fancybox/jquery.fancybox-1.3.4.css" media="screen" />

<meta name="viewport" content="width=device-width" />
 
<link rel="shortcut icon" href="/LoginSources/favicon.ico" type="image/x-icon">

<link href='http://fonts.googleapis.com/css?family=Exo:400,800' rel='stylesheet' type='text/css'>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

<!-- CHANGE THIS TITLE TAG -->


<body data-spy="scroll">

<!-- TOP MENU NAVIGATION -->
<div class="navbar navbar-fixed-top">
	<div class="navbar-inner">
		<div class="container">
	
			<a class="brand pull-left" href="#">
			Solutia Intelligence
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
			<img src="/Img/SolIcon.png">
		</div>
	
		<!-- APP DETAILS -->
		<div class="span7">
	
			<!-- ICON -->
			<div class="visible-desktop" id="icon">
				
			</div>
			
			<!-- APP NAME -->
			<div id="app-name">
				<h1>Control de Tareas</h1>
			</div>
			
			<!-- VERSION -->
			<div id="version">
				<span class="version-top label label-inverse">Version 1.0</span>
			</div>
            
			<!-- TAGLINE -->
			<div id="tagline">
				Administracion de Proyectos, Tareas y Tiempos.
			</div>
		
			<!-- PHONES IMAGE FOR TABLET MEDIA QUERY -->
			<div class="hidden-desktop" id="phones">
				<img src="img/phones.png">
			</div>
            
			<!-- DESCRIPTION -->
			<div id="description">
                
			<table id="Table4">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label3" runat="server" Text="Iniciar Sesion"></asp:Label>
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
                                    <asp:TextBox ID="TextBoxUsuario" MaxLength="30" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contraseña : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <input id="PasswordUsuario" maxlength="12" type="password" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Sistema :
                                </td>
                                <td>
                                    <input id="PasswordConfirmUpdate" maxlength="12" type="password" runat="server" />
                                </td>
                            </tr>                            
                            <tr>
                                <td>
                                   
                                    <asp:Button class="btn btn-primary" ID="ButtonIniciarSesion" runat="server" Text="Actualizar" OnClick="ButtonIniciarSesion_Click" />
                                
                                    
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
                       
    </div>
            
			<!-- FEATURES -->
			<ul id="features">
				<li>Fully Responsive HTML/CSS3 Template</li>
				<li>Built on Bootstrap by Twitter</li>
				<li>Images and Photoshop Files Included</li>
				<li>Completely Free!</li>

			</ul>
		
			<!-- DOWNLOAD & REQUIREMENT BOX -->
			<div class="download-box">
				<a href="#"><img src="img/available-on-the-app-store.png"></a>
			</div>
			<div class="download-box">
				<a href="#"><img src="img/android_app_on_play_logo_large.png"></a>
			</div>
			<div class="download-box">
				<strong>Requirements:</strong><br>
				Compatible with iPhone and iPod touch. Requires iPhone OS 2.2 or later. WiFi, Edge, or 3G network connection sometimes required.
			</div>
			<div class="download-box">
				<strong>Requirements:</strong><br>
				Requires Android 2.3 and higher. WiFi, Edge, or 3G network connection sometimes required.
			</div>
			
		</div>
	</div>
	
	
	
	<!-- ABOUT & UPDATES -->
	<div class="row-fluid" id="about">
	
		<div class="span6">
			<h2 class="page-title" id="scroll_up">
				About
				<a href="#home" class="arrow-top">
				<img src="img/arrow-top.png">
				</a>
			</h2>			
			
		</div>
	
		
	
	</div>			
	
	
</div>


<!-- FOOTER -->
<div class="footer container container-fluid">

	<!-- COPYRIGHT - EDIT HOWEVER YOU WANT! -->
	<div id="copyright">
		Copyright &copy; 2014 Carp.io<br>
		Licensed under <a rel="license" href="http://creativecommons.org/licenses/by/3.0/">CC BY 3.0</a>. Built on <a href="http://twitter.github.com/bootstrap/">Bootstrap</a>.
	</div>
	
	<!-- CREDIT - PLEASE LEAVE THIS LINK! -->
	<div id="credits">
		<a href="http://github.differential.io/flexapp">Theme</a> by <a href="http://carp.io">Carp</a>.
	</div>

</div>

<script src="http://code.jquery.com/jquery-1.7.2.min.js"></script>
<script src="/LoginSources/js/bootstrap.min.js"></script>
<script src="/LoginSources/js/bootstrap-collapse.js"></script>
<script src="/LoginSources/js/bootstrap-scrollspy.js"></script>
<script src="/LoginSources/fancybox/jquery.mousewheel-3.0.4.pack.js"></script>
<script src="/LoginSources/fancybox/jquery.fancybox-1.3.4.pack.js"></script>
<script src="/LoginSources/js/init.js"></script>
<script type="text/javascript" src="/validacion/jquery.validate.js"></script>
</body>
</asp:Content>
