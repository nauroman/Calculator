using UnityEngine;
using System.Collections;
using Flashunity.Screenshots;

namespace Flashunity.Logs
{
	public class Log
	{

		public static void Add (string label = "", string color = "", int type = LogType.LOG, int screenshotType = ScreenshotType.NONE, int reflection = -1)
		{
			if (BCanvas.instance) {
				BCanvas.instance.Add (label, type, color, screenshotType, reflection);
			}
		}

		public static void Error (string label = "", int screenshotType = ScreenshotType.MEMORY, int reflection = -1)
		{
			if (BCanvas.instance) {
				BCanvas.instance.Add (label, LogType.ERROR, "red", screenshotType, reflection);
			}
		}

		public static void Warning (string label = "", int screenshotType = ScreenshotType.MEMORY, int reflection = -1)
		{
			if (BCanvas.instance) {
				BCanvas.instance.Add (label, LogType.WARNING, "orange", screenshotType, reflection);
			}
		}
        
		public static void Remove (string label, string reflection = "")
		{
			if (BCanvas.instance) {
				BCanvas.instance.Remove (label, reflection);
			}
		}

		public static void Clear ()
		{
			if (BCanvas.instance) {
				BCanvas.instance.Clear ();
			}
		}

		public static bool Paused {
			set {
				if (BCanvas.instance) {
					BCanvas.instance.Paused = value;
				}
			}
			get {
				if (BCanvas.instance) {
					return BCanvas.instance.Paused;
				}
				return false;
			}
		}

		public static bool Visible {
			set {
				if (BCanvas.instance) {
					BCanvas.instance.Visible = value;
				}
			}
			get {
				if (BCanvas.instance) {
					return BCanvas.instance.Visible;
				}
				return false;
			}
		}

		public static int Count {
			get {
				if (BCanvas.instance) {
					return BCanvas.instance.Count;
				}
				return 0;
			}
		}

	}
}
