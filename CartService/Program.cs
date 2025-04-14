using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CartService.Models;
using CartService.Data;


namespace CartService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CartDbContext>(opt =>
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

            app.MapGet("/cart/{userId}", async ([FromRoute] string userId, CartDbContext db) =>
            {
                var items = await db.CartItems.Where(ci => ci.UserId == userId).ToListAsync();
                return Results.Ok(items);
            });

            app.MapPost("/cart", async ([FromBody] CartItem item, CartDbContext db) =>
            {
                var existing = await db.CartItems.FirstOrDefaultAsync(ci => ci.UserId == item.UserId && ci.RabbitId == item.RabbitId);
                if (existing != null)
                {
                    existing.Quantity += item.Quantity;
                }
                else
                {
                    db.CartItems.Add(item);
                }
                await db.SaveChangesAsync();
                return Results.Ok(item);
            });

            app.MapPut("/cart", async ([FromBody] CartItem item, CartDbContext db) =>
            {
                var existing = await db.CartItems.FirstOrDefaultAsync(ci => ci.UserId == item.UserId && ci.RabbitId == item.RabbitId);
                if (existing == null) return Results.NotFound();

                existing.Quantity = item.Quantity;
                await db.SaveChangesAsync();
                return Results.Ok(existing);
            });

            app.MapDelete("/cart/{userId}/{rabbitId}", async (string userId, int rabbitId, CartDbContext db) =>
            {
                var existing = await db.CartItems.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.RabbitId == rabbitId);
                if (existing == null) return Results.NotFound();

                db.CartItems.Remove(existing);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            app.Run();
        }
    }



}
