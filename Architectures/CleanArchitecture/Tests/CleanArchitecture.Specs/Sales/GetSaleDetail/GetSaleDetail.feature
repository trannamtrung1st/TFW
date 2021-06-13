Feature: Get Sale Detail
	As a sales person
	I want to get the details of a sale
	So that I can review the sale

Scenario Outline: Get Sale Detail
	Given "default" dataset
	When I request the sale detail for sale <saleId>
	Then the correct sale from dataset should be returned

	Examples:
		| saleId |
		| 1      |
		| 2      |
		| 3      |