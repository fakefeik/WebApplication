using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
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
        public async Task<ActionResult> AddBoard(string name)
        {
            await boards.AddBoard(new BoardModel { Name = name });
            return new JsonResult() {Data = "{'status': 'OK'}"};
        }

		[HttpPost]
        public async Task<ActionResult> AddThread(int boardId, string name, string text)
        {
            await threads.AddThread(new ThreadModel {BoardId = boardId, Text = text, Topic = name});
			return new JsonResult() { Data = "{'status': 'OK'}" };
		}

	    [HttpPost]
	    public async Task<ActionResult> AddPost(int threadId, string name, string text)
	    {
		    await posts.AddPost(new PostModel
		    {
				ThreadId = threadId,
				Topic = name,
				Text = text
		    });
			return new JsonResult() { Data = "{'status': 'OK'}" };
		}

        public async Task<ActionResult> Board(int boardId)
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