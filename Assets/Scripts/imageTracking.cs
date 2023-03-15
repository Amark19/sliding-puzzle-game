using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class imageTracking : MonoBehaviour
{
    [SerializeField] private GameObject imageTrackingObject;
    private ARTrackedImageManager arTrackedImageManager;
    // Start is called before the first frame update
    void Awake()
    {
        arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            updateImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            updateImage(trackedImage);
            Debug.Log("Image Updated");
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            Debug.Log("Image Removed");
        }
    }

    void updateImage(ARTrackedImage trackedImage)
    {
        Vector3 position = trackedImage.transform.position;
        imageTrackingObject.transform.position = position;
        imageTrackingObject.SetActive(true);
    }


}
