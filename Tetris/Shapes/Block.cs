using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris {
	public class Block {
		public const int BlockWidth = 25;
		public const int BlockHeight = 25;

		private readonly Rectangle FObject;

		public Rectangle Object {
			get {
				return this.FObject;
			}
		}

		public Block(SolidColorBrush brush) {
			this.FObject = new Rectangle();
			this.FObject.Width = Block.BlockWidth;
			this.FObject.Height = Block.BlockHeight;
			this.FObject.Fill = brush;
		}
	}
}
