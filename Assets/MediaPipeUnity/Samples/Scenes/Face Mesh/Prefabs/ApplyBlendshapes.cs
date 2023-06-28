using UnityEngine;
using System.Collections.Generic;

public class ApplyBlendshapes : MonoBehaviour
{
  [SerializeField] private GameObject _blendshapeInfo;
  [SerializeField] private GameObject _faceMeshObj;

  private BlendshapeInfo blendShapeInfo;
  private Mesh skinnedMesh;
  private SkinnedMeshRenderer skinnedMeshRenderer;

  // Start is called before the first frame update
  private void Start()
  {
    blendShapeInfo = _blendshapeInfo.GetComponent<BlendshapeInfo>();
    skinnedMeshRenderer = _faceMeshObj.GetComponent<SkinnedMeshRenderer>();
    skinnedMesh = skinnedMeshRenderer.sharedMesh;

    Debug.Log("blend shape count " + skinnedMesh.blendShapeCount);
  }

  private void Update()
  {
    var blendShapeCount = skinnedMesh.blendShapeCount;
    if (blendShapeCount == 0)
    {
      return;
    }

    if (blendShapeInfo.BlendShapes == null)
    {
      return;
    }

    for (var i = 0; i < blendShapeCount; i++)
    {
      var blendShapeName = skinnedMesh.GetBlendShapeName(i);
      // Check if blendShapeName is found in the dictionary
      if (!blendShapeInfo.BlendShapes.ContainsKey(blendShapeName))
      {
        continue;
      }
      var blendShapeValue = blendShapeInfo.BlendShapes[blendShapeName] * 100;
      skinnedMeshRenderer.SetBlendShapeWeight(i, blendShapeValue);
    }

  }
}
