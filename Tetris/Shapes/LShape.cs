using System.Windows.Media;

namespace Tetris {
	public class LShape : BaseShape {
		public LShape() : base() {
			SolidColorBrush _brush = new SolidColorBrush(Colors.Blue);

			this.FBlockArray = new Block[,] {
				{ new Block(_brush), null },
				{ new Block(_brush), null },
				{ new Block(_brush), new Block(_brush) }
			};
		}
	}
}
