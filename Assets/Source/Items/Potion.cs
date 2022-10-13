using System;
using UnityEngine;

namespace Source.Items
{
    public class Potion : MonoBehaviour
    {
        public PotionState State;
        private Animator _animator;
        public bool ghost { get; set; } = true;

        private void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
            _animator.SetBool("ghost", ghost);
        }

        private void Update()
        {
            _animator.SetFloat("Blend", (int)State);
            _animator.SetBool("ghost", ghost);
        }
        public enum PotionState
        {
            potion,
            potion1,
            potion2,
            potion3
        }

        public void test()
        {
            Update();
        }
    }
}