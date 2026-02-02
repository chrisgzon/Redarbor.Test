using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Redarbor.Test.Domain.Entities;

namespace Redarbor.Test.Infrastructure.Persistence.Write.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.CompanyId)
                .IsRequired();

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.PortalId)
                .IsRequired();

            builder.Property(e => e.RoleId)
                .IsRequired();

            builder.Property(e => e.StatusId)
                .IsRequired();

            builder.Property(e => e.CreatedOn)
                .IsRequired();

            builder.Property(e => e.UpdatedOn)
                .IsRequired();

            builder.Property(e => e.Fax)
                .HasMaxLength(20);

            builder.Property(e => e.Name)
                .HasMaxLength(100);

            builder.Property(e => e.Telephone)
                .HasMaxLength(20);

            // Index para búsquedas comunes
            builder.HasIndex(e => e.Email);
            builder.HasIndex(e => e.Username);
            builder.HasIndex(e => e.CompanyId);
        }
    }
}
