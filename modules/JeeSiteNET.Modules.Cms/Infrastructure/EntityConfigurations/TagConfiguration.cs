using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Cms_Tag");
        builder.HasKey(e => e.TagName);
        builder.Property(e => e.TagName).HasMaxLength(200);
    }
}
