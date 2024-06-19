using Microsoft.EntityFrameworkCore;
using WebDatabaseProject;

using var db = new BlogContext();

var allBlogs = db.Blogs.ToList();
db.Blogs.RemoveRange(allBlogs);
db.SaveChanges();
Console.WriteLine("All blogs in the database:" + allBlogs.Count);


db.Add(new TechBlog()
{
    Name = "Tech Blog - " + Guid.NewGuid().ToString("N"),
    GithubUrl = "http://github.com/peanudge",
    ParentBlogId = null
});

db.Add(new ShopBlog()
{
    Name = "Shop Blog - " + Guid.NewGuid().ToString("N"),
    ShopUrl = "http://store/peanudge",
    ChildBlogs = new List<BlogBase> {
        new ShopBlog {
            Name = "Child Shop Blog - " + Guid.NewGuid().ToString("N"),
            ShopUrl = "http://store/peanudge/sub1",
        },
        new ShopBlog {
            Name = "Child Shop Blog - " + Guid.NewGuid().ToString("N"),
            ShopUrl = "http://store/peanudge/sub2",
        }
    }
});


db.SaveChanges();

var blogs = await db.Blogs
    .Include(b => b.ChildBlogs)
    // .Include(b => b.Configs)
    .ToListAsync();

foreach (var blog in blogs)
{
    Console.WriteLine(blog.Id + ", " + blog.Name);
    foreach (var childBlogs in blog.ChildBlogs)
    {
        Console.WriteLine("     Child Blog: " + childBlogs.Id + ", " + childBlogs.Name);
    }
}

// foreach (var blog in blogs)
// {
//     Console.WriteLine(blog.Id + ", " + blog.Name);
//     Console.WriteLine("Configs Count: " + blog.Configs.Count);

//     var count = blog.Count;
//     var createdAt = blog.CreatedAt;

//     Console.WriteLine("Count: " + count);
//     Console.WriteLine("CreatedAt: " + createdAt);
// }


// db.Add(new Blog
// {
//     Name = "Blog - " + Guid.NewGuid().ToString("N"),
//     Configs = new List<BlogConfig> {
//             new BlogConfig {
//                 Key = "Count",
//                 ValueTypeId = ValueTypeId.Int,
//                 Value = 100.ToString()
//             },
//             new BlogConfig {
//                 Key = "CreatedAt",
//                 ValueTypeId = ValueTypeId.DateTime,
//                 Value = DateTime.UtcNow.ToString()
//             },
//             new BlogConfig {
//                 Key = "UpdatedAt",
//                 ValueTypeId = ValueTypeId.DateTime,
//                 Value = "2021-09-01T00:00:00Z"
//             },
//             new BlogConfig {
//                 Key = "User",
//                 ValueTypeId = ValueTypeId.User,
//                 Value = JsonSerializer.Serialize(new User { Name = "Jiho" })
//             },
//             new BlogConfig {
//                 Key = "InvalidData",
//                 ValueTypeId = ValueTypeId.Bool,
//                 Value = "Invalid Data"
//             }
//         }
// });
