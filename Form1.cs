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
TabIndex mapping
00 01 02 03 04 05 06 07
08 09 10 11 12 13 14 15
16 17 18 19 20 21 22 23
24 25 26 27 28 29 30 31 
32 33 34 35 36 37 38 39
40 41 42 43 44 45 46 47
48 49 50 51 52 53 54 55
56 57 58 59 60 61 62 63

i % 8 mapping
0 1 2 3 4 5 6 7
0 1 2 3 4 5 6 7
0 1 2 3 4 5 6 7
0 1 2 3 4 5 6 7
0 1 2 3 4 5 6 7
0 1 2 3 4 5 6 7
0 1 2 3 4 5 6 7
0 1 2 3 4 5 6 7

col1 ~ i%8 = 0
col2 ~ i%8 = 1
col7 ~ i%8 = 6
col8 ~ i%8 = 7

Top-down (AI)
Left-Move	 index +7
Right-Move	 index +9
Left-Cap	 index +7, index +14
Right-Cap	 index +9, index +18 

Bot-up (p1)
Left-Move	 index -9
Right-Move	 index -7
Left-Cap	 index -9, index -18
Right-Cap	 index -7, index -14

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
					if (selected.TabIndex % 8 == 0) {
						//normal move check
						if (selected.TabIndex == button.TabIndex + 7 && button.BackgroundImage == null) {
							label1.Text = "left column, good move right";
							validMove = true;
						}
						//capture move check
						else if (selected.TabIndex == (button.TabIndex + 14) && button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag == "RCoin") {
								label1.Text = "left column, good capture right";
								flowLayoutPanel1.Controls[selected.TabIndex - 7].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag = null;
								//player1Points++
								validMove = true;
							}
						}
						else {
							validMove = false;
						}
					}
					//check move for right column exception
					else if (selected.TabIndex % 8 == 7) {
						//normal move check
						if (selected.TabIndex == button.TabIndex + 9 && button.BackgroundImage == null) {
							label1.Text = "right column, good move left";
							validMove = true;
						}
						//capture move check
						else if (selected.TabIndex == (button.TabIndex + 18) && button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag == "RCoin") {
								label1.Text = "right column, good capture left";
								flowLayoutPanel1.Controls[selected.TabIndex - 9].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag = null;
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
						if (flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag == "RCoin") {
							label1.Text = "good capture left";
							flowLayoutPanel1.Controls[selected.TabIndex - 9].BackgroundImage = null;
							flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag = null;
							//player1Points++
							validMove = true;
						}
					}
					//check capture move up-right
					else if (selected.TabIndex == (button.TabIndex + 14) && button.BackgroundImage == null) {
						if (flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag == "RCoin") {
							label1.Text = "good capture right";
							flowLayoutPanel1.Controls[selected.TabIndex - 7].BackgroundImage = null;
							flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag = null;
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
						AITurn(); //end player turn (might need and if() check for multi-moves above this)
					}
					else {
						label1.Text = "Invalid Move";
					}
				}
			}
		}
	
		public void AICapture(Button start, int middle, int end) {
			flowLayoutPanel1.Controls[start.TabIndex +middle].BackgroundImage = null;
			flowLayoutPanel1.Controls[start.TabIndex +middle].Tag = null;
			//AIPoints++;
			flowLayoutPanel1.Controls[start.TabIndex + end].BackgroundImage = start.BackgroundImage;
			flowLayoutPanel1.Controls[start.TabIndex + end].Tag = start.Tag;
			start.BackgroundImage = null;
			start.Tag = null;
			playerTurn = true;
        }

		public void AITurn() {
			this.Refresh(); //must repaint manually, since this is called within a single click action
			System.Threading.Thread.Sleep(1000);

			flowLayoutPanel1.Controls.CopyTo(temp, 0);
			foreach (Button button in temp) {
				if (button.Tag == "RCoin") {
					AIPieces.Add(button);
				}
			}

			//try capture move
			try {
				foreach (Button button in AIPieces) {
					//if piece is in column 1 or 2, attempt right-sided capture.
					if ((button.TabIndex % 8 == 0 ||button.TabIndex % 8 == 1)
					&& flowLayoutPanel1.Controls[button.TabIndex + 18].BackgroundImage == null
					&& flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BCoin") {
						AICapture(button, 9, 18);
						break;
					}
					//if piece is in column 7 or 8, attempt left-sided capture.
					else if ((button.TabIndex % 8 == 6 || button.TabIndex % 8 == 7)
					&& flowLayoutPanel1.Controls[button.TabIndex + 14].BackgroundImage == null
					&& flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BCoin") {
						label1.Text = "good-capture-left (exception)";
						AICapture(button, 7, 14);
						break;
					}
					//attempt left-sided capture.
					else if (flowLayoutPanel1.Controls[button.TabIndex + 14].BackgroundImage == null
					&& flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BCoin") {
						label1.Text = "good-capture-left (normal)";
						AICapture(button, 7, 14);
						break;
					}
					//attempt right-sided capture.
					else if (flowLayoutPanel1.Controls[button.TabIndex + 18].BackgroundImage == null
					&& flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BCoin") {
						label1.Text = "good-capture-right (normal)";
						AICapture(button, 9, 18);
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
