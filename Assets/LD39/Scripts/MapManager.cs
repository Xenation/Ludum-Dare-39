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

		public List<Orientation> GetOpenSides() {
			List<Orientation> oris = new List<Orientation>();
			if (top != SideType.CLOSED) {
				oris.Add(Orientation.TOP);
			}
			if (right != SideType.CLOSED) {
				oris.Add(Orientation.RIGHT);
			}
			if (bottom != SideType.CLOSED) {
				oris.Add(Orientation.BOTTOM);
			}
			if (left != SideType.CLOSED) {
				oris.Add(Orientation.LEFT);
			}
			return oris;
		}

		public bool isSideOpen(Orientation ori) {
			return GetSideType(ori) != SideType.CLOSED;
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

		private List<Vector2i> mainPath;

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
			GenerateOptionnalPaths();
		}

		private void GenerateMainPath(int mainPathLength) {
			mainPath = new List<Vector2i>();
			MapChunk curChunk = MapChunk.CreateMapChunk(startingRoom);
			Grid.SetChunk(new Vector2i(0, 0), curChunk);
			mainPath.Add(curChunk.FakePos);
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
				mainPath.Add(curChunk.FakePos);
			}
			nextSide = curChunk.GetRandomOpenUnusedSide();
			curChunk = MapChunk.CreateMapChunk(endingRoom);
			Grid.SetChunk(nextSide.GetAdjacentPos(), curChunk);
			mainPath.Add(curChunk.FakePos);
		}

		private void GenerateOptionnalPaths() {
			foreach (Vector2i mainPos in mainPath) {
				MapChunk mainChunk = Grid[mainPos.x, mainPos.z];
				List<Side> openUnusedSides = mainChunk.GetAllOpenSides();
				if (openUnusedSides.Count == 0) {
					continue;
				}
				foreach (Side unusedSide in openUnusedSides) {
					List<Orientation> adjOpen = null;
					if (unusedSide.adjacentChunk != null) {
						adjOpen = unusedSide.adjacentChunk.GetAllOpenSidesOrientations();
					} else {
						adjOpen = new List<Orientation>();
					}
					adjOpen.Add(unusedSide.Orient.GetOposite());
					Grid.SetChunk(unusedSide.GetAdjacentPos(), MapChunk.CreateMapChunk(GetPrefabMatchingExactly(adjOpen)));
				}
			}
		}

		public MapChunkPrefab GetPrefabMatchingExactly(List<Orientation> openSides) {
			List<Orientation> closedSides = new List<Orientation>();
			closedSides.Add(Orientation.TOP);
			closedSides.Add(Orientation.RIGHT);
			closedSides.Add(Orientation.BOTTOM);
			closedSides.Add(Orientation.LEFT);
			foreach (Orientation side in openSides) {
				closedSides.Remove(side);
			}
			foreach (MapChunkPrefab prefab in roomPrefabs) {
				bool matches = true;
				foreach (Orientation requiredOpen in openSides) {
					if (!prefab.isSideOpen(requiredOpen)) {
						matches = false;
						break;
					}
				}
				foreach (Orientation requiredClosed in closedSides) {
					if (prefab.isSideOpen(requiredClosed)) {
						matches = false;
						break;
					}
				}
				if (!matches) {
					continue;
				} else {
					return prefab;
				}
			}
			return null;
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
