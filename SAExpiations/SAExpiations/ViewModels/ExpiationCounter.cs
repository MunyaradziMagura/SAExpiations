using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SAExpiations.ViewModels
{
    public class ExpiationCounter
    {

        public string ExpiationOffenceCode { get; set; } = null!;
        public string? ExpiationOffenceDescription { get; set; }
        public int? ExpiationCount{ get; set; }
        
        [Display(Name = "SearchText")]
        public string? SearchText { get; set; }
        public bool? ShowActiveExpiation { get; set; }

    }
}
