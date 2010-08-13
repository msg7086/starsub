namespace starsub
{
	partial class AudioPanel
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				FMOD.RESULT result;

				/*
					Shut down
				*/
				if (sound != null)
				{
					result = sound.release();
					ERRCHECK(result);
				}
				if (system != null)
				{
					result = system.close();
					ERRCHECK(result);
					result = system.release();
					ERRCHECK(result);
				}

				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.SecondBar = new System.Windows.Forms.HScrollBar();
			this.PlayingTimer = new System.Windows.Forms.Timer(this.components);
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.WaveDisplay = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.XTrackBar = new System.Windows.Forms.TrackBar();
			this.YTrackBar = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.WaveDisplay)).BeginInit();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.XTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.YTrackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// SecondBar
			// 
			this.SecondBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.SecondBar.LargeChange = 5;
			this.SecondBar.Location = new System.Drawing.Point(0, 257);
			this.SecondBar.Maximum = 4;
			this.SecondBar.Name = "SecondBar";
			this.SecondBar.Size = new System.Drawing.Size(450, 20);
			this.SecondBar.TabIndex = 9;
			this.SecondBar.ValueChanged += new System.EventHandler(this.SecondBar_ValueChanged);
			this.SecondBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.SecondBar_Scroll);
			// 
			// PlayingTimer
			// 
			this.PlayingTimer.Interval = 50;
			this.PlayingTimer.Tick += new System.EventHandler(this.PlayingTimer_Tick);
			// 
			// progressBar1
			// 
			this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.progressBar1.Location = new System.Drawing.Point(0, 277);
			this.progressBar1.Maximum = 0;
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(450, 23);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar1.TabIndex = 0;
			this.progressBar1.Visible = false;
			// 
			// WaveDisplay
			// 
			this.WaveDisplay.BackColor = System.Drawing.Color.Black;
			this.WaveDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WaveDisplay.Location = new System.Drawing.Point(0, 0);
			this.WaveDisplay.Name = "WaveDisplay";
			this.WaveDisplay.Size = new System.Drawing.Size(450, 257);
			this.WaveDisplay.TabIndex = 14;
			this.WaveDisplay.TabStop = false;
			this.WaveDisplay.MouseLeave += new System.EventHandler(this.WaveDisplay_MouseLeave);
			this.WaveDisplay.Resize += new System.EventHandler(this.WaveDisplay_Resize);
			this.WaveDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.WaveDisplay_Paint);
			this.WaveDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WaveDisplay_MouseUp);
			this.WaveDisplay.MouseEnter += new System.EventHandler(this.WaveDisplay_MouseEnter);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.WaveDisplay);
			this.panel1.Controls.Add(this.SecondBar);
			this.panel1.Controls.Add(this.progressBar1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(450, 300);
			this.panel1.TabIndex = 15;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.YTrackBar);
			this.groupBox1.Controls.Add(this.XTrackBar);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
			this.groupBox1.Location = new System.Drawing.Point(450, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(150, 300);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "比例调整";
			// 
			// XTrackBar
			// 
			this.XTrackBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.XTrackBar.Location = new System.Drawing.Point(3, 252);
			this.XTrackBar.Maximum = 15;
			this.XTrackBar.Minimum = 5;
			this.XTrackBar.Name = "XTrackBar";
			this.XTrackBar.Size = new System.Drawing.Size(144, 45);
			this.XTrackBar.TabIndex = 1;
			this.XTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.XTrackBar.Value = 10;
			this.XTrackBar.ValueChanged += new System.EventHandler(this.XTrackBar_ValueChanged);
			// 
			// YTrackBar
			// 
			this.YTrackBar.Dock = System.Windows.Forms.DockStyle.Left;
			this.YTrackBar.Location = new System.Drawing.Point(3, 18);
			this.YTrackBar.Maximum = 15;
			this.YTrackBar.Minimum = 5;
			this.YTrackBar.Name = "YTrackBar";
			this.YTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.YTrackBar.Size = new System.Drawing.Size(45, 234);
			this.YTrackBar.TabIndex = 0;
			this.YTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.YTrackBar.Value = 10;
			this.YTrackBar.ValueChanged += new System.EventHandler(this.YTrackBar_ValueChanged);
			// 
			// AudioPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.Name = "AudioPanel";
			this.Size = new System.Drawing.Size(600, 300);
			((System.ComponentModel.ISupportInitialize)(this.WaveDisplay)).EndInit();
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.XTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.YTrackBar)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.HScrollBar SecondBar;
		private System.Windows.Forms.Timer PlayingTimer;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.PictureBox WaveDisplay;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TrackBar XTrackBar;
		private System.Windows.Forms.TrackBar YTrackBar;
	}
}
