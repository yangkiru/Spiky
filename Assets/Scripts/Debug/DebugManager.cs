using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;

public class DebugManager : MonoBehaviour {
    public Transform teleport;
    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) GameController.Instance.model.player.transform.position = teleport.position;
    }
}
