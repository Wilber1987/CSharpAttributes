-- DROP SCHEMA administrative_access;

CREATE SCHEMA administrative_access;
-- administrative_access.Transactional_Configuraciones definition

-- Drop table

-- DROP TABLE administrative_access.Transactional_Configuraciones;

CREATE TABLE administrative_access.Transactional_Configuraciones (
	Id_Configuracion int IDENTITY(1,1) NOT NULL,
	Nombre nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Descripcion nvarchar(250) COLLATE Modern_Spanish_CI_AS NULL,
	Valor nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	Tipo_Configuracion varchar(100) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK_Transactional_Configuraciones PRIMARY KEY (Id_Configuracion),
	CONSTRAINT Transactional_Configuraciones_UN UNIQUE (Nombre)
);

INSERT INTO administrative_access.Transactional_Configuraciones
(Nombre, Descripcion, Valor, Tipo_Configuracion)
VALUES(N'TITULO', N'Encabezado de página', N'EMPRE S.A.', N'THEME');
INSERT INTO administrative_access.Transactional_Configuraciones
(Nombre, Descripcion, Valor, Tipo_Configuracion)
VALUES(N'SUB_TITULO', N'Subtitulo que se muestra en el encabezado', N'Prestamos, empeños y más', N'THEME');
INSERT INTO administrative_access.Transactional_Configuraciones
(Nombre, Descripcion, Valor, Tipo_Configuracion)
VALUES(N'NOMBRE_EMPRESA', N'nombre de la empresa', N'EMPRE S.A. test', N'THEME');
INSERT INTO administrative_access.Transactional_Configuraciones
(Nombre, Descripcion, Valor, Tipo_Configuracion)
VALUES(N'LOGO_PRINCIPAL', N'Logo que se muestra en los encabezados', N'logo.png', N'THEME');
INSERT INTO administrative_access.Transactional_Configuraciones
(Nombre, Descripcion, Valor, Tipo_Configuracion)
VALUES(N'MEDIA_IMG_PATH', N'Ruta de recursos', N'/media/img/', N'THEME');
INSERT INTO administrative_access.Transactional_Configuraciones
(Nombre, Descripcion, Valor, Tipo_Configuracion)
VALUES(N'VERSION', N'Versión de la aplicación', N'2024.07', N'NUMBER');