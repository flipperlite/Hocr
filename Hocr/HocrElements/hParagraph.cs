using System.Collections.Generic;

namespace Net.FairfieldTek.Hocr.HocrElements
{
    public class HParagraph : HOcrClass
    {
        public HParagraph() { Lines = new List<HLine>(); }

        public IList<HLine> Lines { get; set; }
    }
}