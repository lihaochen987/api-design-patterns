# Backend

## Current Entity Relationships

- A `Product` has a hierarchical 1-1 relationship with `ProductPricing`, this is demonstrated using the singleton
  sub-resource pattern.
- A `Product` has a hierarchical 1-N relationship with a `Review`, this is demonstrated using the association pattern.
- A `Product`has an N-N relationship with a `Supplier`, this is demonstrated using the cross-reference pattern.
- Still need to come up with a 1-N relationship example to use the cross-reference pattern for.
- Still need to come up with a hierarchical N-N relationship to use the ... pattern for.

## Development Flow

1. CREATE the UpScripts
2. MODIFY the DbContext
3. APPEND to the domain models
4. UPDATE the field masks
5. DELETE the old fields
6. DELETE the field masks
7. REFACTOR the remaining stuff

## Database Integration

- Essentially this repository works by calling `Views` from the database, rather than stored procedures. Each view is
  paired with the respective `trg_delete_from_products_view` and `trg_update_products_view` respectively.