# Senior BackEnd Engineer Exercise

##### [Live Demo](https://flight-check-in.scuticode.com/swagger/index.html)


Design and implement a .Net (Core) Web API solution, which automates check-in into a flight. 
The solution should register a passenger and his baggage to a flight. 

***
Assumptions:
* Seats support is required. 
* A flight, passenger, and their details already exist in the database. 
* Using abstractions (repository, etc.) to access a DB is enough. 

***
Invariants: 
* Aircraft has a limited load weight. So the total baggage weight must be controlled. 
* Aircraft's seats are limited. Beware of overbooking. 
* Each passenger is allowed to check-in a limited number of bags. 
* The total weight of a passenger's baggage is also limited. 

***
Technical requirements: 
* API controller(s) with proper routes (are) mandatory. 
* At least a few unit tests. 
* Clean, maintainable, production-ready code. 



  



