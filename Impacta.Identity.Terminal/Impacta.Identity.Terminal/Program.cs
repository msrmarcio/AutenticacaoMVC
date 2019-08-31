using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Adicionado referencias para utilizar o IDENTITY
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims; 


namespace Impacta.Identity.Terminal
{
	class Program
	{
		static void Main(string[] args)
		{
			// criar um usuario e senha
			// que será armazenado no banco de dados
			// gerenciado pelo IDENTITY
			var nomeUsuario = "msrmarcio2@outlook.com";
			var senha = "Password123!";


			// vamos criar uma estrutura para receber o usuario
			// e para gerenciar as informações de autenticação
			// Para utilizar o identity e criar um usuario, precisamos 
			// receber uma instancia de UserStore que é tipado com a Classe
			// IdentityUser (esta classe é esperado pelo EntityFramework)
			var usuarioArmazenado = new UserStore<IdentityUser>();
			// criar um objeto para fazer a gestão do usuario
			var usuarioGerenciador = new UserManager<IdentityUser>(usuarioArmazenado);

			IdentityUser objIdentiyUser = new IdentityUser(nomeUsuario);
			var resultado = usuarioGerenciador.Create(objIdentiyUser, senha);
			// as duas linha são similares - fazem a mesma coisa, porem esta instancia diretamente
			// no metodo
			//var novo_resultado = usuarioGerenciador.Create(new IdentityUser(nomeUsuario), senha);

			// verificar o status de retorno da criacao do usuario
			Console.WriteLine("Status Create {0}", resultado.Succeeded);
			Console.ReadLine();


			// recupera as informações do usuario
			var identidadeUsuario = usuarioGerenciador.FindByName(nomeUsuario);

			// add CLAIM
			//usuarioGerenciador.AddClaim(identidadeUsuario.Id, new Claim("Nome_Usuario", "Marcio"));

			// Esta forma usar uma constante do Identity, para colocar a descricao 
			// da claim. Estas duas linhas~fazem a mesma coisa.
			usuarioGerenciador.AddClaim(identidadeUsuario.Id, new Claim(ClaimTypes.GivenName, "Marcio"));

			// vamos verificar se o password existe ou esta correto
			var validaSenha = usuarioGerenciador.CheckPassword(identidadeUsuario, senha);

			// vamos escrever resultado da comparacao da senha
			Console.WriteLine("Senha verificada: {0}", validaSenha);


			Console.ReadLine();
		}
	}
}
