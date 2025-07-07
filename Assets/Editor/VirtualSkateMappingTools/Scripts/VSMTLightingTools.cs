using System.Collections.Generic;
using UnityEngine;

namespace VirtualSkateMappingTools
{
    public class VSMTLightingTools
    {
        // @todo: create lighting presets (low/med/high or blockout/test/release)
        // @todo: add buttons to UI for this, in lighting group

        public static void GenerateLightProbes(Vector3 offset, Vector3 areaSize, float spacing = 2f, float probeTestSize = 0.25f)
        {
            GameObject probeGroupObj = new GameObject("Generated Light Probe Group");
            LightProbeGroup group = probeGroupObj.AddComponent<LightProbeGroup>();

            List<Vector3> probePositions = new List<Vector3>();
            Vector3 start = offset;

            int xCount = Mathf.CeilToInt(areaSize.x / spacing);
            int yCount = Mathf.CeilToInt(areaSize.y / spacing);
            int zCount = Mathf.CeilToInt(areaSize.z / spacing);

            for (int x = 0; x <= xCount; x++)
            {
                for (int y = 0; y <= yCount; y++)
                {
                    for (int z = 0; z <= zCount; z++)
                    {
                        Vector3 point = start + new Vector3(x * spacing, y * spacing, z * spacing);
                        Collider[] overlaps = Physics.OverlapSphere(point, probeTestSize);

                        bool insideGeometry = false;
                        foreach (Collider c in overlaps)
                        {
                            if (!c.isTrigger) insideGeometry = true;
                        }

                        if (!insideGeometry)
                            probePositions.Add(point);
                    }
                }
            }

            group.probePositions = probePositions.ToArray();
            Debug.Log($"Placed {probePositions.Count} light probes.");
        }
    }
}
