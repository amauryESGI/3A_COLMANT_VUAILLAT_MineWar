using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AeraDetectionScript : MonoBehaviour {
	[SerializeField]
	Animator _controller;

	float _timeEllapsed; //temps écoulé
	List<GameObject> listUnite;

	void Start (){
		listUnite = new List<GameObject>();
		_timeEllapsed = 0;
	}

	void Update (){
		int nbUnite = _controller.GetInteger ("nbUnite");
		if (nbUnite>0){
			_timeEllapsed += 1 * Time.deltaTime;
			if(_timeEllapsed >= 1){
				if (nbUnite<5)
					_controller.SetInteger ("nbHarvest", _controller.GetInteger ("nbHarvest") - 5*nbUnite);
				else
					_controller.SetInteger ("nbHarvest", _controller.GetInteger ("nbHarvest") - 5*5);
				_timeEllapsed = 0;

				foreach (GameObject go in listUnite){
					BagScript Bag = go.GetComponent<BagScript>();
					Bag.addItem(5*nbUnite, "Fer");
					print (Bag.getNbItem("Fer"));
				}
			}

			if(_controller.GetInteger ("nbHarvest")<0){
				_controller.SetInteger ("nbHarvest", 100);
				// Bug Animation
				_controller.SetInteger ("nbUnite", 0);

				// Supprimer les element de la list
			}
		}
		// Bug Non retour à 100% après animator : HarvestEtat => PopAnimation
	}

	void OnTriggerEnter(Collider col){
		SetTargetOnClickScript areaDetection = col.transform.parent.gameObject.GetComponent<SetTargetOnClickScript>();
		if (areaDetection.getIsRecoltable ()) {
			_controller.SetInteger ("nbUnite", _controller.GetInteger ("nbUnite") + 1);
			listUnite.Add(col.transform.parent.gameObject);
		}
	}

	void OnTriggerExit(Collider col){
		if (_controller.GetInteger ("nbUnite") > 0) {
			_controller.SetInteger ("nbUnite", _controller.GetInteger ("nbUnite") - 1);
			listUnite.Remove (col.transform.parent.gameObject);
		}
	}
}