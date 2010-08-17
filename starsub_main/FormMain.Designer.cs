namespace starsub
{
	partial class FormMain
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openAudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openSubtitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.controlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.playPauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.OpenAudioDialog = new System.Windows.Forms.OpenFileDialog();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.panel1 = new System.Windows.Forms.Panel();
			this.DialogTextBox = new System.Windows.Forms.TextBox();
			this.StartTimeBox = new System.Windows.Forms.MaskedTextBox();
			this.EndTimeBox = new System.Windows.Forms.MaskedTextBox();
			this.OpenSubDialog = new System.Windows.Forms.OpenFileDialog();
			this.writeStartTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.writeEndTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.writeLastEndTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.audioPanel1 = new starsub.AudioPanel();
			this.menuStrip1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.controlToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
			this.menuStrip1.Size = new System.Drawing.Size(692, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openAudioToolStripMenuItem,
            this.openSubtitleToolStripMenuItem,
            this.toolStripSeparator1});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(36, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openAudioToolStripMenuItem
			// 
			this.openAudioToolStripMenuItem.Name = "openAudioToolStripMenuItem";
			this.openAudioToolStripMenuItem.ShortcutKeyDisplayString = "";
			this.openAudioToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openAudioToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.openAudioToolStripMenuItem.Text = "Open &Audio...";
			this.openAudioToolStripMenuItem.Click += new System.EventHandler(this.openAudioToolStripMenuItem_Click);
			// 
			// openSubtitleToolStripMenuItem
			// 
			this.openSubtitleToolStripMenuItem.Name = "openSubtitleToolStripMenuItem";
			this.openSubtitleToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.openSubtitleToolStripMenuItem.Text = "Open &Subtitle...";
			this.openSubtitleToolStripMenuItem.Click += new System.EventHandler(this.openSubtitleToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(190, 6);
			// 
			// controlToolStripMenuItem
			// 
			this.controlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playPauseToolStripMenuItem,
            this.toolStripSeparator2,
            this.writeStartTimeToolStripMenuItem,
            this.writeEndTimeToolStripMenuItem,
            this.writeLastEndTimeToolStripMenuItem});
			this.controlToolStripMenuItem.Name = "controlToolStripMenuItem";
			this.controlToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
			this.controlToolStripMenuItem.Text = "&Control";
			// 
			// playPauseToolStripMenuItem
			// 
			this.playPauseToolStripMenuItem.Name = "playPauseToolStripMenuItem";
			this.playPauseToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
			this.playPauseToolStripMenuItem.Text = "&Play/Pause";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(203, 6);
			// 
			// OpenAudioDialog
			// 
			this.OpenAudioDialog.Filter = "Audio files|*.mp3;*.wav;*.ogg|All files|*.*";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.audioPanel1);
			this.splitContainer1.Panel1MinSize = 200;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Panel2.Controls.Add(this.panel1);
			this.splitContainer1.Size = new System.Drawing.Size(692, 538);
			this.splitContainer1.SplitterDistance = 250;
			this.splitContainer1.TabIndex = 4;
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader1,
            this.columnHeader2});
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(0, 30);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(692, 254);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
			this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
			this.listView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormMain_KeyPress);
			// 
			// columnHeader3
			// 
			this.columnHeader3.DisplayIndex = 2;
			this.columnHeader3.Width = 320;
			// 
			// columnHeader1
			// 
			this.columnHeader1.DisplayIndex = 0;
			this.columnHeader1.Text = "Start";
			this.columnHeader1.Width = 100;
			// 
			// columnHeader2
			// 
			this.columnHeader2.DisplayIndex = 1;
			this.columnHeader2.Text = "End";
			this.columnHeader2.Width = 100;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.DialogTextBox);
			this.panel1.Controls.Add(this.StartTimeBox);
			this.panel1.Controls.Add(this.EndTimeBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Font = new System.Drawing.Font("Lucida Console", 12F);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(3);
			this.panel1.Size = new System.Drawing.Size(692, 30);
			this.panel1.TabIndex = 2;
			// 
			// DialogTextBox
			// 
			this.DialogTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DialogTextBox.Location = new System.Drawing.Point(243, 3);
			this.DialogTextBox.Name = "DialogTextBox";
			this.DialogTextBox.Size = new System.Drawing.Size(446, 23);
			this.DialogTextBox.TabIndex = 2;
			// 
			// StartTimeBox
			// 
			this.StartTimeBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.StartTimeBox.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
			this.StartTimeBox.Location = new System.Drawing.Point(123, 3);
			this.StartTimeBox.Mask = "0:90:00.00";
			this.StartTimeBox.Name = "StartTimeBox";
			this.StartTimeBox.Size = new System.Drawing.Size(120, 23);
			this.StartTimeBox.TabIndex = 0;
			this.StartTimeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// EndTimeBox
			// 
			this.EndTimeBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.EndTimeBox.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
			this.EndTimeBox.Location = new System.Drawing.Point(3, 3);
			this.EndTimeBox.Mask = "0:90:00.00";
			this.EndTimeBox.Name = "EndTimeBox";
			this.EndTimeBox.Size = new System.Drawing.Size(120, 23);
			this.EndTimeBox.TabIndex = 1;
			this.EndTimeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// OpenSubDialog
			// 
			this.OpenSubDialog.Filter = "Subtitle files|*.ass;*.ssa|All files|*.*";
			// 
			// writeStartTimeToolStripMenuItem
			// 
			this.writeStartTimeToolStripMenuItem.Name = "writeStartTimeToolStripMenuItem";
			this.writeStartTimeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.writeStartTimeToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
			this.writeStartTimeToolStripMenuItem.Text = "Write Start Time";
			this.writeStartTimeToolStripMenuItem.Click += new System.EventHandler(this.writeStartTimeToolStripMenuItem_Click);
			// 
			// writeEndTimeToolStripMenuItem
			// 
			this.writeEndTimeToolStripMenuItem.Name = "writeEndTimeToolStripMenuItem";
			this.writeEndTimeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.writeEndTimeToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
			this.writeEndTimeToolStripMenuItem.Text = "Write End Time";
			this.writeEndTimeToolStripMenuItem.Click += new System.EventHandler(this.writeEndTimeToolStripMenuItem_Click);
			// 
			// writeLastEndTimeToolStripMenuItem
			// 
			this.writeLastEndTimeToolStripMenuItem.Name = "writeLastEndTimeToolStripMenuItem";
			this.writeLastEndTimeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.writeLastEndTimeToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
			this.writeLastEndTimeToolStripMenuItem.Text = "Write Last End Time";
			this.writeLastEndTimeToolStripMenuItem.Click += new System.EventHandler(this.writeLastEndTimeToolStripMenuItem_Click);
			// 
			// audioPanel1
			// 
			this.audioPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.audioPanel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.audioPanel1.Location = new System.Drawing.Point(0, 0);
			this.audioPanel1.Name = "audioPanel1";
			this.audioPanel1.Size = new System.Drawing.Size(692, 250);
			this.audioPanel1.TabIndex = 0;
			this.audioPanel1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormMain_KeyPress);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(692, 562);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FormMain";
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormMain_KeyPress);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

        private AudioPanel audioPanel1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openAudioToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog OpenAudioDialog;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ToolStripMenuItem controlToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem playPauseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openSubtitleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.MaskedTextBox StartTimeBox;
		private System.Windows.Forms.MaskedTextBox EndTimeBox;
		private System.Windows.Forms.TextBox DialogTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.OpenFileDialog OpenSubDialog;
				private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
				private System.Windows.Forms.ToolStripMenuItem writeStartTimeToolStripMenuItem;
				private System.Windows.Forms.ToolStripMenuItem writeEndTimeToolStripMenuItem;
				private System.Windows.Forms.ToolStripMenuItem writeLastEndTimeToolStripMenuItem;
	}
}