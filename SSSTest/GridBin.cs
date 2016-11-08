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
        private int BinsPlaced = 0;
        static Random Rand = new Random();
        public List<GridShape> Shapes = new List<GridShape>();
        public List<GridShape> YBins = new List<GridShape>();
        public List<GridShape> XBins = new List<GridShape>();
        public List<GridShape> YDrawBins = new List<GridShape>();
        public List<GridShape> XDrawBins = new List<GridShape>();

        public GridBin(int _Number, int _X, int _Y) : base(_Number)
        {
            MaxX = _X;
            MaxY = _Y;
            AreaContents = MaxX * MaxY;
            YBins.Add(new GridShape(0, 0, MaxX, MaxY));
            XBins.Add(new GridShape(0, 0, MaxX, MaxY));
        }

        public bool CheckNow(GridShape NewContent)
        {
            foreach(GridShape _Bin in YBins)
            {
                if (_Bin.Width >= NewContent.Width && _Bin.Height >= NewContent.Height)
                    return true;
                else if (_Bin.Width >= NewContent.Height && _Bin.Height >= NewContent.Width)
                    return true;
            }

            foreach (GridShape _Bin in XBins)
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
            for (int _Index = 0; _Index < XBins.Count; _Index++)
            {
                var _Bin = XBins[_Index];
                if (_Bin.Width >= NewContent.Width && _Bin.Height >= NewContent.Height)
                {
                    BinsPlaced++;
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    NewContent.SetNumber(BinsPlaced);
                    Shapes.Add(NewContent);
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    MagicSplit(NewContent);
                    AreaContents -= NewContent.Area;
                    return;
                }
                else if (_Bin.Width >= NewContent.Height && _Bin.Height >= NewContent.Width)
                {
                    BinsPlaced++;
                    NewContent = new GridShape(NewContent.Height, NewContent.Width);
                    NewContent.SetNumber(BinsPlaced);
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    Shapes.Add(NewContent);
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    MagicSplit(NewContent);
                    AreaContents -= NewContent.Area;
                    return;
                }
            }
            for (int _Index = 0; _Index < YBins.Count; _Index++)
            {
                var _Bin = YBins[_Index];
                if (_Bin.Width >= NewContent.Width && _Bin.Height >= NewContent.Height)
                {
                    BinsPlaced++;
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    NewContent.SetNumber(BinsPlaced);
                    Shapes.Add(NewContent);
                    MagicSplit(NewContent);
                    AreaContents -= NewContent.Area;
                    return;
                }
                else if (_Bin.Width >= NewContent.Height && _Bin.Height >= NewContent.Width)
                {
                    BinsPlaced++;
                    NewContent = new GridShape(NewContent.Height, NewContent.Width);
                    NewContent.SetNumber(BinsPlaced);
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    Shapes.Add(NewContent);
                    MagicSplit(NewContent);
                    AreaContents -= NewContent.Area;
                    return;
                }
            }
            YBins.Sort((Bin1, Bin2) => Bin1.X.CompareTo(Bin2.X));
            YBins.Sort((Bin1, Bin2) => Bin1.Y.CompareTo(Bin2.Y));
            XBins.Sort((Bin1, Bin2) => Bin1.X.CompareTo(Bin2.X));
            XBins.Sort((Bin1, Bin2) => Bin1.Y.CompareTo(Bin2.Y));
        }

        /*private void Split(GridShape _CurrentBin, GridShape _CurrentObject, int _Index)
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
        }*/

        private void MagicSplit(GridShape _CurrentObject)
        {
            foreach (var _Bin in XBins.ToArray())
            {
                if (_Bin.CollidesWith(_CurrentObject))
                {
                    XSplit(_Bin, _CurrentObject);
                    XDrawBins.Add(_Bin);
                    XBins.Remove(_Bin);
                }
            }
            foreach (var _Bin in YBins.ToArray())
            {
                if (_Bin.CollidesWith(_CurrentObject))
                {
                    YSplit(_Bin, _CurrentObject);
                    YDrawBins.Add(_Bin);
                    YBins.Remove(_Bin);
                }
            }
            
        }

        private void YSplit(GridShape _Bin, GridShape _Box)
        {
            if (_Box.X <= _Bin.X && _Box.Y <= _Bin.Y)
            {
                if (_Box.Width >= (_Bin.Width + _Bin.X - _Box.X) && _Box.Height < (_Bin.Height + (_Bin.Height + (_Bin.Y - _Box.Y)))) // Case 2 
                {
                    var RemWidth = _Box.Width - (_Bin.X - _Box.X);
                    GridShape G = GridShape.Create(_Bin.X + RemWidth, _Bin.Y, _Bin.Width - RemWidth, _Bin.Height);
                    if (G != null)
                        YBins.Add(G);
                }
                else if (_Box.Width < (_Bin.Width + (_Bin.X - _Box.X)) && _Box.Height >= (_Bin.Height + (_Bin.Height + (_Bin.Y - _Box.Y)))) // Case 3 
                {  
                    var RemHeight = _Box.Height - (_Bin.Y - _Box.Y);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y + RemHeight, _Bin.Width, _Bin.Height - RemHeight);
                    if (G != null)
                        YBins.Add(G);
                }
                else // (_Box.Width < (_Bin.Width + (_Bin.X - _Box.X)) && _Box.Height < (_Bin.Height + (_Bin.Height + (_Bin.Y - _Box.Y)))) // Case 1 
                {
                    var RemWidth = _Box.Width - (_Bin.X - _Box.X);
                    var RemHeight = _Box.Height - (_Bin.Y - _Box.Y);
                    GridShape G = GridShape.Create(_Bin.X + RemWidth, _Bin.Y, _Bin.Width - RemWidth, _Bin.Height);
                    if (G != null)
                        YBins.Add(G);
                    G = GridShape.Create(_Bin.X, _Bin.Y + RemHeight, RemWidth, _Bin.Height - RemHeight);
                    if (G != null)
                        YBins.Add(G);
                }
            }
            else if (_Box.X <= _Bin.X && _Box.Y > _Bin.Y)
            {
                if (_Box.Height >= _Bin.Height - (_Box.Y - _Bin.Y) && _Box.Width >= _Bin.Width - (_Bin.X - _Box.X)) // Case 7 
                {
                    var RemHeight = _Bin.Height - (_Box.Y - _Bin.Y);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Bin.Width, _Bin.Height - RemHeight);
                    if (G != null)
                        YBins.Add(G);
                }
                else if (_Box.Height < _Bin.Height - (_Box.Y - _Bin.Y) && _Box.Width >= _Bin.Width - (_Bin.X - _Box.X)) // Case 5 
                {
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Bin.Width, _Box.Y - _Bin.Y);
                    if (G != null)
                        YBins.Add(G);
                    G = GridShape.Create(_Bin.X, _Box.Y + _Box.Height, _Bin.Width, _Bin.Height - (_Box.Height + (_Box.Y - _Bin.Y)));
                    if (G != null)
                        YBins.Add(G);
                }
                else if (_Box.Height >= _Bin.Height - (_Box.Y - _Bin.Y) && _Box.Width < _Bin.Width - (_Bin.X - _Box.X)) // Case 6 
                {
                    var RemHeight = _Bin.Height - (_Box.Y - _Bin.Y);
                    var RemWidth = _Box.Width - (_Bin.X - _Box.Y);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, RemWidth, _Bin.Height - RemHeight);
                    if (G != null)
                        YBins.Add(G);
                    G = GridShape.Create(_Bin.X + RemWidth, _Bin.Y, _Bin.Width - RemWidth, _Bin.Height);
                    if (G != null)
                        YBins.Add(G);
                }
                else // (_Box.Height < _Bin.Height - (_Box.Y - _Bin.Y) && _Box.Width < _Bin.Width - (_Bin.X - _Box.X)) // Case 4 
                {
                    var RemWidth = _Box.Width - (_Bin.X - _Box.X);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, RemWidth, _Box.Y - _Bin.Y);
                    if (G != null)
                        YBins.Add(G);
                    G = GridShape.Create(_Bin.X, _Box.Y + _Box.Height, RemWidth, _Bin.Height - (_Box.Height + (_Box.Y - _Bin.Y)));
                    if (G != null)
                        YBins.Add(G);
                    G = GridShape.Create(_Bin.X + RemWidth, _Bin.Y, _Bin.Width - RemWidth, _Bin.Height);
                    if (G != null)
                        YBins.Add(G);
                }
            }
            else if (_Box.X > _Bin.X && _Box.Y <= _Bin.Y)
            {
                if (_Box.Width >= _Bin.Width - (_Box.X - _Bin.X) && _Box.Height >= _Bin.Height - (_Bin.Y - _Box.Y)) // Case 11 
                {
                    var RemWidth = _Bin.Width - (_Box.X - _Bin.X);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Bin.Width - RemWidth, _Bin.Height);
                    if (G != null)
                        YBins.Add(G);
                }
                else if (_Box.Width < _Bin.Width - (_Box.X - _Bin.X) && _Box.Height >= _Bin.Height - (_Bin.Y - _Box.Y)) // Case 9 
                {
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Box.X - _Bin.X, _Bin.Height);
                    G = GridShape.Create(_Box.X + _Box.Width, _Bin.Y, _Bin.Width - (_Box.Width + (_Box.X - _Bin.X)), _Bin.Height);
                    if (G != null)
                        YBins.Add(G);
                }
                else if (_Box.Width >= _Bin.Width - (_Box.X - _Bin.X) && _Box.Height < _Bin.Height - (_Bin.Y - _Box.Y)) // Case 10 
                {
                    var RemHeight = _Box.Height - (_Bin.Y - _Box.Y);
                    var RemWidth = _Bin.Width - (_Box.X - _Bin.X);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Bin.Width - RemWidth, _Bin.Height);
                    if (G != null)
                        YBins.Add(G);
                    G = GridShape.Create(_Box.X, _Bin.Y + RemHeight, RemWidth, _Bin.Height - RemHeight);
                    if (G != null)
                        YBins.Add(G);
                }
                else // if (_Box.Width < _Bin.Width - (_Box.X - _Bin.X) && _Box.Height < _Bin.Height - (_Bin.Y - _Box.Y)) // Case 8
                {
                    var RemHeight = _Box.Height - (_Bin.Y - _Box.Y);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Box.X - _Bin.X, _Bin.Height);
                    if (G != null)
                        YBins.Add(G);
                    G = GridShape.Create(_Box.X + _Box.Width, _Bin.Y, _Bin.Width - (_Box.Width + (_Box.X - _Bin.X)), _Bin.Height);
                    if (G != null)
                        YBins.Add(G);
                    G = GridShape.Create(_Box.X, _Bin.Y + RemHeight, _Box.Width, _Bin.Height - RemHeight);
                    if (G != null)
                        YBins.Add(G);
                }

            }
        }

        private void XSplit(GridShape _Bin, GridShape _Box)
        {
            if (_Box.X <= _Bin.X && _Box.Y <= _Bin.Y)
            {
                if (_Box.Width < (_Bin.Width + (_Bin.X - _Box.X)) && _Box.Height < (_Bin.Height + (_Bin.Height + (_Bin.Y - _Box.Y)))) // Case 1 
                {
                    var RemWidth = _Box.Width - (_Bin.X - _Box.X);
                    var RemHeight = _Box.Height - (_Bin.Y - _Box.Y);
                    GridShape G = GridShape.Create(_Bin.X + RemWidth, _Bin.Y, _Bin.Width - RemWidth, RemHeight);
                    if (G != null)
                        XBins.Add(G);
                    G = GridShape.Create(_Bin.X, _Bin.Y + RemHeight, _Bin.Width, _Bin.Height - RemHeight);
                    if (G != null)
                        XBins.Add(G);
                }
                else if (_Box.Width >= (_Bin.Width + _Bin.X - _Box.X) && _Box.Height < (_Bin.Height + (_Bin.Height + (_Bin.Y - _Box.Y)))) // Case 2 
                {
                    var RemWidth = _Box.Width - (_Bin.X - _Box.X);
                    GridShape G = GridShape.Create(_Bin.X + RemWidth, _Bin.Y, _Bin.Width - RemWidth, _Bin.Height);
                    if (G != null)
                        XBins.Add(G);
                }
                else if (_Box.Width < (_Bin.Width + (_Bin.X - _Box.X)) && _Box.Height >= (_Bin.Height + (_Bin.Height + (_Bin.Y - _Box.Y)))) // Case 3 
                {
                    var RemHeight = _Box.Height - (_Bin.Y - _Box.Y);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y + RemHeight, _Bin.Width, _Bin.Height - RemHeight);
                    if (G != null)
                        XBins.Add(G);
                }
            }
            else if (_Box.X <= _Bin.X && _Box.Y > _Bin.Y)
            {
                if (_Box.Height < _Bin.Height - (_Box.Y - _Bin.Y) && _Box.Width < _Bin.Width - (_Bin.X - _Box.X)) // Case 4 
                {
                    var RemWidth = _Box.Width - (_Bin.X - _Box.X);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Bin.Width, _Box.Y - _Bin.Y);
                    if (G != null)
                        XBins.Add(G);
                    G = GridShape.Create(_Bin.X + RemWidth, _Box.Y, _Bin.Width - RemWidth, _Box.Height);
                    if (G != null)
                        XBins.Add(G);
                    G = GridShape.Create(_Bin.X, _Box.Y + _Box.Height, _Bin.Width, _Bin.Height - (_Box.Height + (_Box.Y - _Bin.Y)));
                    if (G != null)
                        XBins.Add(G);
                }
                else if (_Box.Height < _Bin.Height - (_Box.Y - _Bin.Y) && _Box.Width >= _Bin.Width - (_Bin.X - _Box.X)) // Case 5 
                {
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Bin.Width, _Box.Y - _Bin.Y);
                    if (G != null)
                        XBins.Add(G);
                    G = GridShape.Create(_Bin.X, _Box.Y + _Box.Height, _Bin.Width, _Bin.Height - (_Box.Height + (_Box.Y - _Bin.Y)));
                    if (G != null)
                        XBins.Add(G);
                }
                else if (_Box.Height >= _Bin.Height - (_Box.Y - _Bin.Y) && _Box.Width < _Bin.Width - (_Bin.X - _Box.X)) // Case 6 
                {
                    var RemWidth = _Box.Width - (_Bin.X - _Box.X);
                    var RemHeight = _Bin.Height - (_Box.Y - _Bin.Y);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Bin.Width, _Bin.Height - RemHeight);
                    if (G != null)
                        XBins.Add(G);
                    G = GridShape.Create(_Bin.X + RemWidth, _Box.Y, _Bin.Width - RemWidth, RemHeight);
                    if (G != null)
                        XBins.Add(G);
                }
                else if (_Box.Height >= _Bin.Height - (_Box.Y - _Bin.Y) && _Box.Width >= _Bin.Width - (_Bin.X - _Box.X)) // Case 7 
                {
                    var RemHeight = _Bin.Height - (_Box.Y - _Bin.Y);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Bin.Width, _Bin.Height - RemHeight);
                    if (G != null)
                        XBins.Add(G);
                }
            }
            else if (_Box.X > _Bin.X && _Box.Y <= _Bin.Y)
            {
                if (_Box.Width < _Bin.Width - (_Box.X - _Bin.X) && _Box.Height < _Bin.Height - (_Bin.Y - _Box.Y)) // Case 8
                {
                    var RemHeight = _Box.Height - (_Bin.Y - _Box.Y);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Box.X - _Bin.X, RemHeight);
                    if (G != null)
                        XBins.Add(G);
                    G = GridShape.Create(_Box.X + _Box.Width, _Bin.Y, _Bin.Width - (_Box.Width + (_Box.X - _Bin.X)), RemHeight);
                    if (G != null)
                        XBins.Add(G);
                    G = GridShape.Create(_Bin.X, _Bin.Y + RemHeight, _Bin.Width, _Bin.Height - RemHeight);
                    if (G != null)
                        XBins.Add(G);
                }
                else if (_Box.Width < _Bin.Width - (_Box.X - _Bin.X) && _Box.Height >= _Bin.Height - (_Bin.Y - _Box.Y)) // Case 9 
                {
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Box.X - _Bin.X, _Bin.Height);
                    if (G != null)
                        XBins.Add(G);
                    G = GridShape.Create(_Box.X + _Box.Width, _Bin.Y, _Bin.Width - (_Box.Width + (_Box.X - _Bin.X)), _Bin.Height);
                    if (G != null)
                        XBins.Add(G);
                }
                else if (_Box.Width >= _Bin.Width - (_Box.X - _Bin.X) && _Box.Height < _Bin.Height - (_Bin.Y - _Box.Y)) // Case 10 
                {
                    var RemWidth = _Bin.Width - (_Box.X - _Bin.X);
                    var RemHeight = _Box.Height - (_Bin.Y - _Box.Y);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Bin.Width - RemWidth, RemHeight);
                    if (G != null)
                        XBins.Add(G);
                    G = GridShape.Create(_Bin.X, _Bin.Y + RemHeight, _Bin.Width, _Bin.Height - RemHeight);
                    if (G != null)
                        XBins.Add(G);
                }
                else if (_Box.Width >= _Bin.Width - (_Box.X - _Bin.X) && _Box.Height >= _Bin.Height - (_Bin.Y - _Box.Y)) // Case 11 
                {
                    var RemWidth = _Bin.Width - (_Box.X - _Bin.X);
                    GridShape G = GridShape.Create(_Bin.X, _Bin.Y, _Bin.Width - RemWidth, _Bin.Height);
                    if (G != null)
                        XBins.Add(G);
                }
            }
        }

        public Bitmap BinImage()
        {
            var Output = new Bitmap(MaxX * 5 + 5, MaxY * 5 + 5);
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
                    g.DrawRectangle(_LineBrush, _Rect);
                    g.DrawString(_Shape.Number.ToString(), new Font("Tahoma", 8), Brushes.Black, _Rect);
                    g.FillRectangle(_FillBrush,_Rect );
                }
                foreach (GridShape _Bin in XDrawBins)
                {
                    var _Rect = new Rectangle(_Bin.X * 5, _Bin.Y * 5, _Bin.Width * 5, _Bin.Height * 5);
                    g.DrawRectangle(Pens.Red, _Rect);
                }
                foreach (GridShape _Bin in YDrawBins)
                {
                    var _Rect = new Rectangle(_Bin.X * 5, _Bin.Y * 5, _Bin.Width * 5, _Bin.Height * 5);
                    g.DrawRectangle(Pens.Blue, _Rect);
                }
            }
            return Output;
        }
    }
}
