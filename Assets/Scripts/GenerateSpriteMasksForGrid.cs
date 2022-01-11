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
    // used to pull pre-configured sorting order data
    private GameObject spriteSplatPrefab;
    private Tilemap[] tilemapsInScene;

    // data used to properly assign masks to a pre-set order
    private int backgroundOrderInLayer;
    private int foregroundOrderInLayer;

    private void Awake(){
        cellSpriteMaskPrefab = Resources.Load("PrefabSinglePlayer/Bloods/CellSpriteMask") as GameObject;
        
        spriteSplatPrefab = Resources.Load("PrefabSinglePlayer/Bloods/Splat Sprite") as GameObject;
        Splat splat = spriteSplatPrefab.GetComponent<Splat>();
        backgroundOrderInLayer = splat.orderLayerInBackGround;
        foregroundOrderInLayer = splat.orderLayerInForeGround;
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
    
    private void AddMaskSpritePerCell(Tilemap tilemap){
        foreach (Vector3Int tilePosition in tilemap.cellBounds.allPositionsWithin){
            if (!tilemap.HasTile(tilePosition)){
                continue;
            }
            
            Tile tile = (Tile) tilemap.GetTile(tilePosition);
            Vector3 worldLocation = tilemap.layoutGrid.CellToWorld(tilePosition);
            
            // need to add half the grid cell size to match with the tile itself
            worldLocation += tilemap.cellSize / 2;
            
            // NOTE: this can all be more generalized, for now it's just on a foreground/background basis
            string gridSortingLayer = tilemap.GetComponent<TilemapRenderer>().sortingLayerName;
            
            int sortingOrder = gridSortingLayer == "Background"
                ? backgroundOrderInLayer
                : foregroundOrderInLayer;

            int sortingLayerId = tilemap.GetComponent<TilemapRenderer>().sortingLayerID;
            SpawnBloodSpriteMask(worldLocation, tile.sprite, tilemap.transform, sortingOrder, sortingLayerId);
        }
    }
    
    /// <summary>
    /// Spawns the sprite mask prefab at the exact location in world coordinates, of the tile sprite
    /// 
    /// </summary>
    /// <param name="location">Vector3 location to spawn the sprite mask</param>
    /// <param name="sprite">Sprite to set as the sprite mask</param>
    /// <param name="parent">The parent transform to hold all the sprite masks under</param>
    /// <param name="sortingInLayer">The index from the splat prefab for this sprite masks' range</param>
    /// <param name="sortingLayerId">The sorting layer to put this sprite mask's foreground/background on</param>
    private void SpawnBloodSpriteMask(Vector3 location, Sprite sprite, Transform parent, int sortingInLayer, int sortingLayerId){
        GameObject cellSpriteMask = Instantiate(cellSpriteMaskPrefab, location, Quaternion.identity);
        cellSpriteMask.transform.SetParent(parent);
        SpriteMask spriteMask = cellSpriteMask.GetComponent<SpriteMask>();
        spriteMask.sprite = sprite;
        
        // now set the range for the sprite mask to only affect a specific type of blood
        // NOTE: must set front, then back (reverse will not set one of them)
        spriteMask.frontSortingLayerID = sortingLayerId;
        spriteMask.backSortingLayerID = sortingLayerId;

        spriteMask.frontSortingOrder = sortingInLayer + 1;
        spriteMask.backSortingOrder = sortingInLayer - 1;
    }
}
