using System.IO;


namespace starsub
{
	/// <summary>
	/// Provide global static functions for subtitle processing
	/// </summary>
    public class StarSubFuntion
    {
        /// <summary>
        /// Output a subtitle line info stream <para>OutputStream</para>.
        /// </summary>
        /// <param name="OutputStream">The stream for outputing subtitle line.</param>
        /// <param name="StartTime">The start time of a subtitle line.</param>
        /// <param name="EndTime">the end time of a subtitle line.</param>
        /// <param name="Text">The subtitle content.</param>
        static public void OutputSubtitleLine(ref StreamWriter OutputStream, int StartTime, int EndTime, string Text)
        {
			OutputSubtitleLine(ref OutputStream, StartTime, EndTime, Text, false);
        }
		static public void OutputSubtitleLine(ref StreamWriter OutputStream, int StartTime, int EndTime, string Text, bool ASSMode)
		{
            OutputStream.WriteLine("Dialogue: {9}0,{0:0}:{1:00}:{2:00}.{3:0}0,{4:0}:{5:00}:{6:00}.{7:0}0,Default,,0000,0000,0000,,{8}",
                StartTime / 60000, StartTime % 60000 / 1000, StartTime % 1000 / 10, StartTime % 10,
                EndTime / 60000, EndTime % 60000 / 1000, EndTime % 1000 / 10, EndTime % 10,
                Text, ASSMode ? "" : "Marked=");
 		}

    }
}
