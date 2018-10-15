using UnityEngine;

public class CamFollowPlayer : MonoBehaviour {

    //Attach this to the camera and make sure the player is tagged "player"

    [SerializeField] float FollowSpeed = 5f;
    [SerializeField] float ZoomSpeed = 1f;
    [SerializeField] float Offset = 4.5f;
    [SerializeField] float ZoomDelay = 2f;
    float zoomOutDelayCounter;
    float zoomInDelayCounter;
    [SerializeField] float camMinDist = 17f;
    [SerializeField]float camMaxDist = 18f;
    public float CamSpeed;
    bool isMovingUp;
    float lastPos;
  

    Transform _playerPos;



    void Start()
    {
        lastPos = transform.position.y;
        _playerPos = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate ()
    {     
        CamSpeed = (transform.position.y - lastPos) / Time.deltaTime;

        if (transform.position.y >= lastPos)
            isMovingUp = true;
        else
            isMovingUp = false;
            
        lastPos = transform.position.y;

        var targetPos = new Vector3(_playerPos.position.x, _playerPos.position.y + Offset, -10f);
        transform.position -= (transform.position - targetPos) * FollowSpeed * Time.deltaTime;

        var camSize = Camera.main.orthographicSize;

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if(zoomInDelayCounter != 0)
                zoomInDelayCounter = 0f;

            zoomOutDelayCounter += Time.deltaTime;
            
            if (camSize < camMaxDist && zoomOutDelayCounter > ZoomDelay/3)
                Camera.main.orthographicSize = Mathf.Lerp(camSize, camMaxDist, ZoomSpeed * Time.deltaTime);
        }
        else
        {
            if (zoomOutDelayCounter != 0)
                zoomOutDelayCounter = 0f;

                zoomInDelayCounter += Time.deltaTime;

            if (camSize > camMinDist && zoomInDelayCounter > ZoomDelay)
                Camera.main.orthographicSize = Mathf.Lerp(camSize, camMinDist, ZoomSpeed * Time.deltaTime);
        }
    }
}
