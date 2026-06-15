namespace Appointment_Scheduling_System.Domain.Entities
{
    public class Client
    {
        public int Id { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }

        private Client() { }

        public Client(string firstName, string lastName, string phone, string email)
        {
            SetName(firstName, lastName);
            SetContact(phone, email);
        }

        public void SetName(string first, string last)
        {
            if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last))
                throw new ArgumentException("Invalid name");

            FirstName = first;
            LastName = last;
        }

        public void SetContact(string phone, string email)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone required");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email required");

            PhoneNumber = phone;
            Email = email;
        }
    }
}