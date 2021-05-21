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
        public readonly List<Transition<T>> transitions;
        public readonly SortedSet<T> states;
        public T startState;
        public readonly SortedSet<T> finalStates;
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

        public void PrintStates()
        {
            states.ToList().ForEach(s => Console.WriteLine(s));
        }

        public bool Accept(string s)
        {
            T curState = startState;

            foreach (char c in s)
            {
                var transition = transitions.Find(t => t.SourceState.Equals(curState) && t.Symbol == c);
                if (transition == null)
                    return false;
                curState = transition.DestState;
            }

            return finalStates.Contains(curState);
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
                if (transition.SourceState.Equals(source) && transition.Symbol.Equals(symbol))
                    yield return transition.DestState;
            }
        }

        //public void Recursive(T startState, int steps)
        //{
        //    var startTransitions = transitions.Where(t => t.SourceState.Equals(startState));

        //    foreach (Transition<T> transition in startTransitions)
        //    {
        //        int count = steps;
        //        string word = "";
        //        for (int i = 0; i < steps; i++)
        //        {
        //            transition.
        //        }
        //    }

        //}
        //public List<string> GetTest(int steps)
        //{
        //    HashSet<Transition<T>> processed = new HashSet<Transition<T>>();
        //    List<Transition<T>> unCheckedTransitions = new List<Transition<T>>(transitions);

        //    var startTransitions = transitions.Where(t => t.SourceState.Equals(startState));

        //    List<string> words = new List<string>();
        //    foreach (var t in startTransitions)
        //    {
        //        Transition<T> current = t;
        //        string word = "";
        //        for (int i = 0; i < (steps); i++)
        //        {
        //            T destState = current.DestState;
        //            word += current.Symbol;
        //            var trans = transitions.Find(x => x.SourceState.Equals(destState));
        //            current = trans;
        //        }
        //        words.Add(word);
        //    }
        //    return words;
        //    //Transition<T> current = transition;
        //    //string word = "";
        //    //for (int i = 0; i < (steps-1); i++)
        //    //{
        //    //    T destState = current.DestState;
        //    //    word += current.Symbol;
        //    //    var trans = transitions.Find(t => t.SourceState.Equals(destState));
        //    //    current = trans;
        //    //    //processed.Add(trans);
        //    //}


        //}
    }
}
