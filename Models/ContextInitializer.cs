using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;
using System.Drawing;


namespace Blog.Models
{
    public class ContextInitializer : DropCreateDatabaseAlways<BlogContext>
    {
        private byte[] LoadImage(string source)
        {
            var stream = new FileStream(source, FileMode.Open);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }

        protected override void Seed(BlogContext context)
        {
            base.Seed(context);
            var accs = new List<Account>{new Account
            { 
                AccountId = 1,
                Name = "Przemkov",
                Password = "sadelko123",
                Email = "a@a.pl",
                Type = UserTypes.admin
            }};
            //var img1Source = @"C:\Users\Public\Pictures\Sample Pictures\Scan.png";
            //var img2Source = @"C:\Users\Public\Pictures\Sample Pictures\Penguins.jpg";
            var img1Source = @"C:\Users\mprzemys\Desktop\Test pages\indx.png";
            var img2Source = @"C:\Users\mprzemys\Desktop\Test pages\Clearear.jpg";
            var buffer1 = LoadImage(img1Source);
            var buffer2 = LoadImage(img2Source);
            var entries = new List<BlogEntry> {
                new BlogEntry
                {
                    EntryId = 1,
                    Topic = "Cool topic",
                    Author = accs.First(),
                    Date = DateTime.Now,
                    Seen = 0,
                    Image = buffer1
                },
                new BlogEntry
                {
                    EntryId = 2,
                    Topic = "Super topic",
                    Author = accs.First(),
                    Date = DateTime.Now,
                    Seen = 10,
                    Image = buffer2
                },
                new BlogEntry
                {
                    EntryId = 3,
                    Topic = "Extra topic",
                    Author = accs.First(),
                    Date = DateTime.Now,
                    Seen = 5,
                    Image = buffer1
                }
            };
            var comments = new List<Comment> { 
            new Comment {
                Author = accs.First(),
                Entry = entries.First(),
                Date = DateTime.Now,
                Text = "Nice one"
            },
            new Comment {
                Author = accs.First(),
                Entry = entries.First(),
                Date = DateTime.Now,
                Text = "Good one"
            } };
            var tags = new List<EntryTag> {
            new EntryTag
            {
                Name = "travel",
                Entries = new HashSet<BlogEntry> { entries.First(), entries.Last() } 
            },
            new EntryTag
            {
                Name = "wild",
                Entries = new HashSet<BlogEntry> { entries.First() }
            }
            };
            var paragraphs = new List<ParagraphNode> {
            new ParagraphNode
            {
                Entry = entries.First(),
                Index = "1.",
                Heading = "Healthy life",
                Text = "Be healthy mate"
            },
            new ParagraphNode
            {
                Entry = entries.Last(),
                Index = "1.",
                Heading = "Good life",
                Text = "Stay healthy mate"
            },
            new ParagraphNode
            {
                Entry = entries.ElementAt(1),
                Index = "1.",
                Heading = "Best life",
                Text = "Being healthy mate"
            },
            new ParagraphNode
            {
                Entry = entries.First(),
                Index = "1.1.",
                Heading = "Life is life",
                Text = "nananana"
            }
            };

            context.Accounts.AddRange(accs);
            context.Entries.AddRange(entries);
            context.Comments.AddRange(comments);
            context.Tags.AddRange(tags);
            context.Paragraphs.AddRange(paragraphs);
            context.SaveChanges();
        }
    }
}