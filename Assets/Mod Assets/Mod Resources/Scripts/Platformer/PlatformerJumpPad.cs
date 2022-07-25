using UnityEngine;
using Platformer.Mechanics;
using System.Collections;

public class PlatformerJumpPad : MonoBehaviour{
    public float power;
    public Collider2D coll;

    public bool isVelocityConvertToPower;
    WaitForSeconds wait = new WaitForSeconds(3);
    Coroutine coroutine;
    SpriteRenderer renderer;
    Transform tf;

    void Awake(){
        renderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        tf = transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        if (rb == null) return;
        var player = rb.GetComponent<PlayerController>();
        if (player == null) return;
        AddVelocity(player);
        //coll.enabled = false;
        //coroutine = StartCoroutine(ActivatePadCoroutine());
        //renderer.enabled = false;
    }

    void AddVelocity(PlayerController player) {
        player.DisableControl();
        Vector2 velocity = player.body.velocity;
        if(!isVelocityConvertToPower)
            velocity = (Vector2)(tf.up * power);
        else {
            float playerPower = Vector2.SqrMagnitude(player.body.velocity);
            velocity = playerPower * 0.05f * (Vector2)(tf.up * power);
        }
        player.body.velocity = velocity;

    }

    IEnumerator ActivatePadCoroutine() {
        yield return wait;
        coll.enabled = true;
        renderer.enabled = true;
    }
}
