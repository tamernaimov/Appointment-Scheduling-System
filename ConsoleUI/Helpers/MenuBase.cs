using System;
using System.Collections.Generic;
using System.Linq;

namespace Appointment_Scheduling_System.ConsoleUI.Menus
{
    /// <summary>
    /// Обща база за всички менюта.
    /// Маха повтарящите се Header/Success/Error/Pause/ReadInt/ReadDate/ReadEnum,
    /// и дава подравнена таблица за списъци (Clients/Staff/Services...).
    ///
    /// Употреба: наследи от MenuBase вместо нищо, и (по желание) override-ни AccentColor.
    /// </summary>
    public abstract class MenuBase
    {
        /// <summary>Цвят на рамката/акцента за конкретното меню.</summary>
        protected virtual ConsoleColor AccentColor => ConsoleColor.Cyan;

        protected void Header(string title)
        {
            int width = Math.Max(34, title.Length + 8);
            string top = "┌" + new string('─', width - 2) + "┐";
            string bottom = "└" + new string('─', width - 2) + "┘";
            int pad = (width - 2 - title.Length) / 2;
            string mid = "│" + new string(' ', pad) + title +
                         new string(' ', width - 2 - pad - title.Length) + "│";

            Console.ForegroundColor = AccentColor;
            Console.WriteLine(top);
            Console.WriteLine(mid);
            Console.WriteLine(bottom);
            Console.ResetColor();
            Console.WriteLine();
        }

        protected void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ {msg}");
            Console.ResetColor();
            Pause();
        }

        protected void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✗ {msg}");
            Console.ResetColor();
            Pause();
        }

        protected void Warning(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠ {msg}");
            Console.ResetColor();
        }

        protected void Pause()
        {
            Console.WriteLine("\nНатисни клавиш за продължение...");
            Console.ReadKey(true);
        }

        protected int ReadInt(string label)
        {
            while (true)
            {
                Console.Write($"{label}: ");
                if (int.TryParse(Console.ReadLine(), out int v)) return v;
                Warning("Невалидно число.");
            }
        }

        protected DateTime ReadDate(string label)
        {
            while (true)
            {
                Console.Write($"{label}: ");
                if (DateTime.TryParse(Console.ReadLine(), out var v)) return v;
                Warning("Невалидна дата.");
            }
        }

        protected T ReadEnum<T>(string label) where T : struct
        {
            while (true)
            {
                Console.Write($"{label}: ");
                if (Enum.TryParse<T>(Console.ReadLine(), true, out var v)) return v;
                Warning("Невалидна стойност.");
            }
        }

        /// <summary>
        /// Изпълнява действие (create/update/report и т.н.) и прихваща всяко изключение,
        /// идващо от service/domain слоя - вместо да гръмне цялото приложение.
        /// При успех показва зелено съобщение (ако е подадено) или просто пауза;
        /// при грешка показва червено съобщение с текста на изключението.
        /// </summary>
        protected void TryRun(Action action, string successMessage = null)
        {
            try
            {
                action();
                if (successMessage != null)
                    Success(successMessage);
                else
                    Pause();
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        /// <summary>
        /// По-лек вариант на TryRun - само прихваща и показва грешка, без авто-пауза при успех.
        /// Подходящ за обвиване на цели под-менюта (напр. в MainMenu), за да не позволи
        /// изключение оттам да катастрофира цялата конзолна програма.
        /// </summary>
        protected void Safe(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        /// <summary>
        /// Подравнена таблица - вместо "Id | Name", колоните се подравняват според
        /// най-дългия ред, плюс брой записи накрая.
        /// </summary>
        protected void PrintTable(string[] headers, IEnumerable<string[]> rows)
        {
            var rowList = rows.ToList();
            int[] widths = new int[headers.Length];

            for (int i = 0; i < headers.Length; i++)
            {
                widths[i] = headers[i].Length;
                foreach (var row in rowList)
                    if (row[i].Length > widths[i]) widths[i] = row[i].Length;
            }

            void PrintRow(string[] cells)
            {
                for (int i = 0; i < cells.Length; i++)
                    Console.Write(cells[i].PadRight(widths[i] + 2));
                Console.WriteLine();
            }

            Console.ForegroundColor = AccentColor;
            PrintRow(headers);
            Console.WriteLine(new string('─', widths.Sum() + widths.Length * 2));
            Console.ResetColor();

            if (rowList.Count == 0)
            {
                Console.WriteLine("(няма записи)\n");
                return;
            }

            foreach (var row in rowList)
                PrintRow(row);

            Console.WriteLine($"\nВсичко: {rowList.Count}");
        }
    }
}