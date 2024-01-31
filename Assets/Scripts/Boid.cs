using UnityEngine;

public class Boid : MonoBehaviour
{
    Vector3 targetDirection = Vector3.zero;
    Vector3 targetDirection_M = Vector3.zero; 
    Vector3 targetDirection_C = Vector3.zero; 
    Vector3 targetDirection_A = Vector3.zero;

    [SerializeField]
    float rotationSpeed;

    [SerializeField, Range(0, 180)]
    float angleOfView;

    [SerializeField]
    float separationRadi;

    [SerializeField]
    Transform centroidPrefab;
    Transform centroidT = null;

    public void MatchDirection(Transform[] boids, float perceptionDistance){
        Vector3 towards = transform.up;
        int valid = 0;
        for(int i = 0; i < boids.Length; i++){
            Transform other = boids[i];
            if(transform != other && ValidDistance(other, perceptionDistance)){
                towards += other.transform.up;
                valid++;
            }
        }
        if(valid > 0)
            targetDirection_M = (towards / valid).normalized;

    }

    public void FindCentroid(Transform[] boids, float perceptionDistance){
        Vector3 centroid = transform.position;
        int valid = 1;
        for(int i = 0; i < boids.Length; i++){
            Transform other = boids[i];
            if(transform != other && ValidDistance(other, perceptionDistance)){
                centroid += other.position;
                valid++;
            }
        }
        centroid /= valid;
        if(boids[0] == transform){
            if(centroidT == null){
                centroidT = Instantiate(
                    centroidPrefab,
                    centroid,
                    Quaternion.identity
                );
            }
            centroidT.position = centroid;
        }
        
        targetDirection_C = (centroid - transform.position).normalized;


    }
    public void AvoidCollision(Transform[] boids, float perceptionDistance){
        Vector3 direction = Vector3.zero;
        for(int i = 0; i < boids.Length; i++){
            Transform other = boids[i];
            if(transform != other && ValidDistance(other, perceptionDistance)){
                float magnitude =  Mathf.Clamp01((other.position - transform.position).magnitude / separationRadi);
                direction -= magnitude * (other.position - transform.position);
                
            }
        }
        targetDirection_A = direction.normalized;

    }

    public bool ValidDistance(Transform other, float perceptionDistance){
        float d = Vector3.Distance(transform.position, other.position);
        return transform != other && d <= perceptionDistance && ValidVision(other);

    }

    public bool ValidVision(Transform other){
        return true;//Vector3.Angle(transform.up, other.position) < angleOfView;
    }

    public void Move(float speed){
        targetDirection = Vector3.Normalize(targetDirection_C + targetDirection_M + targetDirection_A);
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
