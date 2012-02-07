import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class AmazonsGameState implements GameState {
    public static final char ARROW = 'X';
    public static final char WHITE = 'W';
    public static final char BLACK = 'B';
    public static final char EMPTY = 0;
    public static final char OFF_BOARD = (char)-1;
    
    
    public static final int BOARD_SIZE = 10;
    
    public static final int[] INIT_BLACK_POS_X = {0, 3, 6, 9};
    public static final int[] INIT_BLACK_POS_Y = {3, 0, 0, 3};
    public static final int[] INIT_WHITE_POS_X = INIT_BLACK_POS_X;
    public static final int[] INIT_WHITE_POS_Y = {6, 9, 9, 6};
    /*
    public static final int BOARD_SIZE = 8;

    public static final int[] INIT_BLACK_POS_X = {0, 1};
    public static final int[] INIT_BLACK_POS_Y = {3, 5};
    public static final int[] INIT_WHITE_POS_X = {3, 5};
    public static final int[] INIT_WHITE_POS_Y = {0, 1};
    */
    
    public static final int NUM_QUEENS = INIT_BLACK_POS_X.length;

    private char[][] squareContents;
    private boolean whitesTurn;
    
	public char getSquareContents (int x, int y) {
        //if (x < 0 || y < 0 || x >= BOARD_SIZE || y >= BOARD_SIZE)
        //    return OFF_BOARD;
            
        return squareContents[x][y];
    }
    
    public char getSquareContentsCheckBounds (int x, int y) {
        if (x < 0 || y < 0 || x >= BOARD_SIZE || y >= BOARD_SIZE)
            return OFF_BOARD;
            
        return squareContents[x][y];
    }
    
    
    public boolean isOccupied (int x, int y) {
        return squareContents[x][y] != EMPTY;
    }
    
    public AmazonsGameState () {
        squareContents = new char[BOARD_SIZE][BOARD_SIZE];
        
        for (int i=0; i<NUM_QUEENS; i++) {
            squareContents[INIT_BLACK_POS_X[i]][INIT_BLACK_POS_Y[i]] = BLACK;
            squareContents[INIT_WHITE_POS_X[i]][INIT_WHITE_POS_Y[i]] = WHITE;
        }
        
        whitesTurn = true;
    }
    
    public AmazonsGameState (AmazonsGameState gs) {
        squareContents = new char[AmazonsGameState.BOARD_SIZE][AmazonsGameState.BOARD_SIZE];
        for (int i=0; i<AmazonsGameState.BOARD_SIZE; i++)
            for (int j=0; j<AmazonsGameState.BOARD_SIZE; j++)
                squareContents[i][j] = gs.squareContents[i][j];
        
        this.whitesTurn = gs.whitesTurn;
    }
    
    public AmazonsGameState (AmazonsGameState gs, AmazonsMove m) {
        this(gs);
        
        applyMove(m);
    }
    
    public String toString () {
        char[][] boardVal = new char[BOARD_SIZE][BOARD_SIZE];
        for (int i=0; i<BOARD_SIZE; i++)
            for (int j=0; j<BOARD_SIZE; j++) {
                boardVal[j][i] = squareContents[i][j];
                if (boardVal[j][i] == 0)
                    boardVal[j][i] = ' ';
            }
        
        StringBuffer sb = new StringBuffer();
        for (int i=0; i<BOARD_SIZE; i++) {
            sb.append(boardVal[i]);
            sb.append('\n');
        }
        
        return sb.toString();
    }
    
    public boolean isLine (int x1, int y1, int x2, int y2) {
        if (x1 < 0 || x1 >= BOARD_SIZE)
            return false;
        if (y1 < 0 || y1 >= BOARD_SIZE)
            return false;
        if (x2 < 0 || x2 >= BOARD_SIZE)
            return false;
        if (y2 < 0 || y2 >= BOARD_SIZE)
            return false;
        
        int dx = Math.abs(x2 - x1);
        int dy = Math.abs(y2 - y1);
        
        if (dx == 0 && dy == 0)
            return false;
        return (dx == 0 || dy == 0 || dx == dy);
    }
    
    public boolean existsPath (int x1, int y1, int x2, int y2) {
        if (!isLine(x1, y1, x2, y2)) {
            System.err.println("existsPath failed because isLine failed");
            return false;
        }
        
        int dx = x2-x1;
        if (dx != 0)
            dx = dx>0? 1 : -1;
            
        int dy = y2-y1;
        if (dy != 0)
            dy = dy>0? 1 : -1;
        
        while (x1 != x2 || y1 != y2) {
            if (isOccupied(x1, y1)) {
                System.err.println("existsPath failed at: " + x1 + "," + y1);
                return false;
            }
            x1 += dx;
            y1 += dy;
        }
        
        return true;
    }
    
    public boolean legalMove (Move m) {
        AmazonsMove am = (AmazonsMove)m;
        return legalMove(am);
    }
    
    public boolean legalMove (AmazonsMove m) {
        if (m == null)
            return false;
        
        char moveStart = getSquareContents(m.queenOldX, m.queenOldY);
        char currentColor = whitesTurn? 'W' : 'B';
        if (moveStart != currentColor) {
            System.err.println("Illegal move because of wrong color queen");
            return false;
        }
        
        squareContents[m.getQueenOldX()][m.getQueenOldY()] = 0;
        
        boolean isLegal = existsPath(m.getQueenOldX(), m.getQueenOldY(), m.getQueenNewX(), m.getQueenNewY());
        isLegal = isLegal && !isOccupied(m.getQueenNewX(), m.getQueenNewY());
        
        if (isOccupied(m.getQueenNewX(), m.getQueenNewY()))
            System.err.println("Illegal move because of occupied space for move [" + m.getQueenNewX() + "," + m.getQueenNewY() + "]");
        
        isLegal = isLegal && existsPath(m.getQueenNewX(), m.getQueenNewY(), m.arrowX, m.arrowY);
        isLegal = isLegal && !isOccupied(m.getArrowX(), m.getArrowY());
        
        if (isOccupied(m.getArrowX(), m.getArrowY()))
            System.err.println("Illegal move because of occupied space for arrow [" + m.arrowX + "," + m.arrowY + "]");

        squareContents[m.getQueenOldX()][m.getQueenOldY()] = moveStart;
        
        return isLegal;
    }
    
    private void applyMove (AmazonsMove m) {
        squareContents[m.queenNewX][m.queenNewY] = squareContents[m.queenOldX][m.queenOldY];
        squareContents[m.queenOldX][m.queenOldY] = EMPTY;
        squareContents[m.arrowX][m.arrowY] = ARROW;
        
        whitesTurn = !whitesTurn;
    }
    
    public GameState getResultingGameState (Move m) {
        AmazonsMove am = (AmazonsMove)m;
        GameState gs = new AmazonsGameState(this, am);
        return gs;
    }
    
    private void getAllLegalMovesInDirection (List c, int queenOldX, int queenOldY, int queenNewX, int queenNewY,
            int dx, int dy) {
        if (dx == 0 && dy == 0)
            return;
        
        int x = queenNewX;
        int y = queenNewY;
        
        x += dx;
        y += dy;
        
        while (x >= 0 && x < BOARD_SIZE &&
               y >= 0 && y < BOARD_SIZE &&
               !isOccupied(x, y)) {
            AmazonsMove m = new AmazonsMove(queenOldX, queenOldY, queenNewX, queenNewY, x, y);
            c.add(m);
            x += dx;
            y += dy;
        }
    }
    
    private void getAllLegalMoves (List c, int queenOldX, int queenOldY, int queenNewX, int queenNewY) {
        char saved = getSquareContents(queenOldX, queenOldY);
        squareContents[queenOldX][queenOldY] = 0;
        
        getAllLegalMovesInDirection(c, queenOldX, queenOldY, queenNewX, queenNewY, -1, -1);
        getAllLegalMovesInDirection(c, queenOldX, queenOldY, queenNewX, queenNewY, -1, 0);
        getAllLegalMovesInDirection(c, queenOldX, queenOldY, queenNewX, queenNewY, -1, 1);
        getAllLegalMovesInDirection(c, queenOldX, queenOldY, queenNewX, queenNewY, 0, -1);
        getAllLegalMovesInDirection(c, queenOldX, queenOldY, queenNewX, queenNewY, 0, 1);
        getAllLegalMovesInDirection(c, queenOldX, queenOldY, queenNewX, queenNewY, 1, -1);
        getAllLegalMovesInDirection(c, queenOldX, queenOldY, queenNewX, queenNewY, 1, 0);
        getAllLegalMovesInDirection(c, queenOldX, queenOldY, queenNewX, queenNewY, 1, 1);
        
        squareContents[queenOldX][queenOldY] = saved;
    }
    
    private int getNumPositionsToMoveInDirection (int queenX, int queenY,
            int dx, int dy) {
        if (dx == 0 && dy == 0)
            return 0;
        int toReturn = 0;
        
        int x = queenX += dx;
        int y = queenY += dy;
        
        while (x >= 0 && x < BOARD_SIZE &&
               y >= 0 && y < BOARD_SIZE &&
               !isOccupied(x, y)) {
            toReturn++;
            x += dx;
            y += dy;
        }
        
        return toReturn;
    }
    
    private int getNumPositionsToMove (int queenX, int queenY) {
        int toReturn = 0;
        
        toReturn += getNumPositionsToMoveInDirection(queenX, queenY, -1, -1);
        toReturn += getNumPositionsToMoveInDirection(queenX, queenY, -1, 0);
        toReturn += getNumPositionsToMoveInDirection(queenX, queenY, -1, 1);
        toReturn += getNumPositionsToMoveInDirection(queenX, queenY, 0, -1);
        toReturn += getNumPositionsToMoveInDirection(queenX, queenY, 0, 1);
        toReturn += getNumPositionsToMoveInDirection(queenX, queenY, 1, -1);
        toReturn += getNumPositionsToMoveInDirection(queenX, queenY, 1, 0);
        toReturn += getNumPositionsToMoveInDirection(queenX, queenY, 1, 1);
        
        return toReturn;
    }
    
    private void getAllLegalMovesInDirection (List c, int queenX, int queenY,
            int dx, int dy) {
        if (dx == 0 && dy == 0)
            return;
        
        int x = queenX;
        int y = queenY;
        
        x += dx;
        y += dy;
        
        while (x >= 0 && x < BOARD_SIZE && y >= 0 && y < BOARD_SIZE && !isOccupied(x, y)) {
            getAllLegalMoves(c, queenX, queenY, x, y);
            x += dx;
            y += dy;
        }
    }
    
    private void getAllLegalMoves (List c, int queenX, int queenY) {
        getAllLegalMovesInDirection(c, queenX, queenY, -1, -1);
        getAllLegalMovesInDirection(c, queenX, queenY, -1, 0);
        getAllLegalMovesInDirection(c, queenX, queenY, -1, 1);
        getAllLegalMovesInDirection(c, queenX, queenY, 0, -1);
        getAllLegalMovesInDirection(c, queenX, queenY, 0, 1);
        getAllLegalMovesInDirection(c, queenX, queenY, 1, -1);
        getAllLegalMovesInDirection(c, queenX, queenY, 1, 0);
        getAllLegalMovesInDirection(c, queenX, queenY, 1, 1);
    }
    
    // Doesn't count arrow possibilities
    public int getNumMovePositions (boolean forWhite) {
        int toReturn = 0;
        
        char colorToCheck = forWhite? WHITE : BLACK;
        
        for (int i=0; i<BOARD_SIZE; i++) {
            for (int j=0; j<BOARD_SIZE; j++) {
                if (getSquareContents(i, j) == colorToCheck)
                    toReturn += getNumPositionsToMove(i, j);
            }
        }
        
        return toReturn;
    }
    
	public List getAllLegalMoves () {
        List toReturn = new ArrayList();
        
        char currentColor = whitesTurn? WHITE : BLACK;
        
        for (int i=0; i<BOARD_SIZE; i++) {
            for (int j=0; j<BOARD_SIZE; j++) {
                if (getSquareContents(i, j) == currentColor)
                    getAllLegalMoves(toReturn, i, j);
            }
        }
        
        if (toReturn.isEmpty()) {
            int numMoves = getNumMovePositions(whitesTurn);
            if (numMoves != 0)
                System.err.println("Failed to find legal move");
        }
        
        // Comment out for deterministic play
        Collections.shuffle(toReturn);
        
		return toReturn;
	}

    public boolean isWhitesTurn () {
        return whitesTurn;
    }
}
