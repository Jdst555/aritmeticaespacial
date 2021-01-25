using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera camera;
    public float parallax = 0.5f;
    private float textureUnits;
    private Texture texture;
    private Sprite sprite;
    private float pixelPerUnit;
    private Vector3 cameraPosition = new Vector3();
    private int jumpFactor = 2;

    void Start()
    {
        camera = Camera.main;
        if (GetComponent<SpriteRenderer>() != null)
        {
            sprite = GetComponent<SpriteRenderer>().sprite;
            texture = sprite.texture;
            pixelPerUnit = sprite.pixelsPerUnit;
            textureUnits = texture.width / pixelPerUnit;
        }
        
       
        
        
    }

    private void Update()
    {
        
    }


    private void LateUpdate()
    {
       
        Vector3 cameraDeltaPosition = camera.GetComponent<Follow>().deltaPosition;
        transform.Translate(cameraDeltaPosition * parallax);

        //saltar BG en eje X y Y
        cameraPosition = camera.transform.position;
        float deltaX = cameraPosition.x - transform.position.x;
        float deltaY = cameraPosition.y - transform.position.y;
        if (deltaX >= textureUnits * jumpFactor)
        {
            transform.Translate(new Vector3(textureUnits * jumpFactor, 0));

        }
        if (deltaX <= -textureUnits * jumpFactor)
        {
            transform.Translate(new Vector3(-textureUnits * jumpFactor, 0));

        }
        if (deltaY >= textureUnits * jumpFactor)
        {
            transform.Translate(new Vector3(0, textureUnits * jumpFactor));
        }
        if (deltaY <= -textureUnits * jumpFactor)
        {
            transform.Translate(new Vector3(0, -textureUnits * jumpFactor));
        }
    }
    
}
