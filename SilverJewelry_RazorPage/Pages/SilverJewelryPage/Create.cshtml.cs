using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace SilverJewelry_RazorPage.Pages.SilverJewelryPage
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel()
        {
            _httpClient = new HttpClient();
        }


        [BindProperty]
        public SilverJewelry SilverJewelry { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (!ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5056/api/SilverJewelry");
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var responseCategory1 = await _httpClient.SendAsync(requestMessage);

                    if (responseCategory1.IsSuccessStatusCode)
                    {
                        var data = await responseCategory1.Content.ReadAsStringAsync();
                        var categories = JsonSerializer.Deserialize<List<Category>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
                    }
                }
                return Page();
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var jsonContent = new StringContent(
            JsonSerializer.Serialize(SilverJewelry),
            Encoding.UTF8,
            "application/json"
            );
            //HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:5050/api/SilverJewelry/create", jsonContent);



            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var requestMessage1 = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5056/api/SilverJewelry/create");
            requestMessage1.Content = jsonContent;
            requestMessage1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseCategory = await _httpClient.SendAsync(requestMessage1);

            if (responseCategory.IsSuccessStatusCode)
            {
                var responseData = await responseCategory.Content.ReadAsStringAsync();
                Console.WriteLine("Created Jewelry: " + responseData);
            }
            else
            {
                Console.WriteLine("Error: " + responseCategory.StatusCode);
            }

            return RedirectToPage("./Index");
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != null && role.Equals("1"))
            {
                //HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:516/api/SilverJewelry"); // Adjust the URL based on your API endpoint
                var token = HttpContext.Session.GetString("JwtToken");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }
                var requestMessage1 = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5056/api/SilverJewelry");
                requestMessage1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var responseCategory = await _httpClient.SendAsync(requestMessage1);
                if (responseCategory.IsSuccessStatusCode)
                {
                    var data = await responseCategory.Content.ReadAsStringAsync();
                    var categories = JsonSerializer.Deserialize<List<Category>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName"); // Assuming your Category model has "CategoryId" and "CategoryName"
                }
                else
                {
                    // Handle error if necessary
                    var data = await responseCategory.Content.ReadAsStringAsync();
                    var categories = JsonSerializer.Deserialize<List<Category>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/SilverPage/Index");
            }
        }
    }
}
