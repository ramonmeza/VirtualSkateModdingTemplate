using UnityEngine;

[ExecuteInEditMode]
public class Array : MonoBehaviour
{
    public int count = 3;
    public GameObject prefab;
    public Vector3 offset;

    private void OnEnable()
    {
        if (!Application.isPlaying)
            Generate();
    }

    private void Start()
    {
        if (Application.isPlaying)
            Generate();
    }

    void Generate()
    {
        if (prefab == null) return;

        // Clear previous instances
        while (transform.childCount > 0)
        {
            var child = transform.GetChild(0);
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        // Try to get the width of the prefab
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.transform.localPosition = i * offset;
        }
    }
}
