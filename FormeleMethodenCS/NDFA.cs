using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    class NDFA<T> where T : IComparable
    {
        public readonly List<Transition<T>> transitions;
        public SortedSet<T> states { get; }
        public readonly SortedSet<T> startStates;
        public readonly SortedSet<T> finalStates;
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

            foreach (T startState in startStates)
            {
                T curState = startState;
                bool abrupted = false;
                
                foreach (char c in s)
                {
                    bool transition_found = false;
                    do
                    {
                        var transition = transitions.Find(t => t.SourceState.Equals(curState) && t.Symbol == c);
                        var transition_epsilon = transitions.Find(t => t.SourceState.Equals(curState) && t.Symbol == Transition<T>.EPSILON);

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

                if (finalStates.Contains(curState) && !abrupted)
                {
                    return true;
                }
            }
      
            return false;
        }

        public IEnumerable<string> GetLanguage(int length, bool accepts = false)
        {
            var permutations = GetPermutations(symbols, length);
            foreach (var i in permutations)
            {
                string word = new string(i.ToArray());

                if ((accepts && Accept(word)) || !accepts)
                    yield return word;
            }
        }

        /// <summary>
        /// Recursieve functie voor het berekenen van alle mogelijke combinatie's gegeven een lengte.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="list">Lijst met items.</param>
        /// <param name="length">Lengte van combinaties.</param>
        /// <returns></returns>
        private static IEnumerable<IEnumerable<U>> GetPermutations<U>(IEnumerable<U> list, int length)
        {
            if (length == 1) return list.Select(t => new U[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list,
                    (t1, t2) => t1.Concat(new U[] { t2 }));
        }

        public IEnumerable<T> GetToStates(T source, char symbol)
        {
            foreach (Transition<T> transition in transitions)
            {
                if (transition.SourceState.Equals(source) && 
                    (transition.Symbol.Equals(symbol) || transition.Symbol.Equals(Transition<T>.EPSILON)))
                    yield return transition.DestState;
            }
        }
    }
}
