# Backend

## Development flow

1. CREATE the UpScripts
2. MODIFY the DbContext
3. APPEND to the domain models
4. UPDATE the field masks
5. DELETE the old fields
6. DELETE the field masks
7. REFACTOR the remaining stuff

## Database Integration
- Essentially this repository works by calling `Views` from the database, rather than stored procedures. Each view is paired with the respective `trg_delete_from_products_view` and `trg_update_products_view` respectively.