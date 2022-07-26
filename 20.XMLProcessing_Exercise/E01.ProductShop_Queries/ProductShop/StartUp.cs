namespace ProductShop
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    using ProductShop.Data;
    using ProductShop.Dtos.Product;
    using ProductShop.Dtos.User;
    using ProductShop.Models;

    public class StartUp
    {
        private static string filePath;

        public static void Main(string[] args)
        {
            ProductShopContext dbContext = new ProductShopContext();

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //Console.WriteLine("Database reset successfully.");

            InitializeInputFilePath("products.xml");
            string xml = File.ReadAllText(filePath);

            string result = ImportProducts(dbContext, xml);
            Console.WriteLine(result);
        }

        //Problem 1 - Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            ImportUserDto[] userDtos = Deserialize<ImportUserDto>("Users", inputXml);

            ICollection<User> users = new List<User>();
            foreach (var uDto in userDtos)
            {
                User user = new User()
                {
                    FirstName = uDto.FirstName,
                    LastName = uDto.LastName,
                    Age = uDto.Age
                };

                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        //Problem 2 - Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            ImportProductDto[] productDtos = Deserialize<ImportProductDto>("Products", inputXml);

            ICollection<Product> products = new List<Product>();
            foreach (var pDto in productDtos)
            {
                Product product = new Product()
                {
                    Name = pDto.Name,
                    Price = pDto.Price,
                    SellerId = pDto.SellerId,
                    BuyerId = pDto.BuyerId
                };

                products.Add(product);
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        private static T[] Deserialize<T>(string rootName, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T[]), xmlRoot);
            using StringReader reader = new StringReader(inputXml);

            return (T[])xmlSerializer.Deserialize(reader);
        }

        private static void InitializeInputFilePath(string fileName)
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Datasets/", fileName);
        }
    }
}