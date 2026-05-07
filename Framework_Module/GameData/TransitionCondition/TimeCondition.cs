using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.GameData.TransitionCondition
{
    public sealed class TimeCondition : AiTransitionCondition
    {
        private TimeConditionDefinition def;
        private float expiration = 0f;
        public TimeCondition(TimeConditionDefinition def)
        {
            this.def = def;
            var varianceRange = def.Seconds * def.VariancePercent;
            var randomizedDuration = Random.Range(def.Seconds - varianceRange, def.Seconds + varianceRange);
            expiration = Time.time + randomizedDuration;
        }
        
        public override AiTransitionType TransitionType => AiTransitionType.Time;
        public override bool ConditionMet
        {
            get
            {
                bool expirationMet = Time.time > expiration;
                if (def.FlipLogic)
                    return !expirationMet;
                return expirationMet;
            }
        }
    }
}