
using ChatAppBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Data
{
    public sealed class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, 
        AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public DbSet<AppChat> Chats { get; set; }
        public DbSet<AppMessage> Messages { get; set; }
        public DbSet<AppUserChat> UserChat { get; set; }
        public DbSet<AppChatMessage> ChatMessages { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUserChat>().HasNoKey().HasKey(uc => new { uc.UserId, uc.ChatId });

            builder.Entity<AppChatMessage>().HasKey(cm => new { cm.ChatId, cm.MessageId });
        }

    }
}
