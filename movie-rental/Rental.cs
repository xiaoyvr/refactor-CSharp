namespace MovieRental
{
    public class Rental
    {
        private readonly Movie movie;
        private readonly int daysRented;

        public Rental(Movie movie, int daysRented)
        {
            this.movie = movie;
            this.daysRented = daysRented;
        }

        public int DaysRented
        {
            get { return daysRented; }
        }

        public Movie Movie
        {
            get { return movie; }
        }
    }
}