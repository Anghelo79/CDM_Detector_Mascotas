USE [C:\USERS\OPALI\DOWNLOADS\PROYECTOCDMORIGINALV2\PROYECTOCDM\PROYECTOCDM\DATABASE\BDD.MDF]
GO

/****** Objeto: Table [dbo].[Codigo] Fecha del script: 12/07/2021 20:35:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Mascota];
DROP TABLE [dbo].[Codigo];
DROP TABLE [dbo].[Habitacion];
DROP TABLE [dbo].[lista_De_Accesos];

GO


CREATE TABLE [dbo].[Mascota] (
    [IdMascota]     INT        IDENTITY (1, 1) NOT NULL,
    [NombreMascota] NCHAR (15) NOT NULL,
    [Idcodigo]      INT        NULL
);
CREATE TABLE [dbo].[lista_De_Accesos] (
    [IdListaAcc]   INT IDENTITY (1, 1) NOT NULL,
    [IdMascota]    INT NOT NULL,
    [IdHabitacion] INT NOT NULL
);
CREATE TABLE [dbo].[Codigo] (
    [IdCodigo] INT        IDENTITY (1, 1) NOT NULL,
    [Codigo]   NCHAR (20) NOT NULL
);

CREATE TABLE [dbo].[Habitacion] (
    [IdHabitacion]     INT        IDENTITY (1, 1) NOT NULL,
    [NombreHabitacion] NCHAR (20) NOT NULL
);




