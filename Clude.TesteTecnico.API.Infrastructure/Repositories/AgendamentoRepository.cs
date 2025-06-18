using Clude.TesteTecnico.API.Domain.Entities;
using Clude.TesteTecnico.API.Domain.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Infrastructure.Repositories
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        private readonly string _connectionString;

        // Classes para mapeamento do Dapper
        private class PacienteMap
        {
            public int Paciente_Id { get; set; }
            public string Paciente_Name { get; set; }
            public string Paciente_Cpf { get; set; }
        }

        private class ProfissionalSaudeMap
        {
            public int Profissional_Id { get; set; }
            public string Profissional_Name { get; set; }
            public string Profissional_Cpf { get; set; }
            public string Profissional_CRM { get; set; }
        }

        public AgendamentoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Agendamento> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT agendamento.*, 
                       paciente.Id as Paciente_Id, 
                       paciente.Name as Paciente_Name,
                       paciente.Cpf as Paciente_Cpf,
                       profissionalSaude.Id as Profissional_Id,
                       profissionalSaude.Name as Profissional_Name,
                       profissionalSaude.CRM as Profissional_CRM
                FROM Agendamento agendamento
                LEFT JOIN Paciente paciente ON agendamento.PacienteId = paciente.Id
                LEFT JOIN ProfissionalSaude profissionalSaude ON agendamento.ProfissionalSaudeId = profissionalSaude.Id
                WHERE agendamento.Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            var agendamentoMap = await connection.QueryAsync<Agendamento, PacienteMap, ProfissionalSaudeMap, Agendamento>(
                sql,
                (agendamento, paciente, profissional) =>
                {
                    agendamento.Paciente = new Paciente
                    {
                        Name = paciente.Paciente_Name,
                        Cpf = paciente.Paciente_Cpf,
                        Id = paciente.Paciente_Id
                    };
                    agendamento.ProfissionalSaude = new ProfissionalSaude
                    {
                        Name = profissional.Profissional_Name,
                        Cpf = profissional.Profissional_Cpf,
                        CRM = profissional.Profissional_CRM
                    };
                    return agendamento;
                },
                new { Id = id },
                splitOn: "Paciente_Id,Profissional_Id"
            );

            return agendamentoMap.FirstOrDefault();
        }

        public async Task<List<Agendamento>> GetAllAsync()
        {
            var sql = @"
                SELECT agendamento.*, 
                       paciente.Id as Paciente_Id, 
                       paciente.Name as Paciente_Name,
                       paciente.Cpf as Paciente_Cpf,
                       profissionalSaude.Id as Profissional_Id,
                       profissionalSaude.Name as Profissional_Name,
                       profissionalSaude.CRM as Profissional_CRM
                FROM Agendamento agendamento
                LEFT JOIN Paciente paciente ON agendamento.PacienteId = paciente.Id
                LEFT JOIN ProfissionalSaude profissionalSaude ON agendamento.ProfissionalSaudeId = profissionalSaude.Id ";

            using var connection = new SqlConnection(_connectionString);
            var agendamentosMap = await connection.QueryAsync<Agendamento, PacienteMap, ProfissionalSaudeMap, Agendamento>(
                sql,
                (agendamento, paciente, profissional) =>
                {
                    agendamento.Paciente = new Paciente
                    {
                        Name = paciente.Paciente_Name,
                        Cpf = paciente.Paciente_Cpf,
                        Id = paciente.Paciente_Id
                    };
                    agendamento.ProfissionalSaude = new ProfissionalSaude
                    {
                        Name = profissional.Profissional_Name,
                        Cpf = profissional.Profissional_Cpf,
                        CRM = profissional.Profissional_CRM
                    };
                    return agendamento;
                },
                splitOn: "Paciente_Id,Profissional_Id"
            );

            return agendamentosMap.ToList();
        }

        public async Task<Agendamento> AddAsync(Agendamento entity)
        {
            entity.CreateDate = DateTime.UtcNow;

            using var db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Agendamento (PacienteId, ProfissionalSaudeId, CreateDate, ScheduleDate, TempoDuracaoAtendimentoMinutos) 
                       VALUES (@PacienteId, @ProfissionalSaudeId, @CreateDate, @ScheduleDate, @TempoDuracaoAtendimentoMinutos);
                       SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = await db.QuerySingleAsync<int>(sql, entity);
            entity.Id = id;
            return entity;
        }

        public async Task<Agendamento> UpdateAsync(Agendamento entity)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = @"
                UPDATE Agendamento 
                SET ScheduleDate = @ScheduleDate
                WHERE Id = @Id";

            await db.ExecuteAsync(sql, entity);
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Agendamento WHERE Id = @Id";
            await db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM Agendamento WHERE Id = @Id";
            return await db.ExecuteScalarAsync<bool>(sql, new { Id = id });
        }

        public async Task<bool> ExistsByPacienteAndProfissionalPerDayAsync(int pacienteId, int profissionalId, DateTime scheduleDate, int? id = 0)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = @"SELECT COUNT(1) FROM Agendamento 
                       WHERE PacienteId = @PacienteId 
                       AND ProfissionalSaudeId = @ProfissionalSaudeId 
                       AND CAST(ScheduleDate AS DATE) = CAST(@scheduleDate AS DATE) ";

            if (id > 0)
                sql += " AND Id <> @id ";

            return await db.ExecuteScalarAsync<bool>(sql, new { PacienteId = pacienteId, ProfissionalSaudeId = profissionalId, scheduleDate = scheduleDate, Id = id });
        }

        public async Task<List<Agendamento>> GetAgendamentosByProfissionalAndDateAsync(int profissionalId, DateTime scheduleDate)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Agendamento WHERE ProfissionalSaudeId = @ProfissionalSaudeId AND CONVERT(date, ScheduleDate) = CONVERT(date, @scheduleDate) ";
            var agendamentos = await db.QueryAsync<Agendamento>(sql, new { ProfissionalSaudeId = profissionalId, scheduleDate = scheduleDate });
            return agendamentos.ToList();
        }

        public async Task<List<Agendamento>> GetAgendamentosByProfissional(int profissionalId)
        {
            var sql = @"
                SELECT agendamento.*, 
                       paciente.Id as Paciente_Id, 
                       paciente.Name as Paciente_Name,
                       paciente.Cpf as Paciente_Cpf,
                       profissionalSaude.Id as Profissional_Id,
                       profissionalSaude.Name as Profissional_Name,
                       profissionalSaude.CRM as Profissional_CRM
                FROM Agendamento agendamento
                LEFT JOIN Paciente paciente ON agendamento.PacienteId = paciente.Id
                LEFT JOIN ProfissionalSaude profissionalSaude ON agendamento.ProfissionalSaudeId = profissionalSaude.Id 
                WHERE agendamento.ProfissionalSaudeId = @ProfissionalSaudeId ";

            using var connection = new SqlConnection(_connectionString);
            var agendamentosMap = await connection.QueryAsync<Agendamento, PacienteMap, ProfissionalSaudeMap, Agendamento>(
                sql,
                (agendamento, paciente, profissional) =>
                {
                    agendamento.Paciente = new Paciente
                    {
                        Name = paciente.Paciente_Name,
                        Cpf = paciente.Paciente_Cpf,
                        Id = paciente.Paciente_Id
                    };
                    agendamento.ProfissionalSaude = new ProfissionalSaude
                    {
                        Name = profissional.Profissional_Name,
                        Cpf = profissional.Profissional_Cpf,
                        CRM = profissional.Profissional_CRM
                    };
                    return agendamento;
                },
                 new { ProfissionalSaudeId = profissionalId },
                splitOn: "Paciente_Id,Profissional_Id"
            );

            return agendamentosMap.ToList();
        }

        public async Task<bool> DeletarConsultasDoPaciente(int pacienteId)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Agendamento WHERE PacienteId = @PacienteId";
            return await db.ExecuteScalarAsync<bool>(sql, new { PacienteId = pacienteId });
        }

        public async Task<bool> DeletarConsultasDoProfissionalDeSaude(int profissionalSaudeId)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Agendamento WHERE ProfissionalSaudeId = @ProfissionalSaudeId";
            return await db.ExecuteScalarAsync<bool>(sql, new { ProfissionalSaudeId = profissionalSaudeId });
        }
    }
}
