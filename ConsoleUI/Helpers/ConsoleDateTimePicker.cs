using System;
using System.Text;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
   
    public static class ConsoleDateTimePicker
    {
        private enum Step { Date, Time }

        public static DateTime? Pick(string title, DateTime? initial = null, DateTime? minDate = null, string contextLabel = null)
        {
            DateTime date = (initial ?? DateTime.Now).Date;
            int hour = (initial ?? DateTime.Now).Hour;
            int minute = (initial ?? DateTime.Now).Minute;

            var step = Step.Date;
            bool editingHour = true;

            while (true)
            {
                Draw(title, contextLabel, date, hour, minute, step, editingHour);
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Escape)
                    return null;

                if (step == Step.Date)
                {
                    DateTime newDate = date;
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow: newDate = date.AddDays(-1); break;
                        case ConsoleKey.RightArrow: newDate = date.AddDays(1); break;
                        case ConsoleKey.UpArrow: newDate = date.AddDays(-7); break;
                        case ConsoleKey.DownArrow: newDate = date.AddDays(7); break;
                        case ConsoleKey.PageUp: newDate = date.AddMonths(-1); break;
                        case ConsoleKey.PageDown: newDate = date.AddMonths(1); break;
                        case ConsoleKey.Tab:
                        case ConsoleKey.Enter:
                            step = Step.Time;
                            break;
                    }

                    if (minDate.HasValue && newDate < minDate.Value.Date)
                        newDate = minDate.Value.Date;

                    date = newDate;
                }
                else
                {
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.RightArrow:
                            editingHour = !editingHour;
                            break;
                        case ConsoleKey.UpArrow:
                            if (editingHour) hour = (hour + 1) % 24;
                            else minute = (minute + 5) % 60;
                            break;
                        case ConsoleKey.DownArrow:
                            if (editingHour) hour = (hour + 23) % 24;
                            else minute = (minute + 55) % 60;
                            break;
                        case ConsoleKey.Tab:
                            step = Step.Date;
                            break;
                        case ConsoleKey.Enter:
                            return date.AddHours(hour).AddMinutes(minute);
                    }
                }
            }
        }
        public static (DateTime Start, DateTime End)? PickRange(
            string startTitle = "НАЧАЛО",
            string endTitle = "КРАЙ",
            DateTime? initialStart = null)
        {
            var start = Pick(startTitle, initialStart);
            if (start == null) return null;

            while (true)
            {
                string context = $"Начало: {start.Value:dd.MM.yyyy HH:mm}";
                var end = Pick(endTitle, start, minDate: start.Value.Date, contextLabel: context);
                if (end == null) return null;

                if (end.Value > start.Value)
                    return (start.Value, end.Value);

                Console.WriteLine("\nКраят трябва да е след началото. Натисни клавиш, за да опиташ пак...");
                Console.ReadKey(true);
            }
        }

        private static void Draw(string title, string contextLabel, DateTime cursor, int hour, int minute, Step step, bool editingHour)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine(new string('=', title.Length));
            if (!string.IsNullOrEmpty(contextLabel))
                Console.WriteLine(contextLabel);
            Console.WriteLine();

            var firstOfMonth = new DateTime(cursor.Year, cursor.Month, 1);
            Console.WriteLine($"   {firstOfMonth:MMMM yyyy}".ToUpper());
            Console.WriteLine(" Mo Tu We Th Fr Sa Su");

            int daysInMonth = DateTime.DaysInMonth(cursor.Year, cursor.Month);
            int startOffset = ((int)firstOfMonth.DayOfWeek + 6) % 7; // Monday = 0

            var sb = new StringBuilder();
            for (int i = 0; i < startOffset; i++)
                sb.Append("   ");

            for (int day = 1; day <= daysInMonth; day++)
            {
                string dayStr = day.ToString().PadLeft(2);
                bool isSelected = step == Step.Date && day == cursor.Day;
                sb.Append(isSelected ? $"[{dayStr}]" : $" {dayStr} ");

                if ((startOffset + day) % 7 == 0)
                {
                    Console.WriteLine(sb.ToString());
                    sb.Clear();
                }
            }
            if (sb.Length > 0)
                Console.WriteLine(sb.ToString());

            Console.WriteLine();
            string hourStr = step == Step.Time && editingHour ? $"[{hour:00}]" : $" {hour:00} ";
            string minStr = step == Step.Time && !editingHour ? $"[{minute:00}]" : $" {minute:00} ";
            Console.WriteLine($"Час:   {hourStr}:{minStr}");
            Console.WriteLine();

            Console.WriteLine(step == Step.Date
                ? "Стрелки = ден | PgUp/PgDn = месец | Enter/Tab = към часа | Esc = отказ"
                : "←/→ = час/минута | ↑/↓ = промяна | Tab = назад към дата | Enter = потвърди | Esc = отказ");
        }
    }
}