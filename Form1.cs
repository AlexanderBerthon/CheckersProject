using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersProject {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
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

        on click(){
            if firstbutton == null
                set first button = this.first button
            else
                actual logic
                ...
                if(capture successful and additonal capture can be made){
                    firstbutton = second button?
                    if(user clicks the end turn button){
                        first button = null;
                    else
                    let the user go again, but only if they can make a second capture. you can't just give them a normal
                    turn because they have to chain them if they want to use the second turn.
                    they can't for example make a capture then move to the left and avoid danger.
                    
                firstbutton = null;
        */






    }
}
