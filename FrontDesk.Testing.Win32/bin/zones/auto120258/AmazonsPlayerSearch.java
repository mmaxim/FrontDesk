
public abstract class AmazonsPlayerSearch implements AmazonsPlayer, Cloneable {
    protected Evaluator evaluator;
    
    public void setEvaluator (Evaluator eval) {
        this.evaluator = eval;
    }
    
    public Object clone () {
        try {
            AmazonsPlayerSearch toReturn = (AmazonsPlayerSearch)super.clone();
            toReturn.setEvaluator((Evaluator)evaluator.clone());
            return toReturn;
        } catch (CloneNotSupportedException e) {
            e.printStackTrace();
            return null;
        }
    }
}
