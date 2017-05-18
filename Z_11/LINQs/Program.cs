using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LINQs
{
	class MyException:Exception
	{
		public MyException(string _message)
			:base(_message)
		{
			//Message = _message;
		}
		
	}
	class ZNO
	{
		string surname;
		int school_number;

		int math_score;
		int ul_score;
		int history_score;

		public string Surname
		{
			get{
				return surname;
			}
			set{
				surname = value;
			}
		}
		public int School_Number
		{
			get{
				return school_number;
			}
			set{
				if (value < 1) {
					throw new MyException ("  School number out of range.");
				}
				school_number = value;
			}
		}

		public int Math_Score
		{
			get{
				return math_score;
			}
			set{
				if (value < 100 || value > 200) {
					throw new MyException ("  Math score is out of range.");
				}
				math_score = value;
			}
		}
		public int UL_Score
		{
			get{
				return ul_score;
			}
			set{
				if (value < 100 || value > 200) {
					throw new MyException ("  Ukrainian language score is out of range.");
				}
				ul_score = value;
			}
		}
		public int History_Score
		{
			get{
				return history_score;
			}
			set{
				if (value < 100 || value > 200) {
					throw new MyException ("  History score is out of range.");
				}
				history_score = value;
			}
		}

		public ZNO()
		{
			Surname="None";
			School_Number = int.MaxValue;

			Math_Score = 100;
			UL_Score = 100;
			History_Score = 100;
		}
		public ZNO(string _surname,int _school_number, int _math_score,int _ul_score,int _history_score)
		{
			Surname=_surname;
			School_Number = _school_number;

			Math_Score = _math_score;
			UL_Score = _ul_score;
			History_Score = _history_score;
		}
		public ZNO(string _line)
		{
			Input (_line);
		}

		public void Input(string _line)
		{
			var line = _line.Split (new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (line.Length != 5) {
				throw new MyException ("  Wrong format of inputted data.");
			}

			int _school_number;
			int _math_score;
			int _ul_score;
			int _history_score;

			Surname = line [0];
			if (!int.TryParse (line [1],out _school_number)) {
				throw new MyException ("  School number is in wrong format while inputting.");
			}
			if (!int.TryParse (line [2],out _math_score)) {
				throw new MyException ("  Math score is in wrong format while inputting.");
			}
			if (!int.TryParse (line [3],out _ul_score)) {
				throw new MyException ("  Ukrainian language score is in wrong format while inputting.");
			}
			if (!int.TryParse (line [4],out _history_score)) {
				throw new MyException ("  History score is in wrong format while inputting.");
			}

			School_Number = _school_number;
			Math_Score = _math_score;
			UL_Score = _ul_score;
			History_Score = _history_score;

		}
		public string Output()
		{
			return string.Format ("{0,10} {1,15} {2,12} {3,26} {4,15}",Surname,School_Number,Math_Score,UL_Score,History_Score);
		}

		public static void InputList(ref List<ZNO> _list,string _path)
		{
			if (!File.Exists (_path)) {
				throw new MyException ("  File does not exist while inputting from file.");
			}
			var lines = File.ReadAllLines (_path);
			if (lines.Length == 0) {
				throw new MyException ("  File is empty while inputting from file.");
			}
			for (int i = 0; i < lines.Length; ++i) {
				_list.Add (new ZNO(lines[i]));
			}
		}

	}
	class MainClass
	{


		public static void p1(ref List<ZNO> list)
		{
			if (list.Count != 0) {
				Console.WriteLine ("  List of ZNO results:");
				var words = new string[] { "Surname", "School number", "Math score", "Ukrainian language score", "History score" };
				Console.WriteLine ("{0,10} {1,15} {2,12} {3,26} {4,15}", words [0], words [1], words [2], words [3], words [4]);
				foreach (var i in list) {
					Console.WriteLine (i.Output());
				}
			} else {
				Console.WriteLine ("  List of ZNO results is empty.");
			}

		}
		public static void p2(ref List<ZNO> list)
		{
			if (list.Count != 0) {
				Console.WriteLine("  Lowest summary score is equal: {0}",list.Min (i=>i.History_Score+i.Math_Score+i.UL_Score));

			} else {
				Console.WriteLine ("  List of ZNO results is empty.");
			}
		}
		public static void p3(ref List<ZNO> list)
		{
			if (list.Count != 0) {
				double av = list.Average (j => j.History_Score + j.Math_Score + j.UL_Score);
				Console.WriteLine ("  List of ZNO results with lowest summary score:");
				var l=from i in list
					where i.History_Score+i.Math_Score+i.UL_Score<= av
					orderby i.Surname descending
					select i;
				var words = new string[] { "Surname", "School number", "Math score", "Ukrainian language score", "History score" };
				Console.WriteLine ("{0,10} {1,15} {2,12} {3,26} {4,15}", words [0], words [1], words [2], words [3], words [4]);

				foreach(var i in l)
				{
					Console.WriteLine(i.Output());
				}
			} else {
				Console.WriteLine ("  List of ZNO results is empty.");
			}
		}
		public static void p4(ref List<ZNO> list)
		{
			if (list.Count != 0) {
				Console.WriteLine ("  List of ZNO results sorted by surname:");
				var words = new string[] { "Surname", "School number", "Math score", "Ukrainian language score", "History score" };
				Console.WriteLine ("{0,10} {1,15} {2,12} {3,26} {4,15}", words [0], words [1], words [2], words [3], words [4]);

				var l=from i in list
					orderby i.Surname
					select i;
				foreach(var i in l)
				{
					Console.WriteLine(i.Output());
				}
			} else {
				Console.WriteLine ("  List of ZNO results is empty.");
			}
		}
		public static void p5(ref List<ZNO> list)
		{
			if (list.Count != 0) {
				Console.WriteLine ("  List of ZNO results sorted by summary score:");
				var words = new string[] { "Surname", "School number", "Math score", "Ukrainian language score", "History score" };
				Console.WriteLine ("{0,10} {1,15} {2,12} {3,26} {4,15}", words [0], words [1], words [2], words [3], words [4]);

				var l=from i in list
					orderby i.History_Score+i.Math_Score+i.UL_Score
					select i;
				foreach(var i in l)
				{
					Console.WriteLine(i.Output());
				}
			} else {
				Console.WriteLine ("  List of ZNO results is empty.");
			}
		}
		public static void Rules()
		{
			Console.WriteLine("   Codes:");
			Console.WriteLine("1 - show all ZNO results");
			Console.WriteLine("2 - print minimum summary score");
			Console.WriteLine("3 - print all lowest ZNO results");
			Console.WriteLine("4 - print list sorted by surname");
			Console.WriteLine("5 - print list srted by summary score");
			Console.WriteLine("6 - clean screen");
			Console.WriteLine("default - exit");
		}
		public static void Menu(ref List<ZNO> list)
		{
			bool exit = false;
			Rules();
			while (!exit)
			{
				Console.Write("  Type code: ");
				int code;
				if (!int.TryParse (Console.ReadLine (),out code)) {
					throw new MyException ("Typed code is not in correct format.");
				}
				switch (code)
				{
				case 1:
					p1(ref list);
					break;
				case 2:
					p2(ref list);
					break;
				case 3:
					p3(ref list);
					break;
				case 4:
					p4(ref list);
					break;
				case 5:
					p5(ref list);
					break;
				case 6:
					Console.Clear();
					Rules();
					break;
				default:
					exit = true;
					break;
				}
			}
		}


		public static void Main (string[] args)
		{
			try{
			var ZNOresults = new List<ZNO> ();
				ZNO.InputList(ref ZNOresults,"in.txt");
				Menu(ref ZNOresults);
			}
			catch(Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}
	}
}
