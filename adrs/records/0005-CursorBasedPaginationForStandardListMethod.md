# Cursor Based Pagination for Standard List Methods

## Status

Partial Success / Conditional Recommendation

## Context

The API Design Patterns book suggests using cursor-based pagination over the more common offset-based pagination.

## Decision

This repository adopts cursor-based pagination. By supplying a cursor, the client can continue from where they left off
when using the List method. The book highlights advantages in terms of performance, avoiding skipped or duplicate
records, and improved scalability.

## Consequences

The implementation has been partially successful. The book advises hashing the PageToken field to prevent clients from
depending on it directly. This hasn’t been done, making this a conditional recommendation. Hashing the PageToken could
add unnecessary complexity, so it’s suggested only if deemed beneficial.

## Sources

https://www.manning.com/books/api-design-patterns - Source of the recommendation