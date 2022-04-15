namespace BooksRealm.Test
{
    using BooksRealm.Data;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Data.Sqlite;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class InMemoryDbContext
    {
        private readonly SqliteConnection connection;
        private readonly DbContextOptions<BooksRealmDbContext> dbContextOptions;

        public InMemoryDbContext()
        {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            dbContextOptions = new DbContextOptionsBuilder<BooksRealmDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new BooksRealmDbContext(dbContextOptions);

            context.Database.EnsureCreated();
        }

        public BooksRealmDbContext CreateContext() => new BooksRealmDbContext(dbContextOptions);

        public void Dispose() => connection.Dispose();
    }
}
