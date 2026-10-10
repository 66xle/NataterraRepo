using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionRadial : MonoBehaviour
{
    [Header("Radius Settings")]
    [SerializeField] float Radius = 5f;
    [SerializeField] float RadiusReferenceDistance = 10f;
    [SerializeField] [Range(0, 1)] float RadiusDistanceMultiplier = 1f;

    [Header("Icon Scale Settings")]
    [SerializeField] float Scale = 1f;
    [SerializeField] float ScaleReferenceDistance = 10f;
    [SerializeField] [Range(0, 1)] float ScaleDistanceMultiplier = 1f;

    List<Transform> _listOfIcons;
    Vector3 _origin;
    Camera _cam;

    public void SetIcons(List<Transform> icons, Vector3 position)
    {
        transform.position = position;
        _origin = position;
        _listOfIcons = icons;

        _cam = Camera.main;
    }

    public void Clear()
    {
        _listOfIcons.Clear();
    }

    private void Update()
    {
        UpdateIcons();
    }

    void UpdateIcons()
    {
        if (_cam == null || _listOfIcons == null || _listOfIcons.Count == 0)
            return;

        float distance = Vector3.Distance(_origin, _cam.transform.position);
        float radiusDistanceFactor = distance / Mathf.Max(RadiusReferenceDistance, 0.001f);
        float radius = Radius * Mathf.Lerp(1f, radiusDistanceFactor, RadiusDistanceMultiplier);

        float scaleDistanceFactor = distance / Mathf.Max(ScaleReferenceDistance, 0.001f);
        float iconScale = Scale * Mathf.Lerp(1f, scaleDistanceFactor, ScaleDistanceMultiplier);

        int iconCount = _listOfIcons.Count;
        for (int i = 0; i < iconCount; i++)
        {
            Transform icon = _listOfIcons[i];

            if (icon == null)
                continue;

            float angle = (i * (360f / iconCount) + 90f) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;

            icon.position = _origin + _cam.transform.rotation * offset;
            icon.rotation = _cam.transform.rotation;
            icon.localScale = Vector3.one * iconScale;
        }
    }
}
