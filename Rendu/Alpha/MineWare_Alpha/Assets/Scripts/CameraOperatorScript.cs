using UnityEngine;
using System.Collections;

public class CameraOperatorScript : MonoBehaviour {
	#region structs
	//box limits Struct
	public struct BoxLimit {
		public float Left;
		public float Right;
		public float Top;
		public float Bottom;
	}
	#endregion

	[SerializeField]
	Transform _myTransform;
	
	[SerializeField]
	Transform _TransformHero;

	[SerializeField]
	BluidMapScript _map;
	
	// gestion select
	public Texture2D SelectionHighlight = null;
	private Rect _selection = new Rect(0,0,0,0);
	private Vector3 startClick = -Vector3.one;
	
	// gestion zoom
	[SerializeField]
	private float zoomLimit = 10;
	
	[SerializeField]
	private float cameraZoomSpeed = 10f;
	
	[SerializeField]
	private float shiftBonusZoomSpeed = 15f;
	
	// gestion Move
	private static BoxLimit cameraMoveLimits = new BoxLimit();
	private static BoxLimit mouseMoveLimits  = new BoxLimit();
	
	[SerializeField]
	private float cameraMoveSpeed = 30f;
	
	[SerializeField]
	private float shiftBonusMoveSpeed = 45f;
	
	[SerializeField]
	private float mouseBoundary = 25f;
	
	void Start () {
		// Positionne la camera sur le hero
		_myTransform.position = new Vector3(_TransformHero.position.x, 15f, _TransformHero.position.z-15f);

		//Declare camera limits
		cameraMoveLimits.Right  = _map.getLengthMap()/2f;
		cameraMoveLimits.Left   = -_map.getLengthMap()/2f;
		cameraMoveLimits.Top    = (_map.getWidthMap()/2f)-15f;
		cameraMoveLimits.Bottom = (-_map.getWidthMap()/2f)-15f;
		
		//Declare Mouse Scroll Limits
		mouseMoveLimits.Left   = mouseBoundary;
		mouseMoveLimits.Right  = mouseBoundary;
		mouseMoveLimits.Top    = mouseBoundary;
		mouseMoveLimits.Bottom = mouseBoundary;
	}
	
	void Update () {
		checkSelection ();
		zoomCamera();
		translationCamera();
	}
	
	#region Select function
	private void checkSelection () {
		if (Input.GetMouseButtonDown (0))
			startClick = Input.mousePosition;
		else if(Input.GetMouseButtonUp(0))
			startClick = - Vector3.one;
		
		if (Input.GetMouseButton (0)) {
			_selection = new Rect (startClick.x,
			                       InvertMouseY (startClick.y),
			                       Input.mousePosition.x - startClick.x,
			                       InvertMouseY (Input.mousePosition.y) - InvertMouseY (startClick.y));
			
			// permet de sortir la valeur absolue de la selection en largeur
			if(_selection.width < 0){
				_selection.x += _selection.width;
				_selection.width = - _selection.width;
			}
			// permet de sortir la valeur absolue de la selection en hauteur
			if(_selection.height < 0){
				_selection.y += _selection.height;
				_selection.height = - _selection.height;
			}
		}
	}
	
	// Permet d'afficher le carrer de selection
	private void OnGUI(){
		if (startClick != -Vector3.one) {
			GUI.color = new Color (1, 1, 1, 0.2f);
			GUI.DrawTexture (_selection, SelectionHighlight);
		}
	}
	#endregion
	
