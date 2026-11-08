using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Net.Http.Json;

namespace RaceDay.MVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ParticipantController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Participant or /Participant/Index
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var events = await client.GetFromJsonAsync<List<EventViewModel>>("api/events");
            return View(events ?? new List<EventViewModel>());
        }

        // POST: /Participant/Register
        [HttpPost]
        public async Task<IActionResult> Register(int eventId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "Please log in to register for an event.";
                return RedirectToAction("Login", "Account");
            }

            var enrolmentDto = new
            {
                UserId = userId.Value,
                EventId = eventId,
                EnrolmentDate = DateTime.UtcNow
            };

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var response = await client.PostAsJsonAsync("api/enrolments", enrolmentDto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Successfully registered for the event!";
                return RedirectToAction(nameof(MyEnrolments));
            }

            TempData["Error"] = "Failed to register for the event. You may already be registered.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Participant/MyEnrolments
        public async Task<IActionResult> MyEnrolments()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var enrolments = await client.GetFromJsonAsync<List<EnrolmentViewModel>>($"api/enrolments/user/{userId.Value}");

            return View(enrolments ?? new List<EnrolmentViewModel>());
        }

        // Alias for navbar link compatibility: GET /Participant/MyEvents
        public IActionResult MyEvents()
        {
            return RedirectToAction(nameof(MyEnrolments));
        }

        // GET: /Participant/MyResults
        public async Task<IActionResult> MyResults()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var results = new List<ResultViewModel>();

            try
            {
                // Tries the user-specific route if your API has it
                results = await client.GetFromJsonAsync<List<ResultViewModel>>($"api/results/user/{userId.Value}")
                          ?? new List<ResultViewModel>();
            }
            catch
            {
                try
                {
                    // Fallback: Fetch all results and filter client-side if the API only exposes `api/results`
                    var allResults = await client.GetFromJsonAsync<List<ResultViewModel>>("api/results");
                    if (allResults != null)
                    {
                        results = allResults.Where(r => r.Enrolment?.UserId == userId.Value).ToList();
                    }
                }
                catch
                {
                    results = new List<ResultViewModel>();
                }
            }

            return View(results);
        }

        // GET: /Participant/Profile
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var response = await client.GetAsync($"api/users/{userId.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserViewModel>();
                    if (user != null) return View(user);
                }
            }
            catch
            {
                // Gracefully catch API connection issues and use session fallback
            }

            // Session Fallback so profile view populates cleanly
            var fallbackUser = new UserViewModel
            {
                UserId = userId.Value,
                Username = HttpContext.Session.GetString("UserName") ?? HttpContext.Session.GetString("UserEmail") ?? "Participant User",
                Email = HttpContext.Session.GetString("UserEmail") ?? "N/A"
            };

            return View(fallbackUser);
        }
    }
}