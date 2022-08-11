using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TactileMovement : MonoBehaviour
{
    private Camera m_currentCamera;
    private Rigidbody m_rigidbody;
    private Vector3 m_screenPoint;
    private Vector3 m_offset;
    private Vector3 m_currentVelocity;
    private Vector3 m_previousPos;

    //public TextMesh textMesh;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        m_currentCamera = FindCamera();
        if (m_currentCamera != null)
        {
            m_screenPoint = m_currentCamera.WorldToScreenPoint(gameObject.transform.position);
            m_offset = gameObject.transform.position - m_currentCamera.ScreenToWorldPoint(GetMousePosWithScreenZ(m_screenPoint.z));
        }
    }

    void OnMouseUp()
    {
        //textMesh.text = "Up. Fingers = " + Input.touchCount.ToString(); //No entra ni aqu� cuando suelto uno de los dos dedos, y al soltar el segundo ya entra directamente abajo
        if (Input.touchCount == 0) //solo si se ha soltado del todo
        {
            //textMesh.text = "UP!!. Fingers = " + Input.touchCount.ToString();
            m_rigidbody.velocity = m_currentVelocity;
            m_currentCamera = null;
        }
    }

    void FixedUpdate()
    {
        if (m_currentCamera != null)
        {
            Vector3 currentScreenPoint = GetMousePosWithScreenZ(m_screenPoint.z);
            m_rigidbody.velocity = Vector3.zero;
            m_rigidbody.MovePosition(m_currentCamera.ScreenToWorldPoint(currentScreenPoint) + m_offset);
            m_currentVelocity = (transform.position - m_previousPos) / Time.deltaTime;
            /*if (Input.touchCount == 2) //Parte nueva: mover m�s lejos o m�s cerca
            {
                //textMesh.text = "Two fingers detected";
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;
                //textMesh.text = "Two fingers detected. Difference: " + difference.ToString();

                transform.position += m_currentCamera.transform.forward * difference * 0.005f;
            }
            //Funciona, pero al soltar el segundo dedo se resetea a la posicion del primero, y no consigo arreglarlo (se pegan ambos m�todos de movimiento).
             */
            m_previousPos = transform.position;
        }
    }

    Vector3 GetMousePosWithScreenZ(float screenZ)
    {
        return new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenZ);
    }

    Camera FindCamera()
    {
        Camera[] cameras = FindObjectsOfType<Camera>();
        Camera result = null;
        int camerasSum = 0;
        foreach (var camera in cameras)
        {
            if (camera.enabled)
            {
                result = camera;
                camerasSum++;
            }
        }
        if (camerasSum > 1)
        {
            result = null;
        }
        return result;
    }

}
