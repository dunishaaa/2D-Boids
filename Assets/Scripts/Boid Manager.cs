using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoidManager : MonoBehaviour
{
    [SerializeField, Range(0, 400)]
    int numberOfBoids;
    [SerializeField, Range(0, 3)]
    float speed = 1f;
    [SerializeField, Range(0, 1)]
    float perceptionDistance = 0;

    [SerializeField]
    Transform boidPrefab;

    [SerializeField]
    Vector3 range;

    [SerializeField]
    bool collision, directionMatching, centering, respawn;
    
    Transform[] boids;


    void Awake() {
        boids = new Transform[numberOfBoids];
        for(int i = 0; i < numberOfBoids; i++){
            float rotation = Random.Range(0f, 360f);
            Transform boid = Instantiate(
                boidPrefab, 
                GetRandomPosition(), 
                Quaternion.Euler(0f, 0f, rotation));
            boids[i] = boid;
            boid.SetParent(transform, false);
        }
    }

    void Update() {
        foreach(Transform boid in boids){
            Boid boid1 = boid.GetComponent<Boid>();
            boid1.Move(speed);
            if(directionMatching)
                boid1.DirectionMatching(boids, perceptionDistance);

            if(ValidPosition(boid.transform.localPosition)){
                boid.transform.localPosition = GetRandomPosition();
            }

        }
        
    }

    bool ValidPosition(Vector3 localPosition){
        return respawn && (localPosition.x < -range.x || localPosition.y < -range.y 
            || localPosition.y > range.x || localPosition.y > range.y);
    }

    Vector3 GetRandomPosition(){
        float x, y, z;
        x = Random.Range(-range.x, range.x);
        y = Random.Range(-range.y, range.y);
        z = 0f;
        return new Vector3(x, y, z);
    }
    
}
