using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Interfaces
{
	class Product: IComparable<Product>
	{
		string name;
		double price;

		public string Name
		{
			get{
				return name;
			}
			set{
				name = value;
			}
		}
		public double Price
		{
			get{
				return price;
			}
			set{
				if (value >= 0) {
					price = value;
				} else {
					price = 0;
				}
			}
		}

		public Product(string _name,double _price)
		{
			Name=_name;
			Price=_price;
		}
		public Product()
		{
			Name="None";
			Price=0;
		}

		public int CompareTo(Product obj)
		{
			return Name.CompareTo(obj.Name);
		}
			
		public void Show()
		{
			Console.WriteLine (ShowText());
		}
		public virtual string ShowText()
		{
			return $"   Product {Name} cost {Price:c}.";
		}
		public virtual string PrintText ()
		{
			return $" Product {Name,7} {Price,7}";
		}

		public virtual double SumPrice()
		{
			return Price;
		}
	}
	class Food : Product
	{
		double weight;
		public double Weight
		{
			get{
				return weight;
			}
			set{
				if (value >= 0) {
					weight = value;
				} else {
					weight = 0;
				}
			}
		}

		public Food()
			:base()
		{
			Weight=0;
		}
		public Food(string _name,double _price,double _weight)
			:base(_name,_price)
		{
			Weight=_weight;
		}
			
		public override string ShowText()
		{
			return $"   Food {Name} cost {Price:c} and there is {Weight} kg of it.";
		}
		public override string PrintText ()
		{
			return $"    Food {Name,7} {Price,7} {Weight,9}";
		}

		public override double SumPrice()
		{
			return Weight * Price;
		}
	}
	class Beverage : Product
	{
		double volume;
		public double Volume
		{
			get{
				return volume;
			}
			set{
				if (value >= 0) {
					volume = value;
				} else {
					volume = 0;
				}
			}
		}

		public Beverage()
			:base()
		{
			Volume=0;
		}
		public Beverage(string _name,double _price,double _volume)
			:base(_name,_price)
		{
			Volume=_volume;
		}
			
		public override string ShowText()
		{
			return $"   Beverage {Name} cost {Price:c} and there is {Volume} litr of it.";
		}
		public override string PrintText ()
		{
			return $"Beverage {Name,7} {Price,7} {Volume,9}";
		}

		public override double SumPrice()
		{
			return Volume * Price;
		}
	}

	class NameComparer : IComparer<Product>
	{
		public int Compare(Product obj1, Product obj2)
		{
			return obj1.Name.CompareTo(obj2.Name);
		}
	}
	class PriceComparer : IComparer<Product>
	{
		public int Compare(Product obj1, Product obj2)
		{
			return obj1.Price.CompareTo(obj2.Price);
		}
	}
	class SumPriceComparer : IComparer<Product>
	{
		public int Compare(Product obj1, Product obj2)
		{
			return obj1.SumPrice().CompareTo(obj2.SumPrice());
		}
	}

	class Products:IEnumerable
	{
		Product[] arr;

		public int Length
		{
			get{
				return arr.Length;
			}
		}
		public Product this[int _index]
		{
			get{
				if (_index >= 0 && _index < Length) {
					return arr [_index];
				} else {
					return arr [0];
				} 
			}
			set{
				if (_index >= 0 && _index < Length) {
					arr [_index] = value;
				} else  {
					arr [_index] = value;
				} 
			}
		}

		public Products(string _path)
		{
			Input (_path);
		}
		public Products(params Product[] _arr)
		{
			arr = _arr;
		}
		public Products(Product _sample,int _quantity)
		{
			arr = new Product[_quantity];
			for (int i=0;i<_quantity;++i) {
				arr[i] = _sample;
			}
		}
			
		public IEnumerator GetEnumerator()
		{
			return arr.GetEnumerator ();
		}

		public void Show()
		{
			for (int i = 0; i < Length; ++i) {
				arr [i].Show ();
			}
		}

		public void Input(string _path)
		{
			var lines = File.ReadAllLines (_path);
			arr = new Product[lines.Length];
			for (int i = 0; i < lines.Length; ++i) {
				var line = lines [i].Split (new char[] {' '},StringSplitOptions.RemoveEmptyEntries);
				if (line [0] == "Food") {
					arr [i] = new Food (line [1], double.Parse (line [2]), double.Parse (line [3]));
				} else {
					arr [i] = new Beverage (line [1], double.Parse (line [2]), double.Parse (line [3]));
				}
			}
		}
		public void Output(string _path)
		{
			var lines = new string[Length];
			for (int i = 0; i < Length; ++i) {
				lines [i] = arr [i].PrintText ();
			}
			File.WriteAllLines (_path, lines);
		}

		public void SortByName()
		{
			Array.Sort (arr);
		}
		public void SortByPrice()
		{
			Array.Sort (arr,new PriceComparer());
		}
		public void SortBySumPrice()
		{
			Array.Sort (arr,new SumPriceComparer());
		}

		public void Find(string _name)
		{
			bool exist=false;
			for (int i = 0; i < Length; ++i) {
				if (arr [i].Name == _name) {
					exist=true;
					break;
				}
			}
			if (exist) {
				for (int i = 0; i < Length; ++i) {
					if (arr [i].Name == _name) {
						arr [i].Show ();
					}
				}
			} else {
				Console.WriteLine ("   There is no product with such name.");
			}
		}
	}

	class Program
	{
		public static void p1(ref Products arr)
		{
			Console.WriteLine("   Array of products:");
			string[] ma =  { "Type","Name","Price","Quantity"};
			Console.WriteLine("{0,8} {1,7} {2,7} {3,9}", ma[0], ma[1], ma[2],ma[3]);
			foreach (Product i in arr)
			{
				Console.WriteLine (i.PrintText());

			}
		}
		public static void p2(ref Products arr)
		{
			Console.Write("   Type name of product: ");
			string _name = Console.ReadLine();
			Console.WriteLine("   Informatiom on products with name {0}:", _name);
			arr.Find(_name);
		}
		public static void p3(ref Products arr)
		{
			arr.SortByName();
			Console.WriteLine("   Sorted by name.");
		}
		public static void p4(ref Products arr)
		{
			arr.SortByPrice();
			Console.WriteLine("   Sorted by price.");
		}
		public static void p5(ref Products arr)
		{
			arr.SortBySumPrice();
			Console.WriteLine("   Sorted by summary price.");
		}
		public static void p7(ref Products arr)
		{
			arr.Output("out.txt");
			Console.WriteLine("   Outputed in file.");
		}
		public static void Rules()
		{
			Console.WriteLine("   Codes:");
			Console.WriteLine("1 - show array of products");
			Console.WriteLine("2 - find product by name");
			Console.WriteLine("3 - sort array by name");
			Console.WriteLine("4 - sort array by price");
			Console.WriteLine("5 - sort array by summary price");
			Console.WriteLine("6 - clean screen");
			Console.WriteLine("7 - output in file");
			Console.WriteLine("default - exit");
		}
		public static void Menu(ref Products arr)
		{
			bool exit = false;
			Rules();
			while (!exit)
			{
				Console.Write("   Type code: ");
				int code = int.Parse(Console.ReadLine());
				switch (code)
				{
				case 1:
					p1(ref arr);
					break;
				case 2:
					p2(ref arr);
					break;
				case 3:
					p3(ref arr);
					break;
				case 4:
					p4(ref arr);
					break;
				case 5:
					p5(ref arr);
					break;
				case 6:
					Console.Clear();
					Rules();
					break;
				case 7:
					p7(ref arr);
					break;
				default:
					exit = true;
					break;
				}
				//Console.ReadKey();
			}
		}
		static void Main(string[] args)
		{
			var Shop = new Products ("in.txt");
			Menu (ref Shop);
		}
	}
}
