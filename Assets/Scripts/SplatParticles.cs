using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SplatParticles : MonoBehaviour
{
    public ParticleSystem splatParticles;
    [HideInInspector]
    public GameObject splatPrefab;
    public Transform splatHolder;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    // the only layers that we'll want to spawn blood marks on
    private string[] bloodMaskLayerNames = { "Ground", "Wall" };
    private LayerMask bloodLayerMask;
    
    private void Start(){
        bloodLayerMask = LayerMask.GetMask(bloodMaskLayerNames);
        splatPrefab = Resources.Load("PrefabSinglePlayer/Bloods/Splat Sprite") as GameObject;
    }

    private void OnParticleCollision(GameObject other){
        ParticlePhysicsExtensions.GetCollisionEvents(splatParticles, other, collisionEvents);

        int count = collisionEvents.Count;

        for (int i = 0; i < count; i++) {
            // collect the collider, and by extension, the sprite of the tilemap the particle hit
            ParticleCollisionEvent particleEvent = collisionEvents[i];
            GameObject go = particleEvent.colliderComponent.gameObject;

            // ignore collisions that are not with pre-defined collision layers
            if (!ContainsLayer(bloodLayerMask, go.layer)){
                continue;
            }
            
            // determine the location of the collision and change it's color
            Splat.SplatLocation location = go.layer == LayerMask.NameToLayer("Ground")
                ? Splat.SplatLocation.Foreground
                : Splat.SplatLocation.Background;

            GameObject splat = Instantiate(splatPrefab, particleEvent.intersection, Quaternion.identity);
            splat.transform.SetParent(splatHolder, true);
            Splat splatScript = splat.GetComponent<Splat>();
            splatScript.Initialize(location);
        }
    }

    private static bool ContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}