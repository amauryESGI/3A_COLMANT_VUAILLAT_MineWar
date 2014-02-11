using UnityEngine;
using System.Collections;

public class FollowCheckpointScript : MonoBehaviour {
	
//	[SerializeField]
//	Animator _controller;
	
	[SerializeField]
	public Transform _objectToFollow;
	
	[SerializeField]
	Rigidbody _rigidBody;
	
	[SerializeField]
	float _minReachedDistance;
	
	[SerializeField]
	Transform _myTransform;

	[SerializeField]
	float _velocity = 5f;
	
	void FixedUpdate(){
		if(_objectToFollow!= null){
			//_objectToFollow.position = positionToFollow;

			var direction = _objectToFollow.position - _myTransform.position;
			direction = new Vector3(direction.x,0,direction.z);
			if(direction.sqrMagnitude < _minReachedDistance){
				//_objectToFollow = null;
				_rigidBody.velocity= Vector3.zero;
				//_controller.SetFloat("Speed",0f);
			}else{
				var normalizedDirection = direction.normalized;
				_myTransform.LookAt(_objectToFollow);
				_rigidBody.velocity = normalizedDirection * _velocity;
				//_rigidBody.velocity = direction * _velocity;
				//_myTransform.Translate(Vector3.forward * _velocity * Time.deltaTime);
				//_controller.SetFloat("Speed",_velocity);
			}
		}
	}

	//creer un materiaux physique ->frictions à 0 le mettre sur tous les colliders
	//definir un layer pour la carte "Map"
}
