using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Serialization
{
	interface INameAndCopy<T> where T:Product
	{
		string GetName ();
		T GetCopy ();
	}
	interface IShow<T> where T:Product
	{
		void Show();
		string ShowText ();
	}

	class MyException:Exception
	{
		public MyException(string _message)
			:base(_message)
		{

		}
	}

	[XmlType("Product")] 
	[XmlInclude(typeof(Food)), XmlInclude(typeof(Beverage))]  
	public class Product: IComparable<Product>,INameAndCopy<Product>
	{
		string name;
		double price;

		[XmlElement("Name")]
		public string Name
		{
			get{
				return name;
			}
			set{
				name = value;
			}
		}
		[XmlElement("Price")]
		public double Price
		{
			get{
				return price;
			}
			set{
				if (value >= 0) {
					price = value;
				} else {
					throw new MyException ("Price value is out of range.");
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

		public string GetName ()
		{
			return Name;
		}
		public virtual Product GetCopy()
		{
			return new Product(Name,Price);
		}
	}

	[XmlType("Food")] 
	public class Food : Product
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
					throw new MyException ("Weight value is out of range.");
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

		public override Product GetCopy()
		{
			return new Food(Name,Price,Weight);
		}
	}

	[XmlType("Beverage")] 
	public class Beverage : Product
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
					throw new MyException("Volume value is out of range.");
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

		public override Product GetCopy()
		{
			return new Beverage(Name,Price,Volume);
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

	[XmlRoot("Products")]
	[XmlInclude(typeof(Product))] // include type class Person
	public class Products:IEnumerable<Product>
	{
		
		[XmlArray("List")]
		[XmlArrayItem("item")]
		public List<Product> list;

		public Product this[int _index]
		{
			get{
				return list [_index];
			}
		}

		public int Length
		{
			get{
				return list.Count;
			}
		}
		public void Add(Product _item)
		{
			list.Add (_item);
		}
		public void Add(Object _item)
		{
			list.Add ((Product)_item);
		}

		public Products(List<Product> _list)
		{
			list = new List<Product> (_list);
		}
		public Products(int _capacity)
		{
			if (_capacity < 0) {
				throw new MyException ("Typed capacity of list is out of range.");
			}
			list = new List<Product> (_capacity);
		}
		public Products()
		{
			list = new List<Product> (10);
		}
		public Products(string _path)
		{
			InputXML (_path);
		}

		public void SortByName()
		{
			list.Sort ();
		}
		public void SortByPrice()
		{
			list.Sort (new PriceComparer());
		}
		public void SortBySumPrice()
		{
			list.Sort (new SumPriceComparer());
		}

		public void Show()
		{
			foreach (var i in list) {
				i.Show ();
			}
		}

		public void Find(string _name)
		{
			bool exist=false;
			foreach (var i in list) {
				if (i.Name == _name) {
					exist=true;
					break;
				}
			}
			if (exist) {
				foreach (var i in list) {
					if (i.Name == _name) {
						i.Show ();
					}
				}
			} else {
				Console.WriteLine ("   There is no product with such name.");
			}
		}

		public void Input(string _path)
		{
			if (!File.Exists (_path)) {
				throw new MyException ("File does not exist (inputting).");
			}
			var lines = File.ReadAllLines (_path);
			list = new List<Product> (lines.Length );
			for (int i = 0; i < lines.Length; ++i) {
				var line = lines [i].Split (new char[] {' '},StringSplitOptions.RemoveEmptyEntries);
				if (line.Length != 4) {
					throw new MyException ("Wrong format while inputting.");
				}
				double _price;
				double _quantity;
				if(!double.TryParse(line[2],out _price))
				{
					throw new MyException ("Wrong format of price while inputting.");
				}
				if(!double.TryParse(line[3],out _quantity))
				{
					throw new MyException ("Wrong format of quantity while inputting.");
				}
				if (line [0] == "Food") {
					Add (new Food (line [1], _price, _quantity));
				} else if (line [0] == "Beverage") {
					Add (new Beverage (line [1], _price, _quantity));
				} else {
					throw new MyException ("Wrong name of type while inputting.");
				}
			}
		}
		public void Output(string _path)
		{
			if (!File.Exists (_path)) {
				throw new MyException ("File does not exist (outputting).");
			}
			var lines = new string[Length];
			int count = 0;
			foreach (var i in list) {
				lines [count] = i.PrintText ();
				++count;
			}
			File.WriteAllLines (_path, lines);
		}


		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator<Product>)list.GetEnumerator ();
		}
		IEnumerator<Product> IEnumerable<Product>.GetEnumerator()
		{
			return (IEnumerator<Product>)list.GetEnumerator ();
		}

		public IEnumerator OnlyFood
		{
			get {
				foreach (var i in list) {
					if (i is Food) {
						yield return i;
					}
				}
			}
		}
		public IEnumerator OnlyBeverage
		{
			get {
				foreach (var i in list) {
					if (i is Beverage) {
						yield return i;
					}
				}
			}
		}

		public IEnumerator PriceLowerThen(double _price)
		{
			foreach (var i in list) {
				if (i.Price <= _price) {
					yield return i;
				}
			}
		}

		public void OutputXML(string _path)
		{
			if (!File.Exists ("out.xml")) {
				throw new MyException ("File does not exist while serializing.");
			}
			var ser=new XmlSerializer(typeof(Products),new Type[]{ typeof(Product), typeof(Food), typeof(Beverage) });
			using(Stream stream=new FileStream("out.xml",FileMode.Create,FileAccess.Write ))
			{
				ser.Serialize(stream,this);
			}
		}
		public void InputXML(string _path)
		{
			if (!File.Exists ("in.xml")) {
				throw new MyException ("File does not exist while deserializing.");
			}
			var ser=new XmlSerializer(typeof(Products),new Type[]{ typeof(Product), typeof(Food), typeof(Beverage) });
			using(Stream stream=new FileStream("in.xml",FileMode.OpenOrCreate))
			{
				list = new List<Product> ();
				list=((Products)ser.Deserialize(stream)).list;
			}
		}
	}

	class Program
	{
		public static void p1(ref Products list)
		{
			if (list.Length != 0) {
				Console.WriteLine ("   List of products:");
				string[] ma = { "Type", "Name", "Price", "Quantity" };
				Console.WriteLine ("{0,8} {1,7} {2,7} {3,9}", ma [0], ma [1], ma [2], ma [3]);
				foreach (Product i in list) {
					Console.WriteLine (i.PrintText ());
				}
			} else {
				Console.WriteLine ("   List is empty.");
			}
		}
		public static void p2(ref Products list)
		{
			if (list.Length != 0) {
				Console.Write("   Type name of product: ");
				string _name = Console.ReadLine();
				Console.WriteLine("   Informatiom on products with name {0}:", _name);
				list.Find(_name);
			} else {
				Console.WriteLine ("   List is empty.It is impossible to find smth.");
			}
		}
		public static void p3(ref Products list)
		{
			if (list.Length != 0) {
				list.SortByName();
				Console.WriteLine("   Sorted by name.");
			} else {
				Console.WriteLine ("   List is empty.It is impossible to sort");
			}
		}
		public static void p4(ref Products list)
		{
			if (list.Length != 0) {
				list.SortByPrice();
				Console.WriteLine("   Sorted by price.");
			} else {
				Console.WriteLine ("   List is empty.It is impossible to sort.");
			}

		}
		public static void p5(ref Products list)
		{
			if (list.Length != 0) {
				list.SortBySumPrice();
				Console.WriteLine("   Sorted by summary price.");
			} else {
				Console.WriteLine ("   List is empty.It is impossible to sort.");
			}
		}
		public static void p7(ref Products list)
		{
			list.OutputXML("out.xml");
			Console.WriteLine("   Deserialized.");
		}
		public static void p8(ref Products list)
		{
			Console.Write ("   Type product to add: ");
			var _product = Console.ReadLine ().Split(new char[]{' '},StringSplitOptions.RemoveEmptyEntries);
			if (_product.Length != 4) {
				throw new MyException ("Wrong format of typed product.");
			}

			double _price;
			double _quantity;
			if(!double.TryParse(_product[2],out _price))
			{
				throw new MyException ("Wrong format of price while adding.");
			}
			if(!double.TryParse(_product[3],out _quantity))
			{
				throw new MyException ("Wrong format of quantity while adding.");
			}
			if (_product[0] == "Food") {
				list.Add (new Food (_product[1], _price, _quantity));
			} else if (_product[0] == "Beverage") {
				list.Add (new Beverage (_product[1], _price, _quantity));
			} else {
				throw new MyException ("Wrong name of type while adding.");
			}
			Console.WriteLine("   Added to list.");
		}
		public static void Rules()
		{
			Console.WriteLine("   Codes:");
			Console.WriteLine("1 - show list of products");
			Console.WriteLine("2 - find product by name");
			Console.WriteLine("3 - sort list by name");
			Console.WriteLine("4 - sort list by price");
			Console.WriteLine("5 - sort list by summary price");
			Console.WriteLine("6 - clean screen");
			Console.WriteLine("7 - serialize");
			Console.WriteLine("8 - add product");
			Console.WriteLine("default - exit");
		}
		public static void Menu(ref Products list)
		{
			bool exit = false;
			Rules();
			while (!exit)
			{
				Console.Write("   Type code: ");
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
				case 7:
					p7(ref list);
					break;
				case 8:
					p8(ref list);
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
				var a =new Products("in.xml");

				Menu(ref a);
				//a.OutputXML("out.xml");
			}
			catch(InvalidOperationException ex) {
				Console.WriteLine ("   Error: Error in xml file.");
			}
			catch(MyException ex) {
				Console.WriteLine ("   Error: "+ex.Message);
			}
			catch(Exception ex) {
				Console.WriteLine ("   Error: "+ex.Message+"  //not my exception");
			}

		}
	}
}