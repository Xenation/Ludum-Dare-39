namespace LD39 {
	public enum Orientation {
		TOP,
		RIGHT,
		BOTTOM,
		LEFT
	}

	public static class OrientationExt {

		public static Orientation GetOposite(this Orientation ori) {
			switch (ori) {
				case Orientation.TOP:
					return Orientation.BOTTOM;
				case Orientation.RIGHT:
					return Orientation.LEFT;
				case Orientation.BOTTOM:
					return Orientation.TOP;
				case Orientation.LEFT:
					return Orientation.RIGHT;
				default:
					return Orientation.TOP;
			}
		}

	}

	public class Side {

		public Orientation Orient { get; private set; }
		public SideType Type { get; private set; }

		public MapChunk ParentChunk { get; private set; }
		public MapChunk adjacentChunk;

		public Side(MapChunk parentChunk, Orientation ori, SideType t) {
			ParentChunk = parentChunk;
			Orient = ori;
			Type = t;
		}

		public Vector2i GetAdjacentPos() {
			if (adjacentChunk == null) {
				switch (Orient) {
					case Orientation.TOP:
						return new Vector2i(ParentChunk.FakePos.x, ParentChunk.FakePos.z + 1);
					case Orientation.RIGHT:
						return new Vector2i(ParentChunk.FakePos.x + 1, ParentChunk.FakePos.z);
					case Orientation.BOTTOM:
						return new Vector2i(ParentChunk.FakePos.x, ParentChunk.FakePos.z - 1);
					case Orientation.LEFT:
						return new Vector2i(ParentChunk.FakePos.x - 1, ParentChunk.FakePos.z);
					default:
						return null;
				}
			} else {
				return adjacentChunk.FakePos;
			}
		}

	}
}
