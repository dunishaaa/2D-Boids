using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class Boid : MonoBehaviour
{
    Vector3 targetDirection = Vector3.zero;
    Vector3 targetDirection_M = Vector3.zero; 
    Vector3 targetDirection_C = Vector3.zero; 
    Vector3 targetDirection_A = Vector3.zero;

    [SerializeField]

    float rotationSpeed;

    [SerializeField]
    float separationRadi;

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
            targetDirection_M = Vector3.Normalize(towards / valid);

    }

    public void CenterOfFlock(Transform[] boids, float perceptionDistance){
        Vector3 centroid = transform.position;
        int valid = 1;
        for(int i = 0; i < boids.Length; i++){
            Transform other = boids[i];
            if(ValidDistance(other, perceptionDistance)){
                centroid += other.position;
                valid++;
            }
        }
        targetDirection_C = Vector3.Normalize(centroid/valid);

    }
    public void AvoidCollision(Transform[] boids, float perceptionDistance){
        Vector3 direction = Vector3.zero;
        for(int i = 0; i < boids.Length; i++){
            Transform other = boids[i];
            if(ValidDistance(other, perceptionDistance)){
                float magnitude =  Mathf.Clamp01((other.position - transform.position).magnitude / separationRadi);
                direction -= magnitude * (other.position - transform.position);
                
            }
        }
        targetDirection_A = direction.normalized;

    }

    public bool ValidDistance(Transform other, float perceptionDistance){
        float d = Vector3.Distance(transform.position, other.position);
        return transform != other && d <= perceptionDistance;

    }

    public void Move(float speed){
        targetDirection = targetDirection_C + targetDirection_M + targetDirection_A;
        Vector3 dir = targetDirection - transform.position; 
        
        Vector3 currentDirection = transform.up;
        Vector3 newDirection = Vector3.RotateTowards(
            currentDirection,
            targetDirection,
            rotationSpeed * Time.deltaTime, 0.0f);

        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
//        Quaternion rot = Quaternion.LookRotation(newDirection);
        rot.x = 0f;
        rot.y = 0f;
        transform.rotation = rot;
        newDirection.z = 0f;
        transform.up = newDirection;
        transform.position += speed * Time.deltaTime * transform.up;
    }
   
}
