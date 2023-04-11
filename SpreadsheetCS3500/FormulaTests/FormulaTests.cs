/* Sam Henderson
 * CS 3500 - Software practice
 * Tests for the A3 Formula
 * February 7th, 2022
 */



using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
namespace FormulaTests
{
    /********************************************************************************
     * A considerable amount of the tests here were ported over from A1 grading tests
     ********************************************************************************/
    [TestClass]
    public class ValidValues
    {
        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestSingleNumber()
        {
            Formula t = new Formula("5");
            double value = 5;
            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestSingleVariable()
        {
            Formula t = new Formula("X4");
            double value = 13;

            Assert.AreEqual(value, t.Evaluate(s => 13));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestAddition()
        {
            Formula t = new Formula("5+3");
            double value = 8;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestSubtraction()
        {
            Formula t = new Formula("18-10");
            double value = 8;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestMultiplication()
        {
            Formula t = new Formula("2*4");
            double value = 8;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestDivision()
        {
            Formula t = new Formula("16/2");
            double value = 8;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary> Tests adding a number with a variable </summary>
        [TestMethod()]
        public void TestArithmeticWithVariable()
        {
            Formula t = new Formula("2+X1");
            double value = 6;

            Assert.AreEqual(value, t.Evaluate(s => 4));
        }

        /// <summary>
        /// Tests a formula of more than 2 numbers and 1 operator
        /// expected to travese left to right
        /// </summary>
        [TestMethod()]
        public void TestLeftToRight()
        {
            Formula t = new Formula("2*6+3");
            double value = 15;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary>
        /// Same as TestLeftToRight() but tests PEMDAS
        /// </summary>
        [TestMethod()]
        public void TestOrderOperations()
        {
            Formula t = new Formula("2+6*3");
            double value = 20;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary>
        /// Same as TestOrderOperations() but tests parentheses
        /// </summary>
        [TestMethod()]
        public void TestParenthesesTimes()
        {
            Formula t = new Formula("(2+6)*3");
            double value = 24;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary>
        /// Reverse of TestParenthesesTimes()
        /// </summary>
        [TestMethod()]
        public void TestTimesParentheses()
        {
            Formula t = new Formula("2*(3+5)");
            double value = 16;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary>
        /// Same as TestTimesParentheses() but tests with addition
        /// </summary>
        [TestMethod()]
        public void TestPlusParentheses()
        {
            Formula t = new Formula("2+(3+5)");
            double value = 10;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestPlusComplex()
        {
            Formula t = new Formula("2+(3+5*9)");
            double value = 50;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestComplexTimesParentheses()
        {
            Formula t = new Formula("2+3*(3+5)");
            double value = 26;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestComplexAndParentheses()
        {
            Formula t = new Formula("2+3*5+(3+4*8)*5+2");
            double value = 194;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestComplexNestedParensRight()
        {
            Formula t = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            double value = 6;

            Assert.AreEqual(value, t.Evaluate(s => 1));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestComplexNestedParensLeft()
        {
            Formula t = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            double value = 12;

            Assert.AreEqual(value, t.Evaluate(s => 2));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestRepeatedVar()
        {
            Formula t = new Formula("a4-a4*a4/a4");
            double value = 0;

            Assert.AreEqual(value, t.Evaluate(s => 3));
        }

    }

    [TestClass()]
    public class InvalidValues
    {
        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestUnknownVariable()
        {
            Formula t = new Formula("2+X1");

            object error = t.Evaluate(s => { throw new ArgumentException("Unknown variable"); });
            Assert.IsInstanceOfType(error, typeof(FormulaError));
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestDivideByZero()
        {
            Formula t = new Formula("3/0");

            object error = t.Evaluate(s => 0);
            Assert.IsInstanceOfType(error, typeof(FormulaError));
        }

        /// <summary>
        /// Testing with computed value in the division
        /// </summary>
        [TestMethod()]
        public void TestDivideByZero1()
        {
            Formula t = new Formula("5/(5-5)");

            object error = t.Evaluate(s => 0);
            Assert.IsInstanceOfType(error, typeof(FormulaError));
        }
        /// <summary>
        /// Testing if it correctly skips over a duplicate
        /// </summary>
        [TestMethod]
        public void TestGetVariables()
        {
            Formula t = new Formula("a1 + a2 + a2 + a4");

            IEnumerator<string> variables = t.GetVariables().GetEnumerator();

            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual(variables.Current, "a1");

            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual(variables.Current, "a2");

            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual(variables.Current, "a4");

            Assert.IsFalse(variables.MoveNext());
        }


        [TestMethod()]
        public void TestGetvariables1()
        {
            Formula t = new Formula("x + X + z", s => s.ToUpper(), s => true);

            IEnumerator<string> variables = t.GetVariables().GetEnumerator();

            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual(variables.Current, "X");

            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual(variables.Current, "Z");

            Assert.IsFalse(variables.MoveNext());
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestToString()
        {
            Formula t = new Formula("x+y");

            string s = t.ToString();

            Assert.AreEqual("x+y", s);
        }

        /// <summary> 
        /// Same as TestToString() but with uppercase
        /// </summary>
        [TestMethod()]
        public void TestToString1()
        {
            Formula t = new Formula("x+y", a => a.ToUpper(), a => true);

            string s = t.ToString();

            Assert.AreEqual("X+Y", s);
        }

        /// <summary> See Title </summary>
        [TestMethod()]
        public void TestEquals()
        {
            Formula t = new Formula("x+y");
            Formula r = new Formula("x+y");

            Assert.IsTrue(t.Equals(r));

        }

        /// <summary> 
        /// TestEquals() but with uppercase
        /// </summary>
        [TestMethod()]
        public void TestEquals1()
        {
            Formula t = new Formula("X+Y");
            Formula r = new Formula("x+y", s => s.ToUpper(), s => true);

            Assert.IsTrue(t.Equals(r));

        }

        /// <summary> 
        /// Tests unequal formulas
        /// </summary>
        [TestMethod()]
        public void TestEquals2()
        {
            Formula t = new Formula("x+y");
            Formula r = new Formula("X+y");

            Assert.IsFalse(t.Equals(r));
        }

        /// <summary> 
        /// Tests different strings but equal formulas 
        /// </summary>
        [TestMethod()]
        public void TestEquals3()
        {
            Formula t = new Formula("2.0 + x7");
            Formula r = new Formula("2.0000 + x7");

            Assert.IsTrue(t.Equals(r));
        }

        /// <summary> 
        /// Compares a formula to an object
        /// </summary>
        [TestMethod()]
        public void TestEquals4()
        {
            Formula t = new Formula("2.0 + x7");
            object r = new object();

            Assert.IsFalse(t.Equals(r));
        }

        /// <summary> 
        /// Compares a formula with a number to one with the same number in scientific notation
        /// </summary>
        [TestMethod()]
        public void TestEquals5()
        {
            Formula t = new Formula("10e1 + x7");
            Formula r = new Formula("100 + x7");
            Assert.IsTrue(t.Equals(r));
        }

        /// <summary> 
        /// Tests == and != operators 
        /// </summary>
        [TestMethod()]
        public void TestOperatorEquals()
        {
            Formula t = new Formula("x+y");
            Formula r = new Formula("x+y");

            Assert.IsTrue(t == r);

            Assert.IsFalse(t != r);
        }

        /// <summary> 
        /// Tests == with nulls 
        /// </summary>
        [TestMethod()]
        public void TestOperatorEquals1()
        {
            Formula t = null;
            Formula r = null;

            Assert.IsTrue(t == r);

            Assert.IsTrue(t == null);

            Assert.IsFalse(t != r);
        }

        /// <summary> 
        /// Tests == with a null and a formula
        /// </summary>
        [TestMethod()]
        public void TestOperatorEquals2()
        {
            Formula t = null;
            Formula r = new Formula("x+y");

            Assert.IsFalse(t == r);
        }

        /// <summary> 
        /// Tests == and != operators 
        /// </summary>
        [TestMethod()]
        public void TestOperatorNotEquals()
        {
            Formula t = new Formula("x+Y");
            Formula r = new Formula("x+y");

            Assert.IsTrue(t != r);

            Assert.IsFalse(t == r);

        }

        /// <summary> 
        /// Tests equal HashCodes
        /// </summary>
        [TestMethod()]
        public void HashCode1()
        {
            Formula t = new Formula("2+2");
            Formula r = new Formula("2+2");

            int hash1 = t.GetHashCode();
            int hash2 = r.GetHashCode();

            Assert.AreEqual(hash1, hash2);
        }

        /// <summary> 
        /// Tests unequal HashCodes
        /// </summary>
        [TestMethod()]
        public void HashCode2()
        {
            Formula t = new Formula("2+2");
            Formula r = new Formula("2+3");

            int hash1 = t.GetHashCode();
            int hash2 = r.GetHashCode();

            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
