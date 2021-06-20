using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormeleMethodenCS.Converters;

namespace FormeleMethodenCS
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Kies een optie");
                Console.WriteLine("0: Exit");
                Console.WriteLine("1: DFA");
                Console.WriteLine("2: NDFA");
                Console.WriteLine("3: NDFA met Epsilon");
                Console.WriteLine("4: Regex");
                Console.WriteLine("5: DFA Operaties");
                Console.WriteLine("6: DFA Generator");
                Console.WriteLine("7: ThompsonConstruction");
                Console.WriteLine("8: NDFA naar DFA");
                Console.WriteLine("9: Minimal DFA");
                Console.WriteLine("10: Gelijkheid NDFA-DFA");
                Console.WriteLine("11: Gelijkheid Regex-NDFA");

                int choice = 0;
                Console.Write("Keuze: ");
                if (!Int32.TryParse(Console.ReadLine(), out choice)) continue;
                Console.WriteLine("");
                Console.WriteLine("--------------------------------");
                Console.WriteLine("");

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        TestDFA();
                        break;
                    case 2:
                        TestNDFA();
                        break;
                    case 3:
                        NDFAEpsilons();
                        break;
                    case 4:
                        TestRegex();
                        break;
                    case 5:
                        TestDFAOperations();
                        break;
                    case 6:
                        TestDFAGenerator();
                        break;
                    case 7:
                        TestThompsonConstruction();
                        break;
                    case 8:
                        TestNDFAToDFA();
                        break;
                    case 9:
                        TestToMinimalDFA();
                        break;
                    case 10:
                        TestEquivalenceNDFA_DFA();
                        break;
                    case 11:
                        TestEquivalenceRegNDFA();
                        break;
                    default:
                        break;
                }
            }

            //TestToMinimalDFA();
            //            TestRegex();
            //TestDFAOperations();
            //TestEquivalenceRegNDFA();
            //TestThompsonConstruction();
            TestRegex();
            Console.ReadLine();
            
            //var x = dfa.GetToStates("q4", 'a');
            //foreach (var i in x)
            //{
            //    Console.WriteLine(i);
            //}
            //Console.ReadLine();
        }

        //https://imgur.com/a/6KksxfH 
        static void NDFAEpsilons()
        {
            char[] alphabet = { 'a', 'b' };

            NDFA<string> ndfa = new NDFA<string>(alphabet);
            ndfa.AddTransition(new Transition<string>("S", 'a', "A"));
            ndfa.AddTransition(new Transition<string>("S", 'ε', "A"));

            ndfa.AddTransition(new Transition<string>("A", 'b', "S"));
            ndfa.AddTransition(new Transition<string>("A", 'b', "B"));

            ndfa.AddTransition(new Transition<string>("B", 'b', "F"));
            ndfa.AddTransition(new Transition<string>("B", 'a', "F"));
            ndfa.AddTransition(new Transition<string>("B", 'b', "S"));
            ndfa.AddTransition(new Transition<string>("B", 'ε', "S"));

            ndfa.AddTransition(new Transition<string>("F", 'a', "F"));
            ndfa.AddTransition(new Transition<string>("F", 'b', "A"));

            ndfa.DefineAsStartState("S");
            ndfa.DefineAsFinalState("F");


            do
            {
                Console.Clear();
                Console.Write("Geef string: ");
                string s = Console.ReadLine();
                bool success = ndfa.Accept(s);
                Console.WriteLine($"String geaccepteerd: {success}");
                Console.WriteLine("Druk op esc om te stoppen.");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
        //https://imgur.com/a/6KksxfH
       

        
        static void TestDFAOperations()
        {
            char[] alphabet = { 'a', 'b' };

            DFA<string> dfa = new DFA<string>(alphabet);
            dfa.AddTransition(new Transition<string>("1", 'a', "2"));
            dfa.AddTransition(new Transition<string>("1", 'b', "1"));

            dfa.AddTransition(new Transition<string>("2", 'a', "2"));
            dfa.AddTransition(new Transition<string>("2", 'b', "3"));

            dfa.AddTransition(new Transition<string>("3", 'a', "4"));
            dfa.AddTransition(new Transition<string>("3", 'b', "1"));

            dfa.AddTransition(new Transition<string>("4", 'a', "4"));
            dfa.AddTransition(new Transition<string>("4", 'b', "4"));

            dfa.DefineAsStartState("1");
            dfa.DefineAsFinalState("4");


            DFA<string> other = new DFA<string>(alphabet);
            other.AddTransition(new Transition<string>("1", 'a', "1"));
            other.AddTransition(new Transition<string>("1", 'b', "2"));

            other.AddTransition(new Transition<string>("2", 'a', "3"));
            other.AddTransition(new Transition<string>("2", 'b', "2"));

            other.AddTransition(new Transition<string>("3", 'a', "1"));
            other.AddTransition(new Transition<string>("3", 'b', "4"));

            other.AddTransition(new Transition<string>("4", 'a', "3"));
            other.AddTransition(new Transition<string>("4", 'b', "2"));

            other.DefineAsStartState("1");
            other.DefineAsFinalState("1");
            other.DefineAsFinalState("2");
            other.DefineAsFinalState("3");

            do
            {
                Console.Clear();
                Console.WriteLine("1: OR");
                Console.WriteLine("2: NOT");
                Console.WriteLine("3: AND");
                Console.Write("Geef string: ");
                string s = Console.ReadLine();

                switch (s)
                {
                    case "1":
                        {
                            var newDfa = dfa.Or(other);
                            Graphiz<string> graphiz = new Graphiz<string>(newDfa);
                            graphiz.PrintGraph();
                        }
                        break;
                    case "2":
                        {
                            var newDfa = dfa.Not();
                            Graphiz<string> graphiz = new Graphiz<string>(newDfa);
                            graphiz.PrintGraph();
                        }
                        break;
                    case "3":
                        {
                            var newDfa = dfa.And(other);
                            Graphiz<string> graphiz = new Graphiz<string>(newDfa);
                            graphiz.PrintGraph();
                        }
                        break;
                    default:
                        break;
                }
                
                Console.WriteLine("Druk op esc om te stoppen.");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
           
        }
        static void TestDFA()
        {
            char[] alphabet = { 'a', 'b' };

            Automata<string> dfa = new DFA<string>(alphabet);

            // e | (ab) (ab | aab)* (a | e)
            // Begint met niks of ab, vervolgd door 0 of x keer ab of aab, eindigend op a of epsilon (leeg)
            dfa.AddTransition(new Transition<string>("q0", 'a', "q1"));
            dfa.AddTransition(new Transition<string>("q0", 'b', "q4"));

            dfa.AddTransition(new Transition<string>("q1", 'a', "q4"));
            dfa.AddTransition(new Transition<string>("q1", 'b', "q2"));

            dfa.AddTransition(new Transition<string>("q2", 'a', "q3"));
            dfa.AddTransition(new Transition<string>("q2", 'b', "q4"));

            dfa.AddTransition(new Transition<string>("q3", 'a', "q1"));
            dfa.AddTransition(new Transition<string>("q3", 'b', "q2"));

            dfa.AddTransition(new Transition<string>("q4", 'a', "q4"));
            dfa.AddTransition(new Transition<string>("q4", 'b', "q4"));

            dfa.DefineAsStartState("q0");

            dfa.DefineAsFinalState("q0");
            dfa.DefineAsFinalState("q2");
            dfa.DefineAsFinalState("q3");

            Graphiz<string> graphiz = new Graphiz<string>(dfa);
            graphiz.PrintGraph();

            List<string> words = dfa.GetLanguage(6, true).ToList();//dfa.GetLanguage(2);
            Console.WriteLine($"Number of combinations: {words.Count()}");
            words.ForEach(w => Console.WriteLine(w));
            Console.ReadLine();
            do
            {
                Console.Clear();
                Console.Write("Geef string: ");
                string s = Console.ReadLine();
                bool success = dfa.Accept(s);
                Console.WriteLine($"String geaccepteerd: {success}");
                Console.WriteLine("Druk op esc om te stoppen.");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        static void TestNDFA()
        {
            char[] alphabet = { 'a', 'b' };

            Console.WriteLine("Bevat abb of bevat ba");
            // Bevat 'abb' of bevat 'ba'
            NDFA<string> ndfa = new NDFA<string>(alphabet);
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q0"));
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q1"));
            ndfa.AddTransition(new Transition<string>("q0", 'b', "q0"));
            ndfa.AddTransition(new Transition<string>("q0", 'b', "q4"));

            ndfa.AddTransition(new Transition<string>("q1", 'b', "q2"));

            ndfa.AddTransition(new Transition<string>("q2", 'b', "q3"));

            ndfa.AddTransition(new Transition<string>("q3", 'a', "q3"));
            ndfa.AddTransition(new Transition<string>("q3", 'b', "q3"));

            ndfa.AddTransition(new Transition<string>("q4", 'a', "q3"));

            ndfa.DefineAsStartState("q0");
            ndfa.DefineAsFinalState("q3");


            do
            {
                Console.Clear();
                Console.Write("Geef string: ");
                string s = Console.ReadLine();
                bool success = ndfa.Accept(s);
                Console.WriteLine($"String geaccepteerd: {success}");
                Console.WriteLine("Druk op esc om te stoppen.");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
        static void TestRegex()
        {
            RegExp a = new RegExp("a");
            RegExp b = new RegExp("b");

            // Expr1: "baa"
            RegExp expr1 = new RegExp("baa");
            // Expr2: "bb"
            RegExp expr2 = new RegExp("bb");
            // Expr3: "baa | bb"
            RegExp expr3 = expr1.Or(expr2);

            // All: " (a|b) *"
            RegExp all = (a.Or(b)).Star();
            // Expr4: " (baa | bb)+"
            RegExp expr4 = expr3.Plus();
            // Expr5: "(baa | bb)+ (a|b)*"
            RegExp expr5 = expr4.Dot(all);

            Console.WriteLine("Taal van (baa | bb):");
            expr3.GetLanguage(5).ToList().ForEach(s => Console.WriteLine(s));
            Console.WriteLine("");

            do
            {
                Console.Clear();
                Console.Write("Geef string: ");
                string s = Console.ReadLine();
                bool success = expr3.Accept(s);
                Console.WriteLine($"String geaccepteerd: {success}");
                Console.WriteLine("Druk op esc om te stoppen.");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        static void TestDFAGenerator()
        {
            string str = "bbabba";
            DFA<string> dfa = DFAGenerator.EndsWith(str);

            Graphiz<string> graphiz = new Graphiz<string>(dfa);
            graphiz.PrintGraph();

            Console.WriteLine("DFA Generated -> EndsWith: " + str);
        }

        static void TestThompsonConstruction()
        {
            //RegExp a = new RegExp("a");
            //RegExp b = new RegExp("b");

            //// Expr1: "baa"
            //RegExp expr1 = new RegExp("baa");
            //// Expr2: "bb"
            //RegExp expr2 = new RegExp("bb");
            //// Expr3: "baa | bb"
            //RegExp expr3 = expr1.Or(expr2);

            //// All: " (a|b) *"
            //RegExp all = (a.Or(b)).Star();
            //// Expr4: " (baa | bb)+"
            //RegExp expr4 = expr3.Plus();
            //// Expr5: "(baa | bb)+ (a|b)*"
            //RegExp expr5 = expr4.Dot(all);

            //NDFA<string> ndfa = AutomataConverter.RegexToNDFA(expr5);

            //Graphiz<string> graphiz = new Graphiz<string>(ndfa);
            //graphiz.PrintGraph();

            //Console.WriteLine("Regex to NDFA: " + expr5.ToString());

            RegExp expr1 = new RegExp("abb");
            RegExp expr2 = new RegExp("ba");

            // (abb | ba)
            RegExp regExpOR = expr1.Or(expr2);
            // (aab | ba)+
            RegExp regExpPLUS = regExpOR.Plus();

            RegExp b = new RegExp("b");
            RegExp a = new RegExp("a");
            RegExp aba = new RegExp("aba");

            RegExp expr = new RegExp("b");
            RegExp abStar = a.Or(b);
            abStar = abStar.Star();

            expr = expr.Dot(abStar).Dot(aba).Dot(abStar).Dot(b);
            var x = expr.GetLanguage(10);

            Console.WriteLine();


            NDFA<string> ndfa = AutomataConverter.RegexToNDFA(expr);
            Graphiz<string> graphiz = new Graphiz<string>(ndfa);
            graphiz.PrintGraph();
            bool s = ndfa.Accept("bababa");
            Console.WriteLine("Regex to NDFA: " + expr.ToString());
        }

        static void TestNDFAToDFA()
        {
            char[] alphabet = { 'a', 'b' };

            // Example from lesson 4:
            NDFA<string> ndfa = new NDFA<string>(alphabet);
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q0"));
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q1"));
            ndfa.AddTransition(new Transition<string>("q0", 'b', "q1"));

            ndfa.AddTransition(new Transition<string>("q1", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q1", 'b', "q2"));
            
            ndfa.AddTransition(new Transition<string>("q2", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q2", 'a', "q0"));
            
            ndfa.DefineAsStartState("q0");
            ndfa.DefineAsFinalState("q0");

            DFA<string> dfa = AutomataConverter.NDFAToDFA(ndfa);

            Graphiz<string> graphiz = new Graphiz<string>(dfa);
            graphiz.PrintGraph();

            Console.WriteLine("NDFA to DFA" );
        }

        static void TestToMinimalDFA()
        {
            char[] alphabet = { 'a', 'b' };

            NDFA<string> ndfa = new NDFA<string>(alphabet);
            ndfa.AddTransition(new Transition<string>("q1", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q1", 'b', "q2"));
            ndfa.AddTransition(new Transition<string>("q1", 'b', "q3"));

            ndfa.AddTransition(new Transition<string>("q2", 'a', "q3"));
            ndfa.AddTransition(new Transition<string>("q2", "q4"));
            ndfa.AddTransition(new Transition<string>("q2", 'b', "q4"));

            ndfa.AddTransition(new Transition<string>("q3", 'a', "q2"));

            ndfa.AddTransition(new Transition<string>("q4", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q4", 'a', "q5"));

            ndfa.AddTransition(new Transition<string>("q5", 'b', "q5"));
            ndfa.AddTransition(new Transition<string>("q5", "q3"));

            ndfa.DefineAsStartState("q1");
            ndfa.DefineAsFinalState("q2");
            ndfa.DefineAsFinalState("q3");

            DFA<string> dfa = AutomataConverter.NDFAToDFA(ndfa);

            Graphiz<string> graphiz = new Graphiz<string>(dfa);
            graphiz.PrintGraph("dfa1");

            DFA<string> dfa2 = AutomataConverter.ToMinimalDFA(dfa);

            Graphiz<string> graphiz2 = new Graphiz<string>(dfa2);
            graphiz.PrintGraph("dfa2");
        }

        // https://imgur.com/a/6KksxfH
        static void TestEquivalenceNDFA_DFA()
        {
            char[] alphabet = { 'a', 'b' };
            NDFA<string> ndfa = new NDFA<string>(alphabet);
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q0"));
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q1"));
            ndfa.AddTransition(new Transition<string>("q0", 'b', "q1"));

            ndfa.AddTransition(new Transition<string>("q1", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q1", 'b', "q2"));

            ndfa.AddTransition(new Transition<string>("q2", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q2", 'a', "q0"));

            ndfa.DefineAsStartState("q0");
            ndfa.DefineAsFinalState("q0");

            var x = ndfa.GetLanguage(5).ToList();


            DFA<string> dfa = new DFA<string>(alphabet);

            dfa.AddTransition(new Transition<string>("q0", 'a', "q0q1"));
            dfa.AddTransition(new Transition<string>("q0", 'b', "q1"));

            dfa.AddTransition(new Transition<string>("q1", 'a', "q2"));
            dfa.AddTransition(new Transition<string>("q1", 'b', "q2"));

            dfa.AddTransition(new Transition<string>("q0q1", 'a', "q0q1q2"));
            dfa.AddTransition(new Transition<string>("q0q1", 'b', "q1q2"));

            dfa.AddTransition(new Transition<string>("q2", 'a', "q0q2"));
            dfa.AddTransition(new Transition<string>("q2", 'b', "f"));

            dfa.AddTransition(new Transition<string>("q0q2", 'a', "q0q1q2"));
            dfa.AddTransition(new Transition<string>("q0q2", 'b', "q1"));

            dfa.AddTransition(new Transition<string>("f", 'a', "f"));
            dfa.AddTransition(new Transition<string>("f", 'b', "f"));

            dfa.AddTransition(new Transition<string>("q0q1q2", 'a', "q0q1q2"));
            dfa.AddTransition(new Transition<string>("q0q1q2", 'b', "q1q2"));

            dfa.AddTransition(new Transition<string>("q1q2", 'a', "q0q2"));
            dfa.AddTransition(new Transition<string>("q1q2", 'b', "q2"));

            dfa.DefineAsStartState("q0");
            dfa.DefineAsFinalState("q0");
            dfa.DefineAsFinalState("q0q1");
            dfa.DefineAsFinalState("q0q2");
            dfa.DefineAsFinalState("q0q1q2");

            var y = dfa.GetLanguage(5).ToList();
            bool equal = ndfa.Equivalent(dfa);
            Console.WriteLine($"Equals: {equal}");

        }
        static void TestEquivalenceRegNDFA()
        {
            char[] alphabet = { 'a', 'b' };

            // Bevat 'abb' of bevat 'ba'
            NDFA<string> ndfa = new NDFA<string>(alphabet);
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q0"));
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q1"));
            ndfa.AddTransition(new Transition<string>("q0", 'b', "q0"));
            ndfa.AddTransition(new Transition<string>("q0", 'b', "q4"));

            ndfa.AddTransition(new Transition<string>("q1", 'b', "q2"));

            ndfa.AddTransition(new Transition<string>("q2", 'b', "q3"));

            ndfa.AddTransition(new Transition<string>("q3", 'a', "q3"));
            ndfa.AddTransition(new Transition<string>("q3", 'b', "q3"));

            ndfa.AddTransition(new Transition<string>("q4", 'a', "q3"));

            ndfa.DefineAsStartState("q0");
            ndfa.DefineAsFinalState("q3");

            RegExp expr1 = new RegExp("abb");
            RegExp expr2 = new RegExp("ba");

            // (abb | ba)
            RegExp regExpOR = expr1.Or(expr2);
            // (abb | ba)+
            RegExp regExpPLUS = regExpOR.Plus();


            NDFA<string> regNDFA = AutomataConverter.RegexToNDFA(regExpPLUS);
            //bool match = regExpPLUS.EqualsNDFA(ndfa);//ndfa.EqualsRegex(regExpPLUS);
            // match = regExpPLUS.Accept("baaaa");
            Graphiz<string> graphiz = new Graphiz<string>(regNDFA);
            graphiz.PrintGraph();
            bool accReg = regExpPLUS.Accept("abbabab");
            bool accept = regNDFA.Accept("abbabab");
            bool accep2 = ndfa.Accept("abbabab");

            var x = regNDFA.GetLanguage(4, true).ToList(); ;
            var y = ndfa.GetLanguage(4, true).ToList();


            regNDFA.PrintTransitions();
            Console.WriteLine("");
            ndfa.PrintTransitions();
            Console.WriteLine($"Regex matches NDFA = {regNDFA.Equivalent(ndfa)}");
        }
    }
}


//dfa.AddTransition(new Transition<string>("q0", 'a', "q1"));
//            dfa.AddTransition(new Transition<string>("q0", 'b', "q3"));

//            dfa.AddTransition(new Transition<string>("q1", 'a', "q5"));
//            dfa.AddTransition(new Transition<string>("q1", 'b', "q2"));

//            dfa.AddTransition(new Transition<string>("q2", 'a', "q5"));
//            dfa.AddTransition(new Transition<string>("q2", 'b', "q6"));

//            dfa.AddTransition(new Transition<string>("q3", 'a', "q5"));
//            dfa.AddTransition(new Transition<string>("q3", 'b', "q4"));

//            dfa.AddTransition(new Transition<string>("q4", 'a', "q6"));
//            dfa.AddTransition(new Transition<string>("q4", 'b', "q5"));

//            dfa.AddTransition(new Transition<string>("q5", 'a', "q5"));
//            dfa.AddTransition(new Transition<string>("q5", 'b', "q5"));

//            dfa.AddTransition(new Transition<string>("q6", 'a', "q6"));
//            dfa.AddTransition(new Transition<string>("q6", 'b', "q6"));

//            dfa.DefineAsStartState("q0");
//            dfa.DefineAsFinalState("q6");