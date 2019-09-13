using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuaverCodeChallenge.Services;

namespace QuaverCodeChallenge.Controllers
{
    [Route("[controller]/[action]")]
    public class GitHubController : Controller
    {
        private readonly GitHubService _githubService;
        public GitHubController(GitHubService githubService)
        {
            _githubService = githubService;
        }

        public IActionResult CodingChallenge()
        {
            return View("~/Views/GitHubTopRepos.cshtml");
        }

        [HttpGet]
        public IActionResult StartPullingFromGithubAPI()
        {
            // initialize list contains duplicates to false
            bool listContainsDup = false;

            // make sure local folder and file exists
            _githubService.SetUpDirectoryAndTextFile();

            for (int i = 0; i < 3; i++)
            {
                IList<string> list = new List<string>();
                list = _githubService.GetAPI();
                if (list.Count != list.Distinct().Count())
                    listContainsDup = true;
                ViewData["list" + i.ToString()] = listContainsDup;
            }

            return View("~/Views/Shared/Confirm.cshtml");
        }
    }
}