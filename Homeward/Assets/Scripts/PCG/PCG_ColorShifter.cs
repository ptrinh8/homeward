﻿// ==================================================================================
// <file="PCG_ColorShifter.cs" product="Homeward">
// <date>02-12-2014</date>
// ==================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PCG_ColorShifter : MonoBehaviour
{
    public float MinDistance;
    public float MaxDistance;
    public bool darkest, light, lightest;

    public Transform Target;
    protected SpriteRenderer SpriteRenderer;

    protected void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        float distance = Vector2.Distance(transform.position, Target.transform.position);
        float ratio = Mathf.Clamp01((distance - MinDistance) / (MaxDistance - MinDistance));
        float inverseRatio = 1f - ratio;

        if (darkest) { SpriteRenderer.color = new Color(ratio/2, 0f, inverseRatio/2); }
        else if (light) { SpriteRenderer.color = new Color(ratio, 0f, inverseRatio); }
        else if (lightest) { SpriteRenderer.color = new Color(ratio*3, 0f, inverseRatio*3); }
    }
}