# Scoped-Implementation

	- Scoped DI
		- DI container shares the same service instance within scope but would create a new instance of service for different scopes
		- For WebAPI requests, each request is considered as a scoped. 
		- Meaning services registered with scoped instance would give us seperate instance for each request. If instance is requested multiple times within request, then it would get the same instance
		- But since this is a console application, if we directly use Scope (As seen in ProductTransformerExperiment, then code will misbehave in case of more than 1 items present in product-import.csv)
		- So, correct implementation is done in ProductTransformer where we are creating a new scope per Product (For 2 products, both of them will have a separate scope and the code would work as expected)

	- Application type - Console application
	
	- Implementation
		- Resolve different implementation for IProductTransformer for verifying outcome mentioned below
		
	- Outcome
		- We want to ensure code (Context, CurrencyNormalizer and NameDecapitalizer) are scoped per PRODUCT. 
		- Here since we have 2 products, we want to ensure that all 3 classes are tied to the PRODUCT against which they are executing.

	- For testing scoped implementation
		- 2 implementations for IProductTransformer
			- ProductTransformer
			- ProductTransformerExperiment

	- What does the code do?
		- In simple words, the program takes in product-import.csv file, applies transformations on it and outputs a product-output.csv file
		- Core logic resides in ProductImporter class
		- Product is extracted out of product-import.csv file one by one and processed and written to product-output.csv file

	- Transformation
		- There are 4 classes in picture here.
			- ProductTransformer/ProductTransformerExperiment (Sharing same interface) - Explained below
			- ProductTransformationContext (Along with Interface) - Used as a context class to hold the Product model. Has 2 properties - _initialProduct and _product
			- CurrencyNormalizer (Along with Interface) - Actual transformation logic
			- NameDecapitalizer (Along with Interface) - Actual transformation logic

	- Registering types
		- Registered ProductTransformationContext, CurrencyNormalizer and NameDecapitalizer as Scoped since we want them unique per product

	- Using ProductTransformerExperiment
		- With this class, when a new instance of ProductTransformerExperiment is created, all the dependencies within that class also will be created
		- Execution
			- For first product i.e. "Blue Shoes"
				- Using SetProduct(), _initialProduct property will be set in ProductTransformationContext
				- Both transformations (name and currency) will be executed and will modify the product. Updated product will be set in context
				- When it checks for IsProductChanged(), because there are actual transformations that happen for this product, it will return true
				- Since it returned true, using Import statistics, count will be incremented for transformation
				- When we return Product, we would return updated product (with name as "blue shoes") back

			- For second product i.e. "purple shoes"
				- Using SetProduct(), _initialProduct property WILL NOT BE SET since the value is scoped(Default)
				- Both transformations (name and currency) will be executed and they will not modify the product. 
				- When it checks for IsProductChanged(), it will still return true SINCE it checks current product("purple shoes" against _initialProduct ("Blue Shoes") which is INCORRECT
				- THIS RESULTS IN POLLUTING BEHAVIOR.
				- Since it returned true, using Import statistics, count will be incremented for transformation
				- When we return prodyct, we would return updated product back

	- Using ProductTransformer
		- With this class, we are creating a new Scope from ScopeFactory within ApplyTransformation method
		- This will CREATE A NEW SCOPE FOR EACH PRODUCT (As Expected)
		- Execution
			- For First product i.e. "Blue Shoes"
				- Using SetProduct(), _initialProduct property will be set in ProductTransformationContext
				- Both transformations (name and currency) will be executed and will modify the product. Updated product will be set in context
				- When it checks for IsProductChanged(), because there are actual transformations that happen for this product, it will return true
				- Since it returned true, using Import statistics, count will be incremented for transformation
				- When we return Product, we would return updated product (with name as "blue shoes") back
			
			- For second product i.e. "purple shoes"
					- Using SetProduct(), _initialProduct property WILL BE SET to intial product since the value is scoped(Default)
					- Both transformations (name and currency) will be executed and they will not modify the product
					- When it checks for IsProductChanged(), it will return FALSE SINCE it no updates are made to product (Source product already had correct formatting so no transformation required)
					- Since it returned false, using import statistics, count WILL NOT BE INCREMENTED for this transformatio
					- When we return product, it would be returned back


	- This is how we could leverage scoped implementation