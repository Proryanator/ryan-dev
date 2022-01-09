using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Scans entire scene and generates SpriteMasks per cell in every Tilemap
///
/// NOTE: can be expanded upon/hooked into a level loading system to be called when new areas are loaded
/// </summary>
public class GenerateSpriteMasksForGrid : MonoBehaviour{
    
    private GameObject cellSpriteMaskPrefab;
    private Tilemap[] tilemapsInScene;
    
    private void Awake(){
        cellSpriteMaskPrefab = Resources.Load("PrefabSinglePlayer/Bloods/CellSpriteMask") as GameObject;
    }
    
    void Start(){
        tilemapsInScene = FindObjectsOfType<Tilemap>();
        if (tilemapsInScene == null)
        {
            Debug.LogWarning("No tilemaps found in the scene, is this the right scene?");    
        }
        else
        {
            AddSpriteMasksToAllTilemapsInScene(tilemapsInScene);   
        }
    }

    private void AddSpriteMasksToAllTilemapsInScene(Tilemap[] tilemapsInScene){
        foreach (Tilemap tilemap in tilemapsInScene)
        {
            AddMaskSpritePerCell(tilemap);
        }
    }
    
    private void AddMaskSpritePerCell(Tilemap tilemap)
    {
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
    /// Spawns the sprite mask prefab at the exact location in world coordinates, of the tile sprite
    /// 
    /// </summary>
    /// <param name="location">Vector3 location to spawn the sprite mask</param>
    /// <param name="sprite">Sprite to set as the sprite mask</param>
    /// <param name="parent">The parent transform to hold all the sprite masks under</param>
    private void SpawnBloodSpriteMask(Vector3 location, Sprite sprite, Transform parent){
        GameObject cellSpriteMask = Instantiate(cellSpriteMaskPrefab, location, Quaternion.identity);
        cellSpriteMask.transform.SetParent(parent);
        SpriteMask spriteMask = cellSpriteMask.GetComponent<SpriteMask>();
        spriteMask.sprite = sprite;
    }
}
