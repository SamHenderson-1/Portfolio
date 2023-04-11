// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

//Author: Sam Henderson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private IEnumerable<string> FormulaTokens = Enumerable.Empty<string>();
        private List<string> ValidTokens = new List<string>();
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid) {
            FormulaTokens = GetTokens(formula);
            IsTokensEmpty(FormulaTokens);
            FirstTokenIsValid(FormulaTokens);
            LastTokenIsValid(FormulaTokens);


            int NumOfLeft = 0;
            int NumOfRight = 0;
            string PreviousToken = "";
            foreach (string s in FormulaTokens)
            {
                //Checking for valid token, including valid variable. will add token to ValidTokens List if true
                ValidateToken(s, normalize, isValid);

                //Checking the previous tokens and comparing to current tokens. Throwing exceptions if needed
                FollowTokensAreValid(s, PreviousToken);

                //checking the current token, counting the number of parenthesis, throwing exception if needed. 
                if (IsLPrn(s))
                    NumOfLeft++;

                if (IsRPrn(s))
                    NumOfRight++;

                //If at any point the number of right is greater than the number of left, throw exception
                if (NumOfRight > NumOfLeft)
                    throw new FormulaFormatException("Incorrect usage of parenthesis");

                //setting for iterative checks. 
                PreviousToken = s;
            }

            //At the end of loop, if the number of parens is not equal, throw exception. 
            if (!(NumOfLeft == NumOfRight))
                throw new FormulaFormatException("Incorrect usage of parenthesis");
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> vals = new Stack<double>();
            Stack<String> opers = new Stack<String>();
            //Using a try catch to catch a divide by 0 error, or an argument exception error from the lookup
            //If these exceptions are caught, it will return a formula error. 
            //If not, it will return a double. 
            try
            {
                foreach (string s in ValidTokens)
                {
                    if (IsLPrn(s))
                    {
                        opers.Push(s);
                    }
                    else if (IsRPrn(s))
                    {
                        if (IsPlusOrMinusOnStack(opers))
                        {
                            double computedValue = PerformPlusMinusComputation(opers, vals);
                            vals.Push(computedValue);
                        }
                        opers.Pop();
                        if (IsMultiplyOrDivideOnStack(opers))
                        {
                            double computedValue = PerformMultiplyDivideComputation(opers, vals, 0, false);
                            vals.Push(computedValue);
                        }
                    }
                    else if (IsMultiplyOrDivide(s))
                    {
                        opers.Push(s);
                    }
                    else if (IsPlusOrMinus(s))
                    {
                        if (IsPlusOrMinusOnStack(opers))
                        {
                            double computedValue = PerformPlusMinusComputation(opers, vals);
                            vals.Push(computedValue);
                        }
                        opers.Push(s);
                    }
                    else if (IsNum(s))
                    {
                        double value = double.Parse(s);
                        HandleIntOrVariable(opers, vals, value);
                    }
                    else if (IsVar(s))
                    {
                        double value = lookup(s);
                        HandleIntOrVariable(opers, vals, value);
                    }
                }
                //End of for loop

                if (opers.IsEmpty())
                    return vals.Pop();
                else
                    return PerformPlusMinusComputation(opers, vals);
            }
            catch (ArgumentException)
            {
                FormulaError error = new FormulaError("Invalid variable");
                return error;
            }
            catch (DivideByZeroException)
            {
                FormulaError error = new FormulaError("Division by 0");
                return error;
            }

        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<string> DistinctTokens = new HashSet<string>(ValidTokens);

            foreach (string s in DistinctTokens)
            {
                if (IsVar(s))
                    yield return s;
            }
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string s in ValidTokens)
            {
                if (IsNum(s))
                {
                    string DoubleString = double.Parse(s).ToString();
                    builder.Append(DoubleString);
                }
                else
                    builder.Append(s);
            }
            return builder.ToString();
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (obj.GetType() == typeof(Formula))
            {
                string ObjectString = ToString();
                string OtherString = obj.ToString();

                if (ObjectString.Equals(OtherString))
                    return true;
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            //if both values are not null && they equal each other, or if both values are null, return true. 
            if ((!ReferenceEquals(f1, null) && !ReferenceEquals(f2, null) && f1.Equals(f2)) || (ReferenceEquals(f1, null) && ReferenceEquals(f2, null)))
                return true;
            else
                return false;
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            string s = ToString();
            return s.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        /******************************************
         * Helper Methods for Expression Constructor
         * *****************************************/
        
        /// <summary>
        /// Checks if an expression is valid or not. 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        private void FollowTokensAreValid(string current, string previous)
        {
            if (IsNum(previous) || IsVar(previous) || IsRPrn(previous))
            {
                if (!(IsOp(current) || IsRPrn(current)))
                    throw new FormulaFormatException("The expression is invalid. Consider checking correct use of parenthesis and operators.");
            }
            else if (IsLPrn(previous) || IsOp(previous))
            {
                if (!(IsNum(current) || IsVar(current) || IsLPrn(current)))
                    throw new FormulaFormatException("The expression is invalid. Consider checking correct use of parenthesis and operators.");
            }
        }


        /// <summary>
        /// Checks if a token is valid. Throw's an exception if it isn't.
        /// Will add it to the ValidTokens List if it is.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="normalize"></param>
        /// <param name="isValid"></param>
        private void ValidateToken(string token, Func<string, string> normalize, Func<string, bool> isValid)
        {
            //is valid token
            if (!(IsNum(token) || IsOp(token) || IsRPrn(token) || IsLPrn(token) || IsVar(token)))
                throw new FormulaFormatException("Expression contains invalid token");

            //is variable
            if (IsVar(token))
            {
                string normal = normalize(token);
                if (isValid(normal))
                    ValidTokens.Add(normal);
                else
                    throw new FormulaFormatException("The expression is invalid. Contains illegal variable");
            }
            else
            {
                ValidTokens.Add(token);
            }
        }

        /// <summary>
        /// Checks for empty enumerable
        /// </summary>
        /// <param name="tokens"></param>
        private void IsTokensEmpty(IEnumerable<string> tokens)
        {
            if (tokens.Count() == 0)
            {
                throw new FormulaFormatException("Need at least one token in expression");
            }
        }

        /// <summary>
        /// Checks if the first token is valid
        /// </summary>
        /// <param name="tokens"></param>
        private void FirstTokenIsValid(IEnumerable<string> tokens)
        {
            string s = tokens.First();
            if (!IsNum(s) && !IsLPrn(s) && !IsVar(s))
            {
                throw new FormulaFormatException("The first token in the expression is not valid");
            }
        }


        /// <summary>
        /// Checks if the last token is valid
        /// </summary>
        /// <param name="tokens"></param>
        private void LastTokenIsValid(IEnumerable<string> tokens)
        {
            string s = tokens.Last();
            if (!IsNum(s) && !IsRPrn(s) && !IsVar(s))
            {
                throw new FormulaFormatException("The last token in the expression is not valid");
            }
        }


        /// <summary>
        /// Checking if the token is a number.
        /// It will correclty compute scientific notation
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsNum(string token)
        {
            double i = 0;
            if (double.TryParse(token, out i))
                return true;
            else
                return false;
        }


        /// <summary>
        /// checking if an expression is a valid operator
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsOp(string token)
        {
            switch (token)
            {
                case "+":
                    return true;
                case "-":
                    return true;
                case "/":
                    return true;
                case "*":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if the current token is a "(".
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool IsLPrn(string token)
        {
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
        private static bool IsRPrn(string token)
        {
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
        private static bool IsVar(string token)
        {
            String letters = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            if (!Regex.IsMatch(token, letters))
            {
                return false;
            }

            else
                return true;
        }

        /****************************************
         * Helper Methods for Evaluator
         * *****************************/

        /// <summary>
        /// Checks if the operator passed in, not the stack, is plus or minus.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsPlusOrMinus(string s)
        {
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
        private static bool IsMultiplyOrDivide(string s)
        {
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
        private static bool IsPlusOrMinusOnStack(Stack<string> operStack)
        {
            return (operStack.IsOnTop("+") || operStack.IsOnTop("-"));
        }

        /// <summary>
        /// Checks the top of the stack to see if the token is either a '*' or '/'. 
        /// </summary>
        /// <param name="operStack">The operator stack</param>
        /// <returns>true or false</returns>
        private static bool IsMultiplyOrDivideOnStack(Stack<string> operStack)
        {
            return (operStack.IsOnTop("*") || operStack.IsOnTop("/"));
        }

        /// <summary>
        /// Solves simple expressions passed in by a first and second value and a valid operator. 
        /// </summary>
        /// <param name="val1">left hand value</param>
        /// <param name="val2">right hand value</param>
        /// <param name="op">The operation to perform</param>
        /// <returns>The value of the computation after it is finished. If an incorrect operator was passed in, 0 will be returned</returns>
        private static double Solve(double val1, double val2, string op)
        {
            switch (op)
            {
                case "+":
                    return val1 + val2;
                case "-":
                    return val1 - val2;
                case "*":
                    return val1 * val2;
                case "/":
                    if (val2 == 0)
                    {
                        throw new DivideByZeroException("Division by 0.");
                    }
                    return val1 / val2;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Solves a plus or minus expression by popping values 
        /// from a stack of operators and a stack of values
        /// </summary>
        /// <param name="opers">The operator </param>
        /// <param name="vals">The value stack</param>
        /// <returns>The computed value</returns>
        private static double PerformPlusMinusComputation(Stack<string> opers, Stack<double> vals)
        {

            // pop the top two values from the value stack
            double firstValue = vals.Pop();
            double secondValue = vals.Pop();

            //pop the operator from the operator stack
            string op = opers.Pop();

            //compute value
            double computedValue = Solve(secondValue, firstValue, op);

            return computedValue;
        }

        /// <summary>
        /// Solves a multiplication or division expression by 
        /// popping values from a stack of operators and a 
        /// stack of values. Offers option to perform the 
        /// expression with an input value rather than the 
        /// value stack. This is determined by the bool isTokenInt.
        /// </summary>
        /// <param name="opers">Operator Stack</param>
        /// <param name="vals">Value Stack</param>
        /// <param name="value">Value to use for computation, only if you don't want to use two variables from value stack</param>
        /// <param name="isTokenInt">If True, it will assume the value passed will be used for computation. If false, it will calculate with two values from value stack</param>
        /// <returns></returns>
        private static double PerformMultiplyDivideComputation(Stack<string> opers, Stack<double> vals, double value, bool isTokenInt)
        {
            // if the token is an integer, perform the multiply/divide computation by taking in a variable, and popping from stack
            if (isTokenInt)
            {
                double stackValue = vals.Pop();
                string op = opers.Pop();
                double computedValue = Solve(stackValue, value, op);
                return computedValue;
            }
            // if the token is not an integer, and a ')', perform the multiply/divide computation by popping the two values from the vals, and performing computation
            else
            {
                double firstValue = vals.Pop();
                double secondValue = vals.Pop();
                string op = opers.Pop();
                double computedValue = Solve(secondValue, firstValue, op);
                return computedValue;
            }
        }

        /// <summary>
        /// Used to pass in ints and variables using a given value
        /// </summary>
        /// <param name="opers">Operator Stack</param>
        /// <param name="vals">value Stack</param>
        /// <param name="value">The value used for the computation. should come from integer token or variable token</param>
        private static void HandleIntOrVariable(Stack<string> opers, Stack<double> vals, double value)
        {
            if (IsMultiplyOrDivideOnStack(opers))
            {
                double computedValue = PerformMultiplyDivideComputation(opers, vals, value, true);
                vals.Push(computedValue);
            }
            else
                vals.Push(value);
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}

// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>

static class Extensions
{
    /// <summary>
    /// Extension for the stack class to check if a certain value is at the top of the stack. 
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="stack">The stack to check</param>
    /// <param name="op">The value to check</param>
    /// <returns>True or false, depending on the value of the stack peek and the value passed in. </returns>
    public static bool IsOnTop<t>(this Stack<t> stack, t op)
    {
        if (stack.Count == 0)
            return false;
        return stack.Peek().Equals(op);
    }
    public static bool IsEmpty<t>(this Stack<t> stack)
    {
        if (stack.Count == 0)
            return true;
        else
            return false;

    }
}
