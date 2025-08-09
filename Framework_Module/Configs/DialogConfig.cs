using System;
using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Configs
{
    /// <summary>
    /// </summary>

    [CreateAssetMenu(fileName = "DialogConfig", menuName = "Scriptable Objects/DialogConfig")]
    public class DialogConfig : ScriptableObject
    {
        [SerializeField] private DialogSequenceDefinition[] dialogDefinitions;
        public IReadOnlyCollection<DialogSequenceDefinition> DialogDefinitions => dialogDefinitions;

        public void OnValidate()
        {

        }
    }
}