	#region Zoom Camera function
	private void zoomCamera(){
		float zoomY = 0f;
		float zoomZ = 0f;
		float zoomSpeed = 0f;
		
		//Declare Zoom limits
		float zoomMaxY = zoomLimit + 15;
		float zoomMinY = 15; 

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		
		// Permet de modifier la vitesse de déplacement
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			zoomSpeed = (cameraZoomSpeed + shiftBonusZoomSpeed) * Time.deltaTime;
		else
			zoomSpeed = cameraZoomSpeed * Time.deltaTime;

		zoomY = (-scroll * zoomSpeed) / 0.71f;
		zoomZ = (scroll * zoomSpeed) / 0.71f;

		// Check Limit
		if(
			((_myTransform.position.y + zoomY) <= zoomMaxY)&&
			((_myTransform.position.y + zoomY) >= zoomMinY)
			)
			_myTransform.position += new Vector3(0, zoomY, zoomZ);
	}
	#endregion
	
	#region Move Camera function	
	// Check si l'utilisateur utilise une des commandes pour déplacer la caméra.
	private bool CheckIfCameraCanMove()
	{
		bool keyboardMove;
		bool mouseMove;
		
		// check keyboard		
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)){
			keyboardMove = true;			
		} else {
			keyboardMove = false;
		}
		
		// check mouse position
		if(
			(Input.mousePosition.x < mouseMoveLimits.Left && Input.mousePosition.x > -5) ||
			(Input.mousePosition.x > (Screen.width - mouseMoveLimits.Right) && Input.mousePosition.x < (Screen.width + 5)) ||
			(Input.mousePosition.y < mouseMoveLimits.Bottom && Input.mousePosition.y > -5) ||
			(Input.mousePosition.y > (Screen.height - mouseMoveLimits.Top) && Input.mousePosition.y < (Screen.height + 5))
			)
			mouseMove = true;
		else
			mouseMove = false;
		
		if(keyboardMove || mouseMove)
			return true;
		else
			return false;
	}
	
	// Fonction qui permet le déplacement de la caméra
	private void translationCamera()
	{
		if(CheckIfCameraCanMove()){
			float moveSpeed = 0f;
			float desiredX = 0f;
			float desiredZ = 0f;
			
			// Permet de modifier la vitesse de déplacement
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				moveSpeed = (cameraMoveSpeed + shiftBonusMoveSpeed) * Time.deltaTime;
			else
				moveSpeed = cameraMoveSpeed * Time.deltaTime;
			
			// Move la camera via keyboard
			if(Input.GetKey(KeyCode.Z))
				desiredZ = moveSpeed;
			if(Input.GetKey(KeyCode.S))
				desiredZ = moveSpeed * -1;
			if(Input.GetKey(KeyCode.D))
				desiredX = moveSpeed;
			if(Input.GetKey(KeyCode.Q))
				desiredX = moveSpeed * -1;
			
			// Move la camera via mouse
			if(Input.mousePosition.x < mouseMoveLimits.Left)
				desiredX = moveSpeed * -1;
			if(Input.mousePosition.x > (Screen.width - mouseMoveLimits.Right))
				desiredX = moveSpeed;
			if(Input.mousePosition.y < mouseMoveLimits.Bottom)
				desiredZ = moveSpeed * -1;
			if(Input.mousePosition.y > (Screen.height - mouseMoveLimits.Top))
				desiredZ = moveSpeed;
			
			// Check Limit
			if(
				((_myTransform.position.x + desiredX) > cameraMoveLimits.Left)&&
				((_myTransform.position.x + desiredX) < cameraMoveLimits.Right)
				)				
				_myTransform.position += new Vector3(desiredX, 0, 0);
			if(
				((_myTransform.position.z + desiredZ) < cameraMoveLimits.Top)&&
				((_myTransform.position.z + desiredZ) > cameraMoveLimits.Bottom)
				)
				_myTransform.position += new Vector3(0, 0, desiredZ);
		}
	}
	#endregion
	
	#region Helper function
	// Fonction permetant de positionner le centre en haut à gauche
	public float InvertMouseY(float y){
		return Screen.height - y;
	}
	#endregion
	
	#region Mutateur function	
	public bool setSelectionContain(Vector3 val){
		return _selection.Contains(val);
	}
	#endregion
}
