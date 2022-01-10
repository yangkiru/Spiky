using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            Debug.Log("PlayerSpawn");
            var player = model.player;
            player.move *= 0;
            player.body.velocity *= 0;
            player.body.angularVelocity = 0;
            player.body.rotation = 0;
            player.collider2d.enabled = true;
            player.body.sharedMaterial = null;
            player.body.constraints = RigidbodyConstraints2D.FreezeRotation;
            player.controlEnabled = false;
            if (player.audioSource && player.respawnAudio)
                player.audioSource.PlayOneShot(player.respawnAudio);
            player.health.Increment();
            player.Teleport(model.spawnPoint.transform.position);
            player.jumpState = PlayerController.JumpState.JumpReady;
            player.animator.SetBool("dead", false);
            model.virtualCamera.m_Follow = player.transform;
            model.virtualCamera.m_LookAt = player.transform;
            Simulation.Schedule<EnemyRespawn>();
            Simulation.Schedule<EnablePlayerInput>(2f);
            Simulation.Schedule<ResetPlayerConstraints>(2f);
        }
    }
}