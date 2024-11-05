using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace SilverJewelry_RazorPage.Pages.SilverJewelryPage
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel()
        {
            _httpClient = new HttpClient();
        }

        [BindProperty]
        public SilverJewelry SilverJewelry { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = HttpContext.Session.GetString("Role");
            if (role != null && role.Equals("1"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                //HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5050/api/SilverJewelry/id?id={id}");
                //HttpResponseMessage responseCategory = await _httpClient.GetAsync("http://localhost:5050/api/SilverJewelry");
                var token = HttpContext.Session.GetString("JwtToken");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5056/api/SilverJewelry/id?id={id}");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.SendAsync(requestMessage);

                var requestMessage1 = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5056/api/SilverJewelry");
                requestMessage1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var responseCategory = await _httpClient.SendAsync(requestMessage1);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var dataCategory = await responseCategory.Content.ReadAsStringAsync();
                    var jewelry = JsonSerializer.Deserialize<SilverJewelry>(data, options);
                    SilverJewelry = jewelry;
                    var categories = JsonSerializer.Deserialize<List<Category>>(dataCategory, options);
                    ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
                }
                else
                {
                    // Handle error if necessary
                    ViewData["CategoryId"] = new SelectList(new List<Category>(), "CategoryId", "CategoryName");
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/SilverJewelryPage/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var token = HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var requestMessage1 = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:5056/api/SilverJewelry/delete?id={id}");
            requestMessage1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseCategory = await _httpClient.SendAsync(requestMessage1);
            if (responseCategory.IsSuccessStatusCode)
            {
                var responseData = await responseCategory.Content.ReadAsStringAsync();
                Console.WriteLine("Delete Jewelry: " + responseData);
            }
            else
            {
                Console.WriteLine("Error: " + responseCategory.StatusCode);
            }

            return RedirectToPage("/SilverJewelryPage/Index");
        }
    }
}
