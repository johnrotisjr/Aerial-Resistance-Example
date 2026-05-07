using System.Collections.Generic;
using Framework_Module.GameData.Data;
using UnityEngine;

namespace Framework_Module.GameData.Databases
{
    /// <summary>
    /// </summary>

    [CreateAssetMenu(fileName = "DialogCueDatabase", menuName = "Scriptable Objects/DialogCueDatabase")]
    public class DialogCueDatabase : ScriptableObject
    {
        [SerializeField] private DialogCue[] database;
        public IReadOnlyCollection<DialogCue> Database => database;

        public void OnValidate()
        {

        }
    }

}
