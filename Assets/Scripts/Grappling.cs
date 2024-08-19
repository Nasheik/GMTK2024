using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    private Player player;
    public Transform cam;
    public Transform gunTip;
    public LayerMask grapplableLayers;

    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    public Vector3 grapplePoint;
    public LineRenderer lineRenderer;

    public float grappleCooldown;
    private float grappleCdTimer;

    private bool isGrappling;

    private void Start()
    {
        player = GetComponent<Player>();
        cam = FindObjectOfType<PlayerCamera>().transform;
    }

    private void Update()
    {
        if(player.input.GetGrappleInput() == 1) StartGrapple();
        if (grappleCdTimer > 0) grappleCdTimer -= Time.deltaTime;
    }
    private void LateUpdate()
    {
        if(isGrappling) lineRenderer.SetPosition(0, gunTip.position);
    }

    void StartGrapple()
    {
        if (grappleCdTimer > 0) return;

        isGrappling = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, grapplableLayers))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, grapplePoint);
    }

    void ExecuteGrapple()
    {
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelYPos + overshootYAxis;

        if (grapplePointRelYPos < 0) highestPointOnArc = overshootYAxis;

        player.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        isGrappling = false;

        grappleCdTimer = grappleCooldown;

        lineRenderer.enabled = false;

    }
}
