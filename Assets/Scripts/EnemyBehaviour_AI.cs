using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour_AI : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; set; }
    GameObject SplatHolder;
    ParticleSystem SplatParticles;
    GameObject splatPrefab;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameObject SplatParticlesOBJ = GameObject.Find("SplatParticles");
        if (SplatParticlesOBJ != null)
        {
            SplatParticles = SplatParticlesOBJ.GetComponent<ParticleSystem>();
            SplatHolder = GameObject.Find("SplatHolder");
        }

        splatPrefab = Resources.Load("PrefabSinglePlayer/Bloods/Splat Sprite") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Movements etc
    }

    public void SplatBloods(int BloodCount)
    {
        if (SplatParticles != null)
        {
            for (int i = 0; i < BloodCount; i++)
            {
                float posRandomY = Random.Range(0.25f, 1f);
                float posRandomX = Random.Range(0.2f, 0.8f);
                Vector3 positionNew = new Vector3(posRandomX, posRandomY, 0);
                GameObject splat = Instantiate(splatPrefab, transform.position + positionNew, Quaternion.identity) as GameObject;
                splat.transform.SetParent(SplatHolder.transform, true);
                Splat splatScript = splat.GetComponent<Splat>();
                splatScript.Initialize(Splat.SplatLocation.Background);
            }
            SplatParticles.transform.position = transform.position;
            SplatParticles.Play();
        }
    }
}