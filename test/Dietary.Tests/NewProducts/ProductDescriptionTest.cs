using Dietary.Models.NewProducts;
using Xunit;

namespace Dietary.Tests.NewProducts
{
    public class ProductDescriptionTest
    {
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
            OldProductDescription oldProduct = ProductWithDesc("short", "long");

            //when
            oldProduct.ReplaceCharFromDesc('s', 'z');

            //expect
            Assert.Equal("zhort *** long", oldProduct.FormatDesc());
        }

        private static OldProductDescription ProductWithDesc(string desc, string longDesc)
            => new OldProductDescription(new OldProduct(10, desc, longDesc, 10));
    }
}