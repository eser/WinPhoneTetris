using System.Windows.Controls;
using System.Windows.Shapes;

namespace Tetris {
	public abstract class BaseShape {
		protected Block[,] FBlockArray;
		private bool FIsMoving;
		private int FOffsetX;
		private int FOffsetY;

		public Block[,] BlockArray {
			get {
				return this.FBlockArray;
			}
			internal set {
				this.FBlockArray = value;
			}
		}

		public bool IsMoving {
			get {
				return this.FIsMoving;
			}
			set {
				this.FIsMoving = value;
			}
		}

		public int OffsetX {
			get {
				return this.FOffsetX;
			}
			set {
				this.FOffsetX = value;
			}
		}

		public int OffsetY {
			get {
				return this.FOffsetY;
			}
			set {
				this.FOffsetY = value;
			}
		}

		public BaseShape() {
			this.FOffsetX = 0;
			this.FOffsetY = 0;
		}

		public void AttachToCanvas(Canvas canvas, Canvas sourceCanvas) {
			int _rows = this.FBlockArray.GetLength(0);
			int _cols = this.FBlockArray.GetLength(1);

			for(int _row = 0;_row < _rows;_row++) {
				for(int _col = 0;_col < _cols;_col++) {
					Block _block = this.FBlockArray[_row, _col];
					if(_block == null) {
						continue;
					}

					Rectangle _rectangle = _block.Object;
					if(sourceCanvas != null) {
						sourceCanvas.Children.Remove(_rectangle);
					}
					canvas.Children.Add(_rectangle);
				}
			}
		}

		public void DetachFromCanvas(Canvas canvas) {
			int _rows = this.FBlockArray.GetLength(0);
			int _cols = this.FBlockArray.GetLength(1);

			for(int _row = 0;_row < _rows;_row++) {
				for(int _col = 0;_col < _cols;_col++) {
					Block _block = this.BlockArray[_row, _col];
					if(_block == null) {
						continue;
					}

					canvas.Children.Remove(_block.Object);
				}
			}
		}

		public void UpdatePosition() {
			int _rows = this.FBlockArray.GetLength(0);
			int _cols = this.FBlockArray.GetLength(1);

			for(int _row = 0;_row < _rows;_row++) {
				for(int _col = 0;_col < _cols;_col++) {
					Block _block = this.FBlockArray[_row, _col];
					if(_block == null) {
						continue;
					}

					Rectangle _rectangle = _block.Object;
					Canvas.SetLeft(_rectangle, (this.FOffsetX + _col) * (_rectangle.ActualWidth + 1));
					Canvas.SetTop(_rectangle, (this.FOffsetY + _row) * (_rectangle.ActualHeight + 1));
				}
			}
		}

		public void Rotate() {
			int _newCols = this.FBlockArray.GetLength(0); // y
			int _newRows = this.FBlockArray.GetLength(1); // x
			Block[,] _newBlockArray = new Block[_newRows, _newCols];

			for(int _cols = _newCols - 1;_cols >= 0;_cols--) {
				for(int _rows = 0;_rows < _newRows;_rows++) {
					_newBlockArray[_rows, _newCols - _cols - 1] = this.FBlockArray[_cols, _rows];
				}
			}

			this.FBlockArray = _newBlockArray;
		}
	}
}
