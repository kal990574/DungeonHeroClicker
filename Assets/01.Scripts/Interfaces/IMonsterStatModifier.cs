using _01.Scripts.Core.Utils;

namespace _01.Scripts.Interfaces
{
    public interface IMonsterStatModifier
    {
        float BaseHealth { get; }
        long BaseGold { get; }
        float CalculateHealth(float baseHealth, int stage, bool isBoss);
        BigNumber CalculateGold(long baseGold, int stage, bool isBoss);
    }
}