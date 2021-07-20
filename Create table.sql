CREATE TABLE Employees
(
	EmployeeId int NOT NULL Primary Key Identity,
	SecondName nvarchar(max),
	FirstName nvarchar(max),
	Patronymic nvarchar(max),
	Position nvarchar(max),
	Salary int,
	EmploymentDate date,
	DismissalDate date
)