using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        builder.HasData(
            new ApplicationUser()
            {
                UserName = "admin@seesharp.com",
                Email = "admin@seesharp.com",
                Id = "7a71af0b-c442-4264-9808-3267afc10207",
                FirstName = "System",
                LastName = "Admin",
                NormalizedUserName = "ADMIN@SEESHARP.COM",
                NormalizedEmail = "ADMIN@SEESHARP.COM",
                PasswordHash = hasher.HashPassword(null, "@lloU!1"),
                EmailConfirmed = true
            },
            new ApplicationUser()
            {
                UserName = "user@seesharp.com",
                Email = "user@seesharp.com",
                Id = "0b435a99-5fa3-490a-a0fd-301cabbf0aab",
                FirstName = "System",
                LastName = "User",
                NormalizedUserName = "USER@SEESHARP.COM",
                NormalizedEmail = "USER@SEESHARP.COM",
                PasswordHash = hasher.HashPassword(null, "@llou!"),
                EmailConfirmed = true
            }
        );
    }
}
