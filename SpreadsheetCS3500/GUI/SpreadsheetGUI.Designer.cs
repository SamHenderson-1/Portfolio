//  Author:    Sam Henderson
//  Partner:   Josh Leger         
//  Date:      3/3/22       
//  Course:    CS 3500, University of Utah, School of Computing 
//  Copyright: CS 3500, Sam Henderson, and Joshua Leger - This work may not be copied for use in Academic Coursework.
//
//  We certify that (unless otherwise cited) we wrote this code from scratch and did not copy it
//  in part or whole from another source. All references used in the completion of this assignment are
//  cited in our README file.

namespace GUI
{
    partial class SpreadsheetGUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crimsonClubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spreadsheetGridWidget1 = new SpreadsheetGrid_Core.SpreadsheetGridWidget();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cellNameLabel = new System.Windows.Forms.Label();
            this.cellNameTextBox = new System.Windows.Forms.TextBox();
            this.cellValueLabel = new System.Windows.Forms.Label();
            this.cellValueTextBox = new System.Windows.Forms.TextBox();
            this.cellContentsLabel = new System.Windows.Forms.Label();
            this.cellContentsTextBox = new System.Windows.Forms.TextBox();
            this.setCellButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(956, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.themeToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // themeToolStripMenuItem
            // 
            this.themeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultToolStripMenuItem,
            this.darkModeToolStripMenuItem,
            this.crimsonClubToolStripMenuItem});
            this.themeToolStripMenuItem.Name = "themeToolStripMenuItem";
            this.themeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.themeToolStripMenuItem.Text = "Theme";
            // 
            // defaultToolStripMenuItem
            // 
            this.defaultToolStripMenuItem.Checked = true;
            this.defaultToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            this.defaultToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.defaultToolStripMenuItem.Text = "Default";
            this.defaultToolStripMenuItem.Click += new System.EventHandler(this.defaultToolStripMenuItem_Click);
            // 
            // darkModeToolStripMenuItem
            // 
            this.darkModeToolStripMenuItem.Name = "darkModeToolStripMenuItem";
            this.darkModeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.darkModeToolStripMenuItem.Text = "Dark Mode";
            this.darkModeToolStripMenuItem.Click += new System.EventHandler(this.darkModeToolStripMenuItem_Click);
            // 
            // crimsonClubToolStripMenuItem
            // 
            this.crimsonClubToolStripMenuItem.Name = "crimsonClubToolStripMenuItem";
            this.crimsonClubToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.crimsonClubToolStripMenuItem.Text = "Crimson Club";
            this.crimsonClubToolStripMenuItem.Click += new System.EventHandler(this.crimsonClubToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutMeToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutMeToolStripMenuItem
            // 
            this.aboutMeToolStripMenuItem.Name = "aboutMeToolStripMenuItem";
            this.aboutMeToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.aboutMeToolStripMenuItem.Text = "About Me";
            this.aboutMeToolStripMenuItem.Click += new System.EventHandler(this.aboutMeTool_Click);
            // 
            // spreadsheetGridWidget1
            // 
            this.spreadsheetGridWidget1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.spreadsheetGridWidget1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetGridWidget1.Location = new System.Drawing.Point(3, 157);
            this.spreadsheetGridWidget1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.spreadsheetGridWidget1.Name = "spreadsheetGridWidget1";
            this.spreadsheetGridWidget1.Size = new System.Drawing.Size(1087, 576);
            this.spreadsheetGridWidget1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.spreadsheetGridWidget1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 153F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1093, 757);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.64494F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.35506F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 457F));
            this.tableLayoutPanel2.Controls.Add(this.cellNameLabel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cellNameTextBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.cellValueLabel, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cellValueTextBox, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.cellContentsLabel, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.cellContentsTextBox, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.setCellButton, 2, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel2.MinimumSize = new System.Drawing.Size(51, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1087, 145);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // cellNameLabel
            // 
            this.cellNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cellNameLabel.AutoSize = true;
            this.cellNameLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cellNameLabel.Location = new System.Drawing.Point(75, 25);
            this.cellNameLabel.Name = "cellNameLabel";
            this.cellNameLabel.Size = new System.Drawing.Size(102, 23);
            this.cellNameLabel.TabIndex = 0;
            this.cellNameLabel.Text = "Cell Name :";
            // 
            // cellNameTextBox
            // 
            this.cellNameTextBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.cellNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cellNameTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.cellNameTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cellNameTextBox.Location = new System.Drawing.Point(183, 24);
            this.cellNameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cellNameTextBox.Name = "cellNameTextBox";
            this.cellNameTextBox.ReadOnly = true;
            this.cellNameTextBox.Size = new System.Drawing.Size(277, 22);
            this.cellNameTextBox.TabIndex = 1;
            this.cellNameTextBox.Text = "A1";
            // 
            // cellValueLabel
            // 
            this.cellValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cellValueLabel.AutoSize = true;
            this.cellValueLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cellValueLabel.Location = new System.Drawing.Point(114, 59);
            this.cellValueLabel.Name = "cellValueLabel";
            this.cellValueLabel.Size = new System.Drawing.Size(63, 23);
            this.cellValueLabel.TabIndex = 2;
            this.cellValueLabel.Text = "Value :";
            // 
            // cellValueTextBox
            // 
            this.cellValueTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cellValueTextBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.cellValueTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cellValueTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.cellValueTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cellValueTextBox.Location = new System.Drawing.Point(183, 60);
            this.cellValueTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cellValueTextBox.Name = "cellValueTextBox";
            this.cellValueTextBox.ReadOnly = true;
            this.cellValueTextBox.Size = new System.Drawing.Size(277, 22);
            this.cellValueTextBox.TabIndex = 3;
            // 
            // cellContentsLabel
            // 
            this.cellContentsLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cellContentsLabel.AutoSize = true;
            this.cellContentsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cellContentsLabel.Location = new System.Drawing.Point(86, 93);
            this.cellContentsLabel.Name = "cellContentsLabel";
            this.cellContentsLabel.Size = new System.Drawing.Size(91, 23);
            this.cellContentsLabel.TabIndex = 4;
            this.cellContentsLabel.Text = "Contents :";
            // 
            // cellContentsTextBox
            // 
            this.cellContentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cellContentsTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.cellContentsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cellContentsTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cellContentsTextBox.Location = new System.Drawing.Point(183, 94);
            this.cellContentsTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cellContentsTextBox.Name = "cellContentsTextBox";
            this.cellContentsTextBox.Size = new System.Drawing.Size(443, 22);
            this.cellContentsTextBox.TabIndex = 5;
            this.cellContentsTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ProcessCommandKey_KeyPress);
            // 
            // setCellButton
            // 
            this.setCellButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.setCellButton.BackColor = System.Drawing.Color.LightGreen;
            this.setCellButton.Location = new System.Drawing.Point(632, 89);
            this.setCellButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.setCellButton.Name = "setCellButton";
            this.setCellButton.Size = new System.Drawing.Size(103, 31);
            this.setCellButton.TabIndex = 6;
            this.setCellButton.Text = "Set Cell";
            this.setCellButton.UseVisualStyleBackColor = false;
            this.setCellButton.Click += new System.EventHandler(this.setCellButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SpreadsheetGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1093, 787);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(816, 343);
            this.Name = "SpreadsheetGUI";
            this.Text = "Xcellent";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SpreadsheetGrid_Core.SpreadsheetGridWidget spreadsheetGridWidget1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutMeToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label cellNameLabel;
        private TextBox cellNameTextBox;
        private Label cellValueLabel;
        private TextBox cellValueTextBox;
        private Label cellContentsLabel;
        private TextBox cellContentsTextBox;
        private Button setCellButton;
        private SaveFileDialog saveFileDialog1;

        private ToolStripMenuItem saveAsToolStripMenuItem;
        private OpenFileDialog openFileDialog1;


        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem themeToolStripMenuItem;
        private ToolStripMenuItem defaultToolStripMenuItem;
        private ToolStripMenuItem darkModeToolStripMenuItem;
        private ToolStripMenuItem crimsonClubToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}