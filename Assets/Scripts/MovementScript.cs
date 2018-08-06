using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] points;

	public GameObject[] Points{ get{return points;}}

    [SerializeField]
    private float movSpeed = 5.0f;

	[SerializeField]
    private float rotSpeed = 30.0f;

    private int currentPoint = 0;

    private string buttonLeft = "Left";

    private string buttonRight = "Right";

    bool leftPress, rightPress, bothPress;

    bool slowmoStarted = false, slowmoReloading = false;

	public float SlowMoTime {get; private set;}

	public float SlowMoTimeNormalized {get; private set;}

	[SerializeField]
	private float maxSlowMoTime = 30.0f;

	public float MaxSlowMoTime {get{return maxSlowMoTime;}}

	[SerializeField]
	private float slowMoReloadModifier = 0.5f;

	[SerializeField]
	private float slowmoScale = 0.3f;

	private Coroutine slowmoCoroutine, slowmoReloadCoroutine;

	public UnityEvent onSlowmoChange = new UnityEvent();

	public static MovementScript movement = null;
	private void Awake() {
		if (movement == null) {
			movement = this;
		} else {
			Debug.LogError("Duplicated MovementScript", this.gameObject);
			Destroy(this);
		}
	}

    // Use this for initialization
    void Start()
    {
		SlowMoTime = MaxSlowMoTime;
		SlowMoTimeNormalized = 1.0f;
		onSlowmoChange.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        leftPress = Input.GetButtonDown(buttonLeft);
        rightPress = Input.GetButtonDown(buttonRight);
        bothPress = leftPress && rightPress;
        if (bothPress)
        {
            SlowMotion();
        }
        else if (leftPress)
        {
            PrevPoint();
        }
        else if (rightPress)
        {
            NextPoint();
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(
			transform.position, 
			points[currentPoint].transform.position, 
			movSpeed * Time.deltaTime
		);

		transform.rotation = Quaternion.RotateTowards(
			transform.rotation,
			points[currentPoint].transform.rotation,
			rotSpeed * Time.deltaTime 
		);
    }

    void NextPoint()
    {
        currentPoint++;
        if (currentPoint >= points.Length)
        {
            currentPoint = 0;
        }
    }

    void PrevPoint()
    {
        currentPoint--;
        if (currentPoint < 0)
        {
            currentPoint = points.Length - 1;
        }
    }

    void SlowMotion()
    {
		if (!slowmoStarted) {
			slowmoStarted = true;
			if (slowmoReloading) {
				StopCoroutine(slowmoReloadCoroutine);
				slowmoReloading = false;
			}
			slowmoCoroutine = StartCoroutine(SlowmoUse());
		} else {
			StopCoroutine(slowmoCoroutine);
			Time.timeScale = 1.0f;
			slowmoStarted = false;
			if (!slowmoReloading) {
				slowmoReloading = true;
				slowmoReloadCoroutine = StartCoroutine(SlowmoReload());
			}
		}
    }

	IEnumerator SlowmoUse()
	{
		Time.timeScale = slowmoScale;
		while (SlowMoTime > 0.0f) {
			SlowMoTime -= Time.unscaledDeltaTime;
			SlowMoTimeNormalized = SlowMoTime / MaxSlowMoTime;
			onSlowmoChange.Invoke();
			yield return null;
		}
		SlowMoTime = Mathf.Max(SlowMoTime, 0.0f);
		SlowMoTimeNormalized = SlowMoTime / MaxSlowMoTime;
		onSlowmoChange.Invoke();
		Time.timeScale = 1.0f;
		slowmoStarted = false;

		slowmoReloading = true;
		slowmoReloadCoroutine = StartCoroutine(SlowmoReload());
	}	

	IEnumerator SlowmoReload() 
	{
		while (SlowMoTime < maxSlowMoTime) {
			SlowMoTime += Time.deltaTime * slowMoReloadModifier;
			SlowMoTimeNormalized = SlowMoTime / MaxSlowMoTime;
			onSlowmoChange.Invoke();
			yield return null;
		}
		SlowMoTime = Mathf.Min(SlowMoTime, MaxSlowMoTime);
		SlowMoTimeNormalized = SlowMoTime / MaxSlowMoTime;
		onSlowmoChange.Invoke();
		slowmoReloading = false;
	}

	public void Death() {
		if (slowmoStarted) {
			SlowMotion();
		}
		this.enabled = false;
	}

	public void Revive() {
		Start();
		this.enabled = true;
	}

	public void SlowmoRecharge(float amount) {
		SlowMoTime += amount;
		onSlowmoChange.Invoke();
	}
}
