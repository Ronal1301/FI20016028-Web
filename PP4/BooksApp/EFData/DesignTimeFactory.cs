using Microsoft.EntityFrameworkCore.Design;

namespace BooksApp.EFData;

public class DesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args) => new AppDbContext();
}
