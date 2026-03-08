# UVS Test readme

Dear reviewer,

## Intro

I know that testability is crucial for applications and also important for this assignment.
Considering this is a test assignment and that this application will never ever go to production I did not do tests as I spent much more time I expected for application which will be viewed once and never again. The sad part - time was not spent on development, but on trying to get docker running on my local machine..

So I did not manage to run docker on my machine, thus without being able to test it locally without docker I decided to totally strip away this broken whale. and postgresql part respectively. 
So I changed it at the very end of development stage as I my goal was to make it work.

Moreover, I understand that this application is way far from being perfect, so I am writing what I would do as a must if this app would go to production.

## Things must ToDo:

- add constraints in UVSDbContext on database model creation 
- add validations for queries and commands using fluent validator
- add xUnit tests (as unit and integration tests)
- add exception middleware
- add mapping
- add .gitignore

## How to run 

1. Build solution
2. Run UVS.Console



