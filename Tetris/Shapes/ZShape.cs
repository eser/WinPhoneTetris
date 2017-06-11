using System.Windows.Media;

namespace Tetris {
	public class ZShape : BaseShape {
		public ZShape() : base() {
			SolidColorBrush _brush = new SolidColorBrush(Colors.Cyan);

			this.FBlockArray = new Block[,] {
				{ new Block(_brush), new Block(_brush), null },
				{ null, new Block(_brush), new Block(_brush) }
			};
		}
	}
}
