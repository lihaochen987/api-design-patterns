# Backend

## Development flow

1. CREATE the UpScripts
2. MODIFY the DbContext
3. APPEND to the domain models
4. UPDATE the field masks
5. DELETE the old fields
6. DELETE the field masks
7. REFACTOR the remaining stuff

## Refactoring the remaining stuff

1.You want to ignore calculated fields with `entity.Ignore(e => e.Field);`

2.If you're referencing a subset table, you need to establish the relationship with the code below:

```C#
            pricing.HasOne<DomainModels.Product>()
                .WithOne(p => p.Pricing) 
                .HasForeignKey<DomainModels.ProductPricing>(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);
```

and include the below in the `GET` and `LIST` standard methods:

```C#
        var product = await context.Products
            .Include(p => p.Pricing)
            .FirstOrDefaultAsync(p => p.Id == id);
```