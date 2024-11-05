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
using System.Text.Json;

namespace SilverJewelry_RazorPage.Pages.SilverJewelryPage
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public IndexModel()
        {
            _httpClient = new HttpClient();
        }

        public IList<SilverJewelry> SilverJewelry { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchTerm)
        {
            var role = HttpContext.Session.GetString("Role");
            if ((role != null && role.Equals("1")) || (role != null && role.Equals("2")))
            {
                var token = HttpContext.Session.GetString("JwtToken");

                if (string.IsNullOrEmpty(token))
                {
                    //return Unauthorized();
                    return RedirectToPage("/Login/Login", new { errorMessage = "You do not have permission to do this function!" });
                }
                string apiUrl;
                //apiUrl = "http://localhost:5056/api/SilverJewelry/all";

                if (string.IsNullOrEmpty(searchTerm))
                {
                    // Nếu không có searchTerm, gọi API lấy tất cả dữ liệu
                    apiUrl = "http://localhost:5056/api/SilverJewelry/all";
                }
                else
                {
                    // Nếu có searchTerm, thêm nó vào URL của API
                    // apiUrl = $"http://localhost:5056/api/SilverJewelry/search?search={searchTerm}";

                    if (decimal.TryParse(searchTerm, out decimal weight))
                    {
                        apiUrl = $"http://localhost:5056/api/SilverJewelry/search?$filter=MetalWeight eq {weight}";
                    }
                    else
                    {
                        apiUrl = $"http://localhost:5056/api/SilverJewelry/search?$filter=contains(tolower(SilverJewelryName), '{searchTerm.ToLower()}')";
                    }
                }


                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.SendAsync(requestMessage);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var data = await response.Content.ReadAsStringAsync();
                    SilverJewelry = JsonSerializer.Deserialize<List<SilverJewelry>>(data, options);
                    return Page();
                }
                else
                {
                    /*ModelState.AddModelError("Error", "You do not have permission to do this function!");
                    return RedirectToPage("/Login/Login");*/
                    return RedirectToPage("/Login/Login", new { errorMessage = "You do not have permission to do this function!" });
                }
            }
            else
            {
                return RedirectToPage("/Login/Login");
            }
        }
    }
}
