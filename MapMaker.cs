using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class WrapBool{
    public bool enclosed = true;
    public bool wrapped = false;

    public WrapBool(bool enclosed, bool wrapped)
    {
        this.enclosed = enclosed;
        this.wrapped = wrapped;
    }
}

public class MapMaker : MonoBehaviour
{
    public static MapMaker Instance;

    public Vector3 RequestPosition(Vector3 requestedPosition){
        float newX = requestedPosition.x < 0 ? Width-requestedPosition.x : requestedPosition.x % Width;
        float newY = requestedPosition.y < 0 ? Height-requestedPosition.y : requestedPosition.y % Height;
        return new Vector3( newX, newY, requestedPosition.z);
    }

    public Tilemap Map;
    public int RenderWidth = 25;
    public int Width = 25;
    public int RenderHeight = 25;
    public int Height = 25;
    public Vector2Int Offset;


    [Header("Sprites")]
    public List<Sprite> sprites;
    public GameObject Sheepprefab;
    public GameObject BaddiePrefab;

    void fill_map(){
        Tile defaultTile = new Tile();
        defaultTile.sprite = sprites[0];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Map.SetTile(new Vector3Int(i, j, 0), defaultTile);
            }
        }
    }

    void place_sheep(int sheep){
        for (int i = 0; i < sheep; i++)
        {
            Vector3 location = new Vector3(Random.Range(0, Width), Random.Range(0, Height), -1);
            Instantiate(Sheepprefab, location, Quaternion.identity);
        }
    }

    
    void place_Baddies(int count){
        for (int i = 0; i < count; i++)
        {
            Vector3 location = new Vector3(Random.Range(0, Width), Random.Range(0, Height), -1);
            Instantiate(BaddiePrefab, location, Quaternion.identity);
        }
    }

    // public float Distance(Vector3 a, Vector3 b){
    //     //if()    
    //     return 0;
    // }

    public WrapBool WithinRadius(Vector3 source, float Radius, Vector3 target){
        if(Vector3.Distance(source, target) < Radius)
            return new WrapBool(true, false);
        
        //Mirror x 
        Vector3 tempWrapPosition = new Vector3(Width-target.x, target.y, target.z);
        if(Vector3.Distance(source, tempWrapPosition) < Radius)
            return new WrapBool(true, true);
        
        //Mirror y 
        tempWrapPosition = new Vector3(target.x, Height-target.y, target.z);
        if(Vector3.Distance(source, tempWrapPosition) < Radius)
            return new WrapBool(true, true);
        

        return new WrapBool(false, false);
    }

    public WrapBool WithinRadius2(Vector3 source, float Radius, Vector3 target){
        if(Vector3.Distance(source, target) < Radius)
            return new WrapBool(true, false);
        
        //Normalise positions with dog in centre of map.
        Vector3 normalisedDog = new Vector3(Width/2, Height/2, 0);
        Vector3 normalisedTarget = target;
        if(Mathf.Abs(source.x - target.x) >= Width/2){
            //Normalise me
            if(target.x > Width/2){
                normalisedTarget.x = Width/2 - target.x;
            }
            else{
                normalisedTarget.x = Width/2 + target.x;
            }
        }
        // if(Mathf.Abs(source.y - target.y) <= Height/2){
        //     //Normalise me
        //     normalisedTarget.y = Height/2 - target.y;
        // }
        if(Vector3.Distance(normalisedDog, normalisedTarget) < Radius)
            return new WrapBool(true, true);
        

        return new WrapBool(false, false);
    }


    void init(){
        Offset = new Vector2Int(Width, Height);
        fill_map();
        place_sheep(150);
        place_Baddies(2);
    }

    void Awake(){
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {  
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
