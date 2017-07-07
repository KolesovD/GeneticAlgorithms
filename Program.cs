using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            int generationSize = 100;

            Console.WriteLine("Start with generation size {0}", generationSize);
            Control control = new Control(generationSize);

            control.Optimize(Crosser.CyclicCrossover, Mutator.ReverseSegmentMutation, 2000);
        }
    }
}
