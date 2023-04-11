//Author: Sam Henderson
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTests
    {
        /****************************************
        * Tests for EmptyGraphs
        * *****************************/

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Empty graph should contain nothing when checked for Dependees
        ///</summary>
        [TestMethod]
        public void EmptyTestHasDependees()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependees("a"));
        }

        /// <summary>
        ///Empty graph should contain nothing when checked for Dependents
        ///</summary>
        [TestMethod]
        public void EmptyTestHasDependents()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependents("a"));
        }

        /// <summary>
        ///Empty graph should contain nothing when iterated through
        ///</summary>
        [TestMethod]
        public void SimpleEmptyEnumeratorDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.GetDependees("a").GetEnumerator().MoveNext());
        }

        /// <summary>
        ///Empty graph should contain nothing when iterated through
        ///</summary>
        [TestMethod]
        public void SimpleEmptyEnumeratorDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.GetDependents("a").GetEnumerator().MoveNext());
        }

        /// <summary>
        /// Should be able to remove a dependency from a single connection graph and get an empty graph
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Replace on an empty DGs shouldn't fail
        ///</summary>
        [TestMethod]
        public void ReplaceOnEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependents("a", new HashSet<string>());
            Assert.AreEqual(0, t.Size);

            DependencyGraph u = new DependencyGraph();
            t.ReplaceDependees("a", new HashSet<string>());
            Assert.AreEqual(0, u.Size);
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }

        /****************************************
        * Tests for Adding
        * *****************************/

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void BlankDependencyTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency(" ", " ");

            IEnumerator<string> e = t.GetDependees(" ").GetEnumerator();


            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(" ", e.Current);
        }

        /// <summary>
        ///See Test name
        ///</summary>
        [TestMethod()]
        public void ReplaceDependentsWithEmptyHashSetTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "c");

            HashSet<string> emptyHash = new HashSet<string>();

            t.ReplaceDependents("a", emptyHash);

            IEnumerator<string> dentDict = t.GetDependents("a").GetEnumerator();

            Assert.IsFalse(dentDict.MoveNext());
        }

        /// <summary>
        ///See Test name
        ///</summary>
        [TestMethod()]
        public void ReplaceDependeesWithEmptyHashSetTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("b", "b");
            t.AddDependency("c", "b");
            t.AddDependency("d", "b");

            HashSet<string> emptyHash = new HashSet<string>();

            t.ReplaceDependees("b", emptyHash);

            IEnumerator<string> deeDict = t.GetDependees("b").GetEnumerator();

            Assert.IsFalse(deeDict.MoveNext());
        }

        /// <summary>
        ///See Test name
        ///</summary>
        [TestMethod()]
        public void ReplaceDependentsWithNewHashSetTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");

            HashSet<string> dentHashSet = new HashSet<string>();

            dentHashSet.Add("x");
            dentHashSet.Add("y");

            t.ReplaceDependees("a", dentHashSet);

            IEnumerator<string> deeDict = t.GetDependees("a").GetEnumerator();

            Assert.IsTrue(deeDict.MoveNext());
            Assert.AreEqual("x", deeDict.Current);

            Assert.IsTrue(deeDict.MoveNext());
            Assert.AreEqual("y", deeDict.Current);
        }

        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }

        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }

        /****************************************
        * Tests for Size
        * *****************************/

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }

        /// <summary>
        /// the prior size test with different parameters
        /// </summary>
        [TestMethod()]
        public void SizeTest2()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "c");
            t.AddDependency("c", "d");

            Assert.AreEqual(4, t.Size);
        }

        /// <summary>
        ///Non-empty graph contains something after removing
        ///</summary>
        [TestMethod()]
        public void SizeTestAfterRemoving()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("c", "b");
            t.RemoveDependency("a", "d");
            t.AddDependency("e", "b");
            t.AddDependency("b", "d");
            t.RemoveDependency("e", "b");
            t.RemoveDependency("x", "y");
            Assert.AreEqual(4, t.Size);
        }

        /// <summary>
        /// Indexer test
        /// </summary>
        [TestMethod()]
        public void IndexerTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "c");
            t.AddDependency("c", "d");

            int size = t["c"];

            Assert.AreEqual(2, size);
        }

        /// <summary>
        /// Testing the indexer with remove
        /// </summary>
        [TestMethod()]
        public void IndexerTestWithDependencyRemoval()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.RemoveDependency("a", "b");

            t.AddDependency("a", "c");
            t.RemoveDependency("a", "c");

            t.AddDependency("b", "c");
            t.RemoveDependency("b", "c");

            t.AddDependency("c", "d");
            t.AddDependency("e", "c");


            int size = t["c"];

            Assert.AreEqual(1, size);
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod]
        public void SimpleIndexerOnEmptyGraphTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t["a"]);
        }

        /// <summary>
        /// Tests the Indexer on a graph that had it's dependencies 
        /// </summary>
        [TestMethod()]
        public void IndexerOnEmptyGraphTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.RemoveDependency("a", "b");

            t.AddDependency("a", "c");
            t.RemoveDependency("a", "c");

            t.AddDependency("b", "c");
            t.RemoveDependency("b", "c");

            t.AddDependency("c", "d");
            t.AddDependency("e", "c");
            t.RemoveDependency("e", "c");

            int size = t["c"];

            Assert.AreEqual(0, size);
        }

        /// <summary>
        /// Testing with value that isn't an ordered pair
        /// </summary>
        [TestMethod()]
        public void IndexerTestSingleTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");

            int size = t["c"];

            Assert.AreEqual(0, size);
        }

        /// <summary>
        /// Tests add and GetDependents after a single dependency is added
        /// </summary>
        [TestMethod()]
        public void SimpleEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
        }

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }


        /// <summary>
        ///Adds duplicate dependencies 
        ///</summary>
        [TestMethod()]
        public void SimpleDuplicateTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        ///Test add with duplicate dependencies
        ///</summary>
        [TestMethod()]
        public void DuplicateTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "b");

            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(e.MoveNext());

            Assert.AreEqual("b", e.Current);

            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }
        
        /// <summary>
        /// Just do a bunch of things
        ///</summary>
        [TestMethod()]
        public void StressTest2()
        {
            const string MassiveString = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int SIZE = 1000;

            DependencyGraph t = new DependencyGraph();
            var random = new Random(9834095);

            for (int i = 0; i < SIZE; i++)
            {
                string[] giantArray = Enumerable.Repeat(new string(Enumerable.Repeat(MassiveString, SIZE)
                              .Select(pick => pick[random.Next(MassiveString.Length)])
                              .ToArray())
                , SIZE).Distinct().ToArray();

                var parent = random.Next('a', 'z').ToString();

                t.ReplaceDependents(parent, giantArray);
                for (int k = 0; k < giantArray.Length; k++)
                {
                    t.RemoveDependency(parent, giantArray[k]);
                    Assert.AreEqual(giantArray.Length - k - 1, t.GetDependents(parent).Count());
                }
            }
        }

        /// <summary>
        /// Same things should yield same result 
        ///</summary>
        [TestMethod()]
        public void StressTest3()
        {
            const int SIZE = 100;

            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            var random = new Random(4444);

            for (int i = 0; i < SIZE; i++)
            {
                var parent = (i + 'a').ToString();
                var child = random.Next().ToString();

                t.AddDependency("a", child);
                t.AddDependency("b", child);
                t.AddDependency("c", child);

                t.AddDependency(child, "a");
                t.AddDependency(child, "b");
                t.AddDependency(child, "c");


                Assert.IsTrue(IsSameArray(t.GetDependents("a").ToArray(), t.GetDependents("b").ToArray()));
                Assert.IsTrue(IsSameArray(t.GetDependents("b").ToArray(), t.GetDependents("c").ToArray()));
                Assert.IsTrue(IsSameArray(t.GetDependents("a").ToArray(), t.GetDependents("c").ToArray()));
            }
        }

        /// <summary>
        /// Different things should yield different result 
        ///</summary>
        [TestMethod()]
        public void StressTest4()
        {
            const int SIZE = 100;

            DependencyGraph t = new DependencyGraph();
            var random = new Random(4444);

            for (int i = 0; i < SIZE; i++)
            {
                var parent = (i + 'a').ToString();
                var child = random.Next().ToString();

                t.AddDependency("a", child);
                t.AddDependency("b", child);
                t.AddDependency("c", child);

                t.AddDependency(child, "a");
                t.AddDependency(child, "b");
                t.AddDependency(child, "c");

                t.AddDependency("a", child);
                t.AddDependency("b", child);
                t.AddDependency("c", child + "oink");

                t.AddDependency(child, "a");
                t.AddDependency(child, "b");
                t.AddDependency(child + "oink", "c");


                Assert.IsTrue(IsSameArray(t.GetDependents("a").ToArray(), t.GetDependents("b").ToArray()));
                Assert.IsFalse(IsSameArray(t.GetDependents("b").ToArray(), t.GetDependents("c").ToArray()));
                Assert.IsFalse(IsSameArray(t.GetDependents("a").ToArray(), t.GetDependents("c").ToArray()));
            }
        }

        /// <summary>
        /// helper method meant for StressTest3 and StressTest4
        ///</summary>
        private static bool IsSameArray(string[] a, string[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Constant growth stress test
        ///</summary>
        [TestMethod()]
        public void StressTest5()
        {
            const int SIZE = 100;

            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            for (int i = 0; i < SIZE; i++)
            {
                var parent = (i + 'a').ToString();
                var child = (i - (char)201).ToString();

                t.AddDependency(parent, child);
                t.AddDependency(child, parent);
                Assert.AreEqual((i + 1) * 2, t.Size);
            }
        }

        /// <summary>
        /// Just insert a bunch of stuff
        ///</summary>
        [TestMethod()]
        public void StressTest6()
        {
            const int SIZE = 2000000;

            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            var random = new Random();

            for (int i = 0; i < SIZE; i++)
            {
                var parent = random.Next('a', 'z').ToString();
                var child = random.Next('a', 'z').ToString();

                t.AddDependency(parent, child);
                t.RemoveDependency(parent, child);
            }

            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void StressTest7()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependents
            for (int i = 0; i < SIZE; i += 4)
            {
                HashSet<string> newDents = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 7)
                {
                    newDents.Add(letters[j]);
                }
                t.ReplaceDependents(letters[i], newDents);

                foreach (string s in dents[i])
                {
                    dees[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDents)
                {
                    dees[s[0] - 'a'].Add(letters[i]);
                }

                dents[i] = newDents;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        /// <summary>
        /// Big string tests for time
        ///</summary>
        [TestMethod()]
        public void StressTest8()
        {
            const string LONG_ASS_STRING = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int SIZE = 10000;

            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            var random = new Random(9834095);

            DateTime nowish = DateTime.Now;
            // Take a sample of how long it takes to normally compute //
            byte[] trash = new byte[SIZE];
            random.NextBytes(trash); // Fill random buffer of sorts //
            var timeSpan = DateTime.Now - nowish;

            if (timeSpan.Milliseconds > 2) // Find inconclusive if it took over 5 ms to fill buffer //
                Assert.Inconclusive("We cannot run this test, your computer is too slow.");


            for (int i = 0; i < SIZE; i++)
            {
                // Make a random string that's 1000 characters long. //
                var parent = new string(
                    Enumerable.Repeat(LONG_ASS_STRING, 1000)
                              .Select(pick => pick[random.Next(LONG_ASS_STRING.Length)])
                              .ToArray()
                );
                var child = random.Next('a', 'z').ToString();

                t.AddDependency(parent, child);
            }

            timeSpan = DateTime.Now - nowish;

            if (timeSpan.Milliseconds > SIZE * 2) // If it takes over 2 ms per item, we fail.  //
                Assert.Fail("Time exceeded");
        }

        /// <summary>
        /// Test for checking order
        ///</summary>
        [TestMethod()]
        public void StressTest9()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            t.ReplaceDependees("b", new string[] { "a" });
            t.ReplaceDependees("c", new string[] { "a" });
            t.ReplaceDependees("d", new string[] { "a" });

            Assert.AreEqual(3, t.GetDependents("a").Count());
            Assert.AreEqual("b", t.GetDependents("a").ElementAt(0));
            Assert.AreEqual("c", t.GetDependents("a").ElementAt(1));
            Assert.AreEqual("d", t.GetDependents("a").ElementAt(2));
        }

        /// <summary>
        /// Stress test that works with duplicates
        ///</summary>
        [TestMethod()]
        public void StressTest10()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "a");
            t.AddDependency("a", "a");

            t.AddDependency("a", "c");
            t.RemoveDependency("a", "c");

            t.ReplaceDependees("b", new string[] { "c" });

            Assert.AreEqual(1, t.GetDependents("a").Count());
            Assert.AreEqual("a", t.GetDependents("a").First());
        }

        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void StressTest11()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 1000;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependees
            for (int i = 0; i < SIZE; i += 3)
            {
                HashSet<string> newDees = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 4)
                {
                    newDees.Add(letters[j]);
                }
                t.ReplaceDependees(letters[i], newDees);

                foreach (string s in dees[i])
                {
                    dents[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDees)
                {
                    dents[s[0] - 'a'].Add(letters[i]);
                }

                dees[i] = newDees;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void StressTest12()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependees
            for (int i = 0; i < SIZE; i += 4)
            {
                HashSet<string> newDees = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 7)
                {
                    newDees.Add(letters[j]);
                }
                t.ReplaceDependees(letters[i], newDees);

                foreach (string s in dees[i])
                {
                    dents[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDees)
                {
                    dents[s[0] - 'a'].Add(letters[i]);
                }

                dees[i] = newDees;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }
    }
}
