using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoadAppWEB.Data;
using System;
using System.Linq;

namespace RoadAppWEB.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RoadAppWEBContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RoadAppWEBContext>>()))
            {
                // Look for any node.
                if (!context.node.Any())
                {
                    context.node.AddRange(
                    new node
                    {
                        id = "军工路上00",
                        hubnode = "军工路上00",
                        fathernode = "",
                        childnode = "军工路上01",
                        road_id = "",
                        level = 1,
                        longitude = 121.555057,
                        latitude = 31.300105,

                    },

                    new node
                    {
                        id = "军工路上01",
                        hubnode = "军工路上00",
                        fathernode = "军工路上00",
                        childnode = "军工路上02",
                        road_id = "",
                        level = 1,
                        longitude = 121.554159,
                        latitude = 31.301593,
                    },

                    new node
                    {
                        id = "军工路上02",
                        hubnode = "军工路上00",
                        fathernode = "军工路上01",
                        childnode = "军工路上03",
                        road_id = "",
                        level = 1,
                        longitude = 121.552973,
                        latitude = 31.303487,
                    }
                );
                }

                // Look for any movies.
                if (!context.mainline.Any())
                {
                    context.mainline.AddRange(
                    new mainline
                    {
                        id = "军工路上匝道",
                        viaduct_id = "中环高架",
                        avg_length = 30,
                        direction = "上",
                        StartNode = "军工路上00",
                        EndNode = "军工路上03",
                    }
                );
                }

                context.SaveChanges();
            }
        }
    }
}
