using Microsoft.AspNetCore.Mvc.Razor.Extensions;

namespace SAExpiations.ViewModels
{
    public class LocationServicesDetails
    {
        public string?  SelectedArea { get; set; }
        public int SelectedYear { get; set; } = 2022;
        public int TotalExpiations { get; set; }
        public string? ExpiationCode { get; set; }
        public string? ExpiationDescription { get; set; }




    }
}
