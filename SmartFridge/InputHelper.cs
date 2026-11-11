using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridgeProj
{
    internal static class InputHelper
    {
        // Text that may be empty (returns "" if the user just presses Enter)
        public static string ReadOptionalString(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";
            return input.Trim();
        }

        // Text that must not be empty (asks again until the user types something)
        public static string ReadString(string prompt)
        {
            while (true)
            {
                string input = ReadOptionalString(prompt);
                if (input != "") return input;
                Console.WriteLine("Please enter a value.");
            }
        }

        // Any whole number (asks again until the input is valid)
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                string input = ReadOptionalString(prompt);
                int number;
                if (int.TryParse(input, out number)) return number;
                Console.WriteLine("Please enter a whole number.");
            }
        }

        // Whole number greater than 0
        public static int ReadPositiveInt(string prompt)
        {
            while (true)
            {
                int number = ReadInt(prompt);
                if (number > 0) return number;
                Console.WriteLine("The number must be greater than 0.");
            }
        }

        // Decimal number, 0 or more (used for prices)
        public static double ReadPositiveDouble(string prompt)
        {
            while (true)
            {
                string input = ReadOptionalString(prompt);
                double number;
                if (double.TryParse(input, out number) && number >= 0) return number;
                Console.WriteLine("Please enter a valid price.");
            }
        }

        // Lets the user pick one value of any enum from a numbered list
        public static T ReadEnum<T>(string label) where T : struct, Enum
        {
            T[] values = Enum.GetValues<T>();
            for (int i = 0; i < values.Length; i++)
                Console.WriteLine($"{i + 1}. {values[i]}");

            while (true)
            {
                int choice = ReadInt($"{label}: ");
                if (choice >= 1 && choice <= values.Length) return values[choice - 1];
                Console.WriteLine("Invalid choice.");
            }
        }

        public static void Pause()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
