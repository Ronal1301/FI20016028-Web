using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using PP3.Api.Models;
using PP3.Api.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// ---------- Helpers ----------
IResult JsonError(string message) =>
    Results.Json(new { error = message }, statusCode: 400);

IResult OkResult(string original, string @new, bool asXml)
{
    if (!asXml)
        return Results.Json(new { ori = original, @new });

    var dto = new ResultDto { Ori = original, New = @new };

    var ns = new XmlSerializerNamespaces();
    ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
    ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");

    var serializer = new XmlSerializer(typeof(ResultDto));

    using var stringWriter = new StringWriterWithEncoding(Encoding.Unicode); 
    serializer.Serialize(stringWriter, dto, ns);
    var xml = stringWriter.ToString();

    return Results.Text(xml, "application/xml; charset=utf-16");
}

// ---------- ENDPOINTS ----------

// GET /  → Redirect a Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"))
   .WithSummary("Redirect to Swagger UI")
   .WithDescription("Redirige al UI de Swagger para inspeccionar la definición de los endpoints.")
   .Produces(StatusCodes.Status302Found);

// POST /include/{position}
app.MapPost("/include/{position:int}", (
    [FromRoute] int position,
    [FromQuery] string? value,
    [FromForm] string? text,
    [FromHeader(Name = "xml")] bool? xml 
) =>
{
    if (position < 0)
        return JsonError("'position' must be 0 or higher");

    if (string.IsNullOrWhiteSpace(value))
        return JsonError("'value' cannot be empty");

    if (string.IsNullOrWhiteSpace(text))
        return JsonError("'text' cannot be empty");

    var tokens = WordsHelper.SplitWords(text!);

    if (position >= tokens.Count)
        tokens.Add(value!);
    else
        tokens.Insert(position, value!);

    var result = string.Join(" ", tokens);
    return OkResult(text!, result, xml ?? false);
})
.WithSummary("Incluye una palabra en una posición de la oración.")
.WithDescription("position (route, >=0), value (query, !empty), text (form, !empty), xml (header opcional).")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.DisableAntiforgery(); 

// PUT /replace/{length}
app.MapPut("/replace/{length:int}", (
    [FromRoute] int length,
    [FromQuery] string? value,
    [FromForm] string? text,
    [FromHeader(Name = "xml")] bool? xml
) =>
{
    if (length <= 0)
        return JsonError("'length' must be greater than 0");

    if (string.IsNullOrWhiteSpace(value))
        return JsonError("'value' cannot be empty");

    if (string.IsNullOrWhiteSpace(text))
        return JsonError("'text' cannot be empty");

    var tokens = WordsHelper.SplitWords(text!);
    var replaced = tokens.Select(t =>
    {
        var (prefix, core, suffix) = WordsHelper.AnalyzeToken(t);
        if (core.Length == length)
            return prefix + value + suffix;
        return t;
    });

    var result = string.Join(" ", replaced);
    return OkResult(text!, result, xml ?? false);
})
.WithSummary("Reemplaza palabras de una longitud específica.")
.WithDescription("length (route, >0), value (query, !empty), text (form, !empty), xml (header opcional).")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.DisableAntiforgery(); 

// DELETE /erase/{length}
app.MapDelete("/erase/{length:int}", (
    [FromRoute] int length,
    [FromForm] string? text,
    [FromHeader(Name = "xml")] bool? xml
) =>
{
    if (length <= 0)
        return JsonError("'length' must be greater than 0");

    if (string.IsNullOrWhiteSpace(text))
        return JsonError("'text' cannot be empty");

    var tokens = WordsHelper.SplitWords(text!);
    var filtered = tokens.Where(t =>
    {
        var (_, core, _) = WordsHelper.AnalyzeToken(t);
        return core.Length != length;
    });

    var result = string.Join(" ", filtered);
    return OkResult(text!, result, xml ?? false);
})
.WithSummary("Elimina palabras de una longitud específica.")
.WithDescription("length (route, >0), text (form, !empty), xml (header opcional).")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.DisableAntiforgery(); 

app.Run();

public sealed class StringWriterWithEncoding : StringWriter
{
    private readonly Encoding _encoding;
    public StringWriterWithEncoding(Encoding encoding) => _encoding = encoding;
    public override Encoding Encoding => _encoding;
}
