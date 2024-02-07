using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RabbitMq.Excel.Controllers
{
	public class AccountController : Controller
	{

		private UserManager<IdentityUser> _userManager;
		private SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> LoginAsync(string email,string password)
		{

            var user = await _userManager.FindByEmailAsync(email);
			if(user == null) return View("Error");

			var signInResult = await _signInManager.PasswordSignInAsync(user, password,true,false);
			if (!signInResult.Succeeded) return View("error"); 
			
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

	}
}
