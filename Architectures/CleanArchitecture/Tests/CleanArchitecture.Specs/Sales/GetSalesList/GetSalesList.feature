Feature: Get Sales List
	As a sales person
	I want to get a list of sales
	So I can find a sale to review
	Rules:
	+ BR-000001

@mytag
Scenario: Get a List of Sales
	Given "default" dataset
	When I request a list of sales
	Then the following sales list should be returned:
		| Id | Date       | Customer      | Employee   | Product   | Unit Price | Quantity | Total Price |
		| 1  | 2001-02-03 | Martin Fowler | Eric Evans | Spaghetti | 5.00       | 1        | 5.00        |
		| 2  | 2001-02-04 | Uncle Bob     | Greg Young | Lasagna   | 10.00      | 2        | 20.00       |
		| 3  | 2001-02-05 | Kent Beck     | Udi Dahan  | Ravioli   | 15.00      | 3        | 45.00       |

Scenario: Get a List of Sales (including deleted product)
	Given "default" dataset
	And products with following names are marked as deleted
		| Name      |
		| Spaghetti |
		| Ravioli   |
	When I request a list of sales
	Then the following sales list should be returned:
		| Id | Date       | Customer      | Employee   | Product   | Unit Price | Quantity | Total Price |
		| 1  | 2001-02-03 | Martin Fowler | Eric Evans | Spaghetti | 5.00       | 1        | 5.00        |
		| 2  | 2001-02-04 | Uncle Bob     | Greg Young | Lasagna   | 10.00      | 2        | 20.00       |
		| 3  | 2001-02-05 | Kent Beck     | Udi Dahan  | Ravioli   | 15.00      | 3        | 45.00       |