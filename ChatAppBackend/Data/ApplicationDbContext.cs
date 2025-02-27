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
        public DbSet<AppChannel> Channels { get; set; }
        public DbSet<AppChannelChat> ChannelChat { get; set; }
        public DbSet<AppChannelUser> ChannelUser { get; set; }
        public DbSet<AppChatMessage> ChatMessages { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppChannelChat>().HasKey(cc => new { cc.ChannelId, cc.ChatId });
            builder.Entity<AppChannelUser>().HasKey(uc => new { uc.UserId, uc.ChannelId });
            builder.Entity<AppChatMessage>().HasKey(cm => new { cm.ChatId, cm.MessageId });

            builder.Entity<AppChannelUser>()
                    .HasOne(uc => uc.User)
                    .WithMany(u => u.ChannelUser);

            builder.Entity<AppChannelUser>()
                    .HasOne(uc => uc.Channel)
                    .WithMany(c => c.ChannelUser);

            builder.Entity<AppChannelChat>()
                .HasOne(cc => cc.Channel)
                .WithMany(c =>c.ChannelChats);

            builder.Entity<AppChatMessage>()
                .HasOne(cm => cm.Chat)
                .WithMany(c => c.ChatMessage);

            base.OnModelCreating(builder);
        }

    }
}
