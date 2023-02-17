using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] Tile tilePrefab;
    [SerializeField] CardDropField cardFieldPrefab;
    [SerializeField] Transform tileParent, cardFieldParent;
    [SerializeField] Tile playerTile, enemySpawnerTile;

    const int basePathSize = 10;
    float tileWidthBounds;

    Dictionary<Tile, CardDropField> tilesToField = new Dictionary<Tile, CardDropField>();
    List<Tile> allTiles = new List<Tile>();
    private void Start()
    {
        tileWidthBounds = 1.1f;
        GenerateBasePath();
    }

    public void GenerateBasePath()
    {
        //put player thing at world 0, spawn in X tiles then move enemy spawner tile to the right

        playerTile.transform.position = Vector3.zero;
        Vector3 placeAt = Vector3.zero + Vector3.right * tileWidthBounds; //* x bounds size of a prefab?
        allTiles.Add(playerTile);

        AddFieldToTile(playerTile);

        for (int i = 0; i < basePathSize; i++)
        {
            Tile spawnedTile = Instantiate(tilePrefab, tileParent);
            AddFieldToTile(spawnedTile);
            allTiles.Add(spawnedTile);
            spawnedTile.transform.position = placeAt;
            placeAt+= Vector3.right * tileWidthBounds;
        }

        enemySpawnerTile.transform.position = placeAt;

        allTiles.Add(enemySpawnerTile);
        AddFieldToTile(enemySpawnerTile);

        //RecalculateFields(); -> cinemachine on blend complete
        Invoke("RecalculateFields", 2f);
    }

    public void AddFieldToTile(Tile tile)
    {
        CardDropField f = Instantiate(cardFieldPrefab, cardFieldParent);
        f.gameObject.name = "field #" + cardFieldParent.childCount;
        tilesToField.Add(tile, f);


    }

    public void AddPathTile()
    {
        //add a path tile at enemy spawner position, move enemy spawner 1 tile to the right
        //animate this later
        //slap on the field thing
        Tile spawnedTile = Instantiate(tilePrefab, tileParent);
        AddFieldToTile(spawnedTile);
        allTiles.Add(spawnedTile);

        spawnedTile.transform.position = enemySpawnerTile.transform.position;
        enemySpawnerTile.transform.position += Vector3.right * tileWidthBounds;

        Invoke("RecalculateFields", 2f);//when camera adjusts
    }

    public void RecalculateFields()
    {
        //get screen height for top
        int screenHeight = Screen.height;
        Camera cam = Camera.main;
        foreach (Tile tile in allTiles)
        {
            CardDropField field = tilesToField[tile];
            Vector3 rightSidePos = cam.WorldToScreenPoint(tile.rightSide.position);
            field.rectTransform.position = cam.WorldToScreenPoint(tile.leftSide.position);

            field.rectTransform.sizeDelta = new Vector2(rightSidePos.x - field.rectTransform.position.x, screenHeight - rightSidePos.y);
        }
    }
}
