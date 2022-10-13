using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    // Start is called before the first frame update
    public IngredientState State;
    private Animator _animator;
    private bool isDelete;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    private bool playing;

    private void Update()
    {
        if (isDelete)
        {
            if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){  //If normalizedTime is 0 to 1 means animation is playing, if greater than 1 means finished
                if (playing)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                playing = true;
            }
            
        }
        _animator.SetFloat("Blend", (int)State);
    }

    public void delete()
    {
        _animator.SetBool("Drop", true);
        isDelete = true;

    }

    public enum IngredientState
    {
        Red,
        Blue,
        Yellow,
        Green
    }
}
