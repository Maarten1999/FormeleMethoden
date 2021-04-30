using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    class DFA<T> where T : IComparable
    {
        //public T startState;
        //public SortedSet<T> finalStates;
        //public SortedList<KeyValuePair<T, char>, T> transTable;
        private readonly List<Transition<T>> transitions;
        private readonly SortedSet<T> states;
        private T startState;
        private readonly SortedSet<T> finalStates;
        private SortedSet<char> symbols;

        public DFA() : this(new SortedSet<char>()) { }
        public DFA(char[] symbols) : this(new SortedSet<char>(symbols)) { }
        public DFA(SortedSet<char> symbols)
        {
            transitions = new List<Transition<T>>();
            states = new SortedSet<T>();
            finalStates = new SortedSet<T>();
            SetAlphabet(symbols);
        }

        private void SetAlphabet(SortedSet<char> symbols)
        {
            this.symbols = symbols;
        }

        public void AddTransition(Transition<T> t)
        {
            transitions.Add(t);
            states.Add(t.SourceState);
            states.Add(t.DestState);
        }

        public void DefineAsStartState(T t)
        {
            states.Add(t);
            startState = t;
        }

        public void DefineAsFinalState(T t)
        {
            states.Add(t);
            finalStates.Add(t);
        }
        public void PrintTransitions()
        {
            foreach (var t in transitions)
            {
                Console.WriteLine(t);
            }
        }

        public bool Accept(string s)
        {
            T curState = startState;

            CharEnumerator i = s.GetEnumerator();

            foreach (char c in s)
            {
                var transition = transitions.Find(t => t.SourceState.Equals(curState) && t.Symbol == c);
                if (transition == null)
                    return false;
                curState = transition.DestState;
            }


            return finalStates.Contains(curState);
        }

        public IEnumerable<T> GetToStates(T source, char symbol)
        {
            foreach (Transition<T> transition in transitions)
            {
                if (transition.SourceState.Equals(source) && transition.Symbol.Equals(symbol))
                    yield return transition.DestState;
            }
        }
    }
}
