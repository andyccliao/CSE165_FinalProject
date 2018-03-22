using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BButton : MonoBehaviour {

    //[Tooltip("Attach a Canvas (UI) to BButton if text desired. Note: Must create and align text manually.")]
    //public Text text;

    [Tooltip("Physical Button. Needs Collider.")]
    public GameObject button;
    [Tooltip("Radius of \"Near\" glow.")]
    public float nearRadius = 0.1f;

    private Renderer buttonRenderer;
    private Material originalMaterial;

    public event Action SingleFireButtonAction;
    public event Action ContinuousFireButtonAction;
    private bool canCallAction = true;

    public Color glowColor = Color.grey;
    public Vector3 buttonPushDirection = Vector3.zero;
    public float buttonPushDistance = 0;

    private Vector3 startPoint = Vector3.positiveInfinity;
    private Vector3 originalPoint;

    // Use this for initialization
    void Start () {
        if (button == null) button = gameObject;
        if (buttonPushDirection == Vector3.zero) buttonPushDirection = button.transform.forward;
        else buttonPushDirection.Normalize();
        buttonRenderer = button.GetComponent<Renderer>();
        originalMaterial = buttonRenderer.material;
	}

    private void FixedUpdate()
    {
    }

    // Update is called once per frame
    void Update () {
        var colliders = Physics.OverlapSphere(button.transform.position, nearRadius, LayerMask.GetMask("Presser"), QueryTriggerInteraction.Collide);

        if (colliders.Length == 0) {
            buttonRenderer.material.color = originalMaterial.GetColor("_EmissionColor");
        }

        Collider closestCollider;
        float distance = nearRadius + 1;
        foreach (var item in colliders) {
            float dist = Vector3.Distance(item.ClosestPoint(button.transform.position), button.transform.position);
            if (dist < distance) {
                distance = dist;
                closestCollider = item;
            }
        }

        if (distance < nearRadius + 1) {
            Color mat = originalMaterial.GetColor("_EmissionColor");
            Color final = Color.Lerp(mat, glowColor, 1 - distance/nearRadius);
            buttonRenderer.material.color = final;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Presser")) {
            if (float.IsPositiveInfinity(startPoint.x) || float.IsPositiveInfinity(startPoint.y) || float.IsPositiveInfinity(startPoint.z)) {
                startPoint = transform.InverseTransformPoint(other.ClosestPoint(button.transform.position));
                originalPoint = button.transform.localPosition;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Presser")) {
            Vector3 diff = transform.InverseTransformPoint(other.ClosestPoint(button.transform.position)) - startPoint;
            float distancePushed = Vector3.Dot(diff, buttonPushDirection);

            if (distancePushed > 0) {
                if (distancePushed <= buttonPushDistance) {
                    button.transform.localPosition = originalPoint + (distancePushed * Vector3.forward);
                }
                else { // Button is fully pressed
                    if (canCallAction && SingleFireButtonAction != null) {
                        SingleFireButtonAction();
                        canCallAction = false;
                        // Sound done in SingleFireButtonAction()
                    }
                    if (ContinuousFireButtonAction != null) {
                        ContinuousFireButtonAction();
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Presser")) {
            button.transform.localPosition = originalPoint;
            startPoint = Vector3.positiveInfinity;
            canCallAction = true;
        }
    }
}
