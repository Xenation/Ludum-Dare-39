using UnityEngine;

namespace LD39 {
	public class MapGrid {

		private Vector2i middle;

		public MapChunk[,] chunks;
		private float realChunkSize;

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
			chunks[gridPos.x, gridPos.z] = chunk;
			chunk.transform.position = new Vector3(realChunkSize * fakeGridPos.x, 0, realChunkSize * fakeGridPos.z);
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
