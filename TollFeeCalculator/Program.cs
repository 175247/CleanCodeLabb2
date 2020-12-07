namespace TollFeeCalculator
{
    public class Program
    {
        static void Main()
        {
            var feeCalculator = new FeeCalculator();
            feeCalculator.Run(new Settings().DataFilePath);
        }
    }
}