using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace QuizApp.Controllers
{
    [Route("api/quiz")]
    [ApiController]
    public class QuizController : ControllerBase///////apispecific non view return/
    {
        [HttpGet("music")]
        public IActionResult GetQuizQuestions()
        {
            var questions = new List<object>
            {
                new { id = 1, question = "What note is this?", imageUrl = "http://localhost:5179/images/A_Note.png", answers = new[] { "A: A", "B: B", "C: C", "D: D", "E: E", "F: F", "G: G" }},                
                new { id = 2, question = "What note is this?", imageUrl = "http://localhost:5179/images/B_Note.png", answers = new[] { "A: A", "B: B", "C: C", "D: D", "E: E", "F: F", "G: G" }},
                new { id = 3, question = "What note is this?", imageUrl = "http://localhost:5179/images/C_Note.png", answers = new[] { "A: A", "B: B", "C: C", "D: D", "E: E", "F: F", "G: G" }},
                new { id = 4, question = "What note is this?", imageUrl = "http://localhost:5179/images/d_Note.png", answers = new[] { "A: A", "B: B", "C: C", "D: D", "E: E", "F: F", "G: G" }},
                new { id = 5, question = "What note is this?", imageUrl = "http://localhost:5179/images/E_Note.png", answers = new[] { "A: A", "B: B", "C: C", "D: D", "E: E", "F: F", "G: G" }},
                new { id = 6, question = "What note is this?", imageUrl = "http://localhost:5179/images/F_Note.png", answers = new[] { "A: A", "B: B", "C: C", "D: D", "E: E", "F: F", "G: G" }},
                new { id = 7, question = "What note is this?", imageUrl = "http://localhost:5179/images/G_Note.png", answers = new[] { "A: A", "B: B", "C: C", "D: D", "E: E", "F: F", "G: G" }}
                
            };

            return Ok(questions);
        }
    }
}
