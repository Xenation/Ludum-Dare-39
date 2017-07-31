using System.Collections.Generic;
using UnityEngine;

namespace LD39 {
	public enum MapChunkPrefabType {
		DEAD_END,
		CORNER,
		LINE,
		TJUNCTION,
		CROSSROADS
	}

	[System.Serializable]
	public class MapChunkPrefab {

		public MapChunkPrefabType type;

		public SideType initTop;
		public SideType initRight;
		public SideType initBottom;
		public SideType initLeft;

		#region props
		public SideType top {
			get {
				return sides[Orientation.TOP];
			}
			set {
				sides[Orientation.TOP] = value;
			}
		}
		public SideType right {
			get {
				return sides[Orientation.RIGHT];
			}
			set {
				sides[Orientation.RIGHT] = value;
			}
		}
		public SideType bottom {
			get {
				return sides[Orientation.BOTTOM];
			}
			set {
				sides[Orientation.BOTTOM] = value;
			}
		}
		public SideType left {
			get {
				return sides[Orientation.LEFT];
			}
			set {
				sides[Orientation.LEFT] = value;
			}
		}
		#endregion

		public Dictionary<Orientation, SideType> sides;

		public GameObject prefab;

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

		public MapChunkPrefab() {
			sides = new Dictionary<Orientation, SideType>();
			sides.Add(Orientation.TOP, SideType.CLOSED);
			sides.Add(Orientation.RIGHT, SideType.CLOSED);
			sides.Add(Orientation.BOTTOM, SideType.CLOSED);
			sides.Add(Orientation.LEFT, SideType.CLOSED);
		}

		public void Init() {
			sides = new Dictionary<Orientation, SideType>();
			sides.Add(Orientation.TOP, initTop);
			sides.Add(Orientation.RIGHT, initRight);
			sides.Add(Orientation.BOTTOM, initBottom);
			sides.Add(Orientation.LEFT, initLeft);
		}

