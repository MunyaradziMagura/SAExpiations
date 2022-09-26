namespace SAExpiations.ViewModels
{
    public class LocationExpiationCounter
    {
        public string LocalServiceAreaCode { get; set; } = null!;
        public string? LocalServiceArea1 { get; set; }
        public int NumberofExpiations { get; set; } = 0;
    }
}
