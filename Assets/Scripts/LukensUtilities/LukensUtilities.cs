using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LukensUtils
{
    public static class LukensUtilities
    {
        private static LukensUtilsRuntime m_LukensRuntime;

        public static void ToggleCanvasGroup(CanvasGroup cg, bool active)
        {
            int alpha = active ? 1 : 0;

            cg.alpha = alpha;
            cg.interactable = active;
            cg.blocksRaycasts = active;
        }

        public static void DelayedFire(Action action, float time)
        {
            CheckForMonobehavior();

            m_LukensRuntime.DelayedFire(action, time);
        }

        public static GameObject RaycastFromMouse(float distance = 100, Camera cam = null)
        {
            CheckForMonobehavior();
            return m_LukensRuntime.WorldSpaceMouseRaycast(distance, cam);
        }

        public static GameObject RaycastFromPosition(Vector3 position, Vector3 direction, float distance = 100f)
        {
            CheckForMonobehavior();
            return m_LukensRuntime.RaycastFromPosition(position, direction, distance);
        }

        private static void CheckForMonobehavior()
        {
            if (m_LukensRuntime == null)
            {
                GameObject newMono = new GameObject();
                m_LukensRuntime = newMono.AddComponent<LukensUtilsRuntime>();
            }
        }

    }
}