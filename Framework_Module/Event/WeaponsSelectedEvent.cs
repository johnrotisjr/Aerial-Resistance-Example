using System.Collections.Generic;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Interfaces;

namespace Framework_Module.Event
{
    public class WeaponsSelectedEvent : IGameEvent
    {
        public WeaponDefinition[] Data;
        
        public WeaponsSelectedEvent(WeaponDefinition[] data)
        {
            Data = data;
        }
    }
}