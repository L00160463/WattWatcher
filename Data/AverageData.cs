namespace WattWatcher.Data
{
    public class RecordData
    {
        public CircuitData Circuit1 { get; set; }
        public CircuitData Circuit2 { get; set; }
    }

    public class CircuitData
    {
        public double AvgAmps { get; set; }
        public double AvgKwh { get; set; }
        public double AvgWatts { get; set; }
    }
}
