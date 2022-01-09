using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splat : MonoBehaviour
{
    public int orderLayerInBackGround   = 6;
    public int orderLayerInForeGround   = 7;

    public bool VisibleInsideSpriteMask = true;
    
    public enum SplatLocation {
        Foreground,
        Background,
    }

    public Color backgroundTint;
    public float minSizeMod = 0.8f;
    public float maxSizeMod = 1.5f;

    public Sprite[] sprites;
    
    [SerializeField]
    private SplatLocation splatLocation;
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(SplatLocation splatLocation) {
        this.splatLocation = splatLocation;
        SetSprite();
        SetSize();
        SetRotation();

        SetLocationProperties();
    }

    void SetSprite() {
        int randomIndex = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[randomIndex];
    }

    void SetSize() {
        float sizeMod = Random.Range(minSizeMod, maxSizeMod);
        transform.localScale *= sizeMod;
    }

    void SetRotation() {
        float randomRotaion = Random.Range(-360f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, randomRotaion);
    }

    void SetLocationProperties() {
        switch (splatLocation)
        {
            case SplatLocation.Background:
                spriteRenderer.color = backgroundTint;
                if(VisibleInsideSpriteMask)
                    spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                spriteRenderer.sortingOrder = orderLayerInBackGround;
                break;

            case SplatLocation.Foreground:
                if (VisibleInsideSpriteMask)
                    spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                spriteRenderer.sortingOrder = orderLayerInForeGround;
                break;
        }
    }
}