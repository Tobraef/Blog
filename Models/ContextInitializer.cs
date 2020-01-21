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
            var img1Source = @"C:\Users\mprzemys\Desktop\Test pages\indx.png";
            var img2Source = @"C:\Users\mprzemys\Desktop\Test pages\Clearear.jpg";
            MemoryStream stream = new MemoryStream();
            Image.FromFile(img1Source).Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] buffer1 = new byte[stream.Length];
            stream.Read(buffer1, 0, (int)stream.Length);
            Image.FromFile(img2Source).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] buffer2 = new byte[stream.Length];
            stream.Read(buffer2, 0, (int)stream.Length);
            stream.Close();
            var entries = new List<BlogEntry> {
                new BlogEntry
                {
                    EntryId = 1,
                    Topic = "Cool topic",
                    Text = "Casual long text Casual long text Casual long text Casual long text Casual long text",
                    Author = accs.First(),
                    Date = DateTime.Now,
                    Seen = 0,
                    Image = buffer1
                },
                new BlogEntry
                {
                    EntryId = 2,
                    Topic = "Super topic",
                    Text = "Short textShort textShort textShort textShort textShort textShort text",
                    Author = accs.First(),
                    Date = DateTime.Now,
                    Seen = 10,
                    Image = buffer2
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

            context.Accounts.AddRange(accs);
            context.Entries.AddRange(entries);
            context.Comments.AddRange(comments);
            context.Tags.AddRange(tags);
            context.SaveChanges();
        }
    }
}