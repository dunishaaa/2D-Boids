using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Boid : MonoBehaviour
{
    Vector3 targetDirection;

    [SerializeField]

    float rotationSpeed;


    public void MatchDirection(Transform[] boids, float perceptionDistance){
        Vector3 towards = transform.up;
        Vector3 towards_ = Vector3.zero;
        int valid = 0;
        for(int i = 0; i < boids.Length; i++){
            Transform other = boids[i];
            if(transform != other && ValidDistance(other, perceptionDistance)){
                towards += other.transform.up;
                towards_ += other.transform.up;
                valid++;
            }
        }
        if(valid > 0)
            targetDirection = Vector3.Normalize(towards_ / valid);

    }

    public bool ValidDistance(Transform other, float perceptionDistance){
        float d = Vector3.Distance(transform.position, other.position);
        return transform != other && d <= perceptionDistance;

    }

    public void Move(float speed){
        Vector3 currentDirection = transform.up;
        Vector3 newDirection = Vector3.RotateTowards(
            currentDirection,
            targetDirection,
            rotationSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        transform.up = newDirection;
        transform.position += speed * Time.deltaTime * transform.up;
    }
   
}
