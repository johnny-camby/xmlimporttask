# Xml file data importation into MSSQL database using ASP.NET Core MVC frontend, EF Core and XPath.

In this task i import an xml file using ASP.NET Core MVC frontend, the file's contents are then extracted using XPath expressions and then stored into an MSSQL database.

I chose to use XPath because 
- it allows bidirectional flow which means the traversal can be both ways from parent to child and child to parent as well.
- Queries are compact.
- Queries are easy to type and read.
- Syntax is simple for the simple and common cases.
- Queries are easily parsed.
- One can specify any path that can occur in an XML document and any set of conditions for the nodes in the path.
- One can uniquely identify any node in an XML document.
- Query conditions can be evaluated at any level of a document and are not expected to navigate from the top node of a document.
- I have used it for quite a while (:

![xpath](https://user-images.githubusercontent.com/129853285/229895335-6a231f56-80c2-4549-8e1d-313af0611881.png)

I use some Architectural patterns (MVC pattern), Design Patterns(Chain of Responsibility, Command,Iterator, etc..) and SOLID principles.

The startup project is the `WebXmlImporter project` and upon startup, database migrations are run that delete and recreate the databse but that process can be changed.
Then after the UI appears with a File upload input, a button together with an empty html table.

![no_data](https://user-images.githubusercontent.com/129853285/229884351-eb32ad1b-8d5b-4549-b765-032d4b089ce6.png)


After one has uploaded an xml file, it's contents are immediately displayed in html table where one can `Edit, View details and Delete a record`. There is also a button incase one wants to create a new record in the database.

![now_data](https://user-images.githubusercontent.com/129853285/229885154-81981cbc-93eb-4a66-b6d4-293256e21a99.png)

There Unit and Integration tests though not 100% coverage. Here am mainly testing for dependency injection in my 
- data repository CRUD operations.

![repo_test](https://user-images.githubusercontent.com/129853285/229888037-d4c594e8-83e9-4342-bae0-7d401448abb9.png)

I use `Faker` a nuget package to  generate fake data: names, addresses, phone numbers, etc.

![fake_data](https://user-images.githubusercontent.com/129853285/229889753-e54781ce-b75e-4804-931d-45e809a0185b.png)

- HomeController constructor and Index method

![controller_test](https://user-images.githubusercontent.com/129853285/229891666-ffc9142b-7996-400a-8e36-c3dd258ca153.png)


I also test my DbContext using an InMemory Provider that replaces the Database Provider.
- It emulate Relational Database Management System basic behaviors via in-memory lists.
- It's a better alternative for test mocks that were and are still used.

![dbcontext_test](https://user-images.githubusercontent.com/129853285/229892034-76ae050d-0aff-42d1-b863-50eb83d7f814.png)



