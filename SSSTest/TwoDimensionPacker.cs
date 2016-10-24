using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSSTest
{
    class TwoDimensionPacker
    {
        public TwoDimensionPacker()
        {

        }

        int[,] Input;
        int X;
        int Y;
        List<GridBin> Bins = new List<GridBin>();
        List<GridShape> Shapes = new List<GridShape>();
        int BinCount = 0;

        public List<GridBin> Pack(int[,] _Input, int _X, int _Y, string _Method)
        {
            Input = _Input;
            X = _X;
            Y = _Y;
            Shapes = new List<GridShape>();
            for (int _Index = 0; _Index < Input.GetLength(0); _Index++)
            {
                Shapes.Add(new GridShape(Input[_Index, 0], Input[_Index, 1]));
            }
            switch (_Method)
            {
                default:                //NextFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    NextFit();
                    break;
                case "HighLowNextFit":  //HighLowNextFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    HighLowNextFit();
                    break;
                case "LowHighNextFit":  //LowHighNextFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    LowHighNextFit();
                    break;
                case "FirstFit":        //FirstFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    FirstFit();
                    break;
                case "HighLowFirstFit": //HighLowFirstFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    HighLowFirstFit();
                    break;
                case "LowHighFirstFit": //LowHighFirstFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    LowHighFirstFit();
                    break;
                case "WorstFit":        //WorstFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    WorstFit();
                    break;
                case "HighLowWorstFit": //HighLowWorstFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    HighLowWorstFit(); break;
                case "LowHighWorstFit": //LowHighWorstFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    LowHighWorstFit(); break;
                case "BestFit":         //BestFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    BestFit();
                    break;
                case "HighLowBestFit":  //HighLowBestFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    HighLowBestFit();
                    break;
                case "LowHighBestFit":  //LowHighBestFit
                    Bins = new List<GridBin>();
                    BinCount = 0;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    LowHighBestFit();
                    break;
            }
            return Bins;
        }

        private void NextFit()
        {
            GridBin _CurrentBin = Bins[BinCount];
            for (int _Index = 0; _Index < Shapes.Count; _Index++)
            {
                var _Shape = Shapes[_Index];
                if (_CurrentBin.CheckNow(_Shape))
                    _CurrentBin.AddContent(_Shape);
                else
                {
                    BinCount++;
                    Bins.Add(new GridBin(BinCount, X, Y));
                    _CurrentBin = Bins[BinCount];
                    _CurrentBin.AddContent(_Shape);
                }
            }
        }

        private void HighLowNextFit()
        {
            Shapes = Shapes.OrderByDescending(Area => Area).ToList();
            NextFit();
        }

        private void LowHighNextFit()
        {
            Shapes = Shapes.OrderBy(Area => Area).ToList();
            NextFit();
        }

        private void FirstFit()
        {
            for (int _Index = 0; _Index < Shapes.Count; _Index++)
            {
                bool placed = false;
                var _Shape = Shapes[_Index];
                foreach(GridBin _CurrentBin in Bins)
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
                    var _CurrentBin = new GridBin(BinCount, X, Y);
                    _CurrentBin.AddContent(_Shape);
                    Bins.Add(_CurrentBin);
                }
            }
        }   

        private void HighLowFirstFit()
        {
            Shapes = Shapes.OrderByDescending(Area => Area).ToList();
            FirstFit();
        }

        private void LowHighFirstFit()
        {
            Shapes = Shapes.OrderBy(Area => Area).ToList();
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
                foreach (GridBin _CurrentBin in Bins)
                {
                    var _CurrentSpace = _CurrentBin.AreaContents;
                    if (_LeastSpace > _CurrentSpace && _CurrentBin.CheckNow(_Shape))
                    {
                        _Placable = true;
                        _LeastSpace = _CurrentSpace;
                        MinSpaceIndex = _CurrentBin.Number;
                    }
                }
                if (_Placable)
                    Bins[MinSpaceIndex].AddContent(_Shape);
                else
                {
                    BinCount++;
                    var _CurrentBin = new GridBin(BinCount, X, Y);
                    _CurrentBin.AddContent(_Shape);
                    Bins.Add(_CurrentBin);
                }
            }
        }

        private void HighLowBestFit()
        {
            Shapes = Shapes.OrderByDescending(Area => Area).ToList();
            BestFit();
        }

        private void LowHighBestFit()
        {
            Shapes = Shapes.OrderBy(Area => Area).ToList();
            BestFit();
        }

        private void WorstFit()
        {
            for (int _Index = 0; _Index < Shapes.Count; _Index++)
            {
                int _MostSpace = 0;
                int _MostSpaceIndex = 0;
                var _Shape = Shapes[_Index];
                foreach (GridBin _CurrentBin in Bins)
                {
                    var _CurrentSpace = _CurrentBin.AreaContents;
                    if (_MostSpace < _CurrentSpace && _CurrentBin.CheckNow(_Shape))
                    {
                        _MostSpace = _CurrentSpace;
                        _MostSpaceIndex = _CurrentBin.Number;
                    }
                }
                if (_MostSpace >= _Shape.Area)
                {
                    Bins[_MostSpaceIndex].AddContent(_Shape);
                }
                else
                {
                    BinCount++;
                    var _CurrentBin = new GridBin(BinCount, X, Y);
                    _CurrentBin.AddContent(_Shape);
                    Bins.Add(_CurrentBin);
                }

            }
        }

        private void HighLowWorstFit()
        {
            Shapes = Shapes.OrderByDescending(Area => Area).ToList();
            WorstFit();
        }

        private void LowHighWorstFit()
        {
            Shapes = Shapes.OrderBy(Area => Area).ToList();
            WorstFit();
        }
    }
}
