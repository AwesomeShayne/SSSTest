using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSSTest
{
    class SpaceGrid : Bin
    {
        int MaxX;
        int MaxY;
        int MaxZ;
        public int VolumeContent;
        static Random Rand = new Random();
        public List<SpaceShape> Shapes = new List<SpaceShape>();
        public List<SpaceShape> Bins = new List<SpaceShape>();

        public SpaceGrid(int _Number, int _X, int _Y, int _Z): base(_Number)
        {
            MaxX = _X;
            MaxY = _Y;
            MaxZ = _Z;
            Bins.Add(new SpaceShape(0, 0, 0, MaxX, MaxY, MaxZ));
        }

        public bool CheckNow(SpaceShape NewContent)
        {
            foreach (SpaceShape _Bin in Bins)
            {
                if (_Bin.Width >= NewContent.Width && _Bin.Height >= NewContent.Height && _Bin.Depth >= NewContent.Depth)
                    return true;
                else
                    return false;
            }
        }
    }
}
