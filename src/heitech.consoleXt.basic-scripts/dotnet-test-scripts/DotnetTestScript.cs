using System.Diagnostics;
using heitech.consoleXt.core;
using heitech.consoleXt.core.Builtins;

namespace heitech.consoleXt.basic_scripts.dotnet_test_scripts
{
    public class DotnetTestScript : Script
    {
        public override string Name => "test";

        private const string Short_LOCATION = "l";
        private const string Long_LOCATION = "location";

        private const string Short_FILTER = "f";
        private const string Long_FILTER = "filter";

        
        private const string Short_TRX = "x";
        private const string Long_TRX = "trx";
        
        public override IEnumerable<Parameter> AcceptedParameters 
            => new Parameter[] 
            { 
                new(Short_LOCATION, Long_LOCATION, true), 
                new(Short_FILTER, Long_FILTER),
                new (Short_TRX, Long_TRX) 
            };

        public override async Task RunAsync(ParameterCollection collection, OutputHelperMap output)
        {
            Parameter AcceptedParameterAtIndex(int index)
                => collection.FindByParameter(AcceptedParameters.ElementAt(index));

            _ = Parameter.TryParse(AcceptedParameterAtIndex(0), v => (v, true), out string rootLocation);
            bool hasFilter = Parameter.TryParse(AcceptedParameterAtIndex(1), v => (v, true), out string filterValue);

            string arguments = hasFilter
                               ? $"test {rootLocation} --filter {filterValue}"
                               : $"test {rootLocation}";

            var proc = Process.Start
            (
                new ProcessStartInfo("dotnet", arguments)
                {
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Normal,
                    RedirectStandardOutput = true,
                }
            );
            await proc!.WaitForExitAsync();

            string standardOut = await proc.StandardOutput.ReadToEndAsync();
            await output[OutputHelperMap.Console].WriteAsync($"result: {Environment.NewLine}{standardOut}");
        }
    }
}
