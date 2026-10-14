# Separate back office features from patient features

## Context
Currently, the back office features are mixed with patient features and this raises several issues:
- both features share the same controller with same url path which does not ease authorization management
- they also share the same dependencies though back office features are mainly CRUD actions  with no complex logic, which is not the case for patient features


## Considered options

- Option 1: Separate back office and patient features in different controllers
- Option 2: Separate back office and patient features in different assemblies
- Option 3: Leaving code as it is

## Decision

I choose Option 1: Separate back office and patient features in different controllers. It is the quickest solution compared to potion 2 and gives controller-based authorization management and a separate dependencies management.
This also unlock future evolutions such as putting back office in a separated module or assembly.

## Consequences
Features are now splitted in different controllers that lives in separate folders in the API project and Application projects .




