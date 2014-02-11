using UnityEngine;
using System.Collections;

public class SetTargetOnClickScript : MonoBehaviour {
	
	static int harvesting;
	
	[SerializeField]
	FollowCheckpointScript _followScript;
	
	[SerializeField]
	SelectScript _SelectedScript;
	
	//a remplacer lors du click
	[SerializeField]
	Transform _playerTargetEmptyObject;
	
	[SerializeField]
	Camera _gameCamera;

	[SerializeField]
	Animator _controller;
	
	[SerializeField]
	NetworkView _networkView;
	
	float _timeEllapsed = 0;	//temps écoulé
	bool _isHarvesting;			//s'il recolte
	
	void Start () {
		_isHarvesting = false;
	}

	void Update () {
		if(Input.GetMouseButtonUp(1) && _SelectedScript.getSelect()){
			var ray = _gameCamera.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit hit;
			int mask = (1 << LayerMask.NameToLayer("Map")) | (1 << LayerMask.NameToLayer("Spawn"));

			if(Physics.Raycast(ray, out hit, float.MaxValue, mask)){
				_controller.SetBool("IsOnClick", true);
				switch(hit.collider.tag){
				case "Map":
					if (_isHarvesting){
						_isHarvesting = false;
					}
					//_playerTargetEmptyObject.position = hit.point;
					//_followScript._objectToFollow = _playerTargetEmptyObject;
					_networkView.RPC("SetTarget", RPCMode.All, hit.point);
					_controller.SetBool("IsOnClick", true);
					break;

				case "Spawn":
					if (!_isHarvesting){
						_isHarvesting = true;
					}
					//_playerTargetEmptyObject.position = hit.collider.transform.position;
					//_followScript._objectToFollow = _playerTargetEmptyObject;
					_networkView.RPC("SetTarget", RPCMode.All,  hit.collider.transform.position);
					break;

				default:
					break;
				}
			}
		}
		
		if(_controller.GetBool("IsOnClick")){
			_timeEllapsed += 1 * Time.deltaTime;
			if(_timeEllapsed >= 0.5){
				_controller.SetBool("IsOnClick", false);
				_timeEllapsed = 0;
			}
		}
	}
	
	#region Assesseur function
	public bool getIsRecoltable(){
		return _isHarvesting;
	}
	#endregion
	
	#region Mutateur function
	public void setIsRecoltable(bool isHarvesting){
		_isHarvesting = isHarvesting;
	}
	
	[RPC]
	void SetTarget(Vector3 positionToFollow){
		_followScript._objectToFollow.position = positionToFollow;
	}
	#endregion
}