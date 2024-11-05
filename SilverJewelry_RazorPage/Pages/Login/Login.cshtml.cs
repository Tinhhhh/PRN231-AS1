using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using DataAccess;
using System.Text;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SilverJewelry_RazorPage.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LoginModel()
        {
            _httpClient = new HttpClient();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public BranchAccount BranchAccount { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            LoginRequest request = new LoginRequest();
            request.email = BranchAccount.EmailAddress;
            request.password = BranchAccount.AccountPassword;
            var jsonContent = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json"
            );
            HttpResponseMessage response = await _httpClient.PostAsync($"http://localhost:5056/api/Jwt", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("JwtToken", responseData);
                var role = GetRoleFromToken(responseData);

                HttpContext.Session.SetString("Role", role);

                Console.WriteLine("Created Jewelry: " + responseData);
                return RedirectToPage("/SilverJewelryPage/Index");

            }
            else
            {

                /*                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                                {
                                    return RedirectToPage("/Login/Login", new { errorMessage = "You do not have permission to do this function!" });
                                }*/
                Console.WriteLine("Error: " + response.StatusCode);
                //ModelState.AddModelError("Error", "You do not have permission to do this function!");
                //var errorMessage = "You do not have permission to do this function!";
                //return Page();
                return RedirectToPage("/Login/Login", new { errorMessage = "You do not have permission to do this function!" });
            }
        }

        private string GetRoleFromToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

            // Lấy claim có tên là "roles"
            var role = jsonToken?.Claims
                .FirstOrDefault(claim => claim.Type == "roles" || claim.Type == ClaimTypes.Role)?.Value;

            return role ?? string.Empty;
        }
    }
}
