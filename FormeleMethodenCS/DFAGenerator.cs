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
            string errorState = string.Format("q{0}", str.Length + 1);
            string finalState = string.Format("q{0}", str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                dfa.AddTransition(new Transition<string>("q" + i, c, string.Format("q{0}", (i + 1))));
                for (int j = 0; j < alphabet.Length; j++)
                {
                    char otherChar = alphabet[j];
                    if (otherChar != c)
                    {
                        dfa.AddTransition(new Transition<string>("q" + i, otherChar, errorState));
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

        public static DFA<string> EndsWith(string str)
        {
            char[] alphabet = str.Distinct().ToArray();
            DFA<string> dfa = new DFA<string>(alphabet);

            Dictionary<string, string> textStateCombis = new Dictionary<string, string>();
            string currentStr = "";

            for (int i = str.Length; i-- > 0;)
            {
                textStateCombis.Add(string.Format("q{0}", (i)), str.Substring(0, i));
            }

            string startState = "q0";
            string finalState = string.Format("q{0}", str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                currentStr += c;
                dfa.AddTransition(new Transition<string>("q" + i, c, string.Format("q{0}", (i + 1))));
                for (int j = 0; j < alphabet.Length; j++)
                {
                    bool addedTransition = false;
                    char otherChar = alphabet[j];

                    // skip current char
                    if (otherChar == c)
                        continue;
                    
                    // find shortcut
                    foreach (var combi in textStateCombis)
                    {
                        // Replace last char with otherChar to find possible shortcuts
                        string newStr = currentStr.Remove(currentStr.Length - 1, 1) + otherChar;
                        if ( newStr.EndsWith(combi.Value) && String.Compare("q" + i, combi.Key) >= 0)
                        {
                            addedTransition = true;
                            dfa.AddTransition(new Transition<string>("q" + i, otherChar, combi.Key));
                            break;
                        }
                    }

                    // Add Transition to startstate if shortcut is not found
                    if (!addedTransition)
                    {
                        dfa.AddTransition(new Transition<string>("q" + i, otherChar,  startState));
                    }
                }
            }

            dfa.DefineAsStartState(startState);
            dfa.DefineAsFinalState(finalState);

            return dfa;
        }

        public static DFA<string> Contains(string str)
        {
            //A contains DFA is equal to EndsWith + Loop Transitions for the Finalstates.
            DFA<string> dfa = EndsWith(str);
            char[] alphabet = str.Distinct().ToArray();

            foreach (char c in alphabet)
            {
                foreach (var finalState in dfa.FinalStates)
                {
                    dfa.AddTransition(new Transition<string>(finalState, c, finalState));
                }
                    
            }

            return dfa;
        }
    }
}
