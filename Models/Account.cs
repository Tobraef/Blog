using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId
        {
            get;
            set;
        }

        [Required]
        [MinLength(3)]
        public string Name
        {
            get;
            set;
        }

        [Required]
        [MinLength(6)]
        public string Password
        {
            get;
            set;
        }

        [RegularExpression("(admin|poster|guest)",
            ErrorMessage="Not valid account type")]
        public string Type
        {
            get;
            set;
        }

        [EmailAddress]
        public string Email
        {
            get;
            set;
        }

        public virtual ICollection<BlogEntry> Entries
        {
            get;
            set;
        }

        public virtual ICollection<Comment> Comments
        {
            get;
            set;
        }

        public virtual ICollection<TagToUser> TagsToUser
        {
            get;
            set;
        }
    }

    public static class UserTypes
    {
        public const string admin = "admin";
        public const string poster = "poster";
        public const string guest = "guest";
    }
}