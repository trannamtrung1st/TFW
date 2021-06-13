Feature: Get Employees List
	As a sales person
	I want to get a list of employees
	So I can inspect the employees

Scenario: Get a List of Employees
	Given "default" dataset
	When I request a list of employees
	Then the employees dataset should be returned