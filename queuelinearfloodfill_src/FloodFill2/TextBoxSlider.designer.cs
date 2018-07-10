namespace FloodFill2
{
	partial class TextBoxSlider
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.trackBar = new System.Windows.Forms.TrackBar();
			this.textBox = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.trackBar, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.textBox, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(253, 51);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// trackBar
			// 
			this.trackBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trackBar.Location = new System.Drawing.Point(3, 3);
			this.trackBar.Maximum = 100;
			this.trackBar.Name = "trackBar";
			this.trackBar.Size = new System.Drawing.Size(189, 45);
			this.trackBar.TabIndex = 1;
			this.trackBar.TabStop = false;
			this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
			this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
			// 
			// textBox
			// 
			this.textBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.textBox.Location = new System.Drawing.Point(198, 15);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(52, 20);
			this.textBox.TabIndex = 0;
			this.textBox.Leave += new System.EventHandler(this.textBox_Leave);
			this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			// 
			// TextBoxSlider
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "TextBoxSlider";
			this.Size = new System.Drawing.Size(253, 51);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.TrackBar trackBar;
	}
}
