using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    public abstract class Automata<T> where T : IComparable
    {
        public List<Transition<T>> Transitions { get; protected set; }
        //protected List<Transition<T>> transitions;
        public SortedSet<T> States { get; protected set; }
        //protected SortedSet<T> states;
        public SortedSet<T> StartStates { get; protected set; }

        public SortedSet<T> FinalStates { get; protected set; }
        //protected SortedSet<T> finalStates;

        public SortedSet<char> symbols;

        public Automata() : this(new SortedSet<char>()) { }
        public Automata(char[] symbols) : this(new SortedSet<char>(symbols)) { }
        public Automata(SortedSet<char> symbols)
        {
            Transitions = new List<Transition<T>>();
            States = new SortedSet<T>();
            FinalStates = new SortedSet<T>();
            StartStates = new SortedSet<T>();
            SetAlphabet(symbols);
        }

        public abstract void DefineAsStartState(T t);

        public abstract bool Accept(string s);

        protected void SetAlphabet(SortedSet<char> symbols)
        {
            this.symbols = symbols;
        }

        public void AddTransition(Transition<T> t)
        {
            Transitions.Add(t);
            States.Add(t.SourceState);
            States.Add(t.DestState);
        }

        public void DefineAsFinalState(T t)
        {
            States.Add(t);
            FinalStates.Add(t);
        }

        public void PrintTransitions()
        {
            foreach (var t in Transitions)
            {
                Console.WriteLine(t);
            }
        }

        public void PrintStates()
        {
            States.ToList().ForEach(s => Console.WriteLine(s));
        }

        public bool Equivalent(Automata<T> a)
        {
            int length = a.States.Count;

//            IEnumerable<string> language = GetLanguage(length, true);
//            IEnumerable<string> alanguage = a.GetLanguage(length, true);
//
//            foreach (var s in language)
//            {
//                Console.Write(s + ", ");
//            }
//            Console.WriteLine();
//            foreach (var s in alanguage)
//            {
//                Console.Write(s + ", ");
//            }
//            Console.WriteLine();
//
//            return language.SequenceEqual(alanguage);
            return GetLanguage(length, true).SequenceEqual(a.GetLanguage(length, true));
        }
        

        public IEnumerable<T> GetToStates(T source, char symbol)
        {
            foreach (Transition<T> transition in Transitions)
            {
                if (transition.SourceState.Equals(source) && (transition.Symbol.Equals(symbol)
                    || transition.Symbol.Equals(Transition<T>.EPSILON)))
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
            for (int j = 1; j <= length; j++)
            {
                var permutations = GetPermutations(symbols, j);
                foreach (var i in permutations)
                {
                    string word = new string(i.ToArray());

                    if ((accepts && Accept(word)) || !accepts)
                        yield return word;
                }
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
