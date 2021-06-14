Feature: Get Customers List
	As a sales person
	I want to get a list of customers
	So I can inspect the customers

Scenario: Get a List of Customers
	Given "default" dataset
	And dataset is created
	When I request a list of customers
	Then the customers dataset should be returned