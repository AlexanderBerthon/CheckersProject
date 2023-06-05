using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//bugs
//error message doesn't go away
//
//improvements
//game over ui is gross
//
//end turn button is kind of janky.. but I know it would be a PAIN to get rid of
//
//AI pawn? moved backwards in a game.. not sure how this happened
//theory is that the piece was a king in a previous game and maintained that data
//ie the data wasn't cleared on restart
//needs additional testing to confirm
//if true then should be easy enough to fix
//if the same bug happens in the first game of the session there is a massive problem with the ai logic
//
//enemy pawn went from bottom left to up a row/right side. Illegal move / logic error
//became queen, then wen't off the rails

namespace CheckersProject {
	public partial class Form1 : Form {
		Button selected;
		Boolean playerTurn;//prevents click actions during AI move
		String moveStatus;
		List<Button> AIPieces;
		Button[] ButtonArray;
		int playerScore;
		int AIScore;

		public Form1() {
			InitializeComponent();
			selected = null;
			playerTurn = true;
			moveStatus = "invalid";
			AIPieces = new List<Button>();
			AIScore = 0;
			playerScore = 0;
			ButtonArray = new Button[64];
			flowLayoutPanel1.Controls.CopyTo(ButtonArray, 0);
		}

		///ends the player's turn and begins the AI's turn
		private void endTurn_Click(object sender, EventArgs e) {
            label3.Text = "";
            if (playerTurn && moveStatus != "invalid") {
				//update
				flowLayoutPanel1.Controls.CopyTo(ButtonArray, 0);
				//make kings
				for (int i = 0; i< 8; i++) {
					if(ButtonArray[i].Tag == "BCoin") {
						ButtonArray[i].BackgroundImage = Properties.Resources.BKing;
						ButtonArray[i].Tag = "BKing";
					}
				}
				playerTurn = false;
				selected.FlatStyle = FlatStyle.Flat;
				selected = null;
				AITurn();
			}
            else {
				label3.Text = "Make a move first";
            }
		}

