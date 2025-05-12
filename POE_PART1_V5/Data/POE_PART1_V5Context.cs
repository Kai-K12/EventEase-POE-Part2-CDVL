using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POE_PART1_V5.Models;

namespace POE_PART1_V5.Data
{
    public class POE_PART1_V5Context : DbContext
    {
        public POE_PART1_V5Context (DbContextOptions<POE_PART1_V5Context> options)
            : base(options)
        {
        }

        public DbSet<POE_PART1_V5.Models.Booking> Booking { get; set; } = default!;
        public DbSet<POE_PART1_V5.Models.Event> Event { get; set; } = default!;
        public DbSet<POE_PART1_V5.Models.Venue> Venue { get; set; } = default!;
    }
}
