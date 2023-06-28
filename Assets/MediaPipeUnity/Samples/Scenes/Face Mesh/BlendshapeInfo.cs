using System.Collections.Generic;
using UnityEngine;

public class BlendshapeInfo : MonoBehaviour
{

  private Dictionary<string, float> blendShapes;

  public Dictionary<string, float> BlendShapes
  {
    get { return blendShapes; }
    set { blendShapes = value; }
  }

  private void Start()
  {
    return;
  }

  private void Update()
  {
    if (blendShapes == null)
    {
      return;
    }

    //foreach (KeyValuePair<string, float> shape in blendShapes)
    //{
    //  Debug.Log(shape.Key + ": " + shape.Value);
    //}

    return;
  }
}
