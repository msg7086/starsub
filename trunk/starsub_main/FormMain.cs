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
			if (listView1.SelectedIndices[0] > 1)
				LastLine = listView1.Items[listView1.SelectedIndices[0] - 1];
			if (listView1.SelectedIndices[0] < listView1.Items.Count - 1)
				NextLine = listView1.Items[listView1.SelectedIndices[0] + 1];
			audioPanel1.SetSubtitleLines(
				LastLine == null ? null : new SubtitleLine
				{
					StartTime = TimeSpan.Parse(LastLine.SubItems[1].Text),
					EndTime = TimeSpan.Parse(LastLine.SubItems[2].Text),
					DialogText = StripTags(LastLine.Text)
				}, new SubtitleLine
				{
					StartTime = TimeSpan.Parse(line.SubItems[1].Text),
					EndTime = TimeSpan.Parse(line.SubItems[2].Text),
					DialogText = StripTags(line.Text)
				}, NextLine == null ? null : new SubtitleLine
				{
					StartTime = TimeSpan.Parse(NextLine.SubItems[1].Text),
					EndTime = TimeSpan.Parse(NextLine.SubItems[2].Text),
					DialogText = StripTags(NextLine.Text)
				}
			);
		}

		private string StripTags(string subtitle)
		{
			return Regex.Replace(subtitle, @"\{[^\}]*\}", "★");
		}
	}
}
