using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Collections.Generic;
using System.IO;

namespace chapter12
{
    public class MyDbContext : DbContext, IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {
        }

        public MyDbContext()
        {

        }

        public MyDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var options = new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .Options;

            return new MyDbContext(options);
        }
    }
}