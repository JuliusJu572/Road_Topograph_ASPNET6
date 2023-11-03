using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoadAppWEB.Models;

namespace RoadAppWEB.Data
{
    public class RoadAppWEBContext : DbContext
    {
        public RoadAppWEBContext (DbContextOptions<RoadAppWEBContext> options)
            : base(options)
        {
        }

        public DbSet<RoadAppWEB.Models.node>? node { get; set; }

        public DbSet<RoadAppWEB.Models.mainline>? mainline { get; set; }

        public DbSet<RoadAppWEB.Models.overpass>? overpass { get; set; }

        public DbSet<RoadAppWEB.Models.viaduct>? viaduct { get; set; }

        public DbSet<RoadAppWEB.Models.ramp>? ramp { get; set; }

        public DbSet<RoadAppWEB.Models.facility>? facility { get; set; }

        public DbSet<RoadAppWEB.Models.road>? road { get; set; }

        public DbSet<RoadAppWEB.Models.datadict>? datadict { get; set; }

        public DbSet<AllNodesRes>? AllNodesRes { get; set; }

        public DbSet<RoadAppWEB.Models.HubNodeRes>? HubNodeRes { get; set; }
    }
}
