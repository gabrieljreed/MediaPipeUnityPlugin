// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.UI
{
  public class BlendshapeContents : MonoBehaviour
  {
    [SerializeField] private int _testInt = 5;
    [SerializeField] private GameObject _blendshapeLinePrefab;

    private Transform _contentRoot;
    private const string _ContentPath = "Viewport/Content";

    // Start is called before the first frame update
    private void Start()
    {
      InitializeView();
    }

    private void InitializeView()
    {
      _contentRoot = gameObject.transform.Find(_ContentPath).gameObject.transform;
      Debug.Log(_contentRoot);
      Debug.Log(_contentRoot.childCount);
      Debug.Log("InitializeView");

      // Create as many text boxes as _testInt
      for (var i = 0; i < _testInt; i++)
      {
        var blendShapeLine = Instantiate(_blendshapeLinePrefab, _contentRoot).GetComponent<BlendshapeLine>();
        blendShapeLine.SetBlendShapeName("test", i.ToString());
        Debug.Log("created blendShapeLine " + i);
      }

      return;
    }

    // Update is called once per frame
    private void Update()
    {
      return;
    }
  }
}
