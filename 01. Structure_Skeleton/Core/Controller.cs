using ChristmasPastryShop.Core.Contracts;
using ChristmasPastryShop.Models.Booths;
using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Models.Cocktails;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Models.Delicacies;
using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories;
using ChristmasPastryShop.Repositories.Contracts;
using ChristmasPastryShop.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ChristmasPastryShop.Core
{
    public class Controller : IController
    {
        private IRepository<IBooth> booths;

        public Controller()
        {
            this.booths = new BoothRepository();
        }

        public string AddBooth(int capacity)
        {
            IBooth booth = new Booth(booths.Models.Count + 1, capacity);
            booths.AddModel(booth);
            return string.Format(OutputMessages.NewBoothAdded, booth.BoothId, capacity);
        }

        public string AddCocktail(int boothId, string cocktailTypeName, string cocktailName, string size)
        {
            if (cocktailTypeName != nameof(Hibernation) && cocktailTypeName != nameof(MulledWine))
            {
                return string.Format(OutputMessages.InvalidCocktailType, cocktailTypeName);
            }
            IBooth booth = booths.Models.FirstOrDefault(x => x.BoothId == boothId);
            if (size != "Large" && size != "Middle" && size != "Small")
            {
                return string.Format(OutputMessages.InvalidCocktailSize, size);
            }
            if (booth.CocktailMenu.Models.Any(d => d.Name == cocktailName && d.Size == size))
            {
                return string.Format(OutputMessages.CocktailAlreadyAdded, size, cocktailName);
            }
            ICocktail cocktail;
            if (cocktailTypeName == nameof(Hibernation))
            {
                cocktail = new Hibernation(cocktailName, size);
            }
            else
            {
                cocktail = new MulledWine(cocktailName, size);
            }
            booth.CocktailMenu.AddModel(cocktail);
            return string.Format(OutputMessages.NewCocktailAdded, size, cocktailName, cocktailTypeName);
        }

        public string AddDelicacy(int boothId, string delicacyTypeName, string delicacyName)
        {
            if (delicacyTypeName != nameof(Gingerbread) && delicacyTypeName != nameof(Stolen))
            {
                return string.Format(OutputMessages.InvalidDelicacyType, delicacyTypeName);
            }
            IBooth booth = booths.Models.FirstOrDefault(x => x.BoothId == boothId);
            if (booth.DelicacyMenu.Models.Any(d => d.Name == delicacyName))
            {
                return string.Format(OutputMessages.DelicacyAlreadyAdded, delicacyName);
            }
            IDelicacy delicacy;
            if (delicacyTypeName == nameof(Gingerbread))
            {
                delicacy = new Gingerbread(delicacyName);
            }
            else
            {
                delicacy = new Stolen(delicacyName);
            }
            booth.DelicacyMenu.AddModel(delicacy);
            return string.Format(OutputMessages.NewDelicacyAdded, delicacyTypeName, delicacyName);
        }

        public string BoothReport(int boothId)
        {
            IBooth booth = booths.Models.FirstOrDefault(b => b.BoothId == boothId);
            return booth.ToString();
        }

        public string LeaveBooth(int boothId)
        {
            IBooth booth = booths.Models.FirstOrDefault(b => b.BoothId == boothId);
            double currentBill = booth.CurrentBill;
            booth.Charge();
            booth.ChangeStatus();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Bill {currentBill:f2} lv");
            sb.AppendLine($"Booth {booth.BoothId} is now available!");
            return sb.ToString().TrimEnd();
        }

        public string ReserveBooth(int countOfPeople)
        {
            IBooth booth = booths.Models
                .OrderBy(b => b.Capacity)
                .ThenByDescending(b => b.BoothId)
                .FirstOrDefault(b => b.IsReserved == false && b.Capacity >= countOfPeople);
            if (booth == null)
            {
                return string.Format(OutputMessages.NoAvailableBooth, countOfPeople);
            }
            booth.ChangeStatus();
            return string.Format(OutputMessages.BoothReservedSuccessfully, booth.BoothId, countOfPeople);
        }

        public string TryOrder(int boothId, string order)
        {
            string[] orderTokens = order.Split('/', StringSplitOptions.RemoveEmptyEntries);
            string itemTypeName = orderTokens[0];
            string itemName = orderTokens[1];
            int countOfOrderedPieces = int.Parse(orderTokens[2]);
            string size = string.Empty;
            if (itemTypeName == nameof(Hibernation) || itemTypeName == nameof(MulledWine))
            {
                size = orderTokens[3];
            }
            IBooth booth = booths.Models.FirstOrDefault(b => b.BoothId == boothId);
            if (itemTypeName != nameof(Hibernation)
                && itemTypeName != nameof(MulledWine)
                && itemTypeName != nameof(Gingerbread)
                && itemTypeName != nameof(Stolen))
            {
                return string.Format(OutputMessages.NotRecognizedType, itemTypeName);
            }
            if (!booth.DelicacyMenu.Models.Any(d => d.Name == itemName) 
                && !booth.CocktailMenu.Models.Any(i => i.Name == itemName ))
            {
                return string.Format(OutputMessages.NotRecognizedItemName, itemTypeName, itemName);
            }
            ICocktail cocktail;
            IDelicacy delicacy;
            if (itemTypeName == nameof(Hibernation) || itemTypeName == nameof(MulledWine))
            {
                if (!booth.CocktailMenu.Models.Any(m => m.Size == size && itemName == m.Name))
                {
                    return string.Format(OutputMessages.CocktailStillNotAdded, size, itemName);
                }
                if (itemTypeName == nameof(MulledWine))
                {
                    cocktail = new MulledWine(itemName, size);
                }
                else
                {
                    cocktail = new Hibernation(itemName, size);
                }
                double price = cocktail.Price * countOfOrderedPieces;
                booth.UpdateCurrentBill(price);
                return string.Format(OutputMessages.SuccessfullyOrdered, boothId, countOfOrderedPieces, itemName);
            }
            else 
            {
                if (!booth.DelicacyMenu.Models.Any(m => m.GetType().Name == itemTypeName && itemName == m.Name))
                {
                    return string.Format(OutputMessages.DelicacyStillNotAdded, itemTypeName, itemName);
                }
                if (itemTypeName == nameof(Gingerbread))
                {
                    delicacy = new Gingerbread(itemName);
                }
                else
                {
                    delicacy = new Stolen(itemName); ;
                }
                double price = delicacy.Price * countOfOrderedPieces;
                booth.UpdateCurrentBill(price);
                return string.Format(OutputMessages.SuccessfullyOrdered, boothId, countOfOrderedPieces, itemName);
            }
        }
    }
}
