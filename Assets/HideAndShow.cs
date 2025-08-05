using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HideAndShow : MonoBehaviour
{
    public GameObject RoofKonbini;
    [SerializeField] private List<MeshRenderer> targetRenderers;

    public void OnTriggerEnter(Collider other)
    {
        foreach (var renderer in targetRenderers)
        {
            renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        foreach (var renderer in targetRenderers)
        {
            renderer.shadowCastingMode = ShadowCastingMode.TwoSided;
        }
    }
}
