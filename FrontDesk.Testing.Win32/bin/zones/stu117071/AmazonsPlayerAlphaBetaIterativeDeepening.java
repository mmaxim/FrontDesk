import java.util.*;

public class AmazonsPlayerAlphaBetaIterativeDeepening extends AmazonsPlayerAlphaBeta implements AmazonsPlayer {
    
    private HashMap branches;
    
    public void iterativeDeepening (GameState gs, BestMoveSoFar move, int maximumDepth) {
     
       //Store the decisions made in a HashMap so they can be checked first in deeper searches
       branches = new HashMap();
       for(int i = 1; i < maximumDepth; i++)
           alphaBeta(true, gs, -1.0/0.0, 1.0/0.0, move, i);
              
    } 
    
    public void generateMove (GameState gs, BestMoveSoFar move) {
        iterativeDeepening(gs, move, 15);  //We do not go past a depth of 15 because this only occurs unnecessarily at the endgame.
       
    }
    
    //This modified alphabeta uses branches when possible to place the previous best move at the front of the list.
    private double alphaBeta(boolean topLevel, GameState gs, double alpha, double beta, BestMoveSoFar move, int depth)
    {
        if(depth == 0)
            return evaluator.evaluateGameState(gs);
        
 
        List legalMoves = gs.getAllLegalMoves();
        String gameState = gs.toString()+gs.isWhitesTurn();
        //If the gamestate has been seen before, red
        if(branches.containsKey(gameState)) {
            Move am = (Move)branches.get(gameState);
            if(gs.legalMove(am))
                for(int i = 0; i < legalMoves.size(); i++)
                    if (((Move)legalMoves.get(i)).toString().equals(am.toString())) {
                        //Insert the best move previously found at the beginning of the list.
                        legalMoves.remove(i);
                        legalMoves.add(0, am);
                        break;
                    }

        }
        
        for(Iterator i = legalMoves.iterator(); i.hasNext() && alpha < beta;) {
            AmazonsMove moveToMake = (AmazonsMove)i.next();
        
            double val = -alphaBeta(false, gs.getResultingGameState(moveToMake), -beta, -alpha, move, depth - 1);
            //If this is the best value seen then update alpha and add the move and gamestate to branches.
            if(val > alpha) {
                alpha = val;
                branches.put(gameState, moveToMake);
                if(topLevel)
                    move.set(moveToMake);
            }
       }
       
       return alpha;
   }
        
}


