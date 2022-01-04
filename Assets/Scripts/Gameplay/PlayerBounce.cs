using Platformer.Core;
using Platformer.Mechanics;
using static Platformer.Core.Simulation;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerLanded"></typeparam>
    public class PlayerBounce : Simulation.Event<PlayerBounce>
    {
        public PlayerController player;

        public override void Execute() {
        }
    }
}