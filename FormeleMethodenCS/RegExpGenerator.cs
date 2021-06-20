using System.Linq;

namespace FormeleMethodenCS
{
    class RegExpGenerator
    {
        public static RegExp StartsWith(string str)
        {
            RegExp regExp = new RegExp(str);
            return regExp.Caret();
        }

        public static RegExp EndsWith(string str)
        {
            RegExp regExp = new RegExp(str);
            return regExp.Dollar();
        }

        public static RegExp Contains(string str)
        {
            return new RegExp(str);
        }
    }
}