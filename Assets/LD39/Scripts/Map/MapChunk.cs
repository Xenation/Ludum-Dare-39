using System.Collections.Generic;
using UnityEngine;

namespace LD39 {
	public class MapChunk : MonoBehaviour {

		public Side Top { get; private set; }
		public Side Right { get; private set; }
		public Side Bottom { get; private set; }
		public Side Left { get; private set; }
		
		public Vector2i FakePos { get; set; }

		public int OpenCount {
			get {
				int count = 0;
				if (Top.Type != SideType.CLOSED) {
					count++;
				}
				if (Right.Type != SideType.CLOSED) {
					count++;
				}
				if (Bottom.Type != SideType.CLOSED) {
					count++;
				}
				if (Left.Type != SideType.CLOSED) {
					count++;
				}
				return count;
			}
		}

		public static MapChunk CreateMapChunk(MapChunkPrefab prefab) {
			GameObject go = Instantiate<GameObject>(prefab.prefab, MapManager.I.mapRoot);
			MapChunk mapChunk = go.AddComponent<MapChunk>();
			mapChunk.Top = new Side(mapChunk, Orientation.TOP, prefab.top);
			mapChunk.Left = new Side(mapChunk, Orientation.LEFT, prefab.left);
			mapChunk.Bottom = new Side(mapChunk, Orientation.BOTTOM, prefab.bottom);
			mapChunk.Right = new Side(mapChunk, Orientation.RIGHT, prefab.right);
			return mapChunk;
		}

		private MapChunk() {

		}

		public Side GetRandomOpenUnusedSide() {
			Side[] openUnusedSides = new Side[4];
			int count = 0;
			if (Top.Type != SideType.CLOSED && Top.adjacentChunk == null) {
				openUnusedSides[count++] = Top;
			}
			if (Right.Type != SideType.CLOSED && Right.adjacentChunk == null) {
				openUnusedSides[count++] = Right;
			}
			if (Bottom.Type != SideType.CLOSED && Bottom.adjacentChunk == null) {
				openUnusedSides[count++] = Bottom;
			}
			if (Left.Type != SideType.CLOSED && Left.adjacentChunk == null) {
				openUnusedSides[count++] = Left;
			}
			return openUnusedSides[Random.Range(0, count)];
		}

		public List<Side> GetAllOpenUnusedSides() {
			List<Side> openUnusedSides = new List<Side>(4);
			if (Top.Type != SideType.CLOSED && (Top.adjacentChunk == null || Top.adjacentChunk.Bottom.Type == SideType.CLOSED)) {
				openUnusedSides.Add(Top);
			}
			if (Right.Type != SideType.CLOSED && (Right.adjacentChunk == null || Right.adjacentChunk.Left.Type == SideType.CLOSED)) {
				openUnusedSides.Add(Right);
			}
			if (Bottom.Type != SideType.CLOSED && (Bottom.adjacentChunk == null || Bottom.adjacentChunk.Top.Type == SideType.CLOSED)) {
				openUnusedSides.Add(Bottom);
			}
			if (Left.Type != SideType.CLOSED && (Left.adjacentChunk == null || Left.adjacentChunk.Right.Type == SideType.CLOSED)) {
				openUnusedSides.Add(Left);
			}
			return openUnusedSides;
		}

		public List<Side> GetAllOpenSides() {
			List<Side> openSides = new List<Side>();
			if (Top.Type != SideType.CLOSED) {
				openSides.Add(Top);
			}
			if (Right.Type != SideType.CLOSED) {
				openSides.Add(Right);
			}
			if (Bottom.Type != SideType.CLOSED) {
				openSides.Add(Bottom);
			}
			if (Left.Type != SideType.CLOSED) {
				openSides.Add(Left);
			}
			return openSides;
		}

		public List<Orientation> GetAllOpenSidesOrientations() {
			List<Orientation> openSides = new List<Orientation>();
			if (Top.Type != SideType.CLOSED) {
				openSides.Add(Orientation.TOP);
			}
			if (Right.Type != SideType.CLOSED) {
				openSides.Add(Orientation.RIGHT);
			}
			if (Bottom.Type != SideType.CLOSED) {
				openSides.Add(Orientation.BOTTOM);
			}
			if (Left.Type != SideType.CLOSED) {
				openSides.Add(Orientation.LEFT);
			}
			return openSides;
		}

	}
}
