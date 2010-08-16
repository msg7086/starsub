using System;
using System.Windows.Forms;

namespace starsub
{
	partial class AudioPanel
	{
		private FMOD.System system = null;
		private FMOD.Sound sound = null;
		private FMOD.Channel channel = null;

		private void InitFmod()
		{
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
	}
}
