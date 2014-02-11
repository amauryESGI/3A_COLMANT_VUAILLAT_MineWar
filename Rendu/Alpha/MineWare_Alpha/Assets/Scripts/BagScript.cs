using UnityEngine;
using System.Collections;

public class BagScript : MonoBehaviour {
	int _nbFer;
	// Ajouter dictionnaire des items possédé
	// Ajouter enume des items existant

	public bool addItem(int value, string item){
		_nbFer += value;

		return true;
		// Ajouter item dans le dictionnaire
	}

	public int getNbItem(string item){
		return _nbFer;
	}
}
