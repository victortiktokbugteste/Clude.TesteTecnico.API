using Clude.TesteTecnico.API.Application.Models;
using Clude.TesteTecnico.API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clude.TesteTecnico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;

        public AuthController(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Realiza o login do usuário",
            Description = "Endpoint para autenticação do usuário e geração do token JWT",
            OperationId = "Login",
            Tags = new[] { "Autenticação" }
        )]
        [SwaggerResponse(200, "Login realizado com sucesso", typeof(LoginResponse))]
        [SwaggerResponse(401, "Credenciais inválidas")]
        public IActionResult Login(
            [FromBody]
            [SwaggerParameter(Description = "Dados de login do usuário", Required = true)]
            LoginModel model)
        {
            // 🔐 Validação fake para entrevista. Substitua por acesso real ao banco se necessário
            if (model.Username != "admin" || model.Password != "123")
                return Unauthorized(new { message = "Credenciais inválidas" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, "Administrador")
            }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new LoginResponse { Token = tokenString });
        }
    }
}
