using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Boid : MonoBehaviour
{

    Transform[] inRange;
    public void DirectionMatching(Transform[] boids, float perceptionDistance){
        foreach(Transform other in boids){
            if(transform != other && 
            Vector3.Distance(transform.localPosition, other.localPosition) <= perceptionDistance){
            }
        }
    }

    public void Move(float speed){
        transform.Translate(Vector3.up * Time.deltaTime * speed); 
    }
   
}
