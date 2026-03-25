
## Approach

- Reviewed the existing architecture (ASP.NET Core + MediatR)
- Identified gaps between implementation and stated requirements
- Prioritized backend implementation before UI
- Focused on maintainability, clarity, and alignment with requirements


## Observations

### AstronautDutyController
- GetAstronautDutiesByName currently sends GetPersonByName instead of GetAstronautDutiesByName. Previous query does not return astronaut duty history.

### GetAstronautDutiesByName & GetPersonByName
- String interpolation {request.Name} with SQL injection risk. 
- Identified potential null reference risks in query handling

### CreatePerson.cs
- Requirement calls for add/update by name, but only create is supported
- BadHttpRequestException("Bad Request"); should be more descriptive 'Person already exists' and not generic
- No validation for Name is NULL or empty

### Person Controller
- Requirement calls for add/update by name, but only create is supported
- CreatePerson uses a raw string instead of a request model (limits validation)
- Repeated try/catch blocks in each action (could be centralized to middleware)
- Logging not implemented per task requirements

### ControllerBaseExtensions.cs
- Uses a custom response model for StatusCodes, which is consistent but differs from standard asp.net results

### CreateAstronautDutyPreProcessor
- Possible SQL injection attack on CreateAstronautDutyResult queries 
- BadHttpRequestEx- ceptions are generic and should be more descriptive for debugging purposes
- Mismatch  with career end date being one day before retired duty start date. If RETIRED CareerEndDate not following rule.




## Improvements

### AstronautDutyController
- Updated GET action to send GetAstronautDutiesByName instead of GetPersonByName -done
- Added try/catch to CreateAstronautDuty for consistent exception handling -done

### GetAstronautDutiesByName & GetPersonByName
- Parameterized SQL queries to remove injection risk - done
- Added not-found handling for missing person - done

### CreatePerson.cs
- Requirement calls for add/update by name, but only create is supported - done
- BadHttpRequestException("Bad Request"); should be more descriptive 'Person already exists' and not generic - done

### Person Controller
- Improve request validation and error handling - done
- CreatePerson uses a raw string instead of a request model (limits validation) - done
- Added Update Person (PUT) and create new MediatR for separation of concerns -done

### CreateAstronautDutyPreProcessor
- Update BadHttpRequestExceptions to be more descriptive - done
- Miss-match with career end date being one day before retired duty start date. If RETIRED CareerEndDate not following rule. - done
- Updated verifyNoPreviousDuty preprocessor to validate duplicates per person (multiple astronauts can share the same duty title and date) - done

### Data.Person.cs
- Index  Person.Name for query optimization  - done

### Process Logging
- Implemented database-backed logging via ProcessLog entity and table  - done
- Added IProcessLogService abstraction for centralized logging of business events  - done
- Logged successful operations in CreatePersonHandler  - done
- Logged exceptions in PersonController for failed requests  - done

### Unit Testing
- Added basic unit tests for CreatePerson to cover key validations and successful scenarios - done



## Future Improvements

### Person Controller
- Introduce centralized exception handling via middleware or MediatR pipeline behaviors  - to consider

### CreatePerson.cs
- No validation for Name is NULL or empty - todo

### CreateAstronautDutyPreProcessor
- Update queries to include parameterized queries to prevent SQL injection attack - todo
