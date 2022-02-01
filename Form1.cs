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
        public Form1() {
            InitializeComponent();
			selected = null;
        }

        private void button_Click(object sender, EventArgs e) {
			Button button = (Button)sender;
			if (selected == null) {
				if(button.BackgroundImage == null || button.Tag == "RCoin" || button.Tag == "RKing") {
					label1.Text = "Invalid Selection";
                }
                else{
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
				//valid move check for cells in far left column
				if (selected.TabIndex % 8 == 1) {
					label1.Text = "reeeeeeeeee";
					if (selected.TabIndex == button.TabIndex + 7 && button.BackgroundImage == null) {
						validMove = true;
					}
					//if target is in range, and landing zone is clear, and there is an enemy piece inbetween
					else if(selected.TabIndex == (button.TabIndex + 14) && button.BackgroundImage == null) {
						label1.Text = "Victory is Sweet";
						if (flowLayoutPanel1.Controls[33].Tag == "RCoin") {
							label1.Text = "But Honor is Sweeter";
							flowLayoutPanel1.Controls[33].BackgroundImage = null;
							//add point
							validMove = true;
						}
					}
                    else {
						validMove = false;
                    }
				}
				//valid move check for cells in far right column
				else if (selected.TabIndex % 8 == 0) {
					if (selected.TabIndex == button.TabIndex + 9 && button.BackgroundImage == null) {
						validMove = true;
					}
					//if target is in range, and landing zone is clear, and there is an enemy piece inbetween
					else if (selected.TabIndex == button.TabIndex + 16 && button.BackgroundImage != null && flowLayoutPanel1.Controls[button.TabIndex + 9].Tag == "RCoin") {
						validMove = true;
					}
					else {
						validMove = false;
					}
				}
				//valid move check for all other cells
				else if(selected.TabIndex == (button.TabIndex + 7) || selected.TabIndex == (button.TabIndex + 9)) {
					label1.Text = "wat";
					if(button.BackgroundImage == null) {
						validMove = true;
					}
					else {
						validMove = false;
					}
				}
				//invalid move
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
					//end turn
				}
				else {
					//label1.Text = "Invalid Move";
				}
			}
        }

		//button action.
		/*
        click the first cell, store the button location. 
        click the second cell
            - compare the two button locations/adresses
            - if invalid move, display "invalid move" and clear storage
            - if valid move..
                    - normal move forward
                        - remove background picture of old location
                        - update background picture of new location
                    - jump/capture move
                        - remove background picture of old location
                        - remove background picture of 'captured' location
                        - update background picture of new final location
                        - loop (if chain captures are possible)

        valid move
        one square forward, diagonally forward, or sideways 
        capture, jump over opponent piece as long as landing zone is clear



        how do you tell which button does what? 
        this isn't going to work..
        if you program all buttons to have the same functionality, then which direction is allowed vs not allowed?
        you would have to check the background picture id to distinguish which kind of tile is on it and load logic
        each time

        what kind of piece is this?
        - if red
            - only move up, left, right, diagonal up left, or diagonal up right. and only if those spaces aren't occupied
        - if black
            - only move down, left, right, diagonal down left, or diagonal down right. and only if the space isn't occupied
        - if king
            - can move in any direction

        top and bottom row of buttons must check for opposite sides color and promote the piece if needed

        how do you write the button logic to allow users to even play? if you click? If you click once, on the first tile.
        the entire event runs?

		//psudocode
        Gridbutton_clicked(){
			if this button is already highlighted
				if(another turn is true)
					- "you may move again with this tile or end turn"
				else
					- unhighlight this button //misclick, changed mind, etc. 
			if this button isn't highlighted, but there is another button highlighted
				- check if valid move
					- check background picture
						- if no picture, invalid move. no piece to move
						- if red pawn, can only move up, left, right, up-left, up-right
						- if black pawn, can only move down, left, right, up-left, up-right
						- if king, can move any direction
					- check distance
						- if destination tile is adjacent to highlighted tile
							- check destination tile background picture
								- if background pic exists, invalid move
								- else check direction is valid given it's background picture identifier
									- if direction is valid
										- unhighlight old button
										- remove background picture from old button
										- add correct background picture to destination button
										- end turn
									- else invalid move
						- if destination tile is not adjacent but within '2' tiles of highlighted tile
							- check destination tile background picture
								- if background picture exists, invalid move
							- check tile 'in-between' highlighted tile and destination tile for background picture
								- if background picture does not exist, invalid move
								- if it does exist but is the same color as player tile, invalid move
								- if it does exist and it is the opposite color as the player tile
									- remove background image for tile 'in-between'
									- give the player that made the play a point
									- remove highlight from highlighted button
									- put background image at destination button
									- scan destination tile for all possible moves it can make
										- if there exists a play / chance for another jump (destination tile has a valid directional move 2 spaces away from it self that has an enemy tile in-between and free landing space) continue turn
											- swap highlight to destination
											- don't allow the user to unhighlight it somehow.. flag? another turn?
											//if they hit the "end turn" button early then you can unhighlight it there
										- else end turn
						- if destination tile is further than 2 tiles away //else
							- invalid move	
						- check at the end of all this. if destination tile = enemy backline / coords depend on team color
							- promote piece to king/change tag
			if this isn't highlighted and there aren't any other buttons highlighted
				- highlight this button
		Endturn_clicked(){
		 - clear local variables
		 - clear button highlights
		 - end turn flag
		 - disable buttons 
		 - run code for AI play here
			...
		 - animate AI play somehow or highlight briefly and wait()?
		 - unlock buttons
		 - flag user turn
		 - 

		}
main.
run program
loop program while (gameover = false)
	- player turn //access to UI elements
		- end turn // disable buttons
	- cpu turn //auto play
	- check win condition
        */






	}
}
