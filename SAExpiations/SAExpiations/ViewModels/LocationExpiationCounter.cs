using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SAExpiations.ViewModels
{
    public class LocationExpiationCounter
    {
        [Display(Name = "SearchText")]
        public string? SearchText { get; set; }
        public string LocalServiceAreaCode { get; set; } = null!;
        public string? LocalServiceArea1 { get; set; }
        public int NumberofExpiations { get; set; } = 0;
    }
}
