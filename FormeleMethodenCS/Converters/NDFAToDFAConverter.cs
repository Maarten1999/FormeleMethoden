using System;
using System.Collections.Generic;
using System.Linq;

namespace FormeleMethodenCS.Converters
{
    class NDFAToDFAConverter
    {
        private DFA<string> dfa;
        private NDFA<string> ndfa;

        public NDFAToDFAConverter() {}

        public DFA<string> Convert(NDFA<string> ndfa)
        {
            this.ndfa = ndfa;
            this.dfa = new DFA<string>(ndfa.symbols);

            // Create initial situations with ndfa transitions
            List<Situation> baseSitus = new List<Situation>();
            foreach (var s in ndfa.States)
            {
                Situation situ = new Situation(new SortedSet<string>(){s}, ndfa.symbols);
                GetAllPaths(situ, ndfa.Transitions, s);
                baseSitus.Add(situ);

                Console.WriteLine(situ.ToString()); //debug
            }

            // find transitions for all new states
            bool AllStatesFound = false;

            while (!AllStatesFound)
            {
                AllStatesFound = true;
                List<Situation> newSitus = new List<Situation>();
                foreach (var situ in baseSitus)
                {
                    foreach (var pair in situ.combi)
                    {
                        if (!SituationExists(pair.Value, baseSitus))
                        {
                            newSitus.Add(FillNewSituation(new Situation(pair.Value, ndfa.symbols), baseSitus));
                            AllStatesFound = false;
                        }
                    }
                }
                baseSitus.AddRange(newSitus);
            }

            // create new DFA from situations
            foreach (var situ in baseSitus)
            {
                foreach (var pair in situ.combi)
                {
                    string destState = (pair.Value.Count > 0) ? string.Join("", pair.Value) : " ";
                    Transition<string> ts = new Transition<string>(situ.GetDisplayName(), pair.Key, destState);
                    if (!dfa.Transitions.Contains(ts))
                    {
                        dfa.AddTransition(ts);
                    }
                }

                if (ndfa.FinalStates.Overlaps(situ.name))
                {
                    dfa.DefineAsFinalState(situ.GetDisplayName());
                }
            }

            foreach (var ndfaStartState in ndfa.StartStates)
            {
                if (dfa.States.Contains(ndfaStartState))
                {
                    dfa.DefineAsStartState(ndfaStartState);
                    break; // only one startstate is required
                }
            }
            return dfa;
        }

        private Situation GetAllPaths(Situation situ, List<Transition<string>> transitions, string state)
        {
            foreach (var ts in transitions)
            {
                if (state == ts.SourceState)
                {
                    if ( ts.Symbol == Transition<string>.EPSILON )
                    {
                        if (ts.SourceState != ts.DestState)
                        {
                            situ = GetAllPaths(situ, transitions, ts.DestState);
                        }
                    }
                    else
                    {
                        situ.combi[ts.Symbol].Add(ts.DestState);
                    }
                }
            }

            return situ;
        }

        private bool SituationExists(SortedSet<string> name, List<Situation> situs)
        {
            foreach (var situ in situs)
            {
                if (situ.name.SetEquals(name))
                {
                    return true;
                }
            }
            return false;
        }

        private Situation FillNewSituation(Situation situ, List<Situation> situs)
        {
            foreach (var s in situ.name)
            {
                foreach (var situation in situs)
                {
                    if (situation.name.Count == 1 && situation.name.First() == s)
                    {
                        situ.Combine(situation);
                    }
                }
            }

            return situ;
        }

        private class Situation
        {
            public SortedSet<string> name;
            public Dictionary<char, SortedSet<string>> combi;

            public Situation(SortedSet<string> name, SortedSet<char> alphabet)
            {
                this.name = new SortedSet<string>(name);
                combi = new Dictionary<char, SortedSet<string>>();
                foreach (var s in alphabet)
                {
                    combi.Add(s, new SortedSet<string>());
                }
            }

            public void Combine(Situation addSitu)
            {
                foreach (var pair in addSitu.combi)
                {
                    if (combi.ContainsKey(pair.Key))
                    {
                        combi[pair.Key].AddAll(pair.Value);
                    }
                    else
                    {
                        combi.Add(pair.Key, pair.Value);
                    }
                }
            }

            public string GetDisplayName()
            {
                return (name.Count > 0) ? string.Join("", name.ToArray()) : " ";
            }

            public override string ToString()
            {
                string str = "name: ";
                foreach (var s in name)
                {
                    str += s + ", ";
                }

                str += System.Environment.NewLine + "Combinations: ";

                foreach (var pair in combi)
                {
                    str += pair.Key + " --> ";
                    foreach (var s in pair.Value)
                    {
                        str += s + ", ";
                    }
                }
                return str;
            }
        }
    }
}