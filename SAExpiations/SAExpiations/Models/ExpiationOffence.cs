using System;
using System.Collections.Generic;

namespace SAExpiations.Models
{
    public partial class ExpiationOffence
    {
        public string ExpiationOffenceCode { get; set; } = null!;
        public string? ExpiationOffenceDescription { get; set; }

        public virtual ExpiationCategory ExpiationCategory { get; set; } = null!;
    }
}
