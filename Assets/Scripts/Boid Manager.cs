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

    [SerializeField]
    Transform boidPrefab;

    [SerializeField]
    Vector3 range;
    
    Transform[] boids;


    void Awake() {
        boids = new Transform[numberOfBoids];
        float x, y, z, rotation;
        for(int i = 0; i < numberOfBoids; i++){
            x = Random.Range(-range.x, range.x);
            y = Random.Range(-range.y, range.y);
            z = 0f;
            rotation = Random.Range(0f, 360f);
            Transform boid = Instantiate(
                boidPrefab, 
                new Vector3(x, y, z), 
                Quaternion.Euler(0f, 0f, rotation));
            boids[i] = boid;
            boid.SetParent(transform, false);
        }
    }

    void Update() {
        foreach(Transform boid in boids){
            boid.transform.Translate(Vector3.up * (speed * Time.deltaTime));
        }
        
    }

    
}
