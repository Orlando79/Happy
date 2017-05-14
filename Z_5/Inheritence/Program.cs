using System;


namespace Inheritance
{
	class Product
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
			
		public void Show()
		{
			Console.WriteLine (ShowText());
		}
		public virtual string ShowText()
		{
			return $"   Product {Name} cost {Price:c}.";
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

		public override double SumPrice()
		{
			return Volume * Price;
		}
	}
		
	class MainClass
	{
		static void Main(string[] args)
		{
			
		}
	}
}
