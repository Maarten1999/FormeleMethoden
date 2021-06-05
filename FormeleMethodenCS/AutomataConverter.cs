using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using FormeleMethodenCS.Converters;

namespace FormeleMethodenCS
{
    class AutomataConverter
    {
        public static NDFA<string> RegexToNDFA(RegExp regex)
        {
            char[] alphabet = Regex.Replace(regex.ToString(), @"[^\w\.]", "").Distinct().ToArray();
            
            ThompsonConstruction tc = new ThompsonConstruction(alphabet);
            return tc.Convert(regex);
        }

        public static DFA<string> NDFAToDFA(NDFA<string> ndfa)
        {
            // TODO uitwerken
            return null;
        }

    }
}