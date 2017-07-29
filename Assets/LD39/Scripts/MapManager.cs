using System.Collections.Generic;
using UnityEngine;

namespace LD39 {
	[System.Serializable]
	public class MapChunkPrefab {

		public SideType top;
		public SideType right;
		public SideType bottom;
		public SideType left;

		public int OpenCount {
			get {
				int count = 0;
				if (top != SideType.CLOSED) {
					count++;
				}
				if (right != SideType.CLOSED) {
					count++;
				}
				if (bottom != SideType.CLOSED) {
					count++;
				}
				if (left != SideType.CLOSED) {
					count++;
				}
				return count;
			}
		}

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
			if (mapRoot == null) {
				mapRoot = new GameObject("Map").transform;
			}
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
			bool stuck = false;
			for (int i = 0; i < mainPathLength; i++) {
				List<Side> possibleSides = curChunk.GetAllOpenUnusedSides();
				nextSide = possibleSides[Random.Range(0, possibleSides.Count)];
				possibleSides.Remove(nextSide);
				Orientation requiredOri = nextSide.Orient.GetOposite();
				MapChunkPrefab prefab = null;
				while (Grid.GetAdjacentSidesLeadingToTile(nextSide.GetAdjacentPos(), requiredOri).Length != 0) {
					if (possibleSides.Count == 0) {
						stuck = true;
						break;
					}
					nextSide = possibleSides[Random.Range(0, possibleSides.Count)];
					possibleSides.Remove(nextSide);
					requiredOri = nextSide.Orient.GetOposite();
				}
				List<MapChunkPrefab> possiblePrefabs = GetAllRoomPrefabs(requiredOri, nextSide.Type, Grid.GetAdjacentSidesOccupied(nextSide.GetAdjacentPos(), requiredOri));
				prefab = possiblePrefabs[Random.Range(0, possiblePrefabs.Count)];
				possiblePrefabs.Remove(prefab);
				while (prefab.OpenCount < 2) {
					if (possiblePrefabs.Count == 0) {
						stuck = true;
						break;
					}
					prefab = possiblePrefabs[Random.Range(0, possiblePrefabs.Count)];
					possiblePrefabs.Remove(prefab);
				}
				if (stuck) {
					break;
				}
				curChunk = MapChunk.CreateMapChunk(prefab);
				Grid.SetChunk(nextSide.GetAdjacentPos(), curChunk);
			}
			nextSide = curChunk.GetRandomOpenUnusedSide();
			curChunk = MapChunk.CreateMapChunk(endingRoom);
			Grid.SetChunk(nextSide.GetAdjacentPos(), curChunk);
		}

		public MapChunkPrefab GetRandomRoomPrefab() {
			return roomPrefabs[Random.Range(0, roomPrefabs.Length)];
		}

		public MapChunkPrefab GetRandomRoomPrefab(Orientation requiredOri, SideType requiredType) {
			MapChunkPrefab[] validChunkPrefabs = new MapChunkPrefab[roomPrefabs.Length];
			int count = 0;
			foreach (MapChunkPrefab chunkPrefab in roomPrefabs) {
				if (chunkPrefab.GetSideType(requiredOri) == requiredType) {
					validChunkPrefabs[count++] = chunkPrefab;
				}
			}
			return validChunkPrefabs[Random.Range(0, count)];
		}

		public MapChunkPrefab GetRandomRoomPrefab(Orientation requiredOri, SideType requiredType, Orientation[] blockedSides) {
			MapChunkPrefab[] validChunkPrefabs = new MapChunkPrefab[roomPrefabs.Length];
			int count = 0;
			foreach (MapChunkPrefab chunkPrefab in roomPrefabs) {
				if (chunkPrefab.GetSideType(requiredOri) == requiredType) {
					bool leadsToBlocked = false;
					foreach (Orientation blocked in blockedSides) {
						if (chunkPrefab.GetSideType(blocked) != SideType.CLOSED) {
							leadsToBlocked = true;
							break;
						}
					}
					if (!leadsToBlocked) {
						validChunkPrefabs[count++] = chunkPrefab;
					}
				}
			}
			return validChunkPrefabs[Random.Range(0, count)];
		}

		public List<MapChunkPrefab> GetAllRoomPrefabs(Orientation requiredOri, SideType requiredType, Orientation[] blockedSides) {
			List<MapChunkPrefab> validChunkPrefabs = new List<MapChunkPrefab>();
			foreach (MapChunkPrefab chunkPrefab in roomPrefabs) {
				if (chunkPrefab.GetSideType(requiredOri) == requiredType) {
					bool leadsToBlocked = false;
					foreach (Orientation blocked in blockedSides) {
						if (chunkPrefab.GetSideType(blocked) != SideType.CLOSED) {
							leadsToBlocked = true;
							break;
						}
					}
					if (!leadsToBlocked) {
						validChunkPrefabs.Add(chunkPrefab);
					}
				}
			}
			return validChunkPrefabs;
		}

	}
}
