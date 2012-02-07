
// Implement "negaMax" version of MiniMax search
        
        // A game state can be evaluated with:
        //      ev.evaluateGameState(gs);
        // The evaluators return positive when the current player (to the game state)
        // is winning.
        
        // Possible moves can be gotten with:
        //      List c = gs.getAllLegalMoves();
        
        // A move can be used to create a game state with:
        //      GameState hypothetical = gs.getResultingGameState(currentMove);
        
        // The current best move so far should be stored as:
        //      move.set(currentMove);

import java.util.Iterator;
import java.util.List;

public class AmazonsPlayerNegaMax extends AmazonsPlayerSearch {

    public double negaMax (boolean topLevel, int depth, GameState gs, Evaluator ev, BestMoveSoFar move) {
        if(depth == 0)
            return ev.evaluateGameState(gs);
        //Store the best gamestate's value.
        double valueOfGameBest = Double.NEGATIVE_INFINITY;
        
        List legalMoves = gs.getAllLegalMoves();
       
        for(int i = 0; i < legalMoves.size(); i++) {
            Move moveToMake = (Move)legalMoves.get(i);
            double valueOfCurrGame = -negaMax(false, depth - 1, gs.getResultingGameState(moveToMake), ev, move);
            //Update the best value and the move if at the top level. 
            if(valueOfGameBest < valueOfCurrGame) {
                valueOfGameBest = valueOfCurrGame;
                if(topLevel)
                    move.set(moveToMake);
            }
        } 
        
        return valueOfGameBest;
    }

    public void generateMove (GameState gs, BestMoveSoFar move) {
        negaMax(true, 1, gs, evaluator, move);
    }
    

 }