using System;
using System.Collections.Generic;
using Flashunity.Logs;

namespace Flashunity.PN
{
	/** Simple Reverse Polish Notation
	 */ 
	class RPN
	{
		/** returns double.MaxValue if no result. It is not safe - you can use only these symbols 0123456789.()+-/*
		 */ 
		public static double Calculate (string str)
		{
			if (str.Length < 3) {
				return double.MaxValue;
			}

			var list = ToRPN (str);

			if (list.Count < 3) {
				return double.MaxValue;
			}

			return CalculateRPN (list);
		}

		/** 
		 * remove spaces
		 * replace -3 by -1*3
		 * replace (1+2)-2 by (1+2)+-1*2
		 * replace 4(2+3) by 4*(2+3)
		 * replace (2+3)4 by (2+3)*4
		 * replace (2+3)(4+5) by (2+3)*(4+5)
		 */ 
		static string CorrectString (string str)
		{
			str = str.Replace (" ", "");
			str = str.Replace ("-", "-1*");
			str = str.Replace (")-1*", ")+-1*");
			str = str.Replace ("()", "");

			//	Log.Add ("1: " + str);

			for (int i=0; i<str.Length; i++) {

				// correct 4(2+3) to 4*(2+3)
				if (char.IsDigit (str [i]) && i < str.Length - 1 && str [i + 1] == '(') {
					str = str.Insert (i + 1, "*");
					continue;
				}

				// correct (2+3)4 to (2+3)*4
				if (str [i] == ')' && i < str.Length - 1 && char.IsDigit (str [i + 1])) {
					str = str.Insert (i + 1, "*");
					continue;
				}

				// correct (2+3)(4+5) to (2+3)*(4+5)
				if (i < str.Length - 2 && str.Substring (i, 2) == ")(") {
					str = str.Insert (i + 1, "*");
					continue;
				}
			}

			//	Log.Add ("2: " + str);


			return str;
		}

		/** return list of strings in reverce polish notation
		 * for 2+3 result is list of 2,3,+
		 */ 
		static IList<string> ToRPN (string str)
		{
			Stack<char> operationsStack = new Stack<char> ();
			
			char lastOperation;
			
			var list = new List<string> ();

			str = CorrectString (str);

			var strDouble = "";

			for (int i = 0; i < str.Length; i++) {

				if (strDouble.Length == 0 && i < str.Length - 3) {
					if (str.Substring (i, 3) == "-1*") {
						strDouble = "-1";
						i++;

						if (i >= str.Length - 1) {
							list.Add (strDouble);
						}
						continue;
					}
				}
	

				var c = str [i];

				if (char.IsDigit (c) && (i < (str.Length - 1) && str [i + 1] == '(')) {

				}

				if (IsPartOfDigit (c)) {
					strDouble += c;

					if (i == str.Length - 1) {
						list.Add (strDouble);
					}
					continue;
				}

				if (strDouble.Length > 0) {

					list.Add (strDouble);
					strDouble = "";
				}

				if (IsOperation (c)) {
					if (operationsStack.Count > 0) {
						lastOperation = operationsStack.Peek ();					
					} else {
						operationsStack.Push (c);
						continue;
					}
					
					if (GetOperationPriority (lastOperation) < GetOperationPriority (c)) {
						operationsStack.Push (c);
						continue;
					} else {
						list.Add (operationsStack.Pop ().ToString ());
						operationsStack.Push (c);
						continue;
					}
				}
				
				if (c.Equals ('(')) {
					operationsStack.Push (c);
					continue;
				}
				
				if (c.Equals (')')) {

					if (operationsStack.Count == 0) {
						return new List<string> ();
					}

					while (operationsStack.Count > 0 && operationsStack.Peek() != '(') {
						list.Add (operationsStack.Pop ().ToString ());
					}

					if (operationsStack.Count > 0) {
						operationsStack.Pop ();
					} else {
						return new List<string> ();
					}
				}
			}
			
			while (operationsStack.Count != 0) {
				list.Add (operationsStack.Pop ().ToString ());
			}

			return list;
		}

		/**
		 * returns double.MaxValue if no result
		 */ 
		static double CalculateRPN (IList<string> list)
		{
			Stack<double> numbersStack = new Stack<double> ();
			
			double op1, op2;

			foreach (var i in list) {				
				//	Log.Add ("i: " + i);
			}

			foreach (var i in list) {
				if (i.Length == 1 && IsOperation (i [0])) {

					if (numbersStack.Count < 2) {
						return double.MaxValue;
					}

					op2 = numbersStack.Pop ();
					//Log.Add ("op2:" + op2);

					op1 = numbersStack.Pop ();
					//Log.Add ("op1:" + op1);

					numbersStack.Push (ApplyOperation (i [0], op1, op2));

				} else {

					double d;

					if (double.TryParse (i, out d)) {
						numbersStack.Push (d);
					} else {
						return double.MaxValue;
					}

				}

			}

			return numbersStack.Pop ();
		}

		static bool IsPartOfDigit (char c)
		{
			return Char.IsDigit (c) || c == '.' || c == ',';
		}

		static bool IsOperation (char c)
		{
			return c == '+' || c == '-' || c == '*' || c == '/';
		}

		/** A few operaions only, but It is updatable
		 */ 
		static int GetOperationPriority (char c)
		{
			switch (c) {
			case '+':
				return 1;
			case '-':
				return 1;
			case '*':
				return 2;
			case '/':
				return 2;
			default:
				return 0;
			}
		}
		
		static double ApplyOperation (char operation, double op1, double op2)
		{
			switch (operation) {
			case '+':
				return (op1 + op2);
			case '-':
				return (op1 - op2);
			case '*':
				return (op1 * op2);
			case '/':
				return (op1 / op2);
			default:
				return 0;
			}
		}
	}
}