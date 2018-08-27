namespace Emgu_Forms {
	partial class Form {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.Status_Update_TextBox = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.Right_Header_TextBox = new System.Windows.Forms.TextBox();
			this.Left_Header_TextBox = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// Status_Update_TextBox
			// 
			this.Status_Update_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Status_Update_TextBox.Location = new System.Drawing.Point(0, 568);
			this.Status_Update_TextBox.Multiline = true;
			this.Status_Update_TextBox.Name = "Status_Update_TextBox";
			this.Status_Update_TextBox.ReadOnly = true;
			this.Status_Update_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.Status_Update_TextBox.Size = new System.Drawing.Size(982, 50);
			this.Status_Update_TextBox.TabIndex = 1;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.Right_Header_TextBox, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.Left_Header_TextBox, 0, 0);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, -2);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(982, 26);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// Right_Header_TextBox
			// 
			this.Right_Header_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Right_Header_TextBox.Location = new System.Drawing.Point(494, 3);
			this.Right_Header_TextBox.Name = "Right_Header_TextBox";
			this.Right_Header_TextBox.ReadOnly = true;
			this.Right_Header_TextBox.Size = new System.Drawing.Size(485, 20);
			this.Right_Header_TextBox.TabIndex = 2;
			this.Right_Header_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Left_Header_TextBox
			// 
			this.Left_Header_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Left_Header_TextBox.Location = new System.Drawing.Point(3, 3);
			this.Left_Header_TextBox.Name = "Left_Header_TextBox";
			this.Left_Header_TextBox.ReadOnly = true;
			this.Left_Header_TextBox.Size = new System.Drawing.Size(485, 20);
			this.Left_Header_TextBox.TabIndex = 1;
			this.Left_Header_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Location = new System.Drawing.Point(3, 28);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(976, 534);
			this.panel1.TabIndex = 3;
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Location = new System.Drawing.Point(4, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(481, 528);
			this.panel2.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel3.Location = new System.Drawing.Point(491, 3);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(481, 528);
			this.panel3.TabIndex = 1;
			// 
			// Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(982, 618);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Controls.Add(this.Status_Update_TextBox);
			this.Name = "Form";
			this.Text = "Form1";
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox Status_Update_TextBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TextBox Right_Header_TextBox;
		private System.Windows.Forms.TextBox Left_Header_TextBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel2;
	}
}

