using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WPFVisualizer.Extensions;
using Quaternion = System.Windows.Media.Media3D.Quaternion;

namespace WPFVisualizer.VisualControlls
{


    public static class VisualPlateControll
    {
        private static Matrix3D d45Plus;
        private static Matrix3D d45Minus;

        static VisualPlateControll()
        {
            d45Plus = Matrix3D.Identity;
            d45Plus.Rotate(new Quaternion(new Vector3D(0f, 0f, 1f), 30f));

            d45Minus = Matrix3D.Identity;
            d45Minus.Rotate(new Quaternion(new Vector3D(0f, 0f, 1f), -30f));
        }

        public static void DrawArrow(DrawingContext dc, Vector2 start, Vector2 end, Pen drawingpen) 
        {
            DrawArrow(dc, start.ToPoint(), end.ToPoint(), drawingpen);
        }

        public static void DrawArrow(DrawingContext dc, Point start, Point end, Pen drawingpen) 
        {
            dc.DrawLine(drawingpen, start, end);


            //Vector3D _vec = (start.ToVector2() - end.ToVector2()).ToVector3D();
            //_vec.Normalize();


            //Vector3D left = d45Plus.Transform(_vec);
            //left *= drawingpen.Thickness*3;


            //Vector3D right = d45Minus.Transform(_vec);
            //right *= drawingpen.Thickness*3;

            //dc.DrawLine(drawingpen, (end.ToVector2() + left.ToVector2()).ToPoint(), end);
            //dc.DrawLine(drawingpen, (end.ToVector2() + right.ToVector2()).ToPoint(), end);
        }

        public static VisualHost Wrap(this Visual visual) 
        {
            return new VisualHost { Visual = visual };
        }


    }
}
