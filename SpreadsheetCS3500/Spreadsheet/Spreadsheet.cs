//Author: Sam Henderson
//Date: 2/21/2022

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
//AS5
namespace SS
{
    /// <summary>
    /// This class represents a Spreadsheet object that extends the abstract 
    /// spreadsheet class
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        Dictionary<string, Cell> cells;

        DependencyGraph graph;

        private bool changed;

        /// <summary>
        /// Zero-Argument Constructor
        /// No additional validity condition 
        /// normalizes everything to itself
        /// version is set to "default"
        /// </summary>        
        public Spreadsheet() : base(s => true, s => s, "default")
        {
            cells = new Dictionary<string, Cell>();
            graph = new DependencyGraph();

            Changed = false;
        }

        /// <summary>
        /// Constructor that checks for validity, normalizes varaibles, and keeps track of the version
        /// </summary>
        /// <param name="isValid">validity delegate</param>
        /// <param name="normalize">Normalization method</param>
        /// <param name="version">version tag</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            graph = new DependencyGraph();

            Changed = false;
        }
        
        /// <summary>
        /// Constructor that reads a spreadsheet from an input filepath and constructs a new spreadsheet from it. 
        /// Uses the conditions of the previous constructor
        /// </summary>
        /// <param name="filepath">the input txt file path</param>
        /// <param name="isValid">validity delegate</param>
        /// <param name="normalize">Normalization method</param>
        /// <param name="version">version tag</param>
        public Spreadsheet(string filepath, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            graph = new DependencyGraph();

            if (!(GetSavedVersion(filepath).Equals(version)))
                throw new SpreadsheetReadWriteException("The version of the file does not match the version parameter");

            ReadFile(filepath, false); 

            Changed = false;
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                return changed;
            }
            protected set
            {
                changed = value;
            }

        }

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">the input txt file to be read</param>

        public override string GetSavedVersion(string filename)
        {
            return ReadFile(filename, true);
        }

        ///<summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">the input txt file to be saved to</param>
        public override void Save(string filename)
        {
            if (ReferenceEquals(filename, null))
                throw new SpreadsheetReadWriteException("The filename cannot be null");

            if (filename.Equals(""))
                throw new SpreadsheetReadWriteException("The filename cannot be empty");

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument(); //doc start
                    writer.WriteStartElement("spreadsheet");              
                    writer.WriteAttributeString("version", null, Version);
                    foreach (string cell in cells.Keys)
                    {
                        writer.WriteStartElement("cell");   //cell category                        
                        writer.WriteElementString("name", cell);   //cell input                                            

                        string cell_contents; 
                        if (cells[cell].contents is double)
                        {                          
                            cell_contents = cells[cell].contents.ToString();
                        }
                        else if (cells[cell].contents is Formula)
                        {   
                            cell_contents = "=" + cells[cell].contents.ToString();
                        }
                        else
                        {  
                            cell_contents = (string)cells[cell].contents;
                        }

                        writer.WriteElementString("contents", cell_contents);   //contents category                      
                        writer.WriteEndElement(); // close cell 
                    }
                    writer.WriteEndElement();  // close spreadsheet
                    writer.WriteEndDocument(); // end doc

                } 
            }
            catch (XmlException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            catch (IOException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }

            
            Changed = false;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        /// <param name="name">the key for the cell that is being searched for</param>

        public override object GetCellValue(string name)
        {
            if (ReferenceEquals(name, null) || !(IsValidName(name)))
                throw new InvalidNameException();

            Cell cell; 

            if (cells.TryGetValue(name, out cell))
                return cell.value;
            else
                return "";
        }

        /// <summary>
        /// If content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// </summary>
        /// <param name="name">key for cell being modified</param>
        /// <param name="content">new content for cell "name"</param>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            if (ReferenceEquals(content, null))
                throw new ArgumentNullException();

            if (ReferenceEquals(name, null) || !(IsValidName(name)))
                throw new InvalidNameException();

            // holds the list of dependees to be returned from the SetCellContents method
            List<String> all_dependents;

            double result;  //holds double content

            //blank case
            if (content.Equals(""))
                all_dependents = new List<String>(SetCellContents(name, content));

            //double case
            else if (Double.TryParse(content, out result))
                all_dependents = new List<String>(SetCellContents(name, result));

            //formula case
            else if (content.Substring(0, 1).Equals("="))
            {
                //creates a formula but subtracts the '=' sign
                string formula_as_string = content.Substring(1, content.Length - 1);

                //creates formula object
                Formula f = new Formula(formula_as_string, Normalize, IsValid);

                //If this creates a circular dependency, will throw a CircularException
                //Otherwise, the contents of 'name' become f
                all_dependents = new List<String>(SetCellContents(name, f));
            }
            else //string case
                all_dependents = new List<String>(SetCellContents(name, content));

            //after changing cell content, set changed to true
            Changed = true;

            //for each dependent, we must re-evaluate it based on the new dependee value
            foreach (string s in all_dependents)
            {
                Cell cell_value;
                if (cells.TryGetValue(s, out cell_value))
                    cell_value.ReEvaluate(LookupValue);
            }

            return all_dependents;            
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            // cell.Keys should contain the names of all non-empty cells
            return cells.Keys;
        }

        /// <summary>
        /// returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        /// <param name="name">key for cell being searched for</param>

        public override object GetCellContents(String name)
        {
            if (ReferenceEquals(name, null) || !(IsValidName(name)))
                throw new InvalidNameException();

            Cell cell; 

            name = Normalize(name); 

            if (cells.TryGetValue(name, out cell))
                return cell.contents;
            else
                return "";
        }

        /// <summary>
        /// The contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// </summary>
        /// <param name="name">key for cell being modified</param>
        /// <param name="number">new content for cell "name"</param>
        protected override IList<String> SetCellContents(String name, double number)
        {
            if (ReferenceEquals(name, null) || !(IsValidName(name)))
                throw new InvalidNameException();

            Cell cell = new Cell(number);
            if (cells.ContainsKey(name))    // if it already contains that key
                cells[name] = cell;         // replace the key with the new value
            else
                cells.Add(name, cell);      

            // replace the dependents of 'name' in the dependency graph with an empty hash set
            graph.ReplaceDependees(name, new HashSet<String>());

            List<String> all_dependees = new List<String>(GetCellsToRecalculate(name));
            return all_dependees;
        }

        /// <summary>
        ///The contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// </summary>
        /// /// <param name="name">key for cell being modified</param>
        /// <param name="text">new content for cell "name"</param>
        protected override IList<String> SetCellContents(String name, String text)
        {
            if (ReferenceEquals(text, null))
                throw new ArgumentNullException();

            if (ReferenceEquals(name, null) || !(IsValidName(name)))
                throw new InvalidNameException();

            // Create a new cell
            Cell cell = new Cell(text);
            if (cells.ContainsKey(name))    // if it already contains that key
                cells[name] = cell;         // replace the key with the new value
            else
                cells.Add(name, cell);      

            string cell_content = (string)cells[name].contents; 

            if (cell_content.Equals("")) // if the contents is empty, we don't want it
                cells.Remove(name);

            graph.ReplaceDependees(name, new HashSet<String>());

            List<String> all_dependees = new List<String>(GetCellsToRecalculate(name));
            return all_dependees;
        }

        /// <summary>
        /// The contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// </summary>
        /// <param name="name">key for cell being modified</param>
        /// <param name="formula">new content for cell "name"</param>
        protected override IList<String> SetCellContents(String name, Formula formula)
        {
            if (ReferenceEquals(formula, null))
                throw new ArgumentNullException();

            if (ReferenceEquals(name, null) || !(IsValidName(name)))
                throw new InvalidNameException();

            // temp variable to hold old dependents 
            IEnumerable<String> old_dependees = graph.GetDependees(name);

            // replace the dependents of 'name' in the dependency graph with the variables in formula
            graph.ReplaceDependees(name, formula.GetVariables());

            try // checks for circular reference
            {
                List<String> all_dependees = new List<String>(GetCellsToRecalculate(name));
                Cell cell = new Cell(formula, LookupValue);
                if (cells.ContainsKey(name))    // if it already contains that key
                    cells[name] = cell;         // replace the key with the new value
                else
                    cells.Add(name, cell);     

                return all_dependees;
            }
            catch (CircularException e) // if there is a circular reference, return to old values
            {
                graph.ReplaceDependees(name, old_dependees);
                throw new CircularException();
            }

        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// </summary>
        protected override IEnumerable<String> GetDirectDependents(String name)
        {
            if (ReferenceEquals(name, null))
                throw new ArgumentNullException();

            if (!(IsValidName(name)))
                throw new InvalidNameException();

            return graph.GetDependents(name); 
        }

        /// <summary>
        /// Private helper method to check if the name of a cell is valid or not
        /// </summary>
        /// <param name="name"></param>
        private Boolean IsValidName(String name)
        {
            // remove/add $ at end of Regex
            // if it is a valid cell name, and returns true by 'IsValid' delegate, return true, else return false            
            if (Regex.IsMatch(name, @"^[a-zA-Z]+[\d]+$") && IsValid(name))
                return true;
            else return false;
        }

        /// <summary>
        /// Private helper method to read in a spreadsheet from an xml file
        /// </summary>
        /// <param name="filename">the filename we are reading the spreadsheet from</param>
        /// <param name="only_get_version">true if we only want the verison of the spreadsheet, false otherwise</param>
        private string ReadFile(string filename, bool only_get_version)
        {
            if (ReferenceEquals(filename, null))
                throw new SpreadsheetReadWriteException("The filename cannot be null");

            if (filename.Equals(""))
                throw new SpreadsheetReadWriteException("The filename cannot be empty");

            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    string name = "";       // given cell name
                    string contents = "";   // given cell contents

                    while (reader.Read())   // while reader has elements it can read                   
                    {
                        if (reader.IsStartElement())    // if reader is an opening tag
                        {
                            bool set_contents = false;

                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    if (only_get_version) // if we only want the version information
                                        return reader["version"];   
                                    else
                                        Version = reader["version"];
                                    break;
                                case "cell":
                                    if (only_get_version) // should never be true
                                        throw new SpreadsheetReadWriteException("Error: Version should have already been returned");
                                    break;
                                case "name":
                                    if (only_get_version) // should never be true
                                        throw new SpreadsheetReadWriteException("Error: Version should have already been returned");
                                    reader.Read();
                                    name = reader.Value;
                                    break;
                                case "contents":
                                    if (only_get_version) // should never be true
                                        throw new SpreadsheetReadWriteException("Error: Version should have already been returned");
                                    reader.Read();
                                    contents = reader.Value;
                                    set_contents = true;
                                    break;
                            } 

                            if (set_contents)
                                SetContentsOfCell(name, contents);

                        }                       
                    } 
                } 
            } 
            catch (XmlException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            catch (IOException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }

            return Version;
        }

        /// <summary>
        /// private class that creates cell objects
        /// </summary>
        private class Cell
        {
            
            public Object contents { get; private set; }
            public Object value { get; private set; }

            // the data type of the contents and value
            string contents_type;
            string value_type;

            /// <summary>
            /// Constructor for strings
            /// </summary>
            /// <param name="name">The name of the cell</param>
            public Cell(string name)
            {
                contents = name;
                value = contents;
                contents_type = name.GetType().ToString();
                value_type = contents_type;
            }

            /// <summary>
            /// Constructor for doubles
            /// </summary>
            /// <param name="name">The name of the cell</param>
            public Cell(double name)
            {
                contents = name;
                value = contents;
                contents_type = name.GetType().ToString();
                value_type = contents_type;
            }

            /// <summary>
            /// Constructor for formulas
            /// </summary>
            /// <param name="name">The name of the cell</param>
            /// <param name="lookup">The lookup method for the cell</param>
            public Cell(Formula name, Func<string, double> lookup)
            {
                contents = name;
                value = name.Evaluate(lookup);
                contents_type = name.GetType().ToString();
                value_type = value.GetType().ToString();
            }

            /// <summary>
            /// Helper method for re-evaluating formulas when their dependees 
            /// are changed. Used in the SetContentsOfCell method. This 
            /// method should only be used on cells that have a Formula as
            /// their contents. 
            /// </summary>
            /// <param name="lookup">Lookup delegate for value</param>
            public void ReEvaluate(Func<string, double> lookup)
            {
                if (contents_type.Equals("SpreadsheetUtilities.Formula"))
                {
                    Formula same = (Formula)contents;
                    value = same.Evaluate(lookup);
                }
            }

        } 

        /// <summary>
        /// Returns the value associated with a given cell name.          
        /// </summary>
        /// <param name="s">The cell name to be looked up</param>
        /// <returns>The value of the cell named 's'</returns>
        private double LookupValue(string s)
        {
            Cell cell; 

            // if cells contains s
            if (cells.TryGetValue(s, out cell))
            {
                if (cell.value is double)
                    return (double)cell.value;
                else 
                    throw new ArgumentException();
            }
            else 
                throw new ArgumentException();

        } 

    } 
} 