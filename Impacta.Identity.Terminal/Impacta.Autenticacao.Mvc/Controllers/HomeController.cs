using Impacta.Autenticacao.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.Mvc;

namespace Impacta.Autenticacao.Mvc.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Inicio()
		{
			return View();
		}

		public ActionResult AreaLivre()
		{
			ViewBag.Message = "Você esta em uma Area de Acesso Livre .";

			return View();
		}

		[Authorize]
		public ActionResult AreaRestrita()
		{
			ViewBag.Message = "Você esta na Area Restrita.";

			return View();
		}

		public ActionResult LoginView()
		{
			ViewBag.Message = "Você esta na pagina de Login, seja bem vindo";

			return View();
		}

		public ActionResult CriarLogin()
		{
			Usuario usuario = null;

			return View(usuario);
		}

		[HttpPost]
		public ActionResult CriarLogin(Usuario usuario)
		{
			//chamar metodo SalvarUsuario
			//bool resultado = SalvarUsuario(usuario);

			//if (resultado)
			if (SalvarUsuario(usuario))
			{
				return View("Inicio");
			}
			else
			{
				return View("CriarLogin");
			}
		}

		private bool SalvarUsuario(Usuario usuario)
		{
			bool retorno = false;

			//Obter a UserStore, UserManager
			var usuarioStore = new UserStore<IdentityUser>();
			var usuarioGerenciador =
			new UserManager<IdentityUser>(usuarioStore);

			//criar uma entidade
			var usuarioInfo =
				new IdentityUser() { UserName = usuario.UserName };

			// Gravar na base de dados o usuario
			IdentityResult resultado =
				usuarioGerenciador.Create(usuarioInfo, usuario.Password);

			if (resultado.Succeeded)
			{
				//Autentica e volta para a página inicial
				var gerenciadorDeAutenticacao = HttpContext.GetOwinContext().Authentication;

				var identidadeUsuario =
					usuarioGerenciador.CreateIdentity(usuarioInfo,
											DefaultAuthenticationTypes.ApplicationCookie);

				gerenciadorDeAutenticacao.SignIn(
									new AuthenticationProperties() { }, identidadeUsuario);

				// retorna verdadeiro se o registro foi salvo com sucesso
				retorno = true;
			}
			else
			{
				// houve algum erro ao salvar o registro, então retorna false
				retorno = false;

				// adicionamos o erro na ViewBag para ser capturado em nossa pagina de login
				ViewBag.Erro = resultado.Errors;
			}

			return retorno;
		}

		[HttpPost]
		public ActionResult LoginView(Usuario usuario)
		{
			if (AutenticarUsuario(usuario))
			{
                return RedirectToAction("AreaRestrita", "Home");
			}
			else
			{
                return RedirectToAction("Inicio", "Home");
            }
        }

		private bool AutenticarUsuario(Usuario usuario)
		{
			// variavel de retorno
			bool retorno = false;

			var usuarioStore = new UserStore<IdentityUser>();
			var usuarioGerenciador = new UserManager<IdentityUser>(usuarioStore);

			var identidadeUsuario = usuarioGerenciador.Find(usuario.UserName, usuario.Password);

			if (identidadeUsuario != null)
			{
				var gerenciadorDeAutenticacao = HttpContext.GetOwinContext().Authentication;

				var identidade = usuarioGerenciador.CreateIdentity(identidadeUsuario,
				                    DefaultAuthenticationTypes.ApplicationCookie);

				gerenciadorDeAutenticacao.SignIn(
					new AuthenticationProperties()
						{ IsPersistent = false },identidade);

				// se foi encontrado o usuario retorna TRUE
				retorno = true;
			}
			else
			{
				ViewBag.MensagemErro = "Usuario ou senha invalida.";
				// se não econtrou o usuário ou senha invalida
				retorno = false;
			}

			// retorna o resutlado
			return retorno;
		}

        public ActionResult Logout()
        {
            var gerenciadorAutenticacao =
                HttpContext.GetOwinContext().Authentication;

            gerenciadorAutenticacao.SignOut();

            return RedirectToAction("LoginView", "Home");
        }
	}
}