using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
//using System.Collections.Concurrent;

namespace GeneticAlgorithms
{
    public class ChangeBag<T>
    {
        private object _locker;
        private List<T> Migrate;

        public int Count
        {
            get {
                if (!Monitor.TryEnter(_locker))
                {
                    throw new FlagException();
                }
                try
                {
                    return Migrate.Count;
                }
                finally
                {
                    Monitor.Exit(_locker);
                }
            }
        }

        public ChangeBag() {
            Migrate = new List<T>();
            _locker = new object();
        }

        public void SaveOperate(Action<List<T>> operation) {
            lock(_locker) {
                operation.Invoke(Migrate);
            }
        }
    }
}
