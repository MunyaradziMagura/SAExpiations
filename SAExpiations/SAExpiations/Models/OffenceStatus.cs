using System;
using System.Collections.Generic;

namespace SAExpiations.Models
{
    public partial class OffenceStatus
    {
        public string OffenceStatusCode { get; set; } = null!;
        public string? OffenceStatusDescription { get; set; }
    }
}
