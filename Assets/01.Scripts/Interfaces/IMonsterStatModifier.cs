namespace _01.Scripts.Interfaces
{
    public interface IMonsterStatModifier
    {
        float CalculateHealth(float baseHealth, int stage, bool isBoss);
        int CalculateGold(int baseGold, int stage, bool isBoss);
    }
}