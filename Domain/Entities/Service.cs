namespace Appointment_Scheduling_System.Domain.Entities
{
    public class Service
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int DurationInMinutes { get; private set; }

        private Service() { } // EF

        public Service(string name, int durationInMinutes, decimal price)
        {
            SetDetails(name, durationInMinutes, price);
        }

        public void SetDetails(string name, int durationInMinutes, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required");
            if (durationInMinutes <= 0)
                throw new ArgumentException("Duration must be positive");
            if (price < 0)
                throw new ArgumentException("Price cannot be negative");

            Name = name;
            DurationInMinutes = durationInMinutes;
            Price = price;
        }
    }
}