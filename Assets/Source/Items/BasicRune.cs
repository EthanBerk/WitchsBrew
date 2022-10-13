using System;
using Player;
using UnityEngine;

namespace Source.Items
{
    public class BasicRune : MonoBehaviour
    {
        public runeState State;
        private Animator _animator;

        public bool inract;

        private void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool("Select", inract);
            
        }


        public void PickUp()
        {
            gameObject.SetActive(false);
        }

        public void SetDown(Vector2 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }

        public enum runeState
        {
            doubleJump,
            highJump,
            wallJump,
            dash,
            none
        }
    }
}