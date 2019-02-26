using System;
using System.Collections.Generic;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Battleship!\n");
            Battleship mBattleShip = new Battleship();
        }
    }

    /* BATTLESHIP */
    class Battleship {
        private static int numberOfPlayers = 2;
        private IList<Player> players = new List<Player>();

        public Battleship() {
            initialisePlayers();
            Console.WriteLine("\nGame initialised----------");
            initialiseGame();
        }

        private void initialiseGame() {
            //set the varaibles to negative for checks later
            int x=-1, y=-1;
            foreach(Player player in players){
                //set the oppponent for the plater
                setOpponent(player);
                player.getAttackPoints(ref x, ref y, -1);
                //attack once we have the right coordinates
                attack(player, x, y);
                
                //check to see if game finished after attack,
                //display a message if it has
                if(gameFinished()) {
                    Console.WriteLine(player.getName() + " has won the game by sinking all the opponent's warships\n");
                    break;
                }
            }


            if(!gameFinished()) {
                //initialise if the game hasn't finished
                initialiseGame();
            }
        }

        //set the opponent for the player
        private void setOpponent(Player p){
            foreach(Player player in players){
                if(player != p ) {
                    p.setOpponent(player);
                }
            }
        }

        private void attack(Player p, int x, int y) {
            foreach(Player player in players){
                if(player != p) {
                    player.updatePoint(x, y);
                    //player.getPlayerBoard().displayWarships();
                }
            }
        }

        private Boolean gameFinished() {
            Boolean toReturn = false;

            foreach(Player player in players) {
                if(player.hasWon()) {
                    toReturn = true;
                }
            }
            return toReturn;
        }

        /* initialise players */
        private void initialisePlayers() {
            for (int i =0; i < numberOfPlayers; i++){
                Player mPlayer = new Player("Player " + (i+1));
                players.Add(mPlayer);
            }
        }
    }

    /* PLAYER */
    class Player {
        private string name;
        private Boolean won;
        private int battleShips;    //number of battleships
        
        private Board playerBoard;
        private Player opponent;

        public Player(string name) {
            //set the won to false
            won = false;
            this.name = name;
            //initialise the board for the player
            this.playerBoard = new Board();
            Console.WriteLine("\nPlayer Name: " + name);

            //prompt to place the battleships on the board
            battleShipsPrompt();
        }

        public void setOpponent(Player player) => opponent = player;

        public void getAttackPoints(ref int x, ref int y, int msg) {
            if(msg == 1) {
                //if the supplied coordinates are outside the grid
                Console.WriteLine("\nSorry, coordinates outside the board. Please try again!");
            } else if (msg == 2){
                //if the coordinates have already been hit
                Console.WriteLine("\nSorry, warship on this coordinate has already been hit. Please try again!");
            }

            int tempx, tempy;
            Console.WriteLine("\n" + name + " to attack");

            //get the X cordinate for attack
            Console.Write("X coordinate to attack on opponent's board: ");
            tempx = Convert.ToInt32(Console.ReadLine());
            //get the Y coordinate for attack
            Console.Write("Y coordinate to attack on Oppnonet's board: ");
            tempy = Convert.ToInt32(Console.ReadLine());

            //check the user input to see if it's positive
            //and if it's within the grid
            if((tempx >= 0 && tempy >=0) && (tempx < playerBoard.getDimension() && tempy < playerBoard.getDimension())) {
                //if the warship has already been hit, display a message
                if(opponent.playerBoard.getPoint(tempx,tempy).isHit()){
                    getAttackPoints(ref x,ref y, 2);
                } else {
                    //else attach the point
                    x = tempx;
                    y= tempy;
                }
            } else {
                //coordinates outside the board
                getAttackPoints(ref x,ref y,1);
            }
        }

        private void battleShipsPrompt() {
            //temp variable
            int tempBattleships;
            Console.Write("Number of battleships?: ");
            //convert the string to int
            tempBattleships = Convert.ToInt32(Console.ReadLine());

            //check if the number of battleships is within the dimensions
            if(tempBattleships > playerBoard.numberOfBattleships()){
                //if exceeded, print to let the user know
                Console.WriteLine("Sorry, maximum battleships exceeded");
                //and run the process again
                battleShipsPrompt();
            } else {
                battleShips = tempBattleships;
            }
            //once done, generate random co ordinates for the warships
            playerBoard.generateRandomCoordinates(battleShips);
            //playerBoard.displayBoard();
        }

        public Boolean hasWon() {
            //return won;
            return playerBoard.hasSunk();
        }

        public Board getPlayerBoard(){
            return playerBoard;
        }

        public void updatePoint(int x, int y) {
            playerBoard.updatePoint(x, y);
        }

        public string getName() {
            return name;
        }
    }

    /* BOARD for a player */
    class Board {
        //the grid can only be 10x10
        private static int dimension = 10;  

        //number of battleships that can be placed within the board
        private static int battleShips = 100;   
        //points within the grid, each square is a grid
        private Point[,] points = new Point[dimension, dimension];

        public Board() {
            //set up all the points/dots within the board
            setupPoints();
        }

        public int getDimension() {
            return dimension;
        }

        /* display all the points within the board */
        public void displayBoard() {
            for(int i = 0; i < points.GetLength(0); i++){
                for(int j=0; j < points.GetLength(1); j++) {
                    points[i,j].printPoint();
                }
            }
        }

        /* just display warships location in form of x,y coordinates */
        public void displayWarships() {
            for(int i = 0; i < points.GetLength(0); i++){
                for(int j=0; j < points.GetLength(1); j++) {
                    if(points[i,j].hasBattleship()) {
                        points[i,j].printWarship();
                    }
                }
            }
        }

        public int numberOfBattleships() {
            return battleShips;
        }

        /* based on the dimensions declared in the class,
            setup all the points as the POINT object */
        private void setupPoints() {
            for(int i = 0; i < points.GetLength(0); i++){
                for(int j=0; j < points.GetLength(1); j++) {
                    points[i,j] = new Point(i,j);
                }
            }
        }

        /* check to see if a player's board has been sunk,
         */
        public Boolean hasSunk() {
            //set the boolean to true
            Boolean hasSunk = true;
            //loop through the array
            for(int i = 0; i < points.GetLength(0); i++){
                for(int j=0; j < points.GetLength(1); j++) {
                    //get the point
                    Point p = points[i,j];
                    //check if it's occupied
                    if(p.hasBattleship()){
                        //if it's occupied, check if it has been hit
                        if(!p.isHit()) {
                            //if it hasn't been hit, set the return variable to false
                            //meaning it hasn't been sunk
                            hasSunk = false;
                        }
                    }
                }
            }
            return hasSunk;
        }

        /* based on the user prompt/feedback, generate random
        numbers that act as coordinates for battleships
         */
        public void generateRandomCoordinates(int t){
            for (int i =0; i < t; i++) {
                Random r = new Random();
                //dimension variable makes sure that the random
                //variable is within the board dimensions
                int randomX = r.Next(0, dimension);
                int randomY = r.Next(0, dimension);

                Console.WriteLine("Random warcraft: [" + randomX + ", " + randomY + "]");
                //set it to occupied
                points[randomX, randomY].setOccupied(true);


            }
        }

        public void updatePoint(int x, int y) {
            //Console.WriteLine(this.points[x,y].hasBattleship());
            this.points[x,y].hasBeenHit();
        }

        public Point getPoint(int x, int y){
            return this.points[x,y];
        }

        public void pointHit(int x, int y){
            for(int i = 0; i < points.GetLength(0); i++){
                for(int j=0; j < points.GetLength(1); j++) {
                    if(x==i && y==j){
                        points[i,j].printWarship();
                    }
                }
            }
        }
    }

    /* Point class, individual squares on the board */
    class Point {
        //has battleship variable
        private Boolean hasBS;
        private Boolean hit;
        //x cordinate on the 10x10 grid
        private int x;
        //y coordinate on the 10x10 grid
        private int y;

        public Point() {
            //empty constructor

        }

        public Point(int x, int y) {
            this.hasBS = false;
            this.hit = false;

            this.x = x;
            this.y = y;
        }

        /* print point details */
        public void printPoint() {
            /* for the sake of this exercise, only print if a battleship is present */
            //if(hasBS) {
                Console.WriteLine("-------------");
                Console.WriteLine("Point [" + x + "], [" + y + "]");
                Console.WriteLine("Has Battleship: " + this.hasBS);
                Console.WriteLine("Hit: " + this.hit);
            //}
        }

        public int getX() {
            return this.x;
        }

        public void setX(int xx){
            this.x = xx;
        }

        public int getY() {
            return this.y;
        }

        public void setY(int yy) {
            this.y = yy;
        }

        public Boolean isHit() {
            return hit;
        }

        public void hasBeenHit() {
            if(hasBS){
                Console.WriteLine("HIT!");
                hit = true;
            } else {
                Console.WriteLine("MISS!");
            }
        }

        public void setOccupied(Boolean o) {
            this.hasBS = o;
        }

        public Boolean hasBattleship() {
            return this.hasBS;
        }

        public void printWarship() {
            Console.WriteLine("[" + x + ", " + y + "] hasWarship: " + hasBS + " isHit: " + hit);
        }
    }
}
