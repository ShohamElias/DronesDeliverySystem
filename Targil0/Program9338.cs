using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome9338();
            Welcome6442();
            Console.ReadKey();
        }
        static partial void Welcome6442();
        private static void Welcome9338()
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
