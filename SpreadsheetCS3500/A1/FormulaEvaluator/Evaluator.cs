using System.Collections;
using System.Text.RegularExpressions;

namespace FormulaEvaluator {

    /// <summary>
    ///     A public class for evaluating mathematical formulas. Can operate with addition, 
    ///     subtraction, multiplication and division using infix notation. 
    /// </summary>
    public class Evaluator {

        /// <summary>
        ///     Delegate Lookup function meant to contain a variable.
        /// </summary>
        /// <param name="v">variable name</param>
        /// <returns>An int value associated with v</returns>
        public delegate int Lookup(String v);

        /// <summary>
        ///     Computes an expression utilizing the delegate variableEvaluator from Lookup.
        ///     Passes in a String and utilizes delegate Lookup to keep track of variables.
        /// </summary>
        /// <param name="exp">The arithmetic expression meant to be solved</param>
        /// <param name="variableEvaluator">A Lookup() passed in as a parameter</param>
        /// <returns>Returns an int result</returns>
        public static int Evaluate(string exp, Lookup variableEvaluator) {
            //Splits up exp into substring tokens and put them inside of substrings[]
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //ensure whitespace is not present
            for(int i = 0; i < substrings.Length; i++) {
                String trimmed = substrings[i].Trim();
                substrings[i] = trimmed;
            }

            //Creates the value and operator stacks, called vals and opers respectively, as
            //well as creates an int result meant to keep track of the current total. result
            //serves as our return value.
            Stack<int> vals = new Stack<int>();
            Stack<String> opers = new Stack<String>();
            int result;

            //loops through substrings until it has emptied it's tokens into the stacks, or
            //until an exception is called.
            foreach (string t in substrings)
            {
                if(t.Length == 0)
                    continue;
                //checks for "("
                if (IsLPrn(t))
                    opers.Push(t);

                //checks for ")"
                else if (IsRPrn(t))
                {
                    if (IsPlusOrMinusStack(opers))
                    {
                        result = PerformPlusMinusComputation(opers, vals);
                        vals.Push(result);
                    }
                    if (opers.Count() == 0 || opers.Pop() != "(")
                        throw new ArgumentException("Uneven parentheses");
                    if (IsMultiplyOrDivideStack(opers))
                    {
                        result = PerformMultiplyDivideComputation(opers, vals, 0, false);
                        vals.Push(result);
                    }
                }

                //checks for * or /
                else if (IsMultiplyOrDivide(t))
                    opers.Push(t);

                //checks for + or -
                else if (IsPlusOrMinus(t))
                {
                    if (IsPlusOrMinusStack(opers))
                    {
                        result = PerformPlusMinusComputation(opers, vals);
                        vals.Push(result);
                    }
                    opers.Push(t);
                }

                //checks for int 
                else if (isInt(t))
                {
                    int x = int.Parse(t);
                    HandleIntOrVariable(opers, vals, x);
                }

                //must be a variable
                else if (IsVar(t))
                {
                    string pattern = "[a-zA-Z]+[0-9]+";
                    if (Regex.IsMatch(t, pattern)) { 
                    int x = variableEvaluator(t);
                    HandleIntOrVariable(opers, vals, x);
                    }
                else
                    throw new ArgumentException("Invalid Variable");
                }
            }//end of for loop


            if (opers.Count() == 0)
            {
                if (vals.Count != 1)
                    throw new ArgumentException("There isn't exactly one value on the value stack");
                return vals.Pop();
            }

            else
            {
                if (opers.Count != 1 || vals.Count != 2)
                    throw new ArgumentException("There isn't exactly one operator on the operator stack or exactly two numbers on the value stack.");
                return PerformPlusMinusComputation(opers, vals);
            }
           
        }

        /****************************************
        * Helper Methods for Evaluator
        * *****************************/


        /// <summary>
        /// Checks if the current token is an integer.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool isInt(string token) {
            int i = 0;
            if (int.TryParse(token, out i))
                return true;
            
            else
                return false;
        }

        /// <summary>
        /// Checks if the current token is a "(".
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool IsLPrn(string token) {
            if (token == "(")
                return true;
            
            else
                return false;
        }

        /// <summary>
        /// Checks if the current token is a ")".
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool IsRPrn(string token) {
            if (token == ")")
                return true;
            
            else
                return false;
        }

        /// <summary>
        /// Checks if the current token is a variable using regex. 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool IsVar(string token) {
            String letters = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            if (!Regex.IsMatch(token, letters)) {
                return false;
            }

