using System;
using System.Text;

namespace Appointment_Scheduling_System.ConsoleUI.Helpers
{
    /// <summary>
    /// Интерактивен избор на дата и час в конзолата.
    ///
    /// Управление:
    ///   Стрелки      - местене по дни / промяна на час и минута
    ///   PageUp/Down  - предишен / следващ месец
    ///   Tab или Enter- преминаване от календар към час
    ///   Enter (в час)- потвърждаване на избора
    ///   Esc          - отказ (връща null)
    /// </summary>
    public static class ConsoleDateTimePicker
    {
        private enum Step { Date, Time }

        /// <summary>
        /// Избор на единична дата+час.
        /// </summary>
        /// <param name="title">Заглавие, показва се на първия ред (напр. "НАЧАЛО НА ЧАСА").</param>
        /// <param name="initial">От коя дата/час да тръгне курсорът (по подразбиране - сега).</param>
        /// <param name="minDate">Ако е зададено, не позволява навигация преди тази дата.</param>
        /// <param name="contextLabel">Допълнителен ред под заглавието, напр. "Начало: 16.07.2026 14:30".</param>
        /// <param name="includeTime">Ако е false, спира до избора на ден - няма стъпка за час (за PickDate).</param>
        public static DateTime? Pick(string title, DateTime? initial = null, DateTime? minDate = null, string contextLabel = null, bool includeTime = true)
        {
            DateTime date = (initial ?? DateTime.Now).Date;
            int hour = includeTime ? (initial ?? DateTime.Now).Hour : 0;
            int minute = includeTime ? (initial ?? DateTime.Now).Minute : 0;

            var step = Step.Date;
            bool editingHour = true;

            while (true)
            {
                Draw(title, contextLabel, date, hour, minute, step, editingHour, includeTime);
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
                            if (includeTime)
                            {
                                step = Step.Time;
                            }
                            else
                            {
                                if (minDate.HasValue && newDate < minDate.Value.Date)
                                    newDate = minDate.Value.Date;
                                return newDate; // date-only режим - Enter веднага потвърждава деня
                            }
                            break;
                    }

                    if (minDate.HasValue && newDate < minDate.Value.Date)
                        newDate = minDate.Value.Date; // не позволява да се мине преди минималната дата

                    date = newDate;
                }
                else // Step.Time
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

        /// <summary>
        /// Избор на период (начало + край) с ЕДИН извикване.
        /// - Краят автоматично тръгва от датата на началото (не от януари).
        /// - Показва "Начало: ..." докато избираш края, за контекст.
        /// - Не позволява край преди/равен на началото - ако стане, пита отново само за края.
        /// Връща null ако потребителят откаже (Esc) на някоя от двете стъпки.
        /// </summary>
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

        /// <summary>
        /// Избор само на дата (без час) - за отчети, филтри и т.н., където часът няма значение.
        /// </summary>
        public static DateTime? PickDate(string title, DateTime? initial = null, DateTime? minDate = null, string contextLabel = null)
            => Pick(title, initial, minDate, contextLabel, includeTime: false);

        /// <summary>
        /// Избор на период от дати (начало + край), без час - напр. за отчети.
        /// Краят тръгва от датата на началото и не позволява край преди началото
        /// (но позволява край = начало, т.е. период от 1 ден).
        /// </summary>
        public static (DateTime Start, DateTime End)? PickDateRange(
            string startTitle = "НАЧАЛНА ДАТА",
            string endTitle = "КРАЙНА ДАТА",
            DateTime? initialStart = null)
        {
            var start = PickDate(startTitle, initialStart);
            if (start == null) return null;

            while (true)
            {
                string context = $"Начало: {start.Value:dd.MM.yyyy}";
                var end = PickDate(endTitle, start, minDate: start.Value.Date, contextLabel: context);
                if (end == null) return null;

                if (end.Value >= start.Value)
                    return (start.Value, end.Value);

                Console.WriteLine("\nКраят трябва да е след или равен на началото. Натисни клавиш, за да опиташ пак...");
                Console.ReadKey(true);
            }
        }

        private static void Draw(string title, string contextLabel, DateTime cursor, int hour, int minute, Step step, bool editingHour, bool includeTime)
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

            if (includeTime)
            {
                string hourStr = step == Step.Time && editingHour ? $"[{hour:00}]" : $" {hour:00} ";
                string minStr = step == Step.Time && !editingHour ? $"[{minute:00}]" : $" {minute:00} ";
                Console.WriteLine($"Час:   {hourStr}:{minStr}");
                Console.WriteLine();

                Console.WriteLine(step == Step.Date
                    ? "Стрелки = ден | PgUp/PgDn = месец | Enter/Tab = към часа | Esc = отказ"
                    : "←/→ = час/минута | ↑/↓ = промяна | Tab = назад към дата | Enter = потвърди | Esc = отказ");
            }
            else
            {
                Console.WriteLine("Стрелки = ден | PgUp/PgDn = месец | Enter = потвърди | Esc = отказ");
            }
        }
    }
}