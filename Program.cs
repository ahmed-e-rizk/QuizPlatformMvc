using Microsoft.Net.Http.Headers;
using QuizPlatform.Helder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("Quiz", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7265/api/");

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.UseMiddleware<TokenMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Quiz}/{action=Create}");

app.Run();
