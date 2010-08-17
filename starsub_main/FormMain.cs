using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace starsub
{
	public partial class FormMain : Form
	{
		public string SubtitleFilename;
		public bool SubtitleModified = false;
		public FormMain()
		{
			InitializeComponent();
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			DoubleBuffered = true;
			audioPanel1.Initialize();
		}

		private void openAudioToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (OpenAudioDialog.ShowDialog() == DialogResult.OK)
			{
				Thread t = new Thread(
						new ThreadStart(
								() => audioPanel1.OpenAudio(
										OpenAudioDialog.FileName, new MethodInvoker(
												() => Invoke(
														new MethodInvoker(
																() => openAudioToolStripMenuItem.Enabled = true
														)
												)
										)
								)
						)
				);
				openAudioToolStripMenuItem.Enabled = false;
				t.Start();
			}
		}

		private void FormMain_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ')
				audioPanel1.Pause();
		}

		private void openSubtitleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (OpenSubDialog.ShowDialog() == DialogResult.OK)
			{
				LoadSub(OpenSubDialog.FileName);
			}
		}

		private void LoadSub(string Filename)
		{
			SubtitleFilename = Filename;
			string[] lines = File.ReadAllLines(SubtitleFilename);
			foreach (var line in lines)
			{
				// test if it's ass
				var m = Regex.Match(line, @"(\d+:\d+:\d+\.\d+).*?(\d+:\d+:\d+\.\d+),.*?,.*?,.*?,.*?,.*?,.*?,(.*)$");
				if (!m.Success)
				{
					var lvix = listView1.Items.Add(new ListViewItem(line));
					lvix.SubItems.Add("META");
					lvix.SubItems.Add("META");
					continue;
				}
				var lvi = listView1.Items.Add(new ListViewItem(m.Groups[3].Value));
				lvi.SubItems.Add(m.Groups[1].Value);
				lvi.SubItems.Add(m.Groups[2].Value);
			}
			SubtitleModified = false;
		}

		private void listView1_DoubleClick(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count != 1)
				return;
			var line = listView1.SelectedItems[0];
			if (line.SubItems[1].Text == "META")
				return;
			audioPanel1.Seek(Convert.ToInt32(MyTimeParse(line.SubItems[1].Text).TotalMilliseconds));
		}

		private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			if (listView1.SelectedItems.Count != 1)
				return;
			var line = listView1.SelectedItems[0];
			if (line.SubItems[1].Text == "META")
				return;
			StartTimeBox.Text = line.SubItems[1].Text;
			EndTimeBox.Text = line.SubItems[2].Text;
			DialogTextBox.Text = line.Text;
			ListViewItem LastLine = null, NextLine = null;
			if (listView1.SelectedIndices[0] > 0)
				LastLine = listView1.Items[listView1.SelectedIndices[0] - 1];
			if (listView1.SelectedIndices[0] < listView1.Items.Count - 1)
				NextLine = listView1.Items[listView1.SelectedIndices[0] + 1];
			audioPanel1.SetSubtitleLines(
				LastLine == null || LastLine.SubItems[1].Text == "META" ? null : new SubtitleLine
				{
					StartTime = MyTimeParse(LastLine.SubItems[1].Text),
					EndTime = MyTimeParse(LastLine.SubItems[2].Text),
					DialogText = StripTags(LastLine.Text)
				}, new SubtitleLine
				{
					StartTime = MyTimeParse(line.SubItems[1].Text),
					EndTime = MyTimeParse(line.SubItems[2].Text),
					DialogText = StripTags(line.Text)
				}, NextLine == null || NextLine.SubItems[1].Text == "META" ? null : new SubtitleLine
				{
					StartTime = MyTimeParse(NextLine.SubItems[1].Text),
					EndTime = MyTimeParse(NextLine.SubItems[2].Text),
					DialogText = StripTags(NextLine.Text)
				}
			);
		}

		private TimeSpan MyTimeParse(string TimeStr)
		{
			try
			{
				return TimeSpan.Parse(TimeStr);
			}
			catch (Exception)
			{
				return TimeSpan.Zero;
			}
		}

		private string StripTags(string subtitle)
		{
			return Regex.Replace(subtitle, @"\{[^\}]*\}", "★");
		}

		/// <summary>
		/// Write to CURRENT Line Start Time. Write to LAST Line End Time if empty. Jump to NEXT line.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void writeStartTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count != 1)
				return;
			var line = listView1.SelectedItems[0];
			line.SubItems[1].Text = StartTimeBox.Text = audioPanel1.GetMouseTimeText();

			ListViewItem lastline = null;
			if (listView1.SelectedIndices[0] >= 1)
			{
				lastline = listView1.Items[listView1.SelectedIndices[0] - 1];
				if (lastline.SubItems[1].Text != "META" && lastline.SubItems[2].Text == "META")
					lastline.SubItems[2].Text = audioPanel1.GetMouseTimeText();
			}
			listView1.Items[listView1.SelectedIndices[0] + 1].Selected = true;
			audioPanel1.RefreshWave();
		}

		/// <summary>
		/// Write to LAST Line End Time if CURRENT Line Start Time is empty, else, Write to THIS Line End Time and JUMP.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void writeEndTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count != 1)
				return;
			var line = listView1.SelectedItems[0];
			if (line.SubItems[1].Text != "META")
			{
				line.SubItems[2].Text = EndTimeBox.Text = audioPanel1.GetMouseTimeText();
				listView1.Items[listView1.SelectedIndices[0] + 1].Selected = true;
			}
			else
			{
				ListViewItem lastline = null;
				if (listView1.SelectedIndices[0] >= 1)
				{
					lastline = listView1.Items[listView1.SelectedIndices[0] - 1];
					lastline.SubItems[2].Text = audioPanel1.GetMouseTimeText();
				}
			}
			audioPanel1.RefreshWave();
		}

		private void writeLastEndTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count != 1)
				return;
			ListViewItem line = null;
			if (listView1.SelectedIndices[0] < 1)
				return;
			line = listView1.Items[listView1.SelectedIndices[0] - 1];

			line.SubItems[2].Text = EndTimeBox.Text = audioPanel1.GetMouseTimeText();
			audioPanel1.RefreshWave();

		}

		private void DialogTextBox_TextChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count != 1)
				return;
			listView1.SelectedItems[0].Text = DialogTextBox.Text;
		}

		private void StartTimeBox_TextChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count != 1)
				return;
			listView1.SelectedItems[0].SubItems[1].Text = StartTimeBox.Text;
		}

		private void EndTimeBox_TextChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count != 1)
				return;
			listView1.SelectedItems[0].SubItems[2].Text = EndTimeBox.Text;
		}
	}
}
