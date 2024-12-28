// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace backend.Supplier.InfrastructureLayer.Database;

public class SupplierDbContext(DbContextOptions<SupplierDbContext> options) : DbContext(options)
{
    public DbSet<DomainModels.Supplier> Suppliers { get; init; }
    public DbSet<SupplierView> SupplierViews { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new PhoneNumberConfiguration());
        modelBuilder.ApplyConfiguration(new SupplierViewConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
