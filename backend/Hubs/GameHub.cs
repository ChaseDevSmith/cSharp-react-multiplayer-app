using Microsoft.AspNetCore.SignalR;
using System;

public class GameHub : Hub
{

    public async Task<string> CreateRoom()
    {
      
        string roomCode = Guid.NewGuid().ToString("N").Substring(0, 6); 

     
        Console.WriteLine($"Room created: {roomCode}");

       
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);

       
        await Clients.All.SendAsync("RoomCreated", roomCode);

        
        return roomCode;
    }

   
    public async Task JoinRoom(string roomCode)
    {
       
        Console.WriteLine($"User with connection ID {Context.ConnectionId} joined room {roomCode}");

       
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);

        
        await Clients.Group(roomCode).SendAsync("UserJoined", Context.ConnectionId);
    }

   
    public async Task ReportScore(string roomCode, string userId, int score)
    {
 
        Console.WriteLine($"Score reported for user {userId} in room {roomCode}: {score} points");

       
        await Clients.Group(roomCode).SendAsync("ScoreUpdated", userId, score);
    }
}
