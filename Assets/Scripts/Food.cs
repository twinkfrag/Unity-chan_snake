using UnityEngine;

namespace Assets.Scripts
{
	class Food : MonoBehaviour
	{
		[SerializeField]
		private GameObject foodPrefab;

		public GameObject FoodPrefab
		{
			get { return foodPrefab; }
			set { foodPrefab = value; }
		}

		public void Next()
		{
			var x = Random.Range(-4.5f, 4.5f);
			var z = Random.Range(-4.5f, 4.5f);
			Instantiate(FoodPrefab, new Vector3(x, transform.position.y, z), transform.rotation).name = FoodPrefab.name;
		}
	}
}
