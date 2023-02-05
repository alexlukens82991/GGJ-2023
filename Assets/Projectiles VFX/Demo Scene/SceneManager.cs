using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
	public float projectile_speed = 1100;
	public Transform fps_view;
	public Transform top_view;
	public Transform cam_view;
	public TextMesh text_fx_name;
	public GameObject[] fx_prefabs;
	public int index_fx = 0;


	private Ray ray;
	private RaycastHit ray_cast_hit;

	void Start () {
		text_fx_name.text = "[" + (index_fx + 1) + "] " + fx_prefabs[ index_fx ].name;
		Destroy(GameObject.Find("Instructions"), 12.5f);
	}
	
	void Update () {
		//Instantiate Projectiles
		if ( Input.GetMouseButtonDown(0) ){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if ( Physics.Raycast (ray.origin, ray.direction, out ray_cast_hit, 1000f) ){				
				GameObject projectile = Instantiate(fx_prefabs[ index_fx ], transform.position, Quaternion.identity);	
				projectile.transform.LookAt(ray_cast_hit.point);
				projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * projectile_speed);
			}
		}
		//Change Camera View
		if ( Input.GetKeyDown("1") ){
			cam_view.position = fps_view.position;
			cam_view.rotation = fps_view.rotation;
		}

		if ( Input.GetKeyDown("2") ){
			cam_view.position = top_view.position;
			cam_view.rotation = top_view.rotation;
		}
		//Change-FX Keyboard	
		if ( Input.GetKeyDown("z") || Input.GetKeyDown("left") ){
			index_fx--;
			if(index_fx <= -1)
				index_fx = fx_prefabs.Length - 1;
			text_fx_name.text = "[" + (index_fx + 1) + "] " + fx_prefabs[ index_fx ].name;	
		}

		if ( Input.GetKeyDown("x") || Input.GetKeyDown("right")){
			index_fx++;
			if(index_fx >= fx_prefabs.Length)
				index_fx = 0;
			text_fx_name.text = "[" + (index_fx + 1) + "] " + fx_prefabs[ index_fx ].name;
		}
	}


}
