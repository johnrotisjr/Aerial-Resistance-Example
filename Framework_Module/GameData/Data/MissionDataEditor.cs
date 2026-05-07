#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Framework_Module.GameData.Data
{
    [CustomEditor(typeof(MissionData))]
    public class MissionDataEditor : Editor
    {
        private float percent = 100f;

        public override void OnInspectorGUI()
        {
            // Draw the normal inspector first
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Spawn Distance Tools", EditorStyles.boldLabel);

            percent = EditorGUILayout.FloatField("Scale Percent", percent);

            float factor = percent / 100f;
            EditorGUILayout.LabelField("Scale Factor", factor.ToString("0.###"));

            using (new EditorGUI.DisabledScope(factor <= 0f))
            {
                if (GUILayout.Button("Apply to Vehicle Spawn Distances"))
                {
                    ScaleVehicleSpawnDistances(factor);
                }
            }
        }

        private void ScaleVehicleSpawnDistances(float factor)
        {
            if (factor <= 0f)
            {
                return;
            }

            SerializedObject so = serializedObject;
            so.Update();

            // MissionData has a serialized field "definition"
            SerializedProperty definitionProp = so.FindProperty("definition");
            if (definitionProp == null)
            {
                return;
            }

            // Scale main SpawnVehicleInstructions array
            SerializedProperty vehiclesProp = definitionProp.FindPropertyRelative("spawnVehicleInstructions");
            if (vehiclesProp != null && vehiclesProp.isArray)
            {
                for (int i = 0; i < vehiclesProp.arraySize; i++)
                {
                    SerializedProperty element = vehiclesProp.GetArrayElementAtIndex(i);
                    SerializedProperty distanceProp = element.FindPropertyRelative("spawnDistance");
                    if (distanceProp != null)
                    {
                        distanceProp.floatValue *= factor;
                    }
                }
            }

            // Scale bossSpawnVehicleInstruction
            SerializedProperty bossProp = definitionProp.FindPropertyRelative("bossSpawnVehicleInstruction");
            if (bossProp != null)
            {
                SerializedProperty bossDistanceProp = bossProp.FindPropertyRelative("spawnDistance");
                if (bossDistanceProp != null)
                {
                    bossDistanceProp.floatValue *= factor;
                }
            }

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif
