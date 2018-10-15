using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDeath : MonoBehaviour {

    SpriteRenderer _sprite;
    [SerializeField] Sprite _healthyTree;
    [SerializeField] Sprite _deadTree;
	// Use this for initialization
	void Start () {
        _sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneMgr.Instance._animalsInWorld < 10)
        {
            if (_sprite.sprite != _deadTree)
                _sprite.sprite = _deadTree;
        }
        else if (SceneMgr.Instance._animalsInWorld > 15)
        {
            if(_sprite.sprite != _healthyTree)
            _sprite.sprite = _healthyTree;
        }

    }
}
