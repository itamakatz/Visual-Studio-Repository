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
			this.Left_X = new System.Windows.Forms.Button();
			this.outer_panel = new System.Windows.Forms.Panel();
			this.panel6 = new System.Windows.Forms.Panel();
			this.Identify_X1 = new System.Windows.Forms.Button();
			this.Identify_Y = new System.Windows.Forms.Button();
			this.Identify_Z = new System.Windows.Forms.Button();
			this.panel5 = new System.Windows.Forms.Panel();
			this.Save_Image = new System.Windows.Forms.Button();
			this.panel4 = new System.Windows.Forms.Panel();
			this.Run_Relative_Move_Z = new System.Windows.Forms.Button();
			this.Z_Fine_Relative_Move_Box = new System.Windows.Forms.TextBox();
			this.Z_Axis_Label = new System.Windows.Forms.Label();
			this.Run_Relative_Move_Y = new System.Windows.Forms.Button();
			this.Y_Fine_Relative_Move_Box = new System.Windows.Forms.TextBox();
			this.Y_Axis_Label = new System.Windows.Forms.Label();
			this.Run_Relative_Move_X = new System.Windows.Forms.Button();
			this.X_Fine_Relative_Move_Box = new System.Windows.Forms.TextBox();
			this.X_Axis_Label = new System.Windows.Forms.Label();
			this.Fine_Relative_Header = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.Home_Y = new System.Windows.Forms.Button();
			this.Home_Z = new System.Windows.Forms.Button();
			this.Home_X = new System.Windows.Forms.Button();
			this.Home_All_Motors = new System.Windows.Forms.Button();
			this.Up_Z = new System.Windows.Forms.Button();
			this.Down_Z = new System.Windows.Forms.Button();
			this.Right_X = new System.Windows.Forms.Button();
			this.Forward_Y = new System.Windows.Forms.Button();
			this.Backwards_Y = new System.Windows.Forms.Button();
			this.Status_TextBox = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel7 = new System.Windows.Forms.Panel();
			this.Stop_Motors = new System.Windows.Forms.Button();
			this.Stop_Camera = new System.Windows.Forms.Button();
			this.Init_Camera = new System.Windows.Forms.Button();
			this.Init_Motors = new System.Windows.Forms.Button();
			this.panel8 = new System.Windows.Forms.Panel();
			this.XY_Current_Relative_Step_Box = new System.Windows.Forms.TextBox();
			this.Relative_Step_Tuning_Title = new System.Windows.Forms.Label();
			this.XY_Relative_Step = new System.Windows.Forms.Label();
			this.Set_XY_Relative_Step = new System.Windows.Forms.Button();
			this.Set_Z_Relative_Step = new System.Windows.Forms.Button();
			this.Z_Current_Relative_Step_Box = new System.Windows.Forms.TextBox();
			this.Z_Relative_Step = new System.Windows.Forms.Label();
			this.X_Axis_Label_2 = new System.Windows.Forms.Label();
			this.X_Current_Position = new System.Windows.Forms.Button();
			this.Y_Current_Position = new System.Windows.Forms.Button();
			this.Z_Current_Position = new System.Windows.Forms.Button();
			this.Y_Axis_Label_2 = new System.Windows.Forms.Label();
			this.Z_Axis_Label_2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.DisplayWindow)).BeginInit();
			this.outer_panel.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel7.SuspendLayout();
			this.panel8.SuspendLayout();
			this.SuspendLayout();
			// 
			// DisplayWindow
			// 
			this.DisplayWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DisplayWindow.Location = new System.Drawing.Point(12, 12);
			this.DisplayWindow.Name = "DisplayWindow";
			this.DisplayWindow.Size = new System.Drawing.Size(892, 660);
			this.DisplayWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.DisplayWindow.TabIndex = 0;
			this.DisplayWindow.TabStop = false;
			// 
			// Button_Live_Video
			// 
			this.Button_Live_Video.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Button_Live_Video.Location = new System.Drawing.Point(12, 4);
			this.Button_Live_Video.Name = "Button_Live_Video";
			this.Button_Live_Video.Size = new System.Drawing.Size(117, 29);
			this.Button_Live_Video.TabIndex = 1;
			this.Button_Live_Video.Text = "Start Live";
			this.Button_Live_Video.UseVisualStyleBackColor = true;
			this.Button_Live_Video.Click += new System.EventHandler(this.Button_Live_Video_Click);
			// 
			// Button_Stop_Video
			// 
			this.Button_Stop_Video.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Button_Stop_Video.Location = new System.Drawing.Point(12, 33);
			this.Button_Stop_Video.Name = "Button_Stop_Video";
			this.Button_Stop_Video.Size = new System.Drawing.Size(117, 29);
			this.Button_Stop_Video.TabIndex = 2;
			this.Button_Stop_Video.Text = "Stop Live";
			this.Button_Stop_Video.UseVisualStyleBackColor = true;
			this.Button_Stop_Video.Click += new System.EventHandler(this.Button_Stop_Video_Click);
			// 
			// Button_Exit_Prog
			// 
			this.Button_Exit_Prog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Button_Exit_Prog.Location = new System.Drawing.Point(12, 133);
			this.Button_Exit_Prog.Name = "Button_Exit_Prog";
			this.Button_Exit_Prog.Size = new System.Drawing.Size(117, 32);
			this.Button_Exit_Prog.TabIndex = 3;
			this.Button_Exit_Prog.Text = "Exit";
			this.Button_Exit_Prog.UseVisualStyleBackColor = true;
			this.Button_Exit_Prog.Click += new System.EventHandler(this.Button_Exit_Prog_Click);
			// 
			// Button_Freeze_Video
			// 
			this.Button_Freeze_Video.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Button_Freeze_Video.Location = new System.Drawing.Point(12, 62);
			this.Button_Freeze_Video.Name = "Button_Freeze_Video";
			this.Button_Freeze_Video.Size = new System.Drawing.Size(117, 29);
			this.Button_Freeze_Video.TabIndex = 5;
			this.Button_Freeze_Video.Text = "Freeze Video";
			this.Button_Freeze_Video.UseVisualStyleBackColor = true;
			this.Button_Freeze_Video.Click += new System.EventHandler(this.Button_Freeze_Video_Click);
			// 
			// CB_Auto_Gain_Balance
			// 
			this.CB_Auto_Gain_Balance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.CB_Auto_Gain_Balance.AutoSize = true;
			this.CB_Auto_Gain_Balance.Location = new System.Drawing.Point(25, 6);
			this.CB_Auto_Gain_Balance.Name = "CB_Auto_Gain_Balance";
			this.CB_Auto_Gain_Balance.Size = new System.Drawing.Size(115, 17);
			this.CB_Auto_Gain_Balance.TabIndex = 7;
			this.CB_Auto_Gain_Balance.Text = "Auto Gain Balance";
			this.CB_Auto_Gain_Balance.UseVisualStyleBackColor = true;
			this.CB_Auto_Gain_Balance.CheckedChanged += new System.EventHandler(this.Auto_Gain_Balance_CheckedChanged);
			// 
			// Left_X
			// 
			this.Left_X.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Left_X.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Left_X.Location = new System.Drawing.Point(70, 42);
			this.Left_X.Name = "Left_X";
			this.Left_X.Size = new System.Drawing.Size(41, 29);
			this.Left_X.TabIndex = 9;
			this.Left_X.Text = "- X";
			this.Left_X.UseVisualStyleBackColor = true;
			this.Left_X.Click += new System.EventHandler(this.Left_X_Click);
			// 
			// outer_panel
			// 
			this.outer_panel.AccessibleName = "";
			this.outer_panel.Controls.Add(this.panel6);
			this.outer_panel.Controls.Add(this.panel5);
			this.outer_panel.Controls.Add(this.panel4);
			this.outer_panel.Controls.Add(this.panel3);
			this.outer_panel.Controls.Add(this.panel2);
			this.outer_panel.Dock = System.Windows.Forms.DockStyle.Right;
			this.outer_panel.Location = new System.Drawing.Point(903, 0);
			this.outer_panel.Name = "outer_panel";
			this.outer_panel.Size = new System.Drawing.Size(183, 788);
			this.outer_panel.TabIndex = 10;
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.Z_Axis_Label_2);
			this.panel6.Controls.Add(this.Y_Axis_Label_2);
			this.panel6.Controls.Add(this.Z_Current_Position);
			this.panel6.Controls.Add(this.Y_Current_Position);
			this.panel6.Controls.Add(this.X_Current_Position);
			this.panel6.Controls.Add(this.X_Axis_Label_2);
			this.panel6.Controls.Add(this.Identify_X1);
			this.panel6.Controls.Add(this.Identify_Y);
			this.panel6.Controls.Add(this.Identify_Z);
			this.panel6.Location = new System.Drawing.Point(10, 663);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(161, 64);
			this.panel6.TabIndex = 12;
			// 
			// Identify_X1
			// 
			this.Identify_X1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Identify_X1.Location = new System.Drawing.Point(4, 18);
			this.Identify_X1.Name = "Identify_X1";
			this.Identify_X1.Size = new System.Drawing.Size(51, 21);
			this.Identify_X1.TabIndex = 2;
			this.Identify_X1.Text = "Identify";
			this.Identify_X1.UseVisualStyleBackColor = true;
			this.Identify_X1.Click += new System.EventHandler(this.Identify_X_Click);
			// 
			// Identify_Y
			// 
			this.Identify_Y.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Identify_Y.Location = new System.Drawing.Point(55, 18);
			this.Identify_Y.Name = "Identify_Y";
			this.Identify_Y.Size = new System.Drawing.Size(51, 21);
			this.Identify_Y.TabIndex = 1;
			this.Identify_Y.Text = "Identify";
			this.Identify_Y.UseVisualStyleBackColor = true;
			this.Identify_Y.Click += new System.EventHandler(this.Identify_Y_Click);
			// 
			// Identify_Z
			// 
			this.Identify_Z.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Identify_Z.Location = new System.Drawing.Point(106, 18);
			this.Identify_Z.Name = "Identify_Z";
			this.Identify_Z.Size = new System.Drawing.Size(51, 21);
			this.Identify_Z.TabIndex = 0;
			this.Identify_Z.Text = "Identify";
			this.Identify_Z.UseVisualStyleBackColor = true;
			this.Identify_Z.Click += new System.EventHandler(this.Identify_Z_Click);
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.Save_Image);
			this.panel5.Controls.Add(this.Button_Live_Video);
			this.panel5.Controls.Add(this.Button_Freeze_Video);
			this.panel5.Controls.Add(this.Button_Exit_Prog);
			this.panel5.Controls.Add(this.Button_Stop_Video);
			this.panel5.Location = new System.Drawing.Point(20, 5);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(140, 170);
			this.panel5.TabIndex = 12;
			// 
			// Save_Image
			// 
			this.Save_Image.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Save_Image.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Save_Image.Location = new System.Drawing.Point(12, 91);
			this.Save_Image.Name = "Save_Image";
			this.Save_Image.Size = new System.Drawing.Size(117, 29);
			this.Save_Image.TabIndex = 11;
			this.Save_Image.Text = "Save Image";
			this.Save_Image.UseVisualStyleBackColor = true;
			this.Save_Image.Click += new System.EventHandler(this.Save_Image_Click);
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.Run_Relative_Move_Z);
			this.panel4.Controls.Add(this.Z_Fine_Relative_Move_Box);
			this.panel4.Controls.Add(this.Z_Axis_Label);
			this.panel4.Controls.Add(this.Run_Relative_Move_Y);
			this.panel4.Controls.Add(this.Y_Fine_Relative_Move_Box);
			this.panel4.Controls.Add(this.Y_Axis_Label);
			this.panel4.Controls.Add(this.Run_Relative_Move_X);
			this.panel4.Controls.Add(this.X_Fine_Relative_Move_Box);
			this.panel4.Controls.Add(this.X_Axis_Label);
			this.panel4.Controls.Add(this.Fine_Relative_Header);
			this.panel4.Location = new System.Drawing.Point(10, 527);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(161, 132);
			this.panel4.TabIndex = 13;
			// 
			// Run_Relative_Move_Z
			// 
			this.Run_Relative_Move_Z.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Run_Relative_Move_Z.Location = new System.Drawing.Point(61, 98);
			this.Run_Relative_Move_Z.Name = "Run_Relative_Move_Z";
			this.Run_Relative_Move_Z.Size = new System.Drawing.Size(45, 20);
			this.Run_Relative_Move_Z.TabIndex = 21;
			this.Run_Relative_Move_Z.Text = "Move";
			this.Run_Relative_Move_Z.UseVisualStyleBackColor = true;
			this.Run_Relative_Move_Z.Click += new System.EventHandler(this.Run_Relative_Move_Z_Click);
			// 
			// Z_Fine_Relative_Move_Box
			// 
			this.Z_Fine_Relative_Move_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Z_Fine_Relative_Move_Box.Location = new System.Drawing.Point(24, 98);
			this.Z_Fine_Relative_Move_Box.Name = "Z_Fine_Relative_Move_Box";
			this.Z_Fine_Relative_Move_Box.Size = new System.Drawing.Size(30, 20);
			this.Z_Fine_Relative_Move_Box.TabIndex = 20;
			this.Z_Fine_Relative_Move_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Z_Axis_Label
			// 
			this.Z_Axis_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Z_Axis_Label.AutoSize = true;
			this.Z_Axis_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Z_Axis_Label.Location = new System.Drawing.Point(3, 102);
			this.Z_Axis_Label.Name = "Z_Axis_Label";
			this.Z_Axis_Label.Size = new System.Drawing.Size(19, 13);
			this.Z_Axis_Label.TabIndex = 19;
			this.Z_Axis_Label.Text = "Z:";
			this.Z_Axis_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Run_Relative_Move_Y
			// 
			this.Run_Relative_Move_Y.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Run_Relative_Move_Y.Location = new System.Drawing.Point(61, 67);
			this.Run_Relative_Move_Y.Name = "Run_Relative_Move_Y";
			this.Run_Relative_Move_Y.Size = new System.Drawing.Size(45, 20);
			this.Run_Relative_Move_Y.TabIndex = 18;
			this.Run_Relative_Move_Y.Text = "Move";
			this.Run_Relative_Move_Y.UseVisualStyleBackColor = true;
			this.Run_Relative_Move_Y.Click += new System.EventHandler(this.Run_Relative_Move_Y_Click);
			// 
			// Y_Fine_Relative_Move_Box
			// 
			this.Y_Fine_Relative_Move_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Y_Fine_Relative_Move_Box.Location = new System.Drawing.Point(24, 67);
			this.Y_Fine_Relative_Move_Box.Name = "Y_Fine_Relative_Move_Box";
			this.Y_Fine_Relative_Move_Box.Size = new System.Drawing.Size(30, 20);
			this.Y_Fine_Relative_Move_Box.TabIndex = 17;
			this.Y_Fine_Relative_Move_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Y_Axis_Label
			// 
			this.Y_Axis_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Y_Axis_Label.AutoSize = true;
			this.Y_Axis_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Y_Axis_Label.Location = new System.Drawing.Point(3, 71);
			this.Y_Axis_Label.Name = "Y_Axis_Label";
			this.Y_Axis_Label.Size = new System.Drawing.Size(19, 13);
			this.Y_Axis_Label.TabIndex = 16;
			this.Y_Axis_Label.Text = "Y:";
			this.Y_Axis_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Run_Relative_Move_X
			// 
			this.Run_Relative_Move_X.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Run_Relative_Move_X.Location = new System.Drawing.Point(61, 34);
			this.Run_Relative_Move_X.Name = "Run_Relative_Move_X";
			this.Run_Relative_Move_X.Size = new System.Drawing.Size(45, 20);
			this.Run_Relative_Move_X.TabIndex = 15;
			this.Run_Relative_Move_X.Text = "Move";
			this.Run_Relative_Move_X.UseVisualStyleBackColor = true;
			this.Run_Relative_Move_X.Click += new System.EventHandler(this.Run_Relative_Move_X_Click);
			// 
			// X_Fine_Relative_Move_Box
			// 
			this.X_Fine_Relative_Move_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.X_Fine_Relative_Move_Box.Location = new System.Drawing.Point(24, 34);
			this.X_Fine_Relative_Move_Box.Name = "X_Fine_Relative_Move_Box";
			this.X_Fine_Relative_Move_Box.Size = new System.Drawing.Size(30, 20);
			this.X_Fine_Relative_Move_Box.TabIndex = 14;
			this.X_Fine_Relative_Move_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// X_Axis_Label
			// 
			this.X_Axis_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.X_Axis_Label.AutoSize = true;
			this.X_Axis_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.X_Axis_Label.Location = new System.Drawing.Point(3, 38);
			this.X_Axis_Label.Name = "X_Axis_Label";
			this.X_Axis_Label.Size = new System.Drawing.Size(19, 13);
			this.X_Axis_Label.TabIndex = 13;
			this.X_Axis_Label.Text = "X:";
			this.X_Axis_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Fine_Relative_Header
			// 
			this.Fine_Relative_Header.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Fine_Relative_Header.AutoSize = true;
			this.Fine_Relative_Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Fine_Relative_Header.Location = new System.Drawing.Point(5, 11);
			this.Fine_Relative_Header.Name = "Fine_Relative_Header";
			this.Fine_Relative_Header.Size = new System.Drawing.Size(150, 13);
			this.Fine_Relative_Header.TabIndex = 12;
			this.Fine_Relative_Header.Text = "Fine Relative Movements";
			this.Fine_Relative_Header.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.CB_Auto_Gain_Balance);
			this.panel3.Location = new System.Drawing.Point(10, 750);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(161, 29);
			this.panel3.TabIndex = 11;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.Home_Y);
			this.panel2.Controls.Add(this.Home_Z);
			this.panel2.Controls.Add(this.Home_X);
			this.panel2.Controls.Add(this.Home_All_Motors);
			this.panel2.Controls.Add(this.Up_Z);
			this.panel2.Controls.Add(this.Down_Z);
			this.panel2.Controls.Add(this.Left_X);
			this.panel2.Controls.Add(this.Right_X);
			this.panel2.Controls.Add(this.Forward_Y);
			this.panel2.Controls.Add(this.Backwards_Y);
			this.panel2.Location = new System.Drawing.Point(10, 288);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(161, 153);
			this.panel2.TabIndex = 11;
			// 
			// Home_Y
			// 
			this.Home_Y.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Home_Y.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Home_Y.Location = new System.Drawing.Point(8, 77);
			this.Home_Y.Name = "Home_Y";
			this.Home_Y.Size = new System.Drawing.Size(62, 29);
			this.Home_Y.TabIndex = 18;
			this.Home_Y.Text = "Home Y";
			this.Home_Y.UseVisualStyleBackColor = true;
			this.Home_Y.Click += new System.EventHandler(this.Home_Y_Click);
			// 
			// Home_Z
			// 
			this.Home_Z.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Home_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Home_Z.Location = new System.Drawing.Point(8, 112);
			this.Home_Z.Name = "Home_Z";
			this.Home_Z.Size = new System.Drawing.Size(62, 29);
			this.Home_Z.TabIndex = 17;
			this.Home_Z.Text = "Home Z";
			this.Home_Z.UseVisualStyleBackColor = true;
			this.Home_Z.Click += new System.EventHandler(this.Home_Z_Click);
			// 
			// Home_X
			// 
			this.Home_X.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Home_X.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Home_X.Location = new System.Drawing.Point(8, 42);
			this.Home_X.Name = "Home_X";
			this.Home_X.Size = new System.Drawing.Size(62, 29);
			this.Home_X.TabIndex = 16;
			this.Home_X.Text = "Home X";
			this.Home_X.UseVisualStyleBackColor = true;
			this.Home_X.Click += new System.EventHandler(this.Home_X_Click);
			// 
			// Home_All_Motors
			// 
			this.Home_All_Motors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Home_All_Motors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Home_All_Motors.Location = new System.Drawing.Point(6, 8);
			this.Home_All_Motors.Name = "Home_All_Motors";
			this.Home_All_Motors.Size = new System.Drawing.Size(144, 28);
			this.Home_All_Motors.TabIndex = 15;
			this.Home_All_Motors.Text = "Home All";
			this.Home_All_Motors.UseVisualStyleBackColor = true;
			this.Home_All_Motors.Click += new System.EventHandler(this.Home_Motors_Click);
			// 
			// Up_Z
			// 
			this.Up_Z.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Up_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Up_Z.Location = new System.Drawing.Point(111, 112);
			this.Up_Z.Name = "Up_Z";
			this.Up_Z.Size = new System.Drawing.Size(41, 29);
			this.Up_Z.TabIndex = 13;
			this.Up_Z.Text = "+ Z";
			this.Up_Z.UseVisualStyleBackColor = true;
			this.Up_Z.Click += new System.EventHandler(this.Up_Z_Click);
			// 
			// Down_Z
			// 
			this.Down_Z.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Down_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Down_Z.Location = new System.Drawing.Point(70, 112);
			this.Down_Z.Name = "Down_Z";
			this.Down_Z.Size = new System.Drawing.Size(41, 29);
			this.Down_Z.TabIndex = 14;
			this.Down_Z.Text = "- Z";
			this.Down_Z.UseVisualStyleBackColor = true;
			this.Down_Z.Click += new System.EventHandler(this.Down_Z_Click);
			// 
			// Right_X
			// 
			this.Right_X.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Right_X.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Right_X.Location = new System.Drawing.Point(111, 42);
			this.Right_X.Name = "Right_X";
			this.Right_X.Size = new System.Drawing.Size(41, 29);
			this.Right_X.TabIndex = 10;
			this.Right_X.Text = "+ X";
			this.Right_X.UseVisualStyleBackColor = true;
			this.Right_X.Click += new System.EventHandler(this.Right_X_Click);
			// 
			// Forward_Y
			// 
			this.Forward_Y.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Forward_Y.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Forward_Y.Location = new System.Drawing.Point(111, 77);
			this.Forward_Y.Name = "Forward_Y";
			this.Forward_Y.Size = new System.Drawing.Size(41, 29);
			this.Forward_Y.TabIndex = 12;
			this.Forward_Y.Text = "+ Y";
			this.Forward_Y.UseVisualStyleBackColor = true;
			this.Forward_Y.Click += new System.EventHandler(this.Forward_Y_Click);
			// 
			// Backwards_Y
			// 
			this.Backwards_Y.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Backwards_Y.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Backwards_Y.Location = new System.Drawing.Point(70, 77);
			this.Backwards_Y.Name = "Backwards_Y";
			this.Backwards_Y.Size = new System.Drawing.Size(41, 29);
			this.Backwards_Y.TabIndex = 11;
			this.Backwards_Y.Text = "- Y";
			this.Backwards_Y.UseVisualStyleBackColor = true;
			this.Backwards_Y.Click += new System.EventHandler(this.Backwards_Y_Click);
			// 
			// Status_TextBox
			// 
			this.Status_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Status_TextBox.Location = new System.Drawing.Point(16, 8);
			this.Status_TextBox.Multiline = true;
			this.Status_TextBox.Name = "Status_TextBox";
			this.Status_TextBox.ReadOnly = true;
			this.Status_TextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Status_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.Status_TextBox.Size = new System.Drawing.Size(859, 95);
			this.Status_TextBox.TabIndex = 13;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.Status_TextBox);
			this.panel1.Location = new System.Drawing.Point(12, 678);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(891, 110);
			this.panel1.TabIndex = 11;
			// 
			// panel7
			// 
			this.panel7.Controls.Add(this.Stop_Motors);
			this.panel7.Controls.Add(this.Stop_Camera);
			this.panel7.Controls.Add(this.Init_Camera);
			this.panel7.Controls.Add(this.Init_Motors);
			this.panel7.Location = new System.Drawing.Point(906, 220);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(180, 61);
			this.panel7.TabIndex = 12;
			// 
			// Stop_Motors
			// 
			this.Stop_Motors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Stop_Motors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Stop_Motors.Location = new System.Drawing.Point(93, 33);
			this.Stop_Motors.Name = "Stop_Motors";
			this.Stop_Motors.Size = new System.Drawing.Size(80, 23);
			this.Stop_Motors.TabIndex = 13;
			this.Stop_Motors.Text = "Stop Motors";
			this.Stop_Motors.UseVisualStyleBackColor = true;
			this.Stop_Motors.Click += new System.EventHandler(this.Stop_Motors_Click);
			// 
			// Stop_Camera
			// 
			this.Stop_Camera.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Stop_Camera.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Stop_Camera.Location = new System.Drawing.Point(8, 33);
			this.Stop_Camera.Name = "Stop_Camera";
			this.Stop_Camera.Size = new System.Drawing.Size(80, 23);
			this.Stop_Camera.TabIndex = 14;
			this.Stop_Camera.Text = "Stop Camera";
			this.Stop_Camera.UseVisualStyleBackColor = true;
			this.Stop_Camera.Click += new System.EventHandler(this.Stop_Camera_Click);
			// 
			// Init_Camera
			// 
			this.Init_Camera.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Init_Camera.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Init_Camera.Location = new System.Drawing.Point(8, 5);
			this.Init_Camera.Name = "Init_Camera";
			this.Init_Camera.Size = new System.Drawing.Size(80, 23);
			this.Init_Camera.TabIndex = 1;
			this.Init_Camera.Text = "Init Camera";
			this.Init_Camera.UseVisualStyleBackColor = true;
			this.Init_Camera.Click += new System.EventHandler(this.Init_Camera_Click);
			// 
			// Init_Motors
			// 
			this.Init_Motors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Init_Motors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Init_Motors.Location = new System.Drawing.Point(93, 5);
			this.Init_Motors.Name = "Init_Motors";
			this.Init_Motors.Size = new System.Drawing.Size(80, 23);
			this.Init_Motors.TabIndex = 0;
			this.Init_Motors.Text = "Init Motors";
			this.Init_Motors.UseVisualStyleBackColor = true;
			this.Init_Motors.Click += new System.EventHandler(this.Init_Motors_Click);
			// 
			// panel8
			// 
			this.panel8.Controls.Add(this.Set_Z_Relative_Step);
			this.panel8.Controls.Add(this.Set_XY_Relative_Step);
			this.panel8.Controls.Add(this.Z_Current_Relative_Step_Box);
			this.panel8.Controls.Add(this.Z_Relative_Step);
			this.panel8.Controls.Add(this.XY_Current_Relative_Step_Box);
			this.panel8.Controls.Add(this.Relative_Step_Tuning_Title);
			this.panel8.Controls.Add(this.XY_Relative_Step);
			this.panel8.Location = new System.Drawing.Point(913, 445);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(161, 77);
			this.panel8.TabIndex = 13;
			// 
			// XY_Current_Relative_Step_Box
			// 
			this.XY_Current_Relative_Step_Box.Location = new System.Drawing.Point(72, 28);
			this.XY_Current_Relative_Step_Box.Name = "XY_Current_Relative_Step_Box";
			this.XY_Current_Relative_Step_Box.Size = new System.Drawing.Size(30, 20);
			this.XY_Current_Relative_Step_Box.TabIndex = 4;
			this.XY_Current_Relative_Step_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Relative_Step_Tuning_Title
			// 
			this.Relative_Step_Tuning_Title.AutoSize = true;
			this.Relative_Step_Tuning_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Relative_Step_Tuning_Title.Location = new System.Drawing.Point(17, 8);
			this.Relative_Step_Tuning_Title.Name = "Relative_Step_Tuning_Title";
			this.Relative_Step_Tuning_Title.Size = new System.Drawing.Size(127, 13);
			this.Relative_Step_Tuning_Title.TabIndex = 2;
			this.Relative_Step_Tuning_Title.Text = "Relative Step Tuning";
			// 
			// XY_Relative_Step
			// 
			this.XY_Relative_Step.AutoSize = true;
			this.XY_Relative_Step.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.XY_Relative_Step.Location = new System.Drawing.Point(13, 32);
			this.XY_Relative_Step.Name = "XY_Relative_Step";
			this.XY_Relative_Step.Size = new System.Drawing.Size(57, 13);
			this.XY_Relative_Step.TabIndex = 0;
			this.XY_Relative_Step.Text = "XY Step:";
			// 
			// Set_XY_Relative_Step
			// 
			this.Set_XY_Relative_Step.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Set_XY_Relative_Step.Location = new System.Drawing.Point(110, 28);
			this.Set_XY_Relative_Step.Name = "Set_XY_Relative_Step";
			this.Set_XY_Relative_Step.Size = new System.Drawing.Size(37, 20);
			this.Set_XY_Relative_Step.TabIndex = 22;
			this.Set_XY_Relative_Step.Text = "Set";
			this.Set_XY_Relative_Step.UseVisualStyleBackColor = true;
			this.Set_XY_Relative_Step.Click += new System.EventHandler(this.Set_XY_Relative_Step_Click);
			// 
			// Set_Z_Relative_Step
			// 
			this.Set_Z_Relative_Step.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Set_Z_Relative_Step.Location = new System.Drawing.Point(110, 49);
			this.Set_Z_Relative_Step.Name = "Set_Z_Relative_Step";
			this.Set_Z_Relative_Step.Size = new System.Drawing.Size(37, 20);
			this.Set_Z_Relative_Step.TabIndex = 24;
			this.Set_Z_Relative_Step.Text = "Set";
			this.Set_Z_Relative_Step.UseVisualStyleBackColor = true;
			this.Set_Z_Relative_Step.Click += new System.EventHandler(this.Set_Z_Relative_Step_Click);
			// 
			// Z_Current_Relative_Step_Box
			// 
			this.Z_Current_Relative_Step_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Z_Current_Relative_Step_Box.Location = new System.Drawing.Point(72, 49);
			this.Z_Current_Relative_Step_Box.Name = "Z_Current_Relative_Step_Box";
			this.Z_Current_Relative_Step_Box.Size = new System.Drawing.Size(30, 20);
			this.Z_Current_Relative_Step_Box.TabIndex = 23;
			this.Z_Current_Relative_Step_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Z_Relative_Step
			// 
			this.Z_Relative_Step.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Z_Relative_Step.AutoSize = true;
			this.Z_Relative_Step.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Z_Relative_Step.Location = new System.Drawing.Point(13, 53);
			this.Z_Relative_Step.Name = "Z_Relative_Step";
			this.Z_Relative_Step.Size = new System.Drawing.Size(49, 13);
			this.Z_Relative_Step.TabIndex = 22;
			this.Z_Relative_Step.Text = "Z Step:";
			this.Z_Relative_Step.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// X_Axis_Label_2
			// 
			this.X_Axis_Label_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.X_Axis_Label_2.AutoSize = true;
			this.X_Axis_Label_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.X_Axis_Label_2.Location = new System.Drawing.Point(6, 3);
			this.X_Axis_Label_2.Name = "X_Axis_Label_2";
			this.X_Axis_Label_2.Size = new System.Drawing.Size(46, 13);
			this.X_Axis_Label_2.TabIndex = 22;
			this.X_Axis_Label_2.Text = "X Axis:";
			this.X_Axis_Label_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// X_Current_Position
			// 
			this.X_Current_Position.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.X_Current_Position.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.X_Current_Position.Location = new System.Drawing.Point(4, 40);
			this.X_Current_Position.Name = "X_Current_Position";
			this.X_Current_Position.Size = new System.Drawing.Size(51, 21);
			this.X_Current_Position.TabIndex = 23;
			this.X_Current_Position.Text = "Pos";
			this.X_Current_Position.UseVisualStyleBackColor = true;
			this.X_Current_Position.Click += new System.EventHandler(this.X_Current_Position_Click);
			// 
			// Y_Current_Position
			// 
			this.Y_Current_Position.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Y_Current_Position.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Y_Current_Position.Location = new System.Drawing.Point(55, 40);
			this.Y_Current_Position.Name = "Y_Current_Position";
			this.Y_Current_Position.Size = new System.Drawing.Size(51, 21);
			this.Y_Current_Position.TabIndex = 24;
			this.Y_Current_Position.Text = "Pos";
			this.Y_Current_Position.UseVisualStyleBackColor = true;
			this.Y_Current_Position.Click += new System.EventHandler(this.Y_Current_Position_Click);
			// 
			// Z_Current_Position
			// 
			this.Z_Current_Position.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Z_Current_Position.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Z_Current_Position.Location = new System.Drawing.Point(106, 40);
			this.Z_Current_Position.Name = "Z_Current_Position";
			this.Z_Current_Position.Size = new System.Drawing.Size(51, 21);
			this.Z_Current_Position.TabIndex = 25;
			this.Z_Current_Position.Text = "Pos";
			this.Z_Current_Position.UseVisualStyleBackColor = true;
			this.Z_Current_Position.Click += new System.EventHandler(this.Z_Current_Position_Click);
			// 
			// Y_Axis_Label_2
			// 
			this.Y_Axis_Label_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Y_Axis_Label_2.AutoSize = true;
			this.Y_Axis_Label_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Y_Axis_Label_2.Location = new System.Drawing.Point(58, 3);
			this.Y_Axis_Label_2.Name = "Y_Axis_Label_2";
			this.Y_Axis_Label_2.Size = new System.Drawing.Size(46, 13);
			this.Y_Axis_Label_2.TabIndex = 26;
			this.Y_Axis_Label_2.Text = "Y Axis:";
			this.Y_Axis_Label_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Z_Axis_Label_2
			// 
			this.Z_Axis_Label_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Z_Axis_Label_2.AutoSize = true;
			this.Z_Axis_Label_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Z_Axis_Label_2.Location = new System.Drawing.Point(110, 3);
			this.Z_Axis_Label_2.Name = "Z_Axis_Label_2";
			this.Z_Axis_Label_2.Size = new System.Drawing.Size(46, 13);
			this.Z_Axis_Label_2.TabIndex = 27;
			this.Z_Axis_Label_2.Text = "Z Axis:";
			this.Z_Axis_Label_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Get_Panorama_Images
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1086, 788);
			this.Controls.Add(this.panel8);
			this.Controls.Add(this.panel7);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.outer_panel);
			this.Controls.Add(this.DisplayWindow);
			this.MinimumSize = new System.Drawing.Size(649, 420);
			this.Name = "Get_Panorama_Images";
			this.Text = "uEye C# Simple Live";
			((System.ComponentModel.ISupportInitialize)(this.DisplayWindow)).EndInit();
			this.outer_panel.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.panel6.PerformLayout();
			this.panel5.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel7.ResumeLayout(false);
			this.panel8.ResumeLayout(false);
			this.panel8.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox DisplayWindow;
		private System.Windows.Forms.Button Button_Live_Video;
		private System.Windows.Forms.Button Button_Stop_Video;
		private System.Windows.Forms.Button Button_Exit_Prog;
		private System.Windows.Forms.Button Button_Freeze_Video;
		private System.Windows.Forms.CheckBox CB_Auto_Gain_Balance;
		private System.Windows.Forms.Button Left_X;
		private System.Windows.Forms.Panel outer_panel;
		private System.Windows.Forms.Button Backwards_Y;
		private System.Windows.Forms.Button Right_X;
		private System.Windows.Forms.Button Up_Z;
		private System.Windows.Forms.Button Forward_Y;
		private System.Windows.Forms.Button Down_Z;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button Save_Image;
		private System.Windows.Forms.TextBox Status_TextBox;
		private System.Windows.Forms.Button Home_All_Motors;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label Fine_Relative_Header;
		private System.Windows.Forms.Label X_Axis_Label;
		private System.Windows.Forms.TextBox X_Fine_Relative_Move_Box;
		private System.Windows.Forms.Button Run_Relative_Move_X;
		private System.Windows.Forms.Button Run_Relative_Move_Y;
		private System.Windows.Forms.TextBox Y_Fine_Relative_Move_Box;
		private System.Windows.Forms.Label Y_Axis_Label;
		private System.Windows.Forms.Button Run_Relative_Move_Z;
		private System.Windows.Forms.TextBox Z_Fine_Relative_Move_Box;
		private System.Windows.Forms.Label Z_Axis_Label;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Button Identify_X1;
		private System.Windows.Forms.Button Identify_Y;
		private System.Windows.Forms.Button Identify_Z;
		private System.Windows.Forms.Button Home_Y;
		private System.Windows.Forms.Button Home_Z;
		private System.Windows.Forms.Button Home_X;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Button Init_Motors;
		private System.Windows.Forms.Button Init_Camera;
		private System.Windows.Forms.Button Stop_Motors;
		private System.Windows.Forms.Button Stop_Camera;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Label XY_Relative_Step;
		private System.Windows.Forms.Label Relative_Step_Tuning_Title;
		private System.Windows.Forms.TextBox XY_Current_Relative_Step_Box;
		private System.Windows.Forms.Button Set_Z_Relative_Step;
		private System.Windows.Forms.Button Set_XY_Relative_Step;
		private System.Windows.Forms.TextBox Z_Current_Relative_Step_Box;
		private System.Windows.Forms.Label Z_Relative_Step;
		private System.Windows.Forms.Label X_Axis_Label_2;
		private System.Windows.Forms.Button X_Current_Position;
		private System.Windows.Forms.Button Z_Current_Position;
		private System.Windows.Forms.Button Y_Current_Position;
		private System.Windows.Forms.Label Z_Axis_Label_2;
		private System.Windows.Forms.Label Y_Axis_Label_2;
	}
}

