using System.Windows.Media;

namespace Tetris {
	public class OShape : BaseShape {
		public OShape() : base() {
			SolidColorBrush _brush = new SolidColorBrush(Colors.Yellow);

			this.FBlockArray = new Block[,] {
				{ new Block(_brush), new Block(_brush) },
				{ new Block(_brush), new Block(_brush) }
			};
		}
	}
}
