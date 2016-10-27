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
        public List<GridShape> YBins = new List<GridShape>();
        public List<GridShape> NewYBins = new List<GridShape>();
        public List<GridShape> XBins = new List<GridShape>();
        public List<GridShape> NewXBins = new List<GridShape>();

        public GridBin(int _Number, int _X, int _Y) : base(_Number)
        {
            MaxX = _X;
            MaxY = _Y;
            AreaContents = MaxX * MaxY;
            YBins.Add(new GridShape(MaxX, MaxY, 0, 0));
            XBins.Add(new GridShape(MaxX, MaxY, 0, 0));
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
            for (int _Index = 0; _Index < YBins.Count; _Index++)
            {
                var _Bin = YBins[_Index];
                if (_Bin.Width >= NewContent.Width && _Bin.Height >= NewContent.Height)
                {
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    Shapes.Add(NewContent);
                    MagicSplit(NewContent);
                    AreaContents -= NewContent.Area;
                    break;
                }
                else if (_Bin.Width >= NewContent.Height && _Bin.Height >= NewContent.Width)
                {
                    NewContent = new GridShape(NewContent.Height, NewContent.Width);
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    Shapes.Add(NewContent);
                    MagicSplit(NewContent);
                    AreaContents -= NewContent.Area;
                    break;
                }
            }

            for (int _Index = 0; _Index < XBins.Count; _Index++)
            {
                var _Bin = XBins[_Index];
                if (_Bin.Width >= NewContent.Width && _Bin.Height >= NewContent.Height)
                {
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    Shapes.Add(NewContent);
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    MagicSplit(NewContent);
                    AreaContents -= NewContent.Area;
                    break;
                }
                else if (_Bin.Width >= NewContent.Height && _Bin.Height >= NewContent.Width)
                {
                    NewContent = new GridShape(NewContent.Height, NewContent.Width);
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    Shapes.Add(NewContent);
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y);
                    MagicSplit(NewContent);
                    AreaContents -= NewContent.Area;
                    break;
                }
            }
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
            var remYBins = new List<GridShape>();
            var remXBins = new List<GridShape>();
            foreach (var _Bin in YBins)
            {
                if (_Bin.CollidesWith(_CurrentObject))
                {
                    SplitY(_Bin, _CurrentObject);
                    remYBins.Add(_Bin);
                }
            }
            foreach (var _Bin in remYBins)
            {
                YBins.Remove(_Bin);
            }
            foreach (var _Bin in NewYBins)
            {
                YBins.Add(_Bin);
            }
            foreach (var _Bin in XBins)
            {
                if (_Bin.CollidesWith(_CurrentObject))
                {
                    SplitX(_Bin, _CurrentObject);
                    remXBins.Add(_Bin);
                }
            }
            foreach (var _Bin in remXBins)
            {
                XBins.Remove(_Bin);
            }
            foreach (var _Bin in NewXBins)
            {
                XBins.Add(_Bin);
            }
            NewYBins = new List<GridShape>();
            NewXBins = new List<GridShape>();
        }

        private void SplitY(GridShape _Bin, GridShape _Box)
        {
            
            if (_Box.X <= _Bin.X && _Box.Y <= _Bin.Y && _Box.Height >= _Bin.Y - _Box.Y) // Case 4
            {
                var remWidth = _Box.Width - (_Bin.X - _Box.X);
                NewYBins.Add(new GridShape(_Bin.Width - remWidth, _Bin.Height, _Bin.X + remWidth, _Bin.Y));
            }

            else if (_Box.X <= _Bin.X && _Box.Y <= _Bin.Y && _Box.Width >= _Bin.X - _Box.X) // Case 5
            {
                var remHeight = _Box.Height - (_Bin.Y - _Box.Y);
                NewYBins.Add(new GridShape(_Bin.Width, _Bin.Height - remHeight, _Bin.X, _Bin.Y + remHeight));
            }

            else if (_Box.X <= _Bin.X && _Box.Y <= _Bin.Y) // Case 1
            {
                var remWidth = _Box.Width - (_Bin.X - _Box.X);
                var remHeight = _Box.Height - (_Bin.Y - _Box.Y);
                NewYBins.Add(new GridShape(remWidth, _Bin.Height - remHeight, _Bin.X, _Bin.Y + remHeight));
                NewYBins.Add(new GridShape(_Bin.Width - remWidth, _Bin.Height, _Bin.X + remWidth, _Bin.Y));
            }

            else if (_Box.X <= _Bin.X && _Box.Y > _Bin.Y && _Box.Height >= _Bin.Height - _Box.Y
               && _Box.Width >= _Bin.Width + (_Bin.X - _Box.X)) // Case 8
            {
                var remHeight = _Box.Height - (_Box.Y - _Bin.Y);
                NewYBins.Add(new GridShape(_Bin.Width, _Bin.Height - remHeight, _Bin.X, _Bin.Y));
            }

            else if (_Box.X <= _Bin.X && _Box.Y > _Bin.Y && _Box.Width >= _Bin.Width + (_Bin.X - _Box.X)) // Case 6
            {
                var lowHeight = _Box.Y - _Bin.Y;
                var highHeight = _Bin.Height - (_Box.Height + lowHeight);
                NewYBins.Add(new GridShape(_Bin.Width, lowHeight, _Bin.X, _Bin.Y));
                NewYBins.Add(new GridShape(_Bin.Width, highHeight, _Bin.X, _Box.Y + _Box.Height));
            }

            else if (_Box.X <= _Bin.X && _Box.Y > _Bin.Y && _Box.Height >= _Bin.Height - _Box.Y) // Case 7
            {
                var remWidth = _Box.Width - (_Bin.X - _Box.X);
                var remHeight = _Box.Height - (_Box.Y - _Bin.Y);
                NewYBins.Add(new GridShape(remWidth, _Bin.Height - remHeight, _Bin.X, _Bin.Y));
                NewYBins.Add(new GridShape(_Bin.Width - remWidth, _Bin.Height, _Bin.X + remWidth, _Bin.Y));
            }

            else if (_Box.X <= _Bin.X && _Box.Y > _Bin.Y) // Case 2
            {
                var remWidth = _Box.Width - (_Bin.X - _Box.X);
                var highHeight = _Bin.Height - (_Box.Height + (_Box.Y - _Bin.Y));
                var lowHeight = _Box.Y - _Bin.Y;
                NewYBins.Add(new GridShape(remWidth, highHeight, _Bin.X, _Bin.Y + (_Box.Height + lowHeight)));
                NewYBins.Add(new GridShape(remWidth, lowHeight, _Bin.X, _Bin.Y));
                NewYBins.Add(new GridShape(_Bin.Width - remWidth, _Bin.Height, _Bin.X + remWidth, _Bin.Y));
            }

            else if (_Box.Y <= _Bin.Y && _Box.X > _Bin.X && _Box.Width >= _Bin.Width - _Box.X
                && _Box.Height >= _Bin.Height + (_Bin.Y - _Box.Y)) // Case 11
            {
                var remWidth = _Box.Width - (_Box.X - _Bin.X);
                NewYBins.Add(new GridShape(_Bin.Width - remWidth, _Bin.Height, _Bin.X, _Bin.Y));
            }

            else if (_Box.Y <= _Bin.Y && _Box.X > _Bin.X && _Box.Height >= _Bin.Height + (_Bin.Y - _Box.Y)) // Case 9
            {
                var leftWidth = _Box.X - _Bin.X;
                var rightWidth = _Bin.Width - (_Box.Width + leftWidth);
                NewYBins.Add(new GridShape(leftWidth, _Bin.Height, _Bin.X, _Bin.Y));
                NewYBins.Add(new GridShape(rightWidth, _Bin.Height, _Box.X + _Box.Width, _Bin.Y));
            }

            else if (_Box.Y <= _Bin.Y && _Box.X > _Bin.X && _Box.Width >= _Bin.Width - _Box.X) // Case 10
            {
                var remWidth = _Box.Width - (_Bin.X - _Box.X);
                var remHeight = _Box.Height - (_Box.Y - _Bin.Y);
                NewYBins.Add(new GridShape(_Bin.Width - remWidth, _Bin.Height, _Bin.X, _Bin.Y));
                NewYBins.Add(new GridShape(remWidth, _Bin.Height - remHeight, _Bin.X + (_Bin.Width - remWidth), _Bin.Y + remHeight));
            }

            else if (_Box.Y <= _Bin.Y && _Box.X > _Bin.X) // Case 3
            {
                var remHeight = _Box.Height - (_Bin.Y - _Box.Y);
                var rightWidth = _Bin.Width - (_Box.Width + (_Box.X - _Bin.X));
                var leftWidth = _Box.X - _Bin.X;
                NewYBins.Add(new GridShape(rightWidth, _Bin.Height, _Bin.X, _Bin.Y));
                NewYBins.Add(new GridShape(leftWidth, _Bin.Height, _Bin.X + (_Box.Width + (_Box.X - _Bin.X)), _Bin.Y));
                NewYBins.Add(new GridShape(_Box.Width, _Bin.Height - remHeight, _Bin.X + leftWidth, _Bin.Y + remHeight));
            }
        }
        private void SplitX(GridShape _Bin, GridShape _Box)
        {
            if (_Box.X <= _Bin.X && _Box.Y <= _Bin.Y && _Box.Height >= _Bin.Y - _Box.Y) // Case 4
            {
                var remWidth = _Box.Width - (_Bin.X - _Box.X);
                NewXBins.Add(new GridShape(_Bin.Width - remWidth, _Bin.Height, _Bin.X + remWidth, _Bin.Y));
            }

            else if (_Box.X <= _Bin.X && _Box.Y <= _Bin.Y && _Box.Width >= _Bin.X - _Box.X) // Case 5
            {
                var remHeight = _Box.Height - (_Bin.Y - _Box.Y);
                NewXBins.Add(new GridShape(_Bin.Width, _Bin.Height - remHeight, _Bin.X, _Bin.Y + remHeight));
            }

            else if (_Box.X <= _Bin.X && _Box.Y <= _Bin.Y) // Case 1
            {
                var remWidth = _Box.Width - (_Bin.X - _Box.X);
                var remHeight = _Box.Height - (_Bin.Y - _Box.Y);
                NewXBins.Add(new GridShape(_Bin.Width, _Bin.Height - remHeight, _Bin.X, _Bin.Y + remHeight));
                NewXBins.Add(new GridShape(_Bin.Width - remWidth, remHeight, _Bin.X + remWidth, _Bin.Y));
            }

            else if (_Box.X <= _Bin.X && _Box.Y > _Bin.Y && _Box.Width >= _Bin.Width + (_Bin.X - _Box.X)) // Case 6
            {
                var lowHeight = _Box.Y - _Bin.Y;
                var highHeight = _Bin.Height - (_Box.Height + lowHeight);
                NewXBins.Add(new GridShape(_Bin.Width, lowHeight, _Bin.X, _Bin.Y));
                NewXBins.Add(new GridShape(_Bin.Width, highHeight, _Bin.X, _Box.Y + _Box.Height));
            }

            else if (_Box.X <= _Bin.X && _Box.Y > _Bin.Y && _Box.Height >= _Bin.Height - _Box.Y
                && _Box.Width >= _Bin.Width + (_Bin.X - _Box.X)) // Case 8
            {
                var remHeight = _Box.Height - (_Box.Y - _Bin.Y);
                NewXBins.Add(new GridShape(_Bin.Width, _Bin.Height - remHeight, _Bin.X, _Bin.Y));
            }

            else if (_Box.X <= _Bin.X && _Box.Y > _Bin.Y && _Box.Height >= _Bin.Height - _Box.Y) // Case 7
            {
                var remWidth = _Box.Width - (_Bin.X - _Box.X);
                var remHeight = _Box.Height - (_Box.Y - _Bin.Y);
                NewXBins.Add(new GridShape(_Bin.Width, _Bin.Height - remHeight, _Bin.X, _Bin.Y + remHeight));
                NewXBins.Add(new GridShape(_Bin.Width - remWidth, _Bin.Height, _Bin.X + remWidth, _Bin.Y));
            }

            else if (_Box.X <= _Bin.X && _Box.Y > _Bin.Y) // Case 2 
            {
                var remWidth = _Box.Width - (_Bin.X - _Box.X);
                var lowHeight = _Box.Y - _Bin.Y;
                var highHeight = _Bin.Height - (_Box.Height + lowHeight);

                NewXBins.Add(new GridShape(_Bin.Width, highHeight, _Bin.X, _Bin.Y + (_Box.Height + lowHeight)));
                NewXBins.Add(new GridShape(_Bin.Width, lowHeight, _Bin.X, _Bin.Y));
                NewXBins.Add(new GridShape(_Bin.Width - remWidth, _Box.Height, _Bin.X + remWidth, _Bin.Y + lowHeight));
            }

            else if (_Box.Y <= _Bin.Y && _Box.X > _Bin.X && _Box.Height >= _Bin.Height + (_Bin.Y - _Box.Y)) // Case 9
            {
                var leftWidth = _Box.X - _Bin.X;
                var rightWidth = _Bin.Width - (_Box.Width + leftWidth);
                NewXBins.Add(new GridShape(leftWidth, _Bin.Height, _Bin.X, _Bin.Y));
                NewXBins.Add(new GridShape(rightWidth, _Bin.Height, _Box.X + _Box.Width, _Bin.Y));
            }

            else if (_Box.Y <= _Bin.Y && _Box.X > _Bin.X && _Box.Width >= _Bin.Width - _Box.X
                && _Box.Height >= _Bin.Height + (_Bin.Y - _Box.Y)) // Case 11
            {
                var remWidth = _Box.Width - (_Box.X - _Bin.X);
                NewXBins.Add(new GridShape(_Bin.Width - remWidth, _Bin.Height, _Bin.X, _Bin.Y));
            }

            else if (_Box.Y <= _Bin.Y && _Box.X > _Bin.X && _Box.Width >= _Bin.Width - _Box.X) // Case 10
            {
                var remWidth = _Box.Width - (_Bin.X - _Box.X);
                var remHeight = _Box.Height - (_Box.Y - _Bin.Y);
                NewXBins.Add(new GridShape(_Bin.Width - remWidth, remHeight, _Bin.X, _Bin.Y));
                NewXBins.Add(new GridShape(_Bin.Width, _Bin.Height - remHeight, _Bin.X, _Bin.Y + remHeight));
            }

            else if (_Box.Y <= _Bin.Y && _Box.X > _Bin.X) // Case 3 
            {
                var remHeight = _Box.Height - (_Bin.Y - _Box.Y);
                var rightWidth = _Bin.Width - (_Box.Width + (_Box.X - _Bin.X));
                var leftWidth = _Box.X - _Bin.X;
                NewXBins.Add(new GridShape(leftWidth, remHeight, _Bin.X, _Bin.Y));
                NewXBins.Add(new GridShape(rightWidth, remHeight, _Bin.X + (_Box.Width + leftWidth), _Bin.Y));
                XBins.Add(new GridShape(_Box.Width, _Bin.Height - remHeight, _Bin.X, _Bin.Y + remHeight));
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
                    g.DrawRectangles(_LineBrush, new RectangleF[] { _Rect });
                    g.FillRectangles(_FillBrush, new RectangleF[] { _Rect });
                }
            }
            return Output;
        }
    }
}
