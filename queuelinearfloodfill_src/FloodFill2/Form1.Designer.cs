namespace FloodFill2
{
	partial class Form1
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.comboFillType = new System.Windows.Forms.ComboBox();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabelMousePos = new System.Windows.Forms.ToolStripStatusLabel();
			this.groupBoxTolerance = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBoxFillColor = new System.Windows.Forms.GroupBox();
			this.labelColorPreview = new System.Windows.Forms.Label();
			// this.comboNamedColor = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.labelFillR = new System.Windows.Forms.Label();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnOpen = new System.Windows.Forms.ToolStripButton();
			this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnSlow = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.btnStop = new System.Windows.Forms.ToolStripButton();
			// this.sliderPickerTolB = new FloodFill2.TextBoxSlider();
			// this.sliderPickerTolG = new FloodFill2.TextBoxSlider();
			// this.sliderPickerTolR = new FloodFill2.TextBoxSlider();
			// this.sliderPickerB = new FloodFill2.TextBoxSlider();
			// this.sliderPickerG = new FloodFill2.TextBoxSlider();
			// this.sliderPickerR = new FloodFill2.TextBoxSlider();
			// this.panel = new PictureBoxScroll.PicturePanel();
			this.statusStrip.SuspendLayout();
			this.groupBoxTolerance.SuspendLayout();
			this.groupBoxFillColor.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboFillType
			// 
			this.comboFillType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFillType.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.comboFillType.FormattingEnabled = true;
			this.comboFillType.Location = new System.Drawing.Point(13, 28);
			this.comboFillType.Name = "comboFillType";
			this.comboFillType.Size = new System.Drawing.Size(187, 21);
			this.comboFillType.TabIndex = 0;
			this.comboFillType.SelectedIndexChanged += new System.EventHandler(this.comboFillTypeSelectedIndexChanged);
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripStatusLabel1,
			this.toolStripStatusLabelMousePos});
			this.statusStrip.Location = new System.Drawing.Point(0, 390);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(543, 22);
			this.statusStrip.TabIndex = 6;
			this.statusStrip.Text = "statusBar";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// toolStripStatusLabelMousePos
			// 
			this.toolStripStatusLabelMousePos.Name = "toolStripStatusLabelMousePos";
			this.toolStripStatusLabelMousePos.Size = new System.Drawing.Size(105, 17);
			this.toolStripStatusLabelMousePos.Text = "Mouse position: --,--";
			// 
			// groupBoxTolerance
			// 
			this.groupBoxTolerance.Controls.Add(this.label3);
			// this.groupBoxTolerance.Controls.Add(this.sliderPickerTolB);
			// this.groupBoxTolerance.Controls.Add(this.sliderPickerTolG);
			// this.groupBoxTolerance.Controls.Add(this.sliderPickerTolR);
			this.groupBoxTolerance.Controls.Add(this.label4);
			this.groupBoxTolerance.Controls.Add(this.label5);
			this.groupBoxTolerance.Location = new System.Drawing.Point(12, 227);
			this.groupBoxTolerance.Name = "groupBoxTolerance";
			this.groupBoxTolerance.Size = new System.Drawing.Size(187, 148);
			this.groupBoxTolerance.TabIndex = 9;
			this.groupBoxTolerance.TabStop = false;
			this.groupBoxTolerance.Text = "Tolerance";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(10, 98);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(14, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "B";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(9, 62);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(15, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "G";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(9, 27);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(15, 13);
			this.label5.TabIndex = 1;
			this.label5.Text = "R";
			// 
			// groupBoxFillColor
			// 
			// this.groupBoxFillColor.Controls.Add(this.sliderPickerB);
			// this.groupBoxFillColor.Controls.Add(this.sliderPickerG);
			// this.groupBoxFillColor.Controls.Add(this.sliderPickerR);
			this.groupBoxFillColor.Controls.Add(this.labelColorPreview);
			// this.groupBoxFillColor.Controls.Add(this.comboNamedColor);
			this.groupBoxFillColor.Controls.Add(this.label2);
			this.groupBoxFillColor.Controls.Add(this.label1);
			this.groupBoxFillColor.Controls.Add(this.labelFillR);
			this.groupBoxFillColor.Location = new System.Drawing.Point(12, 55);
			this.groupBoxFillColor.Name = "groupBoxFillColor";
			this.groupBoxFillColor.Size = new System.Drawing.Size(187, 161);
			this.groupBoxFillColor.TabIndex = 8;
			this.groupBoxFillColor.TabStop = false;
			this.groupBoxFillColor.Text = "Fill Color";
			// 
			// labelColorPreview
			// 
			this.labelColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelColorPreview.Location = new System.Drawing.Point(158, 20);
			this.labelColorPreview.Name = "labelColorPreview";
			this.labelColorPreview.Size = new System.Drawing.Size(20, 20);
			this.labelColorPreview.TabIndex = 7;
			// 
			// comboNamedColor
			// 
			// this.comboNamedColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			// this.comboNamedColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			// this.comboNamedColor.FormattingEnabled = true;
			// this.comboNamedColor.Location = new System.Drawing.Point(7, 20);
			// this.comboNamedColor.Name = "comboNamedColor";
			// this.comboNamedColor.Size = new System.Drawing.Size(145, 21);
			// this.comboNamedColor.TabIndex = 1;
			// this.comboNamedColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboNamedColorDrawItem);
			// this.comboNamedColor.SelectedIndexChanged += new System.EventHandler(this.comboNamedColorSelectedIndexChanged);
			// // 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 121);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(14, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "B";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 86);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(15, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "G";
			// 
			// labelFillR
			// 
			this.labelFillR.AutoSize = true;
			this.labelFillR.Location = new System.Drawing.Point(8, 51);
			this.labelFillR.Name = "labelFillR";
			this.labelFillR.Size = new System.Drawing.Size(15, 13);
			this.labelFillR.TabIndex = 1;
			this.labelFillR.Text = "R";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.btnOpen,
			this.btnSaveAs,
			this.toolStripSeparator1,
			this.btnSlow,
			this.toolStripSeparator2,
			this.btnStop});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(543, 25);
			this.toolStrip1.TabIndex = 10;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnOpen
			// 
			this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOpen.Image = global::FloodFill2.Properties.Resources.OpenFolder;
			this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(23, 22);
			this.btnOpen.Text = "Open...";
			this.btnOpen.Click += new System.EventHandler(this.openBtn_Click);
			// 
			// btnSaveAs
			// 
			this.btnSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSaveAs.Image = global::FloodFill2.Properties.Resources.Save;
			this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSaveAs.Name = "btnSaveAs";
			this.btnSaveAs.Size = new System.Drawing.Size(23, 22);
			this.btnSaveAs.Text = "Save As...";
			this.btnSaveAs.Click += new System.EventHandler(this.saveBtn_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnSlow
			// 
			this.btnSlow.CheckOnClick = true;
			this.btnSlow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSlow.Image = global::FloodFill2.Properties.Resources.snail;
			this.btnSlow.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSlow.Name = "btnSlow";
			this.btnSlow.Size = new System.Drawing.Size(23, 22);
			this.btnSlow.Text = "Slow Speed (toggle)";
			this.btnSlow.CheckedChanged += new System.EventHandler(this.btnSlow_CheckedChanged);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// btnStop
			// 
			this.btnStop.Enabled = false;
			this.btnStop.Image = global::FloodFill2.Properties.Resources.NoAction;
			this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(64, 22);
			this.btnStop.Text = "Stop Fill";
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// sliderPickerTolB
			// 
			// this.sliderPickerTolB.DecimalPlaces = 0;
			// this.sliderPickerTolB.Location = new System.Drawing.Point(29, 90);
			// this.sliderPickerTolB.Maximum = 255F;
			// this.sliderPickerTolB.Name = "sliderPickerTolB";
			// this.sliderPickerTolB.Size = new System.Drawing.Size(152, 46);
			// this.sliderPickerTolB.TabIndex = 8;
			// this.sliderPickerTolB.TickFrequency = 15F;
			// this.sliderPickerTolB.ValueChanged += new System.EventHandler(this.sliderPickerTolB_ValueChanged);
			// // 
			// // sliderPickerTolG
			// // 
			// this.sliderPickerTolG.DecimalPlaces = 0;
			// this.sliderPickerTolG.Location = new System.Drawing.Point(29, 54);
			// this.sliderPickerTolG.Maximum = 255F;
			// this.sliderPickerTolG.Name = "sliderPickerTolG";
			// this.sliderPickerTolG.Size = new System.Drawing.Size(152, 46);
			// this.sliderPickerTolG.TabIndex = 7;
			// this.sliderPickerTolG.TickFrequency = 15F;
			// this.sliderPickerTolG.ValueChanged += new System.EventHandler(this.sliderPickerTolG_ValueChanged);
			// // 
			// // sliderPickerTolR
			// // 
			// this.sliderPickerTolR.DecimalPlaces = 0;
			// this.sliderPickerTolR.Location = new System.Drawing.Point(29, 19);
			// this.sliderPickerTolR.Maximum = 255F;
			// this.sliderPickerTolR.Name = "sliderPickerTolR";
			// this.sliderPickerTolR.Size = new System.Drawing.Size(152, 46);
			// this.sliderPickerTolR.TabIndex = 6;
			// this.sliderPickerTolR.TickFrequency = 15F;
			// this.sliderPickerTolR.ValueChanged += new System.EventHandler(this.sliderPickerTolR_ValueChanged);
			// // 
			// // sliderPickerB
			// // 
			// this.sliderPickerB.DecimalPlaces = 0;
			// this.sliderPickerB.Location = new System.Drawing.Point(29, 112);
			// this.sliderPickerB.Maximum = 255F;
			// this.sliderPickerB.Name = "sliderPickerB";
			// this.sliderPickerB.Size = new System.Drawing.Size(149, 46);
			// this.sliderPickerB.TabIndex = 5;
			// this.sliderPickerB.TickFrequency = 15F;
			// this.sliderPickerB.ValueChanged += new System.EventHandler(this.sliderPickerColor_ValueChanged);
			// // 
			// // sliderPickerG
			// // 
			// this.sliderPickerG.DecimalPlaces = 0;
			// this.sliderPickerG.Location = new System.Drawing.Point(29, 77);
			// this.sliderPickerG.Maximum = 255F;
			// this.sliderPickerG.Name = "sliderPickerG";
			// this.sliderPickerG.Size = new System.Drawing.Size(149, 46);
			// this.sliderPickerG.TabIndex = 3;
			// this.sliderPickerG.TickFrequency = 15F;
			// this.sliderPickerG.ValueChanged += new System.EventHandler(this.sliderPickerColor_ValueChanged);
			// // 
			// // sliderPickerR
			// // 
			// this.sliderPickerR.DecimalPlaces = 0;
			// this.sliderPickerR.Location = new System.Drawing.Point(29, 42);
			// this.sliderPickerR.Maximum = 255F;
			// this.sliderPickerR.Name = "sliderPickerR";
			// this.sliderPickerR.Size = new System.Drawing.Size(149, 46);
			// this.sliderPickerR.TabIndex = 2;
			// this.sliderPickerR.TickFrequency = 15F;
			// this.sliderPickerR.ValueChanged += new System.EventHandler(this.sliderPickerColor_ValueChanged);
			// 
			// panel
			// // 
			// this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			// 			| System.Windows.Forms.AnchorStyles.Left)
			// 			| System.Windows.Forms.AnchorStyles.Right)));
			// this.panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			// this.panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			// this.panel.Image = null;
			// this.panel.Location = new System.Drawing.Point(206, 28);
			// this.panel.Name = "panel";
			// this.panel.Size = new System.Drawing.Size(325, 347);
			// this.panel.TabIndex = 1;
			// this.panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelMouseDown);
			// this.panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelMouseMove);
			// this.panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelMouseUp);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(543, 412);
			this.Controls.Add(this.groupBoxTolerance);
			this.Controls.Add(this.groupBoxFillColor);
			this.Controls.Add(this.comboFillType);
			// this.Controls.Add(this.panel);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.toolStrip1);
			this.MinimumSize = new System.Drawing.Size(551, 395);
			this.Name = "Form1";
			this.Text = "Flood Fill Demo";
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.groupBoxTolerance.ResumeLayout(false);
			this.groupBoxTolerance.PerformLayout();
			this.groupBoxFillColor.ResumeLayout(false);
			this.groupBoxFillColor.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		// private PictureBoxScroll.PicturePanel panel;
		private System.Windows.Forms.ComboBox comboFillType;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMousePos;
		private System.Windows.Forms.GroupBox groupBoxTolerance;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBoxFillColor;
		private System.Windows.Forms.Label labelColorPreview;
		// private System.Windows.Forms.ComboBox comboNamedColor;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelFillR;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStripButton btnSaveAs;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnSlow;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton btnStop;
		// private TextBoxSlider sliderPickerR;
		// private TextBoxSlider sliderPickerG;
		// private TextBoxSlider sliderPickerB;
		// private TextBoxSlider sliderPickerTolR;
		// private TextBoxSlider sliderPickerTolG;
		// private TextBoxSlider sliderPickerTolB;
	}
}

