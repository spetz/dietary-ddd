namespace Dietary.Models
{
    public class Product
    {
        private decimal? _price;
        private string _product;
        private int _counter;
        public long Id { get; private set; }
        
        public Product()
        {
        }

        public void DecrementCounter()
        {
            _counter--;
        }

        public void IncrementCounter()
        {
            _counter++;
        }
    }
}