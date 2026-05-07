using System.Collections.Generic;
using Framework_Module.Definitions;
using UnityEngine;

namespace Framework_Module.GameData.Data
{
 
    [CreateAssetMenu(fileName = "DialogCue", menuName = "Scriptable Objects/DialogCue")]
    public class DialogCue : ScriptableObject
    {
        [SerializeField] private int missionIndex;
        public int MissionIndex => missionIndex;
        
        [SerializeField] private DialogCueDefinition[] dialogCues;
        public IReadOnlyCollection<DialogCueDefinition> DialogCues => dialogCues;

        public void OnValidate()
        {

        }
    }

}
