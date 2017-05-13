using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Collections
{
	
	class BusLine
	{
		int busnumber;
		string surname;
		int linenumber;
		double linelength;

		public int BusNumber {
			get {
				return busnumber;
			}
			set {
				if (value < 0 || value > 9999) {
					throw new Exception ("Non valid bus number.");
				} else {
					busnumber = value;
				}
			
			}
		}
		public string Surname
		{
			get{
				return surname;
			}
			set{
				surname=value;
			}
		}
		public int LineNumber
		{
			get{
				return linenumber;
			}
			set{
				if (value <= 0 || value > 100) {
					throw new Exception ("Non valid line number.");
				} else {
					linenumber = value;
				}
			}
		}
		public double LineLength
		{
			get{
				return linelength;
			}
			set{
				if (value <= 0 || value > 10000) {
					throw new Exception ("Non valid line length.");
				} else {
					linelength = value;
				}
			}
		}

		public BusLine(int _busnumber, string _surname, int _linenumber, double _linelength)
		{
			BusNumber = _busnumber;
			Surname = _surname;
			LineNumber = _linenumber;
			LineLength = _linelength;
		}
		public BusLine()
		{
			BusNumber = 9999;
			Surname = "None";
			LineNumber = 100;
			LineLength = 10000;
		}
		public void Show ()
		{

			Console.WriteLine ("  {0} is driving bus #{1:d4} on the line #{2} with length {3} km.",Surname,BusNumber,LineNumber,LineLength);
		}
		public void Print()
		{
			Console.WriteLine ("{0,12:d4} {1,17} {2,14} {3,14}",BusNumber,Surname,LineNumber,LineLength);
		}
	}
	class Program
	{
		public static void p1(ref Dictionary<int,BusLine> _obj)
		{
			
			if (File.Exists ("in.txt")) {
				var lines = File.ReadAllLines ("in.txt");
				for (int i = 0; i < lines.Length; ++i) {
					var line = lines [i].Split (new char [] {' '},StringSplitOptions.RemoveEmptyEntries);

					if (line.Length != 4) {
						throw new Exception ("Wrong number of split values while inputting.");
					}

					int _busnumber;
					int _linenumber;
					double _linelength;

					if (!int.TryParse (line [0],out _busnumber)) {
						throw new Exception ("Wrong format of bus number while inputting.");
					}
					if (!int.TryParse (line [2],out _linenumber)) {
						throw new Exception ("Wrong format of line number while inputting.");
					}
					if (!double.TryParse (line [3],out _linelength)) {
						throw new Exception ("Wrong format of line length while inputting.");
					}
					var buff = new BusLine (_busnumber,line[1],_linenumber,_linelength);
					_obj [buff.BusNumber] = buff;
				}
				Console.WriteLine ("  Dictionary is inputed.");
			} else {
				throw new Exception ("Given file does not exist.");
			}
		}
		public static void p2(ref Dictionary<int,BusLine> _obj)
		{
			if (_obj.Count != 0) {
				Console.WriteLine ("  Dictionary of bus lines:");
				Console.WriteLine ("  Bus number    Driver surname    Line number    Line length");
				foreach (var i in _obj.Keys) {
					_obj [i].Print ();
				}
			} else {
				Console.WriteLine ("  Dictionary is empty.");
			}
		}
		public static void p3(ref Dictionary<int,BusLine> _obj)
		{
			if (_obj.Count != 0) {
				Console.Write ("  Type bus number to delete: ");
				int _busnumber;
				if (!int.TryParse (Console.ReadLine (), out _busnumber)) {
					throw new Exception ("Wrong typed format of bus number while deleting.");
				}
				if (_obj.Remove (_busnumber)) {
					Console.WriteLine ("  Chosen bus line is removed.");
				} else {
					Console.WriteLine ("  Bus line with such bus does not exist.");
				}
			} else {
				Console.WriteLine ("  Dictionary is emty - impossible to delete anything.");
			}
		}
		public static void p4(ref Dictionary<int,BusLine> _obj)
		{
			Console.Write ("  Type bus line to change: ");
			var fullline = Console.ReadLine ();
			var line = fullline.Split (new char [] {' '},StringSplitOptions.RemoveEmptyEntries);

			if (line.Length != 4) {
				throw new Exception ("Wrong number of split values while changing.");
			}

			int _busnumber;
			int _linenumber;
			double _linelength;

			if (!int.TryParse (line [0],out _busnumber)) {
				throw new Exception ("Wrong format of bus number while changing.");
			}
			if (!int.TryParse (line [2],out _linenumber)) {
				throw new Exception ("Wrong format of line number while changing.");
			}
			if (!double.TryParse (line [3],out _linelength)) {
				throw new Exception ("Wrong format of line length while changing.");
			}
			var buff = new BusLine (_busnumber,line[1],_linenumber,_linelength);
		
			if (_obj.ContainsKey (buff.BusNumber)) {
				foreach (var i in _obj.Keys) {
					if (i == buff.BusNumber) {
						if (_obj [i].Surname == buff.Surname && _obj [i].LineNumber == buff.LineNumber && _obj [i].LineLength == buff.LineLength) {
							Console.WriteLine ("  Existing bus line is equal to typed.");
							break;
						} else {
							_obj [i] = buff;
							Console.WriteLine ("  Existing bus line is changed.");
							break;
						}
					}
				}
			} else {
				_obj [buff.BusNumber] = buff;
				Console.WriteLine ("  New bus line is added.");
			}

		}
		public static void p5(ref Dictionary<int,BusLine> _obj)
		{
			if (_obj.Count != 0) {
				Console.Write ("  Type bus number to seek: ");
				int _busnumber;
				if (!int.TryParse (Console.ReadLine (), out _busnumber)) {
					throw new Exception ("Invalid numeric format ( seeking by nus number ).");
				}
				if (_obj.ContainsKey (_busnumber)) {
					_obj [_busnumber].Show ();
				} else {
					Console.WriteLine ("  Bus line with such bus number does not exist.");
				}
			} else {
				Console.WriteLine ("  Dictionary is empty - impossible to seek.");
			}

		}
		public static void p6(ref Dictionary<int,BusLine> _obj)
		{
			if (_obj.Count != 0) {
				Console.Write ("  Type bus driver surname to seek: ");
				string _surname = Console.ReadLine ();
				int count = 0;
				foreach (var i in _obj.Values) {
					if (i.Surname == _surname) {
						count++;
					}
				}
				if (count == 0) {
					Console.WriteLine ("  Bus line with such driver surname does not exist.");
				} else if (count == 1) {
					Console.WriteLine ("  Bus line:");
				} else {
					Console.WriteLine ("  Bus lines:");
				}
				foreach (var i in _obj.Values) {
					if (i.Surname == _surname) {
						i.Show ();
					}
				}
			} else {
				Console.WriteLine ("  Dictionary is empty - impossible to seek.");
			}
		}
		public static void ShowRules()
		{
			Console.WriteLine("   Codes:");
			Console.WriteLine("1 - input bus lines from file");
			Console.WriteLine("2 - show dictionary of bus lines");
			Console.WriteLine("3 - delete bus line");
			Console.WriteLine("4 - change bus line");
			Console.WriteLine("5 - seek by bus number");
			Console.WriteLine("6 - seek by driver surname");
			Console.WriteLine("7 - clean screen");
			Console.WriteLine("default - exit");
		}
		public static void Menu()
		{
			Dictionary<int,BusLine> obj = new Dictionary<int, BusLine> ();
			bool exit = false;
			ShowRules ();
			while (!exit) {
				Console.Write ("  Type code: ");
				int code;
				if (!int.TryParse(Console.ReadLine(),out code)) {
					throw new Exception ("Invalid numeric format.");
				}
				switch (code)
				{
				case 1:
					p1 (ref obj);
					break;
				case 2:
					p2(ref obj);
					break;
				case 3:
					p3(ref obj);
					break;
				case 4:
					p4(ref obj);
					break;
				case 5:
					p5(ref obj);
					break;
				case 7:
					Console.Clear();
					ShowRules();
					break;
				case 6:
					p6(ref obj);
					break;
				default:
					exit = true;
					break;
				}
			}

		}
		public static void Main(string[] args)
		{
			try{
				Menu ();
			}
			catch(Exception ex) {
				Console.WriteLine ("  Error: "+ex.Message);
			}
		}
	}
}
