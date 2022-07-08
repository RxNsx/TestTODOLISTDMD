using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TODOLISTTRY.DAL.Model;

namespace TODOLISTTRY.DAL
{
    public static class DataSeed
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if(context.Database.GetAppliedMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.AllDoes.Any())
            {
                context.AllDoes.AddRange(
                    new Do
                    {
                        Title = "Boil dumplings",
                        Description = "Salt and Pepper It",
                        Executors = "Me",
                        Status = DoStatus.Created,
                        Created = DateTime.Now,
                        Finished = null,
                        Plan = new DateTime(2020, 11, 01),
                        Fact = null,
                        SubTasks = new List<Do> {
                            new Do 
                            { 
                                Title = "Cut Salad",
                                Description = "Iceberg salad is fresh atm",
                                Executors = "Me",
                                Status = DoStatus.Created,
                                Finished = null,
                                Plan = new DateTime(2022, 11, 01),
                                Fact = null,
                                SubTasks = null
                            },
                            new Do 
                            { 
                                Title = "Additional Title",
                                Description = "Additional Description",
                                Executors = "Me",
                                Status = DoStatus.Created,
                                Finished = null,
                                Plan = new DateTime(2022, 12, 12),
                                Fact = null,
                                SubTasks = null
                            }
                        }
                    },
                    new Do
                    {
                        Title = "Fry Chicken",
                        Description = "Do not forget to keep it out",
                        Executors = "Brother",
                        Status = DoStatus.Processing,
                        Created = DateTime.Now,
                        Finished = null,
                        Plan = new DateTime(2022, 08, 12),
                        Fact = null,
                        SubTasks = new List<Do>
                        {
                            new Do
                            {
                                Title = "Something",
                                Description = "Some Description",
                                Executors = "Me",
                                Status = DoStatus.Processing,
                                Finished = null,
                                Plan = new DateTime(2022,11,05),
                                Fact = null,
                                SubTasks = null
                            }
                        }
                        

                    },
                    new Do
                    {
                        Title = "Cook mashhed potatoes",
                        Description = "Any fry meat could be good",
                        Executors = "Me",
                        Status = DoStatus.Created,
                        Created = DateTime.Now,
                        Finished = null,
                        Plan = new DateTime(2022,09,09),
                        Fact = null,
                        SubTasks = null
                    });

                
            }

            context.SaveChanges();
        }
    }
}
