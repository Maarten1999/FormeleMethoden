using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethoden
{
    public class Automata<T> where T : IComparable
    {
        private readonly List<Transition<T>> transitions;
        private readonly SortedSet<T> states;
        private readonly SortedSet<T> startStates;
        private readonly SortedSet<T> finalStates;
        private SortedSet<char> symbols;

        public Automata() : this(new SortedSet<char>()) { }
        public Automata(char[] symbols) : this(new SortedSet<char>(symbols)) { }
        public Automata(SortedSet<char> symbols)
        {
            transitions = new List<Transition<T>>();
            states = new SortedSet<T>();
            startStates = new SortedSet<T>();
            finalStates = new SortedSet<T>();
            SetAlphabet(symbols);
        }

        private void SetAlphabet(SortedSet<char> symbols)
        {
            this.symbols = symbols;
        }

        private void SetAlphabet(char[] symbols)
        {
            this.symbols = new SortedSet<char>(symbols);
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

        public bool IsDFA()
        {
            bool dfa = true;

            foreach (T state in states)
            {
                foreach (char s in symbols)
                {
                    dfa = dfa && GetToStates(state, s).Count() == 1;
                } // Kan eerder returned worden?
            }

            return dfa;
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
