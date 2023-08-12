namespace VeekelApi.Models
{
    public class ConfigurationResponse
    {
        public bool Changed { get; set; }
        public string CurrenciesHash{ get; set; }
        public string CountriesHash{ get; set; }
        public List<Country> Countries { get; set; }
        public List<Currency> Currencies{ get; set; }
    }
}
