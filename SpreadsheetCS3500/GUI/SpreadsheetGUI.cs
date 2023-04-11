//  Author:    Sam Henderson
//  Partner:   Josh Leger         
//  Date:      3/3/22       
//  Course:    CS 3500, University of Utah, School of Computing 
//  Copyright: CS 3500, Sam Henderson, and Joshua Leger - This work may not be copied for use in Academic Coursework.
//
//  We certify that (unless otherwise cited) we wrote this code from scratch and did not copy it
//  in part or whole from another source. All references used in the completion of this assignment are
//  cited in our README file.

using SpreadsheetGrid_Core;
using System.Diagnostics;
using SS;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;
using System.ComponentModel;

namespace GUI
{
    public partial class SpreadsheetGUI : Form
    {
        private AbstractSpreadsheet spreadsheet;
        private string filename;
        private string selected_cell_name;
        private int selected_cell_col;
        private int selected_cell_row;
        

        public SpreadsheetGUI()
        {
            InitializeComponent();
            selected_cell_col = 0;
            selected_cell_row = 0;
            selected_cell_name = "A1";
            spreadsheet = new Spreadsheet(s => is_valid(s), s => s.ToUpper(), "six");
            this.spreadsheetGridWidget1.SelectionChanged += select_the_cell;
            this.FormClosing += check_for_save;            
        }

        /// <summary>
        /// Sets the cell within the spreadsheet back-end. If no Circular or Formula Format
        /// exceptions are caught it also sets the dispalayed cell value.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="user_input"></param>
        /// <returns></returns>
        private IList<string> set_the_cells(int col, int row, string user_input)
        {
             
            string cellName = convert_points_to_name(col, row);

            IList<string> cells_to_recalc = Enumerable.Empty<string>().ToList();
            try
            {
                cells_to_recalc = this.spreadsheet.SetContentsOfCell(cellName, user_input);
            }
            catch(Exception ex)
            {
                if(ex is CircularException)
                {
                    //This was taken from MS c# documentation:
                    // Initializes the variables to pass to the MessageBox.Show method.
                    string message = "Error: A Circular Dependency Was Detected.";
                    string caption = "Error";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.
                    result = MessageBox.Show(message, caption, buttons);
                }
                else if(ex is FormulaFormatException)
                {
                    //This was taken from MS c# documentation:
                    // Initializes the variables to pass to the MessageBox.Show method.
                    string message = "Error: Formula Syntax Error!";
                    string caption = "Error";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.
                    result = MessageBox.Show(message, caption, buttons);
                }
                else
                {
                    throw;
                }
            }
            //sets display value based on spreadsheet back-end data.
            this.spreadsheetGridWidget1.SetValue(col, row, spreadsheet.GetCellValue(cellName).ToString());
            return cells_to_recalc;
        }

        /// <summary>
        /// Ensures string is appropriate cell name for the give gui.
        /// First character must by A-Z and must be followed by a number
        /// between 1-99 inclusively.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool is_valid(string s)
        {
            return Regex.IsMatch(s, "^[A-Z][1-9][0-9]?$");
        }

        /// <summary>
        /// upon selecting a cell, the cell info is stored in
        /// the GUI properties. The cell info is displayed in 
        /// the display area. The contents text box is also highlighted
        /// to allow for editing upon click.
        /// </summary>
        /// <param name="sender"></param>
        public void select_the_cell(SpreadsheetGridWidget sender)
        {
            sender.GetSelection(out int col, out int row);
            this.selected_cell_col = col;
            this.selected_cell_row = row;
            this.selected_cell_name = convert_points_to_name(col, row);

            display_selected_cell_info();
            
            //this highlights the cell contents text box to allow
            //for editing upon click.
            this.cellContentsTextBox.Focus();
        }

        /// <summary>
        /// Display the selected cell info in the top information bar.
        /// The information is retrieved from the spreadsheet obj.
        /// </summary>
        private void display_selected_cell_info()
        {
            this.cellNameTextBox.Text = selected_cell_name;
            this.cellContentsTextBox.Text = spreadsheet.GetCellContents(selected_cell_name).ToString();
            this.cellValueTextBox.Text = spreadsheet.GetCellValue(selected_cell_name).ToString();
        }

        /// <summary>
        /// Converts a coordinate in the form (x,y) to the 
        /// appropriate cell name.
        /// <para>
        /// For example: Input (0,0) will result in A1. While input (25,98) returns Z99.
        /// </para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>A cell name in the form A1</returns>
        private string convert_points_to_name(int x, int y)
        {
            char starting_char = 'A';
            char name_of_col = (Char)(Convert.ToUInt16(starting_char) + x);
            return $"{name_of_col}{y+1}";
        }

