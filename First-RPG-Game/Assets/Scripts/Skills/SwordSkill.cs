using Skills.SkillControllers;
using UnityEngine;

namespace Skills
{
    public enum SwordType
    {
        Regular,
        Bounce,
        Pierce,
        Spin
    }

    public class SwordSkill : Skill
    {
        public SwordType swordType = SwordType.Regular;

        [Header("Bounce info")]
        [SerializeField] private int bounceAmount;
        [SerializeField] private float bounceGravity;
        [SerializeField] private float bounceSpeed;

        [Header("Pierce info")]
        [SerializeField] private int pierceAmount;
        [SerializeField] private float pierceGravity;

        [Header("Spin info")]
        [SerializeField] private float maxTravelDistance = 7;
        [SerializeField] private float spinDuration = 2;
        [SerializeField] private float spinGravity = 1;
        [SerializeField] private float hitCooldown = .25f;

        [Header("Skill info")]
        [SerializeField] private GameObject swordPrefab;
        [SerializeField] private float freezeTimeDuration;
        [SerializeField] private float returnSpeed;

        [SerializeField]
        private Vector2 launchForce;

        [SerializeField] private float swordGravity;

        [Header("Aim dots")]
        [SerializeField] private int numberOfDots;

        [SerializeField] private float spaceBetweenDots;
        [SerializeField] private GameObject dotPrefab;
        [SerializeField] private Transform dotsParent;

        private GameObject[] _dots;

        private Vector2 _finalDir;


        protected override void Start()
        {
            base.Start();

            GenerateDots();

            SetupGravity();
        }

        private void SetupGravity()
        {
            if (swordType == SwordType.Bounce)
            {
                swordGravity = bounceGravity;
            }
            else if (swordType == SwordType.Pierce)
            {
                swordGravity = pierceGravity;
            } 
            else if (swordType == SwordType.Spin)
            {
                swordGravity = spinGravity;
            } 
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                _finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                for (int i = 0; i < _dots.Length; i++)
                {
                    _dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
                }
            }
        }

        public void CreateSword()
        {
            GameObject newSword = Instantiate(swordPrefab, Player.transform.position, transform.rotation);
            SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

            if (swordType == SwordType.Bounce)
            {
                newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
            } 
            else if(swordType == SwordType.Pierce)
            {
                newSwordScript.SetupPierce(pierceAmount);
            } 
            else if (swordType == SwordType.Spin)
            {
                newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown, hitCooldown);
            }

            newSwordScript.SetupSword(_finalDir, swordGravity, Player, freezeTimeDuration, returnSpeed);

            Player.AssignNewSword(newSword);

            SetDotsActive(false);
        }

        #region Aiming

        public void SetDotsActive(bool isActive)
        {
            foreach (var t in _dots)
            {
                t.SetActive(isActive);
            }
        }

        private void GenerateDots()
        {
            _dots = new GameObject[numberOfDots];
            for (int i = 0; i < numberOfDots; i++)
            {
                _dots[i] = Instantiate(dotPrefab, Player.transform.position, Quaternion.identity, dotsParent);
                _dots[i].SetActive(false);
            }
        }

        // y = x0 + v0*t + 0.5*g*t^2
        private Vector2 DotsPosition(float time)
        {
            Vector2 position = (Vector2)Player.transform.position //initial position (x0)
                               + new Vector2( //initial velocity (v0*t)
                                   AimDirection().normalized.x * launchForce.x,
                                   AimDirection().normalized.y * launchForce.y
                               ) * time
                               + Physics2D.gravity * (.5f * swordGravity * (time * time)); //(1/2)gt^2
            return position;
        }

        public Vector2 AimDirection()
        {
            Vector2 playerPosition = Player.transform.position;
            Vector2 mousePosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - playerPosition;

            return direction;
        }

        #endregion
        
        
    }
}