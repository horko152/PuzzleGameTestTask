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
			this.textBoxImagePath = new System.Windows.Forms.TextBox();
			this.buttonImageBrowse = new System.Windows.Forms.Button();
			this.groupBoxPuzzle = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
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
			// textBoxImagePath
			// 
			this.textBoxImagePath.Location = new System.Drawing.Point(6, 22);
			this.textBoxImagePath.Name = "textBoxImagePath";
			this.textBoxImagePath.Size = new System.Drawing.Size(622, 23);
			this.textBoxImagePath.TabIndex = 0;
			// 
			// buttonImageBrowse
			// 
			this.buttonImageBrowse.Location = new System.Drawing.Point(778, 21);
			this.buttonImageBrowse.Name = "buttonImageBrowse";
			this.buttonImageBrowse.Size = new System.Drawing.Size(75, 23);
			this.buttonImageBrowse.TabIndex = 1;
			this.buttonImageBrowse.Text = "...";
			this.buttonImageBrowse.UseVisualStyleBackColor = true;
			// 
			// groupBoxPuzzle
			// 
			this.groupBoxPuzzle.Location = new System.Drawing.Point(12, 76);
			this.groupBoxPuzzle.Name = "groupBoxPuzzle";
			this.groupBoxPuzzle.Size = new System.Drawing.Size(628, 481);
			this.groupBoxPuzzle.TabIndex = 1;
			this.groupBoxPuzzle.TabStop = false;
			this.groupBoxPuzzle.Text = "Puzzle";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(974, 569);
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
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonImageBrowse;
		private System.Windows.Forms.TextBox textBoxImagePath;
		private System.Windows.Forms.GroupBox groupBoxPuzzle;
	}
}

