// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Supplier.InfrastructureLayer.Database;

public class SupplierViewConfiguration : IEntityTypeConfiguration<SupplierView>
{
    public void Configure(EntityTypeBuilder<SupplierView> entity)
    {
        entity.ToView("suppliers_view");

        entity.Property(e => e.Id).HasColumnName("supplier_id");
        entity.Property(e => e.FullName).HasColumnName("supplier_fullname");
        entity.Property(e => e.Email).HasColumnName("supplier_email");
        entity.Property(e => e.CreatedAt).HasColumnName("supplier_created_at");
        entity.OwnsOne(e => e.Address, address =>
        {
            address.WithOwner();

            address.Property(a => a.Street)
                .HasColumnName("supplier_address_street");

            address.Property(p => p.City)
                .HasColumnName("supplier_address_city");

            address.Property(p => p.PostalCode)
                .HasColumnName("supplier_address_postal_code");

            address.Property(p => p.Country).HasColumnName("supplier_address_country");
        });
        entity.Property(e => e.PhoneNumber).HasColumnName("supplier_phone_number");
    }
}
