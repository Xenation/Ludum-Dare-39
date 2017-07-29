using UnityEngine;

namespace LD39 {
	[System.Serializable]
	public class MapChunkPrefab {

		public SideType top;
		public SideType right;
		public SideType bottom;
		public SideType left;

		public GameObject prefab;

		public SideType GetSideType(Orientation ori) {
			switch (ori) {
				case Orientation.TOP:
					return top;
				case Orientation.RIGHT:
					return right;
				case Orientation.BOTTOM:
					return bottom;
				case Orientation.LEFT:
					return left;
				default:
					return SideType.CLOSED;
			}
		}

	}

	[AddComponentMenu("LD39/Managers/MapManager")]
	public class MapManager : Singleton<MapManager> {

		public float chunkSize = 20f;
		public int chunksX = 10;
		public int chunksY = 10;
		public int seed = 456159;

		public Transform mapRoot;

		public MapChunkPrefab startingRoom;
		public MapChunkPrefab endingRoom;
		public MapChunkPrefab[] roomPrefabs;

		public MapGrid Grid { get; private set; }

		public void Start() {
			Random.InitState(seed);
			GenerateMap();
		}

		public void GenerateMap() {
			Grid = new MapGrid(chunksX, chunksY, chunkSize);
			GenerateMainPath(10);
		}

		private void GenerateMainPath(int mainPathLength) {
			MapChunk curChunk = MapChunk.CreateMapChunk(startingRoom);
			Grid.SetChunk(new Vector2i(0, 0), curChunk);
			Side nextSide = null;
			for (int i = 0; i < mainPathLength; i++) {
				nextSide = curChunk.GetRandomOpenUnusedSide();
				curChunk = MapChunk.CreateMapChunk(GetRandomRoomPrefab(nextSide.Orient.GetOposite()));
				Grid.SetChunk(nextSide.GetAdjacentPos(), curChunk);
			}
			nextSide = curChunk.GetRandomOpenUnusedSide();
			curChunk = MapChunk.CreateMapChunk(endingRoom);
			Grid.SetChunk(nextSide.GetAdjacentPos(), curChunk);
		}

		public MapChunkPrefab GetRandomRoomPrefab() {
			return roomPrefabs[Random.Range(0, roomPrefabs.Length)];
		}

		public MapChunkPrefab GetRandomRoomPrefab(Orientation requiredOri) {
			MapChunkPrefab[] validChunkPrefabs = new MapChunkPrefab[roomPrefabs.Length];
			int count = 0;
			foreach (MapChunkPrefab chunkPrefab in roomPrefabs) {
				if (chunkPrefab.GetSideType(requiredOri) != SideType.CLOSED) {
					validChunkPrefabs[count++] = chunkPrefab;
				}
			}
			return validChunkPrefabs[Random.Range(0, count)];
		}

	}
}