        /// <summary>
        /// Action handler for the "Set Cell" button.
        /// Creates a new thread and has it sets the cell value in the spreadsheet obj as well as 
        /// the spreadsheetGridWidget obj
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setCellButton_Click(object sender, EventArgs e)
        {
            //creates a new thread
            Thread thread = new Thread(set_cells_using_thread);

            //disables user input
            this.cellContentsTextBox.Enabled = false;
            this.cellContentsTextBox.Enabled = false;

            
            thread.Start();

            //waits for the thread to finish working.
            thread.Join();
            
            //re-enables user input.
            this.setCellButton.Enabled = true;
            this.cellContentsTextBox.Enabled = true;

            display_selected_cell_info();

        }

        /// <summary>
        /// This is the method being called by the Thread within the setCellButton click.
        /// it sets the cells via the set_the_cells() funciton which returns a list of 
        /// cells that need to be recalculated, and thus their displayed values will 
        /// be adjusted. This operation is completed with the reset_displayed_values().
        /// </summary>
        private void set_cells_using_thread()
        {
            
            IList<string> cells_to_recalc =
               set_the_cells(selected_cell_col, selected_cell_row, cellContentsTextBox.Text);

            reset_displayed_values(cells_to_recalc);
        }

        /// <summary>
        /// Changes the displayed value of each cell in the list.
        /// </summary>
        /// <param name="cells_to_recalc"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void reset_displayed_values(IList<string> cells_to_recalc)
        {
            foreach(string cell in cells_to_recalc)
            {
                get_coord_from_name(cell, out int x, out int y);
                object cell_value_object = spreadsheet.GetCellValue(cell);
                string?  cell_display_value = cell_value_object.ToString();
                
                //if the value is a FormualError, it changes the displayed
                //cell to "RefError", else to the value of the cell
                spreadsheetGridWidget1.SetValue(x, y, (cell_value_object is FormulaError) ? "!!RefError!!" : (cell_display_value));
            }
        }

        /// <summary>
        /// Converts a given cell name into a row, col coordinate.
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void get_coord_from_name(string cellName, out int col, out int row)
        {
            char col_char = cellName[0];
            col = col_char - 65;
            row = int.Parse(cellName.Substring(1)) - 1;
        }

        /// <summary>
        /// Method that creates a new Spreadsheet when the tool strip's "new" is clicked
        /// </summary>
        /// <param name="sender">default</param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadsheetWindow.getAppContext().RunForm(new SpreadsheetGUI());
        }

        /// <summary>
        /// Method that saves the current Spreadsheet when the tool strip's "save" is clicked.
        /// </summary>
        /// <param name="sender">default</param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //If the spreadsheet is currently unamed, call Save As
            if (ReferenceEquals(filename, null))
                saveAsToolStripMenuItem_Click(sender, e);
            

            // if the file name is still empty, it means Save As was canceled, which means we return
            if (ReferenceEquals(filename, null))
            {
                return;
            }

