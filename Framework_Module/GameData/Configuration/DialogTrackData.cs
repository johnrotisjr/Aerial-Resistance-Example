using Framework_Module.Definitions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.GameData.Configuration
{
    [CreateAssetMenu(fileName = "DialogTrackData", menuName = "Scriptable Objects/DialogTrackData")]
    public class DialogTrackData: ScriptableObject
    {
        [FormerlySerializedAs("dialogSequenceDefinition")] [SerializeField] private DialogTrackDefinition dialogTrackDefinition;
        public DialogTrackDefinition DialogTrackDefinition => dialogTrackDefinition;

        public void OnValidate()
        {

        }
    }

}
