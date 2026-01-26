namespace _01.Scripts.Interfaces
{
    public interface IClickable
    {
        public bool IsClickable { get; }
        void OnClick();
    }
}