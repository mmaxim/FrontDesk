import java.util.Iterator;
import java.util.List;

public class CompiledPlayerNegaMax extends AmazonsPlayerSearch {

    public double negaMax (boolean topLevel, int depth, GameState gs, Evaluator ev, BestMoveSoFar move) {
        if (depth <= 0)
            return ev.evaluateGameState(gs);

        double bestValSoFar = -1.0 / 0.0;
        List c = gs.getAllLegalMoves();
        Iterator i = c.iterator();
        while (i.hasNext()) {
            Move currentMove = (Move) i.next();
            GameState hypothetical = gs.getResultingGameState(currentMove);
            double currentMoveEval = -negaMax(false, depth-1, hypothetical, ev, move);
            if (currentMoveEval > bestValSoFar) {
                bestValSoFar = currentMoveEval;
                if (topLevel)
                    move.set(currentMove);
            }
        }

        //if (bestValSoFar < -10000)
        //    bestValSoFar = ev.evaluateGameState(gs) - 10000;
            
        return bestValSoFar;
    }

    public void generateMove (GameState gs, BestMoveSoFar move) {
        negaMax(true, 1, gs, evaluator, move);
    }
}