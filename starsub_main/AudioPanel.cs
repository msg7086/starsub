using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;

namespace starsub
{
	public partial class AudioPanel : UserControl
	{
		public new event KeyPressEventHandler KeyPress;
		public AudioPanel()
		{
			InitializeComponent();
			DoubleBuffered = true;
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
			
		}

		#region uservar
		//private int				theight = 400;
		private string AudioFileName = "";
		private short[] peakdata = null, weakdata = null;
		private uint SliceCount = 0;
		private Pen WaveFormPen = new Pen(Color.FromArgb(0x78, 0xc8, 0xa0));
		private Pen WritePen = new Pen(Color.White);
		private uint MousePointMS = 0, PlayPointMS = 0; // current position in ms
		private int MaxPeakValue = 0;
		//private uint samplerate = 48000;
		private const uint SliceSizeMS = 10; // ms
		private float MyYScale = 1, XScale = 1, YScale;

		private bool MouseInArea = false;

		private const string svntag = "$Id: Form1.cs 40 2007-06-13 02:48:27Z Administrator $";
		private const string mainver = "1.0";
		private uint svnver;
		private uint SlicePerSecond { get { return 1000 / SliceSizeMS; } }
		private uint AudioLengthMS { get { return SliceSizeMS * SliceCount; } }

		#endregion

		#region fmodvar
		private FMOD.System system = null;
		private FMOD.Sound sound = null;
		private FMOD.Channel channel = null;
		#endregion

		public void CloseAudio()
		{
			PlayingTimer.Stop();
			sound.release();
			peakdata = null;
			weakdata = null;
			AudioFileName = "";
			SecondBar.Value = 0;
		}

		public void Pause()
		{
			if (AudioFileName == "")
				return;
			if (PlayingTimer.Enabled)
			{
				channel.setPaused(true);
				PlayingTimer.Stop();
			}
			else
			{
				if (MousePointMS >= AudioLengthMS - 1000)
				{
					return;
				}

				PlayPointMS = MousePointMS;
				//sound.seekData((uint)(playpos * samplerate));
				channel.setPosition(PlayPointMS, FMOD.TIMEUNIT.MS);
				//Application.DoEvents();
				channel.setPaused(false);
				PlayingTimer.Start();
			}
		}

		public void Initialize()
		{
			#region fmodinit
			uint version = 0;
			FMOD.RESULT result;

			/*
				Create a System object and initialize.
			*/
			result = FMOD.Factory.System_Create(ref system);
			ERRCHECK(result);

			result = system.getVersion(ref version);
			ERRCHECK(result);
			if (version < FMOD.VERSION.number)
			{
				MessageBox.Show("Error!  You are using an old version of FMOD " + version.ToString("X") + ".  This program requires " + FMOD.VERSION.number.ToString("X") + ".");
				Application.Exit();
			}

			result = system.init(32, FMOD.INITFLAG.NORMAL, (IntPtr)null);
			ERRCHECK(result);
			#endregion
			SecondBar.Minimum = SecondBar.Maximum = 0;
			svnver = Convert.ToUInt32("$Rev: 40 $".Split(' ')[1], 10);
		}

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
				if(offset >= SliceCount)
					break;
				g.DrawLine(WaveFormPen, i, halfheight + Convert.ToSingle(peakdata[offset] / YScale), i, halfheight + Convert.ToSingle(weakdata[offset] / YScale));
			}


