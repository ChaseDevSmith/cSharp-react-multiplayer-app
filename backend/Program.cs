using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://127.0.0.1:5179", "http://localhost:5179", "http://loaclhost:5500","http://0.0.0.0:5179","http://0.0.0.0:5179")  
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigin", policy =>
//     {
//         policy.AllowAnyOrigin()  
//               .AllowAnyHeader()
//               .AllowAnyMethod()
//               .AllowCredentials();
//     });
// });
builder.Services.AddControllers();
builder.Services.AddHttpClient();

var app = builder.Build();


app.UseCors("AllowSpecificOrigin");


app.UseRouting();


app.MapHub<GameHub>("/gameHub");
app.MapControllers();
app.UseStaticFiles();

app.Run();
