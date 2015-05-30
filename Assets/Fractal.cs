using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {

	public Mesh[] meshes;
	public Material material;
	private Material[,] materials;

	public int maxDepth;
	private int depth;
	public float spawnProbability;
	// Use this for initialization

		
		private void Start () {
		if (materials == null) {
			InitializeMaterial(); 
		}
		gameObject.AddComponent<MeshFilter> ().mesh = meshes[Random.Range(0, meshes.Length)];
		gameObject.AddComponent<MeshRenderer> ().material = materials [depth, Random.Range (0,2)];
		if (depth < maxDepth)
		{
			StartCoroutine(CreateChildren());
		}
	}

	 private void InitializeMaterial(){
		materials = new Material[maxDepth + 1, 2];
		for (int i=0; i<=maxDepth; i++) {
			materials[i, 0] = new Material(material);
			materials[i, 0].color = Color.Lerp (Color.white, Color.yellow, (float)i / maxDepth);
			materials[i, 1] = new Material(material);
			materials[i, 1].color = Color.Lerp (Color.white, Color.cyan, (float)i / maxDepth);
	
		}
		materials [maxDepth, 0].color = Color.red;
		materials [maxDepth, 1].color = Color.magenta;
	}

	public float childScale; // scale for shrinking the next layer (default set to 0.5)

	private void Initialize (Fractal parent, int childIndex){
		spawnProbability = parent.spawnProbability;
		meshes = parent.meshes; 
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
			if (Random.value < spawnProbability){
		yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
		new GameObject("Fractal Child ").
			AddComponent<Fractal>().Initialize(this, i);
		}
		}
	}
		
	// Update is called once per frame
	private void Update () {
		transform.Rotate(15f * Time.deltaTime, 15f * Time.deltaTime, 0f);
	}

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