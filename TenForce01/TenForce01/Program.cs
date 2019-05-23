using System;

namespace TenForce01
{
    class Program
    {

        static void Main(string[] args)
        {
            // Screen info.
            Console.Write("Title: ");
            string Title = Console.ReadLine();
            Console.Write("Author: ");
            string Author = Console.ReadLine();

            // Return the List containing Books.
            var books = AccessAPI.GetBooks(Title, Author);

            Console.WriteLine();
            Console.WriteLine("what output do you like?");
            Console.WriteLine();
            Console.WriteLine("Press 1 for screen.");
            Console.WriteLine("Press 2 for a file.");
            Console.WriteLine("Press 3 for Database.");

            string Output = Console.ReadLine();
            
            // Select output.
            if (Output == "1")
            {
                DataHandling.BooksToConsole(books);              
            }
            else if (Output == "2")
            {
                DataHandling.BooksToFile(books);
                Console.WriteLine("File printed");
            }
            else if (Output == "3")
            {
                DataHandling.BooksToDatabase(books);
                Console.WriteLine("Your data has been transfered.");
            }
            else
            {
                Console.WriteLine("Wrong input");
            }           
            Console.ReadLine();
        }       
    }
}
