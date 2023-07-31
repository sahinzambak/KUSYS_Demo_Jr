using KUSYS_Demo.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<KUSYSDbContext>(options =>
            options.UseSqlServer("Server=.\\SQLEXPRESS;Database=KUSYS-Demo;Integrated Security=true;"));
//dbcontextimi servislere yazdým, conn string de burada

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/");

app.Run();
