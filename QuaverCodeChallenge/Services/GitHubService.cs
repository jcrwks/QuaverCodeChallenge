using Newtonsoft.Json.Linq;
using QuaverCodeChallenge.Utils;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace QuaverCodeChallenge.Services
{
    public class GitHubService
    {
        // Global variables 
        Encryption encryption = new Encryption();
        string folderPath = @"C:\QuaverCode";
        string fullPath = @"C:\QuaverCode\Text.txt";

        public IList<string> GetAPI()
        {
            string content;
            IList<string> repoNames = new List<string>();

            // Pull data from Github
            HttpWebRequest request = WebRequest.Create("https://api.github.com/search/repositories?q=language:php&sort=stars&order=desc&per_page=5") as HttpWebRequest;
            request.UserAgent = "TestApp";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                content = reader.ReadToEnd();
            }

            // Convert data to JObject
            JObject json = JObject.Parse(content);

            // Loop though items looking for URL's and add to a list
            var jsonItemList = json["item"];
            foreach (var i in jsonItemList)
            {
                repoNames.Add((i["owner"]["url"]).ToString().ToUpper());
            }

            // Get what url's are currentyly stored in the text file
            IList<string> repoNamesInFile = ReadFromFile();

            // Write to file sending what was found in the text file as well as what was pulled from Github
            WriteToFile(repoNamesInFile, repoNames);

            // Get the Count of files currently in the text file
            return ReadFromFile();

        }

        // Writes to file
        private void WriteToFile(IList<string> currentList, IList<string> itemsToWrite)
        {
            using (StreamWriter writer = new StreamWriter(fullPath, true))
            {
                foreach (var i in itemsToWrite)
                {
                    if (!currentList.Contains(i))
                    {
                        writer.WriteLine(encryption.EnryptString(i));
                    }
                }
                writer.Close();
            }
        }

        // Reads from file
        private IList<string> ReadFromFile()
        {
            int counter = 0;
            string ln;

            // Read from file
            using (StreamReader file = new StreamReader(fullPath))
            {
                IList<string> repoNames = new List<string>();
                while ((ln = file.ReadLine()) != null)
                {
                    repoNames.Add(ln.ToLower());
                    counter++;
                }
                file.Close();
                return repoNames;
            }
        }


        // Sets up directory and textfile for project.
        public void SetUpDirectoryAndTextFile()
        {
            // If directory or file does not exist create it
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                FileStream f = File.Create(fullPath);
                f.Close();
            }
            else
            {
                FileStream f = File.Create(fullPath);
                f.Close();
            }
        }
    }
}
