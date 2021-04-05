namespace Dietary.Models.Boundaries
{
    public class ClientAddress
    {
        private readonly string _address1;
        private readonly string _address2;
        private readonly string _address3;
        private readonly string _address4;

        public ClientAddress(string address1, string address2, string address3, string address4)
        {
            _address1 = address1;
            _address2 = address2;
            _address3 = address3;
            _address4 = address4;
        }

        public bool IsWithinEurope() => _address4.Contains("Europe");
    }
}