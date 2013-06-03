using System.Collections.Generic;

namespace MovieRental
{
    public class Customer
    {
        private readonly string name;
        private readonly IList<Rental> rentals = new List<Rental>();

        public Customer(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        public void AddRental(Rental rental)
        {
            rentals.Add(rental);
        }

        public string Statement()
        {
            var totalAmount = 0d;
            var frequentRenterPoints = 0;
            var result = "Rental Record for " + Name + "\n";
            var e = rentals.GetEnumerator();
            while (e.MoveNext())
            {
                var thisAmount = 0d;
                var each = e.Current;

                //determine amounts for each line
                switch (each.Movie.PriceCode)
                {
                    case Movie.REGULAR:
                        thisAmount += 2;
                        if (each.DaysRented > 2)
                        {
                            thisAmount += (each.DaysRented - 2)*1.5;
                        }
                        break;
                    case Movie.NEW_RELEASE:
                        thisAmount += each.DaysRented*3;
                        break;
                    case Movie.CHILDRENS:
                        thisAmount += 1.5;
                        if (each.DaysRented > 3)
                        {
                            thisAmount += (each.DaysRented - 3)*1.5;
                        }
                        break;
                }
                // add frequent renter points
                frequentRenterPoints++;
                // add bouns for a two day new release rental
                if (each.Movie.PriceCode == Movie.NEW_RELEASE && each.DaysRented > 1)
                {
                    frequentRenterPoints++;
                }

                // show figures for this rental
                result += "\t" + each.Movie.Title + "\t" + thisAmount + "\n";
                totalAmount += thisAmount;
            }
            // add footer lines
            result += "Amount owed is " + totalAmount + "\n";
            result += "You earned " + frequentRenterPoints + " frequent renter points";
            return result;
        }
    }
}