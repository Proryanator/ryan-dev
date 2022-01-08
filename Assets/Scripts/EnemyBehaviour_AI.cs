using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyBehaviour_AI : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; set; }
    GameObject SplatHolder;
    ParticleSystem SplatParticles;
    GameObject splatPrefab;
    
    private Collider2D playerCollider;
    // would populate this with layers we want the enemy to ignore, like other enemies or items
    private ContactFilter2D enemyVisionFilter;
    private LayerMask playerLayer;
    private Vector2 patrolDirection = Vector2.right;

    [SerializeField] private int moveSpeed = 3;

    private Vector2 directionTowardsPlayer;
        
    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        enemyVisionFilter = new ContactFilter2D();
        enemyVisionFilter.SetLayerMask(playerLayer);
        playerLayer = LayerMask.GetMask("Player", "Ground");
    }

    void Start()
    {
        GameObject SplatParticlesOBJ = GameObject.Find("SplatParticles");
        GetPlayerCollider();
        
        if (SplatParticlesOBJ != null)
        {
            SplatParticles = SplatParticlesOBJ.GetComponent<ParticleSystem>();
            SplatHolder = GameObject.Find("SplatHolder");
        }

        splatPrefab = Resources.Load("PrefabSinglePlayer/Bloods/Splat Sprite") as GameObject;
    }

    private void GetPlayerCollider()
    {
        playerCollider = GameObject.FindWithTag("Player").GetComponentInChildren<Collider2D>();
        if (playerCollider == null)
        {
            Debug.LogWarning("Not able to find the player via 'Player Tag', enemy AI tracking will not work");
        }
    }
    
    public void Update()
    {
        directionTowardsPlayer = playerCollider.transform.position - transform.position;
        
        if (CanSeePlayer())
        {
            FollowPlayer();
        }
        else{
            Patrol();
        }
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
    
    /// <summary>
    /// Normalizes vector towards the player and moves either left/right towards the player
    /// </summary>
    private void FollowPlayer(){
        Vector2 normalizedDirectionToPlayer = new Vector2(directionTowardsPlayer.normalized.x, 0);
        Move(normalizedDirectionToPlayer, moveSpeed);
    }

    /// <summary>
    /// Enemy wanders in a right to left pattern. Enemy reverses direction to avoid falling off a cliff or hitting a wall
    /// </summary>
    private void Patrol(){
        int lookAheadDistance = 1;
        Vector2 lookAhead = (Vector2) transform.position + (patrolDirection * lookAheadDistance);
        
        Debug.DrawLine(transform.position, lookAhead, Color.blue);
        if (WillEnemyWalkOffCliff(lookAhead, lookAheadDistance) || WillEnemyHitWall(lookAhead, lookAheadDistance)){
            patrolDirection *= -1;
        }
        
        // move in the patrol direction
        Move(patrolDirection, moveSpeed);
    }

    private void Move(Vector2 direction, int moveSpeed){
        Rigidbody.velocity = new Vector2(moveSpeed * direction.x, Rigidbody.velocity.y);
    }
    /// <summary>
    /// Determine if some distance ahead of the enemy's current position will cause it to collide with a wall
    /// </summary>
    /// <returns>true if the enemy will collide with a wall soon, false if not</returns>
    private bool WillEnemyHitWall(Vector2 lookAhead, int lookDistance){
        RaycastHit2D wallHit = Physics2D.Raycast(lookAhead, patrolDirection, lookDistance);
        return wallHit.collider != null && wallHit.collider.CompareTag("Ground");
    }
    
    /// <summary>
    /// Determine if some distance ahead of the enemy's current position will cause it to fall off a cliff
    /// </summary>
    /// <returns>true if the enemy falls off soon, false if not</returns>
    private bool WillEnemyWalkOffCliff(Vector2 lookAhead, int lookDistance){
        RaycastHit2D groundHit = Physics2D.Raycast(lookAhead, Vector2.down, lookDistance);
        return groundHit.collider == null || !groundHit.collider.CompareTag("Ground");
    }
    
    /// <summary>
    /// Determine if the enemy can currently see the player, should not count seeing the tilemap layer
    /// </summary>
    /// <returns>true if the player is seen, false if not</returns>
    private bool CanSeePlayer(){
        // can adjust the view distance later on
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionTowardsPlayer, float.MaxValue, playerLayer);
        
        Color rayColor = hit.collider.CompareTag("Player") ? Color.green : Color.black;
        Debug.DrawRay(transform.position, directionTowardsPlayer, rayColor);
        return hit.collider != null && hit.collider.gameObject.CompareTag("Player");
    }
}