using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using WebApplication.DataContexts;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ForumController : Controller
    {
        private ApplicationUserManager UserManager => Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private readonly BoardsRepo _boards = new BoardsRepo();
        private readonly ThreadsRepo _threads = new ThreadsRepo();
        private readonly PostsRepo _posts = new PostsRepo();

        // GET: Forum
        public ActionResult Index()
        {
            return View(_boards.GetBoards());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddBoard(string shortName, string name)
        {
            await _boards.AddBoard(new BoardModel { ShortName = shortName, Name = name });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBoard(string boardId)
        {
            await _boards.DeleteBoard(boardId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteThread(int threadId)
        {
            var boardId = _threads.GetThread(threadId).BoardId;
            await _threads.DeleteThread(threadId);
            return RedirectToAction("Board", new {boardId = boardId});
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> DeletePost(int threadId, int postId)
        {
            var post = _posts.GetPost(postId);
            if (User.IsInRole("Admin") || post.UserId == User.Identity.GetUserId())
                await _posts.DeletePost(postId);
            return RedirectToAction("Thread", new { threadId = threadId });
        }

        [HttpPost]
        public ActionResult GetNewPosts(int threadId, int currentCount)
        {
            var posts = _posts.GetPosts(threadId).ToList();
            if (posts.Count == currentCount)
                return Json(new
                {
                    Posts = new PostModel[0],
                    IsAdmin = User.IsInRole("Admin"),
                    UserId = User.Identity.GetUserId(),
                    DeleteUrl = Url.Action("DeletePost")
                });
            return Json(new
            {
                Posts = posts.Skip(currentCount).Select(p => new
                {
                    UserId = p.UserId,
                    Username = UserManager.Users
                        .ToList()
                        .FirstOrDefault(u => u.Id == p.UserId)
                        ?.UserName,
                    Id = p.Id,
                    Timestamp = p.Timestamp.ToString(),
                    Topic = p.Topic,
                    Text = p.Text,
                    ThreadId = p.ThreadId
                }),
                IsAdmin = User.IsInRole("Admin"),
                UserId = User.Identity.GetUserId(),
                DeleteUrl = Url.Action("DeletePost")
            });
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddThread(string boardId, string name, string text)
        {
            if (!this.CheckCaptcha() && !User.IsInRole("Admin"))
                return RedirectToAction("Board", new { boardId = boardId });

            var thread = await _threads.AddThread(new ThreadModel { BoardId = boardId });
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
        [Authorize]
        public async Task<ActionResult> AddPost(int threadId, string name, string text)
        {
            if (!this.CheckCaptcha() && !User.IsInRole("Admin"))
                return RedirectToAction("Thread", new { threadId = threadId });

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
            foreach (var thread in threads)
            {
                var posts = _posts.GetPosts(thread.Id).ToList();
                foreach (var post in posts.Take(1))
                    post.Username = UserManager.Users
                        .ToList()
                        .FirstOrDefault(x => x.Id == post.UserId)
                        ?.UserName;
                thread.Posts = posts;
            }
            board.Threads = threads;
            return View(board);
        }

        public async Task<ActionResult> Thread(int threadId)
        {
            var thread = _threads.GetThread(threadId);
            var posts = _posts.GetPosts(thread.Id).ToList();
            foreach (var post in posts)
                post.Username = UserManager.Users
                    .ToList()
                    .FirstOrDefault(x => x.Id == post.UserId)
                    ?.UserName;
            thread.Posts = posts;
            thread.BoardName = (await _boards.GetBoard(thread.BoardId)).Name;
            return View(thread);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            return View(UserManager.Users);
        }
    }
}
