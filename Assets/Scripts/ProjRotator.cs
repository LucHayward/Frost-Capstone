using UnityEngine;

/// <summary>
/// Spinds the given object around its vertical axis at a given rate
/// </summary>
public class ProjRotator : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0.0f, 0.9f, 0);
    }
}
