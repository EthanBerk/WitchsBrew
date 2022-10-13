using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Source.Stations
{
    public class IngrdintDisplay : MonoBehaviour
    {
        private BoxCollider2D _boxCollider2D;
        private PotionMaker _potionMaker;
        public GameObject ingrdientPrefab;

        private bool started;
        

        private void Start()
        {
            _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
            _potionMaker = gameObject.GetComponentInParent<PotionMaker>();
            started = true;
        }

        private void OnEnable()
        {
            if (started)
            {
                var interval = _boxCollider2D.size.x / (_potionMaker.Ingredients.Count + 1);
            
                for (var i = 0; i < _potionMaker.Ingredients.Count; i++)
                {
                    var ingredient = _potionMaker.Ingredients[i];
                    var ingredeantClass = Instantiate(ingrdientPrefab, new Vector2(_boxCollider2D.bounds.min.x + interval *
                                (i + 1), transform.position.y),
                            quaternion.identity,
                            transform)
                        .GetComponent<Ingredient>();
                    ingredeantClass.State = ingredient;
                    ingredeantClass.GetComponent<SpriteRenderer>().sortingLayerName = "AboveAbove";
                }
            }
        }

        public void OnDisable()
        {
            var children = gameObject.GetComponentInChildren<Transform>();
            foreach (var child in children)
            {
                var childTr = (Transform) child;
                Destroy(childTr.gameObject);
            }
        }
    }
}