using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sleep_Timer
{
	class Program
	{
		//this method enables the sleep funtion
		[DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

		private static Timer aTimer;
		private static Timer RunningTimer;
		private static int currentTime;

		static void Main(string[] args) {
			StringBuilder stringbuilder = new StringBuilder();

			stringbuilder.Append("Current Version: " + DisplayVersion()).AppendLine().AppendLine();
			stringbuilder.Append("Amount of minutes to wait: ");
			Console.Write(stringbuilder.ToString());
			stringbuilder.Clear();


			//Parse the string from console input stream to a Int32
			if (Int32.TryParse(Console.ReadLine(), out int MinutesToWait)) {
				stringbuilder.AppendLine().AppendFormat("Timer set for {0} minutes \n", MinutesToWait).AppendLine().Append("Starting Timer...\n\n").AppendFormat("Time till sleep: {0} minutes \n\n", MinutesToWait).Append("Press any Key to exit...");
				Console.Write(stringbuilder.ToString());
			} else {
				stringbuilder.AppendLine().Append("ERROR! Invalid input! Use only whole numbers next time").AppendLine();
				Console.Write(stringbuilder.ToString());
				Console.ReadLine();
				return;
			}
			stringbuilder.Clear();

			currentTime = MinutesToWait;
			//timer gets set and starts
			SetTimer(MinutesToWait);
			SetRunningTimer();
			Console.ReadKey();


			//close processes and end application
			aTimer.Stop();
			aTimer.Dispose();
			RunningTimer.Stop();
			RunningTimer.Dispose();
		}

		private static void SetTimer(int minutes) {
			//create a timer for x minutes
			aTimer = new Timer(minutes*60000);

			//Event will trigger when timer has elapsed
			aTimer.Elapsed += OnTimedEvent;
			aTimer.AutoReset = false;
			aTimer.Enabled = true;
		}

		private static void SetRunningTimer() {
			//create a timer for 1 minute
			RunningTimer = new Timer(60000);

			//Event will trigger when timer has elapsed
			RunningTimer.Elapsed += OnRunningTimedEvent;
			RunningTimer.AutoReset = true;
			RunningTimer.Enabled = true;
		}

		private static void OnTimedEvent(object  source, ElapsedEventArgs elapsed) {
			Console.Clear();
			Console.Write("Windows is Going to Sleep at " + DateTime.Now);

			//sleep the computer
			aTimer.Stop();
			aTimer.Dispose();
			RunningTimer.Stop();
			RunningTimer.Dispose();
			SetSuspendState(false, true, false);
		}

		private static void OnRunningTimedEvent(object source, ElapsedEventArgs elapsed) {
			currentTime--;
			Console.Clear();
			Console.Write("Time till sleep: {0} minutes \n\n", currentTime);
			Console.Write("Press any Key to exit...");
		}

		//setup version numbering display. AssemblyInfo.cs AssemblyVersion uses Asterix for build onwards.
		private static string DisplayVersion() {
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			DateTime buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
			string displayableVersion = $"{version} ({buildDate})";

			return displayableVersion;
		}

	}
}
