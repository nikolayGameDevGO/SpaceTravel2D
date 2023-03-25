using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private GameObject _goScore;
    [SerializeField] private Slider _slider;
    public GameObject gameObjectManager;

    public GameObject projectileHero;
    public GameObject pointCreateProjectile;
    public GameObject pointCPLeft;
    public GameObject pointCPRight;
    private int countWepons;


    private Rigidbody2D _rb;
    private float _timeCreate;
    private Score _score;
    private HealthBar _healthBar;
    private int _currentHealth;
    private TakingDamage _takingDamage;

    private bool activeSpeedModForFire = false;
    private float timerForModForFire;
    [SerializeField] private float timeActiveFireMod;
    [SerializeField] private float _rateOfFire;
    private float defultRateOfFire;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.position = Vector2.zero;
        _score = _goScore.GetComponent<Score>();
        _currentHealth = _maxHealth;
        _healthBar = _slider.GetComponent<HealthBar>();
        _takingDamage = GetComponent<TakingDamage>();
        defultRateOfFire = _rateOfFire;
        countWepons = 0;
    }

    private void FixedUpdate()
    {
        GetDamage();
        UpdateHealthBar();
        MovePlayer();
        CreateProjectileHero();
        BonusSpeedFire();
    }

    public int health
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = value;
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
        }
    }
    public float rateOfFire
    {
        get { return _rateOfFire; }
        set
        {
            _rateOfFire = value;
        }
    }


    public void BonusSpeedFire()
    {
        if (Time.time > timerForModForFire)
        {

            activeSpeedModForFire = false;
        }

        if (activeSpeedModForFire == false)
        {
            _rateOfFire = defultRateOfFire;
        }
        if (activeSpeedModForFire == true)
        {
            rateOfFire = 0.25f;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        switch (other.tag)
        {
            case "Enemy":
                health--;
                _takingDamage.ChangeColorTakingDamage();
                Enemy _enemy = other.GetComponent<Enemy>();
                _score.UpdateScore(10);
                _enemy.DestroyEnemy();
                break;
            case "PartBigEnemy":
                health--;
                _takingDamage.ChangeColorTakingDamage();
                PartsBigEnemy _pbe = other.GetComponent<PartsBigEnemy>();
                _score.UpdateScore(10);
                _pbe.DestroyPart();
                break;
            case "HP":
                health++;
                Destroy(other);
                break;
            case "PowerUp":
                timerForModForFire = Time.time + timeActiveFireMod;
                activeSpeedModForFire = true;
                Destroy(other);
                break;
            case "Weapon":
                countWepons++;
                Destroy(other);
                if (countWepons == 1)
                {
                    pointCPLeft.SetActive(true);
                }
                if (countWepons == 2)
                {
                    pointCPRight.SetActive(true);
                }
                break;

        }
    }

    private void MovePlayer()
    {
        Vector3 pos2DMouse = Input.mousePosition;
        pos2DMouse.z = -Camera.main.transform.position.z;
        Vector3 pos3DMouse = Camera.main.ScreenToWorldPoint(pos2DMouse);

        float xAxis = pos3DMouse.x;
        float yAxis = pos3DMouse.y;

        Vector3 pos = transform.position;
        pos.x = xAxis;
        pos.y = yAxis + 4f;
        transform.position = pos;
    }

    public void GetDamage()
    {
        if (health == 0)
        {
            Destroy(gameObject);
            GameManager.Restart();
        }
    }
    private void UpdateHealthBar()
    {
        _healthBar.UpdateHealthBar(_maxHealth, health);
    }

    private void CreateProjectileHero()
    {
        if (_timeCreate < Time.time)
        {
            _timeCreate = Time.time + rateOfFire;
            GameObject proje = Instantiate(projectileHero, pointCreateProjectile.transform);
            proje.transform.position = pointCreateProjectile.transform.position;
            
            GameObject proje1 = Instantiate(projectileHero, pointCPLeft.transform);
            proje1.transform.position = pointCPLeft.transform.position;
            
            GameObject proje2 = Instantiate(projectileHero, pointCPRight.transform);
            proje2.transform.position = pointCPRight.transform.position;
        }
    }


}
