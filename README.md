#Sharpforms - a dynamic form generator

###Overview:
This project is created in ASP.NET and C# and is a simple clone of the Google Forms application. 
It is possible to create and store form structures as well as fill in specific forms and display the data.
Generated links can be used to access already created forms for filling in.

###Details regarding the Business Layer:
The Business Object is stored as a class library. The project strictly follows the domain model 
paradigm which means that every entity in the database is represented as a class in the Business Object.
Furthermore all these entity classes have internal constructors so objects can only be initialised or
returned by the one and only factory class "cMain". This has been done to prevent too much functionality
and therefore risk of persistency problems in the Presentation Layer.