using System;
using Dietary.Models.NewProducts;
using Xunit;

namespace Dietary.Tests.NewProducts
{
    public class OldProductTest
    {
        [Fact]
        public void priceCannotBeNull()
        {
            Assert.Throws<InvalidOperationException>(() => Price.Of(null));
        }

        [Fact]
        public void canIncrementCounterIfPriceIsPositive()
        {
            //given
            OldProduct p = ProductWithPriceAndCounter(10, 10);

            //when
            p.IncrementCounter();

            //then
            Assert.Equal(11, p.GetCounter());
        }

        [Fact]
        public void cannotIncrementCounterIfPriceIsNotPositive()
        {
            //given
            OldProduct p = ProductWithPriceAndCounter(0, 10);

            //expect
            Assert.Throws<InvalidOperationException>(() => p.IncrementCounter());
        }

        [Fact]
        public void canDecrementCounterIfPriceIsPositive()
        {
            //given
            OldProduct p = ProductWithPriceAndCounter(10, 10);
        
            //when
            p.DecrementCounter();
        
            //then
            Assert.Equal(9, p.GetCounter());
        }
        
        [Fact]
        public void cannotDecrementCounterIfPriceIsNotPositive()
        {
            //given
            OldProduct p = ProductWithPriceAndCounter(0, 0);
        
            //expect
            Assert.Throws<InvalidOperationException>(() => p.DecrementCounter());
        }
        
        [Fact]
        public void canChangePriceIfCounterIsPositive()
        {
            //given
            OldProduct p = ProductWithPriceAndCounter(0, 10);
        
            //when
            p.ChangePriceTo(10);
        
            //then
            Assert.Equal(10, p.GetPrice());
        }
        
        [Fact]
        public void cannotChangePriceIfCounterIsNotPositive()
        {
            //given
            OldProduct p = ProductWithPriceAndCounter(0, 0);
        
            //when
            p.ChangePriceTo(10);
        
            //then
            Assert.Equal(0, p.GetPrice());
        }
        
        [Fact]
        public void canFormatDescription()
        {
            //expect
            Assert.Equal("short *** long", ProductWithDesc("short", "long").FormatDesc());
            Assert.Equal("", ProductWithDesc("short", "").FormatDesc());
            Assert.Equal("", ProductWithDesc("", "long2").FormatDesc());
        }
        
        [Fact]
        public void canChangeCharInDescription()
        {
            //given
            var oldProduct = ProductWithDesc("short", "long");
        
            //when
            oldProduct.ReplaceCharFromDesc('s', 'z');
        
            //expect
            Assert.Equal("zhort *** long", oldProduct.FormatDesc());
        }

        private static OldProduct ProductWithPriceAndCounter(decimal? price, int counter)
            => new OldProduct(price, "desc", "longDesc", counter);

        private static OldProduct ProductWithDesc(String desc, String longDesc)
            => new OldProduct(10, desc, longDesc, 10);
    }
}