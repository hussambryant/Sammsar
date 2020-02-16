using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.API.Data
{
    public class IdentityServerDbContext : IdentityDbContext<User, Role, Guid>
    {
        public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class User : IdentityUser<Guid>
    {
        public UserType UserType { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsEnabledByAdmin { get; set; }
        public bool IsActivated { get; set; }
        public string Language { get; set; }
    }

    public enum UserType
    {
        User = 0,
        Partner = 1,
        Operation = 2
    }

    public class Role : IdentityRole<Guid>
    {

    }
}