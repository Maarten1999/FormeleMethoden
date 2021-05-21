using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    public class DFA<T> : Automata<T> where T : IComparable
    {
        public DFA()
        {
        }

        public DFA(char[] symbols) : base(symbols)
        {
        }

        public DFA(SortedSet<char> symbols) : base(symbols)
        {
        }

        public override void DefineAsStartState(T t)
        {
            States.Add(t);
            StartStates.Clear();
            StartStates.Add(t);
        }

        public override bool Accept(string s)
        {
            T curState = StartStates.First();

            foreach (char c in s)
            {
                var transition = Transitions.Find(t => t.SourceState.Equals(curState) && t.Symbol == c);
                if (transition == null)
                    return false;
                curState = transition.DestState;
            }

            return FinalStates.Contains(curState);
        }
    }
}
