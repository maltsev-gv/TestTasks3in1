--
-- Скрипт сгенерирован Devart dbForge Studio 2019 for SQL Server, Версия 5.8.127.0
-- Домашняя страница продукта: http://www.devart.com/ru/dbforge/sql/studio
-- Дата скрипта: 07.11.2020 0:34:59
-- Версия сервера: 11.00.2100
--


SET DATEFORMAT ymd
SET ARITHABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET NUMERIC_ROUNDABORT, IMPLICIT_TRANSACTIONS, XACT_ABORT OFF
GO
USE master
GO

IF DB_NAME() <> N'master' SET NOEXEC ON

--
-- Создать базу данных [Turnstile]
--
PRINT (N'Создать базу данных [Turnstile]')
GO
CREATE DATABASE Turnstile
ON PRIMARY(
    NAME = N'Turnstile',
    FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Turnstile.mdf',
    SIZE = 10240KB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 1024KB
)
LOG ON(
    NAME = N'Turnstile_log',
    FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Turnstile_log.ldf',
    SIZE = 5120KB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 10%
)
GO

--
-- Изменить базу данных
--
PRINT (N'Изменить базу данных')
GO
ALTER DATABASE Turnstile
  SET
    ANSI_NULL_DEFAULT OFF,
    ANSI_NULLS OFF,
    ANSI_PADDING OFF,
    ANSI_WARNINGS OFF,
    ARITHABORT OFF,
    AUTO_CLOSE OFF,
    AUTO_CREATE_STATISTICS ON,
    AUTO_SHRINK OFF,
    AUTO_UPDATE_STATISTICS ON,
    AUTO_UPDATE_STATISTICS_ASYNC OFF,
    COMPATIBILITY_LEVEL = 110,
    CONCAT_NULL_YIELDS_NULL OFF,
    CURSOR_CLOSE_ON_COMMIT OFF,
    CURSOR_DEFAULT GLOBAL,
    DATE_CORRELATION_OPTIMIZATION OFF,
    DB_CHAINING OFF,
    HONOR_BROKER_PRIORITY OFF,
    MULTI_USER,
    NESTED_TRIGGERS = ON,
    NUMERIC_ROUNDABORT OFF,
    PAGE_VERIFY CHECKSUM,
    PARAMETERIZATION SIMPLE,
    QUOTED_IDENTIFIER OFF,
    READ_COMMITTED_SNAPSHOT OFF,
    RECOVERY FULL,
    RECURSIVE_TRIGGERS OFF,
    TRANSFORM_NOISE_WORDS = OFF,
    TRUSTWORTHY OFF
    WITH ROLLBACK IMMEDIATE
GO

ALTER DATABASE Turnstile
  SET DISABLE_BROKER
GO

ALTER DATABASE Turnstile
  SET ALLOW_SNAPSHOT_ISOLATION OFF
GO

ALTER DATABASE Turnstile
  SET FILESTREAM (NON_TRANSACTED_ACCESS = OFF)
GO

USE Turnstile
GO

IF DB_NAME() <> N'Turnstile' SET NOEXEC ON
GO

--
-- Создать таблицу [dbo].[User]
--
PRINT (N'Создать таблицу [dbo].[User]')
GO
CREATE TABLE dbo.[User] (
  UserId int IDENTITY,
  Name varchar(max) NULL,
  LastName varchar(max) NOT NULL,
  CONSTRAINT PK_User_UserId PRIMARY KEY CLUSTERED (UserId)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

--
-- Создать таблицу [dbo].[PassTime]
--
PRINT (N'Создать таблицу [dbo].[PassTime]')
GO
CREATE TABLE dbo.PassTime (
  PassTimeId int IDENTITY,
  UserId int NOT NULL,
  EnterTime datetime NULL,
  ExitTime datetime NULL,
  CONSTRAINT PK_PassTime_PassTimeId PRIMARY KEY CLUSTERED (PassTimeId)
)
ON [PRIMARY]
GO

--
-- Создать внешний ключ [FK_PassTime_User_UserId] для объекта типа таблица [dbo].[PassTime]
--
PRINT (N'Создать внешний ключ [FK_PassTime_User_UserId] для объекта типа таблица [dbo].[PassTime]')
GO
ALTER TABLE dbo.PassTime
  ADD CONSTRAINT FK_PassTime_User_UserId FOREIGN KEY (UserId) REFERENCES dbo.[User] (UserId)
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
