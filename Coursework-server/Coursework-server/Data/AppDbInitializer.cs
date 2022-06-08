using Coursework_server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Data
{
    public static class AppDbInitializer
    {
        public static async Task SeedAsync(IApplicationBuilder builder)
        {
            using var serviceScope = builder.ApplicationServices.CreateScope();
            var db = serviceScope.ServiceProvider.GetService<AppDbContext>();

            if (await db.Topics.AnyAsync() == false)
            {
                db.Topics.AddRange(
                    new Topic
                    {
                        Name = "Alcohol"
                    },
                    new Topic
                    {
                        Name = "Books"
                    },
                    new Topic
                    {
                        Name = "People"
                    },
                    new Topic
                    {
                        Name = "Animals"
                    },
                    new Topic
                    {
                        Name = "Cars"
                    },
                    new Topic
                    {
                        Name = "Clothes"
                    },
                    new Topic
                    {
                        Name = "Houses"
                    }
                );
            }

            if (await db.FieldTypes.AnyAsync() == false)
            {
                db.FieldTypes.AddRange(
                    new FieldType
                    {
                        Name = "Numerical",
                    },
                    new FieldType
                    {
                        Name = "String"
                    },
                    new FieldType
                    {
                        Name = "Text"
                    },
                    new FieldType
                    {
                        Name = "Date"
                    },
                    new FieldType
                    {
                        Name = "Checkbox"
                    }
                );
            }

            if (await db.Users.AnyAsync() == false)
            {
                db.Users.Add(
                    new User
                    {
                        Email = "admin@mail.com",
                        Name = "Admin",
                        Password = "123456",
                        UserRole = UserRole.Admin
                    }
                );
            }

            await db.SaveChangesAsync();
        }
    }
}
