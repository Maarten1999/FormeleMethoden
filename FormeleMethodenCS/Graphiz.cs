using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    class Graphiz
    {
        private readonly string Title;
        private readonly List<Node> nodes;
        private readonly List<Edge> edges;

        public Graphiz(NDFA<string> ndfa)
        {
            Title = "ndfa";
            nodes = new List<Node>();
            edges = new List<Edge>();
            LoadNDFA(ndfa);
        }

        public Graphiz(DFA<string> dfa)
        {
            Title = "dfa";
            nodes = new List<Node>();
            edges = new List<Edge>();
            LoadDFA(dfa);
        }


        // Print Graph to a .gv file
        // Run: 'dot -Tpng < dfa.gv > dfa.png' in CMD to generate PNG file
        public bool PrintGraph()
        {
            List<string> lines = new List<string>(
                new string[] {
                    "digraph " + Title + " {",
                    "rankdir = LR;"
            });

            foreach (var node in nodes)
            {
                lines.Add(node.Parse());
            }

            foreach (var edge in edges)
            {
                lines.Add(edge.Parse());
            }

            lines.Add("}");

            Directory.CreateDirectory("graphiz");
            File.WriteAllLines(Directory.GetCurrentDirectory() + "\\graphiz\\" + Title +".gv", lines.ToArray());
            return true;
        }

        private void LoadNDFA(NDFA<string> ndfa)
        {
            
            // Convert states
            foreach (var s in ndfa.states)
            {
                nodes.Add(new Node(Node.Shape.circle, s));
            }

            // Convert start states
            foreach (var ss in ndfa.startStates)
            {
                var start_node = new Node(Node.Shape.none, ss + "_start", true);
                var node = new Node(Node.Shape.circle, ss);
                nodes.Add(start_node);
                nodes.Add(node);
                edges.Add(new Edge(start_node.Label, node.Label));
            }

            // Convert final states
            foreach (var fs in ndfa.finalStates)
            {
                var node = new Node(Node.Shape.doublecircle, fs);
                if (nodes.Contains(node))
                {
                    nodes.Remove(node);
                }
                nodes.Add(node);
            }

            // Convert transitions 
            foreach (var t in ndfa.transitions)
            {
                edges.Add(new Edge(t.SourceState, t.DestState, t.Symbol));
            }
        }

        private void LoadDFA(DFA<string> dfa)
        {
            // Convert states
            foreach (var s in dfa.states)
            {
                nodes.Add(new Node(Node.Shape.circle, s));
            }

            // Convert start state
            var pre_startnode = new Node(Node.Shape.none, dfa.startState + "_start", true);
            var startnode = new Node(Node.Shape.circle, dfa.startState);
            nodes.Add(pre_startnode);
            nodes.Add(startnode);
            edges.Add(new Edge(pre_startnode.Label, startnode.Label));
            
            // Convert final states
            foreach (var fs in dfa.finalStates)
            {
                var node = new Node(Node.Shape.doublecircle, fs);
                if (nodes.Contains(node))
                {
                    nodes.Remove(node);
                }
                nodes.Add(node);
            }

            // Convert transitions
            foreach (var t in dfa.transitions)
            {
                edges.Add(new Edge(t.SourceState, t.DestState, t.Symbol));
            }
        }

        class Node : IComparer<Node>
        {
            public enum Shape { none, circle, doublecircle, plaintext }

            public Shape @shape;
            public string Label;
            public bool Hide;

            public Node(Shape shape, string label, bool hide = false)
            {
                this.shape = shape;
                this.Label = label;
                Hide = hide;
            }

            public string Parse()
            {
                var dot_label = (Hide) ? "\"\", width=0.01" : "\"" + Label + "\"";
                return "node_" + Label + " [shape = " + @shape.ToString() + ", label = " + dot_label + "];";
            }

            // Compare only based on unique label
            public int Compare(Node x, Node y)
            {
                return x.Label.CompareTo(y.Label);
            }
        }

        class Edge
        {
            public string First;
            public string Second;
            public char Label;
            public int Weight;
            public bool HideLabel;

            public Edge(string first, string second, char label = ' ', int weight = 1, bool hideLabel = false)
            {
                First = first;
                Second = second;
                Label = label;
                Weight = weight;
                HideLabel = hideLabel;
            }

            public string Parse()
            {
                var dot_label = (HideLabel) ? ' ' : Label;
                return "node_" + First + " -> node_" + Second +
                       " [label = \"" + Label + " \", weight =" + Weight + "];";
            }
        }
    }
}
