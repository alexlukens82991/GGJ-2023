using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LukensUtils
{
    public class LukensUtilsRuntime : MonoBehaviour
    {
        public void DelayedFire(Action action, float time)
        {
            StartCoroutine(DelayedFireRoutine(action, time));
        }

        private IEnumerator DelayedFireRoutine(Action action, float time)
        {
            yield return new WaitForSeconds(time);

            if (action.Target != null)
                action.Invoke();
        }
        
        public GameObject WorldSpaceMouseRaycast(float distance = 100, Camera cam = null)
        {
            GameObject foundObject = null;

            if (cam == null)
                cam = Camera.main;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, distance))
                foundObject = hit.collider.gameObject;

            return foundObject;
        }

        public GameObject RaycastFromPosition(Vector3 position, Vector3 direction, float distance = 100f)
        {
            if (Physics.Raycast(position, direction, out RaycastHit hit, distance))
                return hit.collider.gameObject;

            Debug.Log("Raycast did not hit anything!");
            return null;
        }
    }
}