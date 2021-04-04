namespace Dietary.Models
{
    public class Customer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CustomerType Type { get; set; }
        public CustomerOrderGroup Group { get; set; }
        
        public enum CustomerType
        {
            Person,
            Representative,
            Division,
            Company,
            Admin
        }
    }
}