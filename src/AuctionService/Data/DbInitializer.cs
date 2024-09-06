using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        SeedData(scope.ServiceProvider.GetService<AuctionDbContext>());
    }

    private static void SeedData(AuctionDbContext context)
    {
        context.Database.Migrate();
        if(context.Auctions.Any())
        {
            Console.WriteLine("Already exists");
            return;
        }

        var auctions = new List<Auction>()
        {
            new Auction 
            {
                Id = Guid.Parse("uioasdaslk14346nlkaf"),
                Status = Enums.Status.Live,
                ReservePrice = 2000,
                Seller = "Arnold",
                AuctionEnd = DateTime.UtcNow.AddDays(10),
                Item = new Item
                {
                    Make = "Ford",
                    Model = "GT",
                    Color = "Silver",
                    Mileage = 5000,
                    Year = 2020,
                    ImageUrl = "",
                }
            }
        };

        context.AddRange(auctions);
        context.SaveChanges();
    }
}
