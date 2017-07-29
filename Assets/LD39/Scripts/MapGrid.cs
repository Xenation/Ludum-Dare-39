using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD39 {
	public class MapGrid {

		private Vector2i middle;

		public MapChunk[,] chunks;
		private float realChunkSize;

		private int index;

		public MapGrid(int sizeX, int sizeZ, float realSize) {
			if (sizeX % 2 == 0) {
				sizeX++;
			}
			if (sizeZ % 2 == 0) {
				sizeZ++;
			}
			middle = new Vector2i(sizeX / 2, sizeZ / 2);
			chunks = new MapChunk[sizeX, sizeZ];
			realChunkSize = realSize;
		}

		public void SetChunk(Vector2i fakeGridPos, MapChunk chunk) {
			Vector2i gridPos = FakeToRealGridPos(fakeGridPos);
			chunk.FakePos = fakeGridPos;
			if (chunks[gridPos.x, gridPos.z] != null) {
				GameObject.Destroy(chunks[gridPos.x, gridPos.z].gameObject);
			}
			chunks[gridPos.x, gridPos.z] = chunk;
			chunk.transform.position = new Vector3(realChunkSize * fakeGridPos.x, 0, realChunkSize * fakeGridPos.z);
			chunk.name = "Chunk " + (index++);
			UpdateAdjacents(gridPos);
		}

		private void UpdateAdjacents(Vector2i gridPos) {
			if (chunks[gridPos.x, gridPos.z + 1] != null) {
				chunks[gridPos.x, gridPos.z + 1].Bottom.adjacentChunk = chunks[gridPos.x, gridPos.z];
				chunks[gridPos.x, gridPos.z].Top.adjacentChunk = chunks[gridPos.x, gridPos.z + 1];
			}
			if (chunks[gridPos.x + 1, gridPos.z] != null) {
				chunks[gridPos.x + 1, gridPos.z].Left.adjacentChunk = chunks[gridPos.x, gridPos.z];
				chunks[gridPos.x, gridPos.z].Right.adjacentChunk = chunks[gridPos.x + 1, gridPos.z];
			}
			if (chunks[gridPos.x, gridPos.z - 1] != null) {
				chunks[gridPos.x, gridPos.z - 1].Top.adjacentChunk = chunks[gridPos.x, gridPos.z];
				chunks[gridPos.x, gridPos.z].Bottom.adjacentChunk = chunks[gridPos.x, gridPos.z - 1];
			}
			if (chunks[gridPos.x - 1, gridPos.z] != null) {
				chunks[gridPos.x - 1, gridPos.z].Right.adjacentChunk = chunks[gridPos.x, gridPos.z];
				chunks[gridPos.x, gridPos.z].Left.adjacentChunk = chunks[gridPos.x - 1, gridPos.z];
			}
		}

		public Orientation[] GetAdjacentSidesOccupied(Vector2i fakeGridPos) {
			List<Orientation> occupied = new List<Orientation>(4);
			Vector2i gridPos = FakeToRealGridPos(fakeGridPos);
			if (chunks[gridPos.x, gridPos.z + 1] != null) { // TOP
				occupied.Add(Orientation.TOP);
			}
			if (chunks[gridPos.x + 1, gridPos.z] != null) { // RIGHT
				occupied.Add(Orientation.RIGHT);
			}
			if (chunks[gridPos.x, gridPos.z - 1] != null) { // BOTTOM
				occupied.Add(Orientation.BOTTOM);
			}
			if (chunks[gridPos.x - 1, gridPos.z] != null) { // LEFT
				occupied.Add(Orientation.LEFT);
			}
			return occupied.ToArray();
		}

		public Orientation[] GetAdjacentSidesOccupied(Vector2i fakeGridPos, Orientation ignore) {
			List<Orientation> occupied = new List<Orientation>(4);
			Vector2i gridPos = FakeToRealGridPos(fakeGridPos);
			if (ignore != Orientation.TOP && chunks[gridPos.x, gridPos.z + 1] != null) { // TOP
				occupied.Add(Orientation.TOP);
			}
			if (ignore != Orientation.RIGHT && chunks[gridPos.x + 1, gridPos.z] != null) { // RIGHT
				occupied.Add(Orientation.RIGHT);
			}
			if (ignore != Orientation.BOTTOM && chunks[gridPos.x, gridPos.z - 1] != null) { // BOTTOM
				occupied.Add(Orientation.BOTTOM);
			}
			if (ignore != Orientation.LEFT && chunks[gridPos.x - 1, gridPos.z] != null) { // LEFT
				occupied.Add(Orientation.LEFT);
			}
			return occupied.ToArray();
		}

		public Orientation[] GetAdjacentSidesLeadingToTile(Vector2i fakeGridPos, Orientation ignore) {
			List<Orientation> occupied = new List<Orientation>(4);
			Vector2i gridPos = FakeToRealGridPos(fakeGridPos);
			if (ignore != Orientation.TOP && chunks[gridPos.x, gridPos.z + 1] != null && chunks[gridPos.x, gridPos.z + 1].Bottom.Type != SideType.CLOSED) { // TOP
				occupied.Add(Orientation.TOP);
			}
			if (ignore != Orientation.RIGHT && chunks[gridPos.x + 1, gridPos.z] != null && chunks[gridPos.x + 1, gridPos.z].Left.Type != SideType.CLOSED) { // RIGHT
				occupied.Add(Orientation.RIGHT);
			}
			if (ignore != Orientation.BOTTOM && chunks[gridPos.x, gridPos.z - 1] != null && chunks[gridPos.x, gridPos.z - 1].Top.Type != SideType.CLOSED) { // BOTTOM
				occupied.Add(Orientation.BOTTOM);
			}
			if (ignore != Orientation.LEFT && chunks[gridPos.x - 1, gridPos.z] != null && chunks[gridPos.x - 1, gridPos.z].Right.Type != SideType.CLOSED) { // LEFT
				occupied.Add(Orientation.LEFT);
			}
			return occupied.ToArray();
		}

		public Vector2i FakeToRealGridPos(Vector2i fakePos) {
			return fakePos + middle;
		}

		public MapChunk this[int fakeX, int fakeZ] {
			get {
				return chunks[fakeX + middle.x, fakeZ + middle.z];
			}
		}

	}
}
