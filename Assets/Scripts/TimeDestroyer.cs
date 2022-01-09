using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDestroyer : MonoBehaviour {

    public bool isSpriteMask = false;
    public float timeToDestroy = 2f;

    float startTime;
    SpriteRenderer sprite;
    Color color;
    bool dying;

    void Start ()
    {
        if (isSpriteMask)
        {
            sprite = GetComponent<SpriteRenderer>();
            if (sprite)
                color = sprite.color;
            startTime = Time.time;
        }
        else
            Destroy(gameObject, timeToDestroy);
	}

    private void Update()
    {
        if (isSpriteMask == false)
            return;

        if (Time.time - startTime > timeToDestroy && !dying)
        {
            dying = true;
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        if (isSpriteMask) 
        {
            float value = sprite.color.a;
            while (value > 0)
            {
                value -= Time.deltaTime * 0.2f;
                if (sprite)
                    sprite.color = new Color(color.r, color.g, color.b, value);
                yield return new WaitForEndOfFrame();
            }
            Destroy(gameObject);
        }
    }
}