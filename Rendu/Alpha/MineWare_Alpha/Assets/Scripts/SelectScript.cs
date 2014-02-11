using UnityEngine;
using System.Collections;

public class SelectScript : MonoBehaviour {
	
	[SerializeField]
	Transform _myTransform;
	
	[SerializeField]
	Renderer _renderer;
	
	[SerializeField]
	Light _pointLight;
	
	[SerializeField]
	Camera _gameCamera;

	[SerializeField]
	CameraOperatorScript _cameraScript;
	
	[SerializeField]
	bool selected = false;
	
	[SerializeField]
	public bool selectedByClick = false;

	void Update () {
		if (_renderer.isVisible && Input.GetMouseButton(0)){
			if(!selectedByClick){
				Vector3 camPos = _gameCamera.WorldToScreenPoint(_myTransform.position);
				camPos.y = _cameraScript.InvertMouseY(camPos.y);
				selected = _cameraScript.setSelectionContain(camPos);
			}
			
			if (selected)
				_pointLight.intensity = 4f;
			else
				_pointLight.intensity = 1.5f;
		}
	}
	
	// Active la selection
	private void OnMouseDown(){
		selectedByClick = true;
		selected = true;
	}

	// Fonction permetant de selectionner avec un seul click
	private void OnMouseUp(){
		if (selectedByClick)
			selected = true;

		selectedByClick = false;
	}

	// Retourne si l'objet est selectionné
	public bool getSelect(){
		return selected;
	}
}