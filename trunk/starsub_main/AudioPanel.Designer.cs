﻿namespace starsub
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
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.SecondBar = new System.Windows.Forms.HScrollBar();
			this.PlayingTimer = new System.Windows.Forms.Timer(this.components);
			this.WaveDisplay = new System.Windows.Forms.Panel();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 375);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(534, 3);
			this.splitter1.TabIndex = 13;
			this.splitter1.TabStop = false;
			// 
			// SecondBar
			// 
			this.SecondBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.SecondBar.LargeChange = 5;
			this.SecondBar.Location = new System.Drawing.Point(0, 378);
			this.SecondBar.Maximum = 4;
			this.SecondBar.Name = "SecondBar";
			this.SecondBar.Size = new System.Drawing.Size(534, 20);
			this.SecondBar.TabIndex = 9;
			this.SecondBar.ValueChanged += new System.EventHandler(this.SecondBar_ValueChanged);
			this.SecondBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.SecondBar_Scroll);
			// 
			// PlayingTimer
			// 
			this.PlayingTimer.Interval = 40;
			this.PlayingTimer.Tick += new System.EventHandler(this.PlayingTimer_Tick);
			// 
			// WaveDisplay
			// 
			this.WaveDisplay.BackColor = System.Drawing.Color.Black;
			this.WaveDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WaveDisplay.Location = new System.Drawing.Point(0, 0);
			this.WaveDisplay.Name = "WaveDisplay";
			this.WaveDisplay.Size = new System.Drawing.Size(534, 375);
			this.WaveDisplay.TabIndex = 14;
			this.WaveDisplay.MouseLeave += new System.EventHandler(this.WaveDisplay_MouseLeave);
			this.WaveDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.WaveDisplay_Paint);
			this.WaveDisplay.Resize += new System.EventHandler(this.WaveDisplay_Resize);
			this.WaveDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WaveDisplay_MouseUp);
			this.WaveDisplay.MouseEnter += new System.EventHandler(this.WaveDisplay_MouseEnter);
			// 
			// progressBar1
			// 
			this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.progressBar1.Location = new System.Drawing.Point(0, 398);
			this.progressBar1.Maximum = 0;
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(534, 20);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar1.TabIndex = 0;
			this.progressBar1.Visible = false;
			// 
			// AudioPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.WaveDisplay);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.SecondBar);
			this.Controls.Add(this.progressBar1);
			this.Name = "AudioPanel";
			this.Size = new System.Drawing.Size(534, 418);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.HScrollBar SecondBar;
		private System.Windows.Forms.Timer PlayingTimer;
		private System.Windows.Forms.Panel WaveDisplay;
		private System.Windows.Forms.ProgressBar progressBar1;
	}
}
