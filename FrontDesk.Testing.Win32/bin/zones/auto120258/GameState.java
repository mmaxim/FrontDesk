import java.util.List;



public interface GameState {

    public boolean legalMove (Move m);

    public List getAllLegalMoves ();

    public GameState getResultingGameState (Move m);
    
    public boolean isWhitesTurn ();
}