using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations;
public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string>()
            {
                RoleId = "a876e787-64a9-40a7-a745-aedfae4c7560",
                UserId = "7a71af0b-c442-4264-9808-3267afc10207"
            },
            new IdentityUserRole<string>()
            {
                RoleId = "99d48c8e-8f46-44d7-8068-e16a958facde",
                UserId = "0b435a99-5fa3-490a-a0fd-301cabbf0aab"
            }
        );
    }
}
