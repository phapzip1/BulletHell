namespace Phac.State
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Update();
        void FixedUpdate();
    }

    public interface IPredicate
    {
        bool Evaluate();
    }
}