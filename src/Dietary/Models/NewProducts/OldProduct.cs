using System;

namespace Dietary.Models.NewProducts
{
    public class OldProduct
    {
        public Guid SerialNumber { get; private set; } = Guid.NewGuid();
        public decimal? Price { get; private set; }
        public string Desc { get; private set; }
        public string LongDesc { get; private set; }
        public int? Counter { get; private set; }

        public OldProduct(decimal? price, string desc, string longDesc, int? counter)
        {
            Price = price;
            Desc = desc;
            LongDesc = longDesc;
            Counter = counter;
        }

        public void DecrementCounter()
        {
            if (Price is not null && Price > 0)
            {
                if (Counter == null)
                {
                    throw new InvalidOperationException("null counter");
                }

                Counter = Counter - 1;
                if (Counter < 0)
                {
                    throw new InvalidOperationException("Negative counter");
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid price");
            }
        }

        public void IncrementCounter()
        {
            if (Price is not null && Price > 0)
            {
                if (Counter == null)
                {
                    throw new InvalidOperationException("null counter");
                }

                if (Counter + 1 < 0)
                {
                    throw new InvalidOperationException("Negative counter");
                }

                Counter = Counter + 1;

            }
            else
            {
                throw new InvalidOperationException("Invalid price");
            }
        }

        public void ChangePriceTo(decimal? newPrice)
        {
            if (Counter == null)
            {
                throw new InvalidOperationException("null counter");
            }

            if (Counter > 0)
            {
                if (newPrice == null)
                {
                    throw new InvalidOperationException("new price null");
                }

                Price = newPrice;
            }
        }

        public void ReplaceCharFromDesc(string charToReplace, string replaceWith)
        {
            if (string.IsNullOrWhiteSpace(LongDesc) || string.IsNullOrWhiteSpace(Desc))
            {
                throw new InvalidOperationException("null or empty desc");
            }

            LongDesc = LongDesc.Replace(charToReplace, replaceWith);
            Desc = Desc.Replace(charToReplace, replaceWith);
        }

        public string FormatDesc()
        {
            if (string.IsNullOrWhiteSpace(LongDesc) || string.IsNullOrWhiteSpace(Desc))
            {
                return "";
            }

            return Desc + " *** " + LongDesc;
        }
    }
}
