using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Wink());
    }

    private IEnumerator Wink()
    {
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = !spriteRenderer.enabled;
        StartCoroutine(Wink());
    }

    void OnEnable()
    {
        StartCoroutine(Wink());
    }
}
