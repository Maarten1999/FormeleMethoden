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
            NDFAToDFAConverter ndfaConverter = new NDFAToDFAConverter();
            return ndfaConverter.Convert(ndfa);
        }

        public static NDFA<string> DFAToNDFA_Reverse(DFA<string> dfa)
        {
            NDFA<string> ndfa = new NDFA<string>(dfa.symbols);

            foreach (var transition in dfa.Transitions)
            {
                ndfa.AddTransition(new Transition<string>(transition.DestState, transition.Symbol, transition.SourceState));
            }

            foreach (var dfaFinalState in dfa.FinalStates)
            {
                ndfa.DefineAsStartState(dfaFinalState);
            }

            foreach (var dfaStartState in dfa.StartStates)
            {
                ndfa.DefineAsFinalState(dfaStartState);
            }

            return ndfa;
        }

        //Brzozowski's algorithm: toDFA (reverse (toDFA (reverse (dfa)))
        public static DFA<string> ToMinimalDFA(DFA<string> dfa)
        {
            NDFA<string> ndfa1 = DFAToNDFA_Reverse(dfa);
            DFA<string> dfa1 = NDFAToDFA(ndfa1);
            NDFA<string> ndfa2 = DFAToNDFA_Reverse(dfa1);
            DFA<string> dfa2 = NDFAToDFA(ndfa2);
            return dfa2;
        }

    }
}