using Microsoft.Extensions.DependencyInjection;
using System;

namespace TollFeeCalculator
{
    public class Program
    {
        static void Main()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ISettings, Settings>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var feeCalculator = ActivatorUtilities.CreateInstance<FeeCalculator>(serviceProvider);
            feeCalculator.Run(new Settings().DataFilePath);
        }
    }
}