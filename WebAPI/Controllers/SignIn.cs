using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[sign_in]")]

    public class SignIn : ControllerBase
    {
        // GET
        [HttpGet]
        public void Index()
        {
            return ;
        }
    }
}