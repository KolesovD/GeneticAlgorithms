using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GerberLibrary.Core;
using GerberLibrary.Core.Primitives;

namespace GeneticAlgorithms.Information
{
    public class GerberLoader : ILoader
    {
        private string _path;

        public GerberLoader(string path)
        {
            _path = path;
        }

        public Plate Parse()
        {
            StringLog stringLog = new StringLog();
            LoadedStuff loadedStuff = new LoadedStuff();
            loadedStuff.AddFile(stringLog, _path);

            int _id = 0;

            return new Plate
                (
                    loadedStuff
                        .Gerbers
                        .First()
                        .File
                        .DisplayShapes
                        .SelectMany(x =>
                        {
                            int k = 0;
                            k++;

                            return PairSequence(x.Vertices)
                                .Select(_data => new Segment(_id++, _data.Item1.X, _data.Item1.Y, _data.Item2.X, _data.Item2.Y, true));

                            //LinkedList<PointD> ts = new LinkedList<PointD>(x.Vertices);
                            //return ts
                            //    .GetAllNodes()
                            //    .Where(_node => _node.Next != default)
                            //    .Select(_node => (_node.Value, _node.Next.Value))
                            //    .Select(_data => new Segment(_id++, _data.Item1.X, _data.Item1.Y, _data.Item2.X, _data.Item2.Y, true));
                        })
                        .ToList()
                );
        }

        private IEnumerable<(T, T)> PairSequence<T>(IEnumerable<T> collection)
        {
            T _last = collection.First();

            foreach (var item in collection.Skip(1))
            {
                yield return (_last, item);

                _last = item;
            }
        }
    }
}
