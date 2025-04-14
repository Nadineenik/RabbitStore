using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FavoriteService.Models;
using FavoriteService.Data;


namespace FavoriteService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FavoriteDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapGet("/fav/{userId}", async ([FromRoute] string userId, FavoriteDbContext db) =>
            {
                var items = await db.FavoriteItems.Where(ci => ci.UserId == userId).ToListAsync();
                return Results.Ok(items);
            });

            app.MapPost("/fav", async ([FromBody] FavoriteItem item, FavoriteDbContext db) =>
            {
                var existing = await db.FavoriteItems.FirstOrDefaultAsync(ci => ci.UserId == item.UserId && ci.RabbitId == item.RabbitId);
                if (existing != null)
                {
                    existing.Quantity += item.Quantity;
                }
                else
                {
                    db.FavoriteItems.Add(item);
                }
                await db.SaveChangesAsync();
                return Results.Ok(item);
            });

            app.MapPut("/fav", async ([FromBody] FavoriteItem item, FavoriteDbContext db) =>
            {
                var existing = await db.FavoriteItems.FirstOrDefaultAsync(ci => ci.UserId == item.UserId && ci.RabbitId == item.RabbitId);
                if (existing == null) return Results.NotFound();

                existing.Quantity = item.Quantity;
                await db.SaveChangesAsync();
                return Results.Ok(existing);
            });

            app.MapDelete("/fav/{userId}/{rabbitId}", async (string userId, int rabbitId, FavoriteDbContext db) =>
            {
                var existing = await db.FavoriteItems.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.RabbitId == rabbitId);
                if (existing == null) return Results.NotFound();

                db.FavoriteItems.Remove(existing);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            app.Run();
        }
    }



}
