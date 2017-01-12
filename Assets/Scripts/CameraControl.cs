//1
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    public float zoom;
    public int zoomMin;
    public int zoomMax;
    public float zoomSpeed;
    public float cameraRotSpeed;

    public GameObject cameraPivot;

    void Update()
    {
		//Pivots camera based on mouse movement
		if (Input.GetMouseButton(1))
		{
			transform.Rotate(Vector3.up, Input.GetAxisRaw("Mouse X") * cameraRotSpeed * Time.deltaTime, Space.Self);
			cameraPivot.transform.Rotate(-Vector3.right, Input.GetAxisRaw("Mouse Y") * cameraRotSpeed * Time.deltaTime);
		}

		//Zoom camera
		zoom = -Camera.main.transform.localPosition.z;
		if (zoom > zoomMin && zoom < zoomMax)
		{
			if (Camera.main.orthographic)
				Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
			else
			{
				zoom += -(Input.GetAxisRaw("Mouse ScrollWheel")) * zoomSpeed * zoom;
				if (zoom > zoomMin && zoom < zoomMax)
				{
					Camera.main.transform.localPosition = new Vector3(0, 0, -zoom);
				}
			}
		}
    }
}