using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace SilverJewelry_RazorPage.Pages.SilverJewelryPage
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel()
        {
            _httpClient = new HttpClient();
        }

        public SilverJewelry SilverJewelry { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != null && role.Equals("1"))
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
                //HttpResponseMessage respone = await _httpClient.GetAsync("http://localhost:5050/api/SilverJewelry/all");
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5056/api/SilverJewelry/id?id={id}");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.SendAsync(requestMessage);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var data = await response.Content.ReadAsStringAsync();
                    SilverJewelry = JsonSerializer.Deserialize<SilverJewelry>(data, options);
                    return Page();
                }
                else
                {
                    return RedirectToPage("/SilverJewelryPage/Index");
                }
            }
            else
            {
                return RedirectToPage("/SilverJewelryPage/Index");
            }
        }
    }
}
