using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GerberLibrary;
using GerberLibrary.Core;
using System.IO;


#region log

public class StringLog : ProgressLog
{
    private StringBuilder _sb;

    public override void AddString(string text, float progress = -1)
    {
        _sb.AppendLine($"{progress}: {text}");
    }

    public override string ToString()
    {
        return _sb.ToString();
    }
}

#endregion

public class LoadedStuff
{
    public class DisplayGerber

    {
        public bool visible;
        public ParsedGerber File;
        public int sortindex;
    }
    public GerberLibrary.BoardRenderColorSet Colors = new GerberLibrary.BoardRenderColorSet();

    public List<DisplayGerber> Gerbers = new List<DisplayGerber>();

    public void AddFileStream(ProgressLog log, MemoryStream S, string origfilename, double drillscaler = 1.0)
    {
        var FileType = Gerber.FindFileTypeFromStream(new StreamReader( S), origfilename);

        S.Seek(0, SeekOrigin.Begin);
           
        if (FileType == BoardFileType.Unsupported)
        {
            return;
        }

        ParsedGerber PLS;


        GerberParserState State = new GerberParserState() { PreCombinePolygons = false };
        if (FileType == BoardFileType.Drill)
        {
            if (Gerber.ExtremelyVerbose) Console.WriteLine("Log: Drill file: {0}", origfilename);
            PLS = PolyLineSet.LoadExcellonDrillFileFromStream(log, new StreamReader(S), origfilename, false, drillscaler);
            S.Seek(0, SeekOrigin.Begin);

            // ExcellonFile EF = new ExcellonFile();
            // EF.Load(a);
        }
        else
        {

            bool forcezerowidth = false;
            bool precombinepolygons = false;
            BoardSide Side = BoardSide.Unknown;
            BoardLayer Layer = BoardLayer.Unknown;
            Gerber.DetermineBoardSideAndLayer(origfilename, out Side, out Layer);
            if (Layer == BoardLayer.Outline || Layer == BoardLayer.Mill)
            {
                forcezerowidth = true;
                precombinepolygons = true;
            }
            State.PreCombinePolygons = precombinepolygons;
               

            PLS = PolyLineSet.LoadGerberFileFromStream(log, new StreamReader(S),origfilename, forcezerowidth, false, State);
            S.Seek(0, SeekOrigin.Begin);

            PLS.Side = Side;
            PLS.Layer = Layer;
        }

        Gerbers.Add(new DisplayGerber() { File = PLS, visible = true, sortindex = Gerber.GetDefaultSortOrder(PLS.Side, PLS.Layer) });

    }

    public void AddFile(ProgressLog log, string filename, double drillscaler = 1.0)
    {
        MemoryStream MS2 = new MemoryStream();
        FileStream FS = File.OpenRead(filename);
        FS.CopyTo(MS2);
        MS2.Seek(0, SeekOrigin.Begin);
        AddFileStream(log, MS2, filename, drillscaler);
    }
}
