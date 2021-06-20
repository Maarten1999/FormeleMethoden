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

        /// <summary>
        /// Niet operatie.
        /// Omdraaiing in finalstates.
        /// </summary>
        /// <returns>Nieuwe DFA instantie</returns>
        public DFA<T> Not()
        {
            DFA<T> copy = this;
            SortedSet<T> newFinalStates = 
                new SortedSet<T>(copy.States.Where(s => !copy.FinalStates.Contains(s)));

            copy.FinalStates.Clear();
            copy.FinalStates.AddAll(newFinalStates);

            return copy;
        }

        /// <summary>
        /// Creeërt een tabel.
        /// </summary>
        /// <param name="transitions"></param>
        /// <returns></returns>
        private Dictionary<T, Dictionary<char, T>> CreateTable(List<Transition<T>> transitions)
        {
            Dictionary<T, Dictionary<char, T>> table = new Dictionary<T, Dictionary<char, T>>();

            foreach (var transition in transitions)
            {
                var val = new Dictionary<char, T>();
                val.Add(transition.Symbol, transition.DestState);

                if (table.ContainsKey(transition.SourceState))
                {
                    table[transition.SourceState].Add(transition.Symbol, transition.DestState);
                }
                else table.Add(transition.SourceState, val);
            }
            return table;
        }

        private Dictionary<string, Dictionary<char, string>> CreateCombinedTable(Dictionary<T, Dictionary<char, T>> L1, Dictionary<T, Dictionary<char, T>> L2)
        {
            Dictionary<string, Dictionary<char, string>> L3 = new Dictionary<string, Dictionary<char, string>>();

            foreach (var table1 in L1)
            {
                foreach (var table2 in L2)
                {
                    Dictionary<char, string> value = new Dictionary<char, string>();
                    foreach (char c in symbols)
                    {
                        value.Add(c, $"q{table1.Value[c]}q{table2.Value[c]}");
                    }
                    L3.Add($"q{table1.Key}q{table2.Key}", value);
                }
            }

            return L3;
        }

        /// <summary>
        /// Creeër transities aan de hand van een gecombineerde tabel (L3)
        /// </summary>
        /// <param name="L3"></param>
        /// <returns></returns>
        private DFA<string> CreateTableTransitions(Dictionary<string, Dictionary<char, string>> L3)
        {
            DFA<string> result = new DFA<string>();

            // Creeër dfa transitaties aan de hand van de tabel L3
            foreach (var item in L3)
            {
                foreach (var val in item.Value)
                {
                    result.AddTransition(new Transition<string>(item.Key, val.Key, val.Value));
                }
            }

            return result;
        }
        /// <summary>
        /// En operatie.
        /// </summary>
        /// <param name="dfa"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public DFA<string> And(DFA<T> dfa)
        {
            if (!symbols.SetEquals(dfa.symbols)) throw new ArgumentException("Alphabet is not the same.");

            Dictionary<T, Dictionary<char, T>> L1 = CreateTable(Transitions);
            Dictionary<T, Dictionary<char, T>> L2 = CreateTable(dfa.Transitions);

            //foreach (var res in L3)
            //{
            //    Console.WriteLine($"{res.Key} -> a = {res.Value['a']} - b = {res.Value['b']}");
            //}

            // Gecombineerde tabel van L1 en L2
            Dictionary<string, Dictionary<char, string>> L3 = CreateCombinedTable(L1, L2);

            DFA<string> result = CreateTableTransitions(L3);

            // Start state is waar beide tabellen beginnen (combinatie)
            result.DefineAsStartState($"q{StartStates.First()}q{dfa.StartStates.First()}");
            foreach (var itemL1 in L1)
            {
                if (FinalStates.Contains(itemL1.Key))
                {
                    foreach (var itemL2 in L2)
                    {
                        if (dfa.FinalStates.Contains(itemL2.Key))
                        {
                            result.DefineAsFinalState($"q{itemL1.Key}q{itemL2.Key}");
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Of operatie.
        /// </summary>
        /// <param name="dfa"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public DFA<string> Or(DFA<T> dfa)
        {
            if (!symbols.SetEquals(dfa.symbols)) throw new ArgumentException("Alphabet is not the same.");

            Dictionary<T, Dictionary<char, T>> L1 = CreateTable(Transitions);
            Dictionary<T, Dictionary<char, T>> L2 = CreateTable(dfa.Transitions);

            Dictionary<string, Dictionary<char, string>> L3 = CreateCombinedTable(L1, L2);

            DFA<string> result = CreateTableTransitions(L3);

            result.DefineAsStartState($"q{StartStates.First()}q{dfa.StartStates.First()}");

            foreach (var itemL1 in L1)
            {
                foreach (var itemL2 in L2)
                {
                    if (FinalStates.Contains(itemL1.Key) || dfa.FinalStates.Contains(itemL2.Key))
                    {
                        result.DefineAsFinalState($"q{itemL1.Key}q{itemL2.Key}");
                    }
                }
            }

            return result;
        }

    }
}
