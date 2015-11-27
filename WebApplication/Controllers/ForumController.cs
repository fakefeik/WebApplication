using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using WebApplication.DataContexts;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ForumController : Controller
    {
		public ApplicationUserManager UserManager => Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
		private readonly BoardsRepo _boards = new BoardsRepo();
        private readonly ThreadsRepo _threads = new ThreadsRepo();
		private readonly PostsRepo _posts = new PostsRepo();

	    // GET: Forum
        public ActionResult Index()
        {
            return View(_boards.GetBoards());
        }

        [HttpPost]
        public async Task<ActionResult> AddBoard(string shortName, string name)
        {
            await _boards.AddBoard(new BoardModel { ShortName = shortName, Name = name });
	        return RedirectToAction("Index");
        }

		[HttpPost]
        public async Task<ActionResult> AddThread(string boardId, string name, string text)
        {
            var thread = await _threads.AddThread(new ThreadModel {BoardId = boardId});
			var uid = User.Identity.GetUserId();
		    await _posts.AddPost(new PostModel
		    {
			    ThreadId = thread.Id,
				Text = text,
				Topic = name,
				Timestamp = DateTime.Now,
				UserId = User.Identity.GetUserId()
		    });
			return RedirectToAction("Board", new { boardId = boardId });
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
                //if (captchaResponse.ErrorCodes.Count <= 0)
                //    return RedirectToAction("Thread", new { threadId = threadId });

				//var error = captchaResponse.ErrorCodes[0].ToLower();
	            return RedirectToAction("Thread", new { threadId = threadId });
            }
            await _posts.AddPost(new PostModel
		    {
				ThreadId = threadId,
				Topic = name,
				Text = text,
				Timestamp = DateTime.Now,
				UserId = User.Identity.GetUserId()
			});
			return RedirectToAction("Thread", new { threadId = threadId });
		}

        public async Task<ActionResult> Board(string boardId)
        {
	        var board = await _boards.GetBoard(boardId);
	        var threads = _threads.GetThreads(boardId).ToList();
	        for (int i = 0; i < threads.Count; i++)
	        {
		        var posts = _posts.GetPosts(threads[i].Id).ToList();
		        for (int j = 0; j < posts.Count; j++)
		        {
			        var users = UserManager.Users.ToList();
                    posts[j].Username = users.FirstOrDefault(x => x.Id == posts[j].UserId)?.UserName;
		        }
		        threads[i].Posts = posts;
	        }
	        board.Threads = threads;
	        return View(board);
        }

        public async Task<ActionResult> Thread(int threadId)
        {
	        var thread = _threads.GetThread(threadId);
	        thread.Posts = _posts.GetPosts(threadId);
	        thread.BoardName = (await _boards.GetBoard(thread.BoardId)).Name;
            return View(thread);
        }
    }
}