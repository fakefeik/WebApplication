using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class ThreadModel
    {
        [Key]
        public int Id { get; set; }

        public ApplicationUser User;

        [StringLength(32)]
        public string Topic { get; set; }

        [StringLength(200)]
        public string Text { get; set; }

        [StringLength(64)]
        public string BoardId { get; set; }
    }
}