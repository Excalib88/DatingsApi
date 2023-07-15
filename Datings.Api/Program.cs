using Datings.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddControllers();
builder.AddData();
builder.AddSwagger();
builder.AddCors();
builder.AddIdentity();
builder.AddBusinessLogic();
builder.AddLogging();

var app = builder.Build();
await Seed();
app.UseSwaggerApi();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("cors");
app.SetCurrentUser();
app.MapControllers();

app.Run();

async Task Seed()
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.Seed();
}