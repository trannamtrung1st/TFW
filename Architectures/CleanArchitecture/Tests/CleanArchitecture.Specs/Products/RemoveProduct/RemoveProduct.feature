Feature: Remove product
	As an admin
	I want to delete products
	So I can remove defected or obsolete products out of my catalog
	Rules: 
	+ BR-000002

@mytag
Scenario: Remove a product
	Given "default" dataset
	When remove the product "Spaghetti"
	Then that product is marked as deleted
	But it is still stored in data store