using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace starsub
{
	partial class AudioPanel
	{
		private string GetTimeText(uint MS)
		{
			return string.Format("{0:00}:{1:00}.{2:00}", MS / 60000, MS % 60000 / 1000, MS / 100 % 100);
		}

		private void calcscrollbar()
		{
			if (peakdata != null)
				Invoke(new MethodInvoker(() => SecondBar.Maximum = Convert.ToInt32(SliceCount / SlicePerSecond) + 10));
		}

		private void BuildPseudoLine()
		{
			return;
			// Seal this function
			short[] xdata = new short[peakdata.Length];
			short[] ydata = new short[peakdata.Length];
			LinePos = new List<int>();
			// scan
			/*
			for (int i = 2; i < peakdata.Length - 5; i++)
			{
				//xdata[i] = (short)((peakdata[i] + peakdata[i + 1] + peakdata[i + 2] + peakdata[i + 3] + peakdata[i + 4]) / (peakdata[i - 1] + 1) * 1000);
				//if ((peakdata[i] + peakdata[i + 1] + peakdata[i + 2] + peakdata[i + 3] + peakdata[i + 4]) / (peakdata[i - 1] + 1) > 15)
				//	LinePos.Add(i - 1);
				xdata[i] = Math.Min(Math.Min(peakdata[i], peakdata[i - 1]), peakdata[i - 2]);
				//if (peakdata[i - 1] > 0)
			}
			//for (int i = 1; i < xdata.Length; i++)
			//	if (xdata[i] / (xdata[i - 1] + 1) > 2)
			//		LinePos.Add(i - 1);
			*/
			for (int i = 1; i < peakdata.Length - 21; i++)
			{
				for (int j = 0; j < 20; j++)
					//xdata[i] = Math.Max(xdata[i], peakdata[i + j]);
					xdata[i] = (short)(xdata[i] + peakdata[i + j] / 20);
			}
			/*
			for (int i = 2; i < peakdata.Length - 5; i++)
			{
				ydata[i] = Math.Max(Math.Max(xdata[i], xdata[i - 1]), xdata[i - 2]);
			}

			*/
			peakdata = xdata;
		}
	}

	public class SubtitleLine
	{
		public TimeSpan StartTime, EndTime;
		public string DialogText;
	}
}
