# Backend

## Objective

The purpose of the backend project is based heavily off replicating the two books below:

1. [Database Design for Mere Mortals](https://www.oreilly.com/library/view/database-design-for/9780133122282/)
2. [Api Design Patterns](https://www.manning.com/books/api-design-patterns?a_aid=elevysi)

Where the first book was used for the design of the database while the second book served as patterns and practices
which this repository places in action. These include:

- **Standard Methods** : evident from the endpoints in the `Product` folder
- **Partial updates and retrievals** : evident from the `List` and `Update` endpoints in the `Product` folder
- **Singleton sub-resources** : evident from the `ProductPricingControllers` in the `Product` folder. These are
  typically used for a hierarchical 1-1 relationship.
- **Pagination** : evident from the `List` standard method in the `Product` folder
- **Filtering** : evident from the `List` standard method in the `Product` folder

The other patterns mentioned, but not yet implemented from the book include:

- **Custom methods**
- **Long-running operations**
- **Cross references**
- **Association resources**
- **Add and remove custom methods**
- **Polymorphism**
- **Copy and move**
- **Batch operations**
- **Criteria-based deletion**
- **Anonymous writes**
- **Importing and Exporting**
- **Versioning and compatability**
- **Soft deletion**
- **Request deduplication**
- **Request validation**
- **Resource revisions**
- **Request retrial**
- **Request authentication**

## Current Features

## Planned Entity Relationships

- A `Product` has a hierarchical 1-N relationship with a `Review`, this is demonstrated using the association pattern.
- A `Product`has an N-N relationship with a `Supplier`, this is demonstrated using the cross-reference pattern.
- Still need to come up with a 1-N relationship example to use the cross-reference pattern for.
- Still need to come up with a hierarchical N-N relationship to use the ... pattern for.

## Development Flow

1. CREATE and DESIGN the Database via UpScripts
2. MODIFY the View or Domain model
3. MODIFY the DbContext
4. UPDATE the relevant extension controller methods
5. ADD the field masks

## Database Integration

- Essentially this repository works by calling `Views` from the database, rather than stored procedures. Each view is
  paired with the respective `trg_delete_from_products_view` and `trg_update_products_view` respectively.