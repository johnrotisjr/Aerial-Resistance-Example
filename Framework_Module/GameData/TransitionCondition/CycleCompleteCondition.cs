using Framework_Module.Definitions;
using Framework_Module.Enums;

namespace Framework_Module.GameData.TransitionCondition
{
 
    public sealed class CycleCompleteCondition : AiTransitionCondition
    {
        private CycleCompleteConditionDefinition def;
        private int cyclesCompleted = 0;
        public override AiTransitionType TransitionType => AiTransitionType.CycleComplete;
        public void IncreaseCycles() => cyclesCompleted++;
        public void SetCycles(int count) => cyclesCompleted = count;
        public int CycleCount => cyclesCompleted;
        
        public CycleCompleteCondition(CycleCompleteConditionDefinition def)
        {
            this.def = def;
        }

        public override bool ConditionMet
        {
            get
            {
                bool completedCycles = cyclesCompleted >= def.Cycles;
                if (def.FlipLogic)
                    return !completedCycles;

                return completedCycles;
            }
        }
 
    }
}