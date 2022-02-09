using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour
{
    [SerializeField] GameObject cubeToSpawn = null;
    [SerializeField] Transform movingStage;
    [SerializeField] private int maxCubes = 3;

    private List<GameObject> cubeHistory = new List<GameObject>();
    private Camera _cam = null;
    
    void Start()
    {
      _cam = Camera.main;
    }

    void Update()
    {
        SpawnAtMousePos();
    }

    void SpawnAtMousePos(){
        if(Input.GetMouseButtonDown(1)){
            Debug.Log("right Click");
            Vector3 position = _cam.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y, 
                Mathf.Abs(_cam.transform.position.z)
            ));
            if (cubeHistory.Count < maxCubes){
                var newCube = Instantiate(cubeToSpawn, position, Quaternion.identity, movingStage) as GameObject;
                cubeHistory.Add(newCube);
            } else {
                cubeHistory[0].transform.position = position;
                cubeHistory.Add(cubeHistory[0]);
                cubeHistory.RemoveAt(0);
            }
        }
    }
}
