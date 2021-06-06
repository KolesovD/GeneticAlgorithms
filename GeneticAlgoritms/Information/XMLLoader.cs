﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GeneticAlgorithms.Information
{
    public class XMLLoader : ILoader
    {
        private XDocument resourse;

        public XMLLoader(string path)
        {
            resourse = XDocument.Load(path);
        }

        public Plate Parse()
        {
            List<Segment> _Segments = new List<Segment>();
            foreach (XElement el in resourse.Root.Elements())
            {
                XElement point_01 = el.Elements().ElementAt(0);
                XElement point_02 = el.Elements().ElementAt(1);
                _Segments.Add(new Segment(Convert.ToInt32(el.Attribute("id").Value),
                                                              Convert.ToSingle(point_01.Attribute("x").Value),
                                                              Convert.ToSingle(point_01.Attribute("y").Value),
                                                              Convert.ToSingle(point_02.Attribute("x").Value),
                                                              Convert.ToSingle(point_02.Attribute("y").Value),
                                                              Convert.ToBoolean(el.Attribute("direction").Value)));
            }
            return new Plate(_Segments);
        }
    }
}
