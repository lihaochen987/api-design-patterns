# Vertical Slice Architecture

## Status

Partial Success / Conditional Recommendation

## Context

In standard application architectures (such as N-tier), we commonly work with the controller, application, and
repository layers. This is an experiment on the alternative, where a vertical slice is applied based on domain entities
instead of functional layers.

## Decision

The approach provides clearer separation of concerns, which may simplify extracting the repository if needed. While it
feels effective in this repository, several sources I prefer advise against fully endorsing this architecture. It works
well on a smaller scale, though potential challenges on a larger scale are uncertain.

Each folder is sliced through entity objects, value objects related only to the specific entity have been thrown in the
same slice as well.

## Consequences

Improves visibility into the folder’s purpose, potentially allowing easier segmentation of the repository into smaller
services if required.

## Sources

N/A