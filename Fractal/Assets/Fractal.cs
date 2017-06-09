using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {



    public Material material;
    public Mesh mesh;

	public int maxDepth;
	private int depth;

	public float childScale;

    // Use this for initialization
    void Start () {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;

        if (depth < maxDepth)
        {
			StartCoroutine (CreateChildren ());
        }
	}

	private IEnumerator CreateChildren() {
		yield return new WaitForSeconds (0.5f);
		new GameObject ("Fractal Child").AddComponent<Fractal> ()
			.Create (this, Vector3.up);
		yield return new WaitForSeconds (0.5f);
		new GameObject ("Fractal Child").AddComponent<Fractal> ()
			.Create (this, Vector3.right);
		yield return new WaitForSeconds (0.5f);
		new GameObject ("Fractal Child").AddComponent<Fractal> ()
			.Create (this, Vector3.left);

	}

	void Create(Fractal parent, Vector3 direction)
    {
        material = parent.material;
        mesh = parent.mesh;

        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;

        transform.parent = parent.transform;

		childScale = parent.childScale;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = direction * (0.5f + 0.5f * childScale); 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
