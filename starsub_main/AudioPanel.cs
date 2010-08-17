using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace starsub
{
	public partial class AudioPanel : UserControl
	{
		public new event KeyPressEventHandler KeyPress;
		public List<int> LinePos = null;
		public AudioPanel()
		{
			InitializeComponent();
			DoubleBuffered = true;
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

		}

		#region uservar

		private string AudioFileName = "";
		private short[] peakdata = null, weakdata = null;
		private uint SliceCount = 0;
		private Pen WaveFormPen = new Pen(Color.FromArgb(0x78, 0xc8, 0xa0));
		private Pen WritePen = new Pen(Color.White);
		private uint MousePointMS = 0, PlayPointMS = 0;
		private int MaxPeakValue = 0;

		private const uint SliceSizeMS = 10;
		private float MyYScale = 1, XScale = 1, YScale;

		private bool MouseInArea = false;

		private uint SlicePerSecond { get { return 1000 / SliceSizeMS; } }
		private uint AudioLengthMS { get { return SliceSizeMS * SliceCount; } }

		#endregion

		bool SmoothMoving = false;

		private void WaveDisplay_Paint(object sender, PaintEventArgs e)
		{
			if (peakdata == null)
				return;

			int LeftTimeMS = SecondBar.Value * 1000;
			int RightTimeMS = LeftTimeMS + Convert.ToInt32(WaveDisplay.Width * 1000 / XScale / SlicePerSecond);

			if (SmoothMoving)
			{
				if (PlayPointMS - LeftTimeMS < 3000 || RightTimeMS > AudioLengthMS)
					SmoothMoving = false;
				else
					SecondBar.Value++;
			}

			Graphics g = e.Graphics;

			int i;

			// Draw Background WaveForm
			var halfheight = WaveDisplay.Height / 2;
			var LeftOffset = Convert.ToInt32(SecondBar.Value * SlicePerSecond);
			for (i = 0; i < WaveDisplay.Width; i++)
			{
				var offset = LeftOffset + Convert.ToInt32(i / XScale);
				if (offset >= SliceCount)
					break;
				g.DrawLine(WaveFormPen, i, halfheight + Convert.ToSingle(peakdata[offset] / YScale), i, halfheight + Convert.ToSingle(weakdata[offset] / YScale));
			}

			// Draw Middle WhiteLine
			g.DrawLine(WritePen, 0, halfheight, WaveDisplay.Width, halfheight);

			// Draw Pseudo Line
			if (LinePos != null)
				for (i = LeftOffset; i < LeftOffset + Convert.ToInt32(WaveDisplay.Width / XScale); i++)
				{
					if (LinePos.Contains(i))
					{
						float x = (i - LeftOffset) * XScale;
						g.DrawLine(WritePen, x, 0, x, WaveDisplay.Height);
					}
				}

			// Draw Mouse Point Line
			if (MousePointMS >= LeftTimeMS || MousePointMS < RightTimeMS)
			{
				float x = (MousePointMS - LeftTimeMS) * XScale * SlicePerSecond / 1000;
				g.DrawLine(WritePen, x, 0, x, WaveDisplay.Height);
			}

			// Draw Playing Point Line
			if (PlayingTimer.Enabled)
			{
				if (PlayPointMS >= LeftTimeMS && PlayPointMS < RightTimeMS - 1000)
				{
					float x = (PlayPointMS - LeftTimeMS) * XScale * SlicePerSecond / 1000;
					g.DrawLine(WritePen, x, WaveDisplay.Height / 5, x, WaveDisplay.Height * 4 / 5);
				}
				else if (PlayPointMS < SecondBar.Value * 1000)
					SecondBar.Value--;
				else if (RightTimeMS < AudioLengthMS)
					//SecondBar.Value += 5;
					SmoothMoving = true;
			}

			// Print Current Time Position
			uint postodraw = Convert.ToUInt32(LeftTimeMS);
			g.DrawString(GetTimeText(postodraw), new Font("Tahoma", 14), Brushes.White, 0, WaveDisplay.Height - 30);
			postodraw = MousePointMS;
			float xx = (MousePointMS - LeftTimeMS) * XScale * SlicePerSecond / 1000;
			g.DrawString(GetTimeText(postodraw), new Font("Tahoma", 14), Brushes.White, xx, WaveDisplay.Height - 60);
			postodraw = Convert.ToUInt32(RightTimeMS);
			g.DrawString(GetTimeText(postodraw), new Font("Tahoma", 14), Brushes.White, WaveDisplay.Width - 100, WaveDisplay.Height - 30);

			// Draw Last/Curr/Next Subtitle Line Pseudo Line
			if (Current != null)
			{
				if (Current.StartTime.TotalMilliseconds > LeftTimeMS && Current.StartTime.TotalMilliseconds < RightTimeMS)
				{
					xx = Convert.ToSingle((Current.StartTime.TotalMilliseconds - LeftTimeMS) * XScale * SlicePerSecond / 1000);
					g.DrawLine(WritePen, xx, WaveDisplay.Height * 2 / 5, xx, WaveDisplay.Height * 3 / 5);
					g.DrawLine(WritePen, xx, WaveDisplay.Height * 2 / 5, xx + 2, WaveDisplay.Height * 2 / 5 - 10);
					g.DrawLine(WritePen, xx, WaveDisplay.Height * 3 / 5, xx + 2, WaveDisplay.Height * 3 / 5 + 10);
					g.DrawString(GetTimeText(Convert.ToUInt32(Current.StartTime.TotalMilliseconds)), new Font("Tahoma", 14), Brushes.White, xx, WaveDisplay.Height - 60);
				}
			}
		}

		private void WaveDisplay_MouseUp(object sender, MouseEventArgs e)
		{
			MousePointMS = Convert.ToUInt32(SecondBar.Value * 1000 + e.X / XScale / SlicePerSecond * 1000); // downscale by HScale, and divide by SliceSize, from e.X to millisecond
			WaveDisplay.Refresh();
		}
		private void WaveDisplay_MouseEnter(object sender, EventArgs e)
		{
			MouseInArea = true;
		}
		private void WaveDisplay_MouseLeave(object sender, EventArgs e)
		{
			MouseInArea = false;
		}

		private void PlayingTimer_Tick(object sender, EventArgs e)
		{
			uint i = 0;
			channel.getPosition(ref i, FMOD.TIMEUNIT.MS);
			if (i != PlayPointMS)
			{
				PlayPointMS = i;
				//if(this.WindowState != FormWindowState.Minimized)
				WaveDisplay.Refresh();
				if (i >= AudioLengthMS - 1000)
				{
					channel.setPaused(true);
					PlayingTimer.Stop();
					//sound.seekData(0);
				}
			}
		}

		private void SecondBar_Scroll(object sender, ScrollEventArgs e)
		{
			//WaveDisplay.Refresh();
		}

		private void SecondBar_ValueChanged(object sender, EventArgs e)
		{
			if (!SmoothMoving)
				WaveDisplay.Refresh();
		}

		private void WaveDisplay_Resize(object sender, EventArgs e)
		{
			if (MyYScale > 0)
				YScale = Convert.ToInt32(MaxPeakValue / (WaveDisplay.Height - 30) * 2 / MyYScale);

		}

		private void YTrackBar_ValueChanged(object sender, EventArgs e)
		{
			MyYScale = YTrackBar.Value * YTrackBar.Value * YTrackBar.Value / 1000f;
			YScale = Convert.ToInt32(MaxPeakValue / (WaveDisplay.Height - 30) * 2 / MyYScale);
			label1.Text = string.Format("X: {0:0.00}{1}Y: {2:0.00}", XScale, Environment.NewLine, MyYScale);
			WaveDisplay.Refresh();
		}

		private void XTrackBar_ValueChanged(object sender, EventArgs e)
		{
			XScale = XTrackBar.Value * XTrackBar.Value / 100f;
			label1.Text = string.Format("X: {0:0.00}{1}Y: {2:0.00}", XScale, Environment.NewLine, MyYScale);
			WaveDisplay.Refresh();
		}

		private void All_KeyPress(object sender, KeyPressEventArgs e)
		{
			KeyPress(sender, e);
		}

	}
}
