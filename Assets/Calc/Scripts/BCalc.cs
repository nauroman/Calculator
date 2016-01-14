//using System;
//using System.Threading;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Flashunity.PN;
using Flashunity.Logs;

namespace Flashunity.Calc
{
	public class BCalc : MonoBehaviour
	{
		public InputField inputField;

		//	double[] ar = new double[1000000];

		//	Thread t;

		void Start ()
		{
			//Log.Visible = true;
			// Add several simple tests
			AddTests ();

			//	Log.Visible = true;


			//		t = new Thread (new ThreadStart (Generate));
			
			/*
			t = new Thread (
				delegate() {
				while (true) {
					Generate ();
					Thread.Sleep (1000);
				}
			});
			*/

			/*
			t.Start ();

			for (int i = 0; i < 4; i++) {
				Console.WriteLine ("Main thread: Do some work.");
				Thread.Sleep (0);
			}
			*/

			//AddT ();

		}

		void AddTests ()
		{
			Test ("Hello!", "");
			
			Test ("", "");
			
			Test (" ", "");
			Test ("   ", "");
			
			
			Test ("0", "");
			
			Test ("1", "");
			
			Test ("0.00123", "");
			
			Test ("-1", "");
			
			Test ("-0.00123", "");
			
			Test ("-1+2", "1");
			Test ("-11+25", "14");
			
			Test ("(1+2)-2", "1");
			Test ("(1+2)*-2", "-6");
            
			Test ("0.1+0.2", "0.3");
			Test ("-0.1-0.2", "-0.3");
			
			Test ("(2+3)*(4+5)", "45");
			Test ("(2+3)(4+5)", "45");
			Test ("(21+32)*(-45-56)", "-5353");
			Test ("(21+32)(-45-56)", "-5353");
			
			
			Test ("-(0.01-0.02)", "0.01");
			
			Test ("-(0.01+0.02)", "-0.03");
			
			Test ("-(0.010-0.020)", "0.01");
			Test ("(123+1.23-456)(000+123-456)", "110479.41");
			
			Test ("(1)", "");
			Test ("(1)(2)", "2");
			Test ("(10)(20)", "200");
			Test ("(10)(20)(30)", "6000");
			Test ("(10)+(20)+(30)", "60");
			
			Test ("(10*20)+(30*-1)", "170");
			
			Test ("1000000000*20000000000", "2E+19");
			Test ("1/0", "Infinity");
			Test ("-1/0", "-Infinity");
			Test ("1/-0", "0");
			Test ("1/0+2/0", "Infinity");

		}

		void AddT ()
		{

		}

		void OnApplicationQuit ()
		{
//			t.Abort ();
		}

		/*
		void Generate ()
		{
			//	Log.Add ("Generate");

			Debug.Log ("Generate");

			var r = new System.Random ();
			for (int i=0; i<3; i++) {

				ar [i] = r.NextDouble ();
				Debug.Log (ar [i]);
				Thread.Sleep (0);
			}
		}
		*/

		void Test (string s, string answer)
		{

			var d = RPN.Calculate (s);

			var result = "";

			// We show a result if we have it and if result is not the input string
			if (IsResult (s, d)) {
				result = d.ToString ();
				s += " = " + result;
			}

			// Show test result in my Log Canvas. Yes, I made this Log Canvas myself :)
			if (result == answer) {
				Log.Add (s);
			} else {
				Log.Error (s + ", result: " + result + ", answer: " + answer);
			}

		}

		bool IsResult (string str, double d)
		{
			return d != double.MaxValue && d.ToString () != str;
		}

		// We call it after correct input field text
		public void OnValueChangeInputField ()
		{
			BInputField bInputField = inputField.GetComponent<BInputField> ();

			// Yes, It is a little bit dirty, but It is simple and it works :)
			if (!bInputField.doNotHandleEvent) {

				var str = inputField.text;
            
				// Getting the clear string - without the result (anwer) part
				str = BInputField.RemoveTextAfterEqual (str);

				str = str.Trim ();

				var d = RPN.Calculate (str);

				// We show a result if we have it and if result is not the input string
				if (IsResult (str, d)) {
					str += " = " + d.ToString ();
				}

				// We are preventing loop on value change 
				bInputField.doNotHandleEvent = true;
				inputField.text = str;
				bInputField.doNotHandleEvent = false;
			}
		}
	}
}
