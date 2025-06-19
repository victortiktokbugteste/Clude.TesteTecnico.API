create database CludeTesteTecnicoAPI
use CludeTesteTecnicoAPI

CREATE TABLE Paciente (

Id int primary key identity(1,1),
Name nvarchar(255),
Cpf nvarchar(255),
BirthDate DATETIME,
CreateDate DATETIME

);

CREATE TABLE ProfissionalSaude (

Id int primary key identity(1,1),
Name nvarchar(255),
Cpf nvarchar(255),
CRM nvarchar(100),
CreateDate DATETIME

);



CREATE TABLE Agendamento (

Id int primary key identity(1,1),
PacienteId INT,
ProfissionalSaudeId INT,
CreateDate DATETIME,
ScheduleDate DATETIME,
TempoDuracaoAtendimentoMinutos INT,
EmailEnviadoPeloServiceBus BIT DEFAULT 0

FOREIGN KEY (PacienteId) REFERENCES Paciente(Id),
FOREIGN KEY (ProfissionalSaudeId) REFERENCES ProfissionalSaude(Id)

);


CREATE TABLE ApplicationMiddlewareLogError (

Id int primary key identity(1,1),
CreateDate DATETIME,
Method nvarchar(max),
Exception nvarchar(max),
Trace nvarchar(max),
StatusCode int

);