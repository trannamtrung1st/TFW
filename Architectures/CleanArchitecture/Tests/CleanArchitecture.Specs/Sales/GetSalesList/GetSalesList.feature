Feature: Get Sales List
	As a sales person
	I want to get a list of sales
	So I can find a sale to review
	Description:
	+ Get all sales recorded
	+ Example data returned
		| Id | Date       | Customer   | Employee   | Product   | Unit Price | Quantity | Total Price |
		| 1  | 2021-01-01 | Trung Tran | David Evan | Spaghetti | 15.0       | 1        | 15.0        |
	Rules:
	+ BR-000001

@mytag
Scenario: Get a List of Sales
	Given "default" dataset
	And dataset is created
	When I request a list of sales
	Then the sales dataset should be returned

Scenario: Get a List of Sales (including deleted products)
	Given "default" dataset
	And products with following names are marked as deleted
		| Name      |
		| Spaghetti |
		| Ravioli   |
	And dataset is created
	When I request a list of sales
	Then the sales dataset should be returned