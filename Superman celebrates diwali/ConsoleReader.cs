using System;
using System.Linq;

namespace Superman_celebrates_diwali
{
    static class ConsoleReader
    {
        public static int[] ReadNumbers()
        {
            var line = Console.ReadLine();
            if (line == null) return new int[0];

            var stringifiedNumbers = line.Split(' ');
            return stringifiedNumbers.Select(int.Parse).ToArray();
        }
    }
}
