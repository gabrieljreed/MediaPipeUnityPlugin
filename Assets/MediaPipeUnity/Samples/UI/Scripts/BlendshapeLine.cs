using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity
{
  public class BlendshapeLine : MonoBehaviour
  {
    [SerializeField] private Text _blendShapeName;
    [SerializeField] private Text _blendShapeValue;


    // Write a function called SetText that takes in 2 string parameters and sets the text of the 2 text objects to the values of the parameters
    public void SetBlendShapeName(string name, string value)
    {
      _blendShapeName.text = name.ToString();
      _blendShapeValue.text = value.ToString();
    }

    // Start is called before the first frame update
    private void Start()
    {
      return;
    }

    // Update is called once per frame
    private void Update()
    {
      return;
    }
  }
}


