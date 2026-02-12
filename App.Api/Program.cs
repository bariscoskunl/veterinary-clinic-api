using App.Api.Data;
using App.Api.Data.Entities;
using Bogus;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//-------Sql Server Added
builder.Services.AddDbContext<VeterinaryContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Cors Added
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // wwwroot calismasi icin


app.UseCors("AllowAll"); // Cors kullanmak icin ekledim

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    using (var context = scope.ServiceProvider.GetRequiredService<VeterinaryContext>())
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.Patients.Any())  // Eger Veri yoksa diye Seed data olusturdum.
        {
          var faker = new Faker<Patient>("tr")
                .RuleFor(x => x.AnimalName, f => f.PickRandom("Karabas", "Pamuk", "Boncuk", "Zeytin", "Leo", "Max"))
                .RuleFor(x => x.Species, f => f.PickRandom("Kedi", "Köpek"))
                .RuleFor(x => x.Breed, f => f.PickRandom("Tekir", "Golden", "Labrador", "Van Kedisi"))
                .RuleFor(x => x.OwnerName, f => f.Name.FullName())
                .RuleFor(x => x.TreatmentDescription, f => f.Lorem.Sentence(5))
                .RuleFor(x => x.VisitDate, f => f.Date.Past(1))
                .RuleFor(x => x.IsVaccinated, f => f.Random.Bool());
            var patients = faker.Generate(10);
            await context.Patients.AddRangeAsync(patients);
            await context.SaveChangesAsync();            
        }

    }
}


    app.Run();
