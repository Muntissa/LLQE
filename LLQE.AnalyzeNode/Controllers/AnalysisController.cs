using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LLQE.AnalyzeNode.Controllers
{
    [ApiController]
    [Route("analysis")]
    public class AnalysisController : ControllerBase
    {
        [HttpPost]
        public IActionResult Analyze([FromBody] JObject jsonData)
        {
            var posts = jsonData["posts"] as JArray;
            int postCount = posts?.Count ?? 0;

            return Ok(new { PostCount = postCount });
        }
    }
} 