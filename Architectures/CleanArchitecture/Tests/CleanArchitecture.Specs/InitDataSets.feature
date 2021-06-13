Feature: _Init datasets
	Init datasets for the whole projects

Scenario: Init datasets "default"
	Given the name of new dataset is "default"
	And the following Customers
		| Id | Name          |
		| 1  | Martin Fowler |
		| 2  | Uncle Bob     |
		| 3  | Kent Beck     |
	And the following Employees
		| Id | Name       |
		| 1  | Eric Evans |
		| 2  | Greg Young |
		| 3  | Udi Dahan  |
	And the following Products
		| Id | Name      | Unit Price | Deleted |
		| 1  | Spaghetti | 5.00       | false   |
		| 2  | Lasagna   | 10.00      | false   |
		| 3  | Ravioli   | 15.00      | false   |
	And the following Sales
		| Id | Date       | Customer      | Employee   | Product   | Unit Price | Quantity | Total Price |
		| 1  | 2001-02-03 | Martin Fowler | Eric Evans | Spaghetti | 5.00       | 1        | 5.00        |
		| 2  | 2001-02-04 | Uncle Bob     | Greg Young | Lasagna   | 10.00      | 2        | 20.00       |
		| 3  | 2001-02-05 | Kent Beck     | Udi Dahan  | Ravioli   | 15.00      | 3        | 45.00       |
	When init the dataset
	Then it should be added to the list of datasets successfully

Scenario: Init datasets "alternative"
	Given the name of new dataset is "alternative"
	And the following Customers
		| Id | Name          |
		| 1  | Martin Fowler |
		| 2  | Kent Beck     |
		| 3  | TNT           |
	And the following Employees
		| Id | Name          |
		| 1  | Eric Evans    |
		| 2  | Michael Kevin |
		| 3  | Udi Dahan     |
	And the following Products
		| Id | Name    | Unit Price |
		| 1  | Mì trộn | 5.00       |
		| 2  | Cơm nấm | 10.00      |
		| 3  | Ravioli | 15.00      |
	And the following Sales
		| Id | Date       | Customer      | Employee      | Product | Unit Price | Quantity | Total Price |
		| 1  | 2001-02-03 | Martin Fowler | Eric Evans    | Mì trộn | 5.00       | 1        | 5.00        |
		| 2  | 2001-02-04 | TNT           | Michael Kevin | Cơm nấm | 10.00      | 2        | 20.00       |
		| 3  | 2001-02-05 | Kent Beck     | Udi Dahan     | Ravioli | 15.00      | 3        | 45.00       |
	When init the dataset
	Then it should be added to the list of datasets successfully