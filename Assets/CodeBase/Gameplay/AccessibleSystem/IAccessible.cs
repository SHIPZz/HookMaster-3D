namespace CodeBase.Gameplay.AccessibleSystem
{
    public interface IAccessible
    {
        bool IsAccessed { get; }
        void UnLock();
        void Block();
    }
}