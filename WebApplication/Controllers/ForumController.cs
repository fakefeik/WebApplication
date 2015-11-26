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

        public async Task<ActionResult> AddThread(string boardId, string name, string text)
        {
            await threads.AddThread(new ThreadModel {BoardId = boardId, Text = text, Topic = name});
            return new JsonResult {Data = "{'status': 'OK'}"};
        }

        public ActionResult Board(string boardId)
        {
            return View(threads.GetThreads(boardId));
        }

        public ActionResult Thread(string threadId)
        {
            return View();
        }
    }
}