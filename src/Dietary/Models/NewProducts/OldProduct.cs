using System;
using System.ComponentModel.DataAnnotations;

namespace Dietary.Models.NewProducts
{
    public class OldProduct
    {
        private Price _price;
        private Description _desc;
        private Counter _counter;
        
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        private OldProduct()
        {
        }

        public OldProduct(decimal? price, string desc, string longDesc, int counter)
        {
            _price = Price.Of(price);
            _desc = new Description(desc, longDesc);
            _counter = new Counter(counter);
        }

        public void DecrementCounter()
        {
            if (_price.IsNotZero())
            {
                _counter = _counter.Decrement();
            }
            else
            {
                throw new InvalidOperationException("price is zero");
            }
        }

        public void IncrementCounter()
        {
            if (_price.IsNotZero())
            {
                _counter = _counter.Increment();
            }
            else
            {
                throw new InvalidOperationException("price is zero");
            }
        }

        public void ChangePriceTo(decimal? price)
        {
            if (_counter.HasAny())
            {
                _price = Price.Of(price);
            }
        }

        [Obsolete]
        // not public
        public void ReplaceCharFromDesc(char charToReplace, char replaceWith)
        {
            _desc = _desc.Replace(charToReplace, replaceWith);
        }

        [Obsolete]
        // not public
        public string FormatDesc() => _desc.Formatted();

        public decimal GetPrice() => _price.GetAsDecimal();

        public int GetCounter() => _counter.GetIntValue();
    }

    public class Price
    {
        private readonly decimal? _price;

        public static Price Of(decimal? value) => new Price(value);

        private Price()
        {
        }
        
        private Price(decimal? price)
        {
            if (price is null || price < 0)
            {
                throw new InvalidOperationException($"Cannot have negative price: {price}");
            }

            _price = price;
        }

        public bool IsNotZero() => _price != 0;

        public decimal GetAsDecimal() => _price.Value;
    }

    public class Description
    {
        private readonly string _desc;
        private readonly string _longDesc;

        private Description()
        {
        }
        
        public Description(string desc, string longDesc)
        {
            _desc = desc ?? throw new InvalidOperationException("Cannot have a null description");
            _longDesc = longDesc ?? throw new InvalidOperationException("Cannot have null long description");
        }

        public string Formatted()
        {
            if (string.IsNullOrWhiteSpace(_desc) || string.IsNullOrWhiteSpace(_longDesc))
            {
                return "";
            }

            return $"{_desc} *** {_longDesc}";
        }

        public Description Replace(char charToReplace, char replaceWith)
            => new Description(_desc.Replace(charToReplace, replaceWith),
                _longDesc.Replace(charToReplace, replaceWith));
    }

    public class Counter
    {
        private readonly int _counter;

        public static Counter Zero() => new Counter(0);

        private Counter()
        {
        }
        
        public Counter(int counter)
        {
            if (counter < 0)
            {
                throw new InvalidOperationException($"Cannot have negative counter: {counter}");
            }

            _counter = counter;
        }

        public int GetIntValue() => _counter;

        public Counter Increment() => new Counter(_counter + 1);

        public Counter Decrement() => new Counter(_counter - 1);

        public bool HasAny() => _counter > 0;
    }
}
