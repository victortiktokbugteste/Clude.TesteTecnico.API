using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Clude.TesteTecnico.API.Domain.Entities;
using Clude.TesteTecnico.API.Application.Interfaces;
using Clude.TesteTecnico.API.Application.Dtos;

namespace Clude.TesteTecnico.API.Infrastructure.Services
{
    public class LogService : ILogService
    {
        private readonly string _connectionString;

        public LogService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
        }

        public async Task RegistrarAsync(LogDto logDto)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO ApplicationMiddlewareLogError (CreateDate, Method, Exception, Trace, StatusCode) 
                       VALUES (@CreateDate, @Method, @Exception, @Trace, @StatusCode);
                       SELECT CAST(SCOPE_IDENTITY() as int)";

            var log = new ApplicationMiddlewareLogError
            {
                CreateDate = logDto.CreateDate,
                Method = logDto.Method,
                Exception = logDto.Exception,
                Trace = logDto.Trace,
                StatusCode  = logDto.StatusCode
            };

            await db.ExecuteAsync(sql, log);
        }
    }
} 