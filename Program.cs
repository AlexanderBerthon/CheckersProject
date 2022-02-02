using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersProject {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            /*
            Boolean gameOver = false;
            Form1 game = new Form1();
            */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); //.run(game);

            /*
            while (!gameOver) {
                game.p1Turn();
                game.AITurn();
            }
            */

        }
    }
}
