using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractuable : MonoBehaviour
{
	[SerializeField] Rigidbody RB;
	[SerializeField] MeshRenderer MR;
	[SerializeField] BoxCollider PlayerTrigger;

	GameObject tempChild;
	Transform CollactableContainer;

	string TagGround;
	string TagPlayer;

	bool isConnectedPlayer;

	void Start()
	{
		StartMethods();
	}

	#region StartMethods

	void StartMethods()
	{
		SetCollactableContainer();
		GetTags();
	}

	void SetCollactableContainer()
	{
		CollactableContainer = transform.parent;
	}

	void GetTags()
	{
		TagPlayer = GameController.instance.TagPlayer;
		TagGround = GameController.instance.TagGround;
	}
	#endregion

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(TagPlayer))
		{
			PlayerTrigger.enabled = false;
			isConnectedPlayer = true;
			RB.constraints = RigidbodyConstraints.None;
		}
		else if (other.CompareTag(TagGround) && isConnectedPlayer)
		{
			MR.enabled = false;
			while (transform.childCount > 0)
			{
				tempChild = transform.GetChild(0).gameObject;
				tempChild.transform.SetParent(CollactableContainer);
				tempChild.SetActive(true);
			}
			Destroy(gameObject);
		}
	}


}
