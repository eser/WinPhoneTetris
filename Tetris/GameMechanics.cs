using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace Tetris {
	public class GameMechanics {
		public const int GameBoardWidth = 13;
		public const int GameBoardHeight = 19;
		public const int TimerMilliseconds = 800;
		public const int MaxDifficulty = 10;

		private readonly MainPage FOwner;
		private readonly DispatcherTimer FTimer;
		private readonly List<BaseShape> FShapes;
		private BaseShape FNextShape;
		private Random FRandom;
		private int FPlayerScore;
		private DateTime FDifficultyModifierCounter;
		private int FDifficultyModifier;
		private int FSpeedModifier;
		private bool FIsGameOver;

		public GameMechanics(MainPage owner) {
			this.FOwner = owner;
			this.FTimer = new DispatcherTimer();
			this.FShapes = new List<BaseShape>();
			this.FRandom = new Random();
		}

		public void Init() {
			this.FIsGameOver = false;

			this.FDifficultyModifierCounter = DateTime.UtcNow.AddMinutes(1);
			this.FPlayerScore = 0;
			this.FDifficultyModifier = 0;
			this.FSpeedModifier = 1;

			this.FTimer.Tick += this.FTimer_Tick;
			this.RecalculateTimer();
			this.FTimer.Start();

			this.FNextShape = this.GetRandomShape();
			this.CreateShape();
		}

		public void Reset() {
			foreach(BaseShape _shape in this.FShapes) {
				_shape.DetachFromCanvas(this.FOwner.mainCanvas);
			}

			if(this.FNextShape != null) {
				this.FNextShape.DetachFromCanvas(this.FOwner.nextShape);
			}

			this.FShapes.Clear();
			this.Init();

			this.UpdatePlayerScore();
			this.UpdateDifficulty();
		}

		public BaseShape GetRandomShape() {
			switch(this.FRandom.Next(1, 7)) {
			case 1:
				return new IShape();
			case 2:
				return new JShape();
			case 3:
				return new LShape();
			case 4:
				return new OShape();
			case 5:
				return new ZShape();
			case 6:
				return new TShape();
			}

			return new SShape();
		}

		public void CreateShape() {
			BaseShape _shape = this.FNextShape;
			_shape.IsMoving = true;

			_shape.OffsetX = (GameMechanics.GameBoardWidth - _shape.BlockArray.GetLength(1)) / 2;

			_shape.UpdatePosition();
			_shape.AttachToCanvas(this.FOwner.mainCanvas, this.FOwner.nextShape);

			this.FShapes.Add(_shape);
			this.FNextShape = this.GetRandomShape();

			this.FNextShape.AttachToCanvas(this.FOwner.nextShape, null);
			this.FNextShape.UpdatePosition();
		}

		public BaseShape GetMovingShape() {
			foreach(BaseShape _shape in this.FShapes) {
				if(_shape.IsMoving) {
					return _shape;
				}
			}

			return null;
		}

		private void RecalculateTimer() {
			int _milliseconds = GameMechanics.TimerMilliseconds;
			_milliseconds -= (this.FDifficultyModifier * 60);
			_milliseconds /= this.FSpeedModifier;

			this.FTimer.Interval = TimeSpan.FromMilliseconds(_milliseconds);
		}

		private void FTimer_Tick(object sender, EventArgs e) {
			if(DateTime.UtcNow > this.FDifficultyModifierCounter && this.FDifficultyModifier < GameMechanics.MaxDifficulty) {
				this.FDifficultyModifierCounter = DateTime.UtcNow.AddMinutes(1);
				this.FDifficultyModifier++;
				this.UpdateDifficulty();
				this.RecalculateTimer();
			}

			bool _anyOneMoving = false;

			foreach(BaseShape _shape in this.FShapes) {
				if(_shape.IsMoving == false) {
					continue;
				}

				Conflict _conflict = this.CheckConflicts(_shape, 0, 1);

				if(_conflict == Conflict.Block) {
					// if game's over.
					if(_shape.OffsetY - _shape.BlockArray.GetLength(0) < 0) {
						this.FTimer.Tick -= this.FTimer_Tick;
						this.FTimer.Stop();

						this.FIsGameOver = true;
						break;
					}

					// if just finished its movement.
					_shape.IsMoving = false;
					this.CheckForBlocks();

					return;
				}

				_shape.OffsetY += 1;
				_shape.UpdatePosition();

				_anyOneMoving = true;
			}

			if(this.FIsGameOver) {
				if(MessageBox.Show("Game is over. Starting over?", this.FOwner.Name, MessageBoxButton.OKCancel) == MessageBoxResult.OK) {
					this.Reset();
				}

				return;
			}

			if(!_anyOneMoving) {
				this.CreateShape();
			}
		}

		private Conflict CheckConflicts(BaseShape shape, int xDiff, int yDiff) {
			if(shape.OffsetY > GameMechanics.GameBoardHeight - shape.BlockArray.GetLength(0)) {
				return Conflict.Block;
			}

			foreach(BaseShape _otherShape in this.FShapes) {
				if(_otherShape == shape) {
					continue;
				}

				for(int y1 = 0;y1 < shape.BlockArray.GetLength(0);y1++) {
					for(int x1 = 0;x1 < shape.BlockArray.GetLength(1);x1++) {
						if(shape.BlockArray[y1, x1] == null) {
							continue;
						}

						int _x1actualPos = shape.OffsetX + x1;
						int _y1actualPos = shape.OffsetY + y1;

						for(int y2 = 0;y2 < _otherShape.BlockArray.GetLength(0);y2++) {
							for(int x2 = 0;x2 < _otherShape.BlockArray.GetLength(1);x2++) {
								if(_otherShape.BlockArray[y2, x2] == null) {
									continue;
								}

								int _x2actualPos = _otherShape.OffsetX + x2;
								int _y2actualPos = _otherShape.OffsetY + y2;

								if(_y1actualPos + yDiff == _y2actualPos && _x1actualPos + xDiff == _x2actualPos) {
									return Conflict.Block;
								}
							}
						}
					}
				}
			}

			return Conflict.None;
		}

		private void CheckForBlocks() {
			List<int> _blockNumbers = new List<int>();

			for(int _row = 0;_row <= GameMechanics.GameBoardHeight;_row++) {
				bool _allBlock = true;

				for(int _col = 0;_col <= GameMechanics.GameBoardWidth;_col++) {
					bool _exist = false;

					foreach(BaseShape _shape in this.FShapes) {
						if(_shape.OffsetY > _row || _shape.OffsetX > _col) {
							continue;
						}

						int _blockY = _row - _shape.OffsetY;
						int _blockX = _col - _shape.OffsetX;

						if(_blockY > _shape.BlockArray.GetLength(0) - 1 || _blockX > _shape.BlockArray.GetLength(1) - 1) {
							continue;
						}

						Block _block = _shape.BlockArray[_blockY, _blockX];
						if(_block == null) {
							continue;
						}

						_block.Object.Fill.Opacity = 0.6f;
						_exist = true;
					}

					if(!_exist) {
						_allBlock = false;
						continue;
					}
				}

				if(_allBlock) {
					_blockNumbers.Add(_row);
					this.FPlayerScore++;
				}
			}

			this.RemoveBlocks(_blockNumbers);
			this.UpdatePlayerScore();
		}

		private void RemoveBlocks(List<int> blocks) {
			foreach(int _row in blocks) {
				foreach(BaseShape _shape in this.FShapes) {
					if(_shape.OffsetY > _row) {
						continue;
					}

					int _blockY = _row - _shape.OffsetY;
					_shape.OffsetY += 1;
					_shape.UpdatePosition();

					int _rows = _shape.BlockArray.GetLength(0);
					if(_blockY > _rows - 1) {
						continue;
					}

					int _cols = _shape.BlockArray.GetLength(1);

					Block[,] _newArray = new Block[_rows - 1, _cols];
					int _currentRow = 0;
					for(int _row2 = 0;_row2 < _rows;_row2++) {
						for(int _col = 0;_col < _cols;_col++) {
							Block _block = _shape.BlockArray[_row2, _col];
							if(_blockY == _row2) {
								if(_block != null && _block.Object != null) {
									this.FOwner.mainCanvas.Children.Remove(_block.Object);
								}
								continue;
							}

							_newArray[_currentRow, _col] = _block;
						}
						if(_blockY != _row2) {
							_currentRow++;
						}
					}
				}
			}
		}

		private void UpdatePlayerScore() {
			this.FOwner.Score.Text = this.FPlayerScore.ToString();
		}

		private void UpdateDifficulty() {
			this.FOwner.Difficulty.Text = this.FDifficultyModifier.ToString();
		}

		public void MoveLeft() {
			BaseShape _shape = this.GetMovingShape();
			if(_shape == null) {
				return;
			}

			if(_shape.OffsetX <= 0) {
				return;
			}

			if(this.CheckConflicts(_shape, -1, 0) != Conflict.None) {
				return;
			}
			_shape.OffsetX -= 1;
			_shape.UpdatePosition();
		}

		public void MoveRight() {
			BaseShape _shape = this.GetMovingShape();
			if(_shape == null) {
				return;
			}

			if(_shape.OffsetX > GameMechanics.GameBoardWidth - _shape.BlockArray.GetLength(1)) {
				return;
			}

			if(this.CheckConflicts(_shape, 1, 0) != Conflict.None) {
				return;
			}
	
			_shape.OffsetX += 1;
			_shape.UpdatePosition();
		}

		public void MoveDown_Up() {
			this.FSpeedModifier = 1;
			this.RecalculateTimer();
		}

		public void MoveDown_Down() {
			this.FSpeedModifier = 50;
			this.RecalculateTimer();
		}

		public void Rotate() {
			BaseShape _shape = this.GetMovingShape();
			if(_shape == null) {
				return;
			}

			_shape.Rotate();
		}
	}
}
