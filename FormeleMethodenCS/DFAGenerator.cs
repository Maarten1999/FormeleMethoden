using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    class DFAGenerator
    {
        public static DFA<string> StartsWith(string str)
        {
            char[] alphabet = str.Distinct().ToArray();
            DFA<string> dfa = new DFA<string>(alphabet);

            string startState = "q0";
            string errorState = string.Format("q{0}", str.Length+1);
            string finalState = string.Format("q{0}", str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                dfa.AddTransition(new Transition<string>("q"+i, c, string.Format("q{0}", (i+1))));
                for (int j = 0; j < alphabet.Length; j++)
                {
                    char otherChar = alphabet[j];
                    if (otherChar != c)
                    {
                        dfa.AddTransition(new Transition<string>("q"+i, otherChar, errorState));
                    }
                }              
            }
            // Fuik states maken voor final en error
            foreach (char c in alphabet)
            {
                dfa.AddTransition(new Transition<string>(finalState, c, finalState));
                dfa.AddTransition(new Transition<string>(errorState, c, errorState));
            }
            dfa.DefineAsStartState(startState);
            dfa.DefineAsFinalState(finalState);

            return dfa;
        }
    }
}
