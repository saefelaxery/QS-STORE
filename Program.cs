using LaLaStore.Services;
using LaLaStore.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=(localdb)\\mssqllocaldb;Database=LaLaStore;Trusted_Connection=True;MultipleActiveResultSets=true"));

// Register services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();

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

app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    
    // Apply pending migrations (this will create the database and tables if they don't exist)
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    // Check if Products table exists
    bool tablesExist = false;
    try
    {
        // Try to query the Products table
        var testQuery = context.Database.ExecuteSqlRaw("SELECT TOP 1 1 FROM Products");
        tablesExist = true;
        logger.LogInformation("Database tables exist.");
    }
    catch (Exception ex)
    {
        // Table doesn't exist
        tablesExist = false;
        logger.LogWarning("Products table not found. Database will be recreated.");
    }
    
    // If tables don't exist, delete and recreate database
    if (!tablesExist)
    {
        try
        {
            logger.LogInformation("Deleting existing database...");
            context.Database.EnsureDeleted();
            logger.LogInformation("Database deleted. Creating new database...");
            
            // Use EnsureCreated to create tables from model
            context.Database.EnsureCreated();
            logger.LogInformation("Database created successfully with all tables using EnsureCreated.");
            
            // Now apply migrations to ensure schema is up to date
            try
            {
                context.Database.Migrate();
                logger.LogInformation("Migrations applied successfully.");
            }
            catch (Exception migrateEx)
            {
                logger.LogWarning(migrateEx, "Migrations check completed with warning, but tables were created.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to recreate database. Error: {Message}", ex.Message);
            throw;
        }
    }
    else
    {
        // Tables exist, just apply any pending migrations
        try
        {
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Migration check completed with warning.");
        }
    }
    
    // Seed initial data if database is empty
    try
    {
        // Check if Products table exists and is empty
        var hasProducts = false;
        try
        {
            hasProducts = context.Products.Any();
        }
        catch
        {
            // Table doesn't exist, will be created by migrations
            hasProducts = false;
        }
        
        if (!hasProducts)
        {
        var products = new List<LaLaStore.Models.Product>
        {
            new LaLaStore.Models.Product
            {
                Name = "Basic Cotton T-Shirt",
                Description = "Comfortable 100% cotton t-shirt, perfect for daily use. Available in multiple colors.",
                Price = 600m,
                ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=500",
                Category = "men",
                Sizes = new List<string> { "S", "M", "L", "XL" },
                Colors = new List<string> { "White", "Black", "Gray", "Blue" },
                InStock = true,
                Rating = 4.5
            },
            new LaLaStore.Models.Product
            {
                Name = "Classic Polo Shirt",
                Description = "Elegant polo shirt, suitable for work and casual occasions.",
                Price = 750m,
                ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?w=500",
                Category = "men",
                Sizes = new List<string> { "S", "M", "L", "XL", "XXL" },
                Colors = new List<string> { "White", "Black", "Blue", "Red" },
                InStock = true,
                Rating = 4.7
            },
            new LaLaStore.Models.Product
            {
                Name = "Sports T-Shirt",
                Description = "Lightweight and comfortable sports t-shirt, perfect for sports and daily activities.",
                Price = 650m,
                ImageUrl = "https://images.unsplash.com/photo-1618354691373-d851c5c3a990?w=500",
                Category = "men",
                Sizes = new List<string> { "S", "M", "L", "XL" },
                Colors = new List<string> { "Black", "Gray", "Blue", "Green" },
                InStock = true,
                Rating = 4.6
            },
            new LaLaStore.Models.Product
            {
                Name = "Women's Basic T-Shirt",
                Description = "Simple and elegant women's t-shirt, suitable for daily use.",
                Price = 500m,
                ImageUrl = "https://images.unsplash.com/photo-1576566588028-4147f3842f27?w=500",
                Category = "women",
                Sizes = new List<string> { "XS", "S", "M", "L" },
                Colors = new List<string> { "White", "Black", "Gray", "Beige" },
                InStock = true,
                Rating = 4.5
            },
            new LaLaStore.Models.Product
            {
                Name = "Classic Jeans",
                Description = "Comfortable and durable classic jeans. Perfect for any occasion.",
                Price = 1200m,
                ImageUrl = "https://images.unsplash.com/photo-1542272604-787c3835535d?w=500",
                Category = "men",
                Sizes = new List<string> { "28", "30", "32", "34", "36" },
                Colors = new List<string> { "Light Blue", "Dark Blue", "Black" },
                InStock = true,
                Rating = 4.6
            },
            new LaLaStore.Models.Product
            {
                Name = "Women's Jeans",
                Description = "Elegant and comfortable women's jeans with a stylish cut.",
                Price = 1250m,
                ImageUrl = "https://hips.hearstapps.com/vader-prod.s3.amazonaws.com/1736527960-untitled-3-67815035563fa.jpg?crop=0.774xw:0.830xh;0.111xw,0.0871xh&resize=980:*",
                Category = "women",
                Sizes = new List<string> { "26", "28", "30", "32", "34" },
                Colors = new List<string> { "Light Blue", "Dark Blue", "Black" },
                InStock = true,
                Rating = 4.7
            },
            new LaLaStore.Models.Product
            {
                Name = "Women's Sports Pants",
                Description = "Comfortable women's sports pants, perfect for sports and activities.",
                Price = 850m,
                ImageUrl = "https://images.unsplash.com/photo-1552902865-b72c031ac5ea?w=500&h=500&fit=crop",
                Category = "women",
                Sizes = new List<string> { "XS", "S", "M", "L" },
                Colors = new List<string> { "Black", "Gray", "Pink", "Blue" },
                InStock = true,
                Rating = 4.6
            },
            new LaLaStore.Models.Product
            {
                Name = "Kids T-Shirt",
                Description = "Fun and colorful t-shirt for kids, made from soft cotton.",
                Price = 450m,
                ImageUrl = "https://www.bellacanvas.com/bella/product/large/3010y_2.jpg",
                Category = "kids",
                Sizes = new List<string> { "2-3Y", "4-5Y", "6-7Y", "8-9Y" },
                Colors = new List<string> { "Red", "Blue", "Yellow" },
                InStock = true,
                Rating = 4.6
            }
        };
        
        context.Products.AddRange(products);
        context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // Log seeding errors but continue
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
