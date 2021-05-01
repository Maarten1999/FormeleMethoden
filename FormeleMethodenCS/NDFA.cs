using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    class NDFA<T> where T : IComparable
    {
        private readonly List<Transition<T>> transitions;
        private readonly SortedSet<T> states;
        private readonly SortedSet<T> startStates;
        private readonly SortedSet<T> finalStates;
        private SortedSet<char> symbols;

        public NDFA() : this(new SortedSet<char>()) { }
        public NDFA(char[] symbols) : this(new SortedSet<char>(symbols)) { }
        public NDFA(SortedSet<char> symbols)
        {
            transitions = new List<Transition<T>>();
            states = new SortedSet<T>();
            finalStates = new SortedSet<T>();
            startStates = new SortedSet<T>();
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
            startStates.Add(t);
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
            return false;
        }
    }
}
