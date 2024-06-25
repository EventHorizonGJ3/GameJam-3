using UnityEngine;
using System.Collections;
public class BreakableObject : MonoBehaviour, IBreakable
{
	[field: SerializeField] public float HP { get; set; }
	[field: SerializeField] public GameObject BrokenObj { get; set; }
	[SerializeField] public GameObject OriginalObj; //ByEma
	[SerializeField] private GameObject spawnPoint;
	[SerializeField] bool dealOneDmg;
	[SerializeField] private bool deactivateColliderOnBreak; //ByEma
	[SerializeField] private float respawnTime = 10f; //ByEma
	public bool IsBroken { get; set; }
	public Transform colliderTransform { get; set; }
	
	private Collider objectCollider;
	
	private float initialHP; //ByEma
	
	//MeshRenderer mesh;

    private void Awake()
    {
        //mesh = GetComponent<MeshRenderer>(); //ByEma
        objectCollider = GetComponent<Collider>(); //ByEma
        initialHP = HP; //ByEma
    }


    public void Break()
	{
		IsBroken = true;
		BrokenObj.transform.position = transform.position;
		BrokenObj.SetActive(true);
		OriginalObj.SetActive(false); //ByEma
		//mesh.enabled = false; //CommentedByEma
		spawnPoint.SetActive(false); 
		//ByEmaStart
		if (deactivateColliderOnBreak && objectCollider != null)
		{
			objectCollider.enabled = false; // Deactivate the collider
		}
		StartCoroutine(Respawn());
		//ByEmaEnd
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
	
	//ByEma: provo un respawn :}
	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds(respawnTime);

		IsBroken = false;
		BrokenObj.SetActive(false);
		OriginalObj.SetActive(true);
		spawnPoint.SetActive(true);

		if (deactivateColliderOnBreak && objectCollider != null)
		{
			objectCollider.enabled = true;
		}
		HP = initialHP;
	}
}
