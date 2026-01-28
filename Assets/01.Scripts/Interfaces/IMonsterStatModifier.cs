namespace _01.Scripts.Interfaces
{
    public interface IMonsterStatModifier
    {
        float BaseHealth { get; }
        int BaseGold { get; }
        float CalculateHealth(float baseHealth, int stage, bool isBoss);
        int CalculateGold(int baseGold, int stage, bool isBoss);
    }
}