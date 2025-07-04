﻿using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Infrastructure
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
