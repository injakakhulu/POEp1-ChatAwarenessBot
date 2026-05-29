using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace POEp1.UI
{
    public static class AudioPlayer
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, int cchBuffer);

        public static void PlayGreetingAsync()
        {
            Task.Run(async () =>
            {
                try
                {
                    string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greetings.wav");

                    if (!File.Exists(soundPath))
                    {
                        System.Diagnostics.Debug.WriteLine($"AUDIO LOG FAULT: File missing at path {soundPath}");
                        return;
                    }

                    string shortPath = GetShortPath(soundPath);

                    // 1. Clear out any previous remnants locked in the audio driver cache
                    mciSendString("close terminal_audio", null, 0, IntPtr.Zero);

                    // 2. Open up a fresh, dedicated hardware audio channel
                    mciSendString($"open {shortPath} type waveaudio alias terminal_audio", null, 0, IntPtr.Zero);

                    // 3. Thread Stabilization Pause: Give the kernel mixer 150 milliseconds 
                    // to safely buffer the audio file headers into memory before hitting play
                    await Task.Delay(150);

                    // 4. Broadcast the audio stream safely
                    mciSendString("play terminal_audio", null, 0, IntPtr.Zero);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"AUDIO HARDWARE EXCEPTION: {ex.Message}");
                }
            });
        }

        private static string GetShortPath(string longPath)
        {
            StringBuilder shortPathBuffer = new StringBuilder(255);
            GetShortPathName(longPath, shortPathBuffer, shortPathBuffer.Capacity);
            return shortPathBuffer.ToString();
        }
    }
}