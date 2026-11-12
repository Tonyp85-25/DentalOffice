# Remove mediators for CRUD actions

## Context
Following the course, we have been using mediators for CRUD actions. It is highly questionnable since mediator pattern is made for more complex and repeatable workflows.


## Considered options

- Option 1: Remove mediators and pass directly dependencies to actions
- Option 2: Leaving code as it is

## Decision

I choose Option 1: Remove mediators and pass directly dependencies to actions, in order to simplify code and remove useless stages.

## Consequences
Mediator types related types such as `IRequestHandler` and `IRequest` are no longer used for CRUD actions, so their concrete implementations (`Command`
and `CommandHandler`are removed as well as related unit tests.
CRUD actions are now tested only with integration tests.



