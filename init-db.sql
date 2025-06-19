-- Script para inicializar o banco de dados no Docker
-- Este script será executado automaticamente quando o SQL Server estiver pronto

-- Aguardar o SQL Server estar pronto
WAITFOR DELAY '00:00:05'

-- Criar banco de dados se não existir
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CludeTesteTecnicoAPI')
BEGIN
    CREATE DATABASE CludeTesteTecnicoAPI
END
GO

USE CludeTesteTecnicoAPI
GO

-- Criar tabela Paciente se não existir
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Paciente]') AND type in (N'U'))
BEGIN
    CREATE TABLE Paciente (
        Id int PRIMARY KEY IDENTITY(1,1),
        Name nvarchar(255),
        Cpf nvarchar(255),
        BirthDate DATETIME,
        CreateDate DATETIME
    )
END
GO

-- Criar tabela ProfissionalSaude se não existir
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProfissionalSaude]') AND type in (N'U'))
BEGIN
    CREATE TABLE ProfissionalSaude (
        Id int PRIMARY KEY IDENTITY(1,1),
        Name nvarchar(255),
        Cpf nvarchar(255),
        CRM nvarchar(100),
        CreateDate DATETIME
    )
END
GO

-- Criar tabela Agendamento se não existir
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agendamento]') AND type in (N'U'))
BEGIN
    CREATE TABLE Agendamento (
        Id int PRIMARY KEY IDENTITY(1,1),
        PacienteId INT,
        ProfissionalSaudeId INT,
        CreateDate DATETIME,
        ScheduleDate DATETIME,
        TempoDuracaoAtendimentoMinutos INT,
        EmailEnviadoPeloServiceBus BIT DEFAULT 0,
        FOREIGN KEY (PacienteId) REFERENCES Paciente(Id),
        FOREIGN KEY (ProfissionalSaudeId) REFERENCES ProfissionalSaude(Id)
    )
END
GO

-- Criar tabela ApplicationMiddlewareLogError se não existir
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApplicationMiddlewareLogError]') AND type in (N'U'))
BEGIN
    CREATE TABLE ApplicationMiddlewareLogError (
        Id int PRIMARY KEY IDENTITY(1,1),
        CreateDate DATETIME,
        Method nvarchar(max),
        Exception nvarchar(max),
        Trace nvarchar(max),
        StatusCode int
    )
END
GO

-- Inserir dados de exemplo (opcional)
IF NOT EXISTS (SELECT * FROM Paciente)
BEGIN
    INSERT INTO Paciente (Name, Cpf, BirthDate, CreateDate) VALUES 
    ('João Silva', '123.456.789-00', '1990-01-15', GETDATE()),
    ('Maria Santos', '987.654.321-00', '1985-05-20', GETDATE())
END

IF NOT EXISTS (SELECT * FROM ProfissionalSaude)
BEGIN
    INSERT INTO ProfissionalSaude (Name, Cpf, CRM, CreateDate) VALUES 
    ('Dr. Carlos Oliveira', '111.222.333-44', 'CRM-12345', GETDATE()),
    ('Dra. Ana Costa', '555.666.777-88', 'CRM-67890', GETDATE())
END

PRINT 'Banco de dados inicializado com sucesso!' 