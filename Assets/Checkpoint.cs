using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int goldIndex = -1;

    [SerializeField] Transform flagTransform;
    [SerializeField] MeshRenderer flagMeshRenderer;

    public Color onColor = Color.green;
    public Color offColor = Color.red;

    public Vector3 onPosition = Vector3.one;
    public Vector3 offPosition = Vector3.zero;

    private void Start()
    {
        SetActive(false);
    }


    public void SetActive(bool isActive)
    {
        flagTransform.localPosition = isActive ? onPosition : offPosition;
        flagMeshRenderer.material.color = isActive ? onColor : offColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = other.gameObject.GetComponentInParent<Player>();
            if (goldIndex >= 0) player.goldFlags[goldIndex] = true;
            if (player.checkpoint != null && player.checkpoint.goldIndex < 0) player.checkpoint.SetActive(false);
            if (goldIndex == 2) player.goldCheckpoint = this;
            player.checkpoint = this;
            SetActive(true);
        }
    }
}
