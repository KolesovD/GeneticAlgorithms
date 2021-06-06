using Newtonsoft.Json;

namespace GeneticAlgorithms.Information
{
    public class JSONUnloader : IUnloader
    {
        private AbstractIndividual _individual;

        public JSONUnloader(AbstractIndividual individual)
        {
            _individual = individual;
        }

        public string Parse()
        {
            return JsonConvert.SerializeObject(_individual.GetSegmentsToSerialize());
        }
    }
}
