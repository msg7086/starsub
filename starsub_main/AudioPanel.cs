using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text;

namespace starsub
{
	public partial class AudioPanel : UserControl
	{
		public AudioPanel()
		{
			InitializeComponent();
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
		private float WScale = 1, HScale = 1;

		private bool MouseInArea = false;

		private const string svntag = "$Id: Form1.cs 40 2007-06-13 02:48:27Z Administrator $";
		private const string mainver = "1.0";
		private uint svnver;
		private uint SlicePerSecond { get { return 1000 / SliceSizeMS; } }

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
				PlayPointMS = MousePointMS;
				//sound.seekData((uint)(playpos * samplerate));
				channel.setPosition(PlayPointMS * 1000 / SliceSizeMS, FMOD.TIMEUNIT.MS);
				//Application.DoEvents();
				channel.setPaused(false);
				PlayingTimer.Start();
			}
		}

		private void Initialize()
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

		private void WaveDisplay_Paint(object sender, PaintEventArgs e)
		{
			if (peakdata == null)
				return;
			Graphics g = e.Graphics;
			int i, j;

			// Draw Background WaveForm
			for (i = Convert.ToInt32(SecondBar.Value * SlicePerSecond), j = 0; i < SliceCount && j < WaveDisplay.Width; i++, j++)
				g.DrawLine(WaveFormPen, j, Convert.ToSingle(WaveDisplay.Height / 2 + peakdata[i] / WScale), j, Convert.ToSingle(WaveDisplay.Height / 2 + weakdata[i] / WScale));

			int LeftTimeMS = SecondBar.Value * 1000;
			int RightTimeMS = LeftTimeMS + Convert.ToInt32(WaveDisplay.Width * 1000 / HScale / SlicePerSecond);
			if (MousePointMS >= LeftTimeMS || MousePointMS < RightTimeMS)
			{
				float x = (MousePointMS - LeftTimeMS) * HScale * SlicePerSecond;
				g.DrawLine(WritePen, x, 0, x, WaveDisplay.Height);
			}
			if (PlayingTimer.Enabled)
			{
				if (PlayPointMS >= LeftTimeMS && PlayPointMS < RightTimeMS)
				{
					float x = (PlayPointMS - LeftTimeMS) * HScale * SlicePerSecond;
					g.DrawLine(WritePen, x, WaveDisplay.Height / 5, x, WaveDisplay.Height * 4 / 5);
				}
				else if (PlayPointMS < SecondBar.Value * 1000)
					SecondBar.Value--;
				else
					SecondBar.Value += 5;
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
			uint postodraw = (uint)SecondBar.Value * 1000;
			g.DrawString(GetTimeText(postodraw), new Font("Tahoma", 16), Brushes.White, 0, WaveDisplay.Height - 30);
			postodraw = MousePointMS;
			g.DrawString(GetTimeText(postodraw), new Font("Tahoma", 16), Brushes.White, MousePointMS - SecondBar.Value * 100, WaveDisplay.Height - 60);
			postodraw = (uint)(SecondBar.Value * 100 + WaveDisplay.Width - 5);
			g.DrawString(GetTimeText(postodraw), new Font("Tahoma", 16), Brushes.White, WaveDisplay.Width - 100, WaveDisplay.Height - 30);
		}

		private string GetTimeText(uint MS)
		{
			return string.Format("{0:00}:{1:00}.{2:00}", MS / 60000, MS % 60000 / 1000, MS / 100 % 100);
		}

		private void WaveDisplay_MouseUp(object sender, MouseEventArgs e)
		{
			MousePointMS = Convert.ToUInt32(SecondBar.Value + e.X / HScale / SlicePerSecond); // downscale by HScale, and divide by SliceSize, from e.X to millisecond
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

		private void timer_Tick(object sender, EventArgs e)
		{
			uint i = 0;
			channel.getPosition(ref i, FMOD.TIMEUNIT.MS);
			i = i * SliceSizeMS / 1000;
			if (i != PlayPointMS)
			{
				PlayPointMS = i;
				//if(this.WindowState != FormWindowState.Minimized)
				WaveDisplay.Refresh();
				if (i >= SliceCount - 10)
				{
					channel.setPaused(true);
					PlayingTimer.Stop();
					//sound.seekData(0);
				}
			}
		}

		private void SecondBar_Scroll(object sender, ScrollEventArgs e)
		{
			WaveDisplay.Refresh();
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
				SecondBar.Maximum = Convert.ToInt32(SliceCount - WaveDisplay.Width) / 100 + 10;
		}

		private void loadaudio(string Filename)
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
				short peak, weak;
				uint pos = 0, i;
				short[] buffer = new short[SampleCountPerSlice * 2];
				peakdata = new short[SliceCount + 5];
				weakdata = new short[SliceCount + 5];
				uint BytePerSlice = SampleCountPerSlice * 4;
				IntPtr buff = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)BytePerSlice);
				do
				{
					sound.readData(buff, BytePerSlice, ref SampleCount);
					System.Runtime.InteropServices.Marshal.Copy(buff, buffer, 0, (int)BytePerSlice / 2);
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
					//if (pos < (SliceCount + 5) && pos % 500 == 0)
					//	statusBar1.Text = string.Format(lang._("message", "openfile"), curraudio) + "  " + (pos * 100 / (SliceCount + 5)) + "%";
				} while (SampleCount == BytePerSlice);
				SliceCount = pos;
				System.Runtime.InteropServices.Marshal.FreeHGlobal(buff);
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
				w.Write("STSB");
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
			WScale = MaxPeakValue / (WaveDisplay.Height - 30) * 2;
			calcscrollbar();
			WaveDisplay.Refresh();
			sound.release();

			system.createStream(AudioFileName, FMOD.MODE.ACCURATETIME, ref sound);
			system.playSound(FMOD.CHANNELINDEX.FREE, sound, true, ref channel);
		}

	}
}
