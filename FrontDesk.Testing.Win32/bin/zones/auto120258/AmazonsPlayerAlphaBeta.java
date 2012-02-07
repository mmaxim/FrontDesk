
import java.util.*;

public class AmazonsPlayerAlphaBeta extends AmazonsPlayerSearch {

    public double alphaBeta (boolean topLevel, int depth, GameState gs, double alpha, double beta, Evaluator ev, BestMoveSoFar move) {
       //If the maximum depth has been reached return the evaluation of the current game state.
       if(depth == 0)
         return ev.evaluateGameState(gs);
       
       List legalMoves = gs.getAllLegalMoves();
       
       for(Iterator i = legalMoves.iterator(); i.hasNext() && alpha < beta;) {
           Move moveToMake = (Move)i.next();
           //Recursively call the method switching the order and signs of alpha and beta so the same comparisons can be used.
           double val = -alphaBeta(false, depth - 1, gs.getResultingGameState(moveToMake), -beta, -alpha, ev, move);
           if(val > alpha) {
               alpha = val;
               if(topLevel) {
                   move.set(moveToMake);}
           }
        
       }
       
       return alpha;
    }

    public void generateMove (GameState gs, BestMoveSoFar move) {
        double alpha = -1.0/0.0;
        double beta = 1.0/0.0;
       // Evaluator evaluator = new AmazonsEvaluatorSimple();
        alphaBeta(true, 1, gs, alpha, beta, evaluator, move);
    }

}
