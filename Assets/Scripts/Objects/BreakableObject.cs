using UnityEngine;

public class BreakableObject : MonoBehaviour, IBreakable
{
	[field: SerializeField] public float HP { get; set; }
	[field: SerializeField] public GameObject BrokenObj { get; set; }
	[SerializeField] public GameObject OriginalObj; //ByEma
	[SerializeField] private GameObject spawnPoint;
	[SerializeField] bool dealOneDmg;

	public bool IsBroken { get; set; }
	public Transform colliderTransform { get; set; }
	
	/*
	MeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }
    */
    //CommentedByEma


    public void Break()
	{
		IsBroken = true;
		BrokenObj.transform.position = transform.position;
		BrokenObj.SetActive(true);
		OriginalObj.SetActive(false); //ByEma
		//mesh.enabled = false; //CommentedByEma
		spawnPoint.SetActive(false); 

	}


	public void NoHP()
	{
		Break();
	}

	public void TakeDamage(float damage)
	{
		if (IsBroken)
			return;

		if (dealOneDmg)
			HP--;
		else
			HP -= damage;

		Score.OnDmg?.Invoke(damage);
		RageBar.OnRage?.Invoke();

		if (HP <= 0)
		{
			NoHP();
		}
	}
	public void Knockback(float _Power)
	{ }
}
