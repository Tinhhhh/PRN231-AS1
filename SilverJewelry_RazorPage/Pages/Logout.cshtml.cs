using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SilverJewelry_RazorPage.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("Token");

            return RedirectToPage("/Login/Login");
        }
    }
}
