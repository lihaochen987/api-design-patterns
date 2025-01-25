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

## Development Flow

1. CREATE and DESIGN the Database via UpScripts
2. CREATE the View for our Get / List standard methods
3. MODIFY the Domain Model, ChatGPT it from the View script using both the relevant join tables and the view itself
4. CHECK Delete standard method still works
5. MODIFY Create and Replace standard methods
6. MODIFY List, Get and Update standard methods along with field masks

## Notable areas
- ColumnMappers are used to map between a property and the column name in the database, used for filtering in the List standard method.
- FieldPaths are used for valid entries in the FieldMask, used for partial updates in the Get and Update standard methods.
