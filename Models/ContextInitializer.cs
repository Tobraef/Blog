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
            var stream = new FileStream(@"C:\Users\Public\Pictures\Sample Pictures\Jellyfish.jpg", FileMode.Open);
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            var entries = new List<BlogEntry> { new BlogEntry {
                Topic = "Cool topic",
                Text = "Casual long text Casual long text Casual long text Casual long text Casual long text",
                Author = accs.First(),
                Date = System.DateTime.Now,
                Image = bytes
            }};
            var comments = new List<Comment> { 
            new Comment {
                Author = accs.First(),
                Entry = entries.First(),
                Date = System.DateTime.Now,
                Text = "Nice one"
            },
            new Comment {
                Author = accs.First(),
                Entry = entries.First(),
                Date = System.DateTime.Now,
                Text = "Good one"
            } };
            context.Accounts.AddRange(accs);
            context.Entries.AddRange(entries);
            context.Comments.AddRange(comments);
            context.SaveChanges();
        }
    }
}