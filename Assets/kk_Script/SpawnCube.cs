using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour
{
    [SerializeField] GameObject AnglePrefab = null;
    [SerializeField] Transform movingStage;
    private Camera _cam = null;
    
    [SerializeField] private int maxCubes = 3;
    private List<GameObject> cubes = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
      _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnAtMousePos();
    }

    void SpawnAtMousePos(){
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log("left click");
        //}
        if(Input.GetMouseButtonDown(1)){
            Debug.Log("right Click");
            Vector3 position = _cam.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y, 
                Mathf.Abs(_cam.transform.position.z)
            ));
            if (cubes.Count < maxCubes){
                var newCube = Instantiate(AnglePrefab, position, Quaternion.identity, movingStage) as GameObject;
                cubes.Add(newCube);
            } else {
                cubes[0].transform.position = position;
                cubes.Add(cubes[0]);
                cubes.RemoveAt(0);
            }

        }
    }
}
