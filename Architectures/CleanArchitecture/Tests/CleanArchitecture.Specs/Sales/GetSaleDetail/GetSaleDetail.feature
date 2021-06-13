Feature: Get Sale Detail
	As a sales person
	I want to get the details of a sale
	So that I can review the sale

Scenario: Get Sale Details
	Given "default" dataset
	When I request the sale details for sale 1
	Then the following sale details should be returned:
		| Id | Date       | Customer Name | Employee Name | Product Name | Unit Price | Quantity | Total Price |
		| 1  | 2001-02-03 | Martin Fowler | Eric Evans    | Spaghetti    | 5.00       | 1        | 5.00        |