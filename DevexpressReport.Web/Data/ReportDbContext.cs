using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevexpressReport.Web.Models;

namespace DevexpressReport.Web.Data
{
    public class ReportDbContext:DbContext
    {
        public ReportDbContext(DbContextOptions<ReportDbContext> options):base(options)
        {

        }

        public DbSet<ReportModel> ReportModels { get; set; }
    }
}
