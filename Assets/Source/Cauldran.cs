using System;
using System.Collections;
using System.Collections.Generic;
using Source.Items;
using UnityEngine;

public class Cauldran : MonoBehaviour
{
    // Start is called before the first frame update
    public Ingredient.IngredientState State = Ingredient.IngredientState.Blue;
    public GameObject IngredientPrefab;
    private Animator _animator;
    private AudioSource _audioSource;
    public AudioClip _audioClip;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        _animator.SetFloat("Blend", (int)State);
        
    }

    public Ingredient giveIngredient()
    {
        var Ingredient = Instantiate(IngredientPrefab, Vector3.zero, Quaternion.identity);
        var IngredientClass = Ingredient.GetComponent<Ingredient>();
        _audioSource.PlayOneShot(_audioClip);
        
        IngredientClass.State = State;
        return IngredientClass;
    }
    
}
