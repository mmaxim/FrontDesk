import java.awt.Point;
import java.util.Set;
import java.util.HashSet;

public class AmazonsEvaluatorCustom extends AmazonsEvaluator {

    public AmazonsEvaluatorCustom() {
        super();
    }

    public double evaluateGameState (AmazonsGameState gs) {
        double eval = 0;
        
        int[][] whiteQ = new int[10][10], whiteK = new int[10][10];
        int[][] blackQ = new int[10][10], blackK = new int[10][10];
        
        for(int i = 0; i < 10; i++)
          for(int j = 0; j < 10; j++) {
            whiteQ[i][j] = Integer.MAX_VALUE;
            blackQ[i][j] = Integer.MAX_VALUE;
            whiteK[i][j] = Integer.MAX_VALUE;
            blackK[i][j] = Integer.MAX_VALUE;

          }
        
        move(gs, whiteQ, blackQ, whiteK, whiteK);
        
        int blackOwnedQ = 0, whiteOwnedQ = 0, neutralQ = 0;
        int blackOwnedK = 0, whiteOwnedK = 0, neutralK = 0;
		//For each square on the board, determine which player owns it or whether it is neutral through the two types of moves.
		//BY owns it we mean that they are a shorter number of moves away.
        for(int i = 0; i < 10; i++)
          for(int j = 0; j < 10; j++) {
            if(whiteQ[i][j] > blackQ[i][j])
              blackOwnedQ++;
            else if(whiteQ[i][j] < blackQ[i][j])
              whiteOwnedQ++;
            else
              neutralQ++;
            
            if(whiteK[i][j] > blackK[i][j])
              blackOwnedK++;
            else if(whiteK[i][j] < blackK[i][j])
              whiteOwnedK++;
            else
              neutralK++;
            
          }
        
        double KingsMove = (blackOwnedK - whiteOwnedK) * (gs.isWhitesTurn() ? -1 : 1) + 0.2 * neutralK;
        //Return a composite score weighing kings move at 30% and queens move at 70%.
        return ((blackOwnedQ - whiteOwnedQ) * (gs.isWhitesTurn() ? -1 : 1) + 0.2 * neutralQ)*0.7 + 0.3*KingsMove + eval;
    }
    
    //Allows a breadth first Dijkstra like approach to calculating the number of moves an amazon requires to reach a square.
    private Queue Q = new Queue();
    
    private void move(AmazonsGameState gs, int[][] whiteQ, int[][] blackQ, int[][] whiteK, int[][] blackK)
    {   
		//These matrices keep track of whether a square has already been used for the respective type of move.
        boolean[][] whiteMatrixQ = new boolean[10][10];
        boolean[][] blackMatrixQ = new boolean[10][10];
        boolean[][] whiteMatrixK = new boolean[10][10];
        boolean[][] blackMatrixK = new boolean[10][10];
        
        //Initialize the matrices and add the queens of each color to their respective matrices.
        for(int i = 0; i < 10; i++)
          for(int j = 0; j < 10; j++) {
            whiteMatrixQ[i][j] = false;
            blackMatrixQ[i][j] = false;
            whiteMatrixK[i][j] = false;
            blackMatrixK[i][j] = false;
            int type = gs.getSquareContentsCheckBounds(i, j);
            if(type == AmazonsGameState.WHITE) {
              whiteQ[i][j] = 0;
              whiteK[i][j] = 0;
              Q.enqueue(new Point(i,j));
            }
            if(type == AmazonsGameState.BLACK) {
              blackQ[i][j] = 0;
              blackK[i][j] = 0;
              Q.enqueue(new Point(i,j));
            }
          }
        
        
        while(!Q.isEmpty()) {
          Point p = (Point)Q.dequeue();
          int x = (int)p.getX();
          int y = (int)p.getY();
         
          if(whiteQ[x][y] != Integer.MAX_VALUE && !whiteMatrixQ[x][y]) {
              whiteMatrixQ[x][y] = true;
              fillMatrixQM(gs, whiteQ, x, y);
          }
          if(blackQ[x][y] != Integer.MAX_VALUE && !blackMatrixQ[x][y]) {
              blackMatrixQ[x][y] = true;
              fillMatrixQM(gs, blackQ, x, y);
          }
          if(whiteK[x][y] != Integer.MAX_VALUE && !whiteMatrixK[x][y]) {
              whiteMatrixK[x][y] = true;
              fillMatrixKM(gs, whiteK, x, y);
          }
          if(blackK[x][y] != Integer.MAX_VALUE && !blackMatrixK[x][y]) {
              blackMatrixK[x][y] = true;
              fillMatrixKM(gs, blackK, x, y);
          }
        }
    }
    
