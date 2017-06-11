using System.Windows.Controls;
using System.Windows.Input;

namespace Tetris {
	public partial class MainPage : UserControl {
		private GameMechanics FGameMechanics;

		public MainPage() {
			this.InitializeComponent();

			this.KeyDown += this.MainPage_KeyDown;
			this.KeyUp += this.MainPage_KeyUp;

			this.FGameMechanics = new GameMechanics(this);
			this.FGameMechanics.Init();
		}

		private void MainPage_KeyDown(object sender, KeyEventArgs e) {
			switch(e.Key) {
			case Key.Space:
			case Key.Up:
				this.FGameMechanics.Rotate();
				e.Handled = true;
				break;
			case Key.Left:
				this.FGameMechanics.MoveLeft();
				e.Handled = true;
				break;
			case Key.Right:
				this.FGameMechanics.MoveRight();
				e.Handled = true;
				break;
			case Key.Down:
				this.FGameMechanics.MoveDown_Down();
				e.Handled = true;
				break;
			}
		}

		private void MainPage_KeyUp(object sender, KeyEventArgs e) {
			switch(e.Key) {
			case Key.Down:
				this.FGameMechanics.MoveDown_Up();
				e.Handled = true;
				break;
			}
		}
	}
}
