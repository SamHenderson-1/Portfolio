// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)
//Author: Sam Henderson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities {
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph {
        private readonly Dictionary<string, HashSet<string>> dependents;
        private readonly Dictionary<string, HashSet<string>> dependees;
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph() {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
        }
        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size {
            get; private set;
        }

        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s] {
            get {
                if (HasDependees(s))
                    return dependees[s].Count();
                else
                    return 0;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s) {
            if (dependents.ContainsKey(s))
                return !(dependents[s].Count == 0);
            else
                return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s) {
            if (dependees.ContainsKey(s))
                return !(dependees[s].Count == 0);
            else
                return false;
        }

        /// <summary>
        /// Helper method to see if a specific dependent already exsits. 
        /// </summary>
        /// <param name="s">The dependee's key</param>
        /// <param name="t">The value of the dependee's dependent</param>
        /// <returns>True or false</returns>
        private bool HasSpecificDependent(string s, string t) {
            return HasDependents(s) && dependents[s].Contains(t);
        }

        /// <summary>
        /// Helper method to see if a specific dependee already exsits. 
        /// </summary>
        /// <param name="s">The dependee's key</param>
        /// <param name="t">The value of the dependee's dependent</param>
        /// <returns>True or false</returns>
        private bool HasSpecificDependee(string s, string t) {
            return HasDependees(t) && dependees[t].Contains(s);
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s) {
            if (HasDependents(s)) {
                HashSet<string> dentCopy = new HashSet<string>(dependents[s]);
                return dentCopy;
            }
            else
                return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s) {
            if (HasDependees(s)) {
                HashSet<string> deesCopy = new HashSet<string>(dependees[s]);
                return deesCopy;
            }
            else
                return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Helper method to add a dependent. Used for AddDependency()
        /// </summary>
        /// <param name="s">Add 's' first</param>
        /// <param name="t">Add 't' after 's'</param>
        private void AddDependent(string s, string t) {
            if (HasDependents(s)) {
                dependents[s].Add(t);
            }
            else if (!HasDependents(s)) {
                HashSet<string> dentsHash = new HashSet<string>();
                dentsHash.Add(t);
                dependents.Add(s, dentsHash);
            }
        }

        /// <summary>
        /// Helper method to add dependee.Used for AddDependency()
        /// </summary>
        /// <param name="s">Add 's' first</param>
        /// <param name="t">Add 't' after 's' </param>
        private void AddDependee(string s, string t) {
            if (HasDependees(t)) {
                dependees[t].Add(s);
            }
            else if (!HasDependees(t)) {
                HashSet<string> deesHash = new HashSet<string>();
                deesHash.Add(s);
                dependees.Add(t, deesHash);
            }
        }
        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t) {
            //if the dependency does not already exist
            if (!(HasSpecificDependent(s, t) && HasSpecificDependee(s, t))) {
                AddDependee(s, t);
                AddDependent(s, t);
                Size++;
            }
        }

        /// <summary>
        /// Helper method to remove a specific dependee.Used for RemoveDependency()
        /// </summary>
        /// <param name="s">Add 's' first</param>
        /// <param name="t">Add 't' after 's' </param>
        private void RemoveActualDependency(string s, string t) {
            dependents[s].Remove(t);
            dependees[t].Remove(s);
        }

        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t) {
            // if the input exists in the form (s, t)
            if (HasSpecificDependent(s, t) && HasSpecificDependee(s, t)) {
                //remove dependency of form (s,t)
                RemoveActualDependency(s, t);

                //remove hashset from dictionary if empty
                if (dependents[s].Count == 0)
                    dependents.Remove(s);
                if (dependees[t].Count == 0)
                    dependees.Remove(t);
                Size -= 1;
            }
            else {
             
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents) {
            // removing dependents if there are any
            if (HasDependents(s)) {
                foreach (string t in dependents[s].ToList()) {
                    RemoveDependency(s, t);
                }
            }
            //adding newdependents from list
            foreach (string t in newDependents.ToList()) {
                AddDependency(s, t);
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees) {
            //removing dependees if there are any 
            if (HasDependees(s)) {
                foreach (string t in dependees[s].ToList()) {
                    RemoveDependency(t, s);
                }
            }
            //adding new dependencies from list
            foreach (string t in newDependees.ToList()) {
                AddDependency(t, s);
            }
        }
    }

}
