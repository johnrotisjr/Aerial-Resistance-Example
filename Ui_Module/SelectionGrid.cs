using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Service;
using UnityEngine;
using UnityEngine.UI;

namespace Ui_Module
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class SelectionGrid : MonoBehaviour
    {
        [SerializeField] private GameObject gameButtonPrefab;
        [SerializeField] private int rowChildLimit = 2;
        private VerticalLayoutGroup verticalLayoutGroup;
        private HorizontalLayoutGroup[] horizontalLayoutGroups;
        private GameObjectPooler gameObjectPooler;
        private int currentRow = 0;

        private void Awake()
        {
            gameObjectPooler = Services.Get<GameObjectPooler>();
            if (!gameObjectPooler.IsRegistered(PrefabKey.GameButton.ToString()))
            {
                gameObjectPooler.Register(PrefabKey.GameButton.ToString(), gameButtonPrefab, 10, 100);
            }

            verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
            horizontalLayoutGroups = GetComponentsInChildren<HorizontalLayoutGroup>();
            if(horizontalLayoutGroups.Length <= 0)
                DebugLogger.Log("Setup error: Selection grid must have at least 1 horizontalLayoutGroup.", LogCategory.Ui, LogLevel.Error);
        }

        private void OnDestroy()
        {
            //gameObjectPooler.UnRegister(gameButtonPrefab);
        }
 
        public GameButton[] AddElements(GameButtonSettings[] data)
        {
            var list = new List<GameButton>();
            foreach (var d in data)
            {
                var gb = gameObjectPooler.Get<GameButton>(PrefabKey.GameButton.ToString());
                gb.transform.SetParent(GetHorizontalLayoutGroup().transform);
                gb.Set(d);
                list.Add(gb);
            }

            return list.ToArray();
        }

        private HorizontalLayoutGroup GetHorizontalLayoutGroup()
        {
            if (horizontalLayoutGroups[currentRow].transform.childCount > rowChildLimit)
                currentRow++;

            if (currentRow > horizontalLayoutGroups.Length)
                currentRow = 0;
            
            return horizontalLayoutGroups[currentRow];
        }

        public void RemoveElements(GameButton[] gameButtons)
        {
            foreach (var gb in gameButtons)
            {
                if (gb)
                {
                    gb.Clear();
                    gameObjectPooler.Release(PrefabKey.GameButton.ToString(), gb.gameObject);
                }

            }
        }
    }
}
