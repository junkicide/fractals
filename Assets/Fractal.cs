using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {

	public Mesh mesh;
	public Material material;
	private Material[] materials;

	public int maxDepth;
	private int depth;
	// Use this for initialization

		
		private void Start () {
		if (materials == null) {
			InitializeMaterial(); 
		}
		gameObject.AddComponent<MeshFilter> ().mesh = mesh;
		gameObject.AddComponent<MeshRenderer> ().material = materials [depth];
		if (depth < maxDepth)
		{
			StartCoroutine(CreateChildren());
		}
	}

	 private void InitializeMaterial(){
		materials = new Material[maxDepth + 1];
		for (int i=0; i<=maxDepth; i++) {
			materials[i] = new Material(material);
			materials[i].color = Color.Lerp (Color.white, Color.yellow, (float)i / maxDepth);
	
		}
			
	}

	public float childScale; // scale for shrinking the next layer (default set to 0.5)

	private void Initialize (Fractal parent, int childIndex){
		mesh = parent.mesh; 
		materials = parent.materials;
		maxDepth = parent.maxDepth; //conserving parent variables
		transform.localRotation = orientation[childIndex]; //rotating current object by 'orientation' degrees in all three planes
		depth = parent.depth + 1; // increase the value of depth to indicate addition of a layer
		childScale = parent.childScale; //conserving value of childscale
		transform.parent = parent.transform; // inheriting the transform from the parent's transform
		transform.localScale =  Vector3.one * childScale; 
		transform.localPosition =  childDirections[childIndex] * (0.5f + 0.5f * childScale);
	}

	private IEnumerator CreateChildren () {
		for (int i=0; i<childDirections.Length; i++) {
		yield return new WaitForSeconds(0.5f);
		new GameObject("Fractal Child ").
			AddComponent<Fractal>().Initialize(this, i);
		}
	}
		
	// Update is called once per frame

	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};

	private static Quaternion [] orientation = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(-90f, 0f, 0f),
		Quaternion.Euler(90f, 0f,0f)
	};


}