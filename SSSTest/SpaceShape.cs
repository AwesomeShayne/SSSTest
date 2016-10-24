using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSSTest
{
    class SpaceShape : IComparable<SpaceShape>
    {
        public int X;
        public int Y;
        public int Z;
        public int Width;
        public int Height;
        public int Depth;
        public int Volume;

        public SpaceShape(int _Width, int _Height, int _Depth)
        {
            Width = _Width;
            Height = _Height;
            Depth = _Depth;
            Volume = Width * Height * Depth;
        }

        public SpaceShape(int _X, int _Y, int _Z, int _Width, int _Height, int _Depth)
        {
            X = _X;
            Y = _Y;
            Z = _Z;
            Width = _Width;
            Height = _Height;
            Depth = _Depth;
            Volume = Width * Height * Depth;
        }

        public int CompareTo(SpaceShape that)
        {
            if (this.Volume > that.Volume) return -1;
            if (this.Volume == that.Volume) return 0;
            return 1;
        }

        public void AssignOrigin(int _X, int _Y, int _Z)
        {
            X = _X;
            Y = _Y;
            Z = _Z;
        }
    }
}
