using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace World_Module
{
    
    public class BattleFieldManager : GameServiceBase, IBattleFieldManager
    {
        [SerializeField] private Transform spawnTransform;
        private IWorldStateManager worldStateManager;
        private IViewportBoundsProvider viewportBoundsProvider;
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            var db = viewportBoundsProvider.GetDeathBoundaryRect();
            Gizmos.DrawWireCube(db.center, db.size);
            
            Gizmos.color = Color.green;
            var bf = viewportBoundsProvider.GetBattleFieldRect();
            Gizmos.DrawWireCube(bf.center, bf.size);
            
            Gizmos.color = Color.magenta;
            var wv = viewportBoundsProvider.WorldViewBounds;
            Gizmos.DrawWireCube(wv.center, wv.size);
        }
        
        public void Inject(IViewportBoundsProvider viewportBoundsProviderService, IWorldStateManager worldStateManagerService)
        {
            viewportBoundsProvider = viewportBoundsProviderService;
            worldStateManager = worldStateManagerService;
        }
        
        public override void Initialize()
        {
            
        }

        public override void Shutdown()
        {

        }

        private void Update()
        {
            worldStateManager.Update();
        }

        private void FixedUpdate()
        {
            worldStateManager.FixedUpdate();
        }
    }
}
