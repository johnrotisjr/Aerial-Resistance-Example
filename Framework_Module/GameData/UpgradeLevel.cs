using Framework_Module.Enums;

namespace Framework_Module
{
    public class UpgradeLevel
    {
        public readonly UpgradeType Type;
        public int Level;

        public UpgradeLevel(UpgradeType type, int level)
        {
            Type = type;
            Level = level;
        }
    }
}