//  Author:    Sam Henderson
//  Partner:   Josh Leger         
//  Date:      3/3/22       
//  Course:    CS 3500, University of Utah, School of Computing 
//  Copyright: CS 3500, Sam Henderson, and Joshua Leger - This work may not be copied for use in Academic Coursework.
//
//  We certify that (unless otherwise cited) we wrote this code from scratch and did not copy it
//  in part or whole from another source. All references used in the completion of this assignment are
//  cited in our README file.
//
//  NOTE: The majority of this program.cs file was take directly from the
//  demo spreadsheet example given in the ForStudents Repo.

namespace GUI
{
    /// <summary>
    /// Keeps track of how many top-level forms are running
    /// </summary>
    class SpreadsheetWindow : ApplicationContext
    {
        // Number of open forms
        private int formCount = 0;

        // Singleton ApplicationContext
        private static SpreadsheetWindow appContext;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetWindow()
        {
        }

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        /// Taken From ForStudent Example
        public static SpreadsheetWindow getAppContext()
        {
            if (appContext == null)
            {
                appContext = new SpreadsheetWindow();
            }
            return appContext;
        }

        /// <summary>
        /// Runs the form
        /// </summary>
        /// <source>Taken From ForStudent Example</source>
        /// <param name="form">The form to be run</param>
        public int RunForm(Form form)
        {
            // One more form is running
            formCount++;

            // When this form closes, we want to find out
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            // Run the form
            form.Show();

            return formCount;
        }
    }

    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // Start an application context and run one form inside it
            SpreadsheetWindow appContext = SpreadsheetWindow.getAppContext();
            appContext.RunForm(new SpreadsheetGUI());
            Application.Run(appContext);
        }
    }
}