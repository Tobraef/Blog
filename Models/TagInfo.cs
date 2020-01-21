using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class TagInfo
    {
        public List<EntryTag> AllTags
        {
            get;
            set;
        }

        [Display(Name = "How many entries have given tag")]
        public IEnumerable<KeyValuePair<EntryTag, int>> TagToUses
        {
            get;
            set;
        }

        [Display(Name ="How many users have seen given tag")]
        public IEnumerable<KeyValuePair<EntryTag, int>> TagToPopularity
        {
            get;
            set;
        }
    }
}