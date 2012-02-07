
public class AmazonsEvaluatorSimple extends AmazonsEvaluator {
    private double adjacentFreeSpaces (AmazonsGameState gs, int x, int y) {
        int toReturn = 0;
        if (gs.getSquareContentsCheckBounds((int)(x-1), (int)(y-1)) == AmazonsGameState.EMPTY)
            toReturn++;
        if (gs.getSquareContentsCheckBounds((int)(x-1), y) == AmazonsGameState.EMPTY)
            toReturn++;
        if (gs.getSquareContentsCheckBounds((int)(x-1), (int)(y+1)) == AmazonsGameState.EMPTY)
            toReturn++;
        if (gs.getSquareContentsCheckBounds((int)(x), (int)(y-1)) == AmazonsGameState.EMPTY)
            toReturn++;
        if (gs.getSquareContentsCheckBounds((int)(x), (int)(y+1)) == AmazonsGameState.EMPTY)
            toReturn++;
        if (gs.getSquareContentsCheckBounds((int)(x+1), (int)(y-1)) == AmazonsGameState.EMPTY)
            toReturn++;
        if (gs.getSquareContentsCheckBounds((int)(x+1), y) == AmazonsGameState.EMPTY)
            toReturn++;
        if (gs.getSquareContentsCheckBounds((int)(x+1), (int)(y+1)) == AmazonsGameState.EMPTY)
            toReturn++;
        return toReturn;
    }
    
    public double evaluateGameState (AmazonsGameState gs) {
        double eval = 0;
        
        char currentColor = gs.isWhitesTurn()? AmazonsGameState.WHITE : AmazonsGameState.BLACK;
        char oppositeColor = gs.isWhitesTurn()? AmazonsGameState.BLACK : AmazonsGameState.WHITE;
        
        for (int i=0; i<AmazonsGameState.BOARD_SIZE; i++)
            for (int j=0; j<AmazonsGameState.BOARD_SIZE; j++) {
                if (gs.getSquareContents(i, j) == currentColor)
                    eval += adjacentFreeSpaces(gs, i, j);
                else if (gs.getSquareContents(i, j) == oppositeColor)
                    eval -= adjacentFreeSpaces(gs, i, j);
            }
        
        return eval;
    }
}
