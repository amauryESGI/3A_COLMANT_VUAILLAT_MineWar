using UnityEngine;
using System.Collections;

public class MiniMapScript : MonoBehaviour {
	[SerializeField]
	Camera _mapCamera;

	[SerializeField]
	Camera gameCamera;

	[SerializeField]
	BluidMapScript _map;

	void Start () {
		_mapCamera.orthographicSize = _map.getWidthMap() * _map.getLengthMap() / 10;
	}

	/*
	// 
	private void OnMouseDown(){
		print ("ok");
		gameCamera.transform.position = new Vector3 (0,0,0);
	}
	*/
}