using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Source.Items;
using Source.Stations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    [RequireComponent(typeof(BoxCollisionController2D))]
    public class PlayerScript : MonoBehaviour
    {
        public int maxIngredents = 5;
        private Vector3 _velocity;
        public BoxCollisionController2D BoxCollisionController2D { get; private set; }

        private Animator _animator;

        private SpriteRenderer runeRenderer;

        // Start is called before the first frame update
        private void Start()
        {
            Time.timeScale = 1;
            runeSignAnimator = runeSign.GetComponent<Animator>();
            _animator = GetComponent<Animator>();
            _gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            _jumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
            _highgravity = -(2 * highJumpHeight) / Mathf.Pow(timeToHighJumpApex, 2);
            _highJumpVelocity = Mathf.Abs(_highgravity) * timeToHighJumpApex;
            BoxCollisionController2D = gameObject.GetComponent<BoxCollisionController2D>();
            runeRenderer = runeSign.GetComponent<SpriteRenderer>();
        }
        

        // Update is called once per frame
        public LayerMask collectible;
        public LayerMask RuneLayerMask;

        public GameObject runeSign;
        private Animator runeSignAnimator;

        public float moveSpeed;
        public float highSpeed;

        public float jumpHeight = 4; 
        public float timeToJumpApex = .4f;
        
        public float timeToHighJumpApex = .4f;
        public float highJumpHeight = 4;

        private float health;
        public bool canWallJump;

        public Vector2 wallJumpAganst;
        public Vector2 wallJumpOff;
        public Vector2 wallJumpLeap;
        private float _wallSlideSpeedMax = 3;
        public float wallStickTime = .25f;
        private float _timeToWallUnstick;
        

        public BasicRune.runeState RuneState = BasicRune.runeState.none;
        
        


        public float accelerationTimeAirborne = .2f;
        public float accelerationTimeGrounded = .1f;

        private float _gravity;
        private float _highgravity;
        private float _jumpVelocity;
        private float _highJumpVelocity;
        public bool WallSliding { get; private set; }


        private float _smoothDampVelocity;

        
        
        private bool secondJump;
        public float dashWait = 0.3f;

        private bool dashing;
        private bool canDash = true;

        private IEnumerator setCanDash()
        {
            yield return new WaitForSeconds(dashWait);
            canDash = true;
        }

        private void Update()
        {
            Runes();

            if (RuneState == BasicRune.runeState.none)
            {
                runeRenderer.gameObject.SetActive(false);
            }
            else
            {
                runeRenderer.gameObject.SetActive(true);
                runeSignAnimator.SetFloat("Blend", (int)RuneState);
                
            }
            

            if (RuneState == BasicRune.runeState.dash)
            {
                runeRenderer.color = canDash ? Color.white : Color.gray;
            }
            else if (RuneState == BasicRune.runeState.doubleJump)
            {
                runeRenderer.color = secondJump || BoxCollisionController2D.Collisions.Below? Color.white : Color.gray;
            }
            else
            {
                runeRenderer.color = Color.white;
            }
            
            
            
            Interact();


            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var targetVelocity = input.x * moveSpeed;
            WallSliding = false;

            _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocity, ref _smoothDampVelocity,
                (BoxCollisionController2D.Collisions.Below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            var wallDirectionX = (BoxCollisionController2D.Collisions.Left) ? -1 : 1;
            if (RuneState == BasicRune.runeState.wallJump)
            {
                if ((BoxCollisionController2D.Collisions.Left || BoxCollisionController2D.Collisions.Right) &&
                    !BoxCollisionController2D.Collisions.Below &&
                    _velocity.y < 0)
                {
                    WallSliding = true;
                    if (_velocity.y < -_wallSlideSpeedMax)
                    {
                        _velocity.y = -_wallSlideSpeedMax;
                    }

                    if (_timeToWallUnstick > 0)
                    {
                        _smoothDampVelocity = 0;
                        _velocity.x = 0;
                        if (input.x != wallDirectionX && input.x != 0)
                        {
                            _timeToWallUnstick -= Time.deltaTime;
                        }
                        else
                        {
                            _timeToWallUnstick = wallStickTime;
                        }
                    }
                    else
                    {
                        _timeToWallUnstick = wallStickTime;
                    }
                }
            }

            if ((BoxCollisionController2D.Collisions.Above || BoxCollisionController2D.Collisions.Below) && !dashing)
            {
                _velocity.y = 0;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && RuneState == BasicRune.runeState.dash)
            {
                dashing = true;
                _velocity.x = 10 * BoxCollisionController2D.Collisions.faceDirection;
                canDash = false;
                StartCoroutine(setCanDash());
            }
            else
            {
                dashing = false;
            }

            if (BoxCollisionController2D.Collisions.Below)
            {
                secondJump = false;
            }

            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (WallSliding)
                {
                    if (input.x == 0)
                    {
                        _velocity.x = -wallDirectionX * wallJumpOff.x;
                        _velocity.y = wallJumpOff.y;
                    }
                    else if (Math.Abs(input.x - (-wallDirectionX)) < 0.1)
                    {
                        _velocity.x = -wallDirectionX * wallJumpLeap.x;
                        _velocity.y = wallJumpLeap.y;
                    }
                    else if (Math.Abs(wallDirectionX - input.x) < 0.1)
                    {
                        _velocity.x = -wallDirectionX * wallJumpAganst.x;
                        _velocity.y = wallJumpAganst.y;
                    }
                }

                if (BoxCollisionController2D.Collisions.Below || secondJump)
                {
                    _velocity.y = (RuneState == BasicRune.runeState.highJump)? _highJumpVelocity  : _jumpVelocity;
                    if (RuneState == BasicRune.runeState.doubleJump & !secondJump)
                    {
                        secondJump = true;
                    }
                    else
                    {
                        secondJump = false;
                    }

                }
            }


            _animator.SetBool("Walking",
                Input.GetAxisRaw("Horizontal") != 0 && BoxCollisionController2D.Collisions.Below);

            transform.localScale =
                new Vector2(BoxCollisionController2D.Collisions.faceDirection, transform.localScale.y);
            if (!dashing)
            {
                _velocity.y += ((RuneState == BasicRune.runeState.highJump)? _highgravity: _gravity) * Time.deltaTime;
            }
            
            _animator.SetFloat("YVelocity", _velocity.y);
            _animator.SetBool("Grounded", BoxCollisionController2D.Collisions.Below);
            BoxCollisionController2D.Move(_velocity * Time.deltaTime);
        }

        public bool Holding;
        
        
        private List<Ingredient> heldIngredients = new List<Ingredient>();
        private Potion _potion;
        private bool HasPotion;

        private void Interact()
        {
            var hit =
                Physics2D.OverlapBox(transform.position, BoxCollisionController2D.size, 0, collectible);
            if (hit)
            {
                if (_potion == null && hit.tag == "C" && heldIngredients.Count <= maxIngredents)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        heldIngredients.Add(hit.gameObject.GetComponent<Cauldran>().giveIngredient());
                        topIngredint().gameObject.transform.parent = gameObject.transform;
                        topIngredint().transform.localPosition = new Vector2(-0.1878f, -0.094f + (heldIngredients.Count - 1) * topIngredint().GetComponent<BoxCollider2D>().size.y);
                    }
                }
                else if (heldIngredients.Any() && hit.tag == "I" && Input.GetKeyDown(KeyCode.E))
                {
                    var potionMaker = hit.GetComponent<PotionMaker>();
                    if (!potionMaker.canAddIngredient(topIngredint())) return;
                    potionMaker.AddIngredient(topIngredint());
                    
                    var ingrdient = topIngredint();
                    heldIngredients.Remove(ingrdient);
                    Destroy(ingrdient.gameObject);

                }
                else if(!heldIngredients.Any() && hit.tag == "B" && Input.GetKeyDown(KeyCode.E))
                {
                    var potionMaker = hit.GetComponent<PotionMaker>();
                    var potion = potionMaker.makePotion();
                    potion.gameObject.transform.parent = gameObject.transform;
                    _potion = potion;
                    HasPotion = true;
                    potion.transform.localPosition = new Vector2(-0.1878f, -0.094f);
                    
                }
                else if(HasPotion && hit.tag =="J" && Input.GetKeyDown(KeyCode.E))
                {
                    var turnin = hit.GetComponent<Turnin>();
                    turnin.AddPotion(_potion);
                    Destroy(_potion.gameObject);
                    HasPotion = false;

                }
            }
            else if(heldIngredients.Any() && Input.GetKeyDown(KeyCode.E))
            {
                var ingrdient = topIngredint();
                heldIngredients.Remove(ingrdient);
                ingrdient.delete();

            }
        }

        private bool haveRune;
        private GameObject inRange;
        private BasicRune _basicRune;

        private void Runes()
        {
            var hit =
                Physics2D.OverlapBox(transform.position, BoxCollisionController2D.size, 0, RuneLayerMask);
            if (hit && !haveRune)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    var rune = hit.gameObject.GetComponent<BasicRune>();
                    _basicRune = rune;
                    RuneState = rune.State;
                    haveRune = true;
                    rune.PickUp();
                }

                if (inRange != hit.gameObject)
                {
                    if (inRange != null)
                    {
                        inRange.GetComponent<BasicRune>().inract = false;
                    }
                    
                    inRange = hit.gameObject;
                }
                inRange.GetComponent<BasicRune>().inract = true;
                
            }
            else if (Input.GetKeyDown(KeyCode.Q) && haveRune)
            {
                RuneState = BasicRune.runeState.none;
                _basicRune.SetDown(transform.position);
                haveRune = false;
            }
            else
            {
                if (inRange != null)
                {
                    inRange.GetComponent<BasicRune>().inract = false;
                    inRange = null;
                }
                
            }

            

            
            
        }


        public void hit(int damage)
        {
            health -= damage;
        }

        private Ingredient topIngredint()
        {
           return heldIngredients[heldIngredients.Count -1];
        }
        
        
    }
}