using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Clude.TesteTecnico.API.Domain.Entities;
using Clude.TesteTecnico.API.Domain.Interfaces;

namespace Clude.TesteTecnico.API.Infrastructure.Repositories
{
    public class ProfissionalSaudeRepository : IProfissionalSaudeRepository
    {
        private readonly string _connectionString;

        public ProfissionalSaudeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ProfissionalSaude> AddAsync(ProfissionalSaude entity)
        {
            entity.CreateDate = DateTime.UtcNow;

            using var db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO ProfissionalSaude (Name, Cpf, CRM, CreateDate) 
                       VALUES (@Name, @Cpf, @CRM, @CreateDate);
                       SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = await db.QuerySingleAsync<int>(sql, entity);
            entity.Id = id;
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "DELETE FROM ProfissionalSaude WHERE Id = @Id";
            await db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM ProfissionalSaude WHERE Id = @Id";
            return await db.ExecuteScalarAsync<bool>(sql, new { Id = id });
        }

        public async Task<bool> ExistsByCpfOrCRMAsync(string cpf, string crm, int? id = 0)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM ProfissionalSaude WHERE (Cpf = @cpf OR CRM = @crm) ";

            if (id > 0)
                sql += " AND Id <> @id ";

            return await db.ExecuteScalarAsync<bool>(sql, new { Cpf = cpf, CRM  = crm, Id = id });
        }

        public async Task<List<ProfissionalSaude>> GetAllAsync()
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM ProfissionalSaude";
            var pacientes = await db.QueryAsync<ProfissionalSaude>(sql);
            return pacientes.ToList();
        }

        public async Task<ProfissionalSaude> GetByIdAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM ProfissionalSaude WHERE Id = @Id";
            return await db.QuerySingleOrDefaultAsync<ProfissionalSaude>(sql, new { Id = id });
        }

        public async Task<ProfissionalSaude> UpdateAsync(ProfissionalSaude entity)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = @"UPDATE ProfissionalSaude 
                       SET Name = @Name, 
                           Cpf = @Cpf, 
                           CRM = @CRM 
                       WHERE Id = @Id";

            await db.ExecuteAsync(sql, entity);
            return entity;
        }
    }
} 