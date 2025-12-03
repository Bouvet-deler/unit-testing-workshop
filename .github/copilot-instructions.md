# Unit test instructions

## General Guidelines
- Use Nunit as the testing framework for C# code, and JUnit for Java code.
- Use FakeItEasy for mocking dependencies (Mockito for Java).

## Naming Conventions
- Test classes should be named with the suffix `Tests` (e.g., `ProductServiceTests`).
- Test methods should follow the pattern `MethodName_StateUnderTest_ExpectedBehavior` (e.g., `AddProduct_ValidProduct_ReturnsTrue`).

## Structure of a Unit Test
- Use the Arrange-Act-Assert (AAA) pattern:
  - **Arrange**: Set up any necessary objects, mocks, and data.
  - **Act**: Call the method under test.
  - **Assert**: Verify that the outcome is as expected.