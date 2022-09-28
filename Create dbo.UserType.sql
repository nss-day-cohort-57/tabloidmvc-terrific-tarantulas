USE [TabloidMVC]
GO

/****** Object: Table [dbo].[UserType] Script Date: 9/26/2022 11:04:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserType] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (20) NOT NULL
);

INSERT INTO UserType (Name) values ( 'Author')

SELECT *  FRom UserType