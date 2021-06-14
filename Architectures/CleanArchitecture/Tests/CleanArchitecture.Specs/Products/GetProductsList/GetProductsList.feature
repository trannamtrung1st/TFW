Feature: Get Products List
	As a sales person
	I want to get a list of products
	So I can inspect the products
	Description:
	+ Get all products in catalog
	+ Example data returned
		| Id | Name    | Unit Price |
		| 1  | Mì trộn | 10.00      |
	Rules:

Scenario: Get a List of Products
	Given "default" dataset
	And dataset is created
	When I request a list of products
	Then the products dataset should be returned