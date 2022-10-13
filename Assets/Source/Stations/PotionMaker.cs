using System;
using System.Collections.Generic;
using System.Linq;
using Source.Items;

using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Stations
{
    public class PotionMaker : MonoBehaviour
    {
        
        public GameObject PotionPrefab;
        public List<Ingredient.IngredientState> Ingredients { get; private set; }
        public int MaxIngredients;
        public int MinIngredients;
        private Bounds _bounds;
        public LayerMask _layerMask;
        private IngrdintDisplay _ingrdintDisplay;
        private Animator _animator;
        public Potion.PotionState PotionState;
        public bool HasPotion = true;

        public AudioClip LongBubble;
        public AudioClip ShortBubble;
        private AudioSource _audioSource;




        private void Update()
        {
            if (Ingredients.Any())
            {
                var hit =
                    Physics2D.OverlapBox(transform.position, _bounds.size, 0, _layerMask);
                if (hit)
                {
                    _ingrdintDisplay.gameObject.SetActive(true);
                }
                else
                {
                    _ingrdintDisplay.gameObject.SetActive(false);
                }
                
            }
            else 
            {
                _ingrdintDisplay.gameObject.SetActive(false);
                _animator.SetBool("can", true);
                transform.tag = "B";
            }


        }

        private void Start()
        {
            _ingrdintDisplay = gameObject.GetComponentInChildren<IngrdintDisplay>();
            _bounds = gameObject.GetComponent<BoxCollider2D>().bounds;
            _animator = gameObject.GetComponent<Animator>();
            _audioSource = gameObject.GetComponent<AudioSource>();
            Ingredients = RandomiseIngrediants();

        }


        public void AddIngredient(Ingredient ingredient)
        {
            _audioSource.PlayOneShot(ShortBubble);
            Ingredients.Remove(ingredient.State);
            _ingrdintDisplay.gameObject.SetActive(false);
            _ingrdintDisplay.gameObject.SetActive(true);
        }

        public bool canAddIngredient(Ingredient ingredient)
        {
            return Ingredients.Contains(ingredient.State);
        }

        public Potion makePotion()
        {
            _audioSource.PlayOneShot(LongBubble);

            var potion = Instantiate(PotionPrefab, transform.position, Quaternion.identity).GetComponent<Potion>();
            potion.ghost = false;
            potion.State = PotionState;
            transform.tag = "I";
            Ingredients = RandomiseIngrediants();
            HasPotion = false;
            _animator.SetBool("can", false);
            return potion;
        }

        public List<Ingredient.IngredientState> RandomiseIngrediants()
        {
            var numOfIngredients = Random.Range(MinIngredients, MaxIngredients);
            var ingredients = new List<Ingredient.IngredientState>();
            var ingreientsArray = Enum.GetValues(typeof(Ingredient.IngredientState)).Cast<Ingredient.IngredientState>();
            for (int i = 1; i <= numOfIngredients; i++)
            {
                ingredients.Add((Ingredient.IngredientState)Random.Range(0, ingreientsArray.Count()));
            }

            return ingredients;
        }
    }
}