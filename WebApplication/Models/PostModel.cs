using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class PostModel
    {
        [Key]
        public int Id { get; set; }

        public ApplicationUser User { get; set; }

        [StringLength(32)]
        public string Topic { get; set; }

        [StringLength(200)]
        public string Text { get; set; }

        public int ThreadId { get; set; }
    }
}