            else
                return true;
        }

        /// <summary>
        /// Checks if the operator passed in, not the stack, is plus or minus.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsPlusOrMinus(string s) {
            if (s == "+" || s == "-")
                return true;

            else
                return false;
        }


        /// <summary>
        /// Checks if operator passed in, not the stack, is multiply or divide.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsMultiplyOrDivide(string s) {
            if (s == "*" || s == "/")
                return true;

            else
                return false;
        }

        /// <summary>
        /// Checks the top of the stack to see if the token is either a '+' or '-'.
        /// </summary>
        /// <param name="operStack">The operator stack</param>
        /// <returns>true or false</returns>
        private static bool IsPlusOrMinusStack(Stack<string> operStack) {
            if (operStack.Count != 0)
                return (operStack.Peek() == ("+") || operStack.Peek() == ("-"));
            else
                return false;
        }


        /// <summary>
        /// Checks the top of the stack to see if the token is either a '*' or '/'. 
        /// </summary>
        /// <param name="operStack">The operator stack</param>
        /// <returns>true or false</returns>
        private static bool IsMultiplyOrDivideStack(Stack<string> operStack) {
            if (operStack.Count != 0)
                return (operStack.Peek() == ("*") || operStack.Peek() == ("/"));
            else
                return false;
        }

        /// <summary>
        /// Makes simple computations based on the values passed in and the operator to use. 
        /// Similar to IsPlusOrMinus
        /// </summary>
        /// <param name="val1">One of the values for computation</param>
        /// <param name="val2">The other value for computation</param>
        /// <param name="op">The operation to perform</param>
        /// <returns>The value of the computation after it is finished. If an incorrect operator was passed in, 0 will be returned</returns>
        private static int Solve(int val1, int val2, String op)
        { 
            //compute value
            switch (op) {
                case "+":
                    return val1 + val2;
                case "-":
                    return val1 - val2;
                case "*":
                    return val1 * val2;
                case "/":
                    if (val2 == 0)
                    {
                        throw new ArgumentException("Division by 0.");
                    }
                    return val1 / val2;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Uses the Compute Value function to perform a plus or minus computation. 
        /// It does so by popping two values from the vals given,
        /// and using the operation from the operator stack. 
        /// Similar to PerformMultiplyDivideComputation
        /// </summary>
        /// <param name="opers">The operator </param>
        /// <param name="vals">The value stack</param>
        /// <returns>The computed value</returns>
        private static int PerformPlusMinusComputation(Stack<string> opers, Stack<int> vals)
        {

            // pop the top two values from the value stack
            int val1 = vals.Pop();
            int val2 = vals.Pop();

            //pop the operator from the operator stack
            string op = opers.Pop();

            //compute value
            int result = Solve(val2, val1, op);

            return result;
        }

        /// <summary>
        /// Uses the Compute Value function to perform a plus or minus computation. 
        /// It does so by popping two values from the vals given,
        /// or by popping only one value from the vals, and using a value passed in.
        /// Similar to PerformMultiplyDivideComputation
        /// </summary>
        /// <param name="opers">Operator Stack</param>
        /// <param name="vals">Value Stack</param>
        /// <param name="value">Value to use for computation, only if you don't want to use two variables from value stack</param>
        /// <param name="isTokenInt">If True, it will assume the value passed will be used for computation. If false, it will calculate with two values from value stack</param>
        /// <returns></returns>
        private static int PerformMultiplyDivideComputation(Stack<string> opers, Stack<int> vals, int value, bool isTokenInt)
        {
            // if the token is an integer, perform the multiply/divide computation by taking in a variable, and popping from stack
            if (isTokenInt)
            {
                int stackValue = vals.Pop();
                string op = opers.Pop();
                int result = Solve(stackValue, value, op);
                return result;
            }
            // if the token is not an integer, and a ')', perform the multiply/divide computation by popping the two values from the vals, and performing computation
            else
            {
                int val1 = vals.Pop();
                int val2 = vals.Pop();
                string op = opers.Pop();
                int result = Solve(val2, val1, op);
                return result;
            }
        }

        /// <summary>
        /// Performs the same computation for both integers and variables, when given the value. 
        /// </summary>
        /// <param name="opers">Operator Stack</param>
        /// <param name="vals">value Stack</param>
        /// <param name="value">The value used for the computation. should come from integer token or variable token</param>
        private static void HandleIntOrVariable(Stack<string> opers, Stack<int> vals, int value)
        {
            if (IsMultiplyOrDivideStack(opers))
            {
                int result = PerformMultiplyDivideComputation(opers, vals, value, true);
                vals.Push(result);
            }
            else
                vals.Push(value);
        }
    }
}