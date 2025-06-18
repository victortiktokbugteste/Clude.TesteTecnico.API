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
                       profissionalSaude.Cpf as Profissional_Cpf,
                       profissionalSaude.CRM as Profissional_CRM
                FROM Agendamento agendamento
                LEFT JOIN Paciente paciente ON agendamento.PacienteId = paciente.Id
                LEFT JOIN ProfissionalSaude profissionalSaude ON agendamento.ProfissionalSaudeId = profissionalSaude.Id
                WHERE agendamento.Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            var agendamentoMap = await connection.QueryAsync<Agendamento, PacienteMap, ProfissionalSaudeMap, Agendamento>(
                sql,
                (agendamento, pacienteMap, profissionalMap) =>
                {
                    agendamento.Paciente = new Paciente
                    {
                        Name = pacienteMap.Paciente_Name,
                        Cpf = pacienteMap.Paciente_Cpf
                    };
                    agendamento.ProfissionalSaude = new ProfissionalSaude
                    {
                        Name = profissionalMap.Profissional_Name,
                        Cpf = profissionalMap.Profissional_Cpf,
                        CRM = profissionalMap.Profissional_CRM
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
                       profissionalSaude.Cpf as Profissional_Cpf,
                       profissionalSaude.CRM as Profissional_CRM
                FROM Agendamento agendamento
                LEFT JOIN Paciente paciente ON agendamento.PacienteId = paciente.Id
                LEFT JOIN ProfissionalSaude profissionalSaude ON agendamento.ProfissionalSaudeId = profissionalSaude.Id ";

            using var connection = new SqlConnection(_connectionString);
            var agendamentoMap = await connection.QueryAsync<Agendamento, PacienteMap, ProfissionalSaudeMap, Agendamento>(
                sql,
                (agendamento, pacienteMap, profissionalMap) =>
                {
                    agendamento.Paciente = new Paciente
                    {
                        Name = pacienteMap.Paciente_Name,
                        Cpf = pacienteMap.Paciente_Cpf
                    };
                    agendamento.ProfissionalSaude = new ProfissionalSaude
                    {
                        Name = profissionalMap.Profissional_Name,
                        Cpf = profissionalMap.Profissional_Cpf,
                        CRM = profissionalMap.Profissional_CRM
                    };
                    return agendamento;
                },
                splitOn: "Paciente_Id,Profissional_Id"
            );

            return agendamentoMap.ToList();
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
                SET PacienteId = @PacienteId,
                    ProfissionalSaudeId = @ProfissionalSaudeId,
                    CreateDate = @CreateDate,
                    ScheduleDate = @ScheduleDate,
                    TempoDuracaoAtendimentoMinutos = @TempoDuracaoAtendimentoMinutos
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
    }
}
