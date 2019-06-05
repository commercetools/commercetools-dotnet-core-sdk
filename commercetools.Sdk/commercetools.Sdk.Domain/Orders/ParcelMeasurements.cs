namespace commercetools.Sdk.Domain.Orders
{
    public class ParcelMeasurements
    {
        public int HeightInMillimeter { get; set; }
        public int LengthInMillimeter { get; set; }
        public int WidthInMillimeter { get; set; }
        public int WeightInGram { get; set; }

        public ParcelMeasurements()
        {

        }
        public ParcelMeasurements(int heightInMillimeter, int lengthInMillimeter, int widthInMillimeter,
            int weightInGram)
        {
            this.HeightInMillimeter = heightInMillimeter;
            this.LengthInMillimeter = lengthInMillimeter;
            this.WidthInMillimeter = widthInMillimeter;
            this.WeightInGram = weightInGram;
        }

        public override bool Equals(object obj)
        {
            var equal = false;
            if (obj is ParcelMeasurements measurements)
            {
                equal = this.HeightInMillimeter.Equals(measurements.HeightInMillimeter) &&
                        this.LengthInMillimeter.Equals(measurements.LengthInMillimeter) &&
                        this.WidthInMillimeter.Equals(measurements.WidthInMillimeter) &&
                        this.WeightInGram.Equals(measurements.WeightInGram);
            }

            return equal;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
