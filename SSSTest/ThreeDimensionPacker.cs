using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSSTest
{
    class ThreeDimensionPacker
    {

        int[,] Input;
        int X;
        int Y;
        int Z;
        List<SpaceGrid> Bins = new List<SpaceGrid>();
        List<SpaceShape> Shapes = new List<SpaceShape>();
        int BinCount = 0;
        public ThreeDimensionPacker()
        {
        }

        public List<SpaceGrid> Pack(int[,] _Input, int _X, int _Y, int _Z, string _Method)
        {
            Input = _Input;
            X = _X;
            Y = _Y;
            Z = _Z;
            Shapes = new List<SpaceShape>();
            for (int _Index = 0; _Index < Input.GetLength(0); _Index++)
            {
                Shapes.Add(new SpaceShape(Input[_Index, 0], Input[_Index, 1], Input[_Index, 2]));
            }
            switch (_Method)
            {
                default:                //NextFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    NextFit();
                    break;
                case "HighLowNextFit":  //HighLowNextFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    HighLowNextFit();
                    break;
                case "LowHighNextFit":  //LowHighNextFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    LowHighNextFit();
                    break;
                case "FirstFit":        //FirstFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    FirstFit();
                    break;
                case "HighLowFirstFit": //HighLowFirstFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    HighLowFirstFit();
                    break;
                case "LowHighFirstFit": //LowHighFirstFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    LowHighFirstFit();
                    break;
                case "WorstFit":        //WorstFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    WorstFit();
                    break;
                case "HighLowWorstFit": //HighLowWorstFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    HighLowWorstFit();
                    break;
                case "LowHighWorstFit": //LowHighWorstFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    LowHighWorstFit();
                    break;
                case "BestFit":         //BestFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    BestFit();
                    break;
                case "HighLowBestFit":  //HighLowBestFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    HighLowBestFit();
                    break;
                case "LowHighBestFit":  //LowHighBestFit
                    Bins = new List<SpaceGrid>();
                    BinCount = 0;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    LowHighBestFit();
                    break;
            }
            return Bins;
        }

        private void NextFit()
        {
            SpaceGrid _CurrentBin = Bins[BinCount];
            for (int _Index = 0; _Index < Shapes.Count; _Index++)
            {
                var _Shape = Shapes[_Index];
                if (_CurrentBin.CheckNow(_Shape))
                    _CurrentBin.AddContent(_Shape);
                else
                {
                    BinCount++;
                    Bins.Add(new SpaceGrid(BinCount, X, Y, Z));
                    _CurrentBin = Bins[BinCount];
                    _CurrentBin.AddContent(_Shape);
                }
            }
        }

        private void HighLowNextFit()
        {
            Shapes = Shapes.OrderByDescending(Volume => Volume).ToList();
            NextFit();
        }

        private void LowHighNextFit()
        {
            Shapes = Shapes.OrderBy(Volume => Volume).ToList();
            NextFit();
        }

        private void FirstFit()
        {
            for (int _Index = 0; _Index < Shapes.Count; _Index++)
            {
                bool placed = false;
                var _Shape = Shapes[_Index];
                foreach (SpaceGrid _CurrentBin in Bins)
                {
                    if (_CurrentBin.CheckNow(_Shape))
                    {
                        _CurrentBin.AddContent(_Shape);
                        placed = true;
                        break;
                    }
                }
                if (!placed)
                {
                    BinCount++;
                    var _CurrentBin = new SpaceGrid(BinCount, X, Y, Z);
                    _CurrentBin.AddContent(_Shape);
                    Bins.Add(_CurrentBin);
                }
            }
        }

        private void HighLowFirstFit()
        {
            Shapes = Shapes.OrderByDescending(Volume => Volume).ToList();
            FirstFit(); 
        }

        private void LowHighFirstFit()
        {
            Shapes = Shapes.OrderBy(Volume => Volume).ToList();
            FirstFit(); 
        }

        private void BestFit()
        {
            for (int _Index = 0; _Index < Shapes.Count; _Index++)
            {
                bool _Placable = false;
                int _LeastSpace = X * Y;
                int MinSpaceIndex = 0;
                var _Shape = Shapes[_Index];
                for (int _Index1 = 0; _Index1 < Bins.Count; _Index1++)
                {
                    var _CurrentBin = Bins[_Index1];
                    var _CurrentSpace = _CurrentBin.VolumeContents;
                    if (_LeastSpace > _CurrentSpace && _CurrentBin.CheckNow(_Shape))
                    {
                        _Placable = true;
                        _LeastSpace = _CurrentSpace;
                        MinSpaceIndex = _Index1;
                    }
                }
                if (_Placable)
                    Bins[MinSpaceIndex].AddContent(_Shape);
                else
                {
                    BinCount++;
                    var _CurrentBin = new SpaceGrid(BinCount, X, Y, Z);
                    _CurrentBin.AddContent(_Shape);
                    Bins.Add(_CurrentBin);
                }
            }
        }

        private void HighLowBestFit()
        {
            Shapes = Shapes.OrderByDescending(Volume => Volume).ToList();
            BestFit();
        }

        private void LowHighBestFit()
        {
            Shapes = Shapes.OrderBy(Volume => Volume).ToList();
            BestFit();
        }

        private void WorstFit()
        {
            for (int _Index = 0; _Index < Shapes.Count; _Index++)
            {
                int _MostSpace = 0;
                int _MostSpaceIndex = 0;
                var _Shape = Shapes[_Index];
                for (int _Index1 = 0; _Index1 < Bins.Count; _Index1++)
                {
                    var _CurrentBin = Bins[_Index1];
                    var _CurrentSpace = _CurrentBin.VolumeContents;
                    if (_MostSpace < _CurrentSpace && _CurrentBin.CheckNow(_Shape))
                    {
                        _MostSpace = _CurrentSpace;
                        _MostSpaceIndex = _Index1;
                    }
                }
                if (_MostSpace >= _Shape.Volume)
                {
                    Bins[_MostSpaceIndex].AddContent(_Shape);
                }
                else
                {
                    BinCount++;
                    var _CurrentBin = new SpaceGrid(BinCount, X, Y, Z);
                    _CurrentBin.AddContent(_Shape);
                    Bins.Add(_CurrentBin);
                }

            }
        }

        private void HighLowWorstFit()
        {
            Shapes = Shapes.OrderByDescending(Volume => Volume).ToList();
            WorstFit();
        }

        private void LowHighWorstFit()
        {
            Shapes = Shapes.OrderBy(Volume => Volume).ToList();
            WorstFit();
        }
    }
}
