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
- **Polymorphic resources** : evident from endpoints in the `Product` folder which adds additional fields based on the
  `Category`
- **Cross-References**: evident from endpoints in the `Review` folder which references a `Product`

The other patterns mentioned, but not yet implemented from the book include:

- **Custom methods**
- **Long-running operations**
- **Association resources**
- **Add and remove custom methods**
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
2. MODIFY the View
3. MODIFY the Domain Model (ChatGPT it from the SQL script lol)
4. MODIFY the DbContext
5. CHECK Delete standard method still works
6. MODIFY Create and Replace standard methods
7. MODIFY List, Get and Update standard methods along with field masks

## Note to self

- 3 hours to create product_pet_foods in the table and modify all the standard method endpoints (excluding unit tests).
