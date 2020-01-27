using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class ParagraphNode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get;
            set;
        }

        [Required]
        [RegularExpression("[0-9]\\..*")]
        public string Index
        {
            get;
            set;
        }

        [Required]
        [MinLength(1)]
        public string Heading
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public virtual BlogEntry Entry
        {
            get;
            set;
        }
    }
}