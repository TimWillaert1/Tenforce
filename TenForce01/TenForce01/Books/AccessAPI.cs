using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace TenForce01
{
    public class AccessAPI
    {
        // Set constant API key,Link.
        private const string Key = "AIzaSyBxIVDRXiUaTp9PY6Z4TOAh9e4m6x2tMLU";
        private const string Link = "https://www.googleapis.com/books/v1/volumes?q=";

        // Get the List containing content returned by Getbooks
        public static List<Book> GetBooks(string title, string author)
        {
            // Create a request for the URL.   
            WebRequest request = WebRequest.Create(
              Link + title + "+inauthor:" + author + "&key=" + Key);

            // Get the response.  
            using (WebResponse response = request.GetResponse())
            {
                // Get the stream containing content returned by the server. 
                // The using block ensures the stream is automatically closed. 
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.  
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        List<Book> books = new List<Book>();

                        // Read the content.    
                        string responseFromServer = reader.ReadToEnd();

                        // Parse / SelectTokens.
                        JObject JParse = JObject.Parse(responseFromServer);
                        JToken ItemsToken = JParse.SelectToken("items");

                        // Error handling in case no books found.
                        if (ItemsToken == null)
                        {
                            return new List<Book>();
                        }

                        // Cast Jtoken to Jarray.
                        JArray Items = (JArray)ItemsToken;

                        // Loop the Hierarchy to retrieve all childs.
                        foreach (JToken item in Items)
                        {
                            // Volume container
                            JToken Volume = item.SelectToken("volumeInfo");

                            JArray authors = (JArray)Volume.SelectToken("authors");
                            JToken firstAuth = authors.FirstOrDefault();
                            JToken IdentiefierToken = Volume.SelectToken("industryIdentifiers");

                            // Skip IdentiefierToken if empty.
                            if (IdentiefierToken == null)
                            {
                                continue;
                            }

                            JArray Identifiers = (JArray)IdentiefierToken;
                            JToken firstIdent = Identifiers.FirstOrDefault();
                            JToken PublisherToken = Volume.SelectToken("publisher");

                            // Skip if any is empty.
                            if (firstAuth == null || firstIdent == null || PublisherToken == null)
                            {
                                continue;
                            }

                            // Make instance from object.
                            var Book = new Book
                            {
                                Title = Volume.SelectToken("title").ToString(),
                                Publisher = PublisherToken.ToString(),
                                Author = firstAuth.ToString(),
                                ISBN = firstIdent.SelectToken("identifier")?.ToString()
                            };
                            // Add book to list.
                            books.Add(Book);
                        }
                        // Return books                        
                        return books;
                    }
                }
            }
        }
    }
}