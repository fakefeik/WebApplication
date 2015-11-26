﻿using System;
using System.Collections.Generic;
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
			await db.SaveChangesAsync();
		}

		public IEnumerable<PostModel> GetPosts(int threadId)
		{
			return db.Posts.Where(x => x.ThreadId == threadId);
		}
	}
}