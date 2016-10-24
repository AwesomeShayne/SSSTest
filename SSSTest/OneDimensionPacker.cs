using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSSTest
{
    class OneDimensionPacker
    {
        public OneDimensionPacker()
        {

        }
        int[] Input;
        int Max;
        int Length;
        List<Bin> Bins = new List<Bin>();
        int BinCount = 0;
        public List<string> Pack(int[,] _Input, int[] _Max, string _Method)
        {
            Input = new int[_Input.GetLength(0)];
            for (int _Index = 0; _Index < _Input.GetLength(0); _Index++)
                Input[_Index] = _Input[_Index, 0];
            Max = _Max[0];
            Length = _Input.GetLength(0);
            switch (_Method)
            {
                default:                //NextFit
                    Bins = new List<Bin>();
                    BinCount = 0;
                    Bins.Add(new Bin(BinCount, Max));
                    NextFit();
                    break;
                case "HighLowNextFit":  //HighLowNextFit
                    Bins = new List<Bin>();
                    BinCount = 0;
                    Bins.Add(new Bin(BinCount, Max));
                    HighLowNextFit();
                    break;
                case "LowHighNextFit":  //LowHighNextFit
                    Bins = new List<Bin>();
                    BinCount = 0;
                    Bins.Add(new Bin(BinCount, Max));
                    LowHighNextFit();
                    break;
                case "FirstFit":        //FirstFit
                    Bins = new List<Bin>();
                    BinCount = 0;
                    Bins.Add(new Bin(BinCount, Max));
                    FirstFit();
                    break;
                case "HighLowFirstFit": //HighLowFirstFit
                    Bins = new List<Bin>();
                    BinCount = 0;
                    Bins.Add(new Bin(BinCount, Max));
                    HighLowFirstFit();
                    break;
                case "LowHighFirstFit": //LowHighFirstFit
                    Bins = new List<Bin>();
                    BinCount = 0;
                    Bins.Add(new Bin(BinCount, Max));
                    LowHighFirstFit();
                    break;
                case "WorstFit":        //WorstFit
                    Bins = new List<Bin>();
                    BinCount = 0;
                    Bins.Add(new Bin(BinCount, Max));
                    WorstFit();
                    break;
                case "HighLowWorstFit": //HighLowWorstFit
                    Bins = new List<Bin>();
                    BinCount = 0;
                    Bins.Add(new Bin(BinCount, Max));
                    HighLowWorstFit(); break;
                case "LowHighWorstFit": //LowHighWorstFit
                    Bins = new List<Bin>();
                    BinCount = 0;
                    Bins.Add(new Bin(BinCount, Max));
                    LowHighWorstFit(); break;
                case "BestFit":         //BestFit
                    Bins = new List<Bin>();
                    BinCount = 0;
                    Bins.Add(new Bin(BinCount, Max));
                    BestFit();
                    break;
            }
            List<string> _Output = new List<string>();
            foreach (Bin _Bin in Bins)
            {
                string _OutputLine = "\nBin ";
                _OutputLine = String.Concat(_OutputLine, _Bin.Number.ToString());
                _OutputLine = String.Concat(_OutputLine, " contains: ");
                foreach (int _Content in _Bin.Contents)
                    _OutputLine = String.Concat(_OutputLine, _Content.ToString(), ",");
                _Output.Add(_OutputLine);
            }
            _Output.Add(String.Concat("\nThis test used ", BinCount, " bins."));

            return _Output;
        }
        private void NextFit()
        {
            Bin _CurrentBin = Bins[BinCount];
            for (int _Index = 0; _Index < Length; _Index++)
            {
                var _Current = Input[_Index];
                if (_CurrentBin.CheckNew(_Current))
                    _CurrentBin.AddContent(_Current);
                else
                {
                    BinCount++;
                    Bins.Add(new Bin(BinCount, Max));
                    _CurrentBin = Bins[BinCount];
                    _CurrentBin.AddContent(_Current);
                }
            }
        }

        private void HighLowNextFit()
        {
            Input = Input.OrderByDescending(c => c).ToArray();
            NextFit();
        }

        private void LowHighNextFit()
        {
            Input = Input.OrderBy(c => c).ToArray();
            NextFit();
        }

        private void FirstFit()
        {
            
            for (int _Index = 0; _Index < Length; _Index++)
            {
                bool placed = false;
                var _Current = Input[_Index];
                foreach (Bin _CurrentBin in Bins)
                {
                    if (_CurrentBin.CheckNew(_Current))
                    {
                        _CurrentBin.AddContent(_Current);
                        placed = true;
                        break;
                    }
                }
                if (!placed)
                {
                    BinCount++;
                    var _CurrentBin = new Bin(BinCount, Max);
                    _CurrentBin.AddContent(_Current);
                    Bins.Add(_CurrentBin);
                }
            }
        }

        private void HighLowFirstFit()
        {
            Input = Input.OrderByDescending(c => c).ToArray();
            FirstFit();
        }

        private void LowHighFirstFit()
        {
            Input = Input.OrderBy(c => c).ToArray();
            FirstFit();
        }

        private void BestFit()
        {
            for (int _Index = 0; _Index < Length; _Index++)
            {
                bool _Placable = false;
                int _LeastSpace = Max;
                int MinSpaceIndex = 0;
                var _Current = Input[_Index];
                for(int _Index1 = 0; _Index1 < Bins.Count; _Index1++)
                {
                    var _CurrentBin = Bins[_Index1];
                    var _CurrentSpace = _CurrentBin.SpaceRemaining();
                    if (_LeastSpace > _CurrentSpace && _CurrentSpace >= _Current)
                    {
                        _Placable = true;
                        _LeastSpace = _CurrentSpace;
                        MinSpaceIndex = _Index1;
                    }
                }
                if (_Placable)
                {
                    Bins[MinSpaceIndex].AddContent(_Current);
                }
                else
                {
                    BinCount++;
                    var _CurrentBin = new Bin(BinCount, Max);
                    _CurrentBin.AddContent(_Current);
                    Bins.Add(_CurrentBin);
                }
            }
        }
        private void WorstFit()
        {
            for (int _Index = 0; _Index < Length; _Index++)
            {
                
                int MostSpace = 0;
                int MaxSpaceIndex = 0;
                var _Current = Input[_Index];
                for (int _Index1 = 0; _Index1 < Bins.Count; _Index1++)
                {
                    var _CurrentBin = Bins[_Index1];
                    var _CurrentSpace = _CurrentBin.SpaceRemaining();
                    if (MostSpace < _CurrentSpace)
                    {
                        MostSpace = _CurrentSpace;
                        MaxSpaceIndex = _Index1;
                    }
                }
                if (MostSpace >= _Current)
                {
                    Bins[MaxSpaceIndex].AddContent(_Current);
                }
                else
                {
                    BinCount++;
                    var _CurrentBin = new Bin(BinCount, Max);
                    _CurrentBin.AddContent(_Current);
                    Bins.Add(_CurrentBin);
                }
            }
        }

        private void HighLowWorstFit()
        {
            Input = Input.OrderByDescending(c => c).ToArray();
            WorstFit();
        }

        private void LowHighWorstFit()
        {
            Input = Input.OrderBy(c => c).ToArray();
            WorstFit();
        }
    }
}
