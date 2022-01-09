using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Scans entire scene and generates SpriteMasks per cell in every Tilemap.
///
/// NOTE: can be expanded upon/hooked into a level loading system to be called when new areas are loaded.
/// </summary>
public class GenerateSpriteMasksForGrid : MonoBehaviour{
    
    private GameObject bloodSpriteMaskPrefab;
    private Tilemap[] tilemapsInScene;
    
    private void Awake(){
        bloodSpriteMaskPrefab = Resources.Load("PrefabSinglePlayer/Bloods/BloodSPriteMask") as GameObject;
    }

    // Start is called before the first frame update
    void Start(){
        tilemapsInScene = FindObjectsOfType<Tilemap>();
        if (tilemapsInScene == null)
        {
            Debug.LogWarning("No tilemaps found in the scene, is this the right scene?");    
        }
        
        AddSpriteMasksToAllTilemapsInScene(tilemapsInScene);
    }

    private void AddSpriteMasksToAllTilemapsInScene(Tilemap[] tilemapsInScene){
        foreach (Tilemap tilemap in tilemapsInScene)
        {
            AddMaskSpritePerCell(tilemap);
        }
    }
    
    private void AddMaskSpritePerCell(Tilemap tilemap)
    {
        // note: for some reason this is spawning a lot more
        foreach (Vector3Int tilePosition in tilemap.cellBounds.allPositionsWithin){
            if (!tilemap.HasTile(tilePosition)){
                continue;
            }
            
            Tile tile = (Tile) tilemap.GetTile(tilePosition);
            Vector3 worldLocation = tilemap.layoutGrid.CellToWorld(tilePosition);
            // need to add half the grid cell size to match with the tile itself
            worldLocation += tilemap.cellSize / 2;
            SpawnBloodSpriteMask(worldLocation, tile.sprite, tilemap.transform);
        }
    }
    
    /// <summary>
    /// Spawns the sprite mask prefab for blood right at the tile location.
    /// 
    /// </summary>
    /// <param name="location">Vector3 location to spawn the sprite mask</param>
    /// <param name="sprite">Sprite to set as the sprite mask</param>
    /// <param name="parent">The parent transform to hold all the sprite masks under</param>
    private void SpawnBloodSpriteMask(Vector3 location, Sprite sprite, Transform parent){
        GameObject bloodSpriteMask = Instantiate(bloodSpriteMaskPrefab, location, Quaternion.identity);
        bloodSpriteMask.transform.SetParent(parent);
        SpriteMask spriteMask = bloodSpriteMask.GetComponent<SpriteMask>();
        spriteMask.sprite = sprite;
    }
}
