using UnityEngine;

public class HoleLocation : MonoBehaviour
{
    public static HoleLocation instance;
    private static MeshRenderer meshRenderer;
    [SerializeField] MeshRenderer _meshRenderer;

    private void Awake()
    {
        instance = this;
        meshRenderer = _meshRenderer;
        meshRenderer.enabled = false;
    }

    public static void ToggleLocation(bool toggle)
    {
        meshRenderer.enabled = toggle;
    }
}
