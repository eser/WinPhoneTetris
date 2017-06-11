using System.Windows.Media;

namespace Tetris {
	public class SShape : BaseShape {
		public SShape() : base() {
			SolidColorBrush _brush = new SolidColorBrush(Colors.Purple);

			this.FBlockArray = new Block[,] {
				{ null, new Block(_brush), new Block(_brush) },
				{ new Block(_brush), new Block(_brush), null }
			};
		}
	}
}