    private void fillMatrixKM(AmazonsGameState gs, int[][] color, int x, int y)
    {
        int startValue = color[x][y] + 1;
       
        if (gs.getSquareContentsCheckBounds(x-1, y-1) == AmazonsGameState.EMPTY)
            if (color[x-1][y-1] > startValue) {
              color[x-1][y-1] = startValue;
              Q.enqueue(new Point(x-1, y-1));
            }
        
        
        
        if (gs.getSquareContentsCheckBounds(x-1, y) == AmazonsGameState.EMPTY )
            if (color[x-1][y] > startValue) {
              color[x-1][y] = startValue;
              Q.enqueue(new Point(x-1, y));
            }
        
        
        
         if (gs.getSquareContentsCheckBounds(x-1, y+1) == AmazonsGameState.EMPTY )
            if (color[x-1][y+1] > startValue) {
              color[x-1][y+1] = startValue;
              Q.enqueue(new Point(x-1, y+1));
            }
        
        
        if (gs.getSquareContentsCheckBounds(x, y-1) == AmazonsGameState.EMPTY )
            if (color[x][y-1] > startValue) {
              color[x][y-1] = startValue;
              Q.enqueue(new Point(x, y-1));
            }
        
        if (gs.getSquareContentsCheckBounds(x, y+1) == AmazonsGameState.EMPTY )
            if (color[x][y+1] > startValue) {
              color[x][y+1] = startValue;
              Q.enqueue(new Point(x, y+1));
            }        
        
        if (gs.getSquareContentsCheckBounds(x+1, y-1) == AmazonsGameState.EMPTY )
            if (color[x+1][y-1] > startValue) {
              color[x+1][y-1] = startValue;
              Q.enqueue(new Point(x+1, y-1));
            }
        
        if (gs.getSquareContentsCheckBounds(x+1, y) == AmazonsGameState.EMPTY )
            if (color[x+1][y] > startValue) {
              color[x+1][y] = startValue;
              Q.enqueue(new Point(x+1, y));
            }
            
        if (gs.getSquareContentsCheckBounds(x+1, y+1) == AmazonsGameState.EMPTY )
            if (color[x+1][y+1] > startValue) {
              color[x+1][y+1] = startValue;
              Q.enqueue(new Point(x+1, y+1));
            }
    }
    