			if (MousePointMS >= LeftTimeMS || MousePointMS < RightTimeMS)
			{
				float x = (MousePointMS - LeftTimeMS) * XScale * SlicePerSecond / 1000;
				g.DrawLine(WritePen, x, 0, x, WaveDisplay.Height);
			}
			if (PlayingTimer.Enabled)
			{
				if (PlayPointMS >= LeftTimeMS && PlayPointMS < RightTimeMS - 1000)
				{
					float x = (PlayPointMS - LeftTimeMS) * XScale * SlicePerSecond / 1000;
					g.DrawLine(WritePen, x, WaveDisplay.Height / 5, x, WaveDisplay.Height * 4 / 5);
				}
				else if (PlayPointMS < SecondBar.Value * 1000)
					SecondBar.Value--;
				else if(RightTimeMS < AudioLengthMS)
					//SecondBar.Value += 5;
					SmoothMoving = true;
			}
			// find crlf forward and backward in txtsub
			/*
			int bwpos = txtsub.SelectionStart == 0 ? 0 : txtsub.Text.LastIndexOf('\n', txtsub.SelectionStart - 1) + 1;
			int fwpos = txtsub.SelectionStart > txtsub.Text.Length ? 0 : txtsub.Text.IndexOf('\n', txtsub.SelectionStart);
			if(bwpos < 0)
				bwpos = 0;
			if(fwpos < 0)
				fwpos = txtsub.Text.Length;
			if(bwpos >= 0 && fwpos > bwpos)
				g.DrawString(txtsub.Text.Substring(bwpos, fwpos - bwpos).Trim(), new Font(lang._("misc", "drawfont"), 18), Brushes.White, panel1.Width / 3, 0);
			 */
			// print current time position
			uint postodraw = Convert.ToUInt32(LeftTimeMS);
			g.DrawString(GetTimeText(postodraw), new Font("Tahoma", 16), Brushes.White, 0, WaveDisplay.Height - 30);
			postodraw = MousePointMS;
			float xx = (MousePointMS - LeftTimeMS) * XScale * SlicePerSecond / 1000;
			g.DrawString(GetTimeText(postodraw), new Font("Tahoma", 16), Brushes.White, xx, WaveDisplay.Height - 60);
			postodraw = Convert.ToUInt32(RightTimeMS);
			g.DrawString(GetTimeText(postodraw), new Font("Tahoma", 16), Brushes.White, WaveDisplay.Width - 100, WaveDisplay.Height - 30);
		}

		private string GetTimeText(uint MS)
		{
			return string.Format("{0:00}:{1:00}.{2:00}", MS / 60000, MS % 60000 / 1000, MS / 100 % 100);
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

		private void ERRCHECK(FMOD.RESULT result)
		{
			if (result != FMOD.RESULT.OK)
			{
				PlayingTimer.Stop();
				MessageBox.Show("FMOD error! " + result + " - " + FMOD.Error.String(result));
				Environment.Exit(-1);
			}
		}

		private void calcscrollbar()
		{
			if (peakdata != null)
				Invoke(new MethodInvoker(() => SecondBar.Maximum = Convert.ToInt32(SliceCount / SlicePerSecond) + 10));
		}

		public void OpenAudio(string Filename)
		{
			OpenAudio(Filename, null);
		}
		public void OpenAudio(string Filename, Delegate callback)
		{
			if (AudioFileName != "")
			{
				sound.release();
				peakdata = null;
				weakdata = null;
			}
			SecondBar.Value = 0;
			AudioFileName = Filename;
			//statusBar1.Text = string.Format(lang._("message", "openfile"), curraudio);

			FMOD.RESULT result = system.createStream(AudioFileName, FMOD.MODE.HARDWARE | FMOD.MODE.ACCURATETIME, ref sound);
			sound.seekData(0);
			ERRCHECK(result);

			// build waveform data
			/* the following variables used when loading from cache file:
				 * 
				 * audiolength
				 * peakdata[], weakdata[], globalpeak, samplerate
				 * 
				 */
			if (System.IO.File.Exists(AudioFileName + ".peak"))
			{
				// loading from cache file

				/* cache file structure
				 * 'STSB'      4 bytes
				 * version     4 bytes
				 * slicecount  4 bytes
				 * global peak 4 bytes
				 * peakdata[0], weakdata[0], p1, w1, ... (interlaced)  any bytes
				 */
				//statusBar1.Text = lang._("message", "loadcache");
				FileStream fs = new FileStream(AudioFileName + ".peak", FileMode.Open, FileAccess.Read);
				BinaryReader r = new BinaryReader(fs);
				uint STSB = r.ReadUInt32();
				uint Version = r.ReadUInt32();
				SliceCount = r.ReadUInt32();
				MaxPeakValue = r.ReadInt32();
				int i;
				peakdata = new short[SliceCount];
				weakdata = new short[SliceCount];
				for (i = 0; i < SliceCount; i++)
				{
					peakdata[i] = r.ReadInt16();
					weakdata[i] = r.ReadInt16();
				}
				r.Close();
				r = null;
				fs.Close();
				fs = null;

			}
			else
			{
				uint SampleCount = 0, AudioLength = 0;
				sound.getLength(ref SampleCount, FMOD.TIMEUNIT.PCM);
				sound.getLength(ref AudioLength, FMOD.TIMEUNIT.MS);
				uint SampleCountPerSlice = (uint)((ulong)SampleCount * 1000 / AudioLength) / SlicePerSecond;
				SliceCount = AudioLength / SliceSizeMS;
				Invoke(new MethodInvoker(() =>
				{
					progressBar1.Maximum = Convert.ToInt32(SliceCount);
					progressBar1.Value = 0;
					progressBar1.Visible = true;
				}));
				short peak, weak;
				uint pos = 0, i;
				short[] buffer = new short[SampleCountPerSlice * 2];
				peakdata = new short[SliceCount + 5];
				weakdata = new short[SliceCount + 5];
				uint BytePerSlice = SampleCountPerSlice * 4;
				IntPtr buff = Marshal.AllocHGlobal((int)BytePerSlice);
				do
				{
					sound.readData(buff, BytePerSlice, ref SampleCount);
					Marshal.Copy(buff, buffer, 0, (int)BytePerSlice / 2);
					peak = weak = 0;
					for (i = 0; i < SampleCount / 4; i++)
					{
						if (buffer[i] > peak)
							peak = buffer[i];
						if (buffer[i] < weak)
							weak = buffer[i];
					}
					peakdata[pos] = peak;
					weakdata[pos] = weak;
					pos++;
					if (pos < (SliceCount + 5) && pos % 500 == 0)
						Invoke(new MethodInvoker(() => progressBar1.Value = Convert.ToInt32(pos)));
					//	statusBar1.Text = string.Format(lang._("message", "openfile"), curraudio) + "  " + (pos * 100 / (SliceCount + 5)) + "%";
				} while (SampleCount == BytePerSlice);
				SliceCount = pos;
				Invoke(new MethodInvoker(() => progressBar1.Visible = false));
				Marshal.FreeHGlobal(buff);
				peak = 0;
				//statusBar1.Text = lang._("message", "normalize");
				for (i = 0; i < SliceCount; i++)
				{
					if (peakdata[i] > peak)
						peak = peakdata[i];
					if (-weakdata[i] > peak)
						peak = (short)-weakdata[i];
				}
				MaxPeakValue = peak;

				// creating peak cache file
				FileStream fs = new FileStream(AudioFileName + ".peak", FileMode.CreateNew);
				BinaryWriter w = new BinaryWriter(fs, Encoding.ASCII);
				w.Write(0x7890abcd);
				w.Write((uint)2);
				w.Write(SliceCount);
				w.Write(MaxPeakValue);
				for (i = 0; i < SliceCount; i++)
				{
					w.Write(peakdata[i]);
					w.Write(weakdata[i]);
				}
				w.Close();
				w = null;
				fs.Close();
				fs = null;
			}
			//statusBar1.Text += " Done";
			YScale = Convert.ToInt32(MaxPeakValue / (WaveDisplay.Height - 30) * 2 / MyYScale);
			calcscrollbar();
			Invoke(new MethodInvoker(() => WaveDisplay.Refresh()));
			sound.release();

			system.createStream(AudioFileName, FMOD.MODE.ACCURATETIME, ref sound);
			system.playSound(FMOD.CHANNELINDEX.FREE, sound, true, ref channel);

			if (callback != null)
				callback.DynamicInvoke();
		}

		private void SecondBar_ValueChanged(object sender, EventArgs e)
		{
			if(!SmoothMoving)
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
