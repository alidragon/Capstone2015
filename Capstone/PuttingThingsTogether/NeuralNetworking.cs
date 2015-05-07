using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuttingThingsTogether {
    public class NeuralNetworking {

        Dictionary<string, double[]> wordEmbeddings = new Dictionary<string, double[]>();

        public static void RunNetwork() {
            //TODO: wrap genetic algorithm around this for input counts?
            //ActivationNetwork net = new ActivationNetwork(new ThresholdFunction(), 100, new int[] { 10, 15, 20 });

            // set neurons weights randomization range
            Neuron.RandRange = new Range( 0f, 1f);

            int inputRange = 100;
            // create network
            DistanceNetwork network = new DistanceNetwork( 100, 100 * 100 );
            // create learning algorithm
            SOMLearning trainer = new SOMLearning( network );
            // input
            double[] input = new double[100];


            //// loop
            //while ( ... )
            //{
            //    // update learning rate and radius
            //    // ...
            //    trainer.

            //    // prepare network input
            //    input[0] = rand.Next( 256 );
            //    input[1] = rand.Next( 256 );
            //    input[2] = rand.Next( 256 );

            //    // run learning iteration
            //    trainer.Run( input );
    
            //    ...
            //}
        }
    }
}
