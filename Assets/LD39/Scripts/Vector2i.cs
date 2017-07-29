namespace LD39 {
	public class Vector2i {

		public int x;
		public int z;

		public Vector2i(int x, int z) {
			this.x = x;
			this.z = z;
		}

		public Vector2i(int v) : this(v, v) { }

		public Vector2i() : this(0, 0) { }

		public override int GetHashCode() {
			return x + z * 666;
		}

		public static Vector2i operator +(Vector2i v1, Vector2i v2) {
			return new Vector2i(v1.x + v2.x, v1.z + v2.x);
		}

		public static Vector2i operator /(Vector2i v1, Vector2i v2) {
			return new Vector2i(v1.x / v2.x, v1.z / v2.z);
		}

		public static Vector2i operator /(Vector2i v, float d) {
			return new Vector2i();
		}

	}
}
