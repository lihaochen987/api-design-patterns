# Manual Mapping

## Status

Successful Trial / Recommended

## Context

A common practice for mapping contracts to domain models is to use AutoMapper. However, the creator of AutoMapper has
advised against using it in this way.

## Decision

The decision is to create an extension method for each endpoint that manually maps the necessary attributes. This
extension method will handle mapping from the `Request` contract to the domain model and from the domain model to the
`Response` contract.

The extension method will also handle serialization and validation, including parsing strings from the `Request` that
are
mapped to long in the domain model. Given the strong recommendation from AutoMapper's creator against using it for
complex mapping logic, moving away from AutoMapper as a default is preferred.

## Consequences

This method duplicates the mapping logic a lot, but to a certain extent this duplication is justified because we want
more granular control in our requests and contracts. The `ToResponse()` and `ToDomain()` extension methods are useful in
tests, where we don't need to inject a mapper. More complex mapping become readable and easier to achieve compared with
using Automapper.

## Sources

https://www.jimmybogard.com/automappers-design-philosophy/ - Essentially saying that Automapper was not createdd to
stuff business logic inside of and just used for dumb dto mapping.