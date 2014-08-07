<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CapturaTareas.aspx.cs" Inherits="ProyectosWeb.CapturaTareas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
   
  <link rel="stylesheet" href="//code.jquery.com/ui/1.10.4/themes/smoothness/jquery-ui.css">
   <link rel="stylesheet" href="Style/Style.css" />
  
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

  

    <script>
        $(function () {
            $("#TextBoxFechaInicio").datepicker({ dateFormat: "yy-mm-dd" });
            $("#TextBoxFechaFinEst").datepicker({ dateFormat: "yy-mm-dd" });
            $("#TextBoxFechaFinReal").datepicker({ dateFormat: "yy-mm-dd" });
        });
    </script>
</head>
<body id="Body1" runat="server">
<form id="BodyForm" runat="server">
<asp:HiddenField ID="hidAccordionIndex" runat="server" Value="0" />
  <script>
      
</script>

    <div id="header" align="center">
    <img src="Img/SolIcon.png" alt="Empresa" height="100px" width="20%" style="float:left;">  
    <h1 align="center">Sistema de captura de tareas</h1>
    </div>
<div id="Container" runat="server">

    <div id="LeftSideMenu">

<div id="accordion" runat="server">

    <h3 id="Proyecto1" runat="server" align="left">Tarea</h3>
      <div id="DivProyecto2"> 
  <ul>
  <li><asp:LinkButton ID="LinkProyecto" runat="server"   >Proyecto</asp:LinkButton></li>
  <li><asp:LinkButton ID="LinkRequerimiento" runat="server" >Requerimiento</asp:LinkButton></li>
  <li><asp:LinkButton ID="LinkCasosUso" runat="server" >Casos de uso</asp:LinkButton></li>
  <li><asp:LinkButton ID="LinkComponente" runat="server" >Componente</asp:LinkButton></li>
  <li><asp:LinkButton ID="LinkTarea" runat="server">Tarea</asp:LinkButton></li>
  </ul>
  </div>
  <h3 id="Proyecto2" runat="server">Seguridad</h3>
           <div id="DivProyecto1" runat="server">
        <p align="left">Catalogos</p>
        <ul>
        <li><a href="#">Usuarios</a></li>
        <li><a href="#">Grupos</a></li>
        <li><a href="#">Perfiles</a></li>
        <li><a href="#">Relaciones</a></li>
        <li><a href="#">Fases</a></li>
        <li><a href="#">Tipos de persona</a></li>
        <li><a href="#">Tecnologia</a></li>
        <li><a href="#">Parametros de sistema</a></li>
        <li><a href="#">Clientes</a></li>
        </ul>
        <p align="left">Control de acceso</p>
        <ul>
        <li>Proyecto</li>
        <li>Modulos</li>
        <li>Pantallas</li>
        <li>Opciones</li>
        </ul>
        </div>
  

  <h3 id="Proyecto3" runat="server">Consultas y reporte</h3>
  <div id="DivProyecto3" runat="server">
  <ul>
  <li>Horas: planeadas, reales</li>
  <li>Avance</li>
  <li>Fase</li>
  <li>Usuarios-Persona</li>
  <li>Cliente</li>
  <li>Semaforo de proyectos</li>
  <li>Retrasos</li>
  </ul>
  </div>
    <h3 id="Proyecto4" runat="server">Seguimiento de tarea</h3>
  <div id="DivProyecto4" runat="server">
  <ul>
  <li>Actualizar tarea</li>
  </ul>
  </div>
</div>


    </div>


    <div id ="Content" runat="server">   
   <h3 align="center">Captura de tareas</h3>
   <br />
   <h3>Cronometro de Tareas</h3>
<asp:scriptmanager ID="Scriptmanager1" runat="server"></asp:scriptmanager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
  <ContentTemplate>
    <asp:Label ID="Label1" runat="server" Font-Size="XX-Large" Text="00:00:00"></asp:Label><br />
    <asp:Timer ID="tm1" Interval="1000" runat="server" ontick="tm1_Tick" />
      <asp:Button ID="ButtonIniciar" runat="server" Text="Iniciar" 
          onclick="ButtonIniciar_Click" Height="50px" />
      <asp:Button ID="ButtonReset" runat="server" 
          Text="Resetear" onclick="ButtonReset_Click" Height="50px" />
      <asp:Button ID="ButtonEnviar" runat="server" Text="Registrar tiempo" Height="50px" 
          Width="110px" onclick="ButtonEnviar_Click" />
  </ContentTemplate>
  <Triggers>
    <asp:AsyncPostBackTrigger ControlID="tm1" EventName="Tick" />
  </Triggers>
</asp:UpdatePanel>
   <h4>Actualizar Tareas por archivo :</h4><br />
        <asp:FileUpload ID="FileUploadTareas" runat="server" /><br />
        <asp:Button ID="ButtonUpload" runat="server" Text="Subir archivo" 
            onclick="ButtonUpload_Click" /><asp:Label ID="LabelSubir" runat="server" 
            BackColor="Red"></asp:Label>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    </div>
    <div id="RightSideMenu">
    
    </div>
</div>
</form>
</body>
</html>
