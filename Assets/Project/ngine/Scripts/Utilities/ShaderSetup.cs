using Cngine;
using UnityEngine;

public class ShaderSetup : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private MeshRenderer _renderer;
    private Material _referencedMaterial;
    
    private void Awake()
    {
        _referencedMaterial = _renderer?.material ?? GetComponent<Renderer>()?.material;
        if(_referencedMaterial == null)
        {
           Log.Error("material not found ");
        }
    }
}
