using System;
using System.Collections.Generic;

namespace Phac.State
{
    public class StateMachine
    {
        private class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }
            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition) => Transitions.Add(new Transition(to, condition));
        }
        private StateNode m_Current;
        private Dictionary<Type, StateNode> m_Nodes = new Dictionary<Type, StateNode>();
        private HashSet<ITransition> m_AnyTransitions = new HashSet<ITransition>();

        public void Update()
        {
            ITransition transition = GetTransition();
            if (transition != null)
            {
                ChangeState(transition.To);
            }
            m_Current?.State.Update();
        }

        public void FixedUpdate() => m_Current?.State.FixedUpdate();

        public void SetState(IState state)
        {
            m_Current = m_Nodes[state.GetType()];
            m_Current.State?.OnEnter();
        }

        private void ChangeState(IState state)
        {
            if (state == m_Current.State) return;

            IState previousState = m_Current.State;
            IState nextState = m_Nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();
            m_Current = m_Nodes[state.GetType()];
        }

        private ITransition GetTransition()
        {
            foreach (ITransition transition in m_AnyTransitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }

            foreach (ITransition transition in m_Current.Transitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }

            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition) => GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);

        public void AddAnyTransition(IState to, IPredicate condition) => m_AnyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));

        private StateNode GetOrAddNode(IState state)
        {
            StateNode node = m_Nodes.GetValueOrDefault(state.GetType());

            if (node == null)
            {
                node = new StateNode(state);
                m_Nodes.Add(state.GetType(), node);
            }

            return node;
        }
    }

    public static class MyExtensions
    {
        public static void At(this StateMachine sm, IState from, IState to, IPredicate condition) => sm.AddTransition(from, to, condition);
        public static void Any(this StateMachine sm, IState to, IPredicate condition) => sm.AddAnyTransition(to, condition);
    }
}