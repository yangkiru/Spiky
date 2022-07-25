using UnityEngine;
using Platformer.Mechanics;
using System.Collections;

public class PlatformerIce : MonoBehaviour
{
    public float slip = 0.5f;
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Slip In");
        var rb = other.rigidbody;
        if (rb == null) return;
        var player = rb.GetComponent<PlayerController>();
        if (player == null) return;
        player.slip = slip;
    }

    private void OnCollisionStay2D(Collision2D other) {
        Debug.Log("Slip Stay");
        var rb = other.rigidbody;
        if (rb == null) return;
        var player = rb.GetComponent<PlayerController>();
        if (player == null) return;
        player.slip = slip;
    }
}
