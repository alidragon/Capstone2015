using AForge.Neuro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuttingThingsTogether {
    public class NeuralNetworking {

        public static void RunNetwork() {
            //TODO: wrap genetic algorithm around this for input counts?
            ActivationNetwork net = new ActivationNetwork(new ThresholdFunction(), 100, new int[] { 10, 15, 20 });
        }
    }
}
