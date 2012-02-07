
public class CompiledPlayerAlphaBetaIterativeDeepening extends CompiledPlayerAlphaBeta implements AmazonsPlayer {
    
    public void iterativeDeepening (GameState gs, BestMoveSoFar move, int maximumDepth) {
        
        if (maximumDepth <= 1)
            return;
        
        int depth = 1;
        double alpha = -1.0/0.0;
        double beta = 1.0/0.0;
        alphaBeta(true, depth, gs, alpha, beta, evaluator, move);
//        System.out.println("AB Depth " + depth);
        
        while (depth < maximumDepth) {
            alpha = -1.0/0.0;
            beta = 1.0/0.0;
            BestMoveSoFar move2 = new BestMoveSoFar();
            alphaBeta(true, ++depth, gs, alpha, beta, evaluator, move2);
            if (move2.getMove() == null)
                return;
            move.set(move2.getMove());
            
//            System.out.println("AB Depth " + depth);
        }
    }
    
    public void generateMove (GameState gs, BestMoveSoFar move) {        
        iterativeDeepening(gs, move, 999999);
    }
}
