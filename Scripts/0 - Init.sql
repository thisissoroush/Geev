USE master
GO

CREATE DATABASE Geev
GO

USE Geev
GO

CREATE TABLE WeatherType(
Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
Title NVARCHAR(30)
)
GO
CREATE TABLE WeatherStatus(
 Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
 TypeId INT NOT NULL REFERENCES WeatherType(Id),
 [Value] INT NOT NULL,
 CreatedOnUTC DATETIME2(7) NOT NULL DEFAULT GETDATE(),
)
GO
