﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Website.Models.Blog
{
    public class BlogCreate
    {
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(10, ErrorMessage = "Must be at least 10-50 characters")]
        [MaxLength(50, ErrorMessage = "Must be 10-50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MinLength(300, ErrorMessage = "Must be at least 300 characters")]
        [MaxLength(3000, ErrorMessage = "Must be 300-3000 characters")]
        public string Content { get; set; }
        // can be null
        public int? PhotoId { get; set; }


    }
}
