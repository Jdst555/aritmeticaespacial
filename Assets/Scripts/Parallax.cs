using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Controla velocidad de paralaje de las diferentes capas del fondo
public class Parallax : MonoBehaviour
{
    
    
    public float parallax = 0.5f;
    private float textureUnits;
    private Texture texture;
    private Sprite sprite;
    private float pixelPerUnit;
    private Vector3 cameraPosition = new Vector3();
    private int jumpFactor = 2;

    void Start()
    {
        
        if (GetComponent<SpriteRenderer>() != null)
        {
            sprite = GetComponent<SpriteRenderer>().sprite;//el SpriteRenderer asociado a este objeto
            texture = sprite.texture;//la textura del Sprite
            pixelPerUnit = sprite.pixelsPerUnit;//cuantos pixeles de hacen una unidad
            textureUnits = texture.width / pixelPerUnit;//cuantas unidades tiene la textura (ancho y alto son iguales)
        }
    }

    private void LateUpdate()
    {
       
        Vector3 cameraDeltaPosition = Camera.main.GetComponent<Follow>().deltaPosition;
        transform.Translate(cameraDeltaPosition * parallax);

        //saltar BG en eje X y Y
        cameraPosition = Camera.main.transform.position;
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
