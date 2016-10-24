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
        public int VolumeContents;
        static Random Rand = new Random();
        public List<SpaceShape> Shapes = new List<SpaceShape>();
        public List<SpaceShape> Bins = new List<SpaceShape>();

        public SpaceGrid(int _Number, int _X, int _Y, int _Z): base(_Number)
        {
            MaxX = _X;
            MaxY = _Y;
            MaxZ = _Z;
            VolumeContents = MaxX * MaxY * MaxZ;
            Bins.Add(new SpaceShape(0, 0, 0, MaxX, MaxY, MaxZ));
        }

        public bool CheckNow(SpaceShape NewContent)
        {
            foreach (SpaceShape _Bin in Bins)
            {
                if (_Bin.Width >= NewContent.Width && _Bin.Height >= NewContent.Height && _Bin.Depth >= NewContent.Depth)
                    return true;
            }
            return false;
        }

        public void AddContent(SpaceShape NewContent)
        {
            for (int _Index = 0; _Index < Bins.Count; _Index++)
            {
                var _Bin = Bins[_Index];
                if (_Bin.Width >= NewContent.Width && _Bin.Height >= NewContent.Height && _Bin.Depth >= NewContent.Depth)
                {
                    NewContent.AssignOrigin(_Bin.X, _Bin.Y, _Bin.Z);
                    Shapes.Add(NewContent);
                    Split(_Bin, NewContent, _Index);
                    VolumeContents -= NewContent.Volume;
                    break;
                }
            }
        }

        private void Split(SpaceShape _CurrentBin, SpaceShape _CurrentObject, int _Index)
        {
            var _X = _CurrentBin.X;
            var _Y = _CurrentBin.Y;
            var _Z = _CurrentBin.Z;
            var _OriginalHeight = _CurrentBin.Height;
            var _OriginalWidth = _CurrentBin.Width;
            var _OriginalDepth = _CurrentBin.Depth;
            var _RemovedHeight = _CurrentObject.Height;
            var _RemovedWidth = _CurrentObject.Width;
            var _RemovedDepth = _CurrentObject.Depth;
            Bins.RemoveAt(_Index);
            Bins.Add(new SpaceShape(_X + _RemovedWidth, _Y, _Z, _OriginalWidth - _RemovedWidth, _OriginalHeight, _OriginalDepth));
            Bins.Add(new SpaceShape(_X, _Y + _RemovedHeight, _Z, _OriginalWidth, _OriginalHeight - _RemovedHeight, _OriginalDepth));
            Bins.Add(new SpaceShape(_X, _Y, _Z + _RemovedDepth, _OriginalWidth, _OriginalHeight, _OriginalDepth - _RemovedDepth));
            Bins.Add(new SpaceShape(_X + _RemovedWidth, _Y + _RemovedHeight, _Z, _OriginalWidth - _RemovedWidth, _OriginalHeight - _RemovedHeight, _OriginalDepth));
            Bins = Bins.OrderByDescending(Volume => Volume).ToList();

        }
    }
}
