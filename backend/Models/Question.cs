
namespace cSharpQuiz.Models
{
   public class Question
{
    public required int id { get; set; }

 

    public required string question { get; set; }

   
    public required string imageUrl { get; set; }

    public required string[] answers { get; set; }
}
}

