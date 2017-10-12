using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms.Information
{
    class Info
    {
        private static Info instance;

        private Info() { }

        public static Info getInstance
        {
            get
            {
                if (instance==null)
                {
                    instance = new Info();
                }

                return instance;
            }
        }
    }
}
