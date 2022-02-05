using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// pawns doing king moves
/// kings at the bottom not doing anything
/// </summary>


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

king capture logic
1 = right or down
2 = left or down
3 = up or right
4 = up or left
5 = left right or down
6 = left right or up
7 = up or down or right
8 = up or down or left
9 = left right up or down

1 1 5 5 5 5 2 2
1 1 5 5 5 5 2 2
7 7 9 9 9 9 8 8 
7 7 9 9 9 9 8 8 
7 7 9 9 9 9 8 8 
7 7 9 9 9 9 8 8 
3 3 6 6 6 6 4 4
3 3 6 6 6 6 4 4 

king movement logic
1 = right or down
2 = left or down
3 = up or right
4 = up or left
5 = left or right or down
6 = left or right or up
7 = up or down or right
8 = up or down or left
9 = left right up down

1 5 5 5 5 5 5 2
7 9 9 9 9 9 9 8
7 9 9 9 9 9 9 8
7 9 9 9 9 9 9 8
7 9 9 9 9 9 9 8
7 9 9 9 9 9 9 8
7 9 9 9 9 9 9 8 
3 6 6 6 6 6 6 4

write crap code first, fix it later?

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
		Button[] ButtonArray;


		public Form1() {
			InitializeComponent();
			selected = null;
			playerTurn = true;
			AIPieces = new List<Button>();

			ButtonArray = new Button[64];
			flowLayoutPanel1.Controls.CopyTo(ButtonArray, 0);
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

		public void AITurn() {
			//testing
			//label2.Text = "?";

			this.Refresh(); //must repaint manually, since this is called within a single click action
			System.Threading.Thread.Sleep(1000);
			Boolean moved = false;
			Boolean moveAvailable = true;
			Button temp = null;
			int multi = 0;

			updateAIData();
			
			//try capture move
			while (moveAvailable) {
				try {
					foreach (Button button in AIPieces) {
						//if piece is in column 1 or 2, attempt right-sided capture
						if (button.TabIndex % 8 == 0 || button.TabIndex % 8 == 1){
							//King in bottom left quadrent
							if (button.Tag == "RKing" && (button.TabIndex == 48 || button.TabIndex == 49 ||
								button.TabIndex == 56 || button.TabIndex == 9)) {
								//try an upward-right capture
								if (flowLayoutPanel1.Controls[button.TabIndex - 14].BackgroundImage == null
							&& flowLayoutPanel1.Controls[button.TabIndex - 7].Tag == "BCoin") {
									label1.Text = "King capture up-right (lCol-bot)";
									AICapture(button, -7, -14);
									break;
								}
							}
							//King in middle left quadrent
							else if (button.Tag == "RKing" && (button.TabIndex == 16 || button.TabIndex == 24 ||
								button.TabIndex == 32 || button.TabIndex == 40 || button.TabIndex == 17 ||
								button.TabIndex == 25)|| button.TabIndex == 33 || button.TabIndex == 41){
								//try an upward-right capture
								if (flowLayoutPanel1.Controls[button.TabIndex - 14].BackgroundImage == null
									&& flowLayoutPanel1.Controls[button.TabIndex - 7].Tag == "BCoin") {
									label1.Text = "King capture up-right (lCol-mid)";
									AICapture(button, -7, -14);
									break;
								}
								//try a downward-right capture
								else if (flowLayoutPanel1.Controls[button.TabIndex + 18].BackgroundImage == null
									&& flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BCoin") {
									label1.Text = "King capture down-right (lCol-mid)";
									AICapture(button, 9, 18);
									break;
								}
							}
							//down right (also counts for King's in the top left quadrent)
							else if (flowLayoutPanel1.Controls[button.TabIndex + 18].BackgroundImage == null
								&& flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BCoin") {
								label1.Text = "good-capture-right (exception)";
								AICapture(button, 9, 18);
								break;
							}
						}
						//if piece is in column 7 or 8, attempt left-sided capture.
						else if (button.TabIndex % 8 == 6 || button.TabIndex % 8 == 7){
							//King in bottom right quadrent
							if (button.Tag == "RKing" && (button.TabIndex == 54 || button.TabIndex == 55 ||
								button.TabIndex == 62 || button.TabIndex == 63)) {
								//try an upward-left capture
								if (flowLayoutPanel1.Controls[button.TabIndex - 18].BackgroundImage == null
							&& flowLayoutPanel1.Controls[button.TabIndex - 9].Tag == "BCoin") {
									label1.Text = "King capture up-left (rCol-bot)";
									AICapture(button, -9, -18);
									break;
								}
							}
							//King in middle right quadrent ~~~~~~~~~~~~~~~~~~~~~~~
							else if (button.Tag == "RKing" && (button.TabIndex == 22 || button.TabIndex == 30 ||
								button.TabIndex == 38 || button.TabIndex == 46 || button.TabIndex == 23 ||
								button.TabIndex == 31) || button.TabIndex == 39 || button.TabIndex == 47) {
								//try an upward-left capture
								if (flowLayoutPanel1.Controls[button.TabIndex - 18].BackgroundImage == null
									&& flowLayoutPanel1.Controls[button.TabIndex - 9].Tag == "BCoin") {
									label1.Text = "King capture up-left (rCol-mid)";
									AICapture(button, -9, -18);
									break;
								}
								//try a downward-left capture
								else if (flowLayoutPanel1.Controls[button.TabIndex + 14].BackgroundImage == null
									&& flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BCoin") {
									label1.Text = "King capture down-left (rCol-mid)";
									AICapture(button, 7, 14);
									break;
								}
							}
							//down left (also counts for King's in the top left quadrent)
							else if (flowLayoutPanel1.Controls[button.TabIndex + 14].BackgroundImage == null
							&& flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BCoin") {
								label1.Text = "good-capture-left (exception)";
								AICapture(button, 7, 14);
								break;
							}
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
						//final check for kings to go up? all other possible moves have been tried I think
						//so if it gets here the only other option is middle column kings making upward captures..
						else if(button.Tag == "RKing") {
							//label2.Text = "Did I get here?";
							//try an up left capture
							if (flowLayoutPanel1.Controls[button.TabIndex - 18].BackgroundImage == null
								&& flowLayoutPanel1.Controls[button.TabIndex - 9].Tag == "BCoin") {
								label1.Text = "King capture up-left (center?)";
								AICapture(button, -9, -18);
								break;
							}
							//try an upward-right capture
							if (flowLayoutPanel1.Controls[button.TabIndex - 14].BackgroundImage == null
								&& flowLayoutPanel1.Controls[button.TabIndex - 7].Tag == "BCoin") {
								label1.Text = "King capture up-right (center?)";
								AICapture(button, -7, -14);
								break;
							}
						}
						else {
							moveAvailable = false;
						}
					}
                    if (moveAvailable) {
						updateAIData();
						foreach(Button button in AIPieces) {
							if(button.TabIndex == multi) {
								temp = button;
                            }
                        }
						for(int i = 0; i<AIPieces.Count; i++) {
							AIPieces.RemoveAt(i);
                        }
						AIPieces.Add(temp);
					}
				}
				catch (ArgumentOutOfRangeException e) {
					//this might cause an infinite loop...
					//say there is 2 pieces. one is out of range, and triggers this. the other is in range but can't play
					//because the first breaks the loop? 
					//how do you get out? how does moveAvailable ever become false?
					//moved = false; 
					//issue!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
					label1.Text = "Index out of range!";
				}
			}
			if (!moved) {
				//this bit of code mixes up the AIPieces to make movements not predictable
				Random random = new Random();
				int randIndex = 0;
				temp = null;
				for (int i = 0; i < 25; i++) {
					randIndex = random.Next(0, AIPieces.Count);
					temp = AIPieces[randIndex];
					AIPieces.RemoveAt(randIndex);
					AIPieces.Add(temp);
				}

				try {
					foreach (Button button in AIPieces) {
						//if piece is in column 1, attempt right move.
						if (button.TabIndex % 8 == 0){
							if (flowLayoutPanel1.Controls[button.TabIndex + 9].BackgroundImage == null) {
								label1.Text = "good-move-right (exception)";
								AIMove(button, 9);
								break;
							}
						}
						//if piece is in column 8, attempt left move.
						else if (button.TabIndex % 8 == 7) {
							if (flowLayoutPanel1.Controls[button.TabIndex + 7].BackgroundImage == null) {
								label1.Text = "good-move-left (exception)";
								AIMove(button, 7);
								break;
							}
						}
						//attempt left move.
						else if (flowLayoutPanel1.Controls[button.TabIndex + 7].BackgroundImage == null) {
							label1.Text = "good-move-left (normal)";
							AIMove(button, 7);
							break;
						}
						//attempt right move.
						else if (flowLayoutPanel1.Controls[button.TabIndex + 9].BackgroundImage == null) {
							label1.Text = "good-move-right (normal)";
							AIMove(button, 9);
							break;
						}
					}
				}
				catch (ArgumentOutOfRangeException e) {
					//this should never happen?
					label1.Text = "Normal Move Error";
				}
			}

			updateAIData();

            //testing
            foreach(Button button in AIPieces) {
				if(button.Tag == "RKing") {
					label2.Text = "REEEEEEEEEEEEEEEEEEEEE";
                }
            }


			//make kings
			foreach (Button button in AIPieces) {
				if(button.TabIndex > 55) {
					button.BackgroundImage = Properties.Resources.RKing;
					button.Tag = "RKing";
                }
            }

			AIPieces.Clear();
			//label1.Text = "your turn?";
			playerTurn = true;

			//can I just leave these here?
			void AICapture(Button start, int middle, int end) {
				multi = start.TabIndex + end;
				flowLayoutPanel1.Controls[start.TabIndex + middle].BackgroundImage = null;
				flowLayoutPanel1.Controls[start.TabIndex + middle].Tag = null;
				//AIPoints++;
				flowLayoutPanel1.Controls[start.TabIndex + end].BackgroundImage = start.BackgroundImage;
				flowLayoutPanel1.Controls[start.TabIndex + end].Tag = start.Tag;
				start.BackgroundImage = null;
				start.Tag = null;
				this.Refresh();
				moved = true;
				moveAvailable = true;
				System.Threading.Thread.Sleep(1000);
			}

			void AIMove(Button start, int end) {
				//AIPoints++;
				flowLayoutPanel1.Controls[start.TabIndex + end].BackgroundImage = start.BackgroundImage;
				flowLayoutPanel1.Controls[start.TabIndex + end].Tag = start.Tag;
				start.BackgroundImage = null;
				start.Tag = null;
				this.Refresh();
				System.Threading.Thread.Sleep(1000);
			}

			void updateAIData() {
				flowLayoutPanel1.Controls.CopyTo(ButtonArray, 0);
				foreach (Button button in ButtonArray) {
					if (button.Tag == "RCoin" || button.Tag == "RKing") {
						AIPieces.Add(button);
					}
				}

			}
		}
	}
}
//Normal-moves are broken for AI, when piece gets to end since there is no king-movement it goes out of range