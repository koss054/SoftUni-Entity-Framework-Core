namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            //string command = Console.ReadLine();
            //int year = int.Parse(Console.ReadLine());
            string input = Console.ReadLine();
            //string date = Console.ReadLine();

            //Console.WriteLine(GetBooksByAgeRestriction(db, command));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetBooksByPrice(db));
            //Console.WriteLine(GetBooksNotReleasedIn(db, year));
            //Console.WriteLine(GetBooksByCategory(db, input));
            //Console.WriteLine(GetBooksReleasedBefore(db, date));
            //Console.WriteLine(GetAuthorNamesEndingIn(db, input));
            //Console.WriteLine(GetBookTitlesContaining(db, input));
            Console.WriteLine(GetBooksByAuthor(db, input));
        }

        //Exercise 2
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            bool parseSuccess 
                = Enum.TryParse(command, true, out AgeRestriction ageRestriction);

            if (!parseSuccess)
            {
                return String.Empty;
            }

            string[] bookTitles = context
                .Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return String.Join(Environment.NewLine, bookTitles);
        }

        //Exercise 3
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();
            EditionType goldenEdition = (EditionType)2;

            var goldenBookTitles = context
                .Books
                .Where(b => b.EditionType == goldenEdition
                         && b.Copies < 5000)
                .Select(b => new
                {
                    BookId = b.BookId,
                    BookTitle = b.Title
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            foreach(var b in goldenBookTitles)
            {
                output.AppendLine($"{b.BookTitle}");
            }

            return output.ToString().TrimEnd();
        }

        //Exercise 4
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();

            var booksWithPrice = context
                .Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookPrice = b.Price
                })
                .OrderByDescending(b => b.BookPrice)
                .ToArray();

            foreach(var b in booksWithPrice)
            {
                output.AppendLine($"{b.BookTitle} - ${b.BookPrice:F2}");
            }

            return output.ToString().TrimEnd();
        }

        //Exercise 5
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder output = new StringBuilder();

            var bookTitles = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    BookId = b.BookId,
                    BookTitle = b.Title
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            foreach(var b in bookTitles)
            {
                output.AppendLine($"{b.BookTitle}");
            }

            return output.ToString().TrimEnd();
        }

        //Exercise 6
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .ToLower()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string[] books = context
                .Books
                .Where(b => b.BookCategories
                    .Any(c => categories.Contains(c.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return String.Join(Environment.NewLine, books);
        }

        //Exercise 7
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder output = new StringBuilder();
            DateTime parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context
                .Books
                .Where(b => b.ReleaseDate < parsedDate)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookEdition = b.EditionType,
                    BookPrice = b.Price,
                    BookReleaseDate = b.ReleaseDate
                })
                .OrderByDescending(b => b.BookReleaseDate)
                .ToArray();

            foreach(var b in books)
            {
                output.AppendLine($"{b.BookTitle} - {b.BookEdition} - ${b.BookPrice:F2}");
            }

            return output.ToString().TrimEnd();
        }

        //Exercise 8
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorNames = context
                .Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray()
                .OrderBy(a => a)
                .ToArray();

            return String.Join(Environment.NewLine, authorNames);
        }

        //Exercise 9
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var bookTitles = context
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return String.Join(Environment.NewLine, bookTitles);
        }

        //Exercise 10
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder output = new StringBuilder();

            var booksAndAuthors = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(b => new
                {
                    BookId = b.BookId,
                    BookTitle = b.Title,
                    AuthorFullName = $"{b.Author.FirstName} {b.Author.LastName}"
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            foreach(var b in booksAndAuthors)
            {
                output.AppendLine($"{b.BookTitle} ({b.AuthorFullName})");
            }

            return output.ToString().TrimEnd();
        }
    }
}
