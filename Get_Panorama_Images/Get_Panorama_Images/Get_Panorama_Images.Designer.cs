namespace Get_Panorama_Images
{
	partial class Get_Panorama_Images
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.DisplayWindow = new System.Windows.Forms.PictureBox();
			this.Button_Live_Video = new System.Windows.Forms.Button();
			this.Button_Stop_Video = new System.Windows.Forms.Button();
			this.Button_Exit_Prog = new System.Windows.Forms.Button();
			this.Button_Freeze_Video = new System.Windows.Forms.Button();
			this.CB_Auto_Gain_Balance = new System.Windows.Forms.CheckBox();
			this.Right = new System.Windows.Forms.Button();
			this.outer_panel = new System.Windows.Forms.Panel();
			this.Status_TextBox = new System.Windows.Forms.TextBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.Up = new System.Windows.Forms.Button();
			this.Down = new System.Windows.Forms.Button();
			this.Left = new System.Windows.Forms.Button();
			this.Straight = new System.Windows.Forms.Button();
			this.Back = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.Save_Image = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.DisplayWindow)).BeginInit();
			this.outer_panel.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// DisplayWindow
			// 
			this.DisplayWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DisplayWindow.Location = new System.Drawing.Point(12, 12);
			this.DisplayWindow.Name = "DisplayWindow";
			this.DisplayWindow.Size = new System.Drawing.Size(853, 760);
			this.DisplayWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.DisplayWindow.TabIndex = 0;
			this.DisplayWindow.TabStop = false;
			// 
			// Button_Live_Video
			// 
			this.Button_Live_Video.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Button_Live_Video.Location = new System.Drawing.Point(10, 12);
			this.Button_Live_Video.Name = "Button_Live_Video";
			this.Button_Live_Video.Size = new System.Drawing.Size(117, 29);
			this.Button_Live_Video.TabIndex = 1;
			this.Button_Live_Video.Text = "Start Live";
			this.Button_Live_Video.UseVisualStyleBackColor = true;
			this.Button_Live_Video.Click += new System.EventHandler(this.Button_Live_Video_Click);
			// 
			// Button_Stop_Video
			// 
			this.Button_Stop_Video.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Button_Stop_Video.Location = new System.Drawing.Point(10, 47);
			this.Button_Stop_Video.Name = "Button_Stop_Video";
			this.Button_Stop_Video.Size = new System.Drawing.Size(117, 29);
			this.Button_Stop_Video.TabIndex = 2;
			this.Button_Stop_Video.Text = "Stop Live";
			this.Button_Stop_Video.UseVisualStyleBackColor = true;
			this.Button_Stop_Video.Click += new System.EventHandler(this.Button_Stop_Video_Click);
			// 
			// Button_Exit_Prog
			// 
			this.Button_Exit_Prog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Button_Exit_Prog.Location = new System.Drawing.Point(10, 165);
			this.Button_Exit_Prog.Name = "Button_Exit_Prog";
			this.Button_Exit_Prog.Size = new System.Drawing.Size(117, 32);
			this.Button_Exit_Prog.TabIndex = 3;
			this.Button_Exit_Prog.Text = "Exit";
			this.Button_Exit_Prog.UseVisualStyleBackColor = true;
			this.Button_Exit_Prog.Click += new System.EventHandler(this.Button_Exit_Prog_Click);
			// 
			// Button_Freeze_Video
			// 
			this.Button_Freeze_Video.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Button_Freeze_Video.Location = new System.Drawing.Point(10, 82);
			this.Button_Freeze_Video.Name = "Button_Freeze_Video";
			this.Button_Freeze_Video.Size = new System.Drawing.Size(117, 29);
			this.Button_Freeze_Video.TabIndex = 5;
			this.Button_Freeze_Video.Text = "Freeze Video";
			this.Button_Freeze_Video.UseVisualStyleBackColor = true;
			this.Button_Freeze_Video.Click += new System.EventHandler(this.Button_Freeze_Video_Click);
			// 
			// CB_Auto_Gain_Balance
			// 
			this.CB_Auto_Gain_Balance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CB_Auto_Gain_Balance.AutoSize = true;
			this.CB_Auto_Gain_Balance.Location = new System.Drawing.Point(22, 18);
			this.CB_Auto_Gain_Balance.Name = "CB_Auto_Gain_Balance";
			this.CB_Auto_Gain_Balance.Size = new System.Drawing.Size(115, 17);
			this.CB_Auto_Gain_Balance.TabIndex = 7;
			this.CB_Auto_Gain_Balance.Text = "Auto Gain Balance";
			this.CB_Auto_Gain_Balance.UseVisualStyleBackColor = true;
			this.CB_Auto_Gain_Balance.CheckedChanged += new System.EventHandler(this.Auto_Gain_Balance_CheckedChanged);
			// 
			// Right
			// 
			this.Right.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Right.Location = new System.Drawing.Point(104, 82);
			this.Right.Name = "Right";
			this.Right.Size = new System.Drawing.Size(54, 29);
			this.Right.TabIndex = 9;
			this.Right.Text = "Right";
			this.Right.UseVisualStyleBackColor = true;
			this.Right.Click += new System.EventHandler(this.Right_Click);
			// 
			// outer_panel
			// 
			this.outer_panel.AccessibleName = "";
			this.outer_panel.Controls.Add(this.Status_TextBox);
			this.outer_panel.Controls.Add(this.panel3);
			this.outer_panel.Controls.Add(this.panel2);
			this.outer_panel.Controls.Add(this.groupBox1);
			this.outer_panel.Dock = System.Windows.Forms.DockStyle.Right;
			this.outer_panel.Location = new System.Drawing.Point(871, 0);
			this.outer_panel.Name = "outer_panel";
			this.outer_panel.Size = new System.Drawing.Size(180, 784);
			this.outer_panel.TabIndex = 10;
			// 
			// Status_TextBox
			// 
			this.Status_TextBox.Location = new System.Drawing.Point(3, 455);
			this.Status_TextBox.Multiline = true;
			this.Status_TextBox.Name = "Status_TextBox";
			this.Status_TextBox.Size = new System.Drawing.Size(174, 266);
			this.Status_TextBox.TabIndex = 13;
			this.Status_TextBox.TextChanged += new System.EventHandler(this.Output_Update_TextChanged);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.CB_Auto_Gain_Balance);
			this.panel3.Location = new System.Drawing.Point(11, 242);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(158, 52);
			this.panel3.TabIndex = 11;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.Up);
			this.panel2.Controls.Add(this.Down);
			this.panel2.Controls.Add(this.Right);
			this.panel2.Controls.Add(this.Left);
			this.panel2.Controls.Add(this.Straight);
			this.panel2.Controls.Add(this.Back);
			this.panel2.Location = new System.Drawing.Point(10, 300);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(161, 149);
			this.panel2.TabIndex = 11;
			// 
			// Up
			// 
			this.Up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Up.Location = new System.Drawing.Point(86, 3);
			this.Up.Name = "Up";
			this.Up.Size = new System.Drawing.Size(54, 29);
			this.Up.TabIndex = 13;
			this.Up.Text = "Up";
			this.Up.UseVisualStyleBackColor = true;
			this.Up.Click += new System.EventHandler(this.Up_Click);
			// 
			// Down
			// 
			this.Down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Down.Location = new System.Drawing.Point(26, 3);
			this.Down.Name = "Down";
			this.Down.Size = new System.Drawing.Size(54, 29);
			this.Down.TabIndex = 14;
			this.Down.Text = "Down";
			this.Down.UseVisualStyleBackColor = true;
			this.Down.Click += new System.EventHandler(this.Down_Click);
			// 
			// Left
			// 
			this.Left.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Left.Location = new System.Drawing.Point(3, 82);
			this.Left.Name = "Left";
			this.Left.Size = new System.Drawing.Size(54, 29);
			this.Left.TabIndex = 10;
			this.Left.Text = "Left";
			this.Left.UseVisualStyleBackColor = true;
			this.Left.Click += new System.EventHandler(this.Left_Click);
			// 
			// Straight
			// 
			this.Straight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Straight.Location = new System.Drawing.Point(53, 47);
			this.Straight.Name = "Straight";
			this.Straight.Size = new System.Drawing.Size(54, 29);
			this.Straight.TabIndex = 12;
			this.Straight.Text = "Straight";
			this.Straight.UseVisualStyleBackColor = true;
			this.Straight.Click += new System.EventHandler(this.Straight_Click);
			// 
			// Back
			// 
			this.Back.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Back.Location = new System.Drawing.Point(53, 113);
			this.Back.Name = "Back";
			this.Back.Size = new System.Drawing.Size(54, 29);
			this.Back.TabIndex = 11;
			this.Back.Text = "Back";
			this.Back.UseVisualStyleBackColor = true;
			this.Back.Click += new System.EventHandler(this.Back_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.Save_Image);
			this.groupBox1.Controls.Add(this.Button_Live_Video);
			this.groupBox1.Controls.Add(this.Button_Exit_Prog);
			this.groupBox1.Controls.Add(this.Button_Stop_Video);
			this.groupBox1.Controls.Add(this.Button_Freeze_Video);
			this.groupBox1.Location = new System.Drawing.Point(22, 39);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(137, 197);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			// 
			// Save_Image
			// 
			this.Save_Image.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Save_Image.Location = new System.Drawing.Point(10, 117);
			this.Save_Image.Name = "Save_Image";
			this.Save_Image.Size = new System.Drawing.Size(117, 29);
			this.Save_Image.TabIndex = 11;
			this.Save_Image.Text = "Save Image";
			this.Save_Image.UseVisualStyleBackColor = true;
			this.Save_Image.Click += new System.EventHandler(this.Save_Image_Click);
			// 
			// Get_Panorama_Images
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1051, 784);
			this.Controls.Add(this.outer_panel);
			this.Controls.Add(this.DisplayWindow);
			this.MinimumSize = new System.Drawing.Size(649, 420);
			this.Name = "Get_Panorama_Images";
			this.Text = "uEye C# Simple Live";
			((System.ComponentModel.ISupportInitialize)(this.DisplayWindow)).EndInit();
			this.outer_panel.ResumeLayout(false);
			this.outer_panel.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox DisplayWindow;
		private System.Windows.Forms.Button Button_Live_Video;
		private System.Windows.Forms.Button Button_Stop_Video;
		private System.Windows.Forms.Button Button_Exit_Prog;
		private System.Windows.Forms.Button Button_Freeze_Video;
		private System.Windows.Forms.CheckBox CB_Auto_Gain_Balance;
		private System.Windows.Forms.Button Right;
		private System.Windows.Forms.Panel outer_panel;
		private System.Windows.Forms.Button Back;
		private System.Windows.Forms.Button Left;
		private System.Windows.Forms.Button Up;
		private System.Windows.Forms.Button Straight;
		private System.Windows.Forms.Button Down;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button Save_Image;
		private System.Windows.Forms.TextBox Status_TextBox;
	}
}

