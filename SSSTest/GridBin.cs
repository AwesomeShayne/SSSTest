using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SSSTest
{
    class GridBin : Bin
    {
        int MaxX;
        int MaxY;
        public int AreaContents;
        static Random Rand = new Random();
        public List<GridShape> Shapes = new List<GridShape>();
        public List<GridShape> Bins = new List<GridShape>();

        public GridBin(int _Number, int _X, int _Y) : base(_Number)
        {
            MaxX = _X;
            MaxY = _Y;
            AreaContents = MaxX * MaxY;
            Bins.Add(new GridShape(MaxX, MaxY, 0, 0));
        }

        public bool CheckNow(GridShape NewContent)
        {
            foreach(GridShape _Bin in Bins)
            {
                if (_Bin.Width >= NewContent.Width && _Bin.Height >= NewContent.Height)
                    return true;
                else if (_Bin.Width >= NewContent.Height && _Bin.Height >= NewContent.Width)
                    return true;
            }
            return false;
        }

        public void AddContent(GridShape NewContent)
        {
            for (int _Index = 0; _Index < Bins.Count; _Index++)
            {
                var _Bin = Bins[_Index];
                if (_Bin.Width >= NewContent.Width && _Bin.Height >= NewContent.Height)
                {
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    Shapes.Add(NewContent);
                    Split(_Bin, NewContent, _Index);
                    AreaContents -= NewContent.Area;
                    break;
                }
                else if (_Bin.Width >= NewContent.Height && _Bin.Height >= NewContent.Width)
                {
                    NewContent = new GridShape(NewContent.Height, NewContent.Width);
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    Shapes.Add(NewContent);
                    Split(_Bin, NewContent, _Index);
                    AreaContents -= NewContent.Area;
                    break;
                }
            }
        }

        private void Split(GridShape _CurrentBin, GridShape _CurrentObject, int _Index)
        {
            var _X = _CurrentBin.X;
            var _Y = _CurrentBin.Y;
            var _OriginalHeight = _CurrentBin.Height;
            var _OriginalWidth = _CurrentBin.Width;
            var _RemovedHeight = _CurrentObject.Height;
            var _RemovedWidth = _CurrentObject.Width;
            Bins.RemoveAt(_Index);
            Bins.Add(new GridShape(_RemovedWidth, _OriginalHeight - _RemovedHeight, _X, _Y + _RemovedHeight ));
            Bins.Add(new GridShape(_OriginalWidth - _RemovedWidth, _RemovedHeight, _X + _RemovedWidth, _Y));
            Bins.Add(new GridShape(_OriginalWidth - _RemovedWidth, _OriginalHeight - _RemovedHeight, _X + _RemovedWidth, _Y + _RemovedHeight));
            Bins = Bins.OrderByDescending(Area => Area).ToList();
        }

        public Bitmap BinImage()
        {
            var Output = new Bitmap(MaxX * 5, MaxY * 5);
            using (Graphics g = Graphics.FromImage(Output))
            {
                int _R;
                int _G;
                int _B;
                foreach (GridShape _Shape in Shapes)
                {
                    var _Rect = new Rectangle(_Shape.X * 5, _Shape.Y * 5, _Shape.Width * 5, _Shape.Height * 5);
                    _R = Rand.Next(0, 256);
                    _G = Rand.Next(0, 256);
                    _B = Rand.Next(0, 256);
                    Color _FillColor = Color.FromArgb(50, _R, _G, _B);
                    Color _LineColor = Color.Black;
                    SolidBrush _FillBrush = new SolidBrush(_FillColor);
                    Pen _LineBrush = new Pen(_LineColor, 1);
                    g.DrawRectangles(_LineBrush, new RectangleF[] { _Rect });
                    g.FillRectangles(_FillBrush, new RectangleF[] { _Rect });
                }
            }
            return Output;
        }
    }
}
