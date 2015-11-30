using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCotrol : MonoBehaviour 
{
	public float speed;

	private Rigidbody rb;

 	void Start () {
		rb = GetComponent<Rigidbody> ();
		
	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed * Time.deltaTime);
	}
	void OnTriggerEnter(Collider other) { 

		if (other.gameObject.CompareTag ("Finish")) {
            // Application.LoadLevel(0);
            Destroy(GameObject.Find("maze"));

            GameObject generator = GameObject.Find("Maze Generator");
            generator.GetComponent<Maze>().createLevel();
        }

	}

}

