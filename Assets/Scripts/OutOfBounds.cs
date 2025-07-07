using System;
using System.Reflection;
using UnityEngine;

namespace VirtualSkateMappingTools
{
    public class OutOfBounds : MonoBehaviour
    {
        private Type playerManagerType;
        private PropertyInfo instanceProperty;
        private PropertyInfo isRespawningProperty;
        private MethodInfo outOfBoundsRespawnMethod;

        private void Awake()
        {
            // Look for the PlayerManager type in all loaded assemblies
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                playerManagerType = assembly.GetType("PlayerManager");
                if (playerManagerType != null) break;
            }

            if (playerManagerType != null)
            {
                instanceProperty = playerManagerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                isRespawningProperty = playerManagerType.GetProperty("isRespawning", BindingFlags.Public | BindingFlags.Instance);
                outOfBoundsRespawnMethod = playerManagerType.GetMethod("OutOfBoundsRespawn", BindingFlags.Public | BindingFlags.Instance);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (playerManagerType == null || instanceProperty == null || isRespawningProperty == null || outOfBoundsRespawnMethod == null)
                return;

            if (other.gameObject.layer == LayerMask.NameToLayer("OutOfBoundsPlayer"))
            {
                var instance = instanceProperty.GetValue(null);
                if (instance != null)
                {
                    bool isRespawning = (bool)isRespawningProperty.GetValue(instance);
                    if (!isRespawning)
                    {
                        Debug.LogError("Respawn (reflected)");
                        outOfBoundsRespawnMethod.Invoke(instance, null);
                    }
                }
            }
        }
    }
}
