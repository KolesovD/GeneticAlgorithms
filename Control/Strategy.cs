using System;
namespace GeneticAlgorithms
{
	public class Strategy : ABCcontrol
	{
		public Strategy() : base(new LoaderPlate()) {
			//кофигурация для конкретно этой стратегии
		}

		public override void ProgramRuning()
		{
			//здесь пишем логику генетического алгоритма
		}
	}
}
