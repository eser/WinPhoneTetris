using System.Windows.Media;

namespace Tetris {
	public class IShape : BaseShape {
		public IShape() : base() {
			SolidColorBrush _brush = new SolidColorBrush(Colors.LightGray);

			this.FBlockArray = new Block[,] {
				{ new Block(_brush) },
				{ new Block(_brush) },
				{ new Block(_brush) },
				{ new Block(_brush) }
			};
		}
	}
}
