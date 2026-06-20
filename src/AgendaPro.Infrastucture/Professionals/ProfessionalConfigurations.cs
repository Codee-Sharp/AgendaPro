using AgendaPro.Domain.Professionals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaPro.Infrastucture.Professionals;

public class ProfessionalConfigurations : IEntityTypeConfiguration<ProfessionalModel>
{
    public void Configure(EntityTypeBuilder<ProfessionalModel> builder)
    {
        builder.ToTable("Professionals");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.Email)
            .HasMaxLength(254);

        builder.Property(p => p.Phone)
            .HasMaxLength(20);

        builder.Property(p => p.Specialty)
            .HasMaxLength(100);

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.HasIndex(p => p.Email)
            .IsUnique()
            .HasFilter("\"Email\" IS NOT NULL AND \"IsActive\" = true")
            .HasDatabaseName("IX_Professionals_Email");
    }
}
