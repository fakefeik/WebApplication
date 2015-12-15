﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class BoardModel
    {
        [Key]
        [StringLength(4)]
        public string ShortName { get; set; }

        [StringLength(32)]
        public string Name { get; set; }

        public IEnumerable<ThreadModel> Threads;
    }
}