            // If the spreadsheet has been changed then save it
            if (spreadsheet.Changed)
                spreadsheet.Save(filename);
        }

        /// <summary>
        /// Method that saves the current Spreadsheet with a set filename when the tool strip's "save as" is clicked.
        /// </summary>
        /// <param name="sender">default</param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make the user choose a file name
            saveFileDialog1.Filter = "Spreadsheet Files (*.sprd)|*.sprd|All Files (*.*)|*.*";
            saveFileDialog1.DefaultExt = ".sprd";
            saveFileDialog1.Title = "Save";
            saveFileDialog1.ShowDialog();

            // get the name of the selected file
            string savename = saveFileDialog1.FileName;

            // if the file name is empty, return
            if (savename == "")
                return;

            // do a check here to make sure the user wants to overwrite an existing file
            if (File.Exists(savename))
            {
                DialogResult result = MessageBox.Show("Saving this spreadsheet to " + savename +
                        " will overwrite any data contained in that file. Save anyways?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // if 'No' then we don't want to open the file so return
                if (result == DialogResult.No)
                    return;
            }

            this.Text = savename; // set the name of the document to the saved name

            // if the user selected option 1 (needs to be .sprd) then if the selected
            // filename does not end with .sprd, append it to the end
            if (saveFileDialog1.FilterIndex == 1)
                saveFileDialog1.AddExtension = true;

            // once we have a proper file name, save the spreadsheet to that file
            spreadsheet.Save(savename);

            // once we save, set fileName to where we saved the spreadsheet
            filename = savename;
        }

        /// <summary>
        /// Method that opens a Spreadsheet with a filename when the tool strip's "open" is clicked.
        /// </summary>
        /// <param name="sender">default</param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Spreadsheet Files (*.sprd)|*.sprd|All Files (*.*)|*.*";
            openFileDialog1.DefaultExt = ".sprd";
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Open";
            openFileDialog1.ShowDialog();

            // get the name of the selected file
            string openname = openFileDialog1.FileName;

            // if the file name is empty, give a message and return
            if (openname == "")
                return;

            // if the user selected option 1 (needs to be .sprd) then if the selected
            // filename does not end with .sprd, tell it to add the default extension
            if (openFileDialog1.FilterIndex == 1)
                openFileDialog1.AddExtension = true;

            // if the spreadsheet has been changed, make sure the user wants to overwrite changes
            if (spreadsheet.Changed)
            {
                string message = "Your spreadsheet has unsaved changed. Opening " +
                    "this file will overwrite any changes that have not been saved. Open anyways?";
                string caption = "Warning";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Warning);

                // if 'No' then we don't want to open the file so return
                if (result == DialogResult.No)
                    return;
            }

            try // Try to load a spreadsheet from a file
            {
                // clear each cell value in the spreadsheet
                spreadsheetGridWidget1.Clear();

                // update spreadsheet with new file data
                spreadsheet = new Spreadsheet(openname, is_valid, s => s.ToUpper(), "six");
                filename = openname; // initialize to the filepath where opened
                this.Text = openname; // set title to filename            

                Thread loadThread = new Thread(() =>
                {
                   foreach (string s in spreadsheet.GetNamesOfAllNonemptyCells())
                   {
                       get_coord_from_name(s, out int col, out int row);
                       if (spreadsheet.GetCellContents(s) is Formula)
                            set_the_cells(col, row, "=" + spreadsheet.GetCellContents(s).ToString());
                        else
                            set_the_cells(col, row, spreadsheet.GetCellContents(s).ToString());
                   }
                });
                loadThread.Start();           
            }
            catch (SpreadsheetReadWriteException s)
            {
                  MessageBox.Show("Error: There was a problem reading the selected file. Please"
                + " make sure the file exists with a valid name and try again.", "Error Reading Spreadsheet", MessageBoxButtons.OK, MessageBoxIcon.Error); // display the message from the exception
            }
            this.display_selected_cell_info();
        }

        /// <summary>
        /// Method that closes the current Spreadsheet when the tool strip's "close" is clicked.
        /// </summary>
        /// <param name="sender">default</param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(spreadsheet.Changed)) // If no changes have been made just close the file
                Close();
            else
            {   
                string message = "You have unsaved changes, do you wish to save before exiting?";
                string caption = "Warning";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Warning);

                // if 'Yes' then save the file
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem.PerformClick(); // call the save method
        
                }
                else if (result == DialogResult.Cancel)
                    return;

                Close();
            }
        }

        /// <summary>
        /// Checks whether or not the user wants to save before closing the current Spreadsheet using the red X.
        /// Afterwards, closes the Spreadsheet with the red X.
        /// </summary>
        /// <param name="sender">default</param>
        /// <param name="e"></param>
        private void check_for_save(object? sender, FormClosingEventArgs e)
        {
            if(spreadsheet.Changed)
            {
                string message = "You have unsaved changes, do you wish to save before exiting?";
                string caption = "Warning";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;

                DialogResult result = MessageBox.Show(message, caption, buttons);

                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e); 
                }
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }

            }
        }

        /// <summary>
        /// This method will allow the user to use keyboard keys to interact with the spreadsheet
        /// Press Enter/Return to Set Cell.
        /// </summary>
        /// <param name = "sender" > key stroke</param>
        /// <param name = "e" > message </ param >
        /// < returns > true </ returns >
        protected void ProcessCommandKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar) {
                case '\r':
                    setCellButton_Click(sender, e);
                    e.Handled = true;
                    break;
            }
        }
        
        /// <summary>
        /// Changes the appearance of the GUI by changing the text color as well
        /// as multiple background colors of different elements throughout the gui.
        /// </summary>
        /// <param name="text1"></param>
        /// <param name="text2"></param>
        /// <param name="menuColor"></param>
        /// <param name="cellBorderColor"></param>
        /// <param name="mainColor"></param>
        private void change_theme(Color text1, Color text2, Color menuColor, Color cellBorderColor, Color mainColor)
        {
            /////Menu Items////
            this.menuStrip1.BackColor = menuColor;
            this.fileToolStripMenuItem.ForeColor = text1;
            this.helpToolStripMenuItem.ForeColor = text1;
            this.viewToolStripMenuItem.ForeColor = text1;
            this.openToolStripMenuItem.BackColor = menuColor;
            this.saveAsToolStripMenuItem.BackColor = menuColor;
            this.saveToolStripMenuItem.BackColor = menuColor;
            this.newToolStripMenuItem.BackColor = menuColor;
            this.closeToolStripMenuItem.BackColor = menuColor;
            this.openToolStripMenuItem.ForeColor = text1;
            this.newToolStripMenuItem.ForeColor = text1;
            this.saveToolStripMenuItem.ForeColor = text1;
            this.saveAsToolStripMenuItem.ForeColor = text1;
            this.closeToolStripMenuItem.ForeColor = text1;
            this.themeToolStripMenuItem.ForeColor = text1;
            this.themeToolStripMenuItem.BackColor = menuColor;
            this.defaultToolStripMenuItem.ForeColor = text1;
            this.defaultToolStripMenuItem.BackColor = menuColor;
            this.darkModeToolStripMenuItem.ForeColor = text1;
            this.darkModeToolStripMenuItem.BackColor = menuColor;
            this.crimsonClubToolStripMenuItem.ForeColor = text1;
            this.crimsonClubToolStripMenuItem.BackColor = menuColor;
            this.aboutMeToolStripMenuItem.ForeColor = text1;
            this.aboutMeToolStripMenuItem.BackColor = menuColor;

            //////Top UI Elements/////
            this.tableLayoutPanel1.BackColor = mainColor;
            this.cellContentsLabel.ForeColor = text2;
            this.cellValueLabel.ForeColor = text2;
            this.cellValueTextBox.ForeColor = text2;
            this.cellValueTextBox.BackColor = mainColor;
            this.cellNameLabel.ForeColor = text2;
            this.cellNameTextBox.BackColor = mainColor;
            this.cellNameTextBox.ForeColor = text2;

            /////Spreadsheet Border/////
            this.spreadsheetGridWidget1.BackColor = cellBorderColor;
        }

        /// <summary>
        /// This set's the dark mode theme as the selected theme in the menu bar,
        /// and then sets the GUI elements with change_theme() function. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void darkModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.defaultToolStripMenuItem.Checked = false;
            this.crimsonClubToolStripMenuItem.Checked = false;
            this.darkModeToolStripMenuItem.Checked = true;
            change_theme(Color.White, Color.White, Color.FromArgb(36, 59, 83), Color.FromArgb(98, 125, 152), Color.FromArgb(16, 42, 67));
        }

        /// <summary>
        /// This set's the crimson club theme as the selected theme in the menu bar,
        /// and then sets the GUI elements with change_theme() function. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void crimsonClubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.defaultToolStripMenuItem.Checked = false;
            this.crimsonClubToolStripMenuItem.Checked = true;
            this.darkModeToolStripMenuItem.Checked = false;
            change_theme(Color.Black, Color.White, Color.FromArgb(192, 192, 192), Color.FromArgb(192, 192, 192), Color.FromArgb(204, 0, 0));
        }

        /// <summary>
        /// This set's the default theme as the selected theme in the menu bar,
        /// and then sets the GUI elements with change_theme() function. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.defaultToolStripMenuItem.Checked = true;
            this.crimsonClubToolStripMenuItem.Checked = false;
            this.darkModeToolStripMenuItem.Checked = false;
            change_theme(Color.Black, Color.Black, SystemColors.Control, SystemColors.MenuHighlight, SystemColors.InactiveCaption);
        }

        /// <summary>
        /// Displays a message when user clicks on the 'About Me' menu button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutMeTool_Click(object sender, EventArgs e)
        {
            
                MessageBox.Show(
                "This Spreadsheet program, developed by Sam Henderson and Josh Leger, allows users to enter text into " + 
                "cells in a grid.\n\nSelecting and Setting Cells:\n\n\tTo select a cell, simply click on the cell you wish to" +
                "select. Upon selecting a cell, you will see that the selected cell information (Name, value, and contents) will" +
                " be displayed (if cell contains anything). Once selected, you may begin typing to edit the contents of the cell." +
                " You will see that the Cell Contents box at the top of the application will change as you type. " +
                "To set or update the cell, either hit the Enter/Return key, or click on the Set Cell button.\n\n" +
                "Saving and Opening:\n\n\tThe Spreadsheet also allows user to save their content to a .sprd file using either" +
                " the Save or Save As feature. Save is used to save a .sprd to a preexisting location. If the Spreadsheet has" +
                " not previously been saved, Save will call the Save As method, assigning the .sprd file to a new name and location." +
                " Save As can also be called independently. Users can also open saved .sprd files with the Open tool. Lastly, users can close " +
                "spreadsheets using either the 'Close' tool or the red X at the window's top right corner. \n\n Theme Change:\n\n\tThe Spreadsheet's " +
                "color theme may be changed by going to View > Theme and then select either Default, Dark Mode, or Crimson Club.\n\nErrors:\n\n\t" +
                "Each cell is capable of holding a FormulaError object when a formula cannot be evaluated due to a missing variable reference." +
                " If either a Circular Dependency or Formula Syntax error are encountered, the user will be notified by an alert window," +
                " and the cell will revert to their previous contents.", "About Me.", MessageBoxButtons.OK);
        }
    }
}
