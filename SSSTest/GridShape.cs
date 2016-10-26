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
        public GridShape(int _Width, int _Height)
        {
            Height = _Height;
            Width = _Width;
            Area = Height * Width;
        }

        public GridShape(int _Width, int _Height, int _X, int _Y)
        {
            X = _X;
            Y = _Y;
            Height = _Height;
            Width = _Width;
            Area = Height * Width;
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
            return (Math.Abs(this.X - _That.X) * 2 < (this.Width + _That.Width)) 
                && (Math.Abs(this.Y - _That.Y) * 2 < (this.Height + _That.Height));
        }


    }
}
