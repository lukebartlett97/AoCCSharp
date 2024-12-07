using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace AdventOfCode.Solutions
{
    class FlipFlops : SolutionMain
    {

        public FlipFlops() : base("year2023/Day20/") { }

        protected override String Solve(List<String> data)
        {
            var modules = new List<Module>();

            foreach(var row in data)
            {
                var split = row.Split(" -> ");
                var targets = split[1].Split(", ");
                if (split[0] == "broadcaster")
                {
                    modules.Add(new BroadcasterModule("broadcaster", targets));
                }
                else if (split[0].StartsWith("%"))
                {
                    modules.Add(new FlipFlopModule(split[0][1..], targets));
                }
                else if (split[0].StartsWith("&"))
                {
                    modules.Add(new ConjunctionModule(split[0][1..], targets));
                }
            }


            var rxModule = new RxModule();
            modules.Add(rxModule);
            var broadcastModule = modules.First(module => typeof(BroadcasterModule) == module.GetType());
            BigInteger count = 0;
            BigInteger lastKnownHighPings = 0;
            while(rxModule.LowPings == 0)
            {
                count++;
                broadcastModule.ProcessPulse(broadcastModule, false, modules);
                if (rxModule.HighPings % 1000 == 0 && lastKnownHighPings != rxModule.HighPings)
                {
                    lastKnownHighPings = rxModule.HighPings;
                    PrintInfo("High Pings: " + rxModule.HighPings);
                }
            }

            var lowPulses = 1000 + modules.Select(module => module.SentLowPulses).Sum();
            var highPulses = modules.Select(module => module.SentHighPulses).Sum();
            PrintInfo("Low: " + lowPulses);
            PrintInfo("High: " + highPulses);
            return count.ToString();
        }

        private class Pulse
        {
            public string Source;
            public string Target;
            public bool High;

            public Pulse(string source, string target, bool high)
            {
                Source = source;
                Target = target;
                High = high;
            }
        }

        private abstract class Module
        {
            public string Name;
            public int SentLowPulses = 0;
            public int SentHighPulses = 0;
            public string[] Targets;

            public Module(string name, string[] targets)
            {
                Name = name;
                Targets = targets;
            }

            public abstract void ProcessPulse(Module source, bool high, List<Module> modules);

            public void SendPulse(bool high, List<Module> modules)
            {
                foreach(var target in Targets)
                {
                    if (high)
                    {
                        SentHighPulses++;
                    }
                    else
                    {
                        SentLowPulses++;
                    }
                    var targetModule = modules.FirstOrDefault(module => module.Name.Equals(target));
                    if(targetModule != null)
                    {
                        targetModule.ProcessPulse(this, high, modules);
                    }
                }
            }
        }

        private class FlipFlopModule : Module
        {
            private bool On = false;

            public FlipFlopModule(string name, string[] targets) : base(name, targets)
            {
            }

            public override void ProcessPulse(Module source, bool high, List<Module> modules)
            {
                if (!high)
                {
                    On = !On;
                    SendPulse(On, modules);
                }
            }
        }

        private class RxModule : Module
        {
            public BigInteger LowPings = 0;
            public BigInteger HighPings = 0;


            public RxModule() : base("rx", Array.Empty<string>())
            {
            }

            public override void ProcessPulse(Module source, bool high, List<Module> modules)
            {
                if (high)
                {
                    HighPings++;
                }
                else
                {
                    LowPings++;
                }
            }
        }

        private class ConjunctionModule : Module
        {
            private HashSet<Module> HighsRecieved = new HashSet<Module>();

            public ConjunctionModule(string name, string[] targets) : base(name, targets)
            {
            }

            public override void ProcessPulse(Module source, bool high, List<Module> modules)
            {
                if(high)
                {
                    HighsRecieved.Add(source);
                }
                else
                {
                    HighsRecieved.Remove(source);
                }
                var allInputsHigh = modules.Where(module => module.Targets.Contains(Name)).All(module => HighsRecieved.Contains(module));
                SendPulse(!allInputsHigh, modules);
            }
        }

        private class BroadcasterModule : Module
        {
            public BroadcasterModule(string name, string[] targets) : base(name, targets)
            {
            }

            public override void ProcessPulse(Module source, bool high, List<Module> modules)
            {
                SendPulse(high, modules);
            }
        }
    }
}