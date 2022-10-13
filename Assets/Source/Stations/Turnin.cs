using System;
using System.Collections;
using System.Collections.Generic;
using Source.Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Stations
{
    public class Turnin : MonoBehaviour
    {
        public int potionNeedMAx;
        public int potionNeedMin;
        private int potionNeed;
        private int potionNeeded;
        private BoxCollider2D _boxCollider2D;
        public GameObject PotionPrefab;
        public GameObject GameManger;
        private List<Potion> _potions;
        private List<Potion> _potionsToBeDeleted;
        
        private LevelManger _levelManger;

        private Animator grabberAnimator;


        private void Start()
        {
            _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
            _levelManger = GameManger.GetComponent<LevelManger>();
            grabberAnimator = GetComponentInChildren<Animator>();
            setPotions();


        }

        private bool deleting;
        private bool playing;

        private void Update()
        {
            if (potionNeed == 0)
            {
                if (deleting)
                {
                    if(grabberAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5){  //If normalizedTime is 0 to 1 means animation is playing, if greater than 1 means finished
                        if (playing)
                        {
                            foreach (var potion in _potions)
                            {
                                Destroy(potion.gameObject);
                            }

                            _potions = new List<Potion>();
                            grabberAnimator.SetBool("TurnedIn", false);
                            deleting = false;
                            playing = false;
                            setPotions();
                        }
                    }
                    else
                    {
                        playing = true;
                    }
                    

                }
                else
                {
                    _levelManger.totalAmount += potionNeeded;
                    deleting = true;
                    grabberAnimator.SetBool("TurnedIn", true);

                }
                
                
                
                
                
            }
        }

        

        private void setPotions()
        {
            potionNeed = Random.Range(potionNeedMin, potionNeedMAx + 1);
            potionNeeded = potionNeed;
            _potions = new List<Potion>();
            var interval = _boxCollider2D.size.x / (potionNeed);
            
            for (int i = 0; i < potionNeed; i++)
            { 
                var potion = Instantiate(PotionPrefab, new Vector2(_boxCollider2D.bounds.min.x + (interval) * (i +1), _boxCollider2D.bounds.max.y + 0.125f), Quaternion.identity, transform).GetComponent<Potion>();
                potion.ghost = true;
                _potions.Add(potion);
            }
        }

        public void AddPotion(Potion potion)
        {
            potionNeed--;
            _potions[potionNeed].State = potion.State;
            _potions[potionNeed].ghost = false;
            

        }
        
    }
}