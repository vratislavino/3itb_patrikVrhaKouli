using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollision : MonoBehaviour
{
    public event Action<Vector3> BallTouchedGround;

    bool touched = false;

    private void OnCollisionEnter(Collision collision) {
        if (touched) return;
        touched = true;

        BallTouchedGround?.Invoke(collision.contacts[0].point);
    }

    public void ResetThrow() {
        touched = false;
    }
}
