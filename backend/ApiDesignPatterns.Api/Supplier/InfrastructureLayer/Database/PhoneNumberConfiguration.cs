// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Supplier.InfrastructureLayer.Database;

public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
{
    public void Configure(EntityTypeBuilder<PhoneNumber> entity)
    {
        entity.ToTable("supplier_phone_numbers");

        entity.HasKey(e => e.SupplierId);
        entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
        entity.Property(e => e.CountryCode).HasColumnName("supplier_phone_country_code");
        entity.Property(e => e.AreaCode).HasColumnName("supplier_phone_area_code");
        entity.Property(e => e.Number).HasColumnName("supplier_phone_number");
    }
}
