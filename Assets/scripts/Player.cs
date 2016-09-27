using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public delegate void OnHitSpikeAction();
	public delegate void OnHitEnemyAction();
	public delegate void OnHitOrbAction();

	public OnHitSpikeAction onHitSpike;
	public OnHitEnemyAction onHitEnemy;
	public OnHitOrbAction onHitOrb;

	[SerializeField]
	private Bullet bullet;
	private Rigidbody2D rb;

	private float moveForce = 1000;
	private float jumpForce = 10000;
	private float xClamp;
	private bool canJump;
	bool isLookingRight = true;

	private void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}

	private void Update () {
		GetInput ();
	}

	private void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "solid") {
			canJump = true;
		}
		if (col.gameObject.tag == "spike") {
			if (onHitSpike != null) {
				onHitSpike ();
			}		
		} else if (col.gameObject.tag == "enemy") {
			if (transform.position.y > col.gameObject.transform.position.y + col.gameObject.GetComponent<BoxCollider2D> ().size.y / 2) {
				Destroy (col.gameObject);
				if (onHitEnemy != null) {
					onHitEnemy ();
				}
			} else {
				if (onHitSpike != null) {
					onHitSpike ();
				}
			}
		} else if (col.gameObject.tag == "orb") {
			if (onHitOrb != null) {
				onHitOrb ();
			}
		}
	}

	private void GetInput () {
		float xMove = Input.GetAxisRaw ("Horizontal");
		if (xMove > 0) {
			isLookingRight = true;
		}
		if (xMove < 0) {
			isLookingRight = false;
		}
		rb.AddForce (new Vector2 (xMove * moveForce * Time.deltaTime, 0));

		if (Input.GetAxisRaw("Vertical") > 0) {
			Jump ();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			Shoot ();
		}
	}

	private void Jump (bool force = false) {
		if (canJump || force) {
			canJump = false;
			rb.AddForce (Vector2.up * jumpForce * Time.deltaTime);
		}
	}

	private void Shoot () {
		if (Time.timeScale == 1) {
			Bullet bullet = Instantiate (
				                this.bullet,
								transform.position,
				                Quaternion.identity
			) as Bullet;
			//bullet.transform.SetParent (transform);
			bullet.Direction = isLookingRight ? Vector2.right : Vector2.left;
		}
	}
}
