using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using WebApplication.DataContexts;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ForumController : Controller
    {
        private BoardsRepo boards = new BoardsRepo();
        private ThreadsRepo threads = new ThreadsRepo();
		private PostsRepo posts = new PostsRepo();


        // GET: Forum
        public ActionResult Index()
        {
            return View(boards.GetBoards());
        }

        [HttpPost]
        public async Task<ActionResult> AddBoard(string shortName, string name)
        {
            await boards.AddBoard(new BoardModel { ShortName = shortName, Name = name });
            return new JsonResult {Data = "{'status': 'OK'}"};
        }

		[HttpPost]
        public async Task<ActionResult> AddThread(string boardId, string name, string text)
        {
            var thread = await threads.AddThread(new ThreadModel {BoardId = boardId});
		    await posts.AddPost(new PostModel {ThreadId = thread.Id, Text = text, Topic = name, Timestamp = DateTime.Now});
			return new JsonResult() { Data = "{'status': 'OK'}" };
		}

	    [HttpPost]
	    public async Task<ActionResult> AddPost(int threadId, string name, string text)
	    {
            var response = Request["g-recaptcha-response"];
            const string secret = "6LfW4hETAAAAAJuj9x_N6C_gGWwbk3cvpmaeBTzC";
            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            //when response is false check for the error message
            if (!captchaResponse.Success)
            {
                if (captchaResponse.ErrorCodes.Count <= 0)
                    return new JsonResult() { Data = "{'status': 'Not ok'}" };

                var error = captchaResponse.ErrorCodes[0].ToLower();
                return new JsonResult() {Data = "{'status': '" + error + "'}"};
            }
            await posts.AddPost(new PostModel
		    {
				ThreadId = threadId,
				Topic = name,
				Text = text
		    });
			return new JsonResult() { Data = "{'status': 'OK'}" };
		}

        public async Task<ActionResult> Board(string boardId)
        {
	        var board = await boards.GetBoard(boardId);
	        board.Threads = threads.GetThreads(boardId);
	        return View(board);
        }

        public async Task<ActionResult> Thread(int threadId)
        {
	        var thread = threads.GetThread(threadId);
	        thread.Posts = posts.GetPosts(threadId);
	        thread.BoardName = (await boards.GetBoard(thread.BoardId)).Name;
            return View(thread);
        }
    }
}