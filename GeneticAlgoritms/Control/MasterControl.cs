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

    public class MasterControl
    {
        private object _locker;
        private bool _is_change;
        private ManualResetEvent stopper;
        public int MigrationCount { get; private set; }
        public double MigrationProbability { get; private set; }
        public bool IsRunning { get; private set; }
        public Thread[] IslandTasks;
        public Control[] Controls;

        public bool IsAvaliableChange { 
            get {
                //if (!Monitor.TryEnter(_locker, 100))
                //{
                //    throw new FlagException();//эквивалентно false
                //}
                //try
                //{
                //    return _is_change;
                //}
                //finally
                //{
                //    Monitor.Exit(_locker);
                //}

                lock (_locker)
                {
                    return _is_change;
                }

                //return _is_change;
            }
        }

        public bool Set() {//start ring
            lock (_locker)
            {
                if (_is_change == false) { return false; }
                _is_change = false;
                return true;
            }
        }
        public void Reset() {//stop ring

            lock (_locker)
            {
                if (_is_change == true) { return; }
                _is_change = true;
            }
        }

        public MasterControl(
            int migration_count, 
            int populations_count, 
            string xml, 
            int generationSize, 
            double migrationProbability,
            Func<int, Delegates.Crossover> get_cross, 
            Func<int, Delegates.Mutator> get_mutate, 
            Func<int, Action<Control>> onNewStep,
            CancellationToken token
            )
        {
            MigrationCount = migration_count;
            _locker = new object();
            _is_change = true;
            MigrationProbability = migrationProbability;//1.0d /(populations_count*10.0d);
            IsRunning = true;
            stopper = new ManualResetEvent(IsRunning);

            ConcurrentBag<AbstractIndividual>[] changebags = new ConcurrentBag<AbstractIndividual>[populations_count];
            for (int i = 0; i < populations_count; i++) 
            {
                changebags[i] = new ConcurrentBag<AbstractIndividual>();
            }

            Controls = new Control[populations_count];
            IslandTasks = new Thread[populations_count];

            Controls[0] = new Control(0, this, xml, generationSize, changebags[populations_count - 1], changebags[0]);
            IslandTasks[0] = new Thread(() => TaskFunc(Controls[0], get_cross(0), get_mutate(0), onNewStep(0), token));
            for (int j = 1; j < populations_count; j++) {
                int iterator = j;
                Controls[iterator] = new Control(iterator, this, xml, generationSize, changebags[iterator - 1], changebags[iterator]);
                IslandTasks[iterator] = new Thread(() => TaskFunc(Controls[iterator], get_cross(iterator), get_mutate(iterator), onNewStep(iterator), token));
            }
            //Console.WriteLine();
        }
        public void Start()
        {
            foreach (Thread t in IslandTasks)
            {
                t.IsBackground = true;
                t.Start();
            }
        }

        public void Pause() {
            if (IsRunning == false) { return; }
            stopper.Reset();//осановка
            IsRunning = false;
        }
        public void Continue() {
            if (IsRunning == true) { return; }
            stopper.Set();//запуск
            IsRunning = true;
        }

        private void TaskFunc(
            Control c, 
            Delegates.Crossover cross, 
            Delegates.Mutator mutate, 
            Action<Control> onNewStep,
            CancellationToken token
            )
        {
            while (!token.IsCancellationRequested)
            {
                WaitHandle.WaitAny
                    (
                    new[] { token.WaitHandle, stopper }
                    );

                if (token.IsCancellationRequested) 
                {
                    return;
                }

                c.OptimizeStep(cross, mutate);

                if (token.IsCancellationRequested)
                {
                    return;
                }

                onNewStep(c);
            }
        }
    }
}
