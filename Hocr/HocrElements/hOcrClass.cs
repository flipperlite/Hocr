using System.Text.RegularExpressions;

namespace Net.FairfieldTek.Hocr.HocrElements
{
    public class HOcrClass
    {
        public BBox BBox { get; set; }
        public string ClassName { get; set; }
        public string Id { get; set; }

        public string Text { get; set; }

        public string TextUnescaped => ReplaceBadOcr(Unescape(Text));

        public void CleanText()
        {
            if (Text == null)
                return;

            string results = ReplaceBadOcr(Unescape(Text));

            Text = results;
        }

        public override string ToString() { return string.Concat("Id: ", Id, "[", BBox.ToString(), "] Text: ", Text); }

        public static string ReplaceBadOcr(string str)
        {
            // https://practicaltypography.com/straight-and-curly-quotes.html
            str = str.Replace("‘", "'");
            str = str.Replace("’", "'");
            str = str.Replace("“", "\"");
            str = str.Replace("”", "\"");

            // FLYING BU'I'I'RESS(ES) and COMEI'
            str = Regex.Replace(str, "([A-Z])('?I'I')", m => m.Groups[1].Value + "TT");
            str = Regex.Replace(str, "([A-Z])('?I')", m => m.Groups[1].Value + "T");

            // Special ASCII characters
            str = str.Replace("ﬁ", "fi");
            return str;
        }

        public static string Unescape(string str)
        {
            return str.Trim().Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&#39;", "'")
                .Replace("&#44;", "-").Replace("Ã¢â‚¬â€", "-").Replace("â€", "-").Replace("\r\n", string.Empty);
        }
    }
}