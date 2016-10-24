using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSSTest
{
    class Bin
    {
        public int Number;
        int Max;
        public int TotalContent = 0;
        public List<int> Contents = new List<int>();

        public Bin(int _Number, int _Max)
        {
            Number = _Number;
            Max = _Max;
            
        }
        public Bin(int _Number)
        {
            Number = _Number;
        }

        public bool CheckNew(int NewContent)
        { 
            if (TotalContent + NewContent < Max)
                return true;
            else
                return false;
        }

        public void AddContent(int NewContent)
        {
            Contents.Add(NewContent);
            TotalContent = Contents.Sum();
        }

        public bool IsFull()
        {
            var TotalContent = Contents.Sum();
            if (TotalContent == Max)
                return true;
            else
                return false;
        }

        public int SpaceRemaining()
        {
            return Max - TotalContent;
        }
    }
}
