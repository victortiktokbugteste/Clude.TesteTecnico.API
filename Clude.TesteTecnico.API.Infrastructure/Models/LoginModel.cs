using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Clude.TesteTecnico.API.Infrastructure.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório")]
        [SwaggerSchema(Description = "Nome de usuário para login")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [SwaggerSchema(Description = "Senha do usuário", Format = "password")]
        public string Password { get; set; }
    }
} 