using UnityEngine;
using System.Collections;

public class NetworkScript : MonoBehaviour {
	[SerializeField]
	private bool _isServer;

	void Start () {
		Application.runInBackground = true;
		
		if (_isServer)
		{
			Network.InitializeServer(2, 6600, true);
		}
		else
		{
			Network.Connect("127.0.0.1", 6600);
		}
	}

	public bool getIsServer(){
		return _isServer;
	}
}
