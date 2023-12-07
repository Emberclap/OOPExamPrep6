using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Models.Cocktails;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories;
using ChristmasPastryShop.Repositories.Contracts;
using ChristmasPastryShop.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Transactions;

namespace ChristmasPastryShop.Models.Booths
{
    public class Booth : IBooth
    {
        private int boothId;
        private int capacity;
        private IRepository<ICocktail> cocktailRepository;
        private IRepository<IDelicacy> delicacyRepository;

        public Booth(int boothId, int capacity)
        {
            this.boothId = boothId;
            this.capacity = capacity;
            this.cocktailRepository = new CocktailRepository();
            this.delicacyRepository = new DelicacyRepository();
            CurrentBill = 0;
            Turnover = 0;
            IsReserved = false;
        }

        public int BoothId
        {
            get => boothId;
            private set
            {
                boothId = value;
            }
        }

        public int Capacity
        {
            get => capacity;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.CapacityLessThanOne);
                }
                capacity = value;
            }
        }

        public IRepository<ICocktail> CocktailMenu => cocktailRepository;
        public IRepository<IDelicacy> DelicacyMenu => delicacyRepository;


        public double CurrentBill{get; private set;}

        public double Turnover{get; private set;}

        public bool IsReserved{get; private set;}

        public void ChangeStatus()
        {
            if (!IsReserved)
            {
                this.IsReserved = true;
            }
            else
            {
                this.IsReserved = false;
            }
        }

        public void Charge()
        {
            this.Turnover += this.CurrentBill;
            this.CurrentBill = 0;
        }

        public void UpdateCurrentBill(double amount)
        {
            this.CurrentBill += amount;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Booth: {this.BoothId}");
            sb.AppendLine($"Capacity: {this.Capacity}");
            sb.AppendLine($"Turnover: {this.Turnover:f2} lv");
            sb.AppendLine($"-Cocktail menu:");
            foreach (var cocktail in this.CocktailMenu.Models)
            {
                sb.AppendLine($"--{cocktail}");
            }
            sb.AppendLine($"-Delicacy menu:");
            foreach (var delicacy in this.DelicacyMenu.Models)
            {
                sb.AppendLine($"--{delicacy}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
