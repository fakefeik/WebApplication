﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.DataContexts
{
    public class PostsRepo
    {
        private readonly ApplicationDbContext db;

        public PostsRepo() : this(new ApplicationDbContext())
        {
        }

        public PostsRepo(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddPost(PostModel post)
        {
            db.Posts.Add(post);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException)
            {
            }
        }

        public IEnumerable<PostModel> GetPosts(int threadId)
        {
            return db.Posts.Where(x => x.ThreadId == threadId);
        }

        public PostModel GetPost(int postId)
        {
            return db.Posts.Find(postId);
        }

        public async Task DeletePost(int postId)
        {
            db.Posts.Remove(db.Posts.Find(postId));
            await db.SaveChangesAsync();
        }
    }
}
