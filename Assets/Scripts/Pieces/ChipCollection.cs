using UnityEngine;

namespace CardGames.Pieces
{
    public class ChipCollection : MonoBehaviour
    {
        [SerializeField] Chip blue;
        [SerializeField] Chip red;
        [SerializeField] Chip green;
        [SerializeField] Chip black;
        [SerializeField] Transform blueChipSpawnLocation;
        [SerializeField] Transform redChipSpawnLocation;
        [SerializeField] Transform greenChipSpawnLocation;
        [SerializeField] Transform blackChipSpawnLocation;
        private const string chipTag = "Chip";

        public void SpawnChips(int money)
        {
            RemoveChips();
            int remainingValue = money;
            SpawnChips(ref remainingValue, black, blackChipSpawnLocation.position);
            SpawnChips(ref remainingValue, green, greenChipSpawnLocation.position);
            SpawnChips(ref remainingValue, red, redChipSpawnLocation.position);
            SpawnChips(ref remainingValue, blue, blueChipSpawnLocation.position);
        }

        private void SpawnChips(ref int remainingValue, Chip chip, Vector3 position)
        {
            int chipsToSpawn = remainingValue / chip.GetValue();
            float height = chip.GetModel().GetComponent<MeshCollider>().bounds.size.y;
            for (int i = 0; i < chipsToSpawn; i++)
            {
                Instantiate(
                    chip.GetModel(), 
                    new Vector3(position.x, position.y + i * height, position.z), 
                    Quaternion.identity, 
                    transform);
            }
            remainingValue -= chipsToSpawn * chip.GetValue();
        }

        private void RemoveChips()
        {
            var children = GetComponentsInChildren<Transform>();
            if (children.Length <= 5) return;
            foreach(var child in children)
            {
                if(child.gameObject.tag == chipTag)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}