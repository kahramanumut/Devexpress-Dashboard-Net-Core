using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevexpressReport.Web.Areas.Identity.Data;
using DevexpressReport.Web.Models.Auth;

[assembly: HostingStartup(typeof(DevexpressReport.Web.Areas.Identity.IdentityHostingStartup))]
namespace DevexpressReport.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}