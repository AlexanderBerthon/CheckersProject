using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * REMEMBER OFF BY 1 ERRORS: Anytime you reference flowLayout1, layout will be -1. button = 10 layout thinks it's 9 
 * but they will modify the SAME button. just different reference. keep that in mind!
	raw capture values - AI (top to bottom)
	//7, 9 - adjacent(left, right)
	//14, 18 - target(left, right)
 */

namespace CheckersProject {
    public partial class Form1 : Form {
		//Global Variables! I'm sorry!!
		Button selected;
		Boolean playerTurn;
		List<Button> AIPieces;
		Button[] temp;


		public Form1() {
			InitializeComponent();
			selected = null;
			playerTurn = true;
			AIPieces = new List<Button>();

			temp = new Button[64];
			flowLayoutPanel1.Controls.CopyTo(temp, 0);
		}
		
		private void button_Click(object sender, EventArgs e) {
			if (playerTurn) {
				Button button = (Button)sender;
				if (selected == null) {
					if (button.BackgroundImage == null || button.Tag == "RCoin" || button.Tag == "RKing") {
						label1.Text = "Invalid Selection";
					}
					else {
						selected = button;
						button.FlatStyle = FlatStyle.Standard;
					}
				}
				else if (selected == button) { //undo first click, misclick, etc. 
					selected.FlatStyle = FlatStyle.Flat;
					selected = null;
				}
				else {
					Boolean validMove = false;
					//check move for left column exception
					if (selected.TabIndex % 8 == 1) {
						//normal move check
						if (selected.TabIndex == button.TabIndex + 7 && button.BackgroundImage == null) {
							label1.Text = "left column, good move right";
							validMove = true;
						}
						//capture move check
						else if (selected.TabIndex == (button.TabIndex + 14) && button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex - 8].Tag == "RCoin") {
								label1.Text = "left column, good capture right";
								flowLayoutPanel1.Controls[selected.TabIndex - 8].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex - 8].Tag = null;
								//player1Points++
								validMove = true;
							}
						}
						else {
							validMove = false;
						}
					}
					//check move for right column exception
					else if (selected.TabIndex % 8 == 0) {
						//normal move check
						if (selected.TabIndex == button.TabIndex + 9 && button.BackgroundImage == null) {
							label1.Text = "right column, good move left";
							validMove = true;
						}
						//capture move check
						else if (selected.TabIndex == (button.TabIndex + 18) && button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex - 10].Tag == "RCoin") {
								label1.Text = "right column, good capture left";
								flowLayoutPanel1.Controls[selected.TabIndex - 10].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex - 10].Tag = null;
								//player1Points++
								validMove = true;
							}
						}
						else {
							validMove = false;
						}
					}
					//check normal move for all other cells
					else if (selected.TabIndex == (button.TabIndex + 7) || selected.TabIndex == (button.TabIndex + 9)) {
						if (button.BackgroundImage == null) {
							validMove = true;
						}
						else {
							validMove = false;
						}
					}
					//check capture move up-left
					else if (selected.TabIndex == (button.TabIndex + 18) && button.BackgroundImage == null) {//
						if (flowLayoutPanel1.Controls[selected.TabIndex - 10].Tag == "RCoin") {
							label1.Text = "good capture left";
							flowLayoutPanel1.Controls[selected.TabIndex - 10].BackgroundImage = null;
							flowLayoutPanel1.Controls[selected.TabIndex - 10].Tag = null;
							//player1Points++
							validMove = true;
						}
					}
					//check capture move up-right
					else if (selected.TabIndex == (button.TabIndex + 14) && button.BackgroundImage == null) {
						if (flowLayoutPanel1.Controls[selected.TabIndex - 8].Tag == "RCoin") {
							label1.Text = "good capture right";
							flowLayoutPanel1.Controls[selected.TabIndex - 8].BackgroundImage = null;
							flowLayoutPanel1.Controls[selected.TabIndex - 8].Tag = null;
							//player1Points++
							validMove = true;
						}
					}
					else {
						validMove = false;
					}
					if (validMove) {
						button.BackgroundImage = selected.BackgroundImage;
						button.Tag = selected.Tag;
						selected.FlatStyle = FlatStyle.Flat;
						selected.BackgroundImage = null;
						selected.Tag = null;
						selected = null;
						playerTurn = false;
						AITurn();
						//end turn
					}
					else {
						label1.Text = "Invalid Move";
					}
				}
			}
		}
	
		public void AITurn() {
			this.Refresh(); //must repaint manually, since this is called within a single click action
			System.Threading.Thread.Sleep(1000);

			flowLayoutPanel1.Controls.CopyTo(temp, 0); //does this override previous entries? or cause an exception??
			foreach (Button button in temp) {
				if (button.Tag == "RCoin") {
					AIPieces.Add(button);
				}
			}

			//is there a way to condense this? pass in arguments into a helper method? 
			try {
				foreach (Button button in AIPieces) {
					//if piece is in column 1 or 2, can only attempt right-sided capture.
					if ((button.TabIndex % 8 == 1 ||button.TabIndex % 8 == 2)
					&& flowLayoutPanel1.Controls[button.TabIndex + 17].BackgroundImage == null
					&& flowLayoutPanel1.Controls[button.TabIndex + 8].Tag == "BCoin") {
						label1.Text = "good-capture-right (exception)";
						flowLayoutPanel1.Controls[button.TabIndex + 8].BackgroundImage = null;
						flowLayoutPanel1.Controls[button.TabIndex + 8].Tag = null;
						//AIPoints++
						flowLayoutPanel1.Controls[button.TabIndex + 17].BackgroundImage = button.BackgroundImage;
						flowLayoutPanel1.Controls[button.TabIndex + 17].Tag = button.Tag;
						button.BackgroundImage = null;
						button.Tag = null;
						playerTurn = true;
						break;
					}
					//if piece is in column 7 or 8 can only attempt left-sided capture.
					else if ((button.TabIndex % 8 == 7 || button.TabIndex % 8 == 0)
					&& flowLayoutPanel1.Controls[button.TabIndex + 13].BackgroundImage == null
					&& flowLayoutPanel1.Controls[button.TabIndex + 6].Tag == "BCoin") {
						label1.Text = "good-capture-left (exception)";
						flowLayoutPanel1.Controls[button.TabIndex + 6].BackgroundImage = null;
						flowLayoutPanel1.Controls[button.TabIndex + 6].Tag = null;
						//AIPoints++
						flowLayoutPanel1.Controls[button.TabIndex + 13].BackgroundImage = button.BackgroundImage;
						flowLayoutPanel1.Controls[button.TabIndex + 13].Tag = button.Tag;
						button.BackgroundImage = null;
						button.Tag = null;
						playerTurn = true;
						break;
					}
					//check if piece can make a left capture
					else if (flowLayoutPanel1.Controls[button.TabIndex + 13].BackgroundImage == null
					&& flowLayoutPanel1.Controls[button.TabIndex + 6].Tag == "BCoin") {
						label1.Text = "good-capture-left (normal)";
						flowLayoutPanel1.Controls[button.TabIndex + 6].BackgroundImage = null;
						flowLayoutPanel1.Controls[button.TabIndex + 6].Tag = null;
						//AIPoints++
						flowLayoutPanel1.Controls[button.TabIndex + 13].BackgroundImage = button.BackgroundImage;
						flowLayoutPanel1.Controls[button.TabIndex + 13].Tag = button.Tag;
						button.BackgroundImage = null;
						button.Tag = null;
						playerTurn = true;
						break;
					}
					//check if piece can make a right capture
					else if (flowLayoutPanel1.Controls[button.TabIndex + 17].BackgroundImage == null
					&& flowLayoutPanel1.Controls[button.TabIndex + 8].Tag == "BCoin") {
						label1.Text = "good-capture-right (normal)";
						flowLayoutPanel1.Controls[button.TabIndex + 8].BackgroundImage = null;
						flowLayoutPanel1.Controls[button.TabIndex + 8].Tag = null;
						//AIPoints++
						flowLayoutPanel1.Controls[button.TabIndex + 17].BackgroundImage = button.BackgroundImage;
						flowLayoutPanel1.Controls[button.TabIndex + 17].Tag = button.Tag;
						button.BackgroundImage = null;
						button.Tag = null;
						playerTurn = true;
						break;
					}
				}
			}
            catch(ArgumentOutOfRangeException e) {
				//temporary error handler for testing purposes. flag var here to end AICapture and start normal AIMove
				//when I get there
				label1.Text = "Index out of range!";
            }

			AIPieces.Clear();
			//label1.Text = "done sleeping!";
			playerTurn = true;
        }
	}
}
