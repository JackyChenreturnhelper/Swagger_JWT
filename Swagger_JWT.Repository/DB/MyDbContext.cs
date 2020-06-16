using Microsoft.EntityFrameworkCore;
using Swagger_JWT.Common.Helper;
using Swagger_JWT.Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swagger_JWT.Repository.DB
{
    public class MyDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var IsMysql = Appsettings.app(new string[] { "ConnectionStrings", "IsMySQL" }).ObjToBool();
            string ConnectionStrings = IsMysql ? Appsettings.app(new string[] { "ConnectionStrings", "MySQL" }) :
                Appsettings.app(new string[] { "ConnectionStrings", "SQL" });
            if (IsMysql)
            {
                optionsBuilder.UseMySql(ConnectionStrings);
            }
            else
            {
                optionsBuilder.UseSqlServer(ConnectionStrings);
            }
          

        }
        public DbSet<ApiClaims> ApiClaims { get; set; }

        public DbSet<AspNetUsers> AspNetUsers { get; set; }
    }
}
