using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//flowLayoutPanel1.Controls.CopyTo(buttonArray, 0);
namespace CheckersProject {
    public partial class Form1 : Form {
		//Global Variables! I'm sorry!!
		Button selected;
		Boolean playerTurn;
		Button[] AIPieces;
		public Form1() {
			InitializeComponent();
			selected = null;
			playerTurn = true;
			AIPieces = new Button[64];
			flowLayoutPanel1.Controls.CopyTo(AIPieces, 0);
		}
			/*this is really scuffed..
						tab index starts at 0
						but if I do that, then there isn't any way to distinguish special cases like top/bot/left/right
						if I start tab index at 1
						then I can easily separate these columns and rows by doing a currentIndex%8
						then the left col = 1, right col = 0, top and bot can be separated by range
						but
						some functions like flowlayout.controls reads data as if the containers are still 0-63
						even though I changed the values to 1-64
						meaning there is a discrepency in the code that makes no sense
						functions accessing the same location can point to different locations //off by 1 
						if I am referencing a button via click, it's address is 1
						if I am referencing the SAME button via flowlayout, it's address is 0
						so I can get it to work, but it is scuffed.
						*/


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
							//player1Points++
							validMove = true;
						}
					}
					//check capture move up-right
					else if (selected.TabIndex == (button.TabIndex + 14) && button.BackgroundImage == null) {
						if (flowLayoutPanel1.Controls[selected.TabIndex - 8].Tag == "RCoin") {
							label1.Text = "good capture right";
							flowLayoutPanel1.Controls[selected.TabIndex - 8].BackgroundImage = null;
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
			this.Refresh(); //since I am calling this method within the click action, I have to manually repaint. I think.. or I am doing something horribly wrong
			System.Threading.Thread.Sleep(1000);

			//i don't know how to do a foreach statement reee
			//SUPER messy, can maybe make a helper method to reduce repeated code..
			List<Button> pieces = new List<Button>();

			//this does work, but raises some questions
			//either
			//1) I MUST keep an UPDATED array of buttons of the entire 64 button gameboard with newest info each turn
			//2) I make this AI button list global, copy once and remove all other empy buttons, and maintain it by
			//	finding a way to remove and replace the button entries, keeping them in order, and accurate
			//
			// in order to feed valid information into the AIPlay function
			//
			// one major major issue that I am worried about also is the range. if I get a black piece to the bottom 2 rows
			// or a black piece to the top 2 rows, the program will crash because it is looking 2 rows above for a possible
			// capture play. this will cause an out of range exception
			// so I will have to add an exception or come up with a more intelligent solution to my AIPlay algorithm.
			foreach(Button button in AIPieces) {
				if (button.Tag == "RCoin") { // ||button.Tag != "RKing"
					pieces.Add(button);
				}
            }

			/*
            foreach (Button button in AIPieces) {
				if(button.TabIndex % 8 == 1
				&& flowLayoutPanel1.Controls[selected.TabIndex + 18].BackgroundImage == null
				&& flowLayoutPanel1.Controls[selected.TabIndex + 10].Tag == "BCoin") {	
					label1.Text = "good capture right";
					flowLayoutPanel1.Controls[selected.TabIndex + 10].BackgroundImage = null;
					//AIPoints++
					flowLayoutPanel1.Controls[selected.TabIndex + 18].BackgroundImage = button.BackgroundImage;
					flowLayoutPanel1.Controls[selected.TabIndex + 18].Tag = button.Tag;
					button.BackgroundImage = null;
					button.Tag = null;
					playerTurn = true;
					break;
				}
				else if (button.TabIndex % 8 == 0
				&& flowLayoutPanel1.Controls[selected.TabIndex + 14].BackgroundImage == null
				&& flowLayoutPanel1.Controls[selected.TabIndex + 8].Tag == "BCoin") {
					label1.Text = "good capture left";
					flowLayoutPanel1.Controls[selected.TabIndex + 8].BackgroundImage = null;
					//AIPoints++
					flowLayoutPanel1.Controls[selected.TabIndex + 14].BackgroundImage = button.BackgroundImage;
					flowLayoutPanel1.Controls[selected.TabIndex + 14].Tag = button.Tag;
					button.BackgroundImage = null;
					button.Tag = null;
					playerTurn = true;
					break;
				}
				else if (flowLayoutPanel1.Controls[selected.TabIndex + 14].BackgroundImage == null
				&& flowLayoutPanel1.Controls[selected.TabIndex + 8].Tag == "BCoin") {
					label1.Text = "good capture left";
					flowLayoutPanel1.Controls[selected.TabIndex + 8].BackgroundImage = null;
					//AIPoints++
					flowLayoutPanel1.Controls[selected.TabIndex + 14].BackgroundImage = button.BackgroundImage;
					flowLayoutPanel1.Controls[selected.TabIndex + 14].Tag = button.Tag;
					button.BackgroundImage = null;
					button.Tag = null;
					playerTurn = true;
					break;
				}
				else if (flowLayoutPanel1.Controls[selected.TabIndex + 18].BackgroundImage == null
				&& flowLayoutPanel1.Controls[selected.TabIndex + 10].Tag == "BCoin") {
					label1.Text = "good capture right";
					flowLayoutPanel1.Controls[selected.TabIndex + 10].BackgroundImage = null;
					//AIPoints++
					flowLayoutPanel1.Controls[selected.TabIndex + 18].BackgroundImage = button.BackgroundImage;
					flowLayoutPanel1.Controls[selected.TabIndex + 18].Tag = button.Tag;
					button.BackgroundImage = null;
					button.Tag = null;
					playerTurn = true;
					break;
				}
			}
			*/

			

			//label1.Text = "done sleeping!";
			playerTurn = true;
        }
	}
}
