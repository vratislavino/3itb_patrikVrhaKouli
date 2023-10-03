using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    PatrikRotation patrikRotation;

    [SerializeField]
    FloorCollision floorCollision;

    [SerializeField]
    TMPro.TMP_Text text;

    int throwIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        floorCollision.BallTouchedGround += OnBallTouchedGround;
    }

    private void OnBallTouchedGround(Vector3 touchPos) {
        var attData = new AttemptData();
        attData.index = ++throwIndex;
        var angle = Quaternion.Angle(
            patrikRotation.transform.rotation, // rotace hráèe
            Quaternion.Euler(Vector3.forward) // rotace "vpøed"
            );

        Debug.Log(angle);
        if (angle < 30) {
            attData.valid = true;
            attData.distance = Vector3.Distance(patrikRotation.transform.position, touchPos);
        } else {
            attData.valid = false;
        }

        text.text += attData.ToString() + "\n";

        StartCoroutine(ResetAttempt());
    }

    IEnumerator ResetAttempt() {
        yield return new WaitForSeconds(2);

        patrikRotation.ResetThrow();
        floorCollision.ResetThrow();
    }
}

public class AttemptData
{
    public int index;
    public bool valid;
    public float distance;

    public override string ToString() {
        if(valid) {
            return $"Attempt {index}: {distance} m";
        } 
        return $"Attempt {index} failed";
    }
}
