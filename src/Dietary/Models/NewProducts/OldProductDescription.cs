using System;
using System.ComponentModel.DataAnnotations;

namespace Dietary.Models.NewProducts
{
    public class OldProductDescription
    {
        [Key]
        public Guid Id { get; private set; }
        private readonly OldProduct _oldProduct;

        private OldProductDescription()
        {
        }
        
        public OldProductDescription(OldProduct oldProduct)
        {
            Id = oldProduct.SerialNumber;
            _oldProduct = oldProduct;
        }

        public void ReplaceCharFromDesc(char charToReplace, char replaceWith)
        {
            _oldProduct.ReplaceCharFromDesc(charToReplace, replaceWith);
        }

        public string FormatDesc() => _oldProduct.FormatDesc();
    }
}