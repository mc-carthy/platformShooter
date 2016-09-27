using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private Rigidbody2D rb;

	private float lifeTime = 0.4f;
	private float speed = 20;
	private float timer;

	private Vector2 direction;
	public  Vector2 Direction {
		set {
			direction = value;
		}
	}

	private void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = direction * speed;
		Destroy (gameObject, lifeTime);
	}

	private void OnTriggerEnter2D (Collider2D trig) {
		if (trig.tag == "enemy") {
			Destroy (gameObject);
			Destroy (trig.gameObject);
		}
	}
}