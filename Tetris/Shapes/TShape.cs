using System.Windows.Media;

namespace Tetris {
	public class TShape : BaseShape {
		public TShape() : base() {
			SolidColorBrush _brush = new SolidColorBrush(Colors.Red);

			this.FBlockArray = new Block[,] {
				{ null, new Block(_brush), null },
				{ new Block(_brush), new Block(_brush), new Block(_brush) }
			};
		}
	}
}
