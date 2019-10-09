using UnityEngine;

public class MutantTaunt : MonoBehaviour
{
	public Animator animator;

	private Enemy mutant;

	void Start()
	{
		mutant = gameObject.GetComponent<Enemy>();
	}

	void TauntStart()
	{
		mutant.cantMove = true;
		mutant.inVulnerable = true;
	}

	void TauntEnd()
	{
		mutant.cantMove = false;
		mutant.inVulnerable = false;
	}
}
