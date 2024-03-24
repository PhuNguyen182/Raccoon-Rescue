using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLine : MonoBehaviour
{
    public Transform test;
    public LayerMask mask;

    private RaycastHit2D raycast;

    public void Cast(Ray2D ray)
    {
        raycast = Physics2D.Raycast(ray.origin, ray.direction, 25, mask);
        test.position = raycast.point;
    }
}
