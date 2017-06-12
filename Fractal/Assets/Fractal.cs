using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {
	
    public Material material;
    public Mesh mesh;

	public int maxDepth;
	private int depth;

	public float childScale;

	private struct Child {
		public Vector3 direction;
		public Quaternion orientation;

		public Child(Vector3 direction, Quaternion orientation) {
			this.direction = direction;
			this.orientation = orientation;
		}
	};

	public Material[] materials;

	private static Child[] children = {
		new Child (Vector3.up, Quaternion.identity),
		new Child (Vector3.right, Quaternion.Euler (0f, 0f, -90f)),
		new Child (Vector3.left, Quaternion.Euler (0f, 0f, 90f)),
		new Child (Vector3.forward, Quaternion.Euler(90f, 0f, 0f)),
		new Child (Vector3.back, Quaternion.Euler(-90f, 0f, 0f))
	};

    // Use this for initialization
    void Start () {
		if (depth == 0) {
			InitializeMaterials();
		}
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = materials[depth];
		if (depth < maxDepth)
        {
			StartCoroutine (CreateChildren ());
        }
	}

	private IEnumerator CreateChildren() {
		foreach (var child in children) {
			yield return new WaitForSeconds (Random.Range(0.1f, 0.5f));
			new GameObject ("Fractal Child").AddComponent<Fractal> ()
				.Create (this, child.direction, child.orientation);
		}
	}

	void Create(Fractal parent, Vector3 direction, Quaternion rotation)
    {
		this.mesh = parent.mesh;

		this.maxDepth = parent.maxDepth;
		this.depth = parent.depth + 1;

		this.transform.parent = parent.transform;

		this.childScale = parent.childScale;
		this.transform.localScale = Vector3.one * childScale;
		this.transform.localPosition = direction * (0.5f + 0.5f * childScale); 

		this.transform.localRotation = rotation;

		this.materials = parent.materials;
    }

	private void InitializeMaterials () {
		materials = new Material[maxDepth + 1];
		for (int i = 0; i <= maxDepth; i++) {
			float t = (float) i / maxDepth;
			materials [i] = new Material (material);
			if (i == maxDepth) {
				materials [i].color = Color.magenta;
			} else {
				materials [i].color =
					Color.Lerp (Color.white, Color.yellow, t);
			}
		}
	}
}
