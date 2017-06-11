using System.Windows.Media;

namespace Tetris {
	public class JShape : BaseShape {
		public JShape() : base() {
			SolidColorBrush _brush = new SolidColorBrush(Colors.Green);

			this.FBlockArray = new Block[,] {
				{ null, new Block(_brush) },
				{ null, new Block(_brush) },
				{ new Block(_brush), new Block(_brush) }
			};
		}
	}
}
