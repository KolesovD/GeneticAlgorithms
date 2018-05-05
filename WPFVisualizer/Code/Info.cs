using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithms;

namespace WPFVisualizer.Code
{
    public class Info
    {
        public AbstractIndividual Individual { get; private set; }
        public string GenInfo { get; private set; }

        public Info(AbstractIndividual Individual, string info)
        {
            this.Individual = Individual;
            GenInfo = info;
        }

    }
}
