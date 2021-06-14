Feature: Remove product
	As an admin
	I want to delete products
	So I can remove defected or obsolete products out of my catalog
	Rules: 
	+ BR-000002

@mytag
Scenario: Remove a product
	Given "default" dataset
	And dataset is created
	When remove the product "Spaghetti"
	Then that product is still in data store
	But it is marked as deleted