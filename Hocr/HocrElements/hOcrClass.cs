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
            // https://www.babelstone.co.uk/Unicode/whatisit.html
            // \U0001F60A for U+1F60A
            str = str
                .Replace("`", "'")
                .Replace("“", "\"")
                .Replace("”", "\"")
                .Replace("\u2013", "-") // en-dash
                .Replace("\u2014", "-") // em-dash
                .Replace("\u2018", "'") // curly start apos
                .Replace("\u2019", "'") // curly end apos
                .Replace("\uFB00", "ff")
                .Replace("\uFB01", "fi")
                .Replace("\uFB02", "fl")
                .Replace("\uFB03", "ffi")
                .Replace("\uFB04", "ffl")
                .Replace("\uFFFD", "ti")
                .Replace("\U0010019F", "ti") // U+10019F
                .Replace("\U0010019E", "tf") // U+10019E
                .Replace("\U0010019C", "ft") // U+10019C
                .Replace("ﬁ", "fi") // fifth \uFB01
                .Replace("ﬂ", "fl") // flower \uFB02
                .Replace("ﬃ", "ffi") // \uFB03
                .Replace("ﬄ", "ffl") // \uFB04
                .Replace("�", "ti") // \uFFFD
                .Replace("􀆟", "ti") // \U0010019F
                .Replace("􀆞", "tf") // \U0010019E - righ􀆞ul
                .Replace("􀅌", "ft"); // \U0010019C - O􀅌en

            // FLYING BU'I'I'RESS(ES) and COMEI'
            str = Regex.Replace(str, "([A-Z])('?I'I')", m => m.Groups[1].Value + "TT");
            str = Regex.Replace(str, "([A-Z])('?I')", m => m.Groups[1].Value + "T");

            // (LOUISA MAY) ALCO'IT
            str = Regex.Replace(str, "([A-Z])('IT)", m => m.Groups[1].Value + "TT");

            // SQUARE FEEl'
            str = Regex.Replace(str, "([A-Z])('?l'l')", m => m.Groups[1].Value + "TT");
            str = Regex.Replace(str, "([A-Z])('?l')", m => m.Groups[1].Value + "T");

            return str;
        }

        public static string Unescape(string str)
        {
            return str.Trim().Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&#39;", "'")
                .Replace("&#44;", "-").Replace("Ã¢â‚¬â€", "-").Replace("â€", "-").Replace("\r\n", string.Empty);
        }
    }
}