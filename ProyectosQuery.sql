create database ProyectosGestion
use ProyectosGestion
drop database ProyectosGestion
use PruebasD

/*Inicio de Seguridad*/
create table CatalogoPerfiles(
IDCatalogoPerfiles int primary key identity(1,1),
Nombre varchar(30),
Descripcion varchar(30),
Privilegios varchar(30)
)

create table CatalogoProveedores(
IDCatalogoProveedores int primary key identity(1,1),
Nombre varchar(30),
Direccion varchar(30),
Contacto varchar(30),
Telefono varchar(30)

)

create table Usuarios(
IDUsuario int primary key identity(1,1),
IDCatalogoProveedores int foreign key references CatalogoProveedores(IDCatalogoProveedores),
Nombre varchar(30) not null,
esEmpleado char(1) not null,
Contraseña varchar(120) not null,
Estado char(1)not null default 0
)

create table Grupos(
IDGrupos int primary key identity(1,1),
IDusuario int foreign key references Usuarios(IDUsuario),
Nombre varchar(30) not null,
Descripcion varchar(30) not null,
Estado varchar(30) not null default 0
)

create table Perfiles(
IDPerfil int primary key identity(1,1),
IDGrupos int foreign key references Grupos(IDGrupos),
IDCatalogoPerfiles int foreign key references CatalogoPerfiles(IDCatalogoPerfiles),
Nombre varchar(30) not null,
Descripcion varchar(30) not null,
UsuarioAlta char(1) not null default 'N',
UsuarioBaja char(1) not null default 'N',
Usuari0Modifica char(1) not null default 'N'
)


/*Fin de seguridad*/
/*Inicio de Proyectos*/
create table Proyectos(
IDProyectos int primary key identity(1,1),
ClaveProyectos varchar(30),
Nombre varchar(30),
Descripcion varchar(30),
Cliente varchar(30),
FechaRegistro date,
FechaInicio date,
FechaFinEstimada date,
FechaFinReal date,
Tecnologias varchar(30),
Estado char(1) not null default 0
)

create table Requerimientos(
IDRequerimientos int primary key identity(1,1),
IDProyecto int foreign key references Proyectos(IDProyectos),
ClaveRequerimientos varchar(30) not null,
Nombre varchar(30),
Descripcion varchar(30),
Cliente varchar(30),
FechaRegistro date,
FechaInicio date,
FechaFinEstimada date,
FechaFinReal date,
Tecnologias varchar(30),
Estado char(1) not null default 0
)

create table CasosUso(
IDCasosUso int primary key identity(1,1),
IDRequerimientos int foreign key references Requerimientos(IDRequerimientos),
ClaveCasosUso varchar(30),
Nombre varchar(30),
Descripcion varchar(30),
Cliente varchar(30),
FechaRegistro date,
FechaInicio date,
FechaFinEstimada date,
FechaFinReal date,
Tecnologias varchar(30),
Estado char(1) not null default 0
)

create table Componentes(
IDComponentes int primary key identity(1,1),
IDCasosUso int foreign key references CasosUso(IDCasosUso),
ClaveComponentes varchar(30),
Nombre varchar(30),
Descripcion varchar(30),
Cliente varchar(30),
FechaRegistro date,
FechaInicio date,
FechaFinEstimada date,
FechaFinReal date,
Tecnologias varchar(30),
Estado char(1) not null default 0
)

create table Tareas(
IDTareas int primary key identity(1,1),
IDComponentes int foreign key references Componentes(IDComponentes),
ClaveTareas varchar(30),
Nombre varchar(30),
Descripcion varchar(30),
Cliente varchar(30),
FechaRegistro date,
FechaInicio date,
FechaFinEstimada date,
FechaFinReal date,
HorasEstimadas time,
HorasReales time,
Tecnologias varchar(30),
Estado char(1) not null default 0
)

create table Fases(
IDFases int primary key identity(1,1),
IDTareas int foreign key references Tareas(IDTareas),
ClaveFases varchar(30),
Nombre varchar(30),
Descripcion varchar(30),
Cliente varchar(30),
FechaRegistro date,
FechaInicio date,
FechaFinEstimada date,
FechaFinReal date,
HorasEstimadas time,
HorasReales time,
Tecnologias varchar(30),
Estado char(1) not null default 0
)
/*Fin de Proyectos*/
/*Inicio de integracion de usuario a seguridad*/



