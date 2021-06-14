Feature: Get Products List
	As a sales person
	I want to get a list of products
	So I can inspect the products
	Rules:
	+ BR-000003

Scenario: Get a List of Products
	Given "default" dataset
	And dataset is created
	When I request a list of products
	Then the products dataset should be returned

Scenario: Get a List of Products (excluding deleted products)
	Given "default" dataset
	And products with following names are marked as deleted
		| Name      |
		| Spaghetti |
		| Ravioli   |
	And dataset is created
	When I request a list of products
	Then the following products should be returned:
		| Id | Name    | Unit Price |
		| 2  | Lasagna | 10.00      |