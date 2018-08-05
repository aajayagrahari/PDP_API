namespace PowerDesignProAPI.BusinessProcessors.Dtos
{
    public class AddressValidationRequestDto
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string CountryCode { get; set; }
    }
}