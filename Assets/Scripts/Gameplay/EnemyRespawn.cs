using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the health component on an enemy has a hitpoint value of  0.
    /// </summary>
    /// <typeparam name="EnemyRespawn"></typeparam>
    public class EnemyRespawn : Simulation.Event<EnemyRespawn>
    {
        public override void Execute() {
            EnemyController enemy;
            for (int i = 0; i < GameController.Instance.model.deadEnemies.Count; i++) {
                enemy = GameController.Instance.model.deadEnemies[i];
                
                Health health = enemy.GetComponent<Health>();
                if (health != null) {
                    health.SetFull();
                }
                enemy._collider.enabled = true;
                enemy.control.enabled = true;
                enemy.control.animator.SetBool("death", false);
            }
            GameController.Instance.model.deadEnemies.Clear();
        }
    }
}