Create table Personas(
IDPersonas int primary key identity(1,1),
IDUsuario int foreign key references Usuarios(IDUsuario),
Nombre varchar(30),
Apellido varchar(30),
FechaRegistro date,
Tecnologias varchar(30),
Estado char(1) not null default 0,
Email varchar (30) NULL,
Telefono bigint NULL,
Empresa varchar(30) NULL
)


Create table Equipos (
IDEquipos int primary key identity(1,1),
IDPersonas int foreign key references Personas(IDPersonas),
IDProyectos int foreign key references Proyectos(IDProyectos),
Nombre varchar(30),
Descripcion varchar(30),
Cliente varchar(30),
Estado char(1) not null default 0
)

create table gruposUsuarios(
IDGrupoUsuario int primary key identity(1,1),
IDUsuario int foreign key references Usuarios(IDUsuario),
IDGrupo int foreign key references Grupos(IDGrupos)
)

create table PerfilesUsuarios(
IDPerfilUsuario int primary key identity(1,1),
IDUsuario int foreign key references Usuarios(IDUsuario),
IDPerfil int foreign key references Perfiles(IDPerfil)
)

create table Sistema(
IDSistema int primary key identity(1,1),
Nombre varchar(40),
Descripcion varchar(40),
)

create table Sistemas(
IDSistemas int primary key identity(1,1),
ClaveSistemas varchar(30),
Nombre varchar(30),
Descripcion varchar(30),
Cliente varchar(30),
FechaRegistro date,
FechaInicio date,
FechaFinEstimada date,
FechaFinReal date,
Tecnologias varchar(30),
Estado char(1) not null default 0
)

create table Modulos(
IDModulo int primary key identity(1,1),
Nombre varchar(55),
Descripcion varchar(50),
h3id varchar(55) not null,
divid varchar(55) not null
)

create table Pantallas(
IDPantalla int primary key identity(1,1),
IDModulo int foreign key references Modulos(IDModulo),
Nombre varchar(60),
Descripcion varchar(60),
idasp varchar(70) not null,
Estado tinyint not null default 0
)

create table Opciones(
IDOpcion int primary key identity(1,1),
IDPantalla int foreign key references Pantallas(IDPantalla),
Nombre varchar(40),
Descripcion varchar(40),
idasp varchar(50) not null,
componenteIndex varchar(3) null,
chkboxTreeindex tinyint null,
estado tinyint not null default 0
)




create table perfilesModulos(
IDPerfilModulo int primary key identity(1,1),
IDModulo int foreign key references Modulos(IDModulo),
IDPerfil int foreign key references Perfiles(IDPerfil),
h3visible varchar(5) not null,
divvisible varchar(5) not null
)

create table perfilesPantallas(
IDPerfilPantalla int primary key identity(1,1),
IDPantalla int foreign key references Pantallas(IDPantalla),
IDPerfil int foreign key references Perfiles(IDPerfil),
visible varchar(5) not null,
componenteIndex varchar(4) null
)

create table perfilesOpciones(
IDPerfilOpcion int primary key identity(1,1),
IDOpcion int foreign key references Opciones(IDOpcion),
IDPerfil int foreign key references Perfiles(IDPerfil),
visible varchar(5) not null
)

create table usuariosModulos(
IDUsuarioModulo int primary key identity(1,1),
IDModulo int foreign key references Modulos(IDModulo),
IDUsuario int foreign key references Usuarios(IDUsuario),
h3visible varchar(5) not null,
divvisible varchar(5) not null
)

create table usuariosPantallas(
IDUsuarioPantalla int primary key identity(1,1),
IDPantalla int foreign key references Pantallas(IDPantalla),
IDUsuario int foreign key references Usuarios(IDUsuario),
visible varchar(5) not null,
componenteIndex varchar(4) null
)

create table usuariosOpciones(
IDUsuarioOpcion int primary key identity(1,1),
IDOpcion int foreign key references Opciones(IDOpcion),
IDUsuario int foreign key references Usuarios(IDUsuario),
visible varchar(5) not null
)

create table sistemasModulos(
IDSistemaModulo int primary key identity(1,1),
IDModulo int foreign key references Modulos(IDModulo),
IDSistema int foreign key references Sistemas(IDSistemas),
h3visible varchar(5) not null,
divvisible varchar(5) not null
)