		public SideType GetSideType(Orientation ori) {
			return sides[ori];
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

		public List<MapChunkPrefab> GenerateRotatedVersions() {
			List<MapChunkPrefab> rotatedPrefabs = new List<MapChunkPrefab>();
			switch (type) {
				case MapChunkPrefabType.DEAD_END:
					if (top != SideType.CLOSED) {
						MapChunkPrefab pr = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(pr);
						MapChunkPrefab pb = Get90ClockwiseRotatedVersion(pr);
						rotatedPrefabs.Add(pb);
						MapChunkPrefab pl = Get90ClockwiseRotatedVersion(pb);
						rotatedPrefabs.Add(pl);
					} else if (right != SideType.CLOSED) {
						MapChunkPrefab pb = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(pb);
						MapChunkPrefab pl = Get90ClockwiseRotatedVersion(pb);
						rotatedPrefabs.Add(pl);
						MapChunkPrefab pt = Get90ClockwiseRotatedVersion(pl);
						rotatedPrefabs.Add(pt);
					} else if (bottom != SideType.CLOSED) {
						MapChunkPrefab pl = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(pl);
						MapChunkPrefab pt = Get90ClockwiseRotatedVersion(pl);
						rotatedPrefabs.Add(pt);
						MapChunkPrefab pr = Get90ClockwiseRotatedVersion(pt);
						rotatedPrefabs.Add(pr);
					} else if (left != SideType.CLOSED) {
						MapChunkPrefab pt = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(pt);
						MapChunkPrefab pr = Get90ClockwiseRotatedVersion(pt);
						rotatedPrefabs.Add(pr);
						MapChunkPrefab pb = Get90ClockwiseRotatedVersion(pr);
						rotatedPrefabs.Add(pb);
					}
					break;
				case MapChunkPrefabType.CORNER:
					if (top != SideType.CLOSED && right != SideType.CLOSED) {
						MapChunkPrefab prb = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(prb);
						MapChunkPrefab pbl = Get90ClockwiseRotatedVersion(prb);
						rotatedPrefabs.Add(pbl);
						MapChunkPrefab plt = Get90ClockwiseRotatedVersion(pbl);
						rotatedPrefabs.Add(plt);
					} else if (right != SideType.CLOSED && bottom != SideType.CLOSED) {
						MapChunkPrefab pbl = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(pbl);
						MapChunkPrefab plt = Get90ClockwiseRotatedVersion(pbl);
						rotatedPrefabs.Add(plt);
						MapChunkPrefab ptr = Get90ClockwiseRotatedVersion(plt);
						rotatedPrefabs.Add(ptr);
					} else if (bottom != SideType.CLOSED && left != SideType.CLOSED) {
						MapChunkPrefab plt = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(plt);
						MapChunkPrefab ptr = Get90ClockwiseRotatedVersion(plt);
						rotatedPrefabs.Add(ptr);
						MapChunkPrefab prb = Get90ClockwiseRotatedVersion(ptr);
						rotatedPrefabs.Add(prb);
					} else if (left != SideType.CLOSED && top != SideType.CLOSED) {
						MapChunkPrefab ptr = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(ptr);
						MapChunkPrefab prb = Get90ClockwiseRotatedVersion(ptr);
						rotatedPrefabs.Add(prb);
						MapChunkPrefab pbl = Get90ClockwiseRotatedVersion(prb);
						rotatedPrefabs.Add(pbl);
					}
					break;
				case MapChunkPrefabType.LINE:
					if (top != SideType.CLOSED && bottom != SideType.CLOSED) {
						MapChunkPrefab prl = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(prl);
					} else if (right != SideType.CLOSED && left != SideType.CLOSED) {
						MapChunkPrefab ptb = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(ptb);
					}
					break;
				case MapChunkPrefabType.TJUNCTION:
					if (top == SideType.CLOSED) {
						MapChunkPrefab pblt = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(pblt);
						MapChunkPrefab pltr = Get90ClockwiseRotatedVersion(pblt);
						rotatedPrefabs.Add(pltr);
						MapChunkPrefab ptrb = Get90ClockwiseRotatedVersion(pltr);
						rotatedPrefabs.Add(ptrb);
					} else if (right == SideType.CLOSED) {
						MapChunkPrefab pltr = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(pltr);
						MapChunkPrefab ptrb = Get90ClockwiseRotatedVersion(pltr);
						rotatedPrefabs.Add(ptrb);
						MapChunkPrefab pblt = Get90ClockwiseRotatedVersion(ptrb);
						rotatedPrefabs.Add(pblt);
					} else if (bottom == SideType.CLOSED) {
						MapChunkPrefab ptrb = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(ptrb);
						MapChunkPrefab pblt = Get90ClockwiseRotatedVersion(ptrb);
						rotatedPrefabs.Add(pblt);
						MapChunkPrefab pltr = Get90ClockwiseRotatedVersion(pblt);
						rotatedPrefabs.Add(pltr);
					} else if (left == SideType.CLOSED) {
						MapChunkPrefab pblt = Get90ClockwiseRotatedVersion(this);
						rotatedPrefabs.Add(pblt);
						MapChunkPrefab pltr = Get90ClockwiseRotatedVersion(pblt);
						rotatedPrefabs.Add(pltr);
						MapChunkPrefab ptrb = Get90ClockwiseRotatedVersion(pltr);
						rotatedPrefabs.Add(ptrb);
					}
					break;
				case MapChunkPrefabType.CROSSROADS:
					break;
			}
			return rotatedPrefabs;
		}

		public static MapChunkPrefab Get90ClockwiseRotatedVersion(MapChunkPrefab prefab) {
			GameObject go = GameObject.Instantiate(prefab.prefab);
			go.transform.Rotate(new Vector3(0, 90, 0));
			go.transform.SetParent(MapManager.I.rotatedRoot);
			MapChunkPrefab p = new MapChunkPrefab();
			p.prefab = go;
			foreach (Orientation ori in prefab.sides.Keys) {
				p.sides[ori.GetNextClockwise()] = prefab.sides[ori];
			}
			p.initTop = p.sides[Orientation.TOP];
			p.initRight = p.sides[Orientation.RIGHT];
			p.initBottom = p.sides[Orientation.BOTTOM];
			p.initLeft = p.sides[Orientation.LEFT];
			p.type = prefab.type;
			return p;
		}

	}
}
