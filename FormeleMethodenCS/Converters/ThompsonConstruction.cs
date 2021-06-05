using System.Collections.Generic;
using System.Linq;

namespace FormeleMethodenCS.Converters
{
    class ThompsonConstruction
    {
        public NDFA<string> ndfa;
        private int state;

        public ThompsonConstruction(char[] alphabet)
        {
            this.ndfa = new NDFA<string>(alphabet);
        }

        public NDFA<string> Convert(RegExp regex)
        {
            // Set params
            state = 0;
            BlackBox firstBox = new BlackBox(state, ++state, regex);

            // Define NDFA states
            ndfa.DefineAsStartState("q" + firstBox.startState);
            ndfa.DefineAsFinalState("q" + firstBox.endState);

            // Generate NDFA Transitions and Stats
            handleBlackbox(firstBox);

            return ndfa;
        }

        private void handleBlackbox(BlackBox bbox)
        {
            List<BlackBox> boxs = new List<BlackBox>(2);
            switch (bbox.content.@operator)
            {
                case RegExp.Operator.PLUS:
                {
                    int left = ++state;
                    int right = ++state;
                    ndfa.AddTransition(new Transition<string>("q" + bbox.startState, "q" + left));
                    ndfa.AddTransition(new Transition<string>("q" + right, "q" + bbox.endState));
                    ndfa.AddTransition(new Transition<string>("q" + right, "q" + left));

                    boxs.Add(new BlackBox(left, right, bbox.content.Left));
                }
                break;

                case RegExp.Operator.STAR:
                {
                    int left = ++state;
                    int right = ++state;
                    ndfa.AddTransition(new Transition<string>("q" + bbox.startState, "q" + left));
                    ndfa.AddTransition(new Transition<string>("q" + right, "q" + bbox.endState));
                    ndfa.AddTransition(new Transition<string>("q" + right, "q" + left));
                    ndfa.AddTransition(new Transition<string>("q" + bbox.startState, "q" + bbox.endState));

                    boxs.Add(new BlackBox(left, right, bbox.content.Left));
                }
                break;

                case RegExp.Operator.DOT:
                {
                    int left = ++state;
                    int right = ++state;
                    ndfa.AddTransition(new Transition<string>("q" + left, "q" + right));

                    boxs.Add(new BlackBox(bbox.startState, left, bbox.content.Left));
                    boxs.Add(new BlackBox(right, bbox.endState, bbox.content.Right));
                }
                break;

                case RegExp.Operator.OR:
                {
                    int left1 = ++state;
                    int right1 = ++state;
                    int left2 = ++state;
                    int right2 = ++state;
                    ndfa.AddTransition(new Transition<string>("q" + bbox.startState, "q" + left1));
                    ndfa.AddTransition(new Transition<string>("q" + bbox.startState, "q" + left2));
                    ndfa.AddTransition(new Transition<string>("q" + right1, "q" + bbox.endState));
                    ndfa.AddTransition(new Transition<string>("q" + right2, "q" + bbox.endState));

                    boxs.Add(new BlackBox(left1, right1, bbox.content.Left));
                    boxs.Add(new BlackBox(left2, right2, bbox.content.Right));
                }
                break;

                default:
                {
                    // Get terminals
                    string str = bbox.content.ToString();

                    // First setup all states: e.g. bbab has 5 states
                    int[] states = new int[str.Length + 1];
                    states[0] = bbox.startState;
                    states[states.Length - 1] = bbox.endState;
                    for (int i = 1; i < states.Length - 1; i++)
                    {
                        states[i] = ++state;
                    }

                    // Add transition for each char: e.g. b + b + a + b
                    for (int i = 1; i < states.Length; i++)
                    {
                        ndfa.AddTransition(new Transition<string>("q" + states[i - 1], str[i - 1], "q" + states[i]));
                    }
                }
                break;
            }

            foreach (var blackBox in boxs)
            {
                handleBlackbox(blackBox);
            }

        }

        private class BlackBox
        {
            public int startState;
            public int endState;
            public RegExp content;

            public BlackBox(int startState, int endState, RegExp content)
            {
                this.startState = startState;
                this.endState = endState;
                this.content = content;
            }
        }
    }
}