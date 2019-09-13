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
        private Encryption encryption = new Encryption();

        private string folderPath = @"C:\QuaverCode";
        private string fullPath = @"C:\QuaverCode\Text.txt";

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
            var jsonItemList = json["items"];
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
            // looks like we are appending data here - however the file is freshly created each time the routine is run
            // not sure if this is intentional or not so will assume you only want is found on the 3 calls to GITHUB in the output file
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
            string ln;
            
            // initialize return variable
            IList<string> repoNames = new List<string>();

            // Read from file
            using (StreamReader file = new StreamReader(fullPath))
            {
                while ((ln = file.ReadLine()) != null)
                {
                    repoNames.Add(encryption.DecryptString(ln));
                }

                file.Close();
            }
            return repoNames;
        }

        // Sets up directory and textfile for project.
        public void SetUpDirectoryAndTextFile()
        {
            // If directory or file does not exist create it
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // If file exists go ahead and delete it
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            // although the FileStream.Close automatically disposes the object it's good form to use the using statement anyway
            using (FileStream f = File.Create(fullPath))
            {
                f.Close();
            }
        }
    }
}
