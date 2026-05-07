using System.Collections.Generic;
using UnityEngine;

namespace World_Module
{
    public class SpriteRendererBoundsAggregator : MonoBehaviour
    {
        private SpriteRenderer[] renderers;
        [SerializeField] private SpriteRenderer[] ignoredRenderers;
        
        private void Awake()
        {
            var filtered = new List<SpriteRenderer>();
            HashSet<SpriteRenderer> ignoredSet = new HashSet<SpriteRenderer>(ignoredRenderers ?? System.Array.Empty<SpriteRenderer>());
            
            foreach (var sr in GetComponentsInChildren<SpriteRenderer>(true))
            {
                if(!ignoredSet.Contains(sr))
                    filtered.Add(sr);
            }

            renderers = filtered.ToArray();
        }

        public Bounds GetBounds()
        {
            Bounds combined = new Bounds(transform.position, Vector3.zero);
            bool initialized = false;

            for (int i = 0; i < renderers.Length; i++)
            {
                var r = renderers[i];
                if (r == null) continue;

                if (!initialized)
                {
                    combined = r.bounds; 
                    initialized = true;
                }
                else
                {
                    combined.Encapsulate(r.bounds);
                }
            }
            
            return combined;  
        }
    }
}
