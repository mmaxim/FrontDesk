
public class BestMoveSoFar {
    private Move localMove = null;

    public  Move getMove () {
        return localMove;
    }
    
    public BestMoveSoFar () {
        ;
    }
    public synchronized void set (Move mArg) {
        Move temp = null;
        synchronized(this) {
            if (mArg != null)
                temp = (Move)(mArg.clone());
        }
        synchronized(this) {
            localMove = temp;
        }
    }
}