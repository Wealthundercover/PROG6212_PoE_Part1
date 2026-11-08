using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Net.Http.Json;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            TempData.Clear(); // Clears any lingering success or error messages when landing on the login page
            return View();
        }
        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var response = await client.PostAsJsonAsync("api/auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                if (result != null)
                {
                    // Store user info in session
                    HttpContext.Session.SetString("AuthToken", result.Token);
                    HttpContext.Session.SetInt32("UserId", result.UserId);
                    HttpContext.Session.SetString("UserRole", result.Role);
                    HttpContext.Session.SetString("UserName", result.FullName);

                    // Redirect based on Role
                    if (result.Role == "Organiser")
                        return RedirectToAction("Dashboard", "Organiser");

                    return RedirectToAction("Index", "Participant");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login credentials.");
            return View(model);
        }

        // GET: /Account/Register
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            // Map the MVC view model to the exact payload structure expected by the Part 2 API
            var apiPayload = new
            {
                User = model.FullName,        // Satisfies the API's "user" requirement
                Name = model.FullName,        // Covers both naming conventions
                Email = model.Email,
                Password = model.Password,
                PasswordHash = model.Password, // Added to fix the PasswordHash validation error
                RoleId = model.Role == "Organiser" ? 1 : 2 // Organiser = 1, Participant = 2
            };

            var response = await client.PostAsJsonAsync("api/auth/register", apiPayload);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Registration successful! Please log in.";
                return RedirectToAction(nameof(Login));
            }

            // Read the exact error details from the API response body
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"API Error: {errorContent}");
            return View(model);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clears all session keys
            return RedirectToAction(nameof(Login));
        }
    }
}