    private void fillMatrixQM(AmazonsGameState gs, int[][] color, int x, int y)
    {
        int startValue = color[x][y] + 1;
        int index = 1;
    
        while (gs.getSquareContentsCheckBounds(x-index, y-index) == AmazonsGameState.EMPTY) {
            if (color[x-index][y-index] > startValue) {
              color[x-index][y-index] = startValue;
              Q.enqueue(new Point(x-index, y-index));
            }
            index++;            
        }
        index = 1;
        
        while (gs.getSquareContentsCheckBounds(x-index, y) == AmazonsGameState.EMPTY) {
            if (color[x-index][y] > startValue) {
              color[x-index][y] = startValue;
              Q.enqueue(new Point(x-index, y));
            }
            index++;
        }
        index = 1;
        
        while (gs.getSquareContentsCheckBounds(x-index, y+index) == AmazonsGameState.EMPTY) {
            if (color[x-index][y+index] > startValue) {
              color[x-index][y+index] = startValue;
              Q.enqueue(new Point(x-index, y+index));
            }
            index++;
        }
        index = 1;
        
        while (gs.getSquareContentsCheckBounds(x, y-index) == AmazonsGameState.EMPTY) {
            if (color[x][y-index] > startValue) {
              color[x][y-index] = startValue;
              Q.enqueue(new Point(x, y-index));
            }
            index++;
        }
        index = 1;
        
        while (gs.getSquareContentsCheckBounds(x, y+index) == AmazonsGameState.EMPTY) {
            if (color[x][y+index] > startValue) {
              color[x][y+index] = startValue;
              Q.enqueue(new Point(x, y+index));
            }
            index++;
        }
        index = 1;
        
        while (gs.getSquareContentsCheckBounds(x+index, y-index) == AmazonsGameState.EMPTY) {
            if (color[x+index][y-index] > startValue) {
              color[x+index][y-index] = startValue;
              Q.enqueue(new Point(x+index, y-index));
            }
            index++;
        }
        index = 1;
        
        while (gs.getSquareContentsCheckBounds(x+index, y) == AmazonsGameState.EMPTY) {
            if (color[x+index][y] > startValue) {
              color[x+index][y] = startValue;
              Q.enqueue(new Point(x+index, y));
            }
            index++;
        }
        index = 1;
        
        while (gs.getSquareContentsCheckBounds(x+index, y+index) == AmazonsGameState.EMPTY) {
            if (color[x+index][y+index] > startValue) {
              color[x+index][y+index] = startValue;
              Q.enqueue(new Point(x+index, y+index));
            }
            index++;
        }
	}

}
/*
import java.awt.Point;
import java.util.Set;
import java.util.HashSet;

public class AmazonsEvaluatorCustom extends AmazonsEvaluator {

    public AmazonsEvaluatorCustom() {
        super();
    }

    public double evaluateGameState (AmazonsGameState gs) {
        double eval = 0;
        
        /*char currentColor = gs.isWhitesTurn()? AmazonsGameState.WHITE : AmazonsGameState.BLACK;
        char oppositeColor = gs.isWhitesTurn()? AmazonsGameState.BLACK : AmazonsGameState.WHITE;
        
        int[] goodQueens = {0,0,0,0};
        
        for (int i=0; i<AmazonsGameState.BOARD_SIZE; i++)
            for (int j=0; j<AmazonsGameState.BOARD_SIZE; j++) {
                if (gs.getSquareContents(i, j) == currentColor) {
                    eval += mobilityHeuristic(gs, i, j);
                    goodQueens[getQuad(i,j)]++;
                }
                else if (gs.getSquareContents(i, j) == oppositeColor)
                    eval -= mobilityHeuristic(gs, i, j);
            }
            
        double whatever = 1.0;
        for(int i = 0; i < 4; i++)
          if (goodQueens[i] == 0)
            whatever -= 0.1;
          else if(goodQueens[i] == 1)
            whatever += 0.2;
          else if(goodQueens[i] == 2)
            whatever -= 0.3;
          else if(goodQueens[i] == 3)
            whatever -= 0.5;
          else 
            whatever -= 0.65;
        *//*
        
        int[][] white = new int[10][10];
        int[][] black = new int[10][10];
        
        for(int i = 0; i < 10; i++)
          for(int j = 0; j < 10; j++) {
            white[i][j] = Integer.MAX_VALUE;
            black[i][j] = Integer.MAX_VALUE;
          }
        
        queensMove(gs, white, black);
        
        int blackOwned = 0, whiteOwned = 0, neutral = 0;
        for(int i = 0; i < 10; i++)
          for(int j = 0; j < 10; j++) {
            if(white[i][j] > black[i][j])
              blackOwned++;
            else if(white[i][j] < black[i][j])
              whiteOwned++;
            else
              neutral++;
          }
        
        return (blackOwned - whiteOwned) * (gs.isWhitesTurn() ? -1 : 1) + 0.2 * neutral;
    }
    
    private int mobilityHeuristic(AmazonsGameState gs, int x, int y)
    {
        int[] moves = {0,0,0,0,0,0,0,0};
        
        while (gs.getSquareContentsCheckBounds((int)(x-moves[0]-1), (int)(y-moves[0]-1)) == AmazonsGameState.EMPTY)
            moves[0]++;
        while (gs.getSquareContentsCheckBounds((int)(x-moves[1]-1), y) == AmazonsGameState.EMPTY)
            moves[1]++;
        while (gs.getSquareContentsCheckBounds((int)(x-moves[2]-1), (int)(y+moves[2]+1)) == AmazonsGameState.EMPTY)
            moves[2]++;
        while (gs.getSquareContentsCheckBounds((int)(x), (int)(y-moves[3]-1)) == AmazonsGameState.EMPTY)
            moves[3]++;
        while (gs.getSquareContentsCheckBounds((int)(x), (int)(y+moves[4]+1)) == AmazonsGameState.EMPTY)
            moves[4]++;
        while (gs.getSquareContentsCheckBounds((int)(x+moves[5]+1), (int)(y-moves[5]-1)) == AmazonsGameState.EMPTY)
            moves[5]++;
        while (gs.getSquareContentsCheckBounds((int)(x+moves[6]+1), y) == AmazonsGameState.EMPTY)
            moves[6]++;
        while (gs.getSquareContentsCheckBounds((int)(x+moves[7]+1), (int)(y+moves[7]+1)) == AmazonsGameState.EMPTY)
            moves[7]++;
        
        int toReturn = 0, dirns = 0;
        
        for(int i = 0; i < 8; i++) {
          toReturn += moves[i];
          dirns    += moves[i] == 0 ? 0 : 1;
        }
        
        toReturn *= dirns;
        //System.out.println(toReturn);
        return toReturn/ (dirns <= 2 ? 2 : 1);
    }

    private int getQuad(int x, int y)
    {
        if(x < 5)
          if(y < 5)
            return 0;
          else
            return 1;
        else
          if(y < 5)
            return 2;
          else
            return 3;
    }
    
    private Queue Q = new Queue();
    
    private void queensMove(AmazonsGameState gs, int[][] white, int[][] black)
    {   
        for(int i = 0; i < 10; i++)
          for(int j = 0; j < 10; j++) {
            int type = gs.getSquareContentsCheckBounds(i, j);
            if(type == AmazonsGameState.WHITE) {
              white[i][j] = 0;
              Q.enqueue(new Point(i,j));
            }
            if(type == AmazonsGameState.BLACK) {
              black[i][j] = 0;
              Q.enqueue(new Point(i,j));
            }
          }
        
        Set whiteSet = new HashSet();
        Set blackSet = new HashSet();
        
        while(!Q.isEmpty()) {
          Point p = (Point)Q.dequeue();
         
          if(white[(int)p.getX()][(int)p.getY()] != Integer.MAX_VALUE && !whiteSet.contains(p))
            fillMatrix(gs, white, (int)p.getX(), (int)p.getY(), whiteSet);
          if(black[(int)p.getX()][(int)p.getY()] != Integer.MAX_VALUE && !blackSet.contains(p))
            fillMatrix(gs, black, (int)p.getX(), (int)p.getY(), blackSet);
        }
    }
    
    private void fillMatrix(AmazonsGameState gs, int[][] color, int x, int y, Set set)
    {
        set.add(new Point(x,y));
    
        int startValue = color[x][y];
        int index = 1;
        while (gs.getSquareContentsCheckBounds(x-index, y-index) == AmazonsGameState.EMPTY) {
            if (color[x-index][y-index] > startValue) {
              color[x-index][y-index] = startValue+1;
              Q.enqueue(new Point(x-index, y-index));
            }
            index++;
        }
        index = 1;
        while (gs.getSquareContentsCheckBounds(x-index, y) == AmazonsGameState.EMPTY) {
            if (color[x-index][y] > startValue) {
              color[x-index][y] = startValue+1;
              Q.enqueue(new Point(x-index, y));
            }
            index++;
        }
        index = 1;
        while (gs.getSquareContentsCheckBounds(x-index, y+index) == AmazonsGameState.EMPTY) {
            if (color[x-index][y+index] > startValue) {
              color[x-index][y+index] = startValue+1;
              Q.enqueue(new Point(x-index, y+index));
            }
            index++;
        }
        index = 1;
        while (gs.getSquareContentsCheckBounds(x, y-index) == AmazonsGameState.EMPTY) {
            if (color[x][y-index] > startValue) {
              color[x][y-index] = startValue+1;
              Q.enqueue(new Point(x, y-index));
            }
            index++;
        }
        index = 1;
        while (gs.getSquareContentsCheckBounds(x, y+index) == AmazonsGameState.EMPTY) {
            if (color[x][y+index] > startValue) {
              color[x][y+index] = startValue+1;
              Q.enqueue(new Point(x, y+index));
            }
            index++;
        }
        index = 1;
        while (gs.getSquareContentsCheckBounds(x+index, y-index) == AmazonsGameState.EMPTY) {
            if (color[x+index][y-index] > startValue) {
              color[x+index][y-index] = startValue+1;
              Q.enqueue(new Point(x+index, y-index));
            }
            index++;
        }
        index = 1;
        while (gs.getSquareContentsCheckBounds(x+index, y) == AmazonsGameState.EMPTY) {
            if (color[x+index][y] > startValue) {
              color[x+index][y] = startValue+1;
              Q.enqueue(new Point(x+index, y));
            }
            index++;
        }
        index = 1;
        while (gs.getSquareContentsCheckBounds(x+index, y+index) == AmazonsGameState.EMPTY) {
            if (color[x+index][y+index] > startValue) {
              color[x+index][y+index] = startValue+1;
              Q.enqueue(new Point(x+index, y+index));
            }
            index++;
        }
        
        
}}*/