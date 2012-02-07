import java.util.Iterator;
import java.util.List;

public class CompiledPlayerAlphaBeta extends AmazonsPlayerSearch {

    public double alphaBeta (boolean topLevel, int depth, GameState gs, double alpha, double beta, Evaluator ev, BestMoveSoFar move) {
        if (depth <= 0)
            return ev.evaluateGameState(gs);
        
        List c = gs.getAllLegalMoves();    
        Iterator i = c.iterator();
        while (i.hasNext()) {
            Move currentMove = (Move) i.next();
            GameState hypothetical = gs.getResultingGameState(currentMove);
            double currentMoveEval = -alphaBeta(false, depth-1, hypothetical, -beta, -alpha, ev, move);

            if (currentMoveEval >= beta && !topLevel)
                return beta;
            if (currentMoveEval > alpha) {
                alpha = currentMoveEval;
                if (topLevel)
                    move.set(currentMove);
            }
        }

        //if (alpha < -10000)
        //    alpha = ev.evaluateGameState(gs) - 10000;
        
        return alpha;
    }

    public void generateMove (GameState gs, BestMoveSoFar move) {
        double alpha = -1.0/0.0;
        double beta = 1.0/0.0;
        alphaBeta(true, 1, gs, alpha, beta, evaluator, move);
        
        /*
        //TESTING:
        if(move.getMove() == null)
            return;
        
        double abVal = evaluator.evaluateGameState(new GameState(gs, move.getMove()));     
        BestMoveSoFar sanityCheck = new BestMoveSoFar();
        AmazonsPlayerNegaMax nm = new AmazonsPlayerNegaMax();
        nm.setEvaluator(evaluator);
        nm.generateMove(gs, sanityCheck);
        double nmVal = evaluator.evaluateGameState(new GameState(gs, sanityCheck.getMove()));
        System.out.println("Move: " + abVal + ", Sanity: " + nmVal);
        */
    }

}
