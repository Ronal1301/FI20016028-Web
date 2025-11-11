using System.Globalization;
using System.Text;
using BooksApp.EFData;
using BooksApp.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = Encoding.UTF8;

using var db = new AppDbContext();

Console.WriteLine("Comprobando migraciones...");
await db.Database.MigrateAsync();

var isEmpty = !await db.Authors.AnyAsync()
              && !await db.Titles.AnyAsync()
              && !await db.Tags.AnyAsync();

if (isEmpty)
{
    Console.WriteLine();
    Console.WriteLine("La base de datos está vacía, por lo que será llenada a partir de los datos del archivo CSV.");
    Console.WriteLine("Procesando...");

    var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../"));
    var csvPath = Path.Combine(root, "data", "books.csv");
    if (!File.Exists(csvPath))
    {
        Console.WriteLine($"ERROR: No se encontró {csvPath}. Coloca books.csv en la carpeta data.");
        return;
    }

    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        Delimiter = ",",
        BadDataFound = null,
        Encoding = Encoding.UTF8,
        TrimOptions = TrimOptions.Trim
    };

    var authorsByName = new Dictionary<string, Author>(StringComparer.OrdinalIgnoreCase);
    var tagsByName = new Dictionary<string, Tag>(StringComparer.OrdinalIgnoreCase);

    using var reader = new StreamReader(csvPath, Encoding.UTF8);
    using var csv = new CsvReader(reader, config);
    // csv.Context.RegisterClassMap<BookCsvRowMap>(); // opcional

    var records = csv.GetRecords<BookCsvRow>();
    foreach (var row in records)
    {
        if (string.IsNullOrWhiteSpace(row.Author) || string.IsNullOrWhiteSpace(row.Title))
            continue;

        if (!authorsByName.TryGetValue(row.Author, out var author))
        {
            author = new Author { AuthorName = row.Author.Trim() };
            authorsByName[row.Author] = author;
            db.Authors.Add(author);
        }

        var title = new Title
        {
            Author = author,
            TitleName = row.Title.Trim()
        };
        db.Titles.Add(title);

        if (!string.IsNullOrWhiteSpace(row.Tags))
        {
            var parts = row.Tags.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var tagName in parts)
            {
                if (!tagsByName.TryGetValue(tagName, out var tag))
                {
                    tag = new Tag { TagName = tagName };
                    tagsByName[tagName] = tag;
                    db.Tags.Add(tag);
                }
                db.TitlesTags.Add(new TitleTag { Title = title, Tag = tagsByName[tagName] });
            }
        }
    }

    await db.SaveChangesAsync();
    Console.WriteLine("Listo.");
    return;
}

Console.WriteLine();
Console.WriteLine("La base de datos se está leyendo para crear los archivos TSV.");
Console.WriteLine("Procesando...");

var projectRoot2 = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../"));
var dataDir = Path.Combine(projectRoot2, "data");
Directory.CreateDirectory(dataDir);

var titles = await db.Titles
    .AsNoTracking()
    .Include(t => t.Author)
    .Include(t => t.TitlesTags).ThenInclude(tt => tt.Tag)
    .ToListAsync();

var rows = new List<RowOut>();
foreach (var t in titles)
{
    if (t.TitlesTags.Count == 0)
    {
        rows.Add(new RowOut(t.Author.AuthorName, t.TitleName, ""));
        continue;
    }

    foreach (var tt in t.TitlesTags)
        rows.Add(new RowOut(t.Author.AuthorName, t.TitleName, tt.Tag.TagName));
}

char FL(string s) => string.IsNullOrEmpty(s) ? '#' : char.ToUpperInvariant(s[0]);
char First(string s) => string.IsNullOrEmpty(s) ? '#' : char.ToUpperInvariant(s[0]);

var groups = rows.GroupBy(r => FL(r.AuthorName));

foreach (var g in groups)
{
    var path = Path.Combine(dataDir, $"{g.Key}.tsv");

    var ordered = g
        .OrderByDescending(r => First(r.AuthorName))
        .ThenByDescending(r => First(r.TitleName))
        .ThenByDescending(r => First(r.TagName))
        .ToList();

    using var sw = new StreamWriter(path, false, new UTF8Encoding(false));
    await sw.WriteLineAsync("AuthorName\tTitleName\tTagName");
    foreach (var r in ordered)
        await sw.WriteLineAsync($"{r.AuthorName}\t{r.TitleName}\t{r.TagName}");
}

Console.WriteLine("Listo.");

public record BookCsvRow(string Author, string Title, string Tags);
public record RowOut(string AuthorName, string TitleName, string TagName);
