using System;

namespace MovieAssignmentInterfaces
{
    public class Menu
    {
        public int ValueGetter()
        {
            string option = Console.ReadLine();
            int number;
            bool success = Int32.TryParse(option, out number);

            while (!success)
            {
                Console.WriteLine("That isn't a number sorry!");
                option = Console.ReadLine();
                success = Int32.TryParse(option, out number);
            }

            return number;
        }
        public void Display()
        {
            Console.WriteLine("What you want to do?\n1.)Add\n2.)Search\n3.)Display\n4.)Exit");
        }
    }
}