// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Supplier.InfrastructureLayer.Database;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> entity)
    {
        entity.ToTable("supplier_addresses");

        entity.HasKey(e => e.SupplierId);
        entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
        entity.Property(e => e.Street).HasColumnName("supplier_address_street");
        entity.Property(e => e.City).HasColumnName("supplier_address_city");
        entity.Property(e => e.PostalCode).HasColumnName("supplier_address_postal_code");
        entity.Property(e => e.Country).HasColumnName("supplier_address_country");
    }
}
