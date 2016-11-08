using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSSTest
{
    class GridShape : IComparable<GridShape>
    {
        public int X;
        public int Y;
        public int Height;
        public int Width;
        public int Area;
        public int Number;
        public GridShape(int _Width, int _Height)
        {
            Height = _Height;
            Width = _Width;
            Area = Height * Width;
        }

        public GridShape(int _X, int _Y, int _Width, int _Height)
        {
            /*if (_Width <= 0 || _Height <= 0)
                throw new NotImplementedException();*/
            X = _X;
            Y = _Y;
            Height = _Height;
            Width = _Width;
            Area = Height * Width;
        }
        public static GridShape Create(int _X, int _Y, int _Width, int _Height)
        {
            if (_Height == 0 || _Width == 0)
                return null;
            return new GridShape(_X, _Y, _Width, _Height);
        }
        public int CompareTo(GridShape that)
        {
            if (this.Area > that.Area) return -1;
            if (this.Area == that.Area) return 0;
            return 1;
        }
        public void AssignOrigin(int _X, int _Y)
        {
            X = _X;
            Y = _Y;
        }

        public bool CollidesWith(GridShape _That)
        {
            return !(X + Width < _That.X || _That.X + _That.Width < X || 
                Y + Height < _That.Y || _That.Y + _That.Height < Y);
        }

        public void SetNumber(int _Number)

        {
            Number = _Number;
        }
    }
}
