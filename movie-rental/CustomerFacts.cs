using System;
using System.Text.RegularExpressions;
using Xunit;
using FluentAssertions;
using Xunit.Extensions;

namespace MovieRental
{
    public class CustomerFacts
    {
        private static readonly Regex Patten = new Regex(@"Rental Record for (?<name>.*)\n(?:\t(?<title>.*)\t(?<sigleAmount>.*)\n)*Amount owed is (?<amount>.*)\nYou earned (?<points>\d+) frequent renter points");
        [Theory]
        [InlineData("Spider Man", Movie.REGULAR, 2, "2", "1")]
        [InlineData("Spider Man", Movie.REGULAR, 4, "5", "1")]
        [InlineData("Iron Man 3", Movie.NEW_RELEASE, 1, "3", "1")]
        [InlineData("Iron Man 3", Movie.NEW_RELEASE, 2, "6", "2")]
        [InlineData("Ice Age", Movie.CHILDRENS, 3, "1.5", "1")]
        [InlineData("Ice Age", Movie.CHILDRENS, 4, "3", "1")]
        public void should_cal_amount_for_one_rent(string name, int type, int days, string amount, string points)
        {
            var john = new Customer("John");
            john.AddRental(new Rental(new Movie(name, type), days));
            var match = Patten.Match(john.Statement());
            Console.WriteLine(john.Statement());
            match.Groups["name"].Captures[0].Value.Should().Be("John");

            match.Groups["amount"].Captures[0].Value.Should().Be(amount);
            match.Groups["points"].Captures[0].Value.Should().Be(points);

            match.Groups["title"].Captures[0].Value.Should().Be(name);
            match.Groups["sigleAmount"].Captures[0].Value.Should().Be(amount);
        }

        [Fact]
        public void should_get_statement_after_rent_more_movies()
        {
            var john = new Customer("John");
            john.AddRental(new Rental(new Movie("Iron Man 3", Movie.NEW_RELEASE), 5));
            john.AddRental(new Rental(new Movie("Spider Man", Movie.REGULAR), 5));
            john.AddRental(new Rental(new Movie("Ice Age 4", Movie.CHILDRENS), 3));
            Console.WriteLine(john.Statement());
            var match = Patten.Match(john.Statement());

            match.Groups["name"].Captures[0].Value.Should().Be("John");
            match.Groups["amount"].Captures[0].Value.Should().Be("23");
            match.Groups["points"].Captures[0].Value.Should().Be("4");

            match.Groups["title"].Captures[0].Value.Should().Be("Iron Man 3");
            match.Groups["title"].Captures[1].Value.Should().Be("Spider Man");
            match.Groups["title"].Captures[2].Value.Should().Be("Ice Age 4");
            match.Groups["sigleAmount"].Captures[0].Value.Should().Be("15");
            match.Groups["sigleAmount"].Captures[1].Value.Should().Be("6.5");
            match.Groups["sigleAmount"].Captures[2].Value.Should().Be("1.5");

        }
    }
}