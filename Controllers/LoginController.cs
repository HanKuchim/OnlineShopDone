using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace OnlineShop.Controllers
{
    public class LoginController : Controller
    {
        OnlineShopContext _db;
        public LoginController(OnlineShopContext context)
        {
            _db = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Authorization(string login, string password, string returnUrl)
        {
            // Проверка наличия email и пароля
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email и/или пароль не установлены");
            }
            

            // Поиск пользователя в базе данных
            var worker = await _db.Workers.FirstOrDefaultAsync(w => w.Login == login && w.Password == password);
            if (worker == null)
            {
                return Unauthorized();
            }

            // Получение должности пользователя из базы данных
            var jobtitle = await _db.Jobtitles.FirstOrDefaultAsync(j => j.Id == worker.Jobtitleid);
            if (jobtitle == null)
            {
                return NotFound("Должность не найдена");
            }

            // Создание claims для аутентификации
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, worker.Name),
                new Claim(ClaimTypes.Role, jobtitle.Name),
                new Claim("WorkerPickupPoint", worker.PickupPoint.ToString(), ClaimValueTypes.Integer)
                // Другие claims, если необходимо
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Аутентификация пользователя
            await HttpContext.SignInAsync(claimsPrincipal);

            return RedirectToAction("Index", "Products");

        }

    }
}
