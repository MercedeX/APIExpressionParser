
# API Expression Parser
This is an experimental work. Objective is to allow a complex expression from the API and after validation generate a valid SQL statement and/or expression. 

Supported Operations:
- AND
- OR
- NOT
- <
- <=
- &gt;
- &gt;=
- =
- !=
- IN
- BETWEEN
- ()    _as known as **child expressions**_ 

## Sample Expressions
Customer.Name = John AND Customer.Age >32 OR Customer.CreatedOn BETWEEN (22/10/2020 AND 22/12/2020)

will generate 

SELECT * 
FROM Customer
Where (Name like 'John') AND (Age >32) OR (CreatedOn BETWEEN '22/10/2020' AND 22/12/2020')


## Features
- It is possible to replace the column names so that the API uses different names than the database column names.
- Queries can be deep through child queries. 
- Joins can be identified (should be provided to the parser)
- Queries can be validated.
- Expressions should not have SQL resemblance. 
