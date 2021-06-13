Feature: Create new Sale
	As a sales person
	I want to create a sale
	To record a sales transaction

Scenario: Create a Sale
	Given "default" dataset
	Given the following sale info:
		| Customer      | Employee   | Product   | Quantity |
		| Martin Fowler | Eric Evans | Spaghetti | 2        |
	When I create a sale
	Then the following sales record should be recorded:
		# Date: empty mean "Today"
		| Date | Customer      | Employee   | Product   | Unit Price | Quantity | Total Price |
		|      | Martin Fowler | Eric Evans | Spaghetti | 5.00       | 2        | 10.00       |
	And the following sale-occurred notification should be sent to the inventory system:
		| Product ID | Quantity |
		| 1          | 2        |