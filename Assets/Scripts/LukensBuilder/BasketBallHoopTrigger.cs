using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBallHoopTrigger : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        meshRenderer.material.color = Color.gray;
    }

    private void OnTriggerEnter(Collider other)
    {
        meshRenderer.material.color = Color.green;
        audioSource.PlayOneShot(audioSource.clip);
    }

    private void OnTriggerExit(Collider other)
    {
        meshRenderer.material.color = Color.gray;
    }
}
