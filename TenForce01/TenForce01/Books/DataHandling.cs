using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace TenForce01
{
    public class DataHandling
    {
        public static void BooksToConsole(List<Book> books)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Title | Publisher | Author | ISBN");
            sb.AppendLine();

            foreach (Book book in books)
            {
                sb.AppendLine(book.Title + " | " + book.Publisher + " | " + book.Author + " | " + book.ISBN);
            }
            Console.WriteLine(sb.ToString());           
        }
        public static void BooksToFile(List<Book> books)
        {
            StringBuilder sb = new StringBuilder();

            // Set Column.
            sb.AppendLine("Title | Publisher | Author | ISBN");
            sb.AppendLine();

            // Set data in columns.
            foreach (Book book in books)
            {
                sb.AppendLine(book.Title+" | "+ book.Publisher +" | " + book.Author+" | "+book.ISBN);                                                   
            }
            // Write to File.
            System.IO.File.WriteAllText("SavedLists.txt", sb.ToString());
        }
        public static void BooksToDatabase(List<Book> books)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var dbPath = Path.Combine(Path.GetFullPath(Path.Combine(path, @"..\..\..\")), "BookDatabase.db");

            // Make new database.
            var databaseConnection = new SQLiteConnection("Data Source="+ dbPath + ";Version=3;");
            
            // Open database.
            databaseConnection.Open();
            foreach (Book book in books)
            {
                // Query.
                string sqlite = string.Format("insert into Books (Title,Publisher,Author,ISBN) values('{0}','{1}','{2}','{3}')", book.Title, book.Publisher, book.Author, book.ISBN);

                try
                {
                    // Write in database.
                    SQLiteCommand command = new SQLiteCommand(sqlite, databaseConnection);
                    command.ExecuteNonQuery();                   
                }
                catch(SQLiteException e)
                {
                    // Console.WriteLine(e.Message);
                    continue; 
                }              
            }
            // Close database.
            databaseConnection.Close();
            
        }
    }
}
