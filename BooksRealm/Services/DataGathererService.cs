using AngleSharp;
using BooksRealm.Data.Common.Repositories;
using BooksRealm.Data.Models;
using Services.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BooksRealm.Services
{
    public class DataGathererService:IDataGathererService
    {
        private const string BaseUrl = "https://www.bookbrowse.com/reviews/index.cfm/book_number/{0}/#media_reviews";

        private readonly IBrowsingContext context;
        private readonly IDeletableEntityRepository<Genre> genreRepository;
        private readonly IDeletableEntityRepository<Author> authorRepository;
        private readonly IDeletableEntityRepository<Book> bookRepository;
        private readonly IRepository<AuthorBook> authorBookRepository;
        private readonly IRepository<BookGenre> bookGenreRepository;

        private readonly IRepository<Review> reviewRepository;
        
        public DataGathererService(
        IDeletableEntityRepository<Genre> genreRepository,
       IDeletableEntityRepository<Author> authorRepository,
         IDeletableEntityRepository<Book> bookRepository,
         IRepository<AuthorBook> authorBookRepository,
       IRepository<BookGenre> bookGenreRepository,
        IRepository<Review> reviewRepository
        )
        {
            this.genreRepository = genreRepository;
            this.authorRepository = authorRepository;
            this.bookRepository = bookRepository;
            this.reviewRepository = reviewRepository;
           this.authorBookRepository=authorBookRepository;
        this.bookGenreRepository=bookGenreRepository;

        var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
        }
        //4413
        public async Task ImportBooksAsync(int fromId = 1, int toId = 100)
        {
            var concurrentBag = this.ScrapeRecipes(fromId, toId);
            Console.WriteLine($"Scraped recipes: {concurrentBag.Count}");

            int addedCount = 0;
              foreach (var book in concurrentBag)
                {
                    var newBook = new Book
                    {
                        CoverUrl = book.CoverUrl,
                        DateOfPublish = book.DateOfPublish,
                        Description = book.Description,
                        Rating = book.Rating,
                        Title = book.Title,
                    };
                    await this.bookRepository.AddAsync(newBook);
                await this.bookRepository.SaveChangesAsync();
                    var authors = book.Authors.ToList();

                    foreach (var author in authors)
                    {
                        var authorId = await this.GetOrCreateAuthorAsync(author.Trim());

                        var bookAuthor = new AuthorBook
                        {
                            AuthorId = authorId,
                            BookId = newBook.Id,
                        };
                        await this.authorBookRepository.AddAsync(bookAuthor);
                    }
                    var genres = book.Genres.ToList();
                    foreach (var genre in genres)
                    {
                        var genreId = await this.GetOrCreateGenreAsync(genre.Trim());

                        var bookGenre = new BookGenre
                        {
                            GenreId = genreId,
                            BookId = newBook.Id,

                        };
                        await this.bookGenreRepository.AddAsync(bookGenre);
                    }



                    var reviews = book.Reviews.ToList();
                    foreach (var review in reviews)
                    {
                        var rev = new Review
                        {
                            BookId = newBook.Id,
                            Text = review,

                        };
                        await this.reviewRepository.AddAsync(rev);

                    }


                    if (++addedCount % 100 == 0)
                    {

                        await this.bookRepository.SaveChangesAsync();
                        Console.WriteLine($"Saved count: {addedCount}");
                        Thread.Sleep(5000);
                    }
                }

                await this.bookRepository.SaveChangesAsync();
                await this.authorBookRepository.SaveChangesAsync();
                await this.bookGenreRepository.SaveChangesAsync();
            await this.bookRepository.SaveChangesAsync();

            Console.WriteLine($"Count: {addedCount}");
        }

        private ConcurrentBag<BookDto> ScrapeRecipes(int fromId, int toId)
        {
            var concurrentBag = new ConcurrentBag<BookDto>();
            Parallel.For(fromId, toId + 1, i =>
            {
                try
                {
                    var recipe = this.GetRecipe(i);
                    concurrentBag.Add(recipe);
                }
                catch
                {
                    // ignored
                }
            });
            return concurrentBag;
        }

        private BookDto GetRecipe(int id)
        {
            var url = string.Format(BaseUrl, id);

            var document = this.context
                .OpenAsync(url)
                .GetAwaiter()
                .GetResult();

            if (document.StatusCode == HttpStatusCode.NotFound ||
                document.DocumentElement.OuterHtml.Contains("Book Review Not Found"))
            {
                Console.WriteLine("Not Found");
                throw new Exception();
            }

            var book = new BookDto();
            var rating = document.QuerySelector(".rating > div").Id;
           
            if (rating.Contains("fivestar")||rating.StartsWith("fivestar")||rating.EndsWith("fivestar"))
            {
               book.Rating = 5.0;
            }
            if (rating.Contains("threestarh") || rating.StartsWith("threestarh") || rating.EndsWith("threestarh"))
            {
                book.Rating = 3.0;
            }
            if (rating.Contains("fourstarh") || rating.StartsWith("fourstarh") || rating.EndsWith("fourstarh"))
            {
                book.Rating = 4.0;
            }
            if (rating.Contains("twostar") || rating.StartsWith("twostar") || rating.EndsWith("twostar"))
            {
                book.Rating = 2.0;
            }
            else
            {
                book.Rating = 5.0;
            }
            //title
            var title = document
                .QuerySelector(".title")
                .TextContent
                .Trim();
            book.Title = title.Trim();

            // Get authors
            var authors = document.QuerySelectorAll(".desc > p > a")
                .Select(x => x.TextContent)
                .ToList();

            book.Authors.AddRange(authors);

            // Get image url
            book.CoverUrl = "https://www.bookbrowse.com" + document.QuerySelector(".jacket").GetAttribute("src");
            

            //description
            var description  = document
                .QuerySelectorAll("#summary > p")
                .Select(x => x.TextContent)
                .ToList();
            StringBuilder sb= new StringBuilder();
            foreach (var desc in description)
            {
                sb.AppendLine(desc);
            }
            book.Description= sb.ToString();
            // Get reviews
           
            var reviews = document.QuerySelector("#media_reviews");

            var text = reviews.TextContent;
            var repl = Regex.Replace(text, @"\s{2,}", "  ");

            var list = repl.Split("  ").Select(x => x.Trim()).Where(x => x.Length > 20).ToList();

            book.Reviews.AddRange(list);
            //genres
            var listgenres = new List<string>();
            var genres = document.QuerySelectorAll("#gBox_window > ul >li").Select(x => x.TextContent)
                .ToList();
            book.Genres.AddRange(genres);

            //date
            var date =document.QuerySelector("body > div.container > div > div.left_column > div.top_block.book_block > div > figure > figcaption > ul > li:nth-child(2) > p:nth-child(1)");
            var items = Regex.Match(date.TextContent, @"[A-Z][a-z]{2}\s[0-9]{4}");
            var datestring = "1 " + items;
            var dateconv = DateTime.Parse(datestring, new CultureInfo("bg-Bg"),
                                DateTimeStyles.NoCurrentDateDefault);
            book.DateOfPublish = dateconv;
            Console.WriteLine(id);
            return book;
        }

        private async Task<int> GetOrCreateAuthorAsync(string name)
        {
            var author = this.authorRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == name);

            if (author != null)
            {
                return author.Id;
            }

            author = new Author
            {
                Name = name,
            };   

            await this.authorRepository.AddAsync(author);
            await this.authorRepository.SaveChangesAsync();

            return author.Id;
        }

        private async Task<int> GetOrCreateGenreAsync(string categoryName)
        {
            var category = this.genreRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == categoryName);

            if (category != null)
            {
                return category.Id;
            }

            category = new Genre
            {
                Name = categoryName,
            };

            await this.genreRepository.AddAsync(category);
            await this.genreRepository.SaveChangesAsync();

            return category.Id;
        }
    }
}
