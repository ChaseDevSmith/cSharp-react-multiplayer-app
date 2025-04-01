using Microsoft.AspNetCore.SignalR;
using System;
using System.Text.Json;
using cSharpQuiz.Models;

public class GameHub : Hub
{
     private readonly IHttpClientFactory _httpClientFactory;

    public GameHub(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
  private static Dictionary<string, List<string>> rooms = new Dictionary<string, List<string>>(); 
    private static Dictionary<string, Dictionary<string, int>> scores = new Dictionary<string, Dictionary<string, int>>();  
    private static Dictionary<string, string> roomGameTypes = new Dictionary<string, string>(); 
    private static Dictionary<string, string> roomHosts = new Dictionary<string, string>();  
    
    public async Task<string> CreateRoom(string gameType)
    {
        string roomCode = Guid.NewGuid().ToString("N").Substring(0, 6); 
        
        Console.WriteLine($"Room created: {roomCode}");

       
        rooms[roomCode] = new List<string>(); 
        scores[roomCode] = new Dictionary<string, int>();  
        roomGameTypes[roomCode]= gameType;
        roomHosts[roomCode] = Context.ConnectionId;


        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
        await Clients.All.SendAsync("RoomCreated", roomCode, gameType);

        return roomCode;
    }

   
    public async Task JoinRoom(string roomCode)
    {
        if (rooms.ContainsKey(roomCode))
        {
            
            rooms[roomCode].Add(Context.ConnectionId);
            scores[roomCode][Context.ConnectionId] = 0;  
            
            Console.WriteLine($"User with connection ID {Context.ConnectionId} joined room {roomCode}");

            await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
            await Clients.Group(roomCode).SendAsync("UserJoined", Context.ConnectionId);
        }
        else
        {
     
            await Clients.Caller.SendAsync("Error", "Room does not exist.");
        }
    }

    public async Task ReportScore(string roomCode, string userId, int score)
    {
 
        Console.WriteLine($"Score reported for user {userId} in room {roomCode}: {score} points");

       
        await Clients.Group(roomCode).SendAsync("ScoreUpdated", userId, score);
    }
    
// {
//     if (rooms.ContainsKey(roomCode) && rooms[roomCode].Count > 0)
//     {
//         // Ensure only the host can start the game
//         if (roomHosts[roomCode] != Context.ConnectionId)
//         {
//             await Clients.Caller.SendAsync("Error", "Only the host can start the game.");
//             return;
//         }

//         // Get the game type
//         if (!roomGameTypes.ContainsKey(roomCode))
//         {
//             await Clients.Caller.SendAsync("Error", "Game type is not defined for this room.");
//             return;
//         }

//         string gameType = roomGameTypes[roomCode];
//         Console.WriteLine($"Starting game in room {roomCode} with game type {gameType}");

//         // Send game started message to the group
//         await Clients.Group(roomCode).SendAsync("GameStarted", $"The {gameType} is starting!");

//         try
//         {
//             // API to get questions based on the game type
//             string apiUrl = gameType switch
//             {
//                 "MusicQuiz" => "http://localhost:5179/api/quiz/music", 
//                 "CountryQuiz" => "http://localhost:5179/api/quiz/country",
//                 _ => throw new ArgumentException($"Unknown game type: {gameType}")
//             };

//             var client = new HttpClient();
//             var response = await client.GetStringAsync(apiUrl);
//             var questions = JsonSerializer.Deserialize<List<Question>>(response);

//             // Check if questions were fetched successfully
//             if (questions != null && questions.Count > 0)
//             {
//                 var question = questions[0];  // Get the first question
//                 Console.WriteLine($"Sending first question to room {roomCode}: {question.QuestionText}");
//                 // Send the question to all clients in the room
//                 await Clients.Group(roomCode).SendAsync("Question", question);
//             }
//             else
//             {
//                 await Clients.Caller.SendAsync("Error", "No questions available.");
//             }
//         }
//         catch (Exception ex)
//         {
//             await Clients.Caller.SendAsync("Error", $"Error fetching quiz questions: {ex.Message}");
//         }
//     }
//     else
//     {
//         await Clients.Caller.SendAsync("Error", "Room doesn't exist or has no players.");
//     }
// }
// ///
public async Task StartGame(string roomCode)
{
    if (rooms.ContainsKey(roomCode) && rooms[roomCode].Count > 0)
    {
        if (roomHosts[roomCode] != Context.ConnectionId)
        {
            await Clients.Caller.SendAsync("Error", "Only the host can start the game.");
            return;
        }

        if (!roomGameTypes.ContainsKey(roomCode))
        {
            await Clients.Caller.SendAsync("Error", "Game type is not defined for this room.");
            return;
        }

        string gameType = roomGameTypes[roomCode];
       
        try
        {
            string apiUrl = gameType switch
            {
                "MusicQuiz" => "http://localhost:5179/api/quiz/music", 
                "CountryQuiz" => "http://localhost:5179/api/quiz/country",
                _ => throw new ArgumentException($"Unknown game type: {gameType}")
            };

            var client = _httpClientFactory.CreateClient();
                Console.WriteLine($"Making request to: {apiUrl}");

            var response = await client.GetStringAsync(apiUrl);

            Console.WriteLine("API Response: " + response);

            var questions = JsonSerializer.Deserialize<List<Question>>(response);

            if (questions != null && questions.Count > 0)
            {
                Console.WriteLine($"Received {questions.Count} questions");
                var question = questions[0];  
                Console.WriteLine($"Sending first question to room {roomCode}: {question.question}");

                await Clients.Group(roomCode).SendAsync("Question", question);


                Console.WriteLine($"Starting game in room {roomCode} with game type {gameType}");

        await Clients.Group(roomCode).SendAsync("GameStarted", $"The {gameType} is starting!");
            }
            else
            {
                                Console.WriteLine($"sucks");

                await Clients.Caller.SendAsync("Error", "No questions available.");
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", $"Error fetching quiz questions: {ex.Message}");
                                            Console.WriteLine($"sucks1");

        }
    }
    else
    {
        await Clients.Caller.SendAsync("Error", "Room doesn't exist or has no players.");
                                        Console.WriteLine($"sucks3");
                                                Console.WriteLine($"sucks3");

    }
                                    Console.WriteLine($"sucks2");

  

}

   public async Task SendQuestionToRoom(string roomCode, Question question)
    {
        await Clients.Group(roomCode).SendAsync("Question", question);
    }
    
}


