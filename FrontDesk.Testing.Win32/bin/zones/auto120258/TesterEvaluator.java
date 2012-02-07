
public class TesterEvaluator implements Evaluator{
    
    public double evaluateGameState(GameState gs)
    {
        TesterGame game = (TesterGame)gs;
        return game.value();
    }
    
    public Object clone()
    {
        return null;
    }
}
