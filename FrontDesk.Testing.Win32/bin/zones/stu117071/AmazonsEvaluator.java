
public abstract class AmazonsEvaluator implements Evaluator, Cloneable {

    public AmazonsEvaluator() {
        super();
    }

    public double evaluateGameState (GameState gs) {
        AmazonsGameState ags = (AmazonsGameState)gs;
        return evaluateGameState(ags);
    }

    public abstract double evaluateGameState (AmazonsGameState gs);
    
    public Object clone () {
        try {
            return super.clone();
        } catch (CloneNotSupportedException e) {
            e.printStackTrace();
            return null;
        }
    }
}
