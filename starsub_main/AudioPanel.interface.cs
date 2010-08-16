using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace starsub
{
	partial class AudioPanel
	{
		SubtitleLine Last = null, Current = null, Next = null;

		public void Initialize()
		{
			InitFmod();
			SecondBar.Minimum = SecondBar.Maximum = 0;
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

			BuildPseudoLine();

			if (callback != null)
				callback.DynamicInvoke();
		}

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
				channel.setPosition(PlayPointMS, FMOD.TIMEUNIT.MS);
				channel.setPaused(false);
				PlayingTimer.Start();
			}
		}

		public void SetSubtitleLines(SubtitleLine Last, SubtitleLine Current, SubtitleLine Next)
		{
			this.Last = Last;
			this.Current = Current;
			this.Next = Next;
			WaveDisplay.Refresh();
		}
	}
}
