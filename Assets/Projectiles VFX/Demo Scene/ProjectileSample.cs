using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSample : MonoBehaviour {
	public GameObject projectile_fx_prefab;
	public GameObject muzzle_shot_prefab;
	public GameObject hit_fx_prefab;
	private GameObject projectile_fx;

	void Start () {
		projectile_fx = Instantiate(projectile_fx_prefab, transform.position, transform.rotation);
		projectile_fx.transform.parent = transform;
		if(muzzle_shot_prefab != null){
			GameObject muzzle = Instantiate(muzzle_shot_prefab, transform.position, transform.rotation);
			muzzle.transform.rotation = transform.rotation;
			Destroy(muzzle.gameObject, 0.45f);
		}
	}
	
	void OnCollisionEnter(Collision hit)
	{
		//Give the trail a little time to dissipate, then destroy 
		//this is not necesary but it looks better than if you just destroy the entire vfx at once.
		int children_count = projectile_fx.transform.childCount;
		for (int i = 0; i < children_count; i++) {
			Transform c_aux = projectile_fx.transform.GetChild (0);
			if(c_aux.name.Contains("trail")){
				c_aux.transform.parent = null;
				Destroy(c_aux.gameObject, 2.0f);
			}
		}
		//Instantiate & destroy hit vfx
		if(hit_fx_prefab != null){
			GameObject hit_vfx = Instantiate(hit_fx_prefab, transform.position, Quaternion.identity);
			Destroy(hit_vfx, 0.7f);
		}
		//Destroy the prjectile vfx
		Destroy(gameObject);
	}
}
