using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CP1 WebApi", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();


var list = new List<object>();

app.MapGet("/", () => Results.Redirect("/swagger"));
//ChatGPT
app.MapPost("/", ([FromHeader(Name = "xml")] bool? xml) =>
{
    if (xml == true)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<Items>");
        foreach (var item in list)
        {
            if (item is int i)
                sb.AppendLine($"  <Item type=\"int\">{i}</Item>");
            else if (item is float f)
                sb.AppendLine($"  <Item type=\"float\">{f}</Item>");
        }
        sb.AppendLine("</Items>");
        return Results.Text(sb.ToString(), "application/xml", Encoding.UTF8);
    }

    return Results.Json(list);
});

app.MapPut("/", ([FromForm] int? quantity, [FromForm] string? type) =>
{
    if (quantity is null || quantity <= 0)
        return Results.BadRequest(new { error = "'quantity' must be higher than zero" });

    if (string.IsNullOrWhiteSpace(type))
        return Results.BadRequest(new { error = "'type' is required and must be 'int' or 'float'" });

    var t = type.Trim().ToLowerInvariant();
    if (t != "int" && t != "float")
        return Results.BadRequest(new { error = "'type' must be 'int' or 'float'" });

    var rnd = Random.Shared;

    for (int i = 0; i < quantity; i++)
    {
        if (t == "int")
        {
            // Entero aleatorio
            list.Add(rnd.Next(int.MinValue, int.MaxValue));
        }
        else
        {
            // Flotante (float) aleatorio en [0,1)
            list.Add(rnd.NextSingle());
        }
    }

    return Results.Ok(new { added = quantity, type = t, count = list.Count });
}).DisableAntiforgery();

app.MapDelete("/", ([FromForm] int? quantity) =>
{
    if (quantity is null || quantity <= 0)
        return Results.BadRequest(new { error = "'quantity' must be higher than zero" });

    if (list.Count < quantity)
        return Results.BadRequest(new { error = "Not enough elements to delete" });

    list.RemoveRange(0, quantity.Value);
    return Results.Ok(new { deleted = quantity, count = list.Count });
}).DisableAntiforgery();

app.MapPatch("/", () =>
{
    list.Clear();
    return Results.Ok(new { cleared = true, count = list.Count });
});

app.Run();
