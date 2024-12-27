// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Supplier.InfrastructureLayer.Database;

public class SupplierConfiguration : IEntityTypeConfiguration<DomainModels.Supplier>
{
    public void Configure(EntityTypeBuilder<DomainModels.Supplier> entity)
    {
        entity.ToTable("suppliers");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("supplier_id");
        entity.Property(e => e.FirstName).HasColumnName("supplier_firstname");
        entity.Property(e => e.LastName).HasColumnName("supplier_lastname");
        entity.Property(e => e.Email).HasColumnName("supplier_email");
        entity.Property(e => e.CreatedAt).HasColumnName("supplier_created_at");
        entity.HasOne(e => e.Address)
            .WithOne()
            .HasForeignKey<Address>(a => a.SupplierId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_supplier_id");
        entity.HasOne(e => e.PhoneNumber)
            .WithOne()
            .HasForeignKey<PhoneNumber>(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_supplier_id");
    }
}
