# Dental Office reservation

Dental office booking management app made with ASP.NET Core (.NET8)

Based on [Felipe Gavilan's Udemy course](https://www.udemy.com/course/clean-architecture-with-aspnet-core/)

This repository will implement:

- Clean Architecture 
- CQRS
- Mediator Pattern
- Unit test with MS Test
- Asp.NET Core with Identity
...


## TODO
### Disagreements with the instructor
- remove logic from constructors and put them in static factory method (and make private the concerned constructors)
- replace most of mock objects by stubs

### Additional features (WIP)
 - Data scheme modified : Dentist are related to one dental office currently => maybe several in the future
 - A dental office has a location, a list of dentists, and opening days and hours
 - integration tests (test containers?)