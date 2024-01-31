using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField, Range(0, 400)]
    int numberOfBoids;
    [SerializeField, Range(0, 3)]
    float speed = 1f;

    [SerializeField, Range(0.5f, 2f)]
    float perceptionDistance = 0;

    [SerializeField]
    Transform boidPrefab;

    [SerializeField]
    Vector3 range;

    [SerializeField]
    bool AvoidCollision, matchDirection, centerFlock, showDistance;
    
    
    [SerializeField, Range(.01f, .05f)]
    float lineWidth;

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
            if(showDistance) ShowValidDistances();

            if(matchDirection)
                boid1.MatchDirection(boids, perceptionDistance);

            if(centerFlock)
                boid1.FindCentroid(boids, perceptionDistance);
            
            if(AvoidCollision)
                boid1.AvoidCollision(boids, perceptionDistance);

            boid1.Move(speed);

            Vector3 localBoid = boid.transform.position;
            if(!ValidPosition(localBoid)){
                if(localBoid.x > range.x) localBoid.x = -range.x;
                if(localBoid.x < -range.x) localBoid.x = range.x;

                if(localBoid.y > range.y) localBoid.y = -range.y;
                if(localBoid.y < -range.y) localBoid.y = range.y;
                
                boid.transform.position = localBoid;

            }

        }
        
    }

    

    void ShowValidDistances(){
        Boid firstBoid = boids[0].GetComponent<Boid>();
        SpriteRenderer spriteRenderer = boids[0].GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.yellow;

        List<Vector3> endPoint = new List<Vector3>();

        LineRenderer lineRenderer = boids[0].GetComponent<LineRenderer>();
        if(lineRenderer == null){
            lineRenderer = boids[0].AddComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 0;

        for(int i = 1; i < boids.Length; i++){
            Transform other = boids[i];
            SpriteRenderer otherSprite = other.GetComponent<SpriteRenderer>();
            if(firstBoid.ValidDistance(other, perceptionDistance)){
                endPoint.Add(other.position);
                otherSprite.color = Color.green;
            }else{
                otherSprite.color = Color.red;

            }
        }
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor= Color.green;
        lineRenderer.startWidth= lineWidth;
        lineRenderer.endWidth= lineWidth;

        lineRenderer.positionCount =  endPoint.Count * 2 + 1;
        lineRenderer.SetPosition(0, boids[0].position);
        for(int i = 0, j = 1; i < endPoint.Count; i++, j+=2){
            lineRenderer.SetPosition(j, endPoint[i]);
            lineRenderer.SetPosition(j+1, boids[0].position);

        }
       


    }

    bool ValidPosition(Vector3 localPosition){
        return localPosition.x > -range.x && localPosition.y > -range.y 
            && localPosition.x < range.x && localPosition.y < range.y;
    }

    Vector3 GetRandomPosition(){
        float x, y, z;
        x = Random.Range(-range.x, range.x);
        y = Random.Range(-range.y, range.y);
        z = 0f;
        return new Vector3(x, y, z);
    }
    
}
