# Dental Office reservation

Dental office booking management app made with ASP.NET Core (.NET8)

Based on [Felipe Gavilan's Udemy course](https://www.udemy.com/course/clean-architecture-with-aspnet-core/)

This repository will implement:

- Clean Architecture
- basic CQRS
- Mediator Pattern (no library)
- Asp.NET Core with Identity
- Centralized package management
- Centralized settings for projects (Directory.Build.props file)
- Aspire for orchestration and metrics
...

## Tests and code checks
- Unit test with MS Test
- Integration tests with test containers and Xunit
- Architecture tests with ArchiUnitNET
- Static code analyzers : Meziantou, Roslynator and SonarAnalyzer

## TODO
### Disagreements with the instructor
- remove logic from constructors and put them in static factory method (and make private the concerned constructors)
- replace most of mock objects by stubs
- Repositories are not useful since Entity Framework already provides them
- Implementing Unit of Work pattern is not necessary since Entity Framework already provides it

### Additional features (WIP)
 - Data scheme modified : Dentist are related to one dental office currently => maybe several in the future
 - A dental office has a location, a list of dentists, and opening days and hours
