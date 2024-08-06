using Cosmos.System.Network.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.utils.cmd.commands.network
{
    public class NetConfigCommand : Command
    {
        public override string[] Alias => new string[] { "netconfig" };
        public override string Help => "Get IP config";

        // TODO: fix this
        //       currently it just gives an error
        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            while (NetworkConfiguration.CurrentAddress.ToString() == "") ;
            Console.WriteLine($"IP              -> {NetworkConfiguration.CurrentAddress}");
            Console.WriteLine($"Default Gateway -> {NetworkConfiguration.CurrentNetworkConfig.IPConfig.DefaultGateway}");
            Console.WriteLine($"Subnet Mask     -> {NetworkConfiguration.CurrentNetworkConfig.IPConfig.SubnetMask}");
        }
    }
}
