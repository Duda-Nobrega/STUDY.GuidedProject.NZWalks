using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalksAPI.Controllers
{

    //https://localhostt:portumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            string[] StudentsNames = ["Nathan", "John", "Mark", "Luna", "David"];
            return Ok(StudentsNames);
        }
    }
}


