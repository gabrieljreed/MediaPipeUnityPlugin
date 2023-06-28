// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.FaceMesh
{
  public class FaceMeshSolution : ImageSourceSolution<FaceMeshGraph>
  {
    [SerializeField] private DetectionListAnnotationController _faceDetectionsAnnotationController;
    [SerializeField] private MultiFaceLandmarkListAnnotationController _multiFaceLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _faceRectsFromLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _faceRectsFromDetectionsAnnotationController;
    [SerializeField] private GameObject _blendshapeInfo;

    private BlendshapeInfo _blendshapeInfoScript;

    protected override IEnumerator Start()
    {
      _blendshapeInfoScript = _blendshapeInfo.GetComponent<BlendshapeInfo>();
      return base.Start();
    }

    public int maxNumFaces
    {
      get => graphRunner.maxNumFaces;
      set => graphRunner.maxNumFaces = value;
    }

    public bool refineLandmarks
    {
      get => graphRunner.refineLandmarks;
      set => graphRunner.refineLandmarks = value;
    }

    public float minDetectionConfidence
    {
      get => graphRunner.minDetectionConfidence;
      set => graphRunner.minDetectionConfidence = value;
    }

    public float minTrackingConfidence
    {
      get => graphRunner.minTrackingConfidence;
      set => graphRunner.minTrackingConfidence = value;
    }

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnFaceDetectionsOutput += OnFaceDetectionsOutput;
        graphRunner.OnMultiFaceLandmarksOutput += OnMultiFaceLandmarksOutput;
        graphRunner.OnFaceRectsFromLandmarksOutput += OnFaceRectsFromLandmarksOutput;
        graphRunner.OnFaceRectsFromDetectionsOutput += OnFaceRectsFromDetectionsOutput;
        graphRunner.OnFaceClassificationsFromBlendShapesOutput += OnFaceClassificationsFromBlendShapesOutput;
      }

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_faceDetectionsAnnotationController, imageSource);
      SetupAnnotationController(_faceRectsFromLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_multiFaceLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_faceRectsFromDetectionsAnnotationController, imageSource);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      List<Detection> faceDetections = null;
      List<NormalizedLandmarkList> multiFaceLandmarks = null;
      List<NormalizedRect> faceRectsFromLandmarks = null;
      List<NormalizedRect> faceRectsFromDetections = null;
      ClassificationList faceBlendShapes = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out faceDetections, out multiFaceLandmarks, out faceRectsFromLandmarks, out faceRectsFromDetections, out faceBlendShapes, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out faceDetections, out multiFaceLandmarks, out faceRectsFromLandmarks, out faceRectsFromDetections, out faceBlendShapes, false));
      }

      _faceDetectionsAnnotationController.DrawNow(faceDetections);
      _multiFaceLandmarksAnnotationController.DrawNow(multiFaceLandmarks);
      _faceRectsFromLandmarksAnnotationController.DrawNow(faceRectsFromLandmarks);
      _faceRectsFromDetectionsAnnotationController.DrawNow(faceRectsFromDetections);
      if (faceBlendShapes != null)
      {
        Debug.Log($"Blendshapes count (a) {faceBlendShapes.Classification.Count}");
        // Print out each of the blendshapes
        foreach (var blendshape in faceBlendShapes.Classification)
        {
          Debug.Log($"{blendshape.Label} {blendshape.Score}");
        }
      }

      // Loop through the blendshapes and print them to the console
      foreach (var blendshape in faceBlendShapes.Classification)
      {
        Debug.Log($"{blendshape.Label} {blendshape.Score}");
      }
    }

    private void OnFaceDetectionsOutput(object stream, OutputEventArgs<List<Detection>> eventArgs)
    {
      _faceDetectionsAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnMultiFaceLandmarksOutput(object stream, OutputEventArgs<List<NormalizedLandmarkList>> eventArgs)
    {
      _multiFaceLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnFaceRectsFromLandmarksOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
    {
      _faceRectsFromLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnFaceRectsFromDetectionsOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
    {
      _faceRectsFromDetectionsAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnFaceClassificationsFromBlendShapesOutput(object stream, OutputEventArgs<ClassificationList> eventArgs)
    {
      // Debug.Log($"Blendshapes count (b) {eventArgs.value?.Classification?.Count}");
      // // If we have a valid list of blendshapes, print them to the console
      // if (eventArgs.value != null)
      // {
      //   foreach (var blendshape in eventArgs.value.Classification)
      //   {
      //     Debug.Log($"{blendshape.Label} {blendshape.Score}");
      //   }
      // }

      if (eventArgs.value?.Classification?.Count == 0 || eventArgs.value == null)
      {
        return;
      }

      // Create a map of the blendshapes where the key is the name of the blendshape and the value is the score
      var blendshapes = new Dictionary<string, float>();
      foreach (var blendshape in eventArgs.value.Classification)
      {
        blendshapes.Add(blendshape.Label, blendshape.Score);
      }
      // Debug.Log(blendshapes);
      // _blendshapeInfo.GetComponent<BlendshapeInfo>().blendshapes = blendshapes;
      // _blendshapeInfo.GetComponent<BlendshapeInfo>().BlendShapes = blendshapes;
      _blendshapeInfoScript.BlendShapes = blendshapes;
      return;
    }
  }
}
