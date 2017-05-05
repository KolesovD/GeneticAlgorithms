using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class Program
    {
		static ABCcontrol Controll;
        static void Main(string[] args)
        {
			Controll = new Strategy();
			Controll.ProgramRuning();
        }
    }
}
