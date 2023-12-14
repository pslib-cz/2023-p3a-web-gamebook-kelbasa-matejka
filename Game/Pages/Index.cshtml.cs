using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game.Pages
{
    public class IndexModel : PageModel
    {
        private JsonContent _jsonContent = new JsonContent();
        
        public void OnGet()
        {

        }
    }
}
