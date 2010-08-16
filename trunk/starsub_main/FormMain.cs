using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace starsub
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			this.DoubleBuffered = true;
			audioPanel1.Initialize();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			audioPanel1.Pause();
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
	}
}
