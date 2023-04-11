using FormulaEvaluator;
using static FormulaEvaluator.Evaluator;

//keeps track of the number of successful tests
int testsPassed = 0;

/// <summary>
/// Tester method meant to check if an expression works with the Evaluator. If it doesn't, will
/// return a description of the error in the console. If it does, reports a passed test in the
/// console as well as increases the tests passed value.
/// </summary>
/// <param name="expression"> the expression to be solved</param>
/// <param name="look"> the delegate variable</param> 
/// <param name="expected"> the expected return value</param>
/// <param name="description"> description of check as well as description of failure</param>
/// <returns></returns>
void Check(string expression, Lookup look, int expected, string description)
{
    if (Evaluator.Evaluate(expression, look) != expected)
        Console.WriteLine(description + " error");
    else
        Console.WriteLine("Test Pass!");
        testsPassed++;
}

/// <summary>
/// Tester method that assigns the delegate variable a value.
/// </summary>
/// <param name="s">the delegate variable</param>
/// <returns>the int value of the delegate variable</returns>
static int Variable(String s) {
    return 5;
}

/****************************************
  * Tests for Numbers
  * *****************************/
Check("5 + 5", null, 10, "Basic addition");
Check("5 - 5", null, 0, "Basic subtraction");
Check("5 * 5", null, 25, "Basic multiplication");
Check("5 / 5", null, 1, "Basic division");
Check("(5 + 5)", null, 10, "Basic addition with Parenthesis");
Check("(5) + (5)", null, 10, "Basic addition with Parenthesis on each int");
Check("(5 + 5) + (5 + 5)", null, 20, "Basic addition with Parenthesis on each expression");
Check("((5 + 5) + (5 + 5))", null, 20, "Basic addition with Parenthesis on each expression and wrapping the equation");

/****************************************
  * Tests for Variables
  * *****************************/
Check("a1 + a1", Variable, 10, "Basic Addition with variable with letter and number");
Check("a + a", Variable, 10, "Basic Addition with variable with letter");
Check("A + A", Variable, 10, "Basic Addition with variable with upercase letters");
Check("a_ + a_", Variable, 10, "Basic Addition with variable with letter and underscore");
Check("aa + aa", Variable, 10, "Basic Addition with variable with multiple letter");
Check("_ + _", Variable, 10, "Basic Addition with variable with underscore");
Check("_a + _a", Variable, 10, "Basic Addition with variable with underscore followed by a letter");
Check("_1 + _1", Variable, 10, "Basic Addition with variable with underscore followed by a number");
Check("(a1 + a1)", Variable, 10, "Basic Addition with variable in parenthesis");
Check("(a1) + (a1)", Variable, 10, "Basic Addition with variable in seperate parenthesis");

/****************************************
  * Tests for Numbers and Variables
  * *****************************/
Check("3 + a1zab", Variable, 8, "Basic Addition with number and variable with letters and numbers");
Check("3 + _1_1_1_1_1_1", Variable, 8, "Basic Addition with number and variable with underscores and numbers");
Check("3 + a_a_a_a_a_a_a", Variable, 8, "Basic Addition with number and variable with underscores and letters");
Check("3 + a1_a_1_1_a_a_a11", Variable, 8, "Basic Addition with number and variable with underscores and letters");
Check("(5 + a1)", Variable, 10, "Basic Addition with number and variable in parenthesis");
Check("((5 + a1))", Variable, 10, "Basic Addition with number and variable in double parenthesis");
Check("((2 + 2) + a1)", Variable, 9, "Basic Addition with number and variable in parenthesis and seperated");

Console.WriteLine("Passsed Tests: " + testsPassed);