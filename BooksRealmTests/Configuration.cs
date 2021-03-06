namespace BooksRealmTests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.IO;


    public class Configuration
    {
        public Configuration()
        {
            var serviceCollection = new ServiceCollection();

            this.ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                     path: "appsettings.json",
                     optional: false,
                     reloadOnChange: true)
               .Build();

            serviceCollection.AddSingleton<IConfiguration>(this.ConfigurationRoot);
        }

        public IConfigurationRoot ConfigurationRoot { get; private set; }
    }
}

