namespace PuzzleGameTestTask
{
	partial class Form1
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonImageBrowse = new System.Windows.Forms.Button();
			this.textBoxImagePath = new System.Windows.Forms.TextBox();
			this.groupBoxPuzzle = new System.Windows.Forms.GroupBox();
			this.groupBoxControl = new System.Windows.Forms.GroupBox();
			this.textBoxColumns = new System.Windows.Forms.TextBox();
			this.textBoxRows = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.labelRow = new System.Windows.Forms.Label();
			this.buttonCheck = new System.Windows.Forms.Button();
			this.buttonControl = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBoxControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.buttonImageBrowse);
			this.groupBox1.Controls.Add(this.textBoxImagePath);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(950, 58);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Choose Image";
			// 
			// buttonImageBrowse
			// 
			this.buttonImageBrowse.Location = new System.Drawing.Point(778, 21);
			this.buttonImageBrowse.Name = "buttonImageBrowse";
			this.buttonImageBrowse.Size = new System.Drawing.Size(75, 23);
			this.buttonImageBrowse.TabIndex = 1;
			this.buttonImageBrowse.Text = "...";
			this.buttonImageBrowse.UseVisualStyleBackColor = true;
			this.buttonImageBrowse.Click += new System.EventHandler(this.buttonImageBrowse_Click);
			// 
			// textBoxImagePath
			// 
			this.textBoxImagePath.Location = new System.Drawing.Point(6, 22);
			this.textBoxImagePath.Name = "textBoxImagePath";
			this.textBoxImagePath.Size = new System.Drawing.Size(622, 23);
			this.textBoxImagePath.TabIndex = 0;
			// 
			// groupBoxPuzzle
			// 
			this.groupBoxPuzzle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.groupBoxPuzzle.Location = new System.Drawing.Point(14, 76);
			this.groupBoxPuzzle.Margin = new System.Windows.Forms.Padding(5);
			this.groupBoxPuzzle.Name = "groupBoxPuzzle";
			this.groupBoxPuzzle.Padding = new System.Windows.Forms.Padding(5);
			this.groupBoxPuzzle.Size = new System.Drawing.Size(807, 485);
			this.groupBoxPuzzle.TabIndex = 1;
			this.groupBoxPuzzle.TabStop = false;
			this.groupBoxPuzzle.Text = "Puzzle";
			// 
			// groupBoxControl
			// 
			this.groupBoxControl.Controls.Add(this.textBoxColumns);
			this.groupBoxControl.Controls.Add(this.textBoxRows);
			this.groupBoxControl.Controls.Add(this.label1);
			this.groupBoxControl.Controls.Add(this.labelRow);
			this.groupBoxControl.Controls.Add(this.buttonCheck);
			this.groupBoxControl.Controls.Add(this.buttonControl);
			this.groupBoxControl.Location = new System.Drawing.Point(840, 76);
			this.groupBoxControl.Name = "groupBoxControl";
			this.groupBoxControl.Size = new System.Drawing.Size(122, 261);
			this.groupBoxControl.TabIndex = 2;
			this.groupBoxControl.TabStop = false;
			this.groupBoxControl.Text = "Control";
			// 
			// textBoxColumns
			// 
			this.textBoxColumns.Location = new System.Drawing.Point(85, 43);
			this.textBoxColumns.Name = "textBoxColumns";
			this.textBoxColumns.Size = new System.Drawing.Size(30, 23);
			this.textBoxColumns.TabIndex = 4;
			this.textBoxColumns.Text = "4";
			this.textBoxColumns.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxRows
			// 
			this.textBoxRows.Location = new System.Drawing.Point(85, 16);
			this.textBoxRows.Name = "textBoxRows";
			this.textBoxRows.Size = new System.Drawing.Size(30, 23);
			this.textBoxRows.TabIndex = 4;
			this.textBoxRows.Text = "4";
			this.textBoxRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 46);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 15);
			this.label1.TabIndex = 3;
			this.label1.Text = "Columns:";
			// 
			// labelRow
			// 
			this.labelRow.AutoSize = true;
			this.labelRow.Location = new System.Drawing.Point(7, 19);
			this.labelRow.Name = "labelRow";
			this.labelRow.Size = new System.Drawing.Size(38, 15);
			this.labelRow.TabIndex = 2;
			this.labelRow.Text = "Rows:";
			// 
			// buttonCheck
			// 
			this.buttonCheck.Enabled = false;
			this.buttonCheck.Location = new System.Drawing.Point(31, 144);
			this.buttonCheck.Name = "buttonCheck";
			this.buttonCheck.Size = new System.Drawing.Size(75, 23);
			this.buttonCheck.TabIndex = 1;
			this.buttonCheck.Text = "Check";
			this.buttonCheck.UseVisualStyleBackColor = true;
			// 
			// buttonControl
			// 
			this.buttonControl.Enabled = false;
			this.buttonControl.Location = new System.Drawing.Point(31, 72);
			this.buttonControl.Name = "buttonControl";
			this.buttonControl.Size = new System.Drawing.Size(75, 23);
			this.buttonControl.TabIndex = 0;
			this.buttonControl.Text = "Shuffle";
			this.buttonControl.UseVisualStyleBackColor = true;
			this.buttonControl.Click += new System.EventHandler(this.buttonControl_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(974, 569);
			this.Controls.Add(this.groupBoxControl);
			this.Controls.Add(this.groupBoxPuzzle);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Puzzle Game";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBoxControl.ResumeLayout(false);
			this.groupBoxControl.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonImageBrowse;
		private System.Windows.Forms.TextBox textBoxImagePath;
		private System.Windows.Forms.GroupBox groupBoxPuzzle;
		private System.Windows.Forms.GroupBox groupBoxControl;
		private System.Windows.Forms.Button buttonCheck;
		private System.Windows.Forms.Button buttonControl;
		private System.Windows.Forms.TextBox textBoxColumns;
		private System.Windows.Forms.TextBox textBoxRows;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelRow;
	}
}