		///movement function
		private void Boardbutton_Click(object sender, EventArgs e) {
			if (playerTurn) {
				Button button = (Button)sender;
				if (selected == null) {
					if (button.BackgroundImage == null || button.Tag == "RCoin" || button.Tag == "RKing") {
						label3.Text = "Invalid Selection";
					}
					else {
						selected = button;
						button.FlatStyle = FlatStyle.Standard;
					}
				}
				//undo first click / misclick
				else if (selected == button) { 
					if (moveStatus == "invalid") {
						selected.FlatStyle = FlatStyle.Flat;
						selected = null;
					}
				}
				else {
					Boolean validMove = false;
					//check move for left column exception
					if (selected.TabIndex % 8 == 0) {
						//normal move check
						if (moveStatus == "invalid" && selected.TabIndex == button.TabIndex + 7 && button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						//king normal down move
						else if (moveStatus == "invalid" && selected.Tag == "BKing" && (selected.TabIndex == (button.TabIndex - 9))
						&& button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						//capture move check
						else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.TabIndex == (button.TabIndex + 14)
						&& button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag == "RCoin"
							|| flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag == "RKing") {
								flowLayoutPanel1.Controls[selected.TabIndex - 7].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag = null;
								playerScore++;
								validMove = true;
								moveStatus = "captured";
							}
						}
						//king capture down move
						else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.Tag == "BKing"
						&& selected.TabIndex == (button.TabIndex - 18) && button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex + 9].Tag == "RCoin"
							|| flowLayoutPanel1.Controls[selected.TabIndex + 9].Tag == "RKing") {
								flowLayoutPanel1.Controls[selected.TabIndex + 9].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex + 9].Tag = null;
								playerScore++;
								validMove = true;
								moveStatus = "captured";
							}
						}
						else {
							validMove = false;
						}
					}
					//check move for 2nd left column exception
					else if (selected.TabIndex % 8 == 1) {
						//normal move check
						if (moveStatus == "invalid" && selected.TabIndex == button.TabIndex + 7 && button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						else if (moveStatus == "invalid" && selected.TabIndex == button.TabIndex + 9 && button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						//king normal downleft move
						else if (moveStatus == "invalid" && selected.Tag == "BKing" && (selected.TabIndex == (button.TabIndex - 7))
						&& button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						//king normal downright move
						else if (moveStatus == "invalid" && selected.Tag == "BKing" && (selected.TabIndex == (button.TabIndex - 9))
						&& button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						//capture move check
						else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.TabIndex == (button.TabIndex + 14)
						&& button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag == "RCoin"
								|| flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag == "RKing") {
								flowLayoutPanel1.Controls[selected.TabIndex - 7].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag = null;
								playerScore++;
								validMove = true;
								moveStatus = "captured";
							}
						}
						//king capture down move
						else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.Tag == "BKing"
						&& selected.TabIndex == (button.TabIndex - 18) && button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex + 9].Tag == "RCoin"
							|| flowLayoutPanel1.Controls[selected.TabIndex + 9].Tag == "RKing") {
								flowLayoutPanel1.Controls[selected.TabIndex + 9].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex + 9].Tag = null;
								playerScore++;
								validMove = true;
								moveStatus = "captured";
							}
						}
						else {
							validMove = false;
						}
					}
					//check move for right column exception
					else if (selected.TabIndex % 8 == 7) {
						//normal move check
						if (moveStatus == "invalid" && selected.TabIndex == button.TabIndex + 9 && button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						//king normal downleft move
						else if (moveStatus == "invalid" && selected.Tag == "BKing" && (selected.TabIndex == (button.TabIndex - 7))
						&& button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						//capture move check
						else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.TabIndex == (button.TabIndex + 18)
						&& button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag == "RCoin"
							|| flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag == "RKing") {
								flowLayoutPanel1.Controls[selected.TabIndex - 9].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag = null;
								playerScore++; 
								validMove = true;
								moveStatus = "captured";
							}
						}
						//king capture down left
						else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.Tag == "BKing"
						&& selected.TabIndex == (button.TabIndex - 14) && button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex + 7].Tag == "RCoin"
							|| flowLayoutPanel1.Controls[selected.TabIndex + 7].Tag == "RKing") {
								flowLayoutPanel1.Controls[selected.TabIndex + 7].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex + 7].Tag = null;
								playerScore++;
								validMove = true;
								moveStatus = "captured";
							}
						}
						else {
							validMove = false;
						}
					}
					//check move for 2nd right column exception
					else if (selected.TabIndex % 8 == 6) {
						//normal move check
						if (moveStatus == "invalid" && selected.TabIndex == button.TabIndex + 9 && button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						else if (moveStatus == "invalid" && selected.TabIndex == button.TabIndex + 7 && button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						//king normal downleft move
						else if (moveStatus == "invalid" && selected.Tag == "BKing" && (selected.TabIndex == (button.TabIndex - 7))
						&& button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						//king normal downright move
						else if (moveStatus == "invalid" && selected.Tag == "BKing" && (selected.TabIndex == (button.TabIndex - 9))
						&& button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						//capture move check
						else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.TabIndex == (button.TabIndex + 18)
						&& button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag == "RCoin"
							|| flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag == "RKing") {
								flowLayoutPanel1.Controls[selected.TabIndex - 9].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag = null;
								playerScore++;
								validMove = true;
								moveStatus = "captured";
							}
						}
						//king capture down left
						else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.Tag == "BKing"
						&& selected.TabIndex == (button.TabIndex - 14) && button.BackgroundImage == null) {
							if (flowLayoutPanel1.Controls[selected.TabIndex + 7].Tag == "RCoin"
							|| flowLayoutPanel1.Controls[selected.TabIndex + 7].Tag == "RKing") {
								flowLayoutPanel1.Controls[selected.TabIndex + 7].BackgroundImage = null;
								flowLayoutPanel1.Controls[selected.TabIndex + 7].Tag = null;
								playerScore++;
								validMove = true;
								moveStatus = "captured";
							}
						}
						else {
							validMove = false;
						}
					}
					//check normal move for all other cells
					else if (moveStatus == "invalid" && (selected.TabIndex == (button.TabIndex + 7)
					|| selected.TabIndex == (button.TabIndex + 9))) {
						if (button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						else {
							validMove = false;
						}
					}
					//check normal move for kings
					else if (moveStatus == "invalid" && selected.Tag == "BKing" && (selected.TabIndex == (button.TabIndex - 7)
					|| selected.TabIndex == (button.TabIndex - 9))) {
						if (button.BackgroundImage == null) {
							validMove = true;
							moveStatus = "moved";
						}
						else {
							validMove = false;
						}
					}
					//check normal capture move up-left
					else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.TabIndex == (button.TabIndex + 18)
					&& button.BackgroundImage == null) {
						if (flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag == "RCoin"
						|| flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag == "RKing") {
							flowLayoutPanel1.Controls[selected.TabIndex - 9].BackgroundImage = null;
							flowLayoutPanel1.Controls[selected.TabIndex - 9].Tag = null;
							playerScore++; 
							validMove = true;
							moveStatus = "captured";
						}
					}
					//check normal capture move up-right
					else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.TabIndex == (button.TabIndex + 14)
					&& button.BackgroundImage == null) {
						if (flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag == "RCoin"
						|| flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag == "RKing") {
							flowLayoutPanel1.Controls[selected.TabIndex - 7].BackgroundImage = null;
							flowLayoutPanel1.Controls[selected.TabIndex - 7].Tag = null;
							playerScore++; 
							validMove = true;
							moveStatus = "captured";
						}
					}
					//check king capture down-left
					else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.Tag == "BKing"
					&& selected.TabIndex == (button.TabIndex - 14) && button.BackgroundImage == null) {
						if (flowLayoutPanel1.Controls[selected.TabIndex + 7].Tag == "RCoin"
						|| flowLayoutPanel1.Controls[selected.TabIndex + 7].Tag == "RKing") {
							flowLayoutPanel1.Controls[selected.TabIndex + 7].BackgroundImage = null;
							flowLayoutPanel1.Controls[selected.TabIndex + 7].Tag = null;
							playerScore++;
							validMove = true;
							moveStatus = "captured";
						}
					}
					//check king capture down-right
					else if ((moveStatus == "invalid" || moveStatus == "captured") && selected.Tag == "BKing"
					&& selected.TabIndex == (button.TabIndex - 18) && button.BackgroundImage == null) {
						if (flowLayoutPanel1.Controls[selected.TabIndex + 9].Tag == "RCoin"
						|| flowLayoutPanel1.Controls[selected.TabIndex + 9].Tag == "RKing") {
							flowLayoutPanel1.Controls[selected.TabIndex + 9].BackgroundImage = null;
							flowLayoutPanel1.Controls[selected.TabIndex + 9].Tag = null;
							playerScore++;
							validMove = true;
							moveStatus = "captured";
						}
					}
					else {
						validMove = false;
					}
					if (validMove) { //execute
						button.BackgroundImage = selected.BackgroundImage;
						button.Tag = selected.Tag;
						selected.FlatStyle = FlatStyle.Flat;
						selected.BackgroundImage = null;
						selected.Tag = null;
						selected = null;

						//lock
						selected = button;
						button.FlatStyle = FlatStyle.Standard;

						label2.Text = "Player Score: "+playerScore.ToString();
						label1.Text = "AI Score: " + AIScore.ToString();
					}
					else {
						label3.Text = "Invalid Move";
					}

					//check win condition
					if (playerScore == 12) {
						foreach (Button btn in ButtonArray) {
							btn.Enabled = false;
						}
						button65.Enabled = false;
						GameOverLabel.Text = "YOU WIN!";
						GameOverPanel.Visible = true;
					}

				}
			}
		}

		public void AITurn() {
			this.Refresh();
			System.Threading.Thread.Sleep(1000);
			Boolean moved = false;
			Boolean moveAvailable = true;
			Button temp = null;
			int multi = 0;

			updateAIData();

			//try capture move
			while (moveAvailable) {
				moveAvailable = false;
				try {
					foreach (Button button in AIPieces) {
						//if piece is in column 1 or 2, attempt right-sided capture
						if (button.TabIndex % 8 == 0 || button.TabIndex % 8 == 1) {
							//bottom left quadrent
							if (button.TabIndex == 48 || button.TabIndex == 49 ||
								button.TabIndex == 56 || button.TabIndex == 9) {
								//try an upward-right capture if king and available
								if (button.Tag == "RKing" 
								&& flowLayoutPanel1.Controls[button.TabIndex - 14].BackgroundImage == null
								&& (flowLayoutPanel1.Controls[button.TabIndex - 7].Tag == "BCoin"
								|| flowLayoutPanel1.Controls[button.TabIndex - 7].Tag == "BKing")) {
									AICapture(button, -7, -14);
									break;
								}
							}
							//King in middle left quadrent
							else if (button.Tag == "RKing" && (button.TabIndex == 16 || button.TabIndex == 24 ||
								button.TabIndex == 32 || button.TabIndex == 40 || button.TabIndex == 17 ||
								button.TabIndex == 25) || button.TabIndex == 33 || button.TabIndex == 41) {
								//try an upward-right capture
								if (flowLayoutPanel1.Controls[button.TabIndex - 14].BackgroundImage == null
								&& (flowLayoutPanel1.Controls[button.TabIndex - 7].Tag == "BCoin"
								|| flowLayoutPanel1.Controls[button.TabIndex - 7].Tag == "BKing")) {
									AICapture(button, -7, -14);
									break;
								}
								//try a downward-right capture
								else if (flowLayoutPanel1.Controls[button.TabIndex + 18].BackgroundImage == null
								&& (flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BCoin"
								|| flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BKing")) {
									AICapture(button, 9, 18);
									break;
								}
							}
							//down right (also counts for King's in the top left quadrent)
							else if (flowLayoutPanel1.Controls[button.TabIndex + 18].BackgroundImage == null
							&& (flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BCoin"
							|| flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BKing")) {
								AICapture(button, 9, 18);
								break;
							}
						}
						//if piece is in column 7 or 8, attempt left-sided capture.
						else if (button.TabIndex % 8 == 6 || button.TabIndex % 8 == 7) {
							//bottom right quadrent
							if (button.TabIndex == 54 || button.TabIndex == 55 ||
							button.TabIndex == 62 || button.TabIndex == 63) {
								//try an upward-left capture if king and available
								if (button.Tag == "RKing"
								&& flowLayoutPanel1.Controls[button.TabIndex - 18].BackgroundImage == null
								&& (flowLayoutPanel1.Controls[button.TabIndex - 9].Tag == "BCoin"
								|| flowLayoutPanel1.Controls[button.TabIndex - 9].Tag == "BKing")) {
									AICapture(button, -9, -18);
									break;
								}
							}
							//King in middle right quadrent
							else if (button.Tag == "RKing" && (button.TabIndex == 22 || button.TabIndex == 30 ||
							button.TabIndex == 38 || button.TabIndex == 46 || button.TabIndex == 23 ||
							button.TabIndex == 31) || button.TabIndex == 39 || button.TabIndex == 47) {
								//try an upward-left capture
								if (flowLayoutPanel1.Controls[button.TabIndex - 18].BackgroundImage == null
								&& (flowLayoutPanel1.Controls[button.TabIndex - 9].Tag == "BCoin"
								|| flowLayoutPanel1.Controls[button.TabIndex - 9].Tag == "BKing")) {
									AICapture(button, -9, -18);
									break;
								}
								//try a downward-left capture
								else if (flowLayoutPanel1.Controls[button.TabIndex + 14].BackgroundImage == null
								&& (flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BCoin"
								|| flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BKing")) {
									AICapture(button, 7, 14);
									break;
								}
							}
							//down left (also counts for King's in the top left quadrent)
							else if (flowLayoutPanel1.Controls[button.TabIndex + 14].BackgroundImage == null
							&& (flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BCoin"
							|| flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BKing")) {
								AICapture(button, 7, 14);
								break;
							}
						}
                        //else mid section
                        else {
							//bottom 2 rows
							if (button.TabIndex == 50 || button.TabIndex == 51 || button.TabIndex == 52 ||
							button.TabIndex == 53 || button.TabIndex == 58 || button.TabIndex == 59 ||
							button.TabIndex == 60 || button.TabIndex == 61) {
								//if king and up move avail
								if (button.Tag == "RKing") {
									//try an upward-left capture
									if (flowLayoutPanel1.Controls[button.TabIndex - 18].BackgroundImage == null
									&& (flowLayoutPanel1.Controls[button.TabIndex - 9].Tag == "BCoin"
									|| flowLayoutPanel1.Controls[button.TabIndex - 9].Tag == "BKing")) {
										AICapture(button, -9, -18);
										break;
									}
									//try an upward-right capture
									else if (flowLayoutPanel1.Controls[button.TabIndex - 14].BackgroundImage == null
									&& (flowLayoutPanel1.Controls[button.TabIndex - 7].Tag == "BCoin"
									|| flowLayoutPanel1.Controls[button.TabIndex - 7].Tag == "BKing")) {
										AICapture(button, -7, -14);
										break;
									}
								}
								else {
									//do nothing (normal pieces cannot capture down or will go out of range)
								}
							}
							//if in top 2 rows
							else if (button.TabIndex == 2 || button.TabIndex == 3 || button.TabIndex == 4 ||
							button.TabIndex == 5 || button.TabIndex == 10 || button.TabIndex == 11 ||
							button.TabIndex == 12 || button.TabIndex == 13) {
								//try down left
								if (flowLayoutPanel1.Controls[button.TabIndex + 14].BackgroundImage == null
								&& (flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BCoin"
								|| flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BKing")) {
									AICapture(button, 7, 14);
									break;
								}
								//try down right
								else if (flowLayoutPanel1.Controls[button.TabIndex + 18].BackgroundImage == null
								&& (flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BCoin"
								|| flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BKing")) {
									AICapture(button, 9, 18);
									break;
								}
							}
							//mid no restrictions, try every move
                            else {
								if (button.Tag == "RKing") {
									//try an upward-left capture
									if (flowLayoutPanel1.Controls[button.TabIndex - 18].BackgroundImage == null
									&& (flowLayoutPanel1.Controls[button.TabIndex - 9].Tag == "BCoin"
									|| flowLayoutPanel1.Controls[button.TabIndex - 9].Tag == "BKing")) {
										AICapture(button, -9, -18);
										break;
									}
									//try an upward-right capture
									else if (flowLayoutPanel1.Controls[button.TabIndex - 14].BackgroundImage == null
									&& (flowLayoutPanel1.Controls[button.TabIndex - 7].Tag == "BCoin"
									|| flowLayoutPanel1.Controls[button.TabIndex - 7].Tag == "BKing")) {
										AICapture(button, -7, -14);
										break;
									}
								}
                                else {
									//try down left
									if (flowLayoutPanel1.Controls[button.TabIndex + 14].BackgroundImage == null
									&& (flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BCoin"
									|| flowLayoutPanel1.Controls[button.TabIndex + 7].Tag == "BKing")) {
										AICapture(button, 7, 14);
										break;
									}
									//try down right
									else if (flowLayoutPanel1.Controls[button.TabIndex + 18].BackgroundImage == null
									&& (flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BCoin"
									|| flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "BKing")) {
										AICapture(button, 9, 18);
										break;
									}
								}

							}

						}
					}
					//multi-move control. Only the piece that moved will be eligible to make another capture move. 
					if (moveAvailable) {
						updateAIData();
						foreach (Button button in AIPieces) {
							if (button.TabIndex == multi) {
								temp = button;
							}
						}
						AIPieces.RemoveRange(0, AIPieces.Count);
						AIPieces.Add(temp);
					}
				}
				catch (ArgumentOutOfRangeException e) {
					//this should never happen with updated logic
					label3.Text = "CRITICAL ERROR";
				}
			}

			updateAIData();

			//try normal move
			if (!moved) {
				//this bit of code mixes up the AIPieces to make movements unpredictable 
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
						//if in bottom row (guaranteed king)
						if(button.TabIndex >= 56) {
							//if in bot-left corner, try up-right move
							if(button.TabIndex == 56 && flowLayoutPanel1.Controls[button.TabIndex - 7].BackgroundImage == null) {
								AIMove(button, -7);
								break;
							}
							//if in bot-right corner, try up left move
							else if(button.TabIndex == 62 && flowLayoutPanel1.Controls[button.TabIndex - 9].BackgroundImage == null) {
								AIMove(button, -9);
								break;
							}
                            else {
								//try up-right move
								if (flowLayoutPanel1.Controls[button.TabIndex - 7].BackgroundImage == null) {
									AIMove(button, -7);
									break;
								}
								//try up-left move
								else if (flowLayoutPanel1.Controls[button.TabIndex - 9].BackgroundImage == null) {
									AIMove(button, -9);
									break;
								}
							}
                        }
						//if in top row - king range exception check
						if (button.TabIndex <= 7 && button.Tag == "RKing") {
							//if in top-left corner, try down-right move
							if (button.TabIndex == 0 && flowLayoutPanel1.Controls[button.TabIndex + 9].BackgroundImage == null) {
								AIMove(button, 9);
								break;
							}
							//if in top-right corner, try down-left move
							else if (button.TabIndex == 7 && flowLayoutPanel1.Controls[button.TabIndex + 7].BackgroundImage == null) {
								AIMove(button, 7);
								break;
							}
							else {
								//try down-right move
								if (flowLayoutPanel1.Controls[button.TabIndex + 9].BackgroundImage == null) {
									AIMove(button, 9);
									break;
								}
								//try down-left move
								else if (flowLayoutPanel1.Controls[button.TabIndex +7].BackgroundImage == null) {
									AIMove(button, 7);
									break;
								}
							}
						}
						//if piece is in the first column
						else if (button.TabIndex % 8 == 0) {
							//if king attempt up-right move 
							if (button.Tag == "RKing" && flowLayoutPanel1.Controls[button.TabIndex - 7].BackgroundImage == null) {
								AIMove(button, -7);
								break;
							}
							//attempt down-right move
							if (flowLayoutPanel1.Controls[button.TabIndex + 9].BackgroundImage == null) {
								AIMove(button, 9);
								break;
							}
						}
						//if piece is in the last column
						else if (button.TabIndex % 8 == 7) {
							//if king attempt up-left move
							if (button.Tag == "RKing" && flowLayoutPanel1.Controls[button.TabIndex - 9].BackgroundImage == null) {
								AIMove(button, -9);
								break;
							}
							//attempt down-left move
							if (flowLayoutPanel1.Controls[button.TabIndex + 7].BackgroundImage == null) {
								AIMove(button, 7);
								break;
							}
						}
						//if king attempt up-left move
						else if (button.Tag == "RKing" && flowLayoutPanel1.Controls[button.TabIndex - 9].BackgroundImage == null) {
							AIMove(button, -9);
							break;
						}
						//if king attempt up-right move 
						else if (button.Tag == "RKing" && flowLayoutPanel1.Controls[button.TabIndex - 7].BackgroundImage == null) {
							AIMove(button, -7);
							break;
						}
						//attempt down-left move.
						else if (flowLayoutPanel1.Controls[button.TabIndex + 7].BackgroundImage == null) {
							AIMove(button, 7);
							break;
						}
						//attempt down-right move.
						else if (flowLayoutPanel1.Controls[button.TabIndex + 9].BackgroundImage == null) {
							AIMove(button, 9);
							break;
						}
					}
				}
				catch (ArgumentOutOfRangeException e) {
					//this should never happen
					label3.Text = "Normal Move Error";
				}
			}

			updateAIData();

			//make kings (ai)
			foreach (Button button in AIPieces) {
				if (button.Tag == "RCoin" && button.TabIndex > 55) {
					button.BackgroundImage = Properties.Resources.RKing;
					button.Tag = "RKing";
				}
			}

			AIPieces.Clear();
			playerTurn = true;
			moveStatus = "invalid";

			//check win condition
			if (AIScore == 12) {
				foreach (Button btn in ButtonArray) {
					btn.Enabled = false;
				}
				button65.Enabled = false;
				GameOverLabel.Text = "YOU LOSE";
				GameOverPanel.Visible = true;
			}

			///This function modifies the game board data to display AI capture movement
			void AICapture(Button start, int middle, int end) {
				multi = start.TabIndex + end;
				flowLayoutPanel1.Controls[start.TabIndex + middle].BackgroundImage = null;
				flowLayoutPanel1.Controls[start.TabIndex + middle].Tag = null;
				AIScore++;
				flowLayoutPanel1.Controls[start.TabIndex + end].BackgroundImage = start.BackgroundImage;
				flowLayoutPanel1.Controls[start.TabIndex + end].Tag = start.Tag;
				start.BackgroundImage = null;
				start.Tag = null;
				this.Refresh();
				moved = true;
				moveAvailable = true;
				System.Threading.Thread.Sleep(1000);
			}

			///This function modifies the game board data to display an AI piece movement
			void AIMove(Button start, int end) {
				flowLayoutPanel1.Controls[start.TabIndex + end].BackgroundImage = start.BackgroundImage;
				flowLayoutPanel1.Controls[start.TabIndex + end].Tag = start.Tag;
				start.BackgroundImage = null;
				start.Tag = null;
				this.Refresh();
				System.Threading.Thread.Sleep(1000);
			}

			///This function scans all buttons on the board and isolates the ones that contain active AI pieces
			///the pieces are stored in a list to be used by the AI to make movements or captures
			void updateAIData() {
				flowLayoutPanel1.Controls.CopyTo(ButtonArray, 0);
				AIPieces.Clear();
				foreach (Button button in ButtonArray) {
					if (button.Tag == "RCoin" || button.Tag == "RKing") {
						AIPieces.Add(button);
					}
				}
			}

		}

		///restart game
        private void ContinueButton_Click(object sender, EventArgs e) {
			//reset variables and restart the game state
			AIPieces.Clear();
			foreach(Button btn in ButtonArray) {
				btn.BackgroundImage = null;
				btn.Tag = null;
				btn.Enabled = true;
				btn.FlatStyle = FlatStyle.Flat;

			}
			GameOverPanel.Visible = false;

			flowLayoutPanel1.Controls.CopyTo(ButtonArray, 0);

			int[] setupRed = { 1, 3, 5, 7, 8, 10, 12, 14, 17, 19, 21, 23};
			int[] setupBlack = {40, 42, 44, 46, 49, 51, 53, 55, 56, 58, 60, 62 };
			
			foreach (int i in setupRed) {
				ButtonArray[i].BackgroundImage = Properties.Resources.RCoin;
				ButtonArray[i].Tag = "RCoin";
			}
		
			foreach (int i in setupBlack) {
				ButtonArray[i].BackgroundImage = Properties.Resources.BCoin;
				ButtonArray[i].Tag = "BCoin";
			}
			label1.Text = "";
			label2.Text = "";
			label3.Text = "";
			button65.Enabled = true;
			selected = null;
			playerTurn = true;
			moveStatus = "invalid";
			AIScore = 0;
			playerScore = 0;
		}

        private void ExitButton_Click(object sender, EventArgs e) {
			Application.Exit();
        }
    }
}

/* NOTES
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
1 = down-right
2 = down-left
3 = up-right
4 = up-left
5 = down-left or down-right
6 = up-left or up-right
7 = up-right or down-right
8 = up-left or down-left
9 = up-left or down-left or up-right or down-right

1 1 5 5 5 5 2 2
1 1 5 5 5 5 2 2
7 7 9 9 9 9 8 8 
7 7 9 9 9 9 8 8 
7 7 9 9 9 9 8 8 
7 7 9 9 9 9 8 8 
3 3 6 6 6 6 4 4
3 3 6 6 6 6 4 4 

king movement logic
1 = down-right
2 = down-left
3 = up-right
4 = up-left
5 = down-left or down-right
6 = up-left or up-right
7 = up-right or down-right
8 = up-left or down-left
9 = up-left or down-left or up-right or down-right

1 5 5 5 5 5 5 2
7 9 9 9 9 9 9 8
7 9 9 9 9 9 9 8
7 9 9 9 9 9 9 8
7 9 9 9 9 9 9 8
7 9 9 9 9 9 9 8
7 9 9 9 9 9 9 8 
3 6 6 6 6 6 6 4

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