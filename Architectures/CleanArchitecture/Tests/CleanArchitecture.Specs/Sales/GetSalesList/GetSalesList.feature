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
	Then the sales dataset should be returned

Scenario: Get a List of Sales (including deleted products)
	Given "default" dataset
	And products with following names are marked as deleted
		| Name      |
		| Spaghetti |
		| Ravioli   |
	When I request a list of sales
	Then the sales dataset should be returned