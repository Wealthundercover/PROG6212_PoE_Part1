using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Net.Http.Json;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrganiserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Organiser/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var events = await client.GetFromJsonAsync<List<EventViewModel>>("api/events");

            return View(events ?? new List<EventViewModel>());
        }

        // GET: /Organiser/CreateEvent
        public IActionResult CreateEvent()
        {
            return View(new CreateEventViewModel());
        }

        // POST: /Organiser/CreateEvent
        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            // Use MultipartFormDataContent to send text properties AND the image file together
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.Name ?? ""), "Name");
            content.Add(new StringContent(model.Description ?? ""), "Description");
            content.Add(new StringContent(model.Date.ToString("o")), "Date");
            content.Add(new StringContent(model.Location ?? ""), "Location");

            // Send API-required properties (adjust or add fields to your ViewModel if you want user input for these)
            content.Add(new StringContent("5.0"), "DistanceKm");
            content.Add(new StringContent("Run"), "EventType");
            content.Add(new StringContent(userId.Value.ToString()), "UserId");

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var fileStream = model.ImageFile.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                if (!string.IsNullOrEmpty(model.ImageFile.ContentType))
                {
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.ImageFile.ContentType);
                }
                content.Add(fileContent, "ImageFile", model.ImageFile.FileName);
            }

            var response = await client.PostAsync("api/events", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Event created successfully!";
                return RedirectToAction(nameof(Dashboard));
            }

            // Read the exact error from the API for easy troubleshooting
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"API Error: {errorContent}");
            return View(model);
        }
    }

}