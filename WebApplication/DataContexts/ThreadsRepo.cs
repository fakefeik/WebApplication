using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.DataContexts
{
    public class ThreadsRepo
    {
        private readonly ApplicationDbContext db;

        public ThreadsRepo() : this(new ApplicationDbContext())
        {
        }

        public ThreadsRepo(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddThread(ThreadModel thread)
        {
            db.Threads.Add(thread);
            await db.SaveChangesAsync();
        }

        public IEnumerable<ThreadModel> GetThreads(string boardId)
        {
            return db.Threads.Where(x => x.BoardId == boardId);
        }
    }
}