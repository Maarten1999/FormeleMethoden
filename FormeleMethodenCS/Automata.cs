using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    abstract class Automata<T> where T : IComparable
    {
        protected readonly List<Transition<T>> transitions;
        protected readonly SortedSet<T> states;
        protected readonly SortedSet<T> finalStates;

        protected SortedSet<char> symbols;
        protected void SetAlphabet(SortedSet<char> symbols)
        {
            this.symbols = symbols;
        }
        public void AddTransition(Transition<T> t)
        {
            transitions.Add(t);
            states.Add(t.SourceState);
            states.Add(t.DestState);
        }

        public abstract void DefineAsStartState(T t);

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
        public void PrintStates()
        {
            states.ToList().ForEach(s => Console.WriteLine(s));
        }

        public abstract bool Accept(string s);

        public IEnumerable<T> GetToStates(T source, char symbol)
        {
            foreach (Transition<T> transition in transitions)
            {
                if (transition.SourceState.Equals(source) && transition.Symbol.Equals(symbol))
                    yield return transition.DestState;
            }
        }
        /// <summary>
        /// Geeft alle mogelijke woorden met het alfabet en de ingegeven lengte.
        /// </summary>
        /// <param name="length">Lengte van woorden.</param>
        /// <param name="accepts">Of de woorden geaccepteerd moeten worden.</param>
        /// <returns></returns>
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
        protected static IEnumerable<IEnumerable<U>> GetPermutations<U>(IEnumerable<U> list, int length)
        {
            if (length == 1) return list.Select(t => new U[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list,
                    (t1, t2) => t1.Concat(new U[] { t2 }));
        }
    }
}
