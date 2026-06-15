namespace Appointment_Scheduling_System.Domain.Entities
{
    public class Staff
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Position { get; private set; }

        private Staff() { }

        public Staff(string name, string position)
        {
            SetName(name);
            SetPosition(position);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required");

            Name = name;
        }

        public void SetPosition(string position)
        {
            if (string.IsNullOrWhiteSpace(position))
                throw new ArgumentException("Position required");

            Position = position;
        }
    }
}