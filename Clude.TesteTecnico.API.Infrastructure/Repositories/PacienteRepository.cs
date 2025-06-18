using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Clude.TesteTecnico.API.Domain.Entities;
using Clude.TesteTecnico.API.Domain.Interfaces;
using System.Linq.Expressions;

namespace Clude.TesteTecnico.API.Infrastructure.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly string _connectionString;

        public PacienteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Paciente> AddAsync(Paciente entity)
        {
            entity.CreateDate = DateTime.UtcNow;

            using var db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Paciente (Name, Cpf, BirthDate, CreateDate) 
                       VALUES (@Name, @Cpf, @BirthDate, @CreateDate);
                       SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = await db.QuerySingleAsync<int>(sql, entity);
            entity.Id = id;
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Paciente WHERE Id = @Id";
            await db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM Paciente WHERE Id = @Id";
            return await db.ExecuteScalarAsync<bool>(sql, new { Id = id });
        }

        public async Task<List<Paciente>> GetAllAsync()
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Paciente";
            var pacientes = await db.QueryAsync<Paciente>(sql);
            return pacientes.ToList();
        }

        public async Task<Paciente> GetByIdAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Paciente WHERE Id = @Id";
            return await db.QuerySingleOrDefaultAsync<Paciente>(sql, new { Id = id });
        }

        public async Task<Paciente> UpdateAsync(Paciente entity)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = @"UPDATE Paciente 
                       SET Name = @Name, 
                           Cpf = @Cpf, 
                           BirthDate = @BirthDate 
                       WHERE Id = @Id";

            await db.ExecuteAsync(sql, entity);
            return entity;
        }
    }
} 