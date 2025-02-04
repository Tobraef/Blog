﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class BlogEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryId
        {
            get;
            set;
        }

        [DataType(DataType.Date)]
        public DateTime Date
        {
            get;
            set;
        }

        [MaxLength(100)]
        public string Topic
        {
            get;
            set;
        }

        public byte[] Image
        {
            get;
            set;
        }

        [Display(Name = "Seen times")]
        public int Seen
        {
            get;
            set;
        }

        public virtual Account Author
        {
            get;
            set;
        }

        public virtual ICollection<Comment> Comments
        {
            get;
            set;
        }

        public virtual ICollection<EntryTag> Tags
        {
            get;
            set;
        }

        public virtual ICollection<ParagraphNode> Paragraphs
        {
            get;
            set;
        }
    }
}