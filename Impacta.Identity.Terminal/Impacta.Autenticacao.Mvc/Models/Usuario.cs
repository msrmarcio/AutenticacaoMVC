using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Impacta.Autenticacao.Mvc.Models
{
	public class Usuario : IUser
	{
		public string Id { get; set; }
		[Required]
		public string UserName { get; set; }

		[Required]
		[StringLength(10, MinimumLength = 5, ErrorMessage = ("Usuário deve conter entre 5 e 8 caracteres"))]
		public string Password { get; set; }

	}
}