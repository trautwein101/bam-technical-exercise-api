
## Approach

- Reviewed the existing architecture (ASP.NET Core + MediatR)
- Identified gaps between implementation and stated requirements
- Prioritized backend implementation before UI
- Focused on maintainability, clarity, and alignment with requirements


## Observations

### AstronautDutyController
- GetAstronautDutiesByName currently sends GetPersonByName instead of GetAstronautDutiesByName. Previous query does not return astronaut duty history.

### GetAstronautDutiesByName
- String interpolation {request.Name} with SQL injection risk. 
- Possible NULL reference will cause query to fail



### Person Controller
- Requirement calls for add/update by name, but only create is supported
- CreatePerson uses a raw string instead of a request model (limits validation)
- Repeated try/catch blocks in each action (could be centralized to middleware)
- Logging not implemented per task requirements


### CreatePerson.cs
- Requirement calls for add/update by name, but only create is supported
- BadHttpRequestException("Bad Request"); should be more descriptive 'Person already exists' and not generic
- No validation for Name is NULL or empty

### ControllerBaseExtensions.cs
- Uses a custom response model for StatusCodes, which is consistent but differs from standard asp.net reusults





### CreateAstronautDutyPreProcessor
- Possible SQL injection attack on CreateAstronautDutyResult queries 
- verifyNoPreviousDuty preprocessor should have duplicate validation check with person as two astronauts might have the same title/date
- BadHttpRequestEx- ceptions are generic and should be more descriptive for debugging purposes
- Miss-match with career end date being one day before retired duty start date. If RETIRED CareerEndDate not following rule.




## Improvements

### AstronautDutyController
- Updated GET action to send GetAstronautDutiesByName instead of GetPersonByName -done
- Added try/catch to CreateAstronautDuty for consistent exception handling -done

### GetAstronautDutiesByName
- Parameterized SQL queries to remove injection risk - done
- Added null check before using person.PersonId - done



###  ControllerBaseExtensions.cs
- Consider using built-in ASP.NET result types and keeping BaseResponse focused on data and messaging. 
- For consistancy CreateAstronautDuty is missing exception handling 

# Person Controller
- Introduce centralized exception handling via middleware or MediatR pipeline behaviors
- Implement structured logging persisted to the database
- Add unit tests for key business logic
- Improve request validation and error handling





### CreateAstronautDutyPreProcessor
- Update queries to include paramaterized queries
- Update verifyNoPreviousDuty to include unique indentifier after lookup
- Update BadHttpRequestExceptions to be more descriptive

### Data.Person.cs
- Index  Person.Name for query optimization


## Future Improvements
