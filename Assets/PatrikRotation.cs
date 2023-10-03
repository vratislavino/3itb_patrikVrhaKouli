using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PatrikRotation : MonoBehaviour
{
    [SerializeField]
    float maxRotationSpeed = 600;

    [SerializeField]
    Transform arrow;

    [SerializeField]
    Rigidbody ball;
    Vector3 ballOriginPosition;

    [SerializeField]
    float maxPower = 1;

    float currentScale = 0;
    float scaleStep = 1f;
    bool isGrowing = true;

    float currentRotationSpeed = 0;

    ThrowingPhase currentPhase = ThrowingPhase.Rotating;

    // Start is called before the first frame update
    void Start()
    {
        ballOriginPosition = ball.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPhase == ThrowingPhase.Rotating) {
            if (currentRotationSpeed < maxRotationSpeed) {
                currentRotationSpeed += 1f;
            }
            transform.Rotate(Vector3.up * Time.deltaTime * currentRotationSpeed);
        } else if (currentPhase == ThrowingPhase.SettingPower) {
            if (isGrowing) {
                currentScale += scaleStep * Time.deltaTime;
                if (currentScale > maxPower)
                    isGrowing = false;
            }
            else
            {
                currentScale -= scaleStep * Time.deltaTime;
                if (currentScale < 0)
                    isGrowing = true;
            }
            arrow.localScale = Vector3.one * currentScale;
        }
        PlayerInput();
    }

    private void Throw() {
        ball.isKinematic = false;
        var angle = transform.forward;
        angle.y += 1f;
        Debug.Log(angle);
        ball.AddForce(angle * 500 * currentScale);
        //Debug.Break();
    }

    private void PlayerInput() {
        if(Input.GetKeyDown(KeyCode.Space)) { 
            if(currentPhase == ThrowingPhase.Rotating) {
                currentPhase = ThrowingPhase.SettingPower;
            } else if(currentPhase == ThrowingPhase.SettingPower) {
                currentPhase = ThrowingPhase.Thrown;
                Throw();
            }
        }
    }

    public void ResetThrow() {
        transform.rotation = Quaternion.identity;
        ball.isKinematic = true;
        ball.transform.localPosition = ballOriginPosition;

        currentPhase = ThrowingPhase.Rotating;
        currentRotationSpeed = 0;
        currentScale = 0;
        isGrowing = true;
        arrow.localScale = Vector3.zero;
    }
}

enum ThrowingPhase
{
    Rotating,
    SettingPower,
    Thrown
}
