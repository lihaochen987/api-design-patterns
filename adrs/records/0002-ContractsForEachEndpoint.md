# Contracts for each endpoint

## Status

Partial Success / Conditional Recommendation

## Context

Building on the decision from [VerticalSliceArchitecture](./0001-VerticalSliceArchitecture.md), this approach adopts an
endpoint-centric design by creating individual request and response contracts for each endpoint, rather than adding more
layers within each vertical slice.

## Decision

Implement separate request and response contracts for each endpoint. This approach is not strongly recommended due to
the lack of supporting sources.

## Consequences

Improves clarity by emphasizing that each contract is tied to a single endpoint, as intended. Request and response
contracts can also be nested under the corresponding .cs files, providing a visual cue for their dependency.

## Sources

N/A