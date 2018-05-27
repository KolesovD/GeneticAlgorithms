using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace GeneticAlgorithms
{
    public class FlagException : Exception {}

    public class MasterControll
    {
        private object _locker;
        private bool _is_change;
        private ManualResetEvent stopper;
        public int MigrationCount { get; private set; }
        public double MigrationProbability { get; private set; }
        public bool StopState { get; private set; }

        public bool IsAvaliableChange { 
            get {
                if (!Monitor.TryEnter(_locker)) {
                    throw new FlagException();//эквивалентно false
                }
                try {
                    return _is_change;
                }
                finally {
                    Monitor.Exit(_locker);
                }
            }
            private set {
                _is_change = value;
            }
        }

        public void Set() {//start ring
            lock (_locker) {
                if (_is_change == false) { throw new FlagException(); }
                IsAvaliableChange = false;
            }
        }
        public void Reset() {//stop ring
            lock (_locker) {
                if (_is_change == true) { throw new FlagException(); }
                IsAvaliableChange = true;
            }
        }

        public MasterControll(int migration_count, int population_count, string xml, int generationSize, Func<int, Delegates.Crossover> get_cross, Func<int, Delegates.Mutator> get_mutate, Func<int, Action<Control>> onNewStep)
        {
            MigrationCount = migration_count;
            _locker = new object();
            _is_change = true;
            MigrationProbability = 1/(population_count*generationSize);//?
            StopState = true;
            stopper = new ManualResetEvent(StopState);
            ChangeBag<AbstractIndividual>[] changebags = new ChangeBag<AbstractIndividual>[population_count];
            for (int i = 0; i < population_count; i++) {
                changebags[i] = new ChangeBag<AbstractIndividual>();
            }
            Control contr = new Control(this, xml, generationSize, changebags[population_count - 1], changebags[0]);
            Task _task = new Task(() => TaskFunc(contr, get_cross(0), get_mutate(0), onNewStep(0)));
            _task.Start();
            for (int i = 1; i < population_count; i++) {
                int iterator = i;
                contr = new Control(this, xml, generationSize, changebags[i-1], changebags[i]);
                _task = new Task(() => TaskFunc(contr, get_cross(iterator), get_mutate(iterator), onNewStep(iterator)));
                _task.Start();
            }
        }

        public void ResetStopper() {
            if (StopState == false) { return; }
            stopper.Reset();//осановка
            StopState = false;
        }
        public void SetStopper() {
            if (StopState == true) { return; }
            stopper.Set();//запуск
            StopState = true;
        }

        private void TaskFunc(Control c, Delegates.Crossover cross, Delegates.Mutator mutate, Action<Control> onNewStep)
        {
            while (true)
            {
                stopper.WaitOne();
                c.OptimizeStep(cross, mutate);
                onNewStep(c);
            }
        }
    }
}
