using UnityEngine;
using System.Collections;

public class BluidMapScript : MonoBehaviour
{
	[SerializeField]
	private float _length;
	
	[SerializeField]
	private float _width;

	void Awake (){
		_length = 20f;
		_width = 20f;
	}

	public float getLengthMap(){
		return _length;
	}

	public float getWidthMap(){
		return _width;
	}
}