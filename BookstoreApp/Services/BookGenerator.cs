using System;
using System.Collections.Generic;
using Bogus;
using Bogus.DataSets;
using BookstoreApp.Models;

namespace BookstoreApp.Services
{
    public class BookGenerator
    {
        private readonly Faker _faker;

        public BookGenerator(int seed, string locale)
        {
            _faker = new Faker(locale);
            Randomizer.Seed = new Random(seed);
        }

        public List<Book> GenerateBooks(int page, int count, double avgLikes, double avgReviews)
        {
            var bookFaker = CreateFaker(page, count, avgLikes, avgReviews);
            return bookFaker.Generate(count);
        }

        private Faker<Book> CreateFaker(int page, int count, double avgLikes, double avgReviews)
        {
            return new Faker<Book>(_faker.Locale)
                .RuleFor(b => b.Index, (f, b) => (page - 1) * count + f.IndexVariable++)
                .RuleFor(b => b.ISBN, f => f.Random.Long(1000000000L, 9999999999L).ToString())
                .RuleFor(b => b.Title, f => string.Join(" ", [f.Commerce.ProductAdjective(), f.Address.StreetName()]))
                .RuleFor(b => b.Author, f => f.Name.FullName())
                .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
                .RuleFor(b => b.Likes, f => f.Random.Int((int)(avgLikes * 0.8), (int)(avgLikes * 1.2)))
                .RuleFor(b => b.Reviews, f => CalculateReviews(avgReviews, f))
                .RuleFor(b => b.CoverImageUrl, f => $"https://picsum.photos/seed/{f.Random.Guid()}/200/300")
                .RuleFor(b => b.Pages, f => f.Random.Number(100, 800))
                .RuleFor(b => b.Comments, f => GenerateRandomComments(f));
        }

        private List<string> GenerateRandomComments(Faker f)
        {
            var comments = new List<string>();
            for (int i = 0; i < f.Random.Number(4, 5); i++)
            {
                comments.Add(f.Rant.Review("book"));
            }
            return comments;
        }

        private int CalculateReviews(double avgReviews, Faker f)
        {
            if (avgReviews == 0)
            {
                return 0;
            }
            else if (avgReviews % 1 != 0)
            {
                int fullReviews = (int)avgReviews;
                double fractionalPart = avgReviews - fullReviews;

                return fullReviews + (f.Random.Double() < fractionalPart ? 1 : 0);
            }
            else
            {
                return (int)avgReviews;
            }
        }
    }
}
