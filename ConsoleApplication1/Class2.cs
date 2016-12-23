using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class EligibleField
    {
        public byte EligibleRow { set; get; }
        public byte EligibleColumn { set; get; }
    }
    class Lists
    {
        public static List<Figure> FigList = new List<Figure>();
        public static List<EligibleField> ProperFields = new List<EligibleField>();
    }
}
