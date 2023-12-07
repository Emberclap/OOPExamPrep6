using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChristmasPastryShop.Models.Cocktails
{
    public abstract class Cocktail : ICocktail
    {
        private string name;
        private string size;
        private double price;

        public Cocktail(string name, string size, double price)
        {
            this.Name = name;
            this.Size = size;
            this.Price = price;
        }

        public string Name
        {
            get => name;
            private set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.NameNullOrWhitespace);
                }
                name = value;
            }
        }

        public string Size
        {
            get
                => size;
            private set 
                => size = value;
        }

        public double Price
        {
            get => price;
            private set
            {
                if (this.size == "Large")
                {
                    this.price = value;
                }
                else if (this.size == "Middle")
                {
                    this.price = value / 3 * 2;
                }
                else
                {
                    this.price = value / 3;
                }
                
            }
        }
        public override string ToString()
        {
            return $"{this.Name} ({this.Size}) - {this.Price:f2} lv";

        }
    }
}
