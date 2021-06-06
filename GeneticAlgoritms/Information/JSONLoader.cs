using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace GeneticAlgorithms.Information
{
    public class JSONLoader : ILoader
    {
        private string _fileString;

        public JSONLoader(string path)
        {
            _fileString = File.ReadAllText(path);
        }

        public Plate Parse()
        {
            List<Segment> _Segments = JsonConvert.DeserializeObject<List<Segment>>(_fileString);
            return new Plate(_Segments);
        }
    }
}
