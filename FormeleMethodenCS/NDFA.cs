using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    class NDFA<T> : Automata<T> where T : IComparable
    {
        public NDFA()
        {
        }

        public NDFA(char[] symbols) : base(symbols)
        {
        }

        public NDFA(SortedSet<char> symbols) : base(symbols)
        {
        }

        public override void DefineAsStartState(T t)
        {
            States.Add(t);
            StartStates.Add(t);
        }

        public override bool Accept(string s)
        {

            foreach (T startState in StartStates)
            {
                T curState = startState;
                bool abrupted = false;
                
                foreach (char c in s)
                {
                    bool transition_found = false;
                    do
                    {
                        var transition = Transitions.Find(t => t.SourceState.Equals(curState) && t.Symbol == c);
                        var transition_epsilon = Transitions.Find(t => t.SourceState.Equals(curState) && t.Symbol == Transition<T>.EPSILON);

                        if (transition != null)
                        {
                            transition_found = true;
                            curState = transition.DestState;
                        }
                        else if (transition_epsilon != null)
                        {
                            curState = transition_epsilon.DestState;
                        }
                        else
                        {
                            abrupted = true;
                            break;
                        }

                    } while (!transition_found);
                }

                if (FinalStates.Contains(curState) && !abrupted)
                {
                    return true;
                }
            }
      
            return false;
        }

        //public IEnumerable<T> GetToStates(T source, char symbol)
        //{
        //    foreach (Transition<T> transition in transitions)
        //    {
        //        if (transition.SourceState.Equals(source) && 
        //            (transition.Symbol.Equals(symbol) || transition.Symbol.Equals(Transition<T>.EPSILON)))
        //            yield return transition.DestState;
        //    }
        //}
    }
}
