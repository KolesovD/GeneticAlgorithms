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

        public int currentGenerationNumber { get; private set; }
        public int islandID { get; private set; }
        public int migrationCount { get; private set; }

        public Info(AbstractIndividual Individual, int currentGenerationNumber, int islandID, int migrationCount)
        {
            this.Individual = Individual;
            this.currentGenerationNumber = currentGenerationNumber;
            this.islandID = islandID;
            this.migrationCount = migrationCount;
        }

        public override string ToString()
        {
            return string.Format("Поколение № {0} остров № {1} количество миграций {2}", currentGenerationNumber, islandID, migrationCount);
        }

    }
}
