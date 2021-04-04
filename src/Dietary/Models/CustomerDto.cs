namespace Dietary.Models
{
    public class CustomerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public CustomerDto(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
        }
    }
}