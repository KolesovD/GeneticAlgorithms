using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace GeneticAlgorithms
{
	public class LoaderPlate : Loader
	{
		private XDocument resourse;

		public LoaderPlate(string path) {
			resourse = XDocument.Load(path);
		}

		public override Iindividual CreateNewIndividualEmpty() {
			return new Plate();
		}
		public override Iindividual CreateNewIndividual() {
			List<Segment> _Segments = new List<Segment>();
			foreach (XElement el in resourse.Root.Elements()) {
				Regex reg = new Regex(@"[0-9]+");
				MatchCollection collect = reg.Matches(el.Value);
				_Segments.Add(new Segment(Convert.ToInt32(el.Attribute("id").Value),
															  Convert.ToInt32(collect[0].Value),
															  Convert.ToInt32(collect[1].Value),
															  Convert.ToInt32(collect[2].Value),
															  Convert.ToInt32(collect[3].Value),
															  Convert.ToBoolean(el.Attribute("direction").Value)));
			}
			return new Plate(_Segments);
		}

		public override Iindividual CopyFrom(Iindividual _individual)
		{
			return new Plate(_individual as Plate);
		}

		//public override Iindividual Crossover(Iindividual parent1, Iindividual parent2)
		//{
		//	//тестовая версия с логом
		//	//здесь пишем код кроссовера
		//	Console.WriteLine("Crossover is work");
		//	return parent1;
		//}

		//public override void Mutator(Iindividual target)
		//{
		//	//тестовая версия с логом
		//	//здесь пишем код мутатора
		//	Console.WriteLine("Mutator is work");
		//}
